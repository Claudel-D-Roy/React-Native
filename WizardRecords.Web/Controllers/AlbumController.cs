using Microsoft.AspNetCore.Mvc;
using WizardRecords.Repositories;
using WizardRecords.Api.Dtos;
using static WizardRecords.Api.Data.Constants;
using WizardRecords.Api.Domain.Entities;

namespace WizardRecords.Api.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class AlbumController : ControllerBase {
        private readonly IAlbumRepository _albumRepository;

        public AlbumController(IAlbumRepository albumRepository) {
            _albumRepository = albumRepository;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<AlbumDto>>> GetAllAlbums() {
            var albums = await _albumRepository.GetAllAlbumsAsync();
            var albumDtos = AssignPropertiesInCollection(albums);
            return Ok(albumDtos);
        }

        [HttpGet("artist/{artistName}")]
        public async Task<ActionResult<IEnumerable<AlbumDto>>> GetAlbumsByArtistName(string artistName) {
            var albums = await _albumRepository.GetAlbumsByArtistNameAsync(artistName);

            if (albums != null && albums.Any()) {
                var albumDtos = AssignPropertiesInCollection(albums);
                return Ok(albumDtos);
            }
            else {
                return NotFound("No albums found.");
            }
        }

        [HttpGet("label/{labelName}")]
        public async Task<ActionResult<IEnumerable<AlbumDto>>> GetAlbumsByLabelName(string labelName) {
            var albums = await _albumRepository.GetAlbumsByLabelNameAsync(labelName);

            if (albums != null && albums.Any()) {
                var albumDtos = AssignPropertiesInCollection(albums);
                return Ok(albumDtos);
            }
            else {
                return NotFound("No albums found.");
            }
        }

        [HttpGet("media/{media}")]
        public async Task<ActionResult<IEnumerable<AlbumDto>>> GetAlbumsByMedia(Media media) {
            var albums = await _albumRepository.GetAlbumsByMediaAsync(media);

            if (albums != null && albums.Any()) {
                var albumDtos = AssignPropertiesInCollection(albums);
                return Ok(albumDtos);
            }
            else {
                return NotFound("No albums found.");
            }
        }

        [HttpGet("section/{isUsed}")]
        public async Task<ActionResult<IEnumerable<AlbumDto>>> GetAlbumsByUsedOrNew(bool isUsed) {
            var albums = await _albumRepository.GetAlbumsByUsedOrNewAsync(isUsed);

            if (albums != null && albums.Any()) {
                var albumDtos = AssignPropertiesInCollection(albums);
                return Ok(albumDtos);
            }
            else {
                return NotFound("No albums found.");
            }
        }

        [HttpGet("genre/{artistGenre}")]
        public async Task<ActionResult<IEnumerable<AlbumDto>>> GetAlbumsByGenre(ArtistGenre artistGenre) {
            var albums = await _albumRepository.GetAlbumsByGenreAsync(artistGenre);

            if (albums != null && albums.Any()) {
                var albumDtos = AssignPropertiesInCollection(albums);
                return Ok(albumDtos);
            }
            else {
                return NotFound("No albums found.");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<AlbumDto>>> GetSearchAlbums([FromQuery] string query) {
            var albums = await _albumRepository.GetSearchAlbumsAsync(query);

            if (albums != null && albums.Any()) {
                var albumDtos = AssignPropertiesInCollection(albums);
                return Ok(albumDtos);
            }
            else {
                return NotFound("No albums found.");
            }
        }

        [HttpGet("random")]
        public async Task<ActionResult<IEnumerable<AlbumDto>>> GetRandomAlbums([FromQuery] int count = 3, [FromQuery] Media? media = null, [FromQuery] bool? isUsed = null) {
            var albums = await _albumRepository.GetRandomAlbumsAsync(count, media, isUsed);

            if (albums != null && albums.Count() > 0) {
                var albumDtos = albums.Select(a => AssignPropertiesToAlbum(a)).ToList();
                return Ok(albumDtos);
            }
            else {
                return NotFound("Albums not found.");
            }
        }

        [HttpGet("/category")]
        public async Task<ActionResult<IEnumerable<AlbumDto>>> GetAlbumsByMediaAndCondition([FromQuery] Media? media = null, [FromQuery] bool? isUsed = null) {
            var albums = await _albumRepository.GetAlbumsByMediaAndConditionAsync(media, isUsed);

            if (albums != null && albums.Any()) {
                var albumDtos = AssignPropertiesInCollection(albums);
                return Ok(albumDtos);
            }
            else {
                return NotFound("No albums found.");
            }
        }

        [HttpGet("{albumId}")]
        public async Task<ActionResult<AlbumDto>> GetAlbumById(Guid albumId) {
            var album = await _albumRepository.GetAlbumByIdAsync(albumId);

            if (album != null) {
                var albumDto = AssignPropertiesToAlbum(album);
                return Ok(albumDto);
            }
            else {
                return NotFound("Album not found.");
            }
        }

        [HttpGet("title/{title}")]
        public async Task<ActionResult<AlbumDto>> GetAlbumsByTitle(string title) {
            var album = await _albumRepository.GetAlbumByTitleAsync(title);

            if (album != null) {
                var albumDtos = AssignPropertiesToAlbum(album);
                return Ok(albumDtos);
            }
            else {
                return NotFound("No albums found.");
            }
        }

        [HttpGet("artist/{artistName}/title/{title}")]
        public async Task<ActionResult<AlbumDto>> GetAlbumByArtistNameAndTitle(string artistName, string title) {
            var album = await _albumRepository.GetAlbumByArtistNameAndTitleAsync(artistName, title);

            if (album != null) {
                var albumDto = AssignPropertiesToAlbum(album);
                return Ok(albumDto);
            }
            else {
                return NotFound("Album not found.");
            }
        }
        // For CRUD operations, see WizardRecords.Web/Controllers/CRUDController.cs

        // Helper methods
        private IEnumerable<AlbumDto> AssignPropertiesInCollection(IEnumerable<Album>? albums) {
            var albumDtos = albums!.Select(album => new AlbumDto(
                    album!.AlbumId,
                    album.ArtistName,
                    album.Title,
                    album.ArtistGenre,
                    album.AlbumGenre,
                    album.LabelName,
                    album.Price,
                    album.IsUsed,
                    album.Media,
                    album.Quantity,
                    album.ImageFilePath,
                    album.MediaGrade,
                    album.SleeveGrade,
                    album.CatalogNumber,
                    album.MatrixNumber,
                    album.Comments
                )).ToList();

            return albumDtos;
        }

        private AlbumDto AssignPropertiesToAlbum(Album? album) {
            var albumDto = new AlbumDto(
                    album!.AlbumId,
                    album.ArtistName,
                    album.Title,
                    album.ArtistGenre,
                    album.AlbumGenre,
                    album.LabelName,
                    album.Price,
                    album.IsUsed,
                    album.Media,
                    album.Quantity,
                    album.ImageFilePath,
                    album.MediaGrade,
                    album.SleeveGrade,
                    album.CatalogNumber,
                    album.MatrixNumber,
                    album.Comments
                );

            return albumDto;
        }
    }
}
