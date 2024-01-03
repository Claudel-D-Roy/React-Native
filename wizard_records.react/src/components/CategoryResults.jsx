import React, { useState, useEffect } from 'react';
import { useLocation } from 'react-router-dom';
import ProductList from './ProductList';
import { API_BASE_URL } from './utils/config';
import axios from 'axios';

const fetchDataForCategory = (media, isUsed) => {
    const queryParameters = new URLSearchParams();
    if (media !== null) {
        queryParameters.append('media', media);
    }
    if (isUsed !== null) {
        queryParameters.append('isUsed', isUsed);
    }
    const queryString = queryParameters.toString();
    
    return axios.get(`${API_BASE_URL}/category?${queryString}`)
        .then((response) => {
            if (response.status === 200) {
                console.log("Received data from server:", response.data);
                const albums = response.data;
                return albums.map((album) => {
                    return {
                        id: album.albumId,
                        cover: album.imageFilePath,
                        media: album.media === 0 ? "VinylBase.png" : "CDBase.png",
                        artistName: album.artistName,
                        albumTitle: album.title,
                        isUsed: album.isUsed,
                        price: album.price.toFixed(2),
                        quantity: album.quantity
                    };
                });
            } else {
                throw new Error(`Failed to fetch products with status: ${response.status}`);
            }
        });
};

function CategoryResults() {
    const location = useLocation();
    const queryParams = new URLSearchParams(location.search);
    const media = queryParams.get('media');
    const isUsed = queryParams.get('isUsed');

    let mediaLabel = "";
    if (media === 'Vinyl') {
        mediaLabel = 'vinyl';
    } else if (media === 'CD') {
        mediaLabel = 'CDs';
    } else {
        mediaLabel = 'unspecified';
    }

    const categoryLabel = isUsed === 'true' ? 'used' : 'new';
    const titleString = `Results for ${categoryLabel} ${mediaLabel}`;

    const [products, setProducts] = useState([]);

    useEffect(() => {
        fetchDataForCategory(media, isUsed)
        .then(data => {
            console.log("Setting Products:", data);
            setProducts(data);
        })
            .catch(error => {
                console.error('Error fetching products:', error.message);
            });
    }, [media, isUsed]);

    return (
        <div>
            <h3>{titleString}</h3>
            <ProductList title={`Category Results`} products={products} isHomeGallery={false} />
        </div>
    );
}

export default CategoryResults;