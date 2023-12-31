using System;
using UnityEngine;

// Token: 0x02000042 RID: 66
[Serializable]
public struct VectorLF3
{
	// Token: 0x17000054 RID: 84
	// (get) Token: 0x0600023B RID: 571 RVA: 0x00016149 File Offset: 0x00014349
	public static VectorLF3 zero
	{
		get
		{
			return new VectorLF3(0f, 0f, 0f);
		}
	}

	// Token: 0x17000055 RID: 85
	// (get) Token: 0x0600023C RID: 572 RVA: 0x0001615F File Offset: 0x0001435F
	public static VectorLF3 one
	{
		get
		{
			return new VectorLF3(1f, 1f, 1f);
		}
	}

	// Token: 0x17000056 RID: 86
	// (get) Token: 0x0600023D RID: 573 RVA: 0x00016175 File Offset: 0x00014375
	public static VectorLF3 minusone
	{
		get
		{
			return new VectorLF3(-1f, -1f, -1f);
		}
	}

	// Token: 0x17000057 RID: 87
	// (get) Token: 0x0600023E RID: 574 RVA: 0x0001618B File Offset: 0x0001438B
	public static VectorLF3 unit_x
	{
		get
		{
			return new VectorLF3(1f, 0f, 0f);
		}
	}

	// Token: 0x17000058 RID: 88
	// (get) Token: 0x0600023F RID: 575 RVA: 0x000161A1 File Offset: 0x000143A1
	public static VectorLF3 unit_y
	{
		get
		{
			return new VectorLF3(0f, 1f, 0f);
		}
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x06000240 RID: 576 RVA: 0x000161B7 File Offset: 0x000143B7
	public static VectorLF3 unit_z
	{
		get
		{
			return new VectorLF3(0f, 0f, 1f);
		}
	}

	// Token: 0x1700005A RID: 90
	// (get) Token: 0x06000241 RID: 577 RVA: 0x000161CD File Offset: 0x000143CD
	public VectorLF2 xy
	{
		get
		{
			return new VectorLF2(this.x, this.y);
		}
	}

	// Token: 0x1700005B RID: 91
	// (get) Token: 0x06000242 RID: 578 RVA: 0x000161E0 File Offset: 0x000143E0
	public VectorLF2 yx
	{
		get
		{
			return new VectorLF2(this.y, this.x);
		}
	}

	// Token: 0x1700005C RID: 92
	// (get) Token: 0x06000243 RID: 579 RVA: 0x000161F3 File Offset: 0x000143F3
	public VectorLF2 zx
	{
		get
		{
			return new VectorLF2(this.z, this.x);
		}
	}

	// Token: 0x1700005D RID: 93
	// (get) Token: 0x06000244 RID: 580 RVA: 0x00016206 File Offset: 0x00014406
	public VectorLF2 xz
	{
		get
		{
			return new VectorLF2(this.x, this.z);
		}
	}

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x06000245 RID: 581 RVA: 0x00016219 File Offset: 0x00014419
	public VectorLF2 yz
	{
		get
		{
			return new VectorLF2(this.y, this.z);
		}
	}

	// Token: 0x1700005F RID: 95
	// (get) Token: 0x06000246 RID: 582 RVA: 0x0001622C File Offset: 0x0001442C
	public VectorLF2 zy
	{
		get
		{
			return new VectorLF2(this.z, this.y);
		}
	}

	// Token: 0x06000247 RID: 583 RVA: 0x0001623F File Offset: 0x0001443F
	public VectorLF3(VectorLF3 vec)
	{
		this.x = vec.x;
		this.y = vec.y;
		this.z = vec.z;
	}

	// Token: 0x06000248 RID: 584 RVA: 0x00016265 File Offset: 0x00014465
	public VectorLF3(double x_, double y_, double z_)
	{
		this.x = x_;
		this.y = y_;
		this.z = z_;
	}

	// Token: 0x06000249 RID: 585 RVA: 0x0001627C File Offset: 0x0001447C
	public VectorLF3(float x_, float y_, float z_)
	{
		this.x = (double)x_;
		this.y = (double)y_;
		this.z = (double)z_;
	}

	// Token: 0x0600024A RID: 586 RVA: 0x00016296 File Offset: 0x00014496
	public static bool operator ==(VectorLF3 lhs, VectorLF3 rhs)
	{
		return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
	}

	// Token: 0x0600024B RID: 587 RVA: 0x000162C4 File Offset: 0x000144C4
	public static bool operator !=(VectorLF3 lhs, VectorLF3 rhs)
	{
		return lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z;
	}

	// Token: 0x0600024C RID: 588 RVA: 0x000162F5 File Offset: 0x000144F5
	public static VectorLF3 operator *(VectorLF3 lhs, VectorLF3 rhs)
	{
		return new VectorLF3(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z);
	}

	// Token: 0x0600024D RID: 589 RVA: 0x00016323 File Offset: 0x00014523
	public static VectorLF3 operator *(VectorLF3 lhs, double rhs)
	{
		return new VectorLF3(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs);
	}

	// Token: 0x0600024E RID: 590 RVA: 0x00016342 File Offset: 0x00014542
	public static VectorLF3 operator /(VectorLF3 lhs, double rhs)
	{
		return new VectorLF3(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs);
	}

	// Token: 0x0600024F RID: 591 RVA: 0x00016361 File Offset: 0x00014561
	public static VectorLF3 operator -(VectorLF3 vec)
	{
		return new VectorLF3(-vec.x, -vec.y, -vec.z);
	}

	// Token: 0x06000250 RID: 592 RVA: 0x0001637D File Offset: 0x0001457D
	public static VectorLF3 operator -(VectorLF3 lhs, VectorLF3 rhs)
	{
		return new VectorLF3(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
	}

	// Token: 0x06000251 RID: 593 RVA: 0x000163AB File Offset: 0x000145AB
	public static VectorLF3 operator +(VectorLF3 lhs, VectorLF3 rhs)
	{
		return new VectorLF3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
	}

	// Token: 0x06000252 RID: 594 RVA: 0x000163D9 File Offset: 0x000145D9
	public static implicit operator VectorLF3(Vector3 vec3)
	{
		return new VectorLF3(vec3.x, vec3.y, vec3.z);
	}

	// Token: 0x06000253 RID: 595 RVA: 0x000163F2 File Offset: 0x000145F2
	public static implicit operator Vector3(VectorLF3 vec3)
	{
		return new Vector3((float)vec3.x, (float)vec3.y, (float)vec3.z);
	}

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x06000254 RID: 596 RVA: 0x0001640E File Offset: 0x0001460E
	public double sqrMagnitude
	{
		get
		{
			return this.x * this.x + this.y * this.y + this.z * this.z;
		}
	}

	// Token: 0x17000061 RID: 97
	// (get) Token: 0x06000255 RID: 597 RVA: 0x00016439 File Offset: 0x00014639
	public double magnitude
	{
		get
		{
			return Math.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z);
		}
	}

	// Token: 0x06000256 RID: 598 RVA: 0x0001646C File Offset: 0x0001466C
	public double Distance(VectorLF3 vec)
	{
		return Math.Sqrt((vec.x - this.x) * (vec.x - this.x) + (vec.y - this.y) * (vec.y - this.y) + (vec.z - this.z) * (vec.z - this.z));
	}

	// Token: 0x06000257 RID: 599 RVA: 0x000164D4 File Offset: 0x000146D4
	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (obj is VectorLF3)
		{
			VectorLF3 vectorLF = (VectorLF3)obj;
			return this.x == vectorLF.x && this.y == vectorLF.y && this.z == vectorLF.z;
		}
		return false;
	}

