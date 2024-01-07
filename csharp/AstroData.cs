using System;
using UnityEngine;

// Token: 0x02000197 RID: 407
public struct AstroData
{
	// Token: 0x06001208 RID: 4616 RVA: 0x00136EF8 File Offset: 0x001350F8
	public void PositionU(ref VectorLF3 lpos, out VectorLF3 upos)
	{
		double num = 2.0 * lpos.x;
		double num2 = 2.0 * lpos.y;
		double num3 = 2.0 * lpos.z;
		double num4 = (double)(this.uRot.w * this.uRot.w) - 0.5;
		double num5 = (double)this.uRot.x * num + (double)this.uRot.y * num2 + (double)this.uRot.z * num3;
		upos.x = num * num4 + ((double)this.uRot.y * num3 - (double)this.uRot.z * num2) * (double)this.uRot.w + (double)this.uRot.x * num5 + this.uPos.x;
		upos.y = num2 * num4 + ((double)this.uRot.z * num - (double)this.uRot.x * num3) * (double)this.uRot.w + (double)this.uRot.y * num5 + this.uPos.y;
		upos.z = num3 * num4 + ((double)this.uRot.x * num2 - (double)this.uRot.y * num) * (double)this.uRot.w + (double)this.uRot.z * num5 + this.uPos.z;
	}

	// Token: 0x06001209 RID: 4617 RVA: 0x0013707C File Offset: 0x0013527C
	public void PositionU(ref Vector3 lpos, out VectorLF3 upos)
	{
		float num = 2f * lpos.x;
		float num2 = 2f * lpos.y;
		float num3 = 2f * lpos.z;
		float num4 = this.uRot.w * this.uRot.w - 0.5f;
		float num5 = this.uRot.x * num + this.uRot.y * num2 + this.uRot.z * num3;
		upos.x = (double)(num * num4 + (this.uRot.y * num3 - this.uRot.z * num2) * this.uRot.w + this.uRot.x * num5) + this.uPos.x;
		upos.y = (double)(num2 * num4 + (this.uRot.z * num - this.uRot.x * num3) * this.uRot.w + this.uRot.y * num5) + this.uPos.y;
		upos.z = (double)(num3 * num4 + (this.uRot.x * num2 - this.uRot.y * num) * this.uRot.w + this.uRot.z * num5) + this.uPos.z;
	}

	// Token: 0x0600120A RID: 4618 RVA: 0x001371E0 File Offset: 0x001353E0
	public void VelocityU(ref VectorLF3 lpos, out Vector3 uvel)
	{
		double num = 2.0 * lpos.x;
		double num2 = 2.0 * lpos.y;
		double num3 = 2.0 * lpos.z;
		double num4 = (double)(this.uRot.w * this.uRot.w) - 0.5;
		double num5 = (double)this.uRot.x * num + (double)this.uRot.y * num2 + (double)this.uRot.z * num3;
		double num6 = (double)(this.uRotNext.w * this.uRotNext.w) - 0.5;
		double num7 = (double)this.uRotNext.x * num + (double)this.uRotNext.y * num2 + (double)this.uRotNext.z * num3;
		uvel.x = (float)(num * num6 + ((double)this.uRotNext.y * num3 - (double)this.uRotNext.z * num2) * (double)this.uRotNext.w + (double)this.uRotNext.x * num7 + this.uPosNext.x - (num * num4 + ((double)this.uRot.y * num3 - (double)this.uRot.z * num2) * (double)this.uRot.w + (double)this.uRot.x * num5 + this.uPos.x)) * 60f;
		uvel.y = (float)(num2 * num6 + ((double)this.uRotNext.z * num - (double)this.uRotNext.x * num3) * (double)this.uRotNext.w + (double)this.uRotNext.y * num7 + this.uPosNext.y - (num2 * num4 + ((double)this.uRot.z * num - (double)this.uRot.x * num3) * (double)this.uRot.w + (double)this.uRot.y * num5 + this.uPos.y)) * 60f;
		uvel.z = (float)(num3 * num6 + ((double)this.uRotNext.x * num2 - (double)this.uRotNext.y * num) * (double)this.uRotNext.w + (double)this.uRotNext.z * num7 + this.uPosNext.z - (num3 * num4 + ((double)this.uRot.x * num2 - (double)this.uRot.y * num) * (double)this.uRot.w + (double)this.uRot.z * num5 + this.uPos.z)) * 60f;
	}

