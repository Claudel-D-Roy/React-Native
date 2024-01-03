using static WizardRecords.Api.Data.Constants;

namespace WizardRecords.Api.Domain.Entities {
    public class Album {
        public Guid AlbumId { get; set; }
        public string ArtistName { get; set; } = "-";
        public string Title { get; set; } = "-";
        public ArtistGenre ArtistGenre { get; set; }
        public AlbumGenre AlbumGenre { get; set; }
        public string LabelName { get; set; } = "-";
        public float Price { get; set; } = 0.0f;
        public bool IsUsed { get; set; } = false;
        public Media Media { get; set; }
        public int Quantity { get; set; } = 0;
        public string ImageFilePath { get; set; } = "-"; // TODO: Find default image 
        public Grade? MediaGrade { get; set; } = Grade.NONE; // USED only!
        public Grade? SleeveGrade { get; set; } = Grade.NONE; // USED only!
        public string? CatalogNumber { get; set; } = "-"; // USED only!
        public string? MatrixNumber { get; set; } = "-"; // USED only!
        public string? Comments { get; set; } = "-";

        // Constructors
        internal Album() { }

        public Album(
             Guid albumId,
             string artistName,
             string title,
             ArtistGenre artistGenre,
             AlbumGenre albumGenre,
             string labelName,
             float price,
             bool isUsed,
             Media media,
             int quantity,
             string imageFilePath,
             Grade? mediaCondition,
             Grade? sleeveCondition,
             string? catalogNumber,
             string? matrixNumber,
             string? comments) {
            AlbumId = albumId;
            ArtistName = artistName;
            Title = title;
            ArtistGenre = artistGenre;
            AlbumGenre = albumGenre;
            LabelName = labelName;
            Price = price;
            IsUsed = isUsed;
            Media = media;
            Quantity = quantity;
            ImageFilePath = imageFilePath;
            MediaGrade = mediaCondition;
            SleeveGrade = sleeveCondition;
            CatalogNumber = catalogNumber;
            MatrixNumber = matrixNumber;
            Comments = comments;
        }
    }
}
