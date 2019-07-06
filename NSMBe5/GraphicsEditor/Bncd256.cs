using NSMBe5.DSFileSystem;
using System.Collections.Generic;

namespace NSMBe5
{
  public class Bncd256
  {
    public static int[,] widths = new int[4, 3]
    {
      {
        1,
        2,
        1
      },
      {
        2,
        4,
        1
      },
      {
        4,
        4,
        2
      },
      {
        8,
        8,
        4
      }
    };
    public static int[,] heights = new int[4, 3]
    {
      {
        1,
        1,
        2
      },
      {
        2,
        1,
        4
      },
      {
        4,
        2,
        4
      },
      {
        8,
        4,
        8
      }
    };
    public List<Bncd256.Bncd256Entry> entries = new List<Bncd256.Bncd256Entry>();
    public List<Bncd256.Bncd256Image> images = new List<Bncd256.Bncd256Image>();
    private ushort unk;

    public Bncd256(File f)
    {
      ByteArrayInputStream arrayInputStream = new ByteArrayInputStream(f.getContents());
      arrayInputStream.readInt();
      this.unk = arrayInputStream.readUShort();
      ushort num1 = arrayInputStream.readUShort();
      uint pos = arrayInputStream.readUInt();
      uint num2 = arrayInputStream.readUInt();
      uint num3 = arrayInputStream.readUInt();
      arrayInputStream.readUInt();
      arrayInputStream.seek(pos);
      Dictionary<uint, int> dictionary = new Dictionary<uint, int>();
      for (uint index1 = 0; index1 < (uint) num1; ++index1)
      {
        Bncd256.Bncd256Entry bncdEntry = new Bncd256.Bncd256Entry();
        bncdEntry.width = (int) arrayInputStream.readByte();
        bncdEntry.height = (int) arrayInputStream.readByte();
        uint num4 = (uint) arrayInputStream.readUShort();
        uint num5 = (uint) arrayInputStream.readUShort();
        arrayInputStream.savePos();
        arrayInputStream.seek(num2 + num4 * 12U);
        for (int index2 = 0; (long) index2 < (long) num5; ++index2)
        {
          Bncd256.Bncd256SubEntry bncdSubEntry = new Bncd256.Bncd256SubEntry();
          bncdEntry.subEntries.Add(bncdSubEntry);
          bncdSubEntry.oamAttr0 = arrayInputStream.readUShort();
          bncdSubEntry.oamAttr1 = arrayInputStream.readUShort();
          bncdSubEntry.unk = arrayInputStream.readUInt();
          bncdSubEntry.tileNumber = arrayInputStream.readUShort();
          bncdSubEntry.tileCount = arrayInputStream.readUShort();
          bncdSubEntry.tileCount *= (ushort) 2;
          uint key = (uint) bncdSubEntry.tileNumber << 16 | (uint) bncdSubEntry.tileCount;
          int count = dictionary.Count;
          if (dictionary.ContainsKey(key))
          {
            count = dictionary[key];
          }
          else
          {
            dictionary[key] = count;
            Bncd256.Bncd256Image bncdImage = new Bncd256.Bncd256Image();
            this.images.Add(bncdImage);
            bncdImage.tileNumber = (int) bncdSubEntry.tileNumber;
            bncdImage.tileCount = (int) bncdSubEntry.tileCount;
            int index3 = (int) bncdSubEntry.oamAttr0 >> 14;
            int index4 = (int) bncdSubEntry.oamAttr1 >> 14;
            bncdImage.tileWidth = Bncd256.widths[index4, index3];
          }
          bncdSubEntry.imageId = count;
        }
        arrayInputStream.loadPos();
      }
      LevelChooser.showImgMgr();
      int num6 = 32;
      foreach (Bncd256.Bncd256Image image in this.images)
      {
        File f1 = (File) new InlineFile(f, (int) num3 + image.tileNumber * num6, image.tileCount * num6, f.name);
        LevelChooser.imgMgr.m.addImage((PalettedImage) new Image2D(f1, 8 * image.tileWidth, false));
      }
    }

    public class Bncd256Entry
    {
      public List<Bncd256.Bncd256SubEntry> subEntries = new List<Bncd256.Bncd256SubEntry>();
      public int width;
      public int height;
    }

    public class Bncd256SubEntry
    {
      public ushort oamAttr0;
      public ushort oamAttr1;
      public uint unk;
      public ushort tileNumber;
      public ushort tileCount;
      public int imageId;
    }

    public class Bncd256Image
    {
      public int tileNumber;
      public int tileCount;
      public int tileWidth;
    }
  }
}