	// Token: 0x0600120B RID: 4619 RVA: 0x001374AC File Offset: 0x001356AC
	public void VelocityU(ref Vector3 lpos, out Vector3 uvel)
	{
		double num = 2.0 * (double)lpos.x;
		double num2 = 2.0 * (double)lpos.y;
		double num3 = 2.0 * (double)lpos.z;
		double num4 = (double)(this.uRot.w * this.uRot.w) - 0.5;
		double num5 = (double)this.uRot.x * num + (double)this.uRot.y * num2 + (double)this.uRot.z * num3;
		double num6 = (double)(this.uRotNext.w * this.uRotNext.w) - 0.5;
		double num7 = (double)this.uRotNext.x * num + (double)this.uRotNext.y * num2 + (double)this.uRotNext.z * num3;
		uvel.x = (float)(num * num6 + ((double)this.uRotNext.y * num3 - (double)this.uRotNext.z * num2) * (double)this.uRotNext.w + (double)this.uRotNext.x * num7 + this.uPosNext.x - (num * num4 + ((double)this.uRot.y * num3 - (double)this.uRot.z * num2) * (double)this.uRot.w + (double)this.uRot.x * num5 + this.uPos.x)) * 60f;
		uvel.y = (float)(num2 * num6 + ((double)this.uRotNext.z * num - (double)this.uRotNext.x * num3) * (double)this.uRotNext.w + (double)this.uRotNext.y * num7 + this.uPosNext.y - (num2 * num4 + ((double)this.uRot.z * num - (double)this.uRot.x * num3) * (double)this.uRot.w + (double)this.uRot.y * num5 + this.uPos.y)) * 60f;
		uvel.z = (float)(num3 * num6 + ((double)this.uRotNext.x * num2 - (double)this.uRotNext.y * num) * (double)this.uRotNext.w + (double)this.uRotNext.z * num7 + this.uPosNext.z - (num3 * num4 + ((double)this.uRot.x * num2 - (double)this.uRot.y * num) * (double)this.uRot.w + (double)this.uRot.z * num5 + this.uPos.z)) * 60f;
	}

