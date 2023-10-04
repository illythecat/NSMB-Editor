/*
*   This file is part of NSMB Editor 5.
*
*   NSMB Editor 5 is free software: you can redistribute it and/or modify
*   it under the terms of the GNU General Public License as published by
*   the Free Software Foundation, either version 3 of the License, or
*   (at your option) any later version.
*
*   NSMB Editor 5 is distributed in the hope that it will be useful,
*   but WITHOUT ANY WARRANTY; without even the implied warranty of
*   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*   GNU General Public License for more details.
*
*   You should have received a copy of the GNU General Public License
*   along with NSMB Editor 5.  If not, see <http://www.gnu.org/licenses/>.
*/

﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;


namespace NSMBe5.Patcher
{
	public class CodePatcherNSMBe : CodePatcher
	{
		private struct Replace
		{
			public int ramAddr;
			public int ovId;
			public string funcName;
		}

		private int ArenaLoOffs;
		private Arm9BinaryHandler handler;


		public CodePatcherNSMBe(DirectoryInfo romdir) : base(romdir)
		{
			handler = new Arm9BinaryHandler();
		}


		public override void Execute()
		{
			string hookmap;
			string replaces;

			InsertCode(romdir, out hookmap, out replaces);

			handler.restoreFromBackup();

			uint codeAddr = GetCodeAddr();
			RunProcess("make CODEADDR=0x" + codeAddr.ToString("X8"), romdir.FullName);

			GeneratePatch(hookmap, replaces);
		}

		public override void CleanBuild()
		{
			RunProcessThrow("make clean", romdir.FullName);
		}

		private uint GetCodeAddr()
		{
			handler.load();
			LoadArenaLoOffsFile(romdir);
			uint codeAddr = handler.readFromRamAddr(ArenaLoOffs, -1);
			return codeAddr;
		}

