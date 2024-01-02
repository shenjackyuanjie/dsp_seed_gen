using System;

// Token: 0x0200056E RID: 1390
public class PlanetAlgorithm0 : PlanetAlgorithm
{
	// Token: 0x06003A8D RID: 14989 RVA: 0x0030E2AC File Offset: 0x0030C4AC
	public override void GenerateTerrain(double modX, double modY)
	{
		PlanetRawData data = this.planet.data;
		for (int i = 0; i < data.dataLength; i++)
		{
			data.heightData[i] = (ushort)((double)this.planet.radius * 100.0);
			data.biomoData[i] = 0;
		}
	}

	// Token: 0x06003A8E RID: 14990 RVA: 0x00008BD9 File Offset: 0x00006DD9
	public override void GenerateVegetables()
	{
	}

	// Token: 0x06003A8F RID: 14991 RVA: 0x00008BD9 File Offset: 0x00006DD9
	public override void GenerateVeins()
	{
	}
}
