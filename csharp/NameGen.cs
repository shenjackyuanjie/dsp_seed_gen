using System;

// Token: 0x0200056B RID: 1387
public static class NameGen
{
	// Token: 0x06003A7A RID: 14970 RVA: 0x0030B0E0 File Offset: 0x003092E0
	public static string RandomName(int seed)
	{
		DotNet35Random dotNet35Random = new DotNet35Random(seed);
		int num = (int)(dotNet35Random.NextDouble() * 1.8 + 2.3);
		string text = "";
		for (int i = 0; i < num; i++)
		{
			if (dotNet35Random.NextDouble() < 0.05000000074505806 && i == 0)
			{
				text += NameGen.vow0[dotNet35Random.Next(NameGen.vow0.Length)];
			}
			else
			{
				if (dotNet35Random.NextDouble() < 0.9700000286102295 || num >= 4)
				{
					text += NameGen.con0[dotNet35Random.Next(NameGen.con0.Length)];
				}
				else
				{
					text += NameGen.con1[dotNet35Random.Next(NameGen.con1.Length)];
				}
				if (i == num - 1 && dotNet35Random.NextDouble() < 0.8999999761581421)
				{
					text += NameGen.ending[dotNet35Random.Next(NameGen.ending.Length)];
				}
				else if (dotNet35Random.NextDouble() < 0.9700000286102295)
				{
					text += NameGen.vow1[dotNet35Random.Next(NameGen.vow1.Length)];
				}
				else
				{
					text += NameGen.vow2[dotNet35Random.Next(NameGen.vow2.Length)];
				}
			}
		}
		if (text.IndexOf("uu") >= 0)
		{
			text = text.Replace("uu", "u");
		}
		if (text.IndexOf("ooo") >= 0)
		{
			text = text.Replace("ooo", "oo");
		}
		if (text.IndexOf("eee") >= 0)
		{
			text = text.Replace("eee", "ee");
		}
		if (text.IndexOf("eea") >= 0)
		{
			text = text.Replace("eea", "ea");
		}
		if (text.IndexOf("aa") >= 0)
		{
			text = text.Replace("aa", "a");
		}
		if (text.IndexOf("yy") >= 0)
		{
			text = text.Replace("yy", "y");
		}
		return text.Substring(0, 1).ToUpper() + text.Substring(1);
	}

	// Token: 0x06003A7B RID: 14971 RVA: 0x0030B2F8 File Offset: 0x003094F8
	public static string RandomStarName(int seed, StarData starData, GalaxyData galaxy)
	{
		DotNet35Random dotNet35Random = new DotNet35Random(seed);
		int num = 0;
		while (num++ < 256)
		{
			string text = NameGen._RandomStarName(dotNet35Random.Next(), starData);
			bool flag = false;
			for (int i = 0; i < galaxy.starCount; i++)
			{
				if (galaxy.stars[i] != null && galaxy.stars[i].name.Equals(text))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return text;
			}
		}
		return "XStar";
	}

	// Token: 0x06003A7C RID: 14972 RVA: 0x0030B370 File Offset: 0x00309570
	private static string _RandomStarName(int seed, StarData starData)
	{
		DotNet35Random dotNet35Random = new DotNet35Random(seed);
		int seed2 = dotNet35Random.Next();
		double num = dotNet35Random.NextDouble();
		double num2 = dotNet35Random.NextDouble();
		if (starData.type == EStarType.GiantStar)
		{
			if (num2 < 0.4000000059604645)
			{
				return NameGen.RandomGiantStarNameFromRawNames(seed2);
			}
			if (num2 < 0.699999988079071)
			{
				return NameGen.RandomGiantStarNameWithConstellationAlpha(seed2);
			}
			return NameGen.RandomGiantStarNameWithFormat(seed2);
		}
		else
		{
			if (starData.type == EStarType.NeutronStar)
			{
				return NameGen.RandomNeutronStarNameWithFormat(seed2);
			}
			if (starData.type == EStarType.BlackHole)
			{
				return NameGen.RandomBlackHoleNameWithFormat(seed2);
			}
			if (num < 0.6000000238418579)
			{
				return NameGen.RandomStarNameFromRawNames(seed2);
			}
			if (num < 0.9300000071525574)
			{
				return NameGen.RandomStarNameWithConstellationAlpha(seed2);
			}
			return NameGen.RandomStarNameWithConstellationNumber(seed2);
		}
	}

