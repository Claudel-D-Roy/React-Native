using Microsoft.EntityFrameworkCore;
using WizardRecords.Api.Domain.Entities;
using WizardRecords.Api.Data;
using WizardRecords.Repositories;

namespace WizardRecords.Api.Repositories {
    public class AlbumRepository : IAlbumRepository {

        private readonly WizRecDbContext _context;
        private readonly Random _random = new Random();

        public AlbumRepository(WizRecDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<Album>> GetAllAlbumsAsync() {
            return await _context.Albums.ToListAsync();
        }

        public async Task<IEnumerable<Album>> GetAlbumsByArtistNameAsync(string artistName) {
            return await _context.Albums.Where(a => a.ArtistName.ToLower() == artistName.ToLower()).ToListAsync();
        }

        public async Task<IEnumerable<Album>> GetAlbumsByLabelNameAsync(string labelName) {
            return await _context.Albums.Where(a => a.LabelName.ToLower() == labelName.ToLower()).ToListAsync();
        }

        public async Task<IEnumerable<Album>> GetAlbumsByMediaAsync(Constants.Media media) {
            return await _context.Albums.Where(a => a.Media == media).ToListAsync();
        }

        public async Task<IEnumerable<Album>> GetAlbumsByUsedOrNewAsync(bool isUsed) {
            return await _context.Albums.Where(a => a.IsUsed).ToListAsync();
        }

        public async Task<IEnumerable<Album>> GetAlbumsByGenreAsync(Constants.ArtistGenre artistGenre) {
            return await _context.Albums.Where(a => a.ArtistGenre == artistGenre).ToListAsync();
        }

        public async Task<IEnumerable<Album>> GetSearchAlbumsAsync(string query) {
            return await _context.Albums.Where(a => a.Title.ToLower().Contains(query.ToLower()) ||
                                               a.ArtistName.ToLower().Contains(query.ToLower()) ||
                                               a.LabelName.ToLower().Contains(query.ToLower()))
                                               .ToListAsync(); ;
        }

        public async Task<IEnumerable<Album>> GetRandomAlbumsAsync(int count, Constants.Media? media = null, bool? isUsed = null) {
            IQueryable<Album> query = _context.Albums;

            if (media.HasValue) {
                query = query.Where(a => a.Media == media && a.Quantity > 0);
            }

            if (isUsed.HasValue) {
                query = query.Where(a => a.IsUsed == isUsed.Value && a.Quantity > 0);
            }

            query = query.OrderBy(a => Guid.NewGuid()).Take(count);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Album>> GetAlbumsByMediaAndConditionAsync(Constants.Media? media = null, bool? isUsed = null) {
            IQueryable<Album> query = _context.Albums;

            if (media.HasValue) {
                query = query.Where(a => a.Media == media);
            }

            if (isUsed.HasValue) {
                query = query.Where(a => a.IsUsed == isUsed.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<Album?> GetAlbumByIdAsync(Guid albumId) {
            return await _context.Albums.Where(a => a.AlbumId == albumId).FirstOrDefaultAsync();
        }

        public async Task<Album?> GetAlbumByArtistNameAndTitleAsync(string artistName, string title) {
            return await _context.Albums.Where(a => a.ArtistName.ToLower() == artistName.ToLower() && a.Title.ToLower() == title.ToLower()).FirstOrDefaultAsync();
        }

        public async Task<Album?> GetAlbumByTitleAsync(string title) {
            return await _context.Albums.Where(a => a.Title.ToLower() == title).FirstOrDefaultAsync();
        }

        // CRUD
        public async Task<Album?> UpdateAlbumAsync(Guid albumId, Album updateData) {
            try {
                var album = await _context.Albums.FirstOrDefaultAsync(a => a.AlbumId == albumId);

                if (album != null) {
                    album.Title = updateData.Title;
                    album.Quantity = updateData.Quantity;
                    album.Price = updateData.Price;
                    album.ImageFilePath = updateData.ImageFilePath;
                    album.Comments = updateData.Comments;
                    // TODO: Add all properties except AlbumId

                    await _context.SaveChangesAsync();

                    return album;
                }
                return null;
            }
            catch (Exception) {
                return null;
            }
        }

        public async Task<Album?> DeleteAlbumAsync(Guid albumId) {
            var album = await _context.Albums.Where(a => a.AlbumId == albumId).FirstOrDefaultAsync();
            if (album != null) {
                _context.Albums.Remove(album);
                await _context.SaveChangesAsync();
                return album;
            }
            else {
                return null;
            }
        }

        public async Task CreateAlbumAsync(Album album) {
            if (album == null) {
                throw new ArgumentNullException(nameof(album), "Provided album cannot be null.");
            }

            _context.Albums.Add(album);
            await _context.SaveChangesAsync();
        }
    }
}