		private void GeneratePatch(string hookmap = "", string replaces = "")
		{
			int codeAddr = (int)GetCodeAddr();
			Console.Out.WriteLine(string.Format("New code address: {0:X8}", codeAddr));

			FileInfo f = new FileInfo(romdir.FullName + "/newcode.bin");
			if (!f.Exists) return;
			FileStream fs = f.OpenRead();

			byte[] newdata = new byte[fs.Length];
			fs.Read(newdata, 0, (int)fs.Length);
			fs.Close();

			ByteArrayOutputStream extradata = new ByteArrayOutputStream();

			extradata.write(newdata);
			extradata.align(4);
			int hookAddr = codeAddr + extradata.getPos();

			#region Decompiled replaces.x code mess
			Console.WriteLine(replaces);
			string str1 = replaces;
			char[] chArray1 = new char[1] { '\n' };
			foreach (string str2 in str1.Split(chArray1))
			{
				char[] chArray2 = new char[1] { ' ' };
				string[] strArray = str2.Split(chArray2);
				if (strArray.Length == 2)
				{
					strArray[1] = strArray[1].Replace("0x", "");
					int ov = -1;
					if (strArray[0].Contains("_ov_"))
						ov = int.Parse(strArray[0].Substring(strArray[0].IndexOf("_ov_") + 4, 2), NumberStyles.HexNumber);
					uint uhex = CodePatcherNSMBe.ParseUHex(strArray[1]);
					this.handler.writeToRamAddr(CodePatcherNSMBe.ParseHex(strArray[0].Substring(0, 8)), uhex, ov);
				}
			}
			FileInfo fileInfo2 = new FileInfo(this.romdir.FullName + "/replaces.x");
			List<CodePatcherNSMBe.Replace> replaceList = new List<CodePatcherNSMBe.Replace>();
			if (fileInfo2.Exists)
			{
				StreamReader streamReader = new StreamReader(fileInfo2.FullName, Encoding.UTF8);
				while (!streamReader.EndOfStream)
				{
					string str2 = streamReader.ReadLine().Replace("\n", "").Replace("\t", "");
					if (str2.Contains("@"))
						str2 = str2.Substring(0, str2.IndexOf("@"));
					List<string> stringList = new List<string>((IEnumerable<string>)str2.Split(' '));
					for (int index = stringList.Count - 1; index >= 0; --index)
					{
						if (stringList[index] == "")
							stringList.RemoveAt(index);
					}
					if (stringList.Count != 0)
					{
						if (stringList.Count != 2)
						{
							new ErrorMSGBox("", "", "", "Failed parsing replace: Wrong format.\n\nInput line: \"" + str2 + "\"").ShowDialog();
						}
						else
						{
							int num2 = 9;
							if (stringList[0].Contains("_ov_"))
								num2 += 6;
							if (stringList[0].Length != num2 || !stringList[0].EndsWith(":"))
							{
								new ErrorMSGBox("", "", "", "Failed parsing replace: Wrong format.\n\nInput line: \"" + str2 + "\"").ShowDialog();
							}
							else
							{
								CodePatcherNSMBe.Replace replace;
								if (!int.TryParse(str2.Substring(0, 8), NumberStyles.HexNumber, (IFormatProvider)null, out replace.ramAddr))
								{
									new ErrorMSGBox("", "", "", "Failed parsing replace: Wrong address format.\n\nInput address: \"" + str2.Substring(0, 8) + "\"").ShowDialog();
								}
								else
								{
									replace.ovId = -1;
									if (stringList[0].Contains("_ov_") && !int.TryParse(str2.Substring(str2.IndexOf("_ov_") + 4, 2), NumberStyles.HexNumber, (IFormatProvider)null, out replace.ovId))
									{
										new ErrorMSGBox("", "", "", "Failed parsing replace: Wrong overlay format.\n\nInput overlay: \"" + str2.Substring(str2.IndexOf("_ov_") + 4, 2) + "\"").ShowDialog();
									}
									else
									{
										uint result;
										if (uint.TryParse(stringList[1].Replace("0x", ""), NumberStyles.HexNumber, (IFormatProvider)null, out result))
										{
											this.handler.writeToRamAddr(replace.ramAddr, result, replace.ovId);
										}
										else
										{
											replace.funcName = stringList[1];
											replaceList.Add(replace);
										}
									}
								}
							}
						}
					}
				}
				streamReader.Close();
			}
			#endregion

			f = new FileInfo(romdir.FullName + "/newcode.sym");
			StreamReader s = f.OpenText();

			while (!s.EndOfStream)
			{
				string l = s.ReadLine();

				#region Decompiled replaces.x code mess
				for (int index = replaceList.Count - 1; index >= 0; --index)
				{
					CodePatcherNSMBe.Replace replace = replaceList[index];
					if (l.Contains(replace.funcName) && (l.Contains(".text") || l.Contains(".data")))
					{
						uint result;
						if (!uint.TryParse(l.Substring(0, 8), NumberStyles.HexNumber, (IFormatProvider)null, out result))
						{
							new ErrorMSGBox("", "", "", "Failed parsing destination adress: Wrong adress format.\n\nInput adress: \"" + l.Substring(0, 8) + "\"").ShowDialog();
						}
						else
						{
							this.handler.writeToRamAddr(replace.ramAddr, result, replace.ovId);
							replaceList.Remove(replace);
						}
					}
				}
				#endregion

				int ind = -1;
				string[,] instructionNames = new string[2,5] {
					{ "nsub", "hook", "repl", "xrpl", "lrpl" }, // NSMB instructions
					{ "ansub", "ahook", "arepl", "trepl", "btrpl" } // MKDS instructions
				};


				foreach(string instructionName in instructionNames)
				{
					if (l.Contains(instructionName + "_"))
					{
						ind = l.IndexOf(instructionName + "_");
						break;
					}
				}

				if (ind != -1 && l.IndexOf(" _ZZ") != 31)
				{
					int destRamAddr= ParseHex(l.Substring(0, 8));    //Redirect dest addr

					//Determining if the instruction is 4 or 5 characters long (repl vs arepl, ...)
					bool isFiveLetterInstruction = l[ind+5]=='_';
					int startingIndex = isFiveLetterInstruction ? ind + 6 : ind + 5;
					//Determining if the address is 7 or 8 characters long (200... vs 0200...)
					bool isEightCharactersAddress = l[startingIndex] == '0';
					string ramAddress = l.Substring(startingIndex, isEightCharactersAddress ? 8 : 7);
					if (!isEightCharactersAddress) ramAddress = "0" + ramAddress;

					int ramAddr = ParseHex(ramAddress); //Patched addr
					uint val = 0;

					int ovId = -1;
					if (l.Contains("_ov_"))
						ovId = ParseHex(l.Substring(l.IndexOf("_ov_") + 4, 2));

					string cmd = l.Substring(ind, isFiveLetterInstruction ? 5 : 4 );
					int thisHookAddr = 0;

					switch(cmd)
					{
						case "ansub":
						case "nsub":
							val = MakeBranchOpcode(ramAddr, destRamAddr, 0);
							break;
						case "arepl":
						case "repl":
							val = MakeBranchOpcode(ramAddr, destRamAddr, 1);
							break;
						case "trepl":
						case "xrpl":
							val = MakeBranchOpcode(ramAddr, destRamAddr, 2);
							break;
						case "btrpl":
						case "lrpl":
							UInt16 lrvalue = 0xB500; //push {r14}
							handler.writeToRamAddr(ramAddr, lrvalue, ovId);
							ramAddr += 2;
							val = MakeBranchOpcode(ramAddr, destRamAddr, 2);
							break;
						case "ahook":
						case "hook":
							//Jump to the hook addr
							thisHookAddr = hookAddr;
							val = MakeBranchOpcode(ramAddr, hookAddr, 0);

							uint originalOpcode = handler.readFromRamAddr(ramAddr, ovId);
							
							//TODO: Parse and fix original opcode in case of BL instructions
							//so it's possible to hook over them too.
							extradata.writeUInt(originalOpcode);
							hookAddr += 4;
							extradata.writeUInt(0xE92D5FFF); //push {r0-r12, r14}
							hookAddr += 4;
							extradata.writeUInt(MakeBranchOpcode(hookAddr, destRamAddr, 1));
							hookAddr += 4;
							extradata.writeUInt(0xE8BD5FFF); //pop {r0-r12, r14}
							hookAddr += 4;
							extradata.writeUInt(MakeBranchOpcode(hookAddr, ramAddr+4, 0));
							hookAddr += 4;
							extradata.writeUInt(0x12345678);
							hookAddr += 4;
							break;
						default:
							continue;
					}

					Console.Out.WriteLine(string.Format("              {0:X8} {1:X8}", destRamAddr, thisHookAddr));

					handler.writeToRamAddr(ramAddr, val, ovId);
				}
			}

			#region Decompiled replaces.x code mess
			string str4 = hookmap;
			char[] chArray3 = new char[1] { '\n' };
			foreach (string str2 in str4.Split(chArray3))
			{
				if (!(str2 == ""))
				{
					int hex1 = CodePatcherNSMBe.ParseHex(str2.Substring(str2.Length - 8, 8));
					int hex2 = CodePatcherNSMBe.ParseHex(str2.Substring(5, 8));
					int ov = -1;
					if (str2.Contains("_ov_"))
						ov = CodePatcherNSMBe.ParseHex(str2.Substring(str2.IndexOf("_ov_") + 4, 2));
					string str3 = str2.Substring(0, 4);
					uint val;
					if (!(str3 == "nsub"))
					{
						if (!(str3 == "repl"))
						{
							if (str3 == "hook")
							{
								val = CodePatcherNSMBe.MakeBranchOpcode(hex2, hookAddr, 0);
								uint u = this.handler.readFromRamAddr(hex2, ov);
								extradata.writeUInt(u);
								int num1 = hookAddr + 4;
								extradata.writeUInt(3912065023U);
								int srcAddr1 = num1 + 4;
								extradata.writeUInt(CodePatcherNSMBe.MakeBranchOpcode(srcAddr1, hex1, 1));
								int num2 = srcAddr1 + 4;
								extradata.writeUInt(3904724991U);
								int srcAddr2 = num2 + 4;
								extradata.writeUInt(CodePatcherNSMBe.MakeBranchOpcode(srcAddr2, hex2 + 4, 0));
								int num3 = srcAddr2 + 4;
								extradata.writeUInt(305419896U);
								hookAddr = num3 + 4;
							}
							else
								continue;
						}
						else
							val = CodePatcherNSMBe.MakeBranchOpcode(hex2, hex1, 1);
					}
					else
						val = CodePatcherNSMBe.MakeBranchOpcode(hex2, hex1, 0);
					this.handler.writeToRamAddr(hex2, val, ov);
				}
			}
			string str5 = "";
			foreach (CodePatcherNSMBe.Replace replace in replaceList)
				str5 = str5 + "\n" + replace.funcName;
			if (str5 != "")
			{
				new ErrorMSGBox("", "", "", "Some replaces were not found:\n" + str5).ShowDialog();
			}
			#endregion

			s.Close();

			int newArenaOffs = codeAddr + extradata.getPos();
			handler.writeToRamAddr(ArenaLoOffs, (uint)newArenaOffs, -1);

			handler.sections.Add(new Arm9BinSection(extradata.getArray(), codeAddr, 0));
			handler.saveSections();
		}

