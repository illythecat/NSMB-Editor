using System;
using System.Collections.Generic;
using System.Text;
using NSMBe5.DSFileSystem;
using System.IO;

namespace NSMBe5
{
    public class InternalLevelSource : LevelSource
    {
        DSFileSystem.File levelFile;
        DSFileSystem.File BGDatFile;
        NarcFilesystem narcFs;
        string filename;
        string levelname;
        byte[] levelData;
        byte[] BGDatData;
        bool editing = false;

        public InternalLevelSource(string filename, string levelname)
            : this(filename, levelname, "") { }

        public InternalLevelSource(string filename, string levelname, string loadFileName)
        {
            //If load from NARC in FS
            if(filename.Contains("@"))
            {
                string fileName = filename.Split('@')[0];
                string narcName = filename.Split('@')[1];
                narcFs = new NarcFilesystem(ROM.FS.getFileByName(narcName + ".narc"));
                levelFile = narcFs.getFileByName(fileName + ".bin");
                BGDatFile = narcFs.getFileByName(fileName + "_bgdat.bin");
            }
            //If load from FS
            else
            {
                levelFile = ROM.getLevelFile(filename);
                BGDatFile = ROM.getBGDatFile(filename);
            }
            
            this.filename = filename;
            this.levelname = levelname;
            if (loadFileName == "")
            {
                levelData = levelFile.getContents();
                BGDatData = BGDatFile.getContents();
            }
            else
            {
                FileStream fs = new FileStream(loadFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                BinaryReader br = new BinaryReader(fs);
                ExportedLevel level = new ExportedLevel(br);
                br.Close();
                levelData = level.LevelFile;
                BGDatData = level.BGDatFile;
            }
        }

        public override byte[] getData()
        {
            return levelData;
        }

        public override byte[] getBGDatData()
        {
            return BGDatData;
        }

        public override int getLevelFileID()
        {
            return (int)levelFile.id;
        }

        public override int getBGDatFileID()
        {
            return (int)BGDatFile.id;
        }

        public override void save(ExportedLevel level)
        {
            levelFile.replace(level.LevelFile, this);
            BGDatFile.replace(level.BGDatFile, this);
        }

        public override string getLevelName()
        {
            return levelname;
        }

        public override string getBackupText()
        {
            return filename;
        }

        public override string getBackupFileName()
        {
            return filename + ".nml";
        }

        public override void enableWrite()
        {
            if (editing) return;
            levelFile.beginEdit(this);
            BGDatFile.beginEdit(this);
            editing = true;
        }

        public override void close()
        {
            if (editing)
            {
                levelFile.endEdit(this);
                BGDatFile.endEdit(this);

                if (filename.Contains("@"))
                {
                    narcFs.save();
                    narcFs.close();
                }

                editing = false;
            }
        }
    }
}