	// Token: 0x06003A7D RID: 14973 RVA: 0x0030B41C File Offset: 0x0030961C
	public static string RandomStarNameFromRawNames(int seed)
	{
		int num = new DotNet35Random(seed).Next();
		num %= NameGen.raw_star_names.Length;
		return NameGen.raw_star_names[num];
	}

	// Token: 0x06003A7E RID: 14974 RVA: 0x0030B448 File Offset: 0x00309648
	public static string RandomStarNameWithConstellationAlpha(int seed)
	{
		DotNet35Random dotNet35Random = new DotNet35Random(seed);
		int num = dotNet35Random.Next();
		int num2 = dotNet35Random.Next();
		num %= NameGen.constellations.Length;
		num2 %= NameGen.alphabeta.Length;
		string text = NameGen.constellations[num];
		if (text.Length > 10)
		{
			return NameGen.alphabeta_letter[num2] + " " + text;
		}
		return NameGen.alphabeta[num2] + " " + text;
	}

	// Token: 0x06003A7F RID: 14975 RVA: 0x0030B4B4 File Offset: 0x003096B4
	public static string RandomStarNameWithConstellationNumber(int seed)
	{
		DotNet35Random dotNet35Random = new DotNet35Random(seed);
		int num = dotNet35Random.Next();
		int num2 = dotNet35Random.Next(27, 75);
		num %= NameGen.constellations.Length;
		return num2.ToString() + " " + NameGen.constellations[num];
	}

	// Token: 0x06003A80 RID: 14976 RVA: 0x0030B4FC File Offset: 0x003096FC
	public static string RandomGiantStarNameFromRawNames(int seed)
	{
		int num = new DotNet35Random(seed).Next();
		num %= NameGen.raw_giant_names.Length;
		return NameGen.raw_giant_names[num];
	}

	// Token: 0x06003A81 RID: 14977 RVA: 0x0030B528 File Offset: 0x00309728
	public static string RandomGiantStarNameWithConstellationAlpha(int seed)
	{
		DotNet35Random dotNet35Random = new DotNet35Random(seed);
		int num = dotNet35Random.Next();
		int num2 = dotNet35Random.Next(15, 26);
		int num3 = dotNet35Random.Next(0, 26);
		num %= NameGen.constellations.Length;
		int num4 = (int)((ushort)(65 + num2));
		char c = (char)(65 + num3);
		return num4 + (int)c + " " + NameGen.constellations[num];
	}

	// Token: 0x06003A82 RID: 14978 RVA: 0x0030B584 File Offset: 0x00309784
	public static string RandomGiantStarNameWithFormat(int seed)
	{
		DotNet35Random dotNet35Random = new DotNet35Random(seed);
		int num = dotNet35Random.Next();
		int num2 = dotNet35Random.Next(10000);
		int num3 = dotNet35Random.Next(100);
		num %= NameGen.giant_name_formats.Length;
		return string.Format(NameGen.giant_name_formats[num], num2, num3);
	}

	// Token: 0x06003A83 RID: 14979 RVA: 0x0030B5D4 File Offset: 0x003097D4
	public static string RandomNeutronStarNameWithFormat(int seed)
	{
		DotNet35Random dotNet35Random = new DotNet35Random(seed);
		int num = dotNet35Random.Next();
		int num2 = dotNet35Random.Next(24);
		int num3 = dotNet35Random.Next(60);
		int num4 = dotNet35Random.Next(0, 60);
		num %= NameGen.neutron_star_name_formats.Length;
		return string.Format(NameGen.neutron_star_name_formats[num], num2, num3, num4);
	}

	// Token: 0x06003A84 RID: 14980 RVA: 0x0030B634 File Offset: 0x00309834
	public static string RandomBlackHoleNameWithFormat(int seed)
	{
		DotNet35Random dotNet35Random = new DotNet35Random(seed);
		int num = dotNet35Random.Next();
		int num2 = dotNet35Random.Next(24);
		int num3 = dotNet35Random.Next(60);
		int num4 = dotNet35Random.Next(0, 60);
		num %= NameGen.black_hole_name_formats.Length;
		return string.Format(NameGen.black_hole_name_formats[num], num2, num3, num4);
	}

