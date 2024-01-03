import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { API_BASE_URL } from './utils/config';
import ProductList from './ProductList';
import axios from 'axios';

const fetchDataForArtist = (artistName) => {
    return axios.get(`${API_BASE_URL}/album/artist/${artistName}`)
    .then((response) => {
        if (response.status === 200) {
            return response.data.map((album) => ({
                id: album.albumId,
                cover: album.imageFilePath,
                media: album.media === 0 ? "VinylBase.png" : "CDBase.png",
                artistName: album.artistName,
                albumTitle: album.title,
                isUsed: album.isUsed,
                price: album.price.toFixed(2),
                stockQuantity: album.stockQuantity
            }));
        } else {
            throw new Error(`Failed to fetch albums with status: ${response.status}`);
        }
    });
};

function ArtistAlbums() {
    const { artistName } = useParams(); 
    const [ products, setProducts ] = useState([]);
    const [ isLoading, setIsLoading ] = useState(true);
    const [ error, setError ] = useState(null);

    useEffect(() => {
        setIsLoading(true);
        fetchDataForArtist(artistName)
        .then(data => {
            setProducts(data);
            setIsLoading(false);
        })
        .catch(err => {
            console.error('Error fetching albums:', err.message);
            setError(err.message);
            setIsLoading(false);
        });
    }, [artistName]);

    if (isLoading) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div>Error: {error}</div>;
    }

    return (
        <div>
            <h2>Albums by {artistName}</h2>
            <ProductList title="Artist's Albums" products={products} isHomeGallery={false}/>
        </div>
    );
};

export default ArtistAlbums;