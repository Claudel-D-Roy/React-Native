namespace WizardRecords.Api.Data {
    public class Constants {
        public enum Media {
            VINYL,
            CD
        }

        // Represents the in-store section of the artist
        public enum ArtistGenre {
            ROCK,
            POP,
            JAZZ,
            HIPHOP,
            ALTERNATIVE,
            CLASSICAL,
            FRANCOPHONE,
            METAL,
            PUNK,
            BLUES,
            WORLD,
            FOLK,
            COUNTRY,
            SOUL,
            FUNK,
            ELECTRONICA,
            SOUNDTRACK,
            UNSPECIFIED
        }

        // Represents a specific genre for a single album
        public enum AlbumGenre {
            ROCK,
            POP,
            JAZZ,
            HIPHOP,
            ALTERNATIVE,
            CLASSICAL,
            FRANCOPHONE,
            METAL,
            PUNK,
            BLUES,
            WORLD,
            FOLK,
            COUNTRY,
            SOUL,
            FUNK,
            ELECTRONICA,
            SOUNDTRACK,
            HARD,
            FUSION,
            PROG,
            MODAL,
            FREEJAZZ,
            INDIE,
            POSTBOP,
            MINIMALIST,
            PSYCH,
            HARDROCK,
            RAP,
            GARAGE,
            VAPORWAVE,
            ROCKABILLY,
            EXPERIMENTAL,
            INSTRUMENTAL,
            INDUSTRIAL,
            SYNTHWAVE,
            KITSCH,
            REGGAE,
            NEWWAVE,
            DOWNTEMPO,
            ACOUSTIC,
            NEWAGE,
            OPERA,
            STONER,
            DOOM,
            IDM,
            TRIPHOP,
            DEATHCORE,
            DEATHMETAL,
            METALCORE,
            HARDCORE,
            MODERN,
            HOUSE,
            BAROQUE,
            ROMANTIC,
            AFROBEAT,
            DANCE,
            NOISE,
            AMBIENT,
            VARIETY,
            GRUNGE,
            HISTORICAL,
            MUSICAL,
            DISCO,
            COMEDY,
            SOUNDEFFECTS,
            KIDS,
            CHRISTMAS,
            UNSPECIFIED
        }



        public enum Format {
            RPM_33,
            RPM_45,
            RPM_78,
            LP,
            EP,
            ALBUM,
            SINGLE,
            DOUBLE,
            FLEXIDISC,
            PICTUREDISC,
            SHELLAC,
            DIGIPAK,
            INCH_12,
            INCH_10,
            INCH_7,
            INCH_5,
            INCH_3,
            UNSPECIFIED
        }

        // For USED items only
        public enum Grade {
            M,
            NM,
            VG_PLUS,
            VG,
            G_PLUS,
            G,
            F,
            P,
            NONE
        }

        public enum Province {
            BC,
            AB,
            SK,
            MB,
            ON,
            QC,
            NB,
            NS,
            PE,
            NL,
            NT,
            NU,
            YT
        }
    }
}