	// Token: 0x06000258 RID: 600 RVA: 0x00016523 File Offset: 0x00014723
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	// Token: 0x06000259 RID: 601 RVA: 0x00016535 File Offset: 0x00014735
	public override string ToString()
	{
		return string.Format("[{0},{1},{2}]", this.x, this.y, this.z);
	}

	// Token: 0x0600025A RID: 602 RVA: 0x00016562 File Offset: 0x00014762
	public static double Dot(VectorLF3 a, VectorLF3 b)
	{
		return a.x * b.x + a.y * b.y + a.z * b.z;
	}

	// Token: 0x0600025B RID: 603 RVA: 0x00016590 File Offset: 0x00014790
	public static VectorLF3 Cross(VectorLF3 a, VectorLF3 b)
	{
		return new VectorLF3(a.y * b.z - b.y * a.z, a.z * b.x - b.z * a.x, a.x * b.y - b.x * a.y);
	}

	// Token: 0x0600025C RID: 604 RVA: 0x000165F4 File Offset: 0x000147F4
	public static double AngleRAD(VectorLF3 a, VectorLF3 b)
	{
		VectorLF3 normalized = a.normalized;
		VectorLF3 normalized2 = b.normalized;
		double num = normalized.x * normalized2.x + normalized.y * normalized2.y + normalized.z * normalized2.z;
		if (num > 1.0)
		{
			num = 1.0;
		}
		else if (num < -1.0)
		{
			num = -1.0;
		}
		return Math.Acos(num);
	}

	// Token: 0x0600025D RID: 605 RVA: 0x00016670 File Offset: 0x00014870
	public static double AngleDEG(VectorLF3 a, VectorLF3 b)
	{
		VectorLF3 normalized = a.normalized;
		VectorLF3 normalized2 = b.normalized;
		double num = normalized.x * normalized2.x + normalized.y * normalized2.y + normalized.z * normalized2.z;
		if (num > 1.0)
		{
			num = 1.0;
		}
		else if (num < -1.0)
		{
			num = -1.0;
		}
		return Math.Acos(num) / 3.141592653589793 * 180.0;
	}

	// Token: 0x0600025E RID: 606 RVA: 0x00016700 File Offset: 0x00014900
	public static VectorLF3 MoveTowards(VectorLF3 current, VectorLF3 target, double maxDistanceDelta)
	{
		VectorLF3 lhs = target - current;
		double magnitude = lhs.magnitude;
		if (magnitude <= maxDistanceDelta || magnitude == 0.0)
		{
			return target;
		}
		return current + lhs / magnitude * maxDistanceDelta;
	}

	// Token: 0x17000062 RID: 98
	// (get) Token: 0x0600025F RID: 607 RVA: 0x00016744 File Offset: 0x00014944
	public VectorLF3 normalized
	{
		get
		{
			double num = this.x * this.x + this.y * this.y + this.z * this.z;
			if (num < 1E-34)
			{
				return new VectorLF3(0f, 0f, 0f);
			}
			double num2 = Math.Sqrt(num);
			return new VectorLF3(this.x / num2, this.y / num2, this.z / num2);
		}
	}

	// Token: 0x0400030A RID: 778
	public double x;

	// Token: 0x0400030B RID: 779
	public double y;

	// Token: 0x0400030C RID: 780
	public double z;
}
