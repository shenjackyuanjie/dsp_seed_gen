using System;
using UnityEngine;

// Token: 0x020001D4 RID: 468
public class PlanetRawData
{
	// Token: 0x06001591 RID: 5521 RVA: 0x00176F48 File Offset: 0x00175148
	public PlanetRawData(int _precision)
	{
		this.precision = _precision;
		int dataLength = this.dataLength;
		this.heightData = new ushort[dataLength];
		this.vegeIds = new ushort[dataLength];
		this.biomoData = new byte[dataLength];
		this.temprData = new short[dataLength];
		this.vertices = new Vector3[dataLength];
		this.normals = new Vector3[dataLength];
		this.indexMapPrecision = this.precision >> 2;
		this.indexMapFaceStride = this.indexMapPrecision * this.indexMapPrecision;
		this.indexMapCornerStride = this.indexMapFaceStride * 3;
		this.indexMapDataLength = this.indexMapCornerStride * 8;
		this.indexMap = new int[this.indexMapDataLength];
		this.SetVegeCapacity(32);
		this.SetVeinCapacity(32);
	}

	// Token: 0x06001592 RID: 5522 RVA: 0x0017701D File Offset: 0x0017521D
	public byte[] InitModData(byte[] refModData)
	{
		if (refModData != null)
		{
			this.modData = refModData;
		}
		else
		{
			this.modData = new byte[this.dataLength / 2];
		}
		return this.modData;
	}

	// Token: 0x06001593 RID: 5523 RVA: 0x00177044 File Offset: 0x00175244
	public void Free()
	{
		this.precision = 0;
		this.heightData = null;
		this.biomoData = null;
		this.temprData = null;
		this.vertices = null;
		this.normals = null;
		this.veinPool = null;
		this.vegePool = null;
	}

	// Token: 0x170002FE RID: 766
	// (get) Token: 0x06001594 RID: 5524 RVA: 0x0017707E File Offset: 0x0017527E
	public int dataLength
	{
		get
		{
			return (this.precision + 1) * (this.precision + 1) * 4;
		}
	}

	// Token: 0x170002FF RID: 767
	// (get) Token: 0x06001595 RID: 5525 RVA: 0x00177093 File Offset: 0x00175293
	public int stride
	{
		get
		{
			return (this.precision + 1) * 2;
		}
	}

	// Token: 0x17000300 RID: 768
	// (get) Token: 0x06001596 RID: 5526 RVA: 0x0017709F File Offset: 0x0017529F
	public int substride
	{
		get
		{
			return this.precision + 1;
		}
	}