	// Token: 0x04004BC7 RID: 19399
	public static string[] con0 = new string[]
	{
		"p",
		"t",
		"c",
		"k",
		"b",
		"d",
		"g",
		"f",
		"ph",
		"s",
		"sh",
		"th",
		"h",
		"v",
		"z",
		"th",
		"r",
		"ch",
		"tr",
		"dr",
		"m",
		"n",
		"l",
		"y",
		"w",
		"sp",
		"st",
		"sk",
		"sc",
		"sl",
		"pl",
		"cl",
		"bl",
		"gl",
		"fr",
		"fl",
		"pr",
		"br",
		"cr"
	};

	// Token: 0x04004BC8 RID: 19400
	public static string[] con1 = new string[]
	{
		"thr",
		"ex",
		"ec",
		"el",
		"er",
		"ev",
		"il",
		"is",
		"it",
		"ir",
		"up",
		"ut",
		"ur",
		"un",
		"gt",
		"phr"
	};

	// Token: 0x04004BC9 RID: 19401
	public static string[] vow0 = new string[]
	{
		"a",
		"an",
		"am",
		"al",
		"o",
		"u",
		"xe"
	};

	// Token: 0x04004BCA RID: 19402
	public static string[] vow1 = new string[]
	{
		"ea",
		"ee",
		"ie",
		"i",
		"e",
		"a",
		"er",
		"a",
		"u",
		"oo",
		"u",
		"or",
		"o",
		"oa",
		"ar",
		"a",
		"ei",
		"ai",
		"i",
		"au",
		"ou",
		"ao",
		"ir"
	};

	// Token: 0x04004BCB RID: 19403
	public static string[] vow2 = new string[]
	{
		"y",
		"oi",
		"io",
		"iur",
		"ur",
		"ac",
		"ic"
	};

	// Token: 0x04004BCC RID: 19404
	public static string[] ending = new string[]
	{
		"er",
		"n",
		"un",
		"or",
		"ar",
		"o",
		"o",
		"ans",
		"us",
		"ix",
		"us",
		"iurs",
		"a",
		"eo",
		"urn",
		"es",
		"eon",
		"y"
	};

	// Token: 0x04004BCD RID: 19405
	public static string[] roman = new string[]
	{
		"",
		"I",
		"II",
		"III",
		"IV",
		"V",
		"VI",
		"VII",
		"VIII",
		"IX",
		"X",
		"XI",
		"XII",
		"XIII",
		"XIV",
		"XV",
		"XVI",
		"XVII",
		"XVIII",
		"XIX",
		"XX"
	};

	// Token: 0x04004BCE RID: 19406
	public static string[] constellations = new string[]
	{
		"Andromedae",
		"Antliae",
		"Apodis",
		"Aquarii",
		"Aquilae",
		"Arae",
		"Arietis",
		"Aurigae",
		"Bootis",
		"Caeli",
		"Camelopardalis",
		"Cancri",
		"Canum Venaticorum",
		"Canis Majoris",
		"Canis Minoris",
		"Capricorni",
		"Carinae",
		"Cassiopeiae",
		"Centauri",
		"Cephei",
		"Ceti",
		"Chamaeleontis",
		"Circini",
		"Columbae",
		"Comae Berenices",
		"Coronae Australis",
		"Coronae Borealis",
		"Corvi",
		"Crateris",
		"Crucis",
		"Cygni",
		"Delphini",
		"Doradus",
		"Draconis",
		"Equulei",
		"Eridani",
		"Fornacis",
		"Geminorum",
		"Gruis",
		"Herculis",
		"Horologii",
		"Hydrae",
		"Hydri",
		"Indi",
		"Lacertae",
		"Leonis",
		"Leonis Minoris",
		"Leporis",
		"Librae",
		"Lupi",
		"Lyncis",
		"Lyrae",
		"Mensae",
		"Microscopii",
		"Monocerotis",
		"Muscae",
		"Normae",
		"Octantis",
		"Ophiuchii",
		"Orionis",
		"Pavonis",
		"Pegasi",
		"Persei",
		"Phoenicis",
		"Pictoris",
		"Piscium",
		"Piscis Austrini",
		"Puppis",
		"Pyxidis",
		"Reticuli",
		"Sagittae",
		"Sagittarii",
		"Scorpii",
		"Sculptoris",
		"Scuti",
		"Serpentis",
		"Sextantis",
		"Tauri",
		"Telescopii",
		"Trianguli",
		"Trianguli Australis",
		"Tucanae",
		"Ursae Majoris",
		"Ursae Minoris",
		"Velorum",
		"Virginis",
		"Volantis",
		"Vulpeculae"
	};