		private void InsertCode(DirectoryInfo romdir, out string hookmap, out string replaces)
		{
			#region Decompiled replaces.x code mess
			hookmap = "";
			replaces = "";

			if (!File.Exists(romdir.FullName + @"\bak_safe\main.bin"))
			{
				if (File.Exists(romdir.FullName + @"\bak\main.bin"))
				{
					if (!Directory.Exists(romdir.FullName + @"\bak_safe"))
						Directory.CreateDirectory(romdir.FullName + @"\bak_safe");
					File.Copy(romdir.FullName + @"\bak\main.bin", romdir.FullName + @"\bak_safe\main.bin", true);
				}
				else
					this.handler.makeBinBackup(-1, true);
			}
			File.Copy(romdir.FullName + @"\bak_safe\main.bin", romdir.FullName + @"\bak\main.bin", true);

			FileInfo fileInfo1 = new FileInfo(romdir.FullName + "/codemap.x");
			if (!fileInfo1.Exists)
				return;
			StreamReader streamReader1 = new StreamReader(fileInfo1.FullName, Encoding.UTF8);
			while (!streamReader1.EndOfStream)
			{
				string str1 = streamReader1.ReadLine();
				string[] strArray = str1.Split(' ');
				if (strArray.Length != 2 && strArray.Length != 3)
				{
					throw new Exception("Wrong Codemap line format (" + str1 + ")");
				}
				else
				{
					string str2 = strArray[0];
					if (!Directory.Exists(romdir.FullName + "/" + str2))
					{
						throw new Exception("Directory \"" + str2 + "\" does not exist");
					}
					else
					{
						if (strArray[1].StartsWith("0x"))
							strArray[1] = strArray[1].Substring(2);
						if (strArray[2].StartsWith("0x"))
							strArray[2] = strArray[2].Substring(2);
						int hex = CodePatcherNSMBe.ParseHex(strArray[1]);
						if (hex < 33554432)
						{
							throw new Exception("Destination address of " + strArray[0] + " is too small (0x" + hex.ToString("X8") + ")");
						}
						else if (hex > 50331647)
						{
							throw new Exception("Destination address of " + strArray[0] + " is too big (0x" + hex.ToString("X8") + ")");
						}
						else
						{
							int num5 = -1;
							if (strArray.Length == 3)
								num5 = CodePatcherNSMBe.ParseHex(strArray[2]);
							RunProcess("make clean", romdir.FullName + "/" + strArray[0]);
							if (RunProcess("make CODEADDR=0x" + hex.ToString("X8"), romdir.FullName + "/" + strArray[0]) != 0)
							{
								throw new Exception("Compilation of " + strArray[0] + " failed.");
							}
							else
							{
								string str3 = romdir.FullName + "/" + str2 + "/newcode.bin";
								if (!File.Exists(str3))
								{
									throw new Exception("Insert Code Binary \"" + str3 + "\" does not exist!");
								}
								long length1 = new FileInfo(str3).Length;
								if (length1 > (long)num5)
								{
									throw new Exception("Insert Code Binary \"" + str3 + "\" size (" + (object)length1 + ") exeeds given maximum size (" + (object)num5 + ")");
								}
								FileInfo fileInfo2 = new FileInfo(romdir.FullName + "/" + strArray[0] + "/replaces.x");
								List<CodePatcherNSMBe.Replace> replaceList = new List<CodePatcherNSMBe.Replace>();
								if (fileInfo2.Exists)
								{
									StreamReader streamReader2 = new StreamReader(fileInfo2.FullName, Encoding.UTF8);
									while (!streamReader2.EndOfStream)
									{
										string str4 = streamReader2.ReadLine().Replace("\n", "").Replace("\t", "");
										if (str4.Contains("@"))
											str4 = str4.Substring(0, str4.IndexOf("@"));
										List<string> stringList = new List<string>((IEnumerable<string>)str4.Split(' '));
										for (int index = stringList.Count - 1; index >= 0; --index)
										{
											if (stringList[index] == "")
												stringList.RemoveAt(index);
										}
										if (stringList.Count != 0)
										{
											if (stringList.Count != 2)
											{
												throw new Exception("Failed parsing replace: Wrong format.\n\nInput line: \"" + str4 + "\"");
											}
											else
											{
												int num8 = 9;
												if (stringList[0].Contains("_ov_"))
													num8 += 6;
												if (stringList[0].Length != num8 || !stringList[0].EndsWith(":"))
												{
													throw new Exception("Failed parsing replace: Wrong format.\n\nInput line: \"" + str4 + "\"");
												}
												else
												{
													CodePatcherNSMBe.Replace replace;
													if (!int.TryParse(str4.Substring(0, 8), NumberStyles.HexNumber, (IFormatProvider)null, out replace.ramAddr))
													{
														throw new Exception("Failed parsing replace: Wrong address format.\n\nInput address: \"" + str4.Substring(0, 8) + "\"");
													}
													else
													{
														replace.ovId = -1;
														if (stringList[0].Contains("_ov_") && !int.TryParse(str4.Substring(str4.IndexOf("_ov_") + 4, 2), NumberStyles.HexNumber, (IFormatProvider)null, out replace.ovId))
														{
															throw new Exception("Failed parsing replace: Wrong overlay format.\n\nInput overlay: \"" + str4.Substring(str4.IndexOf("_ov_") + 4, 2) + "\"");
														}
														else
														{
															uint result;
															if (uint.TryParse(stringList[1].Replace("0x", ""), NumberStyles.HexNumber, (IFormatProvider)null, out result))
															{
																replaces = replaces + str4 + "\n";
															}
															else
															{
																replace.funcName = stringList[1];
																replaceList.Add(replace);
															}
														}
													}
												}
											}
										}
									}
									streamReader2.Close();
								}
								StreamReader streamReader3 = new StreamReader(romdir.FullName + "/" + strArray[0] + "/newcode.sym", Encoding.UTF8);
								while (!streamReader3.EndOfStream)
								{
									string str4 = streamReader3.ReadLine();
									for (int index = replaceList.Count - 1; index >= 0; --index)
									{
										CodePatcherNSMBe.Replace replace = replaceList[index];
										if (str4.Contains(replace.funcName) && (str4.Contains(".text") || str4.Contains(".data")))
										{
											uint result;
											if (!uint.TryParse(str4.Substring(0, 8), NumberStyles.HexNumber, (IFormatProvider)null, out result))
											{
												throw new Exception("Failed parsing destination adress: Wrong adress format.\n\nInput adress: \"" + str4.Substring(0, 8) + "\"");
											}
											else
											{
												if (replace.ovId == -1)
													replaces = replaces + replace.ramAddr.ToString("X8") + " 0x" + result.ToString("X8") + "\n";
												else
													replaces = replaces + replace.ramAddr.ToString("X8") + "_ov_" + replace.ovId.ToString("X2") + " 0x" + result.ToString("X8") + "\n";
												replaceList.Remove(replace);
											}
										}
									}
									int startIndex = -1;
									if (str4.Contains("nsub_"))
										startIndex = str4.IndexOf("nsub_");
									if (str4.Contains("hook_"))
										startIndex = str4.IndexOf("hook_");
									if (str4.Contains("repl_"))
										startIndex = str4.IndexOf("repl_");
									if (startIndex != -1)
									{
										string str5 = str4.Substring(startIndex);
										int length2 = 13;
										if (str5.Contains("_ov_"))
											length2 += 6;
										else if (str5.Contains("_main"))
											length2 += 5;
										string str6 = str5.Substring(0, length2);
										hookmap = hookmap + str6 + ": " + str4.Substring(0, 8) + "\n";
									}
								}
								streamReader3.Close();
								string str7 = "";
								foreach (CodePatcherNSMBe.Replace replace in replaceList)
									str7 = str7 + "\n" + replace.funcName;
								if (str7 != "")
								{
									throw new Exception("Some replaces were not found:\n" + str7);
								}
								string path = romdir.FullName + "/bak/main.bin";
								if (!File.Exists(path))
								{
									throw new Exception("ARM 9 Binary \"" + path + "\" does not exist!");
								}
								byte[] bytes = File.ReadAllBytes(romdir.ToString() + "/bak/main.bin");
								byte[] numArray = File.ReadAllBytes(str3);
								Array.Copy((Array)numArray, 0, (Array)bytes, hex - 33554432, numArray.Length);
								File.WriteAllBytes(path, bytes);
							}
						}
					}
				}
			}
			#endregion
		}

