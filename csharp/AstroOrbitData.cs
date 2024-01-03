using System;
using UnityEngine;

// Token: 0x0200019A RID: 410
public class AstroOrbitData
{
	// Token: 0x0600121B RID: 4635 RVA: 0x00138B00 File Offset: 0x00136D00
	public void PredictPose(long time, VectorLF3 center, ref AstroData astroData)
	{
		double num = 40000.0 * (double)this.orbitRadius;
		double num2 = (double)time / (this.orbitalPeriod * 60.0) + (double)this.orbitPhase / 360.0;
		int num3 = (int)(num2 + 0.1);
		num2 -= (double)num3;
		this.runtimeOrbitPhase = (float)num2 * 360f;
		num2 *= 6.283185307179586;
		astroData.uPos = Maths.QRotateLF(this.orbitRotation, new VectorLF3((float)Math.Cos(num2), 0f, (float)Math.Sin(num2)));
		astroData.uRot = Quaternion.LookRotation(this.orbitNormal, astroData.uPos);
		astroData.uPos.x = astroData.uPos.x * num + center.x;
		astroData.uPos.y = astroData.uPos.y * num + center.y;
		astroData.uPos.z = astroData.uPos.z * num + center.z;
		double num4 = (double)(time + 1L) / (this.orbitalPeriod * 60.0) + (double)this.orbitPhase / 360.0;
		int num5 = (int)(num4 + 0.1);
		num4 -= (double)num5;
		num4 *= 6.283185307179586;
		astroData.uPosNext = Maths.QRotateLF(this.orbitRotation, new VectorLF3((float)Math.Cos(num4), 0f, (float)Math.Sin(num4)));
		astroData.uRotNext = Quaternion.LookRotation(this.orbitNormal, astroData.uPosNext);
		astroData.uPosNext.x = astroData.uPosNext.x * num + center.x;
		astroData.uPosNext.y = astroData.uPosNext.y * num + center.y;
		astroData.uPosNext.z = astroData.uPosNext.z * num + center.z;
	}

	// Token: 0x0600121C RID: 4636 RVA: 0x00138D08 File Offset: 0x00136F08
	public VectorLF3 GetVelocityAtPoint(VectorLF3 center, VectorLF3 upos)
	{
		VectorLF3 vectorLF;
		vectorLF.x = upos.x - center.x;
		vectorLF.y = upos.y - center.y;
		vectorLF.z = upos.z - center.z;
		return Maths.QRotateLF(Quaternion.AngleAxis(-360f / (float)(this.orbitalPeriod * 60.0), this.orbitNormal), vectorLF) - vectorLF;
	}

	// Token: 0x0600121D RID: 4637 RVA: 0x00138D84 File Offset: 0x00136F84
	public float GetEstimatePointOffset(float eta)
	{
		double num = 0.017453292519943295 * (double)eta * 360.0 / this.orbitalPeriod;
		return (float)((double)this.orbitRadius * 40000.0 * num * num * 0.5);
	}

	// Token: 0x040015AC RID: 5548
	public float orbitRadius = 1f;

	// Token: 0x040015AD RID: 5549
	public float orbitInclination;

	// Token: 0x040015AE RID: 5550
	public float orbitLongitude;

	// Token: 0x040015AF RID: 5551
	public double orbitalPeriod = 3600.0;

	// Token: 0x040015B0 RID: 5552
	public float orbitPhase;

	// Token: 0x040015B1 RID: 5553
	public float runtimeOrbitPhase;

	// Token: 0x040015B2 RID: 5554
	public Quaternion orbitRotation;

	// Token: 0x040015B3 RID: 5555
	public VectorLF3 orbitNormal;
}