	// Token: 0x04004BCF RID: 19407
	public static string[] alphabeta = new string[]
	{
		"Alpha",
		"Beta",
		"Gamma",
		"Delta",
		"Epsilon",
		"Zeta",
		"Eta",
		"Theta",
		"Iota",
		"Kappa",
		"Lambda"
	};

	// Token: 0x04004BD0 RID: 19408
	public static string[] alphabeta_letter = new string[]
	{
		"α",
		"β",
		"γ",
		"δ",
		"ε",
		"ζ",
		"η",
		"θ",
		"ι",
		"κ",
		"λ"
	};

	// Token: 0x04004BD1 RID: 19409
	public static string[] raw_star_names = new string[]
	{
		"Acamar",
		"Achernar",
		"Achird",
		"Acrab",
		"Acrux",
		"Acubens",
		"Adhafera",
		"Adhara",
		"Adhil",
		"Agena",
		"Aladfar",
		"Albaldah",
		"Albali",
		"Albireo",
		"Alchiba",
		"Alcor",
		"Alcyone",
		"Alderamin",
		"Aldhibain",
		"Aldib",
		"Alfecca",
		"Alfirk",
		"Algedi",
		"Algenib",
		"Algenubi",
		"Algieba",
		"Algjebbath",
		"Algol",
		"Algomeyla",
		"Algorab",
		"Alhajoth",
		"Alhena",
		"Alifa",
		"Alioth",
		"Alkaid",
		"Alkalurops",
		"Alkaphrah",
		"Alkes",
		"Alkhiba",
		"Almach",
		"Almeisan",
		"Almuredin",
		"AlNa'ir",
		"Alnasl",
		"Alnilam",
		"Alnitak",
		"Alniyat",
		"Alphard",
		"Alphecca",
		"Alpheratz",
		"Alrakis",
		"Alrami",
		"Alrescha",
		"AlRijil",
		"Alsahm",
		"Alsciaukat",
		"Alshain",
		"Alshat",
		"Alshemali",
		"Alsuhail",
		"Altair",
		"Altais",
		"Alterf",
		"Althalimain",
		"AlTinnin",
		"Aludra",
		"AlulaAustralis",
		"AlulaBorealis",
		"Alwaid",
		"Alwazn",
		"Alya",
		"Alzirr",
		"AmazonStar",
		"Ancha",
		"Anchat",
		"AngelStern",
		"Angetenar",
		"Ankaa",
		"Anser",
		"Antecanis",
		"Apollo",
		"Arich",
		"Arided",
		"Arietis",
		"Arkab",
		"ArkebPrior",
		"Arneb",
		"Arrioph",
		"AsadAustralis",
		"Ascella",
		"Aschere",
		"AsellusAustralis",
		"AsellusBorealis",
		"AsellusPrimus",
		"Ashtaroth",
		"Asmidiske",
		"Aspidiske",
		"Asterion",
		"Asterope",
		"Asuia",
		"Athafiyy",
		"Atik",
		"Atlas",
		"Atria",
		"Auva",
		"Avior",
		"Azelfafage",
		"Azha",
		"Azimech",
		"BatenKaitos",
		"Becrux",
		"Beid",
		"Bellatrix",
		"Benatnasch",
		"Biham",
		"Botein",
		"Brachium",
		"Bunda",
		"Cajam",
		"Calbalakrab",
		"Calx",
		"Canicula",
		"Capella",
		"Caph",
		"Castor",
		"Castula",
		"Cebalrai",
		"Ceginus",
		"Celaeno",
		"Chara",
		"Chertan",
		"Choo",
		"Clava",
		"CorCaroli",
		"CorHydrae",
		"CorLeonis",
		"Cornu",
		"CorScorpii",
		"CorSepentis",
		"CorTauri",
		"Coxa",
		"Cursa",
		"Cymbae",
		"Cynosaura",
		"Dabih",
		"DenebAlgedi",
		"DenebDulfim",
		"DenebelOkab",
		"DenebKaitos",
		"DenebOkab",
		"Denebola",
		"Dhalim",
		"Dhur",
		"Diadem",
		"Difda",
		"DifdaalAuwel",
		"Dnoces",
		"Dubhe",
		"Dziban",
		"Dzuba",
		"Edasich",
		"ElAcola",
		"Elacrab",
		"Electra",
		"Elgebar",
		"Elgomaisa",
		"ElKaprah",
		"ElKaridab",
		"Elkeid",
		"ElKhereb",
		"Elmathalleth",
		"Elnath",
		"ElPhekrah",
		"Eltanin",
		"Enif",
		"Erakis",
		"Errai",
		"FalxItalica",
		"Fidis",
		"Fomalhaut",
		"Fornacis",
		"FumAlSamakah",
		"Furud",
		"Gacrux",
		"Gallina",
		"GarnetStar",
		"Gemma",
		"Genam",
		"Giausar",
		"GiedePrime",
		"Giedi",
		"Gienah",
		"Gienar",
		"Gildun",
		"Girtab",
		"Gnosia",
		"Gomeisa",
		"Gorgona",
		"Graffias",
		"Hadar",
		"Hamal",
		"Haris",
		"Hasseleh",
		"Hastorang",
		"Hatysa",
		"Heka",
		"Hercules",
		"Heze",
		"Hoedus",
		"Homam",
		"HyadumPrimus",
		"Icalurus",
		"Iclarkrav",
		"Izar",
		"Jabbah",
		"Jewel",
		"Jugum",
		"Juza",
		"Kabeleced",
		"Kaff",
		"Kaffa",
		"Kaffaljidma",
		"Kaitain",
		"KalbalAkrab",
		"Kat",
		"KausAustralis",
		"KausBorealis",
		"KausMedia",
		"Keid",
		"KeKouan",
		"Kelb",
		"Kerb",
		"Kerbel",
		"KiffaBoraelis",
		"Kitalpha",
		"Kochab",
		"Kornephoros",
		"Kraz",
		"Ksora",
		"Kuma",
		"Kurhah",
		"Kursa",
		"Lesath",
		"Maasym",
		"Maaz",
		"Mabsuthat",
		"Maia",
		"Marfik",
		"Markab",
		"Marrha",
		"Matar",
		"Mebsuta",
		"Megres",
		"Meissa",
		"Mekbuda",
		"Menkalinan",
		"Menkar",
		"Menkent",
		"Menkib",
		"Merak",
		"Meres",
		"Merga",
		"Meridiana",
		"Merope",
		"Mesartim",
		"Metallah",
		"Miaplacidus",
		"Mimosa",
		"Minelauva",
		"Minkar",
		"Mintaka",
		"Mirac",
		"Mirach",
		"Miram",
		"Mirfak",
		"Mirzam",
		"Misam",
		"Mismar",
		"Mizar",
		"Muhlifain",
		"Muliphein",
		"Muphrid",
		"Muscida",
		"NairalSaif",
		"NairalZaurak",
		"Naos",
		"Nash",
		"Nashira",
		"Navi",
		"Nekkar",
		"Nicolaus",
		"Nihal",
		"Nodus",
		"Nunki",
		"Nusakan",
		"OculusBoreus",
		"Okda",
		"Osiris",
		"OsPegasi",
		"Palilicium",
		"Peacock",
		"Phact",
		"Phecda",
		"Pherkad",
		"PherkadMinor",
		"Pherkard",
		"Phoenice",
		"Phurad",
		"Pishpai",
		"Pleione",
		"Polaris",
		"Pollux",
		"Porrima",
		"Postvarta",
		"Praecipua",
		"Procyon",
		"Propus",
		"Protrygetor",
		"Pulcherrima",
		"Rana",
		"RanaSecunda",
		"Rasalas",
		"Rasalgethi",
		"Rasalhague",
		"Rasalmothallah",
		"RasHammel",
		"Rastaban",
		"Reda",
		"Regor",
		"Regulus",
		"Rescha",
		"RigilKentaurus",
		"RiglalAwwa",
		"Rotanen",
		"Ruchba",
		"Ruchbah",
		"Rukbat",
		"Rutilicus",
		"Saak",
		"Sabik",
		"Sadachbia",
		"Sadalbari",
		"Sadalmelik",
		"Sadalsuud",
		"Sadatoni",
		"Sadira",
		"Sadr",
		"Saidak",
		"Saiph",
		"Salm",
		"Sargas",
		"Sarin",
		"Sartan",
		"Sceptrum",
		"Scheat",
		"Schedar",
		"Scheddi",
		"Schemali",
		"Scutulum",
		"SeatAlpheras",
		"Segin",
		"Seginus",
		"Shaula",
		"Shedir",
		"Sheliak",
		"Sheratan",
		"Singer",
		"Sirius",
		"Sirrah",
		"Situla",
		"Skat",
		"Spica",
		"Sterope",
		"Subra",
		"Suha",
		"Suhail",
		"SuhailHadar",
		"SuhailRadar",
		"Suhel",
		"Sulafat",
		"Superba",
		"Svalocin",
		"Syrma",
		"Tabit",
		"Tais",
		"Talitha",
		"TaniaAustralis",
		"TaniaBorealis",
		"Tarazed",
		"Tarf",
		"TaTsun",
		"Taygeta",
		"Tegmen",
		"Tejat",
		"TejatPrior",
		"Terebellum",
		"Theemim",
		"Thuban",
		"Tolimann",
		"Tramontana",
		"Tsih",
		"Tureis",
		"Unukalhai",
		"Vega",
		"Venabulum",
		"Venator",
		"Vendemiatrix",
		"Vespertilio",
		"Vildiur",
		"Vindemiatrix",
		"Wasat",
		"Wazn",
		"YedPosterior",
		"YedPrior",
		"Zaniah",
		"Zaurak",
		"Zavijava",
		"ZenithStar",
		"Zibel",
		"Zosma",
		"Zubenelakrab",
		"ZubenElgenubi",
		"Zubeneschamali",
		"ZubenHakrabi",
		"Zubra"
	};

