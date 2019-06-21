// Decompiled with JetBrains decompiler
// Type: NSMBe4.DSFileSystem.PhysicalFilesystem
// Assembly: NSMBe5, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 375C0264-8422-4B10-96C9-D574BA1AC306
// Assembly location: C:\Users\tiago\Desktop\nsmbeBent\NSMBe5.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace NSMBe5.DSFileSystem
{
    public abstract class PhysicalFilesystem : Filesystem
    {
        private HashSet<int> badFiles = new HashSet<int>();
        protected FilesystemSource source;
        public Stream s;
        protected File freeSpaceDelimiter;
        protected int fileDataOffsetP;

        public int fileDataOffset
        {
            get
            {
                return this.fileDataOffsetP;
            }
        }

        protected PhysicalFilesystem(FilesystemSource fs)
        {
            this.source = fs;
            this.s = this.source.load();
        }

        public bool checkForOverlaps(bool inform)
        {
            bool flag = false;
            foreach (PhysicalFile allFile1 in this.allFiles)
            {
                int fileBegin1 = allFile1.fileBegin;
                int val1 = fileBegin1 + allFile1.fileSize;
                foreach (PhysicalFile allFile2 in this.allFiles)
                {
                    if (!(allFile1.name == allFile2.name))
                    {
                        int fileBegin2 = allFile2.fileBegin;
                        int val2 = fileBegin2 + allFile2.fileSize;
                        if (val1 != fileBegin2 && val2 != fileBegin1 && Math.Max(fileBegin1, fileBegin2) <= Math.Min(val1, val2))
                        {
                            flag = true;
                            this.badFiles.Add((int)allFile1.id);
                            this.badFiles.Add((int)allFile2.id);
                            if (inform)
                            {
                                int num = (int)MessageBox.Show("Found file overlap: \n- " + allFile1.name + " (0x" + fileBegin1.ToString("X") + " to 0x" + val1.ToString("X") + ")\n- " + allFile2.name + " (0x" + fileBegin2.ToString("X") + " to 0x" + val2.ToString("X") + ")", "NSMBe - File overlap check", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                            else
                                Console.WriteLine("Found file overlap: " + allFile1.name + " (0x" + fileBegin1.ToString("X") + " to 0x" + val1.ToString("X") + ") and " + allFile2.name + " (0x" + fileBegin2.ToString("X") + " to 0x" + val2.ToString("X") + ")");
                        }
                    }
                }
            }
            if (flag & inform)
            {
                int num1 = (int)MessageBox.Show("Do not import/save any files before these issues are not resolved or otherwise the file system might get corrupted further!", "NSMBe - File overlap check", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return flag;
        }

        public int findFreeSpace(int len, int align)
        {
            this.allFiles.Sort();
            PhysicalFile physicalFile = (PhysicalFile)null;
            int num1 = int.MaxValue;
            int num2 = -1;
            string str1 = (string)null;
            string str2 = (string)null;
            for (int index = this.allFiles.IndexOf(this.freeSpaceDelimiter); index < this.allFiles.Count - 1; ++index)
            {
                PhysicalFile allFile1 = (PhysicalFile)this.allFiles[index];
                PhysicalFile allFile2 = (PhysicalFile)this.allFiles[index + 1];
                if (this.badFiles.Contains((int)allFile1.id) || this.badFiles.Contains((int)allFile2.id))
                {
                    Console.WriteLine("Skipping this one...");
                }
                else
                {
                    int num3 = this.alignUp(allFile1.fileBegin + allFile1.fileSize, align);
                    int num4 = this.alignDown(allFile2.fileBegin, align) - num3;
                    if (num4 >= len)
                    {
                        int num5 = num4 - len;
                        if (num5 < num1)
                        {
                            Console.WriteLine("New best Space between " + allFile1.name + " and " + allFile2.name);
                            str1 = allFile1.name;
                            str2 = allFile2.name;
                            num1 = num5;
                            physicalFile = allFile1;
                            num2 = num3;
                        }
                    }
                }
            }
            if (physicalFile != null)
            {
                Console.WriteLine("Found space between files " + str1 + " and " + str2 + " at " + (object)physicalFile.fileBegin);
                Console.WriteLine("\nTesting found free space for conflicts...");
                int fileBegin1 = physicalFile.fileBegin;
                int val1 = fileBegin1 + physicalFile.fileSize;
                foreach (PhysicalFile allFile in this.allFiles)
                {
                    int fileBegin2 = allFile.fileBegin;
                    int val2 = fileBegin2 + allFile.fileSize;
                    if (val1 != fileBegin2 && val2 != fileBegin1 && Math.Max(fileBegin1, fileBegin2) <= Math.Min(val1, val2) && (fileBegin2 != fileBegin1 || val2 != val1))
                    {
                        Console.WriteLine("WARNING! Found overlap: " + allFile.name);
                        int num3 = (int)MessageBox.Show("Found file overlap: \n- This (0x" + fileBegin1.ToString("X") + " to 0x" + val1.ToString("X") + ")\n- " + allFile.name + " (0x" + fileBegin2.ToString("X") + " to 0x" + val2.ToString("X") + ")", "NSMBe - File overlap check", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                return num2;
            }
            Console.WriteLine("No suitable free space between files found. Appending the file.");
            PhysicalFile allFile3 = (PhysicalFile)this.allFiles[this.allFiles.Count - 1];
            return this.alignUp(allFile3.fileBegin + allFile3.fileSize, align);
        }

        public void moveAllFiles(PhysicalFile first, int firstOffs)
        {
            this.allFiles.Sort();
            Console.Out.WriteLine("Moving file " + first.name);
            Console.Out.WriteLine("Into " + firstOffs.ToString("X"));
            int fileBegin = first.fileBegin;
            int num1 = firstOffs - fileBegin;
            Console.Out.WriteLine("DIFF " + num1.ToString("X"));
            int num2 = 4;
            for (int index = this.allFiles.IndexOf((File)first); index < this.allFiles.Count; ++index)
            {
                int alignment = ((PhysicalFile)this.allFiles[index]).alignment;
                if (alignment > num2)
                    num2 = alignment;
            }
            if (num1 % num2 != 0)
                num1 += num2 - num1 % num2;
            int count = this.getFilesystemEnd() - fileBegin;
            byte[] buffer = new byte[count];
            this.s.Seek((long)fileBegin, SeekOrigin.Begin);
            this.s.Read(buffer, 0, count);
            this.s.Seek((long)(fileBegin + num1), SeekOrigin.Begin);
            this.s.Write(buffer, 0, count);
            for (int index = this.allFiles.IndexOf((File)first); index < this.allFiles.Count; ++index)
                ((PhysicalFile)this.allFiles[index]).fileBeginP += num1;
            for (int index = this.allFiles.IndexOf((File)first); index < this.allFiles.Count; ++index)
                ((PhysicalFile)this.allFiles[index]).saveOffsets();
        }

        public override void close()
        {
            this.source.close();
        }

        public override void save()
        {
            this.source.save();
        }

        public void dumpFilesOrdered(TextWriter outs)
        {
            this.allFiles.Sort();
            foreach (PhysicalFile allFile in this.allFiles)
            {
                TextWriter textWriter = outs;
                string[] strArray = new string[5];
                int num = allFile.fileBegin;
                strArray[0] = num.ToString("X8");
                strArray[1] = " .. ";
                num = allFile.fileBegin + allFile.fileSize - 1;
                strArray[2] = num.ToString("X8");
                strArray[3] = ":  ";
                strArray[4] = allFile.getPath();
                string str = string.Concat(strArray);
                textWriter.WriteLine(str);
            }
        }

        public int getFilesystemEnd()
        {
            this.allFiles.Sort();
            PhysicalFile allFile = (PhysicalFile)this.allFiles[this.allFiles.Count - 1];
            return allFile.fileBegin + allFile.fileSize;
        }
    }
}