	// Token: 0x0600120C RID: 4620 RVA: 0x0013777C File Offset: 0x0013597C
	public void VelocityL2U(ref Vector3 lpos, ref Vector3 lvel, out Vector3 uvel)
	{
		float num = 2f * lvel.x;
		float num2 = 2f * lvel.y;
		float num3 = 2f * lvel.z;
		float num4 = this.uRot.w * this.uRot.w - 0.5f;
		float num5 = this.uRot.x * num + this.uRot.y * num2 + this.uRot.z * num3;
		uvel.x = num * num4 + (this.uRot.y * num3 - this.uRot.z * num2) * this.uRot.w + this.uRot.x * num5;
		uvel.y = num2 * num4 + (this.uRot.z * num - this.uRot.x * num3) * this.uRot.w + this.uRot.y * num5;
		uvel.z = num3 * num4 + (this.uRot.x * num2 - this.uRot.y * num) * this.uRot.w + this.uRot.z * num5;
		double num6 = 2.0 * (double)lpos.x;
		double num7 = 2.0 * (double)lpos.y;
		double num8 = 2.0 * (double)lpos.z;
		double num9 = (double)(this.uRot.w * this.uRot.w) - 0.5;
		double num10 = (double)this.uRot.x * num6 + (double)this.uRot.y * num7 + (double)this.uRot.z * num8;
		double num11 = (double)(this.uRotNext.w * this.uRotNext.w) - 0.5;
		double num12 = (double)this.uRotNext.x * num6 + (double)this.uRotNext.y * num7 + (double)this.uRotNext.z * num8;
		uvel.x += (float)(num6 * num11 + ((double)this.uRotNext.y * num8 - (double)this.uRotNext.z * num7) * (double)this.uRotNext.w + (double)this.uRotNext.x * num12 + this.uPosNext.x - (num6 * num9 + ((double)this.uRot.y * num8 - (double)this.uRot.z * num7) * (double)this.uRot.w + (double)this.uRot.x * num10 + this.uPos.x)) * 60f;
		uvel.y += (float)(num7 * num11 + ((double)this.uRotNext.z * num6 - (double)this.uRotNext.x * num8) * (double)this.uRotNext.w + (double)this.uRotNext.y * num12 + this.uPosNext.y - (num7 * num9 + ((double)this.uRot.z * num6 - (double)this.uRot.x * num8) * (double)this.uRot.w + (double)this.uRot.y * num10 + this.uPos.y)) * 60f;
		uvel.z += (float)(num8 * num11 + ((double)this.uRotNext.x * num7 - (double)this.uRotNext.y * num6) * (double)this.uRotNext.w + (double)this.uRotNext.z * num12 + this.uPosNext.z - (num8 * num9 + ((double)this.uRot.x * num7 - (double)this.uRot.y * num6) * (double)this.uRot.w + (double)this.uRot.z * num10 + this.uPos.z)) * 60f;
	}

	// Token: 0x0600120D RID: 4621 RVA: 0x00137BA8 File Offset: 0x00135DA8
	public void VelocityL2U(ref VectorLF3 lpos, ref Vector3 lvel, out Vector3 uvel)
	{
		float num = 2f * lvel.x;
		float num2 = 2f * lvel.y;
		float num3 = 2f * lvel.z;
		float num4 = this.uRot.w * this.uRot.w - 0.5f;
		float num5 = this.uRot.x * num + this.uRot.y * num2 + this.uRot.z * num3;
		uvel.x = num * num4 + (this.uRot.y * num3 - this.uRot.z * num2) * this.uRot.w + this.uRot.x * num5;
		uvel.y = num2 * num4 + (this.uRot.z * num - this.uRot.x * num3) * this.uRot.w + this.uRot.y * num5;
		uvel.z = num3 * num4 + (this.uRot.x * num2 - this.uRot.y * num) * this.uRot.w + this.uRot.z * num5;
		double num6 = 2.0 * lpos.x;
		double num7 = 2.0 * lpos.y;
		double num8 = 2.0 * lpos.z;
		double num9 = (double)(this.uRot.w * this.uRot.w) - 0.5;
		double num10 = (double)this.uRot.x * num6 + (double)this.uRot.y * num7 + (double)this.uRot.z * num8;
		double num11 = (double)(this.uRotNext.w * this.uRotNext.w) - 0.5;
		double num12 = (double)this.uRotNext.x * num6 + (double)this.uRotNext.y * num7 + (double)this.uRotNext.z * num8;
		uvel.x += (float)(num6 * num11 + ((double)this.uRotNext.y * num8 - (double)this.uRotNext.z * num7) * (double)this.uRotNext.w + (double)this.uRotNext.x * num12 + this.uPosNext.x - (num6 * num9 + ((double)this.uRot.y * num8 - (double)this.uRot.z * num7) * (double)this.uRot.w + (double)this.uRot.x * num10 + this.uPos.x)) * 60f;
		uvel.y += (float)(num7 * num11 + ((double)this.uRotNext.z * num6 - (double)this.uRotNext.x * num8) * (double)this.uRotNext.w + (double)this.uRotNext.y * num12 + this.uPosNext.y - (num7 * num9 + ((double)this.uRot.z * num6 - (double)this.uRot.x * num8) * (double)this.uRot.w + (double)this.uRot.y * num10 + this.uPos.y)) * 60f;
		uvel.z += (float)(num8 * num11 + ((double)this.uRotNext.x * num7 - (double)this.uRotNext.y * num6) * (double)this.uRotNext.w + (double)this.uRotNext.z * num12 + this.uPosNext.z - (num8 * num9 + ((double)this.uRot.x * num7 - (double)this.uRot.y * num6) * (double)this.uRot.w + (double)this.uRot.z * num10 + this.uPos.z)) * 60f;
	}

