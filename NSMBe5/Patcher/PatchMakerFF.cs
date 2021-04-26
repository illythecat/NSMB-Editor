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

using System.IO;

namespace NSMBe5.Patcher
{
    public class PatchMakerFF
    {
        DirectoryInfo romdir;
        private string codedir;
        private static readonly byte[] dummyFnt = { 0x08, 0x00, 0x00, 0x00, 0x83, 0x0, 0x1, 0x00, 0x00 };

        public PatchMakerFF(DirectoryInfo romdir)
        {
            this.romdir = romdir;
            this.codedir = Path.Combine(romdir.FullName, "__tmp");
        }

        public void compilePatch()
        {
            byte[] contents1 = ROM.arm9binFile.getContents();
            byte[] contents2 = ROM.arm9ovFile.getContents();
            byte[] contents3 = ROM.arm7binFile.getContents();
            byte[] contents4 = ROM.arm7ovFile.getContents();
            byte[] contents5 = ROM.headerFile.getContents();

            Directory.CreateDirectory(codedir);
            Directory.CreateDirectory(codedir + "/root");
            Directory.CreateDirectory(codedir + "/overlay9");
            Directory.CreateDirectory(codedir + "/overlay7");

            File.WriteAllBytes(codedir + "/arm9.bin", contents1);
            File.WriteAllBytes(codedir + "/arm9ovt.bin", contents2);
            File.WriteAllBytes(codedir + "/arm7.bin", contents3);
            File.WriteAllBytes(codedir + "/arm7ovt.bin", contents4);
            File.WriteAllBytes(codedir + "/header.bin", contents5);
            File.WriteAllBytes(codedir + "/fnt.bin", dummyFnt);

            DSFileSystem.Directory dirByPath1 = ROM.FS.getDirByPath("overlay9");
            DSFileSystem.Directory dirByPath2 = ROM.FS.getDirByPath("overlay7");

            foreach (DSFileSystem.File childrenFile in dirByPath1.childrenFiles)
                File.WriteAllBytes(codedir + "/overlay9/" + childrenFile.name, childrenFile.getContents());

            foreach (DSFileSystem.File childrenFile in dirByPath2.childrenFiles)
                File.WriteAllBytes(codedir + "/overlay7/" + childrenFile.name, childrenFile.getContents());

            PatchCompiler.compilePatchFF(romdir);
        }

        public void importPatch()
        {
            byte[] newFile1 = File.ReadAllBytes(codedir + "/arm9.bin");
            byte[] newFile2 = File.ReadAllBytes(codedir + "/arm9ovt.bin");
            byte[] newFile3 = File.ReadAllBytes(codedir + "/arm7.bin");
            byte[] newFile4 = File.ReadAllBytes(codedir + "/arm7ovt.bin");

            ROM.arm9binFile.beginEdit(this);
            ROM.arm9ovFile.beginEdit(this);
            ROM.arm7binFile.beginEdit(this);
            ROM.arm7ovFile.beginEdit(this);

            ROM.arm9binFile.replace(newFile1, this);
            ROM.arm9ovFile.replace(newFile2, this);
            ROM.arm7binFile.replace(newFile3, this);
            ROM.arm7ovFile.replace(newFile4, this);

            ROM.arm9binFile.endEdit(this);
            ROM.arm9ovFile.endEdit(this);
            ROM.arm7binFile.endEdit(this);
            ROM.arm7ovFile.endEdit(this);

            DirectoryInfo directoryInfo1 = new DirectoryInfo(codedir + "/overlay9");
            DirectoryInfo directoryInfo2 = new DirectoryInfo(codedir + "/overlay7");

            foreach (FileInfo file in directoryInfo1.GetFiles())
            {
                byte[] newFile5 = File.ReadAllBytes(file.FullName);
                DSFileSystem.File fileByName = ROM.FS.getFileByName(file.Name);
                fileByName.beginEdit(this);
                fileByName.replace(newFile5, this);
                fileByName.endEdit(this);
            }
            foreach (FileInfo file in directoryInfo2.GetFiles())
            {
                byte[] newFile5 = File.ReadAllBytes(file.FullName);
                DSFileSystem.File fileByName = ROM.FS.getFileByName(file.Name);
                fileByName.beginEdit(this);
                fileByName.replace(newFile5, this);
                fileByName.endEdit(this);
            }
            
            Directory.Delete(codedir, true);
        }
    }
}