		private void LoadArenaLoOffsFile(DirectoryInfo romdir)
		{
			FileInfo f = new FileInfo(romdir.FullName + "/arenaoffs.txt");
			StreamReader s = f.OpenText();
			string l = s.ReadLine();
			ArenaLoOffs = int.Parse(l, System.Globalization.NumberStyles.HexNumber);
			s.Close();
		}

		private static uint MakeBranchOpcode(int srcAddr, int destAddr, int withLink)
		{
			unchecked
			{
				uint res = (uint)0xEA000000;

				if (withLink == 1)
					res |= 0x01000000;

				int offs = (destAddr / 4) - (srcAddr / 4) - 2;
				offs &= 0x00FFFFFF;

				res |= (uint)offs;

				if (withLink == 2)
				{
					UInt16 res1 = 0xF000;
					UInt16 res2 = 0xE800;
					
					offs = destAddr - srcAddr - 2;
					offs >>= 2;
					offs &= 0x003FFFFF;
					
					res1 |= (UInt16)((offs >> 10) & 0x7FF);
					res2 |= (UInt16)((offs << 1)  & 0x7FF);

					res = (uint)(((uint)res2 << 16) | res1);

				}

				return res;
			}
		}

		private static uint ParseUHex(string s)
		{
			return uint.Parse(s, System.Globalization.NumberStyles.HexNumber);
		}

		private static int ParseHex(string s)
		{
			return int.Parse(s, System.Globalization.NumberStyles.HexNumber);
		}
	}
}