	// Token: 0x0600120E RID: 4622 RVA: 0x00137FD0 File Offset: 0x001361D0
	public void VelocityL2U(ref VectorLF3 lpos, ref Vector3 lvel, out Vector3 uvel_astro, out Vector3 uvel_obj)
	{
		float num = 2f * lvel.x;
		float num2 = 2f * lvel.y;
		float num3 = 2f * lvel.z;
		float num4 = this.uRot.w * this.uRot.w - 0.5f;
		float num5 = this.uRot.x * num + this.uRot.y * num2 + this.uRot.z * num3;
		uvel_obj.x = num * num4 + (this.uRot.y * num3 - this.uRot.z * num2) * this.uRot.w + this.uRot.x * num5;
		uvel_obj.y = num2 * num4 + (this.uRot.z * num - this.uRot.x * num3) * this.uRot.w + this.uRot.y * num5;
		uvel_obj.z = num3 * num4 + (this.uRot.x * num2 - this.uRot.y * num) * this.uRot.w + this.uRot.z * num5;
		double num6 = 2.0 * lpos.x;
		double num7 = 2.0 * lpos.y;
		double num8 = 2.0 * lpos.z;
		double num9 = (double)(this.uRot.w * this.uRot.w) - 0.5;
		double num10 = (double)this.uRot.x * num6 + (double)this.uRot.y * num7 + (double)this.uRot.z * num8;
		double num11 = (double)(this.uRotNext.w * this.uRotNext.w) - 0.5;
		double num12 = (double)this.uRotNext.x * num6 + (double)this.uRotNext.y * num7 + (double)this.uRotNext.z * num8;
		uvel_astro.x = (float)(num6 * num11 + ((double)this.uRotNext.y * num8 - (double)this.uRotNext.z * num7) * (double)this.uRotNext.w + (double)this.uRotNext.x * num12 + this.uPosNext.x - (num6 * num9 + ((double)this.uRot.y * num8 - (double)this.uRot.z * num7) * (double)this.uRot.w + (double)this.uRot.x * num10 + this.uPos.x)) * 60f;
		uvel_astro.y = (float)(num7 * num11 + ((double)this.uRotNext.z * num6 - (double)this.uRotNext.x * num8) * (double)this.uRotNext.w + (double)this.uRotNext.y * num12 + this.uPosNext.y - (num7 * num9 + ((double)this.uRot.z * num6 - (double)this.uRot.x * num8) * (double)this.uRot.w + (double)this.uRot.y * num10 + this.uPos.y)) * 60f;
		uvel_astro.z = (float)(num8 * num11 + ((double)this.uRotNext.x * num7 - (double)this.uRotNext.y * num6) * (double)this.uRotNext.w + (double)this.uRotNext.z * num12 + this.uPosNext.z - (num8 * num9 + ((double)this.uRot.x * num7 - (double)this.uRot.y * num6) * (double)this.uRot.w + (double)this.uRot.z * num10 + this.uPos.z)) * 60f;
	}

