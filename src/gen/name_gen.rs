use crate::data_struct::enums::EStarType;
use crate::data_struct::galaxy_data::GalaxyData;
use crate::data_struct::star_data::StarData;

use dotnet35_rand_rs::DotNet35Random;

/// 给定种子，直接返回一个随机名称
pub fn random_name(seed: i32) -> String {
    let mut rng = DotNet35Random::new(seed);
    let num = (rng.next_double() * 1.8 + 2.3) as usize;
    let mut name = String::new();
    for i in 0..num {
        if rng.next_double() < 0.05 && i == 0 {
            name.push_str(VOW0[rng.next() as usize % VOW0.len()]);
        } else {
            if rng.next_double() < 0.97 || num >= 4 {
                name.push_str(CON0[rng.next() as usize % CON0.len()]);
            } else {
                name.push_str(CON1[rng.next() as usize % CON1.len()]);
            }
            if i == num - 1 && rng.next_double() < 0.9 {
                name.push_str(ENDING[rng.next() as usize % ENDING.len()]);
            } else if rng.next_double() < 0.97 {
                name.push_str(VOW1[rng.next() as usize % VOW1.len()]);
            } else {
                name.push_str(VOW2[rng.next() as usize % VOW2.len()]);
            }
        }
    }
    name = name.replace("uu", "u");
    name = name.replace("ooo", "oo");
    name = name.replace("eee", "ee");
    name = name.replace("eea", "ea");
    name = name.replace("aa", "a");
    name = name.replace("yy", "y");
    let mut chars = name.chars();
    match chars.next() {
        None => String::new(),
        Some(f) => f.to_uppercase().collect::<String>() + chars.as_str(),
    }
}

/// 给定种子，返回一个随机恒星名称
#[allow(unused)]
pub fn random_star_name(seed: i32, star_data: &StarData, galaxy: &GalaxyData) -> String {
    let mut rng = DotNet35Random::new(seed);
    for _ in 0..256 {
        let name = _random_star_name(rng.next(), star_data);
        if !galaxy.stars.iter().any(|star: &StarData| star.name == name) {
            return name;
        }
    }
    "XStar".to_string()
}

/// 给定种子，返回一个随机恒星名称
/// (内部函数)
pub fn _random_star_name(seed: i32, star_data: &StarData) -> String {
    let mut rng = DotNet35Random::new(seed);
    let seed2 = rng.next();
    let num = rng.next_double();
    let num2 = rng.next_double();
    match star_data.star_type {
        EStarType::GiantStar => {
            if num2 < 0.4000000059604645 {
                random_giant_star_name_from_raw_names(seed2)
            } else if num2 < 0.699999988079071 {
                random_giant_star_name_with_constellation_alpha(seed2)
            } else {
                random_giant_star_name_with_format(seed2)
            }
        }
        EStarType::NeutronStar => random_neutron_star_name_with_format(seed2),
        EStarType::BlackHole => random_black_hole_name_with_format(seed2),
        _ => {
            if num < 0.6000000238418579 {
                random_star_name_from_raw_names(seed2)
            } else if num < 0.9300000071525574 {
                random_star_name_with_constellation_alpha(seed2)
            } else {
                random_star_name_with_constellation_number(seed2)
            }
        }
    }
}

pub fn random_star_name_from_raw_names(seed: i32) -> String {
    let mut rng = DotNet35Random::new(seed);
    let index = rng.next() as usize % RAW_STAR_NAMES.len();
    RAW_STAR_NAMES[index].to_string()
}

pub fn random_star_name_with_constellation_alpha(seed: i32) -> String {
    let mut rng = DotNet35Random::new(seed);
    let index1 = rng.next() as usize % CONSTELLATIONS.len();
    let index2 = rng.next() as usize % ALPHABETA.len();
    let text = CONSTELLATIONS[index1];
    if text.len() > 10 {
        format!("{} {}", ALPHABETA_LETTER[index2], text)
    } else {
        format!("{} {}", ALPHABETA[index2], text)
    }
}

pub fn random_star_name_with_constellation_number(seed: i32) -> String {
    let mut rng = DotNet35Random::new(seed);
    let index = rng.next() as usize % CONSTELLATIONS.len();
    let num = rng.next_in_range(27, 75);
    format!("{} {}", num, CONSTELLATIONS[index])
}

pub fn random_giant_star_name_from_raw_names(seed: i32) -> String {
    let mut rng = DotNet35Random::new(seed);
    let index = rng.next() as usize % RAW_GIANT_NAMES.len();
    RAW_GIANT_NAMES[index].to_string()
}

pub fn random_giant_star_name_with_constellation_alpha(seed: i32) -> String {
    let mut rng = DotNet35Random::new(seed);
    let index = rng.next() as usize % CONSTELLATIONS.len();
    let num2 = rng.next_in_range(15, 26);
    let num3 = rng.next_with_max(26);
    let num4 = (65 + num2) as u8 as char;
    let c = (65 + num3) as u8 as char;
    format!("{}{} {}", num4, c, CONSTELLATIONS[index])
}

