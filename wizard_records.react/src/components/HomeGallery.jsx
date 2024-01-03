import React, { useState, useEffect } from 'react';
import { API_BASE_URL } from './utils/config';
import ProductList from './ProductList';
import axios from 'axios';
import '../styles/Home.css';
import '../styles/Fonts.css';

const fetchDataForCategory = async (count = 3, media = null, isUsed = null) => {
    const params = {};
    if (media !== null) params.media = media;
    if (isUsed !== null) params.isUsed = isUsed;
    params.count = count;

    const response = await axios.get(`${API_BASE_URL}/album/random`, { params });
    if (response.status === 200) {
        return response.data.map(album => ({
            id: album.albumId,
            cover: album.imageFilePath,
            media: album.media === 0 ? "VinylBase.png" : "CDBase.png",
            artistName: album.artistName,
            albumTitle: album.title,
            price: album.price.toFixed(2),
            isUsed: album.isUsed,
            quantity: album.quantity
        }));
    }
    else {
        throw new Error(`Failed to fetch random album with status: ${response.status}`);
    }
};

function HomeGallery() {
    const [featuredProducts, setFeaturedProducts] = useState([]);
    const [newVinyl, setNewVinyl] = useState([]);
    const [newCDs, setNewCDs] = useState([]);
    const [usedVinyl, setUsedVinyl] = useState([]);
    const [usedCDs, setUsedCDs] = useState([]);

    useEffect(() => {
        fetchDataForCategory(3)
            .then(data => setFeaturedProducts(data));
        fetchDataForCategory(3, 'Vinyl', false)
            .then(data => setNewVinyl(data));
        fetchDataForCategory(3, 'CD', false)
            .then(data => setNewCDs(data));
        fetchDataForCategory(3, 'Vinyl', true)
            .then(data => setUsedVinyl(data));
        fetchDataForCategory(3, 'CD', true)
            .then(data => setUsedCDs(data));
    }, []);

    return (
        <div>
             <hr className="divider" />
            <ProductList title="Featured Products" products={featuredProducts} isHomeGallery={true} />
            <hr className="divider" />
            <ProductList title="New Vinyl" products={newVinyl} isHomeGallery={true} />
            <hr className="divider" />
            <ProductList title="New CDs" products={newCDs} isHomeGallery={true} />
            <hr className="divider" />
            <ProductList title="Used Vinyl" products={usedVinyl} isHomeGallery={true} />
            <hr className="divider" />
            <ProductList title="Used CDs" products={usedCDs} isHomeGallery={true} />
            <hr className="divider" />
        </div>
    );
}

export default HomeGallery;