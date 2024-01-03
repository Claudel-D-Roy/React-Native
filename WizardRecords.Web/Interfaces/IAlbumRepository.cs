using WizardRecords.Api.Domain.Entities;

using static WizardRecords.Api.Data.Constants;

namespace WizardRecords.Repositories {
    public interface IAlbumRepository {
        Task<IEnumerable<Album>> GetAllAlbumsAsync();
        Task<IEnumerable<Album>> GetAlbumsByArtistNameAsync(string artistName);
        Task<IEnumerable<Album>> GetAlbumsByLabelNameAsync(string labelName);
        Task<IEnumerable<Album>> GetAlbumsByMediaAsync(Media mediaType);
        Task<IEnumerable<Album>> GetAlbumsByUsedOrNewAsync(bool isUsed);
        Task<IEnumerable<Album>> GetAlbumsByGenreAsync(ArtistGenre artistGenre);
        Task<IEnumerable<Album>> GetSearchAlbumsAsync(string query);
        Task<IEnumerable<Album>> GetRandomAlbumsAsync(int count, Media? media = null, bool? isUsed = null);
        Task<IEnumerable<Album>> GetAlbumsByMediaAndConditionAsync(Media? media = null, bool? isUsed = null);

        Task<Album?> GetAlbumByIdAsync(Guid albumId);
        Task<Album?> GetAlbumByArtistNameAndTitleAsync(string artistName, string title);
        Task<Album?> GetAlbumByTitleAsync(string title);

        // CRUD
        Task<Album?> UpdateAlbumAsync(Guid albumId, Album updateData);
        Task<Album?> DeleteAlbumAsync(Guid albumId);
        Task CreateAlbumAsync(Album album);
    }
}
