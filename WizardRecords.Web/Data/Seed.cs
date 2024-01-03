using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WizardRecords.Api.Domain.Entities;
using static WizardRecords.Api.Data.Constants;

namespace WizardRecords.Api.Data {
    public static class Seed {
        public static readonly PasswordHasher<User> PASSWORD_HASHER = new();
        private static readonly List<string> Roles = new List<string> {
            "Administrator",
            "Manager",
            "Client",
            "Guest"
        };

        public static void LoadSeed(this ModelBuilder builder) {
            foreach (var role in Roles) {
                AddRole(builder, role);
            }

            var adminUser = AddUser(builder,
                "Ti-Coq",
                "Tremblay",
                "Admin123",
                "Admin123@wizardrecords.com",
                "555-555-5555",
                666,
                "Rue Delarue",
                "Sainte-Grosse-Roche-De-L'Achigan",
                Province.QC,
                "J0K3H0",
                "Canada",
                "Admin123!"
            );

            AddUserToRole(builder, adminUser, "Administrator");
            SeedAll(builder);
        }

        private static void AddRole(ModelBuilder builder, string roleName) {
            var role = builder.Model.FindEntityType(typeof(IdentityRole<Guid>))
                .GetSeedData().FirstOrDefault(sd => sd.Values.Contains(roleName.ToUpper()));

            if (role == null) {
                builder.Entity<IdentityRole<Guid>>().HasData(new IdentityRole<Guid> {
                    Id = Guid.NewGuid(),
                    Name = roleName,
                    NormalizedName = roleName.ToUpper()
                });
            }
        }

        private static User AddUser(
                ModelBuilder builder,
                string firstName,
                string lastName,
                string username,
                string email,
                string phone,
                int addressNum,
                string streetName,
                string city,
                Province province,
                string postalCode,
                string country,
                string password
            ) {
            var newUser = new User(username) {
                Id = Guid.NewGuid(),
                UserName = username,
                NormalizedUserName = username.ToUpper(),
                Email = email,
                NormalizedEmail = email.ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phone,
                AddressNum = addressNum,
                StreetName = streetName,
                City = city,
                Province = province,
                PostalCode = postalCode,
                Country = country
            };

            newUser.PasswordHash = PASSWORD_HASHER.HashPassword(newUser, password);
            builder.Entity<User>().HasData(newUser);

            return newUser;
        }

        private static void AddUserToRole(ModelBuilder builder, User user, string roleName) {
            var role = builder.Model.FindEntityType(typeof(IdentityRole<Guid>))
                .GetSeedData().FirstOrDefault(sd => sd.Values.Contains(roleName.ToUpper()));

            if (role != null) {
                builder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid> {
                    UserId = user.Id,
                    RoleId = (Guid)role["Id"]
                });
            }
        }

