using Microsoft.AspNetCore.Mvc;
using WizardRecords.Api.Domain.Entities;
using WizardRecords.Repositories;
using WizardRecords.Api.Dtos;

namespace WizardRecords.Api.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class CRUDController : ControllerBase {
        private readonly IAlbumRepository _albumRepository;

        public CRUDController(IAlbumRepository albumRepository) {
            _albumRepository = albumRepository;
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<AlbumUpdate>> UpdateAlbum(Guid id, [FromBody] AlbumUpdate updatedAlbum) {
            try {
                var album = await _albumRepository.GetAlbumByIdAsync(id);
                if (album != null) {
                    album.Title = updatedAlbum.Title;
                    album.Quantity = updatedAlbum.Quantity;
                    album.Price = updatedAlbum.Price;
                  
                    await _albumRepository.UpdateAlbumAsync(id, album);
                    return Ok();
                }
                else {
                    return NotFound();
                }
            }
            catch (Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<AlbumDelete>> DeleteAlbum(Guid id) {
            try {
                var album = await _albumRepository.GetAlbumByIdAsync(id);
                if (album != null) {
                    await _albumRepository.DeleteAlbumAsync(id);
                    return Ok();
                }
                else {
                    return NotFound();
                }
            }
            catch (Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<AlbumCreate>> CreateAlbum([FromBody] AlbumCreate newAlbum) {
            try {
                var existingAlbum = await _albumRepository.GetAlbumByArtistNameAndTitleAsync(newAlbum.ArtistName, newAlbum.Title);
                if (existingAlbum == null || existingAlbum.ArtistName != newAlbum.ArtistName) {
                    // Map AlbumCreate DTO to Album entity
                    var albumToCreate = new Album {
                        AlbumId = Guid.NewGuid(),
                        ArtistName = newAlbum.ArtistName,
                        Title = newAlbum.Title,
                        ArtistGenre = newAlbum.ArtistGenre,
                        AlbumGenre = newAlbum.AlbumGenre,
                        LabelName = newAlbum.LabelName,
                        Price = newAlbum.Price,
                        IsUsed = newAlbum.IsUsed,
                        Media = newAlbum.Media,
                        Quantity = newAlbum.Quantity,
                        ImageFilePath = newAlbum.ImageFilePath,
                        MediaGrade = newAlbum.MediaGrade,
                        SleeveGrade = newAlbum.SleeveGrade,
                        CatalogNumber = newAlbum.CatalogNumber,
                        MatrixNumber = newAlbum.MatrixNumber,
                        Comments = newAlbum.Comments
                    };

                    await _albumRepository.CreateAlbumAsync(albumToCreate);
                    return Ok();
                }
                else {
                    return BadRequest("Album already exists.");
                }
            }
            catch (Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
    }
}
