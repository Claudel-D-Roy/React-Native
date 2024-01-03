import React, { useState, useEffect } from 'react';
import { useLocation } from 'react-router-dom';
import ProductList from './ProductList';
import { API_BASE_URL } from './utils/config';
import axios from 'axios';

const fetchDataForCategory = (searchQuery) => {
    return axios.get(`${API_BASE_URL}/album/search?query=${searchQuery}`)
    .then((response) => {
        if (response.status === 200) {
            const albums = response.data;

           

            const albumPromises = albums.map((album) => {
                return {
                    id: album.albumId,
                    cover: album.imageFilePath === "" ? "default.webp" : album.imageFilePath,
                    media: album.media === 0 ? "VinylBase.png" : "CDBase.png",
                    artistName: album.artistName,
                    albumTitle: album.title,
                    isUsed: album.isUsed,
                    price: album.price.toFixed(2),
                    stockQuantity: album.stockQuantity
                };
            });

            return Promise.all(albumPromises);
        } else {
            throw new Error(`Failed to fetch albums with status: ${response.status}`);
        }
    });
};

function SearchResults() {
    const searchQuery = new URLSearchParams(useLocation().search).get('query');
    const [products, setProducts] = useState([]);

    useEffect(() => {
        fetchDataForCategory(searchQuery)
            .then(data => setProducts(data))
            .catch(error => {
                console.error('Error fetching albums:', error.message);
            });
    }, [searchQuery]);

    return (
        <div>
            <h3>Search results for "{searchQuery}"</h3>
            <ProductList title={`Search results for "${searchQuery}"`} products={products} isHomeGallery={false}/>
        </div>
    );
}

export default SearchResults;