pub static GIANT_NAME_FORMATS: [&str; 7] = [
    "HD {:04}{:02}",
    "HDE {:04}{:02}",
    "HR {:04}",
    "HV {:04}",
    "LBV {:04}-{:02}",
    "NSV {:04}",
    "YSC {:04}-{:02}",
];

pub fn random_giant_star_name_with_format(seed: i32) -> String {
    let mut rng = DotNet35Random::new(seed);
    let index = rng.next_with_max(GIANT_NAME_FORMATS.len() as i32);
    let num2 = rng.next_with_max(10000);
    let num3 = rng.next_with_max(100);
    // format!(GIANT_NAME_FORMATS[index], num2, num3)
    match index {
        0 => format!("HD {:04}{:02}", num2, num3),
        1 => format!("HDE {:04}{:02}", num2, num3),
        2 => format!("HR {:04}", num2),
        3 => format!("HV {:04}", num2),
        4 => format!("LBV {:04}-{:02}", num2, num3),
        5 => format!("NSV {:04}", num2),
        6 => format!("YSC {:04}-{:02}", num2, num3),
        _ => format!("HD {:04}{:02}", num2, num3), // unreachable
    }
}

pub static NEUTRON_STAR_NAME_FORMATS: [&str; 2] = ["NTR J{:02}{:02}+{:02}", "NTR J{:02}{:02}-{:02}"];

pub fn random_neutron_star_name_with_format(seed: i32) -> String {
    let mut rng = DotNet35Random::new(seed);
    let index = rng.next_with_max(NEUTRON_STAR_NAME_FORMATS.len() as i32);
    let num2 = rng.next_with_max(24);
    let num3 = rng.next_with_max(60);
    let num4 = rng.next_with_max(60);
    // format!(format_template, num2, num3, num4)
    format!("NTR J{:02}{:02}{}{:02}", num2, num3, if index == 0 { '+' } else { '-' }, num4)
}

pub static BLACK_HOLE_NAME_FORMATS: [&str; 2] = ["DSR J{:02}{:02}+{:02}", "DSR J{:02}{:02}-{:02}"];
pub fn random_black_hole_name_with_format(seed: i32) -> String {
    let mut rng = DotNet35Random::new(seed);
    let index = rng.next_with_max(BLACK_HOLE_NAME_FORMATS.len() as i32);
    let num2 = rng.next_with_max(24);
    let num3 = rng.next_with_max(60);
    let num4 = rng.next_with_max(60);
    // format!(black_hole_name_formats!(index), num2, num3, num4)
    format!("DSR J{:02}{:02}{}{:02}", num2, num3, if index == 0 { '+' } else { '-' }, num4)
}

pub static CON0: [&str; 39] = [
    "p", "t", "c", "k", "b", "d", "g", "f", "ph", "s", "sh", "th", "h", "v", "z", "th", "r", "ch", "tr", "dr", "m",
    "n", "l", "y", "w", "sp", "st", "sk", "sc", "sl", "pl", "cl", "bl", "gl", "fr", "fl", "pr", "br", "cr",
];

pub static CON1: [&str; 16] =
    ["thr", "ex", "ec", "el", "er", "ev", "il", "is", "it", "ir", "up", "ut", "ur", "un", "gt", "phr"];

pub static VOW0: [&str; 7] = ["a", "an", "am", "al", "o", "u", "xe"];

pub static VOW1: [&str; 23] = [
    "ea", "ee", "ie", "i", "e", "a", "er", "a", "u", "oo", "u", "or", "o", "oa", "ar", "a", "ei", "ai", "i", "au",
    "ou", "ao", "ir",
];

pub static VOW2: [&str; 7] = ["y", "oi", "io", "iur", "ur", "ac", "ic"];

pub static ENDING: [&str; 18] = [
    "er", "n", "un", "or", "ar", "o", "o", "ans", "us", "ix", "us", "iurs", "a", "eo", "urn", "es", "eon", "y",
];

#[allow(unused)]
pub static ROMAN: [&str; 21] = [
    "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV", "XVI", "XVII",
    "XVIII", "XIX", "XX",
];

pub static CONSTELLATIONS: [&str; 88] = [
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
    "Vulpeculae",
];

pub static ALPHABETA: [&str; 11] =
    ["Alpha", "Beta", "Gamma", "Delta", "Epsilon", "Zeta", "Eta", "Theta", "Iota", "Kappa", "Lambda"];

pub static ALPHABETA_LETTER: [&str; 11] = ["α", "β", "γ", "δ", "ε", "ζ", "η", "θ", "ι", "κ", "λ"];

pub static RAW_STAR_NAMES: [&str; 425] = [
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
    "Zubra",
];

pub static RAW_GIANT_NAMES: [&str; 60] = [
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
    "XX Persei",
];