	// Token: 0x06001597 RID: 5527 RVA: 0x001770AC File Offset: 0x001752AC
	public void CalcVerts()
	{
		if (this.precision == 200 && PlanetRawData.verts200 != null)
		{
			Array.Copy(PlanetRawData.verts200, this.vertices, PlanetRawData.verts200.Length);
			Array.Copy(PlanetRawData.indexMap200, this.indexMap, PlanetRawData.indexMap200.Length);
			return;
		}
		if (this.precision == 80 && PlanetRawData.verts80 != null)
		{
			Array.Copy(PlanetRawData.verts80, this.vertices, PlanetRawData.verts80.Length);
			Array.Copy(PlanetRawData.indexMap80, this.indexMap, PlanetRawData.indexMap80.Length);
			return;
		}
		for (int i = 0; i < this.indexMapDataLength; i++)
		{
			this.indexMap[i] = -1;
		}
		int num = (this.precision + 1) * 2;
		int num2 = this.precision + 1;
		for (int j = 0; j < this.dataLength; j++)
		{
			int num3 = j % num;
			int num4 = j / num;
			int num5 = num3 % num2;
			int num6 = num4 % num2;
			int num7 = (((num3 >= num2) ? 1 : 0) + ((num4 >= num2) ? 1 : 0) * 2) * 2 + ((num5 >= num6) ? 0 : 1);
			float num8 = (float)((num5 >= num6) ? (this.precision - num5) : num5);
			float num9 = (float)((num5 >= num6) ? num6 : (this.precision - num6));
			float num10 = (float)this.precision - num9;
			num9 /= (float)this.precision;
			num8 = ((num10 > 0f) ? (num8 / num10) : 0f);
			Vector3 a;
			Vector3 a2;
			Vector3 b;
			int corner;
			switch (num7)
			{
			case 0:
				a = PlanetRawData.poles[2];
				a2 = PlanetRawData.poles[0];
				b = PlanetRawData.poles[4];
				corner = 7;
				break;
			case 1:
				a = PlanetRawData.poles[3];
				a2 = PlanetRawData.poles[4];
				b = PlanetRawData.poles[0];
				corner = 5;
				break;
			case 2:
				a = PlanetRawData.poles[2];
				a2 = PlanetRawData.poles[4];
				b = PlanetRawData.poles[1];
				corner = 6;
				break;
			case 3:
				a = PlanetRawData.poles[3];
				a2 = PlanetRawData.poles[1];
				b = PlanetRawData.poles[4];
				corner = 4;
				break;
			case 4:
				a = PlanetRawData.poles[2];
				a2 = PlanetRawData.poles[1];
				b = PlanetRawData.poles[5];
				corner = 2;
				break;
			case 5:
				a = PlanetRawData.poles[3];
				a2 = PlanetRawData.poles[5];
				b = PlanetRawData.poles[1];
				corner = 0;
				break;
			case 6:
				a = PlanetRawData.poles[2];
				a2 = PlanetRawData.poles[5];
				b = PlanetRawData.poles[0];
				corner = 3;
				break;
			case 7:
				a = PlanetRawData.poles[3];
				a2 = PlanetRawData.poles[0];
				b = PlanetRawData.poles[5];
				corner = 1;
				break;
			default:
				a = PlanetRawData.poles[2];
				a2 = PlanetRawData.poles[0];
				b = PlanetRawData.poles[4];
				corner = 7;
				break;
			}
			this.vertices[j] = Vector3.Slerp(Vector3.Slerp(a, b, num9), Vector3.Slerp(a2, b, num9), num8);
			int num11 = this.PositionHash(this.vertices[j], corner);
			if (this.indexMap[num11] == -1)
			{
				this.indexMap[num11] = j;
			}
		}
		int num12 = 0;
		for (int k = 1; k < this.indexMapDataLength; k++)
		{
			if (this.indexMap[k] == -1)
			{
				this.indexMap[k] = this.indexMap[k - 1];
				num12++;
			}
		}
		if (this.precision == 200)
		{
			if (PlanetRawData.verts200 == null)
			{
				PlanetRawData.verts200 = new Vector3[this.vertices.Length];
				PlanetRawData.indexMap200 = new int[this.indexMap.Length];
				Array.Copy(this.vertices, PlanetRawData.verts200, this.vertices.Length);
				Array.Copy(this.indexMap, PlanetRawData.indexMap200, this.indexMap.Length);
				return;
			}
		}
		else if (this.precision == 80 && PlanetRawData.verts80 == null)
		{
			PlanetRawData.verts80 = new Vector3[this.vertices.Length];
			PlanetRawData.indexMap80 = new int[this.indexMap.Length];
			Array.Copy(this.vertices, PlanetRawData.verts80, this.vertices.Length);
			Array.Copy(this.indexMap, PlanetRawData.indexMap80, this.indexMap.Length);
		}
	}

