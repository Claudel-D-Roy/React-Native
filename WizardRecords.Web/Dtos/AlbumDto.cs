using static WizardRecords.Api.Data.Constants;

namespace WizardRecords.Api.Dtos {
    public record AlbumDto(
         Guid AlbumId,
         string ArtistName,
         string Title,
         ArtistGenre ArtistGenre,
         AlbumGenre AlbumGenre,
         string LabelName,
         float Price,
         bool IsUsed,
         Media Media,
         int Quantity,
         string ImageFilePath,
         Grade? MediaGrade,
         Grade? SleeveGrade,
         string? CatalogNumber,
         string? MatrixNumber,
         string? Comments
     );

    // CRUD
    public record AlbumUpdate(
        string Title, 
        int Quantity, 
        float Price
    );

    public record AlbumCreate(
         Guid AlbumId,
         string ArtistName,
         string Title,
         ArtistGenre ArtistGenre,
         AlbumGenre AlbumGenre,
         string LabelName,
         float Price,
         bool IsUsed,
         Media Media,
         int Quantity,
         string ImageFilePath,
         Grade? MediaGrade,
         Grade? SleeveGrade,
         string? CatalogNumber,
         string? MatrixNumber,
         string? Comments
    );

    public record AlbumDelete(string Title);
}
