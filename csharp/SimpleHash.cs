using System;

// Token: 0x020001DD RID: 477
public struct SimpleHash
{
	// Token: 0x060015D1 RID: 5585 RVA: 0x0017BD77 File Offset: 0x00179F77
	public void SetEmpty()
	{
		this.bits = 0U;
	}

	// Token: 0x060015D2 RID: 5586 RVA: 0x0017BD80 File Offset: 0x00179F80
	public void InitHashBits(float x, float y, float z)
	{
		int num = (int)((x + 200f) / 40f);
		int num2 = (int)((y + 200f) / 40f);
		int num3 = (int)((z + 200f) / 40f);
		num = ((num < 9) ? ((num < 0) ? 0 : num) : 9);
		num2 = ((num2 < 9) ? ((num2 < 0) ? 0 : num2) : 9);
		num3 = ((num3 < 9) ? ((num3 < 0) ? 0 : num3) : 9);
		this.bits = (1U << num | 1024U << num2 | 1048576U << num3);
	}

	// Token: 0x060015D3 RID: 5587 RVA: 0x0017BE12 File Offset: 0x0017A012
	public bool MaskPass(uint mask)
	{
		return (mask & this.bits) == this.bits;
	}

	// Token: 0x060015D4 RID: 5588 RVA: 0x0017BE24 File Offset: 0x0017A024
	public static uint GenerateHashMask(float x, float y, float z, int area = 1)
	{
		int num = (int)((x + 200f) / 40f);
		int num2 = (int)((y + 200f) / 40f);
		int num3 = (int)((z + 200f) / 40f);
		num = ((num < 9) ? ((num < 0) ? 0 : num) : 9);
		num2 = ((num2 < 9) ? ((num2 < 0) ? 0 : num2) : 9);
		num3 = ((num3 < 9) ? ((num3 < 0) ? 0 : num3) : 9);
		uint num4 = 0U;
		num4 |= 1U << num;
		num4 |= 1024U << num2;
		num4 |= 1048576U << num3;
		if (area > 9)
		{
			area = 9;
		}
		for (int i = 1; i <= area; i++)
		{
			if (num >= i)
			{
				num4 |= 1U << num - i;
			}
			if (num2 >= i)
			{
				num4 |= 1024U << num2 - i;
			}
			if (num3 >= i)
			{
				num4 |= 1048576U << num3 - i;
			}
			if (num < 10 - i)
			{
				num4 |= 1U << num + i;
			}
			if (num2 < 10 - i)
			{
				num4 |= 1024U << num2 + i;
			}
			if (num3 < 10 - i)
			{
				num4 |= 1048576U << num3 + i;
			}
		}
		return num4;
	}

	// Token: 0x04001C27 RID: 7207
	public uint bits;
}