	// Token: 0x0600120F RID: 4623 RVA: 0x001383F0 File Offset: 0x001365F0
	public void VelocityU2L(ref Vector3 lpos, ref Vector3 uvel, out Vector3 lvel)
	{
		double num = 2.0 * (double)lpos.x;
		double num2 = 2.0 * (double)lpos.y;
		double num3 = 2.0 * (double)lpos.z;
		double num4 = (double)(this.uRot.w * this.uRot.w) - 0.5;
		double num5 = (double)this.uRot.x * num + (double)this.uRot.y * num2 + (double)this.uRot.z * num3;
		double num6 = (double)(this.uRotNext.w * this.uRotNext.w) - 0.5;
		double num7 = (double)this.uRotNext.x * num + (double)this.uRotNext.y * num2 + (double)this.uRotNext.z * num3;
		Vector3 vector;
		vector.x = (float)(num * num6 + ((double)this.uRotNext.y * num3 - (double)this.uRotNext.z * num2) * (double)this.uRotNext.w + (double)this.uRotNext.x * num7 + this.uPosNext.x - (num * num4 + ((double)this.uRot.y * num3 - (double)this.uRot.z * num2) * (double)this.uRot.w + (double)this.uRot.x * num5 + this.uPos.x)) * 60f;
		vector.y = (float)(num2 * num6 + ((double)this.uRotNext.z * num - (double)this.uRotNext.x * num3) * (double)this.uRotNext.w + (double)this.uRotNext.y * num7 + this.uPosNext.y - (num2 * num4 + ((double)this.uRot.z * num - (double)this.uRot.x * num3) * (double)this.uRot.w + (double)this.uRot.y * num5 + this.uPos.y)) * 60f;
		vector.z = (float)(num3 * num6 + ((double)this.uRotNext.x * num2 - (double)this.uRotNext.y * num) * (double)this.uRotNext.w + (double)this.uRotNext.z * num7 + this.uPosNext.z - (num3 * num4 + ((double)this.uRot.x * num2 - (double)this.uRot.y * num) * (double)this.uRot.w + (double)this.uRot.z * num5 + this.uPos.z)) * 60f;
		Vector3 vector2 = new Vector3(uvel.x - vector.x, uvel.y - vector.y, uvel.z - vector.z);
		num = 2.0 * (double)vector2.x;
		num2 = 2.0 * (double)vector2.y;
		num3 = 2.0 * (double)vector2.z;
		num4 = (double)(this.uRot.w * this.uRot.w) - 0.5;
		num5 = (double)this.uRot.x * num + (double)this.uRot.y * num2 + (double)this.uRot.z * num3;
		lvel.x = (float)(num * num4 - ((double)this.uRot.y * num3 - (double)this.uRot.z * num2) * (double)this.uRot.w + (double)this.uRot.x * num5);
		lvel.y = (float)(num2 * num4 - ((double)this.uRot.z * num - (double)this.uRot.x * num3) * (double)this.uRot.w + (double)this.uRot.y * num5);
		lvel.z = (float)(num3 * num4 - ((double)this.uRot.x * num2 - (double)this.uRot.y * num) * (double)this.uRot.w + (double)this.uRot.z * num5);
	}

