using System;

// Token: 0x020001B2 RID: 434
public class GalaxyData
{
	// Token: 0x06001319 RID: 4889 RVA: 0x00147B90 File Offset: 0x00145D90
	public GalaxyData()
	{
		this.astrosData = new AstroData[25700];
		this.astrosFactory = new PlanetFactory[25700];
	}

	// Token: 0x0600131A RID: 4890 RVA: 0x00147BB8 File Offset: 0x00145DB8
	public void UnloadAll()
	{
		for (int i = 0; i < this.starCount; i++)
		{
			this.stars[i].Unload();
		}
	}

	// Token: 0x0600131B RID: 4891 RVA: 0x00147BE4 File Offset: 0x00145DE4
	public void Free()
	{
		for (int i = 0; i < this.starCount; i++)
		{
			this.stars[i].Free();
			this.stars[i] = null;
		}
		this.stars = null;
		for (int j = 0; j < this.starCount; j++)
		{
			this.graphNodes[j].Free();
			this.graphNodes[j] = null;
		}
		this.graphNodes = null;
		this.astrosData = null;
	}

	// Token: 0x0600131C RID: 4892 RVA: 0x00147C54 File Offset: 0x00145E54
	public StarData StarById(int starId)
	{
		int num = starId - 1;
		if (num < 0 || num >= this.stars.Length)
		{
			return null;
		}
		return this.stars[num];
	}

	// Token: 0x0600131D RID: 4893 RVA: 0x00147C80 File Offset: 0x00145E80
	public PlanetData PlanetById(int planetId)
	{
		int num = planetId / 100 - 1;
		int num2 = planetId % 100 - 1;
		if (num < 0 || num >= this.stars.Length)
		{
			return null;
		}
		if (this.stars[num] == null)
		{
			return null;
		}
		if (num2 < 0 || num2 >= this.stars[num].planets.Length)
		{
			return null;
		}
		return this.stars[num].planets[num2];
	}

	// Token: 0x0600131E RID: 4894 RVA: 0x00147CE0 File Offset: 0x00145EE0
	public void UpdatePoses(double time)
	{
		for (int i = 0; i < this.starCount; i++)
		{
			for (int j = 0; j < this.stars[i].planetCount; j++)
			{
				this.stars[i].planets[j].UpdateRuntimePose(time);
			}
		}
	}

	// Token: 0x0600131F RID: 4895 RVA: 0x00147D2C File Offset: 0x00145F2C
	public void RegeneratePlanetNames()
	{
		foreach (StarData starData in this.stars)
		{
			if (!string.IsNullOrEmpty(starData.overrideName))
			{
				starData.RegeneratePlanetNames(false);
			}
		}
	}

	// Token: 0x1400001D RID: 29
	// (add) Token: 0x06001320 RID: 4896 RVA: 0x00147D68 File Offset: 0x00145F68
	// (remove) Token: 0x06001321 RID: 4897 RVA: 0x00147DA0 File Offset: 0x00145FA0
	public event Action onAstroNameChange;

	// Token: 0x06001322 RID: 4898 RVA: 0x00147DD5 File Offset: 0x00145FD5
	public void NotifyAstroNameChange()
	{
		if (this.onAstroNameChange != null)
		{
			this.onAstroNameChange();
		}
	}

	// Token: 0x04001785 RID: 6021
	public int seed;

	// Token: 0x04001786 RID: 6022
	public int starCount;

	// Token: 0x04001787 RID: 6023
	public StarData[] stars;

	// Token: 0x04001788 RID: 6024
	public int birthPlanetId;

	// Token: 0x04001789 RID: 6025
	public int birthStarId;

	// Token: 0x0400178A RID: 6026
	public int habitableCount;

	// Token: 0x0400178B RID: 6027
	public const double AU = 40000.0;

	// Token: 0x0400178C RID: 6028
	public const double LY = 2400000.0;

	// Token: 0x0400178D RID: 6029
	public StarGraphNode[] graphNodes;

	// Token: 0x0400178E RID: 6030
	public AstroData[] astrosData;

	// Token: 0x0400178F RID: 6031
	public PlanetFactory[] astrosFactory;
}