	// Token: 0x04004BD2 RID: 19410
	public static string[] raw_giant_names = new string[]
	{
		"AH Scorpii",
		"Aldebaran",
		"Alpha Herculis",
		"Antares",
		"Arcturus",
		"AV Persei",
		"BC Cygni",
		"Betelgeuse",
		"BI Cygni",
		"BO Carinae",
		"Canopus",
		"CE Tauri",
		"CK Carinae",
		"CW Leonis",
		"Deneb",
		"Epsilon Aurigae",
		"Eta Carinae",
		"EV Carinae",
		"IX Carinae",
		"KW Sagittarii",
		"KY Cygni",
		"Mira",
		"Mu Cephei",
		"NML Cygni",
		"NR Vulpeculae",
		"PZ Cassiopeiae",
		"R Doradus",
		"R Leporis",
		"Rho Cassiopeiae",
		"Rigel",
		"RS Persei",
		"RT Carinae",
		"RU Virginis",
		"RW Cephei",
		"S Cassiopeiae",
		"S Cephei",
		"S Doradus",
		"S Persei",
		"SU Persei",
		"TV Geminorum",
		"U Lacertae",
		"UY Scuti",
		"V1185 Scorpii",
		"V354 Cephei",
		"V355 Cepheus",
		"V382 Carinae",
		"V396 Centauri",
		"V437 Scuti",
		"V509 Cassiopeiae",
		"V528 Carinae",
		"V602 Carinae",
		"V648 Cassiopeiae",
		"V669 Cassiopeiae",
		"V838 Monocerotis",
		"V915 Scorpii",
		"VV Cephei",
		"VX Sagittarii",
		"VY Canis Majoris",
		"WOH G64",
		"XX Persei"
	};

	// Token: 0x04004BD3 RID: 19411
	public static string[] giant_name_formats = new string[]
	{
		"HD {0:0000}{1:00}",
		"HDE {0:0000}{1:00}",
		"HR {0:0000}",
		"HV {0:0000}",
		"LBV {0:0000}-{1:00}",
		"NSV {0:0000}",
		"YSC {0:0000}-{1:00}"
	};

	// Token: 0x04004BD4 RID: 19412
	public static string[] neutron_star_name_formats = new string[]
	{
		"NTR J{0:00}{1:00}+{2:00}",
		"NTR J{0:00}{1:00}-{2:00}"
	};

	// Token: 0x04004BD5 RID: 19413
	public static string[] black_hole_name_formats = new string[]
	{
		"DSR J{0:00}{1:00}+{2:00}",
		"DSR J{0:00}{1:00}-{2:00}"
	};
}