	// Token: 0x06001210 RID: 4624 RVA: 0x0013884C File Offset: 0x00136A4C
	public void VelocityU2L(ref VectorLF3 lpos, ref Vector3 uvel, out Vector3 lvel)
	{
		double num = 2.0 * lpos.x;
		double num2 = 2.0 * lpos.y;
		double num3 = 2.0 * lpos.z;
		double num4 = (double)(this.uRot.w * this.uRot.w) - 0.5;
		double num5 = (double)this.uRot.x * num + (double)this.uRot.y * num2 + (double)this.uRot.z * num3;
		double num6 = (double)(this.uRotNext.w * this.uRotNext.w) - 0.5;
		double num7 = (double)this.uRotNext.x * num + (double)this.uRotNext.y * num2 + (double)this.uRotNext.z * num3;
		Vector3 vector;
		vector.x = (float)(num * num6 + ((double)this.uRotNext.y * num3 - (double)this.uRotNext.z * num2) * (double)this.uRotNext.w + (double)this.uRotNext.x * num7 + this.uPosNext.x - (num * num4 + ((double)this.uRot.y * num3 - (double)this.uRot.z * num2) * (double)this.uRot.w + (double)this.uRot.x * num5 + this.uPos.x)) * 60f;
		vector.y = (float)(num2 * num6 + ((double)this.uRotNext.z * num - (double)this.uRotNext.x * num3) * (double)this.uRotNext.w + (double)this.uRotNext.y * num7 + this.uPosNext.y - (num2 * num4 + ((double)this.uRot.z * num - (double)this.uRot.x * num3) * (double)this.uRot.w + (double)this.uRot.y * num5 + this.uPos.y)) * 60f;
		vector.z = (float)(num3 * num6 + ((double)this.uRotNext.x * num2 - (double)this.uRotNext.y * num) * (double)this.uRotNext.w + (double)this.uRotNext.z * num7 + this.uPosNext.z - (num3 * num4 + ((double)this.uRot.x * num2 - (double)this.uRot.y * num) * (double)this.uRot.w + (double)this.uRot.z * num5 + this.uPos.z)) * 60f;
		Vector3 vector2 = new Vector3(uvel.x - vector.x, uvel.y - vector.y, uvel.z - vector.z);
		num = 2.0 * (double)vector2.x;
		num2 = 2.0 * (double)vector2.y;
		num3 = 2.0 * (double)vector2.z;
		num4 = (double)(this.uRot.w * this.uRot.w) - 0.5;
		num5 = (double)this.uRot.x * num + (double)this.uRot.y * num2 + (double)this.uRot.z * num3;
		lvel.x = (float)(num * num4 - ((double)this.uRot.y * num3 - (double)this.uRot.z * num2) * (double)this.uRot.w + (double)this.uRot.x * num5);
		lvel.y = (float)(num2 * num4 - ((double)this.uRot.z * num - (double)this.uRot.x * num3) * (double)this.uRot.w + (double)this.uRot.y * num5);
		lvel.z = (float)(num3 * num4 - ((double)this.uRot.x * num2 - (double)this.uRot.y * num) * (double)this.uRot.w + (double)this.uRot.z * num5);
	}

	// Token: 0x06001211 RID: 4625 RVA: 0x00138CA8 File Offset: 0x00136EA8
	public void SetEmpty()
	{
		this.id = 0;
		this.type = EAstroType.None;
		this.parentId = 0;
		this.uRadius = 0f;
		this.uRot.x = (this.uRot.y = (this.uRot.z = 0f));
		this.uRotNext.x = (this.uRotNext.y = (this.uRotNext.z = 0f));
		this.uRot.w = (this.uRotNext.w = 1f);
		this.uPos.x = (this.uPos.y = (this.uPos.z = 0.0));
		this.uPosNext.x = (this.uPosNext.y = (this.uPosNext.z = 0.0));
	}

	// Token: 0x06001212 RID: 4626 RVA: 0x00138DAC File Offset: 0x00136FAC
	public static string IdString(int id)
	{
		if (id == 0)
		{
			return "0";
		}
		if (id > 1000000)
		{
			return string.Format("S{0}", id - 1000000);
		}
		if (id <= 204899)
		{
			return string.Format("G{0}", id);
		}
		return "??";
	}

	// Token: 0x0400159E RID: 5534
	public int id;

	// Token: 0x0400159F RID: 5535
	public EAstroType type;

	// Token: 0x040015A0 RID: 5536
	public int parentId;

	// Token: 0x040015A1 RID: 5537
	public float uRadius;

	// Token: 0x040015A2 RID: 5538
	public Quaternion uRot;

	// Token: 0x040015A3 RID: 5539
	public Quaternion uRotNext;

	// Token: 0x040015A4 RID: 5540
	public VectorLF3 uPos;

	// Token: 0x040015A5 RID: 5541
	public VectorLF3 uPosNext;
}