        private static void SeedAll(this ModelBuilder builder) {
            var albums = new List<Album> {
                new Album(Guid.NewGuid(), "Essenger", "After Dark", ArtistGenre.ELECTRONICA, AlbumGenre.SYNTHWAVE, "FiXT Neon", 24.99f, true, Media.CD, 2, "essenger_after_dark.webp", Grade.M, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Aversions Crown", "Tyrant", ArtistGenre.METAL, AlbumGenre.DEATHCORE, "Nuclear Blast", 19.99f, true, Media.CD, 3, "aversions_crown_tyrant.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Make Them Suffer", "How To Survive A Funeral", ArtistGenre.METAL, AlbumGenre.METALCORE, "Rise Records", 29.99f, false, Media.VINYL, 2, "make_them_suffer_how_to_survive_a_funeral.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Gojira", "L'Enfant Sauvage", ArtistGenre.METAL, AlbumGenre.DEATHMETAL, "Roadrunner Records", 39.99f, true, Media.VINYL, 1, "gojira_l_enfant_sauvage.webp", Grade.VG, Grade.M, "-", "-", ""),
                new Album(Guid.NewGuid(), "Nirvana", "Bleach", ArtistGenre.ALTERNATIVE, AlbumGenre.GRUNGE, "Sub Pop", 29.99f, false, Media.CD, 3, "nirvana_bleach.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Nirvana", "Nevermind", ArtistGenre.ALTERNATIVE, AlbumGenre.GRUNGE, "Sub Pop", 29.99f, true, Media.VINYL, 4, "nirvana_nevermind.webp", Grade.VG, Grade.VG_PLUS, "-", "-", ""),
                new Album(Guid.NewGuid(), "Nirvana", "In Utero", ArtistGenre.ALTERNATIVE, AlbumGenre.GRUNGE, "Sub Pop", 19.99f, false, Media.CD, 2, "nirvana_in_utero.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Nirvana", "MTV Unplugged", ArtistGenre.ALTERNATIVE, AlbumGenre.GRUNGE, "Geffen Records", 29.99f, true, Media.VINYL, 2, "nirvana_mtv_unplugged_in_new_york.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Norma Jean", "All Hail", ArtistGenre.METAL, AlbumGenre.METALCORE, "Solid State Records", 24.99f, false, Media.CD, 1, "norma_jean_all_hail.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Red Hot Chili Peppers", "Californication", ArtistGenre.FUNK, AlbumGenre.ROCK, "Warner Bros. Records", 29.99f, false, Media.CD, 3, "red_hot_chili_peppers_californication.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Metallica", "Master Of Puppets", ArtistGenre.METAL, AlbumGenre.ROCK, "Elektra Records", 79.99f, true, Media.VINYL, 4, "metallica_master_of_puppets.webp", Grade.M, Grade.VG_PLUS, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Killers", "Hot Fuss", ArtistGenre.ROCK, AlbumGenre.POP, "Island Records", 24.99f, false, Media.CD, 3, "killers_hot_fuss.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Emarosa", "131", ArtistGenre.ROCK, AlbumGenre.HARDCORE, "Hopeless Records", 29.99f, true, Media.CD, 1, "emarosa_131.webp", Grade.M, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Bad Omens", "Finding God Before God Finds Me", ArtistGenre.METAL, AlbumGenre.METALCORE, "Sumerian Records", 19.99f, false, Media.VINYL, 1, "bad_omens_finding_god_before_god_finds_me.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Bring Me The Horizon", "Sempiternal", ArtistGenre.METAL, AlbumGenre.METALCORE, "Epitaph Records", 34.99f, false, Media.VINYL, 1, "bring_me_the_horizon_sempiternal.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Fushitsusha", "1st", ArtistGenre.ROCK, AlbumGenre.PSYCH, "P.S.F. Records", 99.99f, true, Media.CD, 1, "fushitsusha_1st.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Fushitsusha", "2nd", ArtistGenre.ROCK, AlbumGenre.PSYCH, "P.S.F. Records", 119.99f, true, Media.CD, 1, "fushitsusha_2nd.webp", Grade.M, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Three Days Grace", "One-X", ArtistGenre.ALTERNATIVE, AlbumGenre.ALTERNATIVE, "Jive", 19.99f, false, Media.VINYL, 1, "three_days_grace_one_x.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Fall Out Boy", "From Under The Cork Tree", ArtistGenre.PUNK, AlbumGenre.POP, "Fueled By Ramen", 24.99f, false, Media.VINYL, 1, "fall_out_boy_from_under_the_cork_tree.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "2 Live Crew", "As Nasty As They Wanna Be", ArtistGenre.HIPHOP, AlbumGenre.RAP, "Skywalker Records", 19.99f, true, Media.CD, 1, "2_live_crew_as_nasty_as_they_wanna_be.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Death Grips", "The Money Store", ArtistGenre.HIPHOP, AlbumGenre.EXPERIMENTAL, "Epic", 39.99f, false, Media.VINYL, 1, "death_grips_the_money_store.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Death Grips", "Ex-Military", ArtistGenre.HIPHOP, AlbumGenre.EXPERIMENTAL, "Third Worlds", 999.99f, true, Media.VINYL, 1, "death_grips_ex_military.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Kanye West", "The College Dropout", ArtistGenre.HIPHOP, AlbumGenre.RAP, "Roc-A-Fella Recordings", 29.99f, false, Media.CD, 1, "west_kanye_the_college_dropout.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Nick Drake", "Pink Moon", ArtistGenre.FOLK, AlbumGenre.POP, "Island Records", 29.99f, false, Media.VINYL, 1, "drake_nick_pink_moon.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Boards Of Canada", "Music Has The Right To Children", ArtistGenre.ELECTRONICA, AlbumGenre.IDM, "Warp Records", 34.99f, false, Media.VINYL, 1, "boards_of_canada_music_has_the_right_to_children.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Boards Of Canada", "Geogaddi", ArtistGenre.ELECTRONICA, AlbumGenre.IDM, "Warp Records", 39.99f, false, Media.VINYL, 1, "boards_of_canada_geogaddi.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Donna Summer", "I Remember Yesterday", ArtistGenre.POP, AlbumGenre.DISCO, "Casablanca", 4.99f, true, Media.CD, 1, "summer_donna_i_remember_yesterday.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Bee Gees", "Spirits Have Flown", ArtistGenre.POP, AlbumGenre.DISCO, "RSO", 2.99f, true, Media.VINYL, 1, "bee_gees_spirits_having_flown.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "ABBA", "Arrival", ArtistGenre.POP, AlbumGenre.ROCK, "Polydor", 19.99f, false, Media.CD, 1, "abba_arrival.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "ABBA", "ABBA", ArtistGenre.POP, AlbumGenre.ROCK, "Polar", 19.99f, true, Media.VINYL, 1, "abba_abba.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Michael Jackson", "Thriller", ArtistGenre.POP, AlbumGenre.ROCK, "Epic", 34.99f, false, Media.VINYL, 2, "jackson_michael_thriller.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Michael Jackson", "Bad", ArtistGenre.POP, AlbumGenre.ROCK, "Epic", 34.99f, false, Media.VINYL, 2, "jackson_michael_bad.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Gary Numan", "The Pleasure Principle", ArtistGenre.POP, AlbumGenre.NEWWAVE, "Beggars Banquet", 19.99f, true, Media.CD, 1, "numan_gary_the_pleasure_principle.webp", Grade.M, Grade.VG_PLUS, "-", "-", ""),
                new Album(Guid.NewGuid(), "Miles Davis", "Bitches Brew", ArtistGenre.JAZZ, AlbumGenre.FUSION, "Columbia", 39.99f, false, Media.VINYL, 1, "davis_miles_bitches_brew.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Miles Davis", "Filles De Kilimanjaro", ArtistGenre.JAZZ, AlbumGenre.FUSION, "Columbia", 39.99f, true, Media.VINYL, 1, "davis_miles_filles_de_kilimanjaro.webp", Grade.VG, Grade.VG_PLUS, "-", "-", ""),
                new Album(Guid.NewGuid(), "Miles Davis", "Miles In The Sky", ArtistGenre.JAZZ, AlbumGenre.FUSION, "Columbia", 39.99f, true, Media.VINYL, 1, "davis_miles_miles_in_the_sky.webp", Grade.VG_PLUS, Grade.M, "-", "-", ""),
                new Album(Guid.NewGuid(), "Miles Davis", "On The Corner", ArtistGenre.JAZZ, AlbumGenre.FUSION, "Columbia", 39.99f, false, Media.VINYL, 1, "davis_miles_on_the_corner.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Miles Davis", "Big Fun", ArtistGenre.JAZZ, AlbumGenre.FUSION, "Columbia", 39.99f, true, Media.VINYL, 1, "davis_miles_big_fun.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Miles Davis", "Black Beauty (Miles Davis At Fillmore West)", ArtistGenre.JAZZ, AlbumGenre.FUSION, "Columbia", 39.99f, false, Media.VINYL, 1, "davis_miles_black_beauty.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "John Coltrane", "A Love Supreme", ArtistGenre.JAZZ, AlbumGenre.POSTBOP, "Impulse!", 29.99f, true, Media.CD, 1, "coltrane_john_a_love_supreme.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "John Coltrane", "Ascension", ArtistGenre.JAZZ, AlbumGenre.FREEJAZZ, "Impulse!", 49.99f, true, Media.VINYL, 1, "coltrane_john_ascension_edition_i.webp", Grade.VG, Grade.M, "-", "-", ""),
                new Album(Guid.NewGuid(), "Peter Brötzmann", "Machine Gun", ArtistGenre.JAZZ, AlbumGenre.FREEJAZZ, "BRÖ", 1199.99f, true, Media.VINYL, 1, "brotzmann_peter_machine_gun.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Charles Mingus", "Mingus Mingus Mingus Mingus Mingus", ArtistGenre.JAZZ, AlbumGenre.POSTBOP, "Impulse!", 29.99f, false, Media.VINYL, 1, "mingus_charles_mingus_mingus_mingus_mingus_mingus.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Art Ensemble Of Chicago", "Les Stances À Sophie", ArtistGenre.JAZZ, AlbumGenre.FREEJAZZ, "Pathé", 349.99f, true, Media.VINYL, 1, "art_ensemble_of_chicago_les_stances_a_sophie.webp", Grade.VG, Grade.VG_PLUS, "-", "-", ""),
                new Album(Guid.NewGuid(), "Naked City", "Naked City", ArtistGenre.JAZZ, AlbumGenre.EXPERIMENTAL, "Nonesuch Records", 149.99f, true, Media.VINYL, 1, "naked_city_naked_city.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Beatles", "Abbey Road", ArtistGenre.ROCK, AlbumGenre.POP, "Apple Records", 29.99f, false, Media.VINYL, 2, "beatles_abbey_road.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Beatles", "Magical Mystery Tour", ArtistGenre.ROCK, AlbumGenre.PSYCH, "Apple Records", 199.99f, true, Media.VINYL, 1, "beatles_magical_mystery_tour.webp", Grade.VG, Grade.VG_PLUS, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Beatles", "Revolver", ArtistGenre.ROCK, AlbumGenre.PSYCH, "Apple Records", 29.99f, false, Media.CD, 1, "beatles_revolver.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Beatles", "Rubber Soul", ArtistGenre.ROCK, AlbumGenre.POP, "Apple Records", 29.99f, false, Media.CD, 2, "beatles_rubber_soul.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Beatles", "Sgt. Pepper's Lonely Hearts Club Band", ArtistGenre.ROCK, AlbumGenre.PSYCH, "Apple Records", 49.99f, false, Media.VINYL, 3, "beatles_sgt_peppers_lonely_hearts_club_band.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Beatles", "The Beatles (White Album)", ArtistGenre.ROCK, AlbumGenre.PSYCH, "Apple Records", 29.99f, false, Media.VINYL, 1, "beatles_white_album.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Soundtrack Of Our Lives", "Behind The Music", ArtistGenre.ROCK, AlbumGenre.INDIE, "Stickman Records", 29.99f, true, Media.VINYL, 1, "soundtrack_of_our_lives_behind_the_music.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "La Monte Young", "The Well-Tuned Piano 81 X 25 6:17:50 - 11:18:59 PM NYC", ArtistGenre.CLASSICAL, AlbumGenre.EXPERIMENTAL, "Gramavision", 1499.99f, true, Media.VINYL, 1, "young_lamonte_the_well_tuned_piano.webp", Grade.VG, Grade.M, "-", "-", ""),
                new Album(Guid.NewGuid(), "Philip Glass", "Koyaanisqatsi", ArtistGenre.SOUNDTRACK, AlbumGenre.MINIMALIST, "Orange Mountain Music", 199.99f, true, Media.VINYL, 1, "soundtrack_koyaanisqatsi.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Toby Fox", "Undertale", ArtistGenre.SOUNDTRACK, AlbumGenre.ELECTRONICA, "Iam8bit", 29.99f, true, Media.VINYL, 1, "soundtrack_undertale.webp", Grade.VG, Grade.M, "-", "-", ""),
                new Album(Guid.NewGuid(), "Various", "Après Ski", ArtistGenre.SOUNDTRACK, AlbumGenre.FUNK, "Trans-Canada International", 99.99f, true, Media.VINYL, 1, "soundtrack_apres_ski.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Leos Janacek", "The Glagolitic Mass", ArtistGenre.CLASSICAL, AlbumGenre.MODERN, "Supraphon", 29.99f, true, Media.VINYL, 1, "janacek_leos_glagolitic_mass.webp", Grade.VG, Grade.M, "-", "-", ""),
                new Album(Guid.NewGuid(), "Domenico Scarlatti", "Sonatas", ArtistGenre.CLASSICAL, AlbumGenre.BAROQUE, "EMI", 9.99f, true, Media.CD, 1, "scarlatti_domenico_sonatas_landowska.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Ludwig Van Beethoven", "Symphony No. 9", ArtistGenre.CLASSICAL, AlbumGenre.ROMANTIC, "Philips", 4.99f, true, Media.CD, 1, "beethoven_ludwig_von_symphony_no_9.webp", Grade.VG, Grade.M, "-", "-", ""),
                new Album(Guid.NewGuid(), "Johann Sebastian Bach", "The Well-Tempered Clavier", ArtistGenre.CLASSICAL, AlbumGenre.BAROQUE, "Columbia", 9.99f, true, Media.VINYL, 1, "bach_johann_sebastian_the_well_tempered_clavier.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Johann Sebastian Bach", "The Brandenburg Concertos", ArtistGenre.CLASSICAL, AlbumGenre.BAROQUE, "Westbound Records", 29.99f, true, Media.VINYL, 1, "bach_johann_sebastian_the_brandenburg_concertos_marriner.webp", Grade.VG, Grade.M, "-", "-", ""),
                new Album(Guid.NewGuid(), "Funkadelic", "Maggot Brain", ArtistGenre.FUNK, AlbumGenre.PSYCH, "Westbound Records", 79.99f, true, Media.VINYL, 1, "funkadelic_maggot_brain.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Budos Band", "The Budos Band III", ArtistGenre.FUNK, AlbumGenre.AFROBEAT, "Daptone Records", 29.99f, false, Media.VINYL, 1, "budos_band_iii.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Fela Kuti", "Expensive Shit", ArtistGenre.WORLD, AlbumGenre.AFROBEAT, "Kalakuta Sunrise", 39.99f, false, Media.VINYL, 1, "kuti_fela_expensive_shit.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Yma Sumac", "Mambo", ArtistGenre.WORLD, AlbumGenre.KITSCH, "Capitol Records", 19.99f, false, Media.CD, 1, "sumac_yma_mambo.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Yma Sumac", "Voice Of The Xtabay", ArtistGenre.WORLD, AlbumGenre.KITSCH, "Capitol Records", 19.99f, false, Media.VINYL, 1, "sumac_yma_voice_of_the_xtabay.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Bob Marley", "Legend - The Best Of Bob Marley", ArtistGenre.WORLD, AlbumGenre.REGGAE, "Island Records", 29.99f, false, Media.CD, 2, "marley_bob_legend_the_best_of.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Pye Corner Audio", "Black Mill Tapes Volumes 1 & 2", ArtistGenre.ELECTRONICA, AlbumGenre.IDM, "Type", 44.99f, true, Media.VINYL, 1, "pye_corner_audio_the_black_mill_tapes_volumes_1_2.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Autechre", "Amber", ArtistGenre.ELECTRONICA, AlbumGenre.IDM, "Warp Records", 39.99f, false, Media.VINYL, 1, "autechre_amber.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Autechre", "Oversteps", ArtistGenre.ELECTRONICA, AlbumGenre.IDM, "Warp Records", 89.99f, false, Media.VINYL, 1, "autechre_oversteps.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Aphex Twin", "Windowlicker", ArtistGenre.ELECTRONICA, AlbumGenre.IDM, "Warp Records", 24.99f, false, Media.VINYL, 1, "aphex_twin_windowlicker.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Aphex Twin", "Selected Ambient Works Volume II", ArtistGenre.ELECTRONICA, AlbumGenre.AMBIENT, "Warp Records", 599.99f, true, Media.VINYL, 1, "aphex_twin_selected_ambient_works_volume_ii.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Howlin' Wolf", "The Real Folk Blues", ArtistGenre.BLUES, AlbumGenre.BLUES, "Chess", 19.99f, false, Media.VINYL, 1, "howlin_wolf_the_real_folk_blues.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "John Lee Hooker", "Burnin'", ArtistGenre.BLUES, AlbumGenre.BLUES, "Playboy Records", 24.99f, false, Media.CD, 1, "hooker_john_lee_burnin.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Robert Johnson", "King Of The Delta Blues Singers", ArtistGenre.BLUES, AlbumGenre.BLUES, "Columbia", 19.99f, false, Media.VINYL, 1, "johnson_robert_king_of_the_delta_blues.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Leadbelly", "Leadbelly", ArtistGenre.BLUES, AlbumGenre.BLUES, "Playboy Records", 24.99f, false, Media.VINYL, 1, "leadbelly_leadbelly.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Stereolab", "Peng!", ArtistGenre.ALTERNATIVE, AlbumGenre.ALTERNATIVE, "Too Pure", 29.99f, false, Media.VINYL, 1, "stereolab_peng.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Stereolab", "Transient Random-Noise Bursts With Announcements", ArtistGenre.ALTERNATIVE, AlbumGenre.ALTERNATIVE, "Duophonic Ultra High Frequency Disks", 29.99f, false, Media.VINYL, 1, "stereolab_transient_noise_bursts_with_announcements.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Stereolab", "Emperor Tomato Ketchup", ArtistGenre.ALTERNATIVE, AlbumGenre.ALTERNATIVE, "Duophonic Ultra High Frequency Disks", 29.99f, false, Media.VINYL, 1, "stereolab_emperor_tomato_ketchup.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Pixies", "Surfer Rosa", ArtistGenre.ALTERNATIVE, AlbumGenre.ALTERNATIVE, "4AD", 34.99f, false, Media.VINYL, 1, "pixies_surfer_rosa.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Pixies", "Doolitlle", ArtistGenre.ALTERNATIVE, AlbumGenre.ALTERNATIVE, "4AD", 34.99f, false, Media.VINYL, 1, "pixies_doolittle.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Pixies", "Bossanova", ArtistGenre.ALTERNATIVE, AlbumGenre.ALTERNATIVE, "4AD", 34.99f, false, Media.VINYL, 1, "pixies_bossanova.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Pixies", "Trompe Le Monde", ArtistGenre.ALTERNATIVE, AlbumGenre.ALTERNATIVE, "4AD", 34.99f, false, Media.CD, 1, "pixies_trompe_le_monde.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Black Moth Super Rainbow", "Dandelion Gum", ArtistGenre.ALTERNATIVE, AlbumGenre.PSYCH, "Graveface Records", 29.99f, true, Media.VINYL, 1, "black_moth_super_rainbow_dandelion_gum.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Cure", "The Head On The Door", ArtistGenre.ROCK, AlbumGenre.POP, "Fiction Records", 29.99f, false, Media.VINYL, 1, "cure_the_head_on_the_door.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Cure", "Disintegration", ArtistGenre.ROCK, AlbumGenre.ALTERNATIVE, "Fiction Records", 39.99f, true, Media.VINYL, 1, "cure_disintegration.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Cure", "Kiss Me, Kiss Me, Kiss Me", ArtistGenre.ROCK, AlbumGenre.POP, "Fiction Records", 29.99f, false, Media.VINYL, 1, "cure_kiss_me_kiss_me_kiss_me.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Curtis Mayfield", "Curtom", ArtistGenre.SOUL, AlbumGenre.FUNK, "Curtis", 24.99f, true, Media.VINYL, 2, "mayfield_curtis_curtis.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Curtis Mayfield", "Roots", ArtistGenre.SOUL, AlbumGenre.FUNK, "Curtom", 24.99f, true, Media.CD, 1, "mayfield_curtis_roots.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Sam Cooke", "Night Beat", ArtistGenre.SOUL, AlbumGenre.FUNK, "RCA Victor", 29.99f, true, Media.CD, 1, "cooke_sam_night_beat.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Aretha Franklin", "Lady Soul", ArtistGenre.SOUL, AlbumGenre.FUNK, "Atlantic", 24.99f, true, Media.CD, 1, "franklin_aretha_lady_soul.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Supremes", "Where Did Our Love Go", ArtistGenre.SOUL, AlbumGenre.FUNK, "Tamla", 24.99f, true, Media.VINYL, 1, "supremes_where_did_our_love_go.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Idles", "Joy As An Act Of Resistance", ArtistGenre.ALTERNATIVE, AlbumGenre.ALTERNATIVE, "Partisan Records", 24.99f, false, Media.VINYL, 2, "idles_joy_as_an_act_of_resistance.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Ramones", "Ramones", ArtistGenre.PUNK, AlbumGenre.ROCK, "Sire", 24.99f, false, Media.VINYL, 1, "ramones_ramones.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Ramones", "Rocket To Russia", ArtistGenre.PUNK, AlbumGenre.ROCK, "Sire", 24.99f, false, Media.VINYL, 1, "ramones_rocket_to_russia.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Ramones", "It's Alive", ArtistGenre.PUNK, AlbumGenre.ROCK, "Sire", 24.99f, false, Media.CD, 1, "ramones_its_alive.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Gas", "Gas", ArtistGenre.ELECTRONICA, AlbumGenre.AMBIENT, "Mille Plateaux", 49.99f, true, Media.VINYL, 1, "gas_gas.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Gas", "Zauberberg", ArtistGenre.ELECTRONICA, AlbumGenre.AMBIENT, "Mille Plateaux", 49.99f, true, Media.VINYL, 1, "gas_zauberberg.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Gas", "Königsforst", ArtistGenre.ELECTRONICA, AlbumGenre.AMBIENT, "Mille Plateaux", 49.99f, true, Media.VINYL, 1, "gas_konigsforst.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Gas", "Pop", ArtistGenre.ELECTRONICA, AlbumGenre.AMBIENT, "Mille Plateaux", 49.99f, true, Media.VINYL, 1, "gas_pop.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Stooges", "The Stooges", ArtistGenre.ROCK, AlbumGenre.ROCK, "Elektra Records", 24.99f, false, Media.VINYL, 1, "stooges_stooges.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Stooges", "Fun House", ArtistGenre.ROCK, AlbumGenre.ROCK, "Elektra Records", 24.99f, false, Media.VINYL, 1, "stooges_funhouse.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Stooges", "Raw Power", ArtistGenre.ROCK, AlbumGenre.ROCK, "Columbia", 24.99f, false, Media.VINYL, 1, "stooges_raw_power.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "King Gizzard And The Lizard Wizard", "Nonagon Infinity", ArtistGenre.ROCK, AlbumGenre.INDIE, "Flightless", 24.99f, false, Media.VINYL, 1, "king_gizzard_and_the_lizard_wizard_nonagon_infinity.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "King Gizzard And The Lizard Wizard", "I'm In Your Mind Fuzz", ArtistGenre.ROCK, AlbumGenre.PSYCH, "Flightless", 24.99f, false, Media.VINYL, 1, "king_gizzard_and_the_lizard_wizard_im_in_your_mind_fuzz.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "King Gizzard And The Lizard Wizard", "Infest The Rats' Nest", ArtistGenre.ROCK, AlbumGenre.METAL, "Flightless", 24.99f, true, Media.VINYL, 1, "king_gizzard_and_the_lizard_wizard_infest_the_rats_nest.webp", Grade.VG, Grade.NM, "-", "-", ""),
                new Album(Guid.NewGuid(), "King Gizzard And The Lizard Wizard", "Petrodragonic Apocalypse; Or, Dawn Of Eternal Night: An Annihilation Of Planet Earth And The Beginning Of Merciless Damnation", ArtistGenre.ROCK, AlbumGenre.METAL, "KGLW", 24.99f, false, Media.VINYL, 1, "king_gizzard_and_the_lizard_wizard_petradragonic.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "King Gizzard And The Lizard Wizard", "Float Along - Fill Your Lungs", ArtistGenre.ROCK, AlbumGenre.PSYCH, "Flightless", 29.99f, false, Media.VINYL, 1, "king_gizzard_and_the_lizard_wizard_float_along_fill_your_lungs.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Thee Oh Sees", "Face Stabber", ArtistGenre.ROCK, AlbumGenre.GARAGE, "Castle Face", 44.99f, false, Media.VINYL, 1, "thee_oh_sees_face_stabber.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "The White Stripes", "The White Stripes", ArtistGenre.ROCK, AlbumGenre.GARAGE, "XL Recordings", 29.99f, false, Media.VINYL, 1, "white_stripes_white_stripes.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "The White Stripes", "De Stijl", ArtistGenre.ROCK, AlbumGenre.GARAGE, "XL Recordings", 29.99f, false, Media.VINYL, 1, "white_stripes_de_stijl.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "The White Stripes", "White Blood Cells", ArtistGenre.ROCK, AlbumGenre.GARAGE, "XL Recordings", 29.99f, false, Media.VINYL, 3, "white_stripes_white_blood_cells.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "The White Stripes", "Elephant", ArtistGenre.ROCK, AlbumGenre.GARAGE, "XL Recordings", 29.99f, false, Media.VINYL, 2, "white_stripes_elephant.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Joni Mitchell", "Blue", ArtistGenre.FOLK, AlbumGenre.POP, "Reprise Records", 69.99f, true, Media.VINYL, 1, "mitchell_joni_blue.webp", Grade.M, Grade.NM, "-", "-", ""),
                new Album(Guid.NewGuid(), "Joni Mitchell", "Clouds", ArtistGenre.FOLK, AlbumGenre.POP, "Reprise Records", 49.99f, true, Media.VINYL, 1, "mitchell_joni_clouds.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Comus", "First Utterance", ArtistGenre.FOLK, AlbumGenre.PSYCH, "Rise Above Records", 49.99f, true, Media.VINYL, 1, "comus_first_utterance.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Incredible String Band", "The Hangman's Beautiful Daughter", ArtistGenre.FOLK, AlbumGenre.PSYCH, "Island Records", 34.99f, true, Media.VINYL, 1, "incredible_string_band_the_hangmans_beautiful_daughter.webp", Grade.NM, Grade.VG_PLUS, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Incredible String Band", "The 5000 Spirits Or The Layers Of The Onion", ArtistGenre.FOLK, AlbumGenre.PSYCH, "Island Records", 34.99f, true, Media.VINYL, 1, "incredible_string_band_the_5000_spirits_or_the_layers_of_the_onion.webp", Grade.VG, Grade.VG_PLUS, "-", "-", ""),
                new Album(Guid.NewGuid(), "Neil Young", "Everybody Knows This Is Nowhere", ArtistGenre.ROCK, AlbumGenre.FOLK, "Reprise Records", 39.99f, false, Media.CD, 1, "young_neil_everybody_knows_this_is_nowhere.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Neil Young", "After The Gold Rush", ArtistGenre.ROCK, AlbumGenre.FOLK, "Reprise Records", 39.99f, true, Media.VINYL, 1, "young_neil_after_the_gold_rush.webp", Grade.NONE, Grade.NM, "-", "-", ""),
                new Album(Guid.NewGuid(), "Herbie Hancock", "Head Hunters", ArtistGenre.JAZZ, AlbumGenre.FUSION, "Columbia", 24.99f, true, Media.CD, 1, "hancock_herbie_head_hunters.webp", Grade.NONE, Grade.M, "-", "-", ""),
                new Album(Guid.NewGuid(), "Herbie Hancock", "Future Shock", ArtistGenre.JAZZ, AlbumGenre.ELECTRONICA, "Reprise Records", 29.99f, true, Media.VINYL, 1, "hancock_herbie_future_shock.webp", Grade.NONE, Grade.VG_PLUS, "-", "-", ""),
                new Album(Guid.NewGuid(), "Led Zeppelin", "Led Zeppelin", ArtistGenre.ROCK, AlbumGenre.HARDROCK, "Atlantic", 39.99f, true, Media.VINYL, 1, "led_zeppelin_i.webp", Grade.NONE, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Led Zeppelin", "Led Zeppelin II", ArtistGenre.ROCK, AlbumGenre.HARDROCK, "Atlantic", 39.99f, false, Media.VINYL, 1, "led_zeppelin_ii.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Led Zeppelin", "Led Zeppelin III", ArtistGenre.ROCK, AlbumGenre.HARDROCK, "Atlantic", 39.99f, false, Media.VINYL, 1, "led_zeppelin_iii.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Led Zeppelin", "Untitled", ArtistGenre.ROCK, AlbumGenre.HARDROCK, "Atlantic", 19.99f, false, Media.VINYL, 1, "led_zeppelin_untitled.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Led Zeppelin", "Untitled", ArtistGenre.ROCK, AlbumGenre.HARDROCK, "Atlantic", 39.99f, true, Media.CD, 2, "led_zeppelin_untitled.webp", Grade.NONE, Grade.VG_PLUS, "-", "-", ""),
                new Album(Guid.NewGuid(), "Budgie", "Squawk", ArtistGenre.ROCK, AlbumGenre.HARDROCK, "Kapp Records", 39.99f, false, Media.VINYL, 1, "budgie_squawk.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Doors", "L.A. Woman", ArtistGenre.ROCK, AlbumGenre.PSYCH, "Elektra Records", 39.99f, true, Media.VINYL, 1, "doors_l_a_woman.webp", Grade.NONE, Grade.VG_PLUS, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Doors", "Strange Days", ArtistGenre.ROCK, AlbumGenre.PSYCH, "Elektra Records", 39.99f, true, Media.VINYL, 1, "doors_strange_days.webp", Grade.NONE, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Doors", "Waiting For The Sun", ArtistGenre.ROCK, AlbumGenre.PSYCH, "Elektra Records", 39.99f, false, Media.VINYL, 1, "doors_waiting_for_the_sun.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Childish Gambino", "Awaken, My Love!", ArtistGenre.HIPHOP, AlbumGenre.SOUL, "Glassnote", 29.99f, false, Media.VINYL, 2, "childish_gambino_awaken_my_love.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Kendrick Lamar", "To Pimp A Butterfly", ArtistGenre.HIPHOP, AlbumGenre.RAP, "Interscope Records", 29.99f, false, Media.VINYL, 2, "lamar_kendrick_to_pimp_a_butterfly.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Clams Casino", "Rainforest", ArtistGenre.HIPHOP, AlbumGenre.AMBIENT, "Tri Angle", 29.99f, true, Media.CD, 1, "clams_casino_rainforest.webp", Grade.NONE, Grade.VG_PLUS, "-", "-", ""),
                new Album(Guid.NewGuid(), "Melvins", "Houdini", ArtistGenre.ALTERNATIVE, AlbumGenre.STONER, "Boner Records", 34.99f, false, Media.VINYL, 1, "melvins_houdini.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Melvins", "Bullhead", ArtistGenre.ALTERNATIVE, AlbumGenre.STONER, "Boner Records", 39.99f, false, Media.VINYL, 1, "melvins_bullhead.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Melvins", "Ozma", ArtistGenre.ALTERNATIVE, AlbumGenre.STONER, "Boner Records", 39.99f, false, Media.VINYL, 1, "melvins_ozma.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Residents", "Not Available", ArtistGenre.ROCK, AlbumGenre.EXPERIMENTAL, "MVD Audio", 34.99f, false, Media.VINYL, 1, "residents_not_available.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Residents", "Duck Stab / Buster & Glen", ArtistGenre.ROCK, AlbumGenre.EXPERIMENTAL, "MVD Audio", 34.99f, false, Media.VINYL, 1, "residents_duck_stab_buster_and_glenn.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Residents", "Eskimo", ArtistGenre.ROCK, AlbumGenre.EXPERIMENTAL, "MVD Audio", 34.99f, false, Media.VINYL, 1, "residents_eskimo.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Les Rita Mitsouko", "The No Comprendo", ArtistGenre.FRANCOPHONE, AlbumGenre.FRANCOPHONE, "Virgin", 24.99f, false, Media.VINYL, 1, "rita_mitsouko_the_no_comprendo.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Fernand Gignac", "Vous Souvenez-Vous De...", ArtistGenre.FRANCOPHONE, AlbumGenre.FRANCOPHONE, "Disques Diva", 1.99f, true, Media.CD, 1, "gignac_fernand_vous_souvenez_vous_de.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Celine Dion", "D'Eux", ArtistGenre.POP, AlbumGenre.FRANCOPHONE, "Columbia", 49.99f, true, Media.CD, 1, "dion_celine_d_eux.webp", Grade.M, Grade.NM, "-", "-", ""),
                new Album(Guid.NewGuid(), "Celine Dion", "Unison", ArtistGenre.POP, AlbumGenre.POP, "Columbia", 49.99f, true, Media.VINYL, 1, "dion_celine_unison.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Celine Dion", "Des Mots Qui Sonnent", ArtistGenre.POP, AlbumGenre.FRANCOPHONE, "Columbia", 29.99f, true, Media.VINYL, 1, "dion_celine_des_mots_qui_sonnent.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Ginette Reno", "Noël Avec Ginette Reno", ArtistGenre.FRANCOPHONE, AlbumGenre.CHRISTMAS, "Grand Prix", 4.99f, true, Media.VINYL, 1, "reno_ginette_noel_avec_ginette_reno.webp", Grade.VG, Grade.NM, "-", "-", ""),
                new Album(Guid.NewGuid(), "Ginette Reno", "Ginette En Amour", ArtistGenre.FRANCOPHONE, AlbumGenre.POP, "Apex", 4.99f, true, Media.VINYL, 1, "reno_ginette_ginette_en_amour.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Jean Leloup", "L'Amour Est Sans Pitié", ArtistGenre.FRANCOPHONE, AlbumGenre.ROCK, "Grand Prix", 39.99f, true, Media.VINYL, 1, "leloup_jean_l_amour_est_sans_pitie.webp", Grade.NM, Grade.VG_PLUS, "-", "-", ""),
                new Album(Guid.NewGuid(), "Eric Lapointe", "Obsession", ArtistGenre.FRANCOPHONE, AlbumGenre.ROCK, "Audiogram", 2.99f, true, Media.CD, 1, "lapointe_eric_obsession.webp", Grade.M, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Johnny Cash", "At Folsom Prison", ArtistGenre.COUNTRY, AlbumGenre.COUNTRY, "Columbia", 29.99f, true, Media.VINYL, 1, "cash_johnny_at_folsom_prison.webp", Grade.NM, Grade.VG_PLUS, "-", "-", ""),
                new Album(Guid.NewGuid(), "Johnny Cash", "At San Quentin", ArtistGenre.COUNTRY, AlbumGenre.COUNTRY, "Columbia", 29.99f, true, Media.VINYL, 1, "cash_johnny_at_san_quentin.webp", Grade.VG, Grade.VG_PLUS, "-", "-", ""),
                new Album(Guid.NewGuid(), "Johnny Cash", "American Recordings", ArtistGenre.COUNTRY, AlbumGenre.COUNTRY, "American Recordings", 29.99f, false, Media.VINYL, 1, "cash_johnny_american_recordings.webp", Grade.VG, Grade.M, "-", "-", ""),
                new Album(Guid.NewGuid(), "Dolly Parton", "Jolene", ArtistGenre.COUNTRY, AlbumGenre.COUNTRY, "Atlantic", 24.99f, true, Media.VINYL, 1, "parton_dolly_jolene.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Rolling Stones", "Let It Bleed", ArtistGenre.ROCK, AlbumGenre.PSYCH, "London Records", 39.99f, true, Media.VINYL, 1, "rolling_stones_let_it_bleed.webp", Grade.VG, Grade.VG_PLUS, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Rolling Stones", "Aftermath", ArtistGenre.ROCK, AlbumGenre.PSYCH, "London Records", 34.99f, true, Media.VINYL, 1, "rolling_stones_aftermath.webp", Grade.NM, Grade.M, "-", "-", ""),
                new Album(Guid.NewGuid(), "Kris Kristofferson", "Me And Bobby McGee", ArtistGenre.COUNTRY, AlbumGenre.COUNTRY, "Monument", 29.99f, true, Media.VINYL, 1, "kristofferson_kris_me_and_bobby_mcgee.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Willie Nelson", "Red Headed Stranger", ArtistGenre.COUNTRY, AlbumGenre.COUNTRY, "Columbia", 29.99f, true, Media.CD, 1, "nelson_willie_red_headed_stranger.webp", Grade.NM, Grade.M, "-", "-", ""),
                new Album(Guid.NewGuid(), "John Fahey", "The Transfiguration Of Blind Joe Death", ArtistGenre.FOLK, AlbumGenre.ACOUSTIC, "Takoma", 34.99f, true, Media.VINYL, 1, "fahey_john_the_transfiguration_of_blind_joe_death.webp", Grade.VG, Grade.VG_PLUS, "-", "-", ""),
                new Album(Guid.NewGuid(), "John Fahey", "The Yellow Princess", ArtistGenre.FOLK, AlbumGenre.ACOUSTIC, "Vanguard", 34.99f, true, Media.CD, 1, "fahey_john_the_yellow_princess.webp", Grade.NM, Grade.M, "-", "-", ""),
                new Album(Guid.NewGuid(), "Emmylou Harris", "Elite Hotel", ArtistGenre.COUNTRY, AlbumGenre.COUNTRY, "Reprise Records", 24.99f, true, Media.VINYL, 1, "harris_emmylou_elite_hotel.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Billie Eilish", "When We All Fall Asleep, Where Do We Go?", ArtistGenre.POP, AlbumGenre.INDIE, "Interscope Records", 29.99f, false, Media.VINYL, 1, "eilish_billie_when_we_all_fall_asleep_where_do_we_go.webp", Grade.M, Grade.NM, "-", "-", ""),
                new Album(Guid.NewGuid(), "FKA Twigs", "Magdalene", ArtistGenre.POP, AlbumGenre.DOWNTEMPO, "Young Turks", 34.99f, false, Media.VINYL, 2, "fka_twigs_magdalene.webp", Grade.VG, Grade.VG_PLUS, "-", "-", ""),
                new Album(Guid.NewGuid(), "FKA Twigs", "LP1", ArtistGenre.POP, AlbumGenre.DOWNTEMPO, "Young Turks", 34.99f, false, Media.VINYL, 1, "fka_twigs_lp1.webp", Grade.NM, Grade.M, "-", "-", ""),
                new Album(Guid.NewGuid(), "Björk", "Debut", ArtistGenre.POP, AlbumGenre.ALTERNATIVE, "One Little Indian", 29.99f, true, Media.CD, 1, "bjork_debut.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Björk", "Post", ArtistGenre.POP, AlbumGenre.ALTERNATIVE, "One Little Indian", 29.99f, true, Media.CD, 1, "bjork_post.webp", Grade.M, Grade.NM, "-", "-", ""),
                new Album(Guid.NewGuid(), "Björk", "Homogenic", ArtistGenre.POP, AlbumGenre.ALTERNATIVE, "One Little Indian", 29.99f, true, Media.CD, 1, "bjork_homogenic.webp", Grade.NM, Grade.M, "-", "-", ""),
                new Album(Guid.NewGuid(), "Björk", "Vespertine", ArtistGenre.POP, AlbumGenre.ALTERNATIVE, "One Little Indian", 29.99f, true, Media.CD, 1, "bjork_vespertine.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Beyonce", "Lemonade", ArtistGenre.POP, AlbumGenre.SOUL, "Columbia", 29.99f, true, Media.VINYL, 3, "beyonce_lemonade.webp", Grade.NM, Grade.M, "-", "-", ""),
                new Album(Guid.NewGuid(), "Beyonce", "Lemonade", ArtistGenre.POP, AlbumGenre.SOUL, "Columbia", 29.99f, true, Media.CD, 3, "beyonce_lemonade.webp", Grade.M, Grade.NM, "-", "-", ""),
                new Album(Guid.NewGuid(), "Lana Del Rey", "Born To Die", ArtistGenre.POP, AlbumGenre.INDIE, "Polydor", 29.99f, true, Media.VINYL, 1, "del_rey_lana_born_to_die.webp", Grade.VG, Grade.VG_PLUS, "-", "-", ""),
                new Album(Guid.NewGuid(), "Lana Del Rey", "Ultraviolence", ArtistGenre.POP, AlbumGenre.INDIE, "Polydor", 29.99f, true, Media.VINYL, 1, "del_rey_lana_ultraviolence.webp", Grade.M, Grade.NM, "-", "-", ""),
                new Album(Guid.NewGuid(), "Normand L'Amour", "C'est Pas Possible!", ArtistGenre.FRANCOPHONE, AlbumGenre.KITSCH, "Boulevard Musique", 39.99f, true, Media.CD, 1, "lamour_normand_cest_pas_possible.webp", Grade.NM, Grade.M, "-", "-", ""),
                new Album(Guid.NewGuid(), "Various", "Total 7", ArtistGenre.ELECTRONICA, AlbumGenre.IDM, "Kompakt", 34.99f, true, Media.CD, 1, "various_total_7.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Various", "No Alternative", ArtistGenre.ALTERNATIVE, AlbumGenre.ALTERNATIVE, "Arista", 29.99f, true, Media.CD, 1, "various_no_alternative.webp", Grade.NM, Grade.M, "-", "-", ""),
                new Album(Guid.NewGuid(), "Various", "The Music Of Islam", ArtistGenre.WORLD, AlbumGenre.FOLK, "Celestial Harmonies", 19.99f, true, Media.CD, 1, "various_the_music_of_islam.webp", Grade.M, Grade.NM, "-", "-", ""),
                new Album(Guid.NewGuid(), "Amon Düül II", "Yeti", ArtistGenre.ROCK, AlbumGenre.PSYCH, "Purple Pyramid", 49.99f, false, Media.VINYL, 1, "amon_duul_ii_yeti.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "b e g o t t e n", "(death cycle)", ArtistGenre.ELECTRONICA, AlbumGenre.VAPORWAVE, "Geometric Lullaby", 34.99f, false, Media.VINYL, 1, "begotten_death_cycle.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Acid Mothers Temple And The Melting Paraiso UFO", "Acid Mothers Temple & The Melting Paraiso UFO", ArtistGenre.ROCK, AlbumGenre.PSYCH, "P.S.F. Records", 29.99f, true, Media.CD, 1, "acid_mothers_temple_acid_mothers_temple.webp", Grade.M, Grade.NM, "-", "-", ""),
                new Album(Guid.NewGuid(), "Acid Mothers Temple And The Melting Paraiso UFO", "Pataphisical Freak Out MU!!", ArtistGenre.ROCK, AlbumGenre.PSYCH, "P.S.F. Records", 29.99f, true, Media.CD, 1, "acid_mothers_temple_pataphysical_freakout.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Acid Mothers Temple And The Melting Paraiso UFO", "Absolutely Freak Out (Zap Your Mind!!)", ArtistGenre.ROCK, AlbumGenre.PSYCH, "Static Caravan", 29.99f, true, Media.CD, 1, "acid_mothers_temple_absolutely_freak_out.webp", Grade.VG, Grade.NM, "-", "-", ""),
                new Album(Guid.NewGuid(), "AMM", "AMMMusic", ArtistGenre.JAZZ, AlbumGenre.EXPERIMENTAL, "Matchless Recordings", 29.99f, true, Media.CD, 1, "amm_ammmusic.webp", Grade.M, Grade.VG_PLUS, "-", "-", ""),
                new Album(Guid.NewGuid(), "AMM", "Before Driving To The Chapel We Took Coffee With Rick And Jennifer Reed", ArtistGenre.JAZZ, AlbumGenre.EXPERIMENTAL, "Matchless Recordings", 29.99f, true, Media.CD, 1, "amm_before_driving_to_the_chapel.webp", Grade.NM, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "AMM", "The Crypt", ArtistGenre.JAZZ, AlbumGenre.EXPERIMENTAL, "Matchless Recordings", 39.99f, true, Media.CD, 1, "amm_the_crypt.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Anthony Naples", "El Portal", ArtistGenre.ELECTRONICA, AlbumGenre.ELECTRONICA, "The Trilogy Tapes", 19.99f, true, Media.VINYL, 1, "anthony_naples_el_portal.webp", Grade.M, Grade.NM, "-", "-", ""),
                new Album(Guid.NewGuid(), "Anthony Naples", "Zipacon", ArtistGenre.ELECTRONICA, AlbumGenre.ELECTRONICA, "The Trilogy Tapes", 19.99f, true, Media.VINYL, 1, "anthony_naples_zipacon.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Butthole Surfers", "Hairway To Steven", ArtistGenre.ALTERNATIVE, AlbumGenre.ALTERNATIVE, "Latino Bugger Veil", 19.99f, true, Media.CD, 1, "butthole_surfers_hairway_to_steven.webp", Grade.M, Grade.NM, "-", "-", ""),
                new Album(Guid.NewGuid(), "Butthole Surfers", "Independant Worm Saloon", ArtistGenre.ALTERNATIVE, AlbumGenre.ALTERNATIVE, "Latino Bugger Veil", 19.99f, true, Media.CD, 1, "butthole_surfers_independant_worm_saloon.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Butthole Surfers", "Locaust Abortion Technician", ArtistGenre.ALTERNATIVE, AlbumGenre.ALTERNATIVE, "Latino Bugger Veil", 19.99f, true, Media.CD, 1, "butthole_surfers_locust_abortion_technician.webp", Grade.VG, Grade.NM, "-", "-", ""),
                new Album(Guid.NewGuid(), "Butthole Surfers", "Psychic... Powerless... Another Man's Sac", ArtistGenre.ALTERNATIVE, AlbumGenre.ALTERNATIVE, "Latino Bugger Veil", 19.99f, true, Media.CD, 1, "butthole_surfers_psychic_powerless.webp", Grade.M, Grade.VG_PLUS, "-", "-", ""),
                new Album(Guid.NewGuid(), "Butthole Surfers", "Rembrandt Pussyhorse", ArtistGenre.ALTERNATIVE, AlbumGenre.ALTERNATIVE, "Latino Bugger Veil", 29.99f, false, Media.VINYL, 1, "butthole_surfers_rembrandt_pussyhorse.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "Captain Beefheart And The Magic Band", "Trout Mask Replica", ArtistGenre.ROCK, AlbumGenre.PSYCH, "Straight", 19.99f, true, Media.CD, 1, "captain_beefheart_trout_mask_replica.webp", Grade.M, Grade.NM, "-", "-", ""),
                new Album(Guid.NewGuid(), "Captain Beefheart And The Magic Band", "Lick My Decals Off, Baby!", ArtistGenre.ROCK, AlbumGenre.PSYCH, "Straight", 19.99f, true, Media.CD, 1, "captain_beefheart_lick_my_decals.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Chemotex", "Thusla", ArtistGenre.ELECTRONICA, AlbumGenre.ELECTRONICA, "The Trilogy Tapes", 19.99f, true, Media.VINYL, 1, "chemotex_thulsa.webp", Grade.M, Grade.NM, "-", "-", ""),
                new Album(Guid.NewGuid(), "Dinosaur Jr.", "Where You Been?", ArtistGenre.ALTERNATIVE, AlbumGenre.ALTERNATIVE, "Sire", 19.99f, true, Media.CD, 1, "dinosaur_jr_where_you_been.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Dinosaur Jr.", "Without A Sound", ArtistGenre.ALTERNATIVE, AlbumGenre.ALTERNATIVE, "Sire", 19.99f, true, Media.CD, 1, "dinosaur_jr_without_a_sound.webp", Grade.M, Grade.NM, "-", "-", ""),
                new Album(Guid.NewGuid(), "Dinosaur Jr.", "Bug", ArtistGenre.ALTERNATIVE, AlbumGenre.ALTERNATIVE, "SST Records", 19.99f, true, Media.CD, 1, "dinosaur_jr_bug.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Dinosaur Jr.", "Where You Been?", ArtistGenre.ALTERNATIVE, AlbumGenre.ALTERNATIVE, "Sire", 19.99f, true, Media.CD, 1, "dinosaur_jr_where_you_been.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Dinosaur Jr.", "Without A Sound", ArtistGenre.ALTERNATIVE, AlbumGenre.ALTERNATIVE, "Sire", 19.99f, true, Media.CD, 1, "dinosaur_jr_without_a_sound.webp", Grade.M, Grade.NM, "-", "-", ""),
                new Album(Guid.NewGuid(), "Godspeed You Black Emperor!", "F♯ A♯ ∞", ArtistGenre.ALTERNATIVE, AlbumGenre.ALTERNATIVE, "Kranky", 19.99f, true, Media.CD, 1, "godspeed_you_black_emperor_fa.webp", Grade.M, Grade.NM, "-", "-", ""),
                new Album(Guid.NewGuid(), "Godspeed You Black Emperor!", "Lift Your Skinny Fists Like Antennas To Heaven", ArtistGenre.ALTERNATIVE, AlbumGenre.ALTERNATIVE, "Kranky", 19.99f, true, Media.CD, 1, "godspeed_you_black_emperor_lift_yr_skinny_fists.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Godspeed You Black Emperor!", "Slow Riot For New Zero Kanada E.P.", ArtistGenre.ALTERNATIVE, AlbumGenre.ALTERNATIVE, "Kranky", 19.99f, true, Media.CD, 1, "godspeed_you_black_emperor_slow_riot_for_zero.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Godspeed You Black Emperor!", "Yanqui U.X.O.", ArtistGenre.ALTERNATIVE, AlbumGenre.ALTERNATIVE, "Kranky", 19.99f, true, Media.CD, 1, "godspeed_you_black_emperor_yanqui_uxo.webp", Grade.M, Grade.NM, "-", "-", ""),
                new Album(Guid.NewGuid(), "Gong", "Angel's Egg", ArtistGenre.ROCK, AlbumGenre.PSYCH, "Charly Records", 9.99f, true, Media.CD, 1, "gong_angels_egg.webp", Grade.M, Grade.NM, "-", "-", ""),
                new Album(Guid.NewGuid(), "Gong", "Camembert Électrique", ArtistGenre.ROCK, AlbumGenre.PSYCH, "Charly Records", 14.99f, true, Media.CD, 1, "gong_camembert_electrique.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Lightning Bolt", "Hypermagic Mountain", ArtistGenre.ALTERNATIVE, AlbumGenre.NOISE, "Load Records", 14.99f, true, Media.CD, 1, "lightning_bolt_hypermagic_mountain.webp", Grade.M, Grade.NM, "-", "-", ""),
                new Album(Guid.NewGuid(), "Lightning Bolt", "Ride The Skies", ArtistGenre.ALTERNATIVE, AlbumGenre.NOISE, "Load Records", 14.99f, true, Media.CD, 1, "lightning_bolt_ride_the_skies.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Lightning Bolt", "Wonderful Rainbow", ArtistGenre.ALTERNATIVE, AlbumGenre.NOISE, "Load Records", 14.99f, true, Media.CD, 1, "lightning_bolt_wonderful_rainbow.webp", Grade.M, Grade.NM, "-", "-", ""),
                new Album(Guid.NewGuid(), "Neu!", "Neu!", ArtistGenre.ROCK, AlbumGenre.NOISE, "Astralwerks", 14.99f, true, Media.CD, 1, "neu_neu.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Neu!", "Neu! 2", ArtistGenre.ROCK, AlbumGenre.NOISE, "Astralwerks", 14.99f, true, Media.CD, 1, "neu_neu_ii.webp", Grade.M, Grade.NM, "-", "-", ""),
                new Album(Guid.NewGuid(), "Neu!", "Neu! 75", ArtistGenre.ROCK, AlbumGenre.NOISE, "Astralwerks", 14.99f, true, Media.CD, 1, "neu_neu_75.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Rezzett", "Boshly", ArtistGenre.ELECTRONICA, AlbumGenre.ELECTRONICA, "The Trilogy Tapes", 24.99f, true, Media.VINYL, 1, "rezzett_boshly.webp", Grade.M, Grade.NM, "-", "-", ""),
                new Album(Guid.NewGuid(), "Rezzett", "Goodness", ArtistGenre.ELECTRONICA, AlbumGenre.ELECTRONICA, "The Trilogy Tapes", 24.99f, true, Media.VINYL, 1, "rezzett_goodness.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Sonic Youth", "Bad Moon Rising", ArtistGenre.ALTERNATIVE, AlbumGenre.EXPERIMENTAL, "Geffen Records", 24.99f, true, Media.CD, 1, "sonic_youth_bad_moon_rising.webp", Grade.M, Grade.NM, "-", "-", ""),
                new Album(Guid.NewGuid(), "Sonic Youth", "Daydream Nation", ArtistGenre.ALTERNATIVE, AlbumGenre.EXPERIMENTAL, "Geffen Records", 24.99f, true, Media.CD, 1, "sonic_youth_daydream_nation.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Sonic Youth", "Dirty", ArtistGenre.ALTERNATIVE, AlbumGenre.EXPERIMENTAL, "Geffen Records", 24.99f, true, Media.CD, 1, "sonic_youth_dirty.webp", Grade.M, Grade.NM, "-", "-", ""),
                new Album(Guid.NewGuid(), "Sonic Youth", "Evol", ArtistGenre.ALTERNATIVE, AlbumGenre.EXPERIMENTAL, "Geffen Records", 24.99f, true, Media.CD, 1, "sonic_youth_evol.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "Sonic Youth", "Goo", ArtistGenre.ALTERNATIVE, AlbumGenre.EXPERIMENTAL, "Geffen Records", 24.99f, true, Media.CD, 1, "sonic_youth_goo.webp", Grade.M, Grade.NM, "-", "-", ""),
                new Album(Guid.NewGuid(), "Sonic Youth", "Sister", ArtistGenre.ALTERNATIVE, AlbumGenre.EXPERIMENTAL, "Geffen Records", 24.99f, true, Media.CD, 1, "sonic_youth_sister.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "SunnO)))", "GrimmRobe Demos", ArtistGenre.METAL, AlbumGenre.MINIMALIST, "Southern Lord", 24.99f, true, Media.CD, 1, "sunn_o_grimmrobe_demos.webp", Grade.M, Grade.NM, "-", "-", ""),
                new Album(Guid.NewGuid(), "SunnO)))", "White 1", ArtistGenre.METAL, AlbumGenre.MINIMALIST, "Southern Lord", 24.99f, true, Media.CD, 1, "sunn_o_white_i.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "SunnO)))", "White 2", ArtistGenre.METAL, AlbumGenre.MINIMALIST, "Southern Lord", 24.99f, true, Media.CD, 1, "sunn_o_white_ii.webp", Grade.M, Grade.NM, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Gerogerigegege", "Uguisudani Apocalypse", ArtistGenre.ELECTRONICA, AlbumGenre.FUNK, "The Trilogy Tapes", 24.99f, false, Media.VINYL, 1, "the_gerogerigegege_uguisudani_apocalypse.webp", Grade.NONE, Grade.NONE, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Necks", "Aether", ArtistGenre.JAZZ, AlbumGenre.AMBIENT, "Fish Of Milk", 24.99f, true, Media.CD, 1, "the_necks_aether.webp", Grade.M, Grade.NM, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Necks", "Drive By", ArtistGenre.JAZZ, AlbumGenre.AMBIENT, "Fish Of Milk", 24.99f, true, Media.CD, 1, "the_necks_drive_by.webp", Grade.VG_PLUS, Grade.VG, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Necks", "Unfold", ArtistGenre.JAZZ, AlbumGenre.AMBIENT, "Fish Of Milk", 24.99f, true, Media.CD, 1, "the_necks_unfold.webp", Grade.M, Grade.NM, "-", "-", ""),
                new Album(Guid.NewGuid(), "The Shaggs", "Philosophy Of The World", ArtistGenre.ROCK, AlbumGenre.POP, "Light In The Attic", 24.99f, false, Media.CD, 1, "the_shaggs_philosophy.webp", Grade.NONE, Grade.NONE, "-", "-", "")
            };

            builder.Entity<Album>().HasData(albums);
        }
    }
}