	// Token: 0x06001598 RID: 5528 RVA: 0x00177548 File Offset: 0x00175748
	public int QueryIndex(Vector3 vpos)
	{
		vpos.Normalize();
		int num = this.PositionHash(vpos, 0);
		int num2 = this.indexMap[num];
		float num3 = 3.1415927f / (float)(this.precision * 2) * 0.25f;
		num3 *= num3;
		int stride = this.stride;
		float num4 = 10f;
		int result = num2;
		for (int i = -1; i <= 3; i++)
		{
			for (int j = -1; j <= 3; j++)
			{
				int num5 = num2 + i + j * stride;
				if ((ulong)num5 < (ulong)((long)this.dataLength))
				{
					float sqrMagnitude = (this.vertices[num5] - vpos).sqrMagnitude;
					if (sqrMagnitude < num3)
					{
						return num5;
					}
					if (sqrMagnitude < num4)
					{
						num4 = sqrMagnitude;
						result = num5;
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06001599 RID: 5529 RVA: 0x00177606 File Offset: 0x00175806
	public int GetModLevel(int index)
	{
		return this.modData[index >> 1] >> ((index & 1) << 2) & 3;
	}

	// Token: 0x0600159A RID: 5530 RVA: 0x0017761D File Offset: 0x0017581D
	public short GetModPlane(int index)
	{
		return (short)((this.modData[index >> 1] >> ((index & 1) << 2) + 2 & 3) * 133 + 20020);
	}

	// Token: 0x0600159B RID: 5531 RVA: 0x00177644 File Offset: 0x00175844
	public void SetModLevel(int index, int level)
	{
		int num = (index & 1) << 2;
		int num2 = ~(3 << num);
		int num3 = (level & 3) << num;
		byte[] array = this.modData;
		int num4 = index >> 1;
		array[num4] &= (byte)num2;
		byte[] array2 = this.modData;
		int num5 = index >> 1;
		array2[num5] |= (byte)num3;
	}

	// Token: 0x0600159C RID: 5532 RVA: 0x00177694 File Offset: 0x00175894
	public void SetModPlane(int index, int plane)
	{
		if (plane > 3)
		{
			plane = 3;
		}
		else if (plane < 0)
		{
			plane = 0;
		}
		int num = ((index & 1) << 2) + 2;
		int num2 = ~(3 << num);
		int num3 = (plane & 3) << num;
		byte[] array = this.modData;
		int num4 = index >> 1;
		array[num4] &= (byte)num2;
		byte[] array2 = this.modData;
		int num5 = index >> 1;
		array2[num5] |= (byte)num3;
	}

	// Token: 0x0600159D RID: 5533 RVA: 0x001776F4 File Offset: 0x001758F4
	public bool AddModLevel(int index, int level)
	{
		int num = this.modData[index >> 1] >> ((index & 1) << 2) & 3;
		level += num;
		if (level > 3)
		{
			level = 3;
		}
		if (level == num)
		{
			return false;
		}
		int num2 = (index & 1) << 2;
		int num3 = ~(3 << num2);
		int num4 = (level & 3) << num2;
		byte[] array = this.modData;
		int num5 = index >> 1;
		array[num5] &= (byte)num3;
		byte[] array2 = this.modData;
		int num6 = index >> 1;
		array2[num6] |= (byte)num4;
		return true;
	}

	// Token: 0x0600159E RID: 5534 RVA: 0x0017776C File Offset: 0x0017596C
	public void EraseVegetableAtPoint(Vector3 vpos)
	{
		vpos.Normalize();
		int num = this.PositionHash(vpos, 0);
		int num2 = this.indexMap[num];
		int stride = this.stride;
		for (int i = -3; i <= 3; i++)
		{
			for (int j = -3; j <= 3; j++)
			{
				int num3 = num2 + i + j * stride;
				if ((ulong)num3 < (ulong)((long)this.dataLength))
				{
					int num4 = (int)this.vegeIds[num3];
					this.vegePool[num4].SetNull();
				}
			}
		}
	}

	// Token: 0x0600159F RID: 5535 RVA: 0x001777EC File Offset: 0x001759EC
	public float QueryHeight(Vector3 vpos)
	{
		vpos.Normalize();
		int num = this.PositionHash(vpos, 0);
		int num2 = this.indexMap[num];
		float num3 = 3.1415927f / (float)(this.precision * 2) * 1.2f;
		float num4 = num3 * num3;
		float num5 = 0f;
		float num6 = 0f;
		int stride = this.stride;
		for (int i = -1; i <= 3; i++)
		{
			for (int j = -1; j <= 3; j++)
			{
				int num7 = num2 + i + j * stride;
				if ((ulong)num7 < (ulong)((long)this.dataLength))
				{
					float sqrMagnitude = (this.vertices[num7] - vpos).sqrMagnitude;
					if (sqrMagnitude <= num4)
					{
						float num8 = 1f - Mathf.Sqrt(sqrMagnitude) / num3;
						float num9 = (float)this.heightData[num7];
						num5 += num8;
						num6 += num9 * num8;
					}
				}
			}
		}
		if (num5 == 0f)
		{
			Debug.LogWarning("bad query");
			return (float)this.heightData[0] * 0.01f;
		}
		return num6 / num5 * 0.01f;
	}

	// Token: 0x060015A0 RID: 5536 RVA: 0x001778FC File Offset: 0x00175AFC
	public float QueryModifiedHeight(Vector3 vpos)
	{
		vpos.Normalize();
		int num = this.PositionHash(vpos, 0);
		int num2 = this.indexMap[num];
		float num3 = 3.1415927f / (float)(this.precision * 2) * 1.2f;
		float num4 = num3 * num3;
		float num5 = 0f;
		float num6 = 0f;
		int stride = this.stride;
		for (int i = -1; i <= 3; i++)
		{
			for (int j = -1; j <= 3; j++)
			{
				int num7 = num2 + i + j * stride;
				if ((ulong)num7 < (ulong)((long)this.dataLength))
				{
					float sqrMagnitude = (this.vertices[num7] - vpos).sqrMagnitude;
					if (sqrMagnitude <= num4)
					{
						float num8 = 1f - Mathf.Sqrt(sqrMagnitude) / num3;
						int modLevel = this.GetModLevel(num7);
						float num9 = (float)this.heightData[num7];
						if (modLevel > 0)
						{
							float num10 = (float)this.GetModPlane(num7);
							if (modLevel == 3)
							{
								num9 = num10;
							}
							else
							{
								float num11 = (float)modLevel * 0.3333333f;
								num9 = (float)this.heightData[num7] * (1f - num11) + num10 * num11;
							}
						}
						num5 += num8;
						num6 += num9 * num8;
					}
				}
			}
		}
		if (num5 == 0f)
		{
			Debug.LogWarning("bad query");
			return (float)this.heightData[0] * 0.01f;
		}
		return num6 / num5 * 0.01f;
	}

	// Token: 0x060015A1 RID: 5537 RVA: 0x00177A64 File Offset: 0x00175C64
	private void SetVeinCapacity(int newCapacity)
	{
		VeinData[] array = this.veinPool;
		this.veinPool = new VeinData[newCapacity];
		if (array != null)
		{
			Array.Copy(array, this.veinPool, (newCapacity > this.veinCapacity) ? this.veinCapacity : newCapacity);
		}
		this.veinCapacity = newCapacity;
	}

	// Token: 0x060015A2 RID: 5538 RVA: 0x00177AAC File Offset: 0x00175CAC
	public int AddVeinData(VeinData vein)
	{
		int num = this.veinCursor;
		this.veinCursor = num + 1;
		vein.id = num;
		if (vein.id == this.veinCapacity)
		{
			this.SetVeinCapacity(this.veinCapacity * 2);
		}
		this.veinPool[vein.id] = vein;
		return vein.id;
	}

	// Token: 0x060015A3 RID: 5539 RVA: 0x00177B08 File Offset: 0x00175D08
	private void SetVegeCapacity(int newCapacity)
	{
		VegeData[] array = this.vegePool;
		this.vegePool = new VegeData[newCapacity];
		if (array != null)
		{
			Array.Copy(array, this.vegePool, (newCapacity > this.vegeCapacity) ? this.vegeCapacity : newCapacity);
		}
		this.vegeCapacity = newCapacity;
	}

	// Token: 0x060015A4 RID: 5540 RVA: 0x00177B50 File Offset: 0x00175D50
	public int AddVegeData(VegeData vege)
	{
		int num = this.vegeCursor;
		this.vegeCursor = num + 1;
		vege.id = num;
		if (vege.id == this.vegeCapacity)
		{
			this.SetVegeCapacity(this.vegeCapacity * 2);
		}
		this.vegePool[vege.id] = vege;
		return vege.id;
	}

	// Token: 0x060015A5 RID: 5541 RVA: 0x00177BAC File Offset: 0x00175DAC
	private int trans(float x, int pr)
	{
		int num = (int)((Mathf.Sqrt(x + 0.23f) - 0.4795832f) / 0.6294705f * (float)pr);
		if (num >= pr)
		{
			num = pr - 1;
		}
		return num;
	}

	// Token: 0x060015A6 RID: 5542 RVA: 0x00177BE0 File Offset: 0x00175DE0
	public int PositionHash(Vector3 v, int corner = 0)
	{
		if (corner == 0)
		{
			corner = ((v.x > 0f) ? 1 : 0) + ((v.y > 0f) ? 2 : 0) + ((v.z > 0f) ? 4 : 0);
		}
		if (v.x < 0f)
		{
			v.x = -v.x;
		}
		if (v.y < 0f)
		{
			v.y = -v.y;
		}
		if (v.z < 0f)
		{
			v.z = -v.z;
		}
		if ((double)v.x < 1E-06 && (double)v.y < 1E-06 && (double)v.z < 1E-06)
		{
			return 0;
		}
		int num;
		int num2;
		int num3;
		if (v.x >= v.y && v.x >= v.z)
		{
			num = 0;
			num2 = this.trans(v.z / v.x, this.indexMapPrecision);
			num3 = this.trans(v.y / v.x, this.indexMapPrecision);
		}
		else if (v.y >= v.x && v.y >= v.z)
		{
			num = 1;
			num2 = this.trans(v.x / v.y, this.indexMapPrecision);
			num3 = this.trans(v.z / v.y, this.indexMapPrecision);
		}
		else
		{
			num = 2;
			num2 = this.trans(v.x / v.z, this.indexMapPrecision);
			num3 = this.trans(v.y / v.z, this.indexMapPrecision);
		}
		return num2 + num3 * this.indexMapPrecision + num * this.indexMapFaceStride + corner * this.indexMapCornerStride;
	}

	// Token: 0x04001A44 RID: 6724
	public int precision;

	// Token: 0x04001A45 RID: 6725
	public ushort[] heightData;

	// Token: 0x04001A46 RID: 6726
	public byte[] modData;

	// Token: 0x04001A47 RID: 6727
	public ushort[] vegeIds;

	// Token: 0x04001A48 RID: 6728
	public byte[] biomoData;

	// Token: 0x04001A49 RID: 6729
	public short[] temprData;

	// Token: 0x04001A4A RID: 6730
	public Vector3[] vertices;

	// Token: 0x04001A4B RID: 6731
	public Vector3[] normals;

	// Token: 0x04001A4C RID: 6732
	public int[] indexMap;

	// Token: 0x04001A4D RID: 6733
	public int indexMapPrecision;

	// Token: 0x04001A4E RID: 6734
	public int indexMapDataLength;

	// Token: 0x04001A4F RID: 6735
	public int indexMapFaceStride;

	// Token: 0x04001A50 RID: 6736
	public int indexMapCornerStride;

	// Token: 0x04001A51 RID: 6737
	private static Vector3[] verts200;

	// Token: 0x04001A52 RID: 6738
	private static Vector3[] verts80;

	// Token: 0x04001A53 RID: 6739
	private static int[] indexMap200;

	// Token: 0x04001A54 RID: 6740
	private static int[] indexMap80;

	// Token: 0x04001A55 RID: 6741
	public static Vector3[] poles = new Vector3[]
	{
		Vector3.right,
		Vector3.left,
		Vector3.up,
		Vector3.down,
		Vector3.forward,
		Vector3.back
	};

	// Token: 0x04001A56 RID: 6742
	public VeinData[] veinPool;

	// Token: 0x04001A57 RID: 6743
	public int veinCursor = 1;

	// Token: 0x04001A58 RID: 6744
	private int veinCapacity;

	// Token: 0x04001A59 RID: 6745
	public VegeData[] vegePool;

	// Token: 0x04001A5A RID: 6746
	public int vegeCursor = 1;

	// Token: 0x04001A5B RID: 6747
	private int vegeCapacity;
}
