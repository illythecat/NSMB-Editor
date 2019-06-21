// Decompiled with JetBrains decompiler
// Type: NSMBe4.NSMBExtraData
// Assembly: NSMBe5, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 375C0264-8422-4B10-96C9-D574BA1AC306
// Assembly location: C:\Users\tiago\Desktop\nsmbeBent\NSMBe5.exe

using System;

namespace NSMBe5
{
  public class NSMBExtraData
  {
    public byte[] data;

    public NSMBExtraData()
    {
      this.data = new byte[16];
    }

    private void setBit(int byteIndex, byte bitIndex, bool val)
    {
      if (val)
        this.data[byteIndex] |= (byte) (1U << (int) bitIndex);
      else
        this.data[byteIndex] &= (byte) ((int) byte.MaxValue ^ 1 << (int) bitIndex);
    }

    private bool getBit(int byteIndex, byte bitIndex)
    {
      return Convert.ToBoolean((int) this.data[byteIndex] >> (int) bitIndex & 1);
    }

    public void setClassID(ushort classID)
    {
      byte[] bytes = BitConverter.GetBytes(classID);
      this.data[0] = bytes[0];
      this.data[1] = Convert.ToByte((int) this.data[1] & 254 | (int) bytes[1] & 1);
    }

    public void setDrawDistance(byte drawDistance)
    {
      this.data[1] = Convert.ToByte((int) this.data[1] & 15 | (int) drawDistance << 4);
    }

    public void setSpriteData(byte[] spriteData)
    {
      this.data[14] = spriteData[0];
      this.data[15] = spriteData[1];
      this.data[7] = spriteData[2];
      this.data[6] = spriteData[3];
      this.data[5] = spriteData[4];
      this.data[4] = spriteData[5];
    }

    public ushort getClassID()
    {
      return (ushort) (BitConverter.ToInt32(this.data, 0) & 511);
    }

    public byte getDrawDistance()
    {
      return (byte) ((uint) this.data[1] >> 4);
    }

    public byte[] getSpriteData()
    {
      return new byte[6]
      {
        this.data[14],
        this.data[15],
        this.data[7],
        this.data[6],
        this.data[5],
        this.data[4]
      };
    }

    public void setOffsetX(sbyte offsetX)
    {
      this.data[2] = (byte) offsetX;
    }

    public void setOffsetY(sbyte offsetY)
    {
      this.data[3] = (byte) offsetY;
    }

    public void setParticleID(ushort particleID)
    {
      byte[] bytes = BitConverter.GetBytes(particleID);
      this.data[8] = bytes[0];
      this.data[9] = bytes[1];
    }

    public void setParticleOffsetX(sbyte particleOffsetX)
    {
      this.data[10] = (byte) particleOffsetX;
    }

    public void setParticleOffsetY(sbyte particleOffsetY)
    {
      this.data[11] = (byte) particleOffsetY;
    }

    public void setSfxID(ushort sfxID)
    {
      byte[] bytes = BitConverter.GetBytes(sfxID);
      this.data[12] = bytes[0];
      this.data[13] = bytes[1];
    }

    public sbyte getOffsetX()
    {
      return (sbyte) this.data[2];
    }

    public sbyte getOffsetY()
    {
      return (sbyte) this.data[3];
    }

    public ushort getParticleID()
    {
      return BitConverter.ToUInt16(this.data, 8);
    }

    public sbyte getParticleOffsetX()
    {
      return (sbyte) this.data[10];
    }

    public sbyte getParticleOffsetY()
    {
      return (sbyte) this.data[11];
    }

    public ushort getSfxID()
    {
      return BitConverter.ToUInt16(this.data, 12);
    }

    public void setAmount(byte amount)
    {
      this.data[2] = amount;
    }

    public void setZPos(byte zPos)
    {
      this.data[3] = zPos;
    }

    public void setGapFlag(bool gapFlag)
    {
      this.setBit(10, (byte) 0, gapFlag);
    }

    public void setNoSwayAcceleration(bool noSwayAcceleration)
    {
      this.setBit(10, (byte) 1, noSwayAcceleration);
    }

    public void setModelRotation(int mode, int axisIndex)
    {
      byte bitIndex1 = (byte) (6 - axisIndex * 2);
      byte bitIndex2 = (byte) (7 - axisIndex * 2);
      this.setBit(10, bitIndex1, false);
      this.setBit(10, bitIndex2, false);
      if (mode == 1 || mode == 2)
        this.setBit(10, bitIndex2, true);
      if (mode != 2)
        return;
      this.setBit(10, bitIndex1, true);
    }

    public void setTileShiftX(byte tileShiftX)
    {
      this.data[12] = tileShiftX;
    }

    public void setTileShiftY(byte tileShiftY)
    {
      this.data[13] = tileShiftY;
    }

    public void setHalfTileShiftX(bool halfTileShiftX)
    {
      this.setBit(11, (byte) 0, halfTileShiftX);
    }

    public void setHalfTileShiftY(bool halfTileShiftY)
    {
      this.setBit(11, (byte) 1, halfTileShiftY);
    }

    public void setSpeedMultiplier(byte speedMultiplier)
    {
      this.data[8] = speedMultiplier;
    }

    public void setUnintIsHalfTiles(bool unitHalfTile)
    {
      this.setBit(11, (byte) 2, unitHalfTile);
    }

    public void setSpritesNotMoved(bool spritesNotMoved)
    {
      this.setBit(11, (byte) 3, spritesNotMoved);
    }

    public void setRotationStart(byte rotationStart)
    {
      this.data[11] = (byte) ((int) this.data[11] & 15 | (int) rotationStart << 4);
    }

    public byte getAmount()
    {
      return this.data[2];
    }

    public byte getZPos()
    {
      return this.data[3];
    }

    public bool getGapFlag()
    {
      return this.getBit(10, (byte) 0);
    }

    public bool getNoSwayAcceleration()
    {
      return this.getBit(10, (byte) 1);
    }

    public int getModelRotation(int axisIndex)
    {
      bool bit1 = this.getBit(10, (byte) (6 - axisIndex * 2));
      bool bit2 = this.getBit(10, (byte) (7 - axisIndex * 2));
      if (bit2 & bit1)
        return 2;
      return bit2 && !bit1 ? 1 : 0;
    }

    public byte getTileShiftX()
    {
      return this.data[12];
    }

    public byte getTileShiftY()
    {
      return this.data[13];
    }

    public bool getHalfTileShiftX()
    {
      return this.getBit(11, (byte) 0);
    }

    public bool getHalfTileShiftY()
    {
      return this.getBit(11, (byte) 1);
    }

    public byte getSpeedMultiplier()
    {
      return this.data[8];
    }

    public bool getUnintIsHalfTiles()
    {
      return this.getBit(11, (byte) 2);
    }

    public bool getSpritesNotMoved()
    {
      return this.getBit(11, (byte) 3);
    }

    public byte getRotationStart()
    {
      return (byte) ((uint) this.data[11] >> 4);
    }
  }
}
