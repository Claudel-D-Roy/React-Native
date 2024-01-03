import React from 'react';

const FilterMenu = ({
    selectedSortOption,
    handleSortChange,
    selectedGenreFilterOption,
    handleGenreFilterChange,
    selectedFormatFilterOption,
    handleFormatFilterChange,
    selectedCategoryFilterOption,
    handleCategoryFilterChange,
    selectedAvailabilityFilterOption,
    handleAvailabilityFilterChange,
    ArtistGenre
}) => {
    return (
        <div className="dropdown-container">
            <div className="dropdown-group">
                <label htmlFor="dropdown-label">SORTING OPTIONS: </label>
                <select id="dropdown-main" value={selectedSortOption} onChange={handleSortChange}>
                    <option value="default">Default</option>
                    <option value="priceLowToHigh">Price: Low to High</option>
                    <option value="priceHighToLow">Price: High to Low</option>
                    <option value="AlbumNameAsc">Album Name: A..Z</option>
                    <option value="AlbumNameDesc">Album Name: Z..A</option>
                    <option value="ArtistNameAsc">Artist Name: A..Z</option>
                    <option value="ArtistNameDesc">Artist Name: Z..A</option>
                </select>
            </div>

            <div className="dropdown-group">
                <label htmlFor="dropdown-label">FILTER BY GENRE: </label>
                <select id="dropdown-genre" value={selectedGenreFilterOption} onChange={handleGenreFilterChange}>
                    <option value="default">Any genre</option>
                    {ArtistGenre.map((genre) => (
                        <option key={genre.value} value={genre.label.toLowerCase()}>{genre.label}</option>
                    ))}
            </select>
            </div>

            <div className="dropdown-group">
                <label htmlFor="dropdown-label">FILTER BY FORMAT: </label>
                <select id="dropdown-media" value={selectedFormatFilterOption} onChange={handleFormatFilterChange}>
                    <option value="default">All formats</option>
                    <option value="cdOnly">CD only</option>
                    <option value="vinylOnly">Vinyl only</option>
                </select>
            </div>

            <div className="dropdown-group">
                <label htmlFor="dropdown-label">FILTER BY AVAILABILITY: </label>
                <select id="dropdown-availability" value={selectedAvailabilityFilterOption} onChange={handleAvailabilityFilterChange}>
                    <option value="default">All</option>
                    <option value="availableOnly">Available only</option>
                    <option value="unavailableOnly">Unavailable only</option>
                </select>
            </div>

            <div className="dropdown-group">
                <label htmlFor="dropdown-label">FILTER BY CATEGORY: </label>
                <select id="dropdown-condition" value={selectedCategoryFilterOption} onChange={handleCategoryFilterChange}>
                    <option value="default">Any category</option>
                    <option value="newOnly">New only</option>
                    <option value="usedOnly">Used only</option>
                </select>
            </div>
        </div>);
};

export default FilterMenu;