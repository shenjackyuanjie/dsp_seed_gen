// #[allow(unused)]
mod data_struct;
// #[allow(unused)]
mod gen;

fn main() {
    println!("Hello, world!");
    // if 'name' in sys.argv:
    if std::env::args().any(|x| x == "name") {
        rand_name();
    }
}

fn rand_name() {
    use crate::gen::name_gen;
    let now_time: i32 = std::time::SystemTime::now().duration_since(std::time::UNIX_EPOCH).unwrap().as_secs() as i32;
    let rand_name = name_gen::random_name(now_time);
    println!("you get a name: {}", rand_name);
    println!(
        "and a star: {}! ",
        name_gen::random_star_name_with_constellation_alpha(now_time)
    );
    println!(
        "and a star: {}! ",
        name_gen::random_star_name_with_constellation_number(now_time)
    );
    println!("and a star: {}! ", name_gen::random_star_name_from_raw_names(now_time));
    println!(
        "and a netron star: {}! ",
        name_gen::random_neutron_star_name_with_format(now_time)
    );
    println!("and a black hole: {}! ", name_gen::random_black_hole_name_with_format(now_time));
    println!("and a giant: {}! ", name_gen::random_giant_star_name_from_raw_names(now_time));
    println!(
        "and a giant: {}! ",
        name_gen::random_giant_star_name_with_constellation_alpha(now_time)
    );
    println!("and a giant: {}! ", name_gen::random_giant_star_name_with_format(now_time));
}
