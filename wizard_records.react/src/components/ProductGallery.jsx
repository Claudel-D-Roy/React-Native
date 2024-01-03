import React, { useState, useEffect } from 'react';
import { API_BASE_URL } from './utils/config';
import { ArtistGenre } from './utils/constants';
import FilterMenu from './FilterMenu';
import Pagination from './Pagination';
import ProductList from './ProductList';
import axios from 'axios';
import '../styles/ProductGallery.css';
import { jwtDecode as jwt_decode } from 'jwt-decode';
import { Link } from 'react-router-dom';
import { useNavigate } from 'react-router-dom';

const fetchDataForCategory = () => {
    return axios.get(`${API_BASE_URL}/album/all`)
        .then((response) => {
            if (response.status === 200) {
                const albums = response.data;
                const albumPromises = albums.map((album) => {
                    return {
                        id: album.albumId,
                        cover: album.imageFilePath === "" ? "default.webp" : album.imageFilePath,
                        media: album.media === 0 ? "VinylBase.png" : "CDBase.png",
                        artistName: album.artistName,
                        artistGenre: album.artistGenre,
                        albumTitle: album.title,
                        isUsed: album.isUsed,
                        price: album.price.toFixed(2),
                        quantity: album.quantity
                    };
                });
                return Promise.all(albumPromises);

            } else {
                throw Error(`Failed to fetch albums with status: ${response.status}`);
            }
        });
};

function ProductGallery() {
    const navigate = useNavigate();
    const [role, setRole] = useState([]);
    const [allProducts, setAllProducts] = useState([]);
    const [products, setProducts] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [selectedSortOption, setSelectedSortOption] = useState('default'); // sort
    const [selectedGenreFilterOption, setSelectedGenreFilterOption] = useState('any genre'); //rock ou wtv
    const [selectedFormatFilterOption, setSelectedFormatFilterOption] = useState('all formats'); //cd ou vynil media
    const [selectedCategoryFilterOption, setSelectedCategoryFilterOption] = useState('any category'); //new ou used category
    const [selectedAvailabilityFilterOption, setSelectedAvailabilityFilterOption] = useState('all'); //disponible ou pas

    //get token
    useEffect(() => {
        var token = sessionStorage.getItem('userToken');
        if(token)
        {
            var decodedToken = jwt_decode(token);
            setRole(decodedToken["role"]);
        }
    }, []);
    
    const handleSortChange = (event) => {
        setSelectedSortOption(event.target.value);
        setCurrentPage(1);
        updateUrl(selectedCategoryFilterOption, selectedFormatFilterOption, event.target.value, selectedGenreFilterOption, selectedAvailabilityFilterOption, selectedAvailabilityFilterOption);
    }

    useEffect(() => {
        fetchDataForCategory().then((data) => {
            setAllProducts(data); 


            const urlSearchParams = new URLSearchParams(window.location.search);
            const categoryParam = urlSearchParams.get('category');
            const mediaParam = urlSearchParams.get('media');
            const sortParam = urlSearchParams.get('sort');
            const genreParam = urlSearchParams.get('genre');
            const availableParam = urlSearchParams.get('available');
            
            if (sortParam && sortParam !== 'default') {
                setSelectedSortOption(sortParam);
            } else {
                setSelectedSortOption('default');
            }

             if (categoryParam && categoryParam !== 'any category') {

            if(categoryParam === 'Featured')
            {
                setSelectedCategoryFilterOption('newOnly');
            }
             else if(categoryParam === 'New' || categoryParam === 'newOnly'){
                setSelectedCategoryFilterOption('newOnly');
             }
            else if(categoryParam === 'Used' || categoryParam === 'usedOnly'){
                setSelectedCategoryFilterOption('usedOnly');
             }
            } else {
                setSelectedCategoryFilterOption('any category');
            }
        if (mediaParam && mediaParam !== 'all formats') {
            if(mediaParam === 'Products'){
                setSelectedFormatFilterOption('cdOnly');
            }
            else if(mediaParam === 'Vinyl' || mediaParam === 'vinylOnly'){
                 setSelectedFormatFilterOption('vinylOnly');
            }
            else if(mediaParam === 'CDs' || mediaParam === 'cdOnly'){
                setSelectedFormatFilterOption('cdOnly');
            }

         if(genreParam && genreParam !== 'any genre'){
             setSelectedGenreFilterOption(genreParam);
         } else {
                setSelectedGenreFilterOption('any genre');
            }
            if(availableParam && availableParam !== 'all'){
                setSelectedAvailabilityFilterOption(availableParam);
            } else {
                setSelectedAvailabilityFilterOption('all');
            }
      
        }
        setProducts(data);  
        });
      }, []);

      useEffect(() => {
        // Effect to handle URL changes (e.g., browser back button)
        const handleUrlChange = () => {
            const urlSearchParams = new URLSearchParams(window.location.search);
            const categoryParam = urlSearchParams.get('category');
            const mediaParam = urlSearchParams.get('media');
            const sortParam = urlSearchParams.get('sort');
            const genreParam = urlSearchParams.get('genre');
            const availableParam = urlSearchParams.get('available');

            if(genreParam && genreParam !== 'any genre'){
                setSelectedGenreFilterOption(genreParam);
            }
            else{
                setSelectedGenreFilterOption('any genre');
            }
            if(availableParam && availableParam !== 'all'){
                setSelectedAvailabilityFilterOption(availableParam);
            }
            else{
                setSelectedAvailabilityFilterOption('all');
            }
            if (categoryParam && categoryParam !== 'any category') {
                setSelectedCategoryFilterOption(categoryParam);
            } else {
                setSelectedCategoryFilterOption('any category');
            }

            if (mediaParam && mediaParam !== 'all formats') {
                setSelectedFormatFilterOption(mediaParam);
            } else {
                setSelectedFormatFilterOption('all formats');
            }

            if (sortParam && sortParam !== 'default') {
                setSelectedSortOption(sortParam);
            } else {
                setSelectedSortOption('default');
            }
        };

        window.addEventListener('popstate', handleUrlChange);

        return () => {
            window.removeEventListener('popstate', handleUrlChange);
        };
    }, []);


   
    // Filters
    let filteredProducts = [...allProducts];

    // Genre filter
    if (selectedGenreFilterOption === 'rock') {
        filteredProducts = filteredProducts.filter((product) => product.artistGenre === 0);
    } else if (selectedGenreFilterOption === 'pop') {
        filteredProducts = filteredProducts.filter((product) => product.artistGenre === 1);
    } else if (selectedGenreFilterOption === 'jazz') {
        filteredProducts = filteredProducts.filter((product) => product.artistGenre === 2);
    } else if (selectedGenreFilterOption === 'hiphop') {
        filteredProducts = filteredProducts.filter((product) => product.artistGenre === 3);
    } else if (selectedGenreFilterOption === 'alternative') {
        filteredProducts = filteredProducts.filter((product) => product.artistGenre === 4);
    } else if (selectedGenreFilterOption === 'classical') {
        filteredProducts = filteredProducts.filter((product) => product.artistGenre === 5);
    } else if (selectedGenreFilterOption === 'francophone') {
        filteredProducts = filteredProducts.filter((product) => product.artistGenre === 6);
    } else if (selectedGenreFilterOption === 'metal') {
        filteredProducts = filteredProducts.filter((product) => product.artistGenre === 7);
    } else if (selectedGenreFilterOption === 'punk') {
        filteredProducts = filteredProducts.filter((product) => product.artistGenre === 8);
    } else if (selectedGenreFilterOption === 'blues') {
        filteredProducts = filteredProducts.filter((product) => product.artistGenre === 9);
    } else if (selectedGenreFilterOption === 'world') {
        filteredProducts = filteredProducts.filter((product) => product.artistGenre === 10);
    } else if (selectedGenreFilterOption === 'folk') {
        filteredProducts = filteredProducts.filter((product) => product.artistGenre === 11);
    }else if (selectedGenreFilterOption === 'country') {
        filteredProducts = filteredProducts.filter((product) => product.artistGenre === 12);
    } else if (selectedGenreFilterOption === 'soul') {
        filteredProducts = filteredProducts.filter((product) => product.artistGenre === 13);
    } else if (selectedGenreFilterOption === 'funk') {
        filteredProducts = filteredProducts.filter((product) => product.artistGenre === 14);
    } else if (selectedGenreFilterOption === 'electronica') {
        filteredProducts = filteredProducts.filter((product) => product.artistGenre === 15);
    } else if (selectedGenreFilterOption === 'soundtrack') {
        filteredProducts = filteredProducts.filter((product) => product.artistGenre === 16);
    }

    // Format filter
    if (selectedFormatFilterOption === 'cdOnly') {
        filteredProducts = filteredProducts.filter((product) => product.media === 'CDBase.png');
    } else if (selectedFormatFilterOption === 'vinylOnly') {
        filteredProducts = filteredProducts.filter((product) => product.media === 'VinylBase.png');
    }

    // Category filter
    if (selectedCategoryFilterOption === 'newOnly') {
        filteredProducts = filteredProducts.filter((product) => product.isUsed === false);
    } else if (selectedCategoryFilterOption === 'usedOnly') {
        filteredProducts = filteredProducts.filter((product) => product.isUsed === true);
    }

    // Availability filter
    if (selectedAvailabilityFilterOption === 'availableOnly') {
        filteredProducts = filteredProducts.filter((product) => product.quantity > 0);
    } else if (selectedAvailabilityFilterOption === 'unavailableOnly') {
        filteredProducts = filteredProducts.filter((product) => product.quantity === 0);
    }

    // Sorting options
    const sortedProducts = [...filteredProducts];
    if (selectedSortOption === 'priceLowToHigh') {
        sortedProducts.sort((a, b) => a.price - b.price);
    } else if (selectedSortOption === 'priceHighToLow') {
        sortedProducts.sort((a, b) => b.price - a.price);
    } else if (selectedSortOption === 'AlbumNameAsc') {
        sortedProducts.sort((a, b) => a.albumTitle.localeCompare(b.albumTitle));
    } else if (selectedSortOption === 'AlbumNameDesc') {
        sortedProducts.sort((a, b) => -1 * a.albumTitle.localeCompare(b.albumTitle));
    } else if (selectedSortOption === 'ArtistNameAsc') {
        sortedProducts.sort((a, b) => a.artistName.localeCompare(b.artistName));
    } else if (selectedSortOption === 'ArtistNameDesc') {
        sortedProducts.sort((a, b) => -1 * a.artistName.localeCompare(b.artistName));
    }

    const productsPerPage = 12;


    //GENRE
    const handleGenreFilterChange = (event) => {
        console.log("handle_genre: :" + event.target.value);
        setSelectedGenreFilterOption(event.target.value);
        updateUrl(selectedCategoryFilterOption, selectedFormatFilterOption, selectedSortOption,  event.target.value, selectedAvailabilityFilterOption);
        setProducts(filteredProducts);
        setCurrentPage(1);
    };

    const handleFormatFilterChange = (event) => {
        console.log("handle_type: :" + event.target.value);
        setSelectedFormatFilterOption(event.target.value);
        updateUrl(selectedCategoryFilterOption, event.target.value,selectedSortOption, selectedGenreFilterOption, selectedAvailabilityFilterOption);
        setProducts(filteredProducts);
        setCurrentPage(1);
    };

    const handleCategoryFilterChange = (event) => {
        console.log("handle_category: :" + event.target.value);
        setSelectedCategoryFilterOption(event.target.value);
        updateUrl(event.target.value, selectedFormatFilterOption,selectedSortOption,  selectedGenreFilterOption, selectedAvailabilityFilterOption);
        setProducts(filteredProducts);
        setCurrentPage(1);
    };

    const handleAvailabilityFilterChange = (event) => {
        console.log("handle_availability: :" + event.target.value);
        updateUrl(selectedCategoryFilterOption, selectedFormatFilterOption ,selectedSortOption, selectedGenreFilterOption, event.target.value);
        setSelectedAvailabilityFilterOption(event.target.value);
        setProducts(filteredProducts);
        setCurrentPage(1);
    };

    const nextPage = () => {
        const totalSortedPages = Math.ceil(products.length / productsPerPage);
    if (currentPage < totalSortedPages) {
        setCurrentPage(currentPage + 1);
    }
    };

    const prevPage = () => {
        if (currentPage > 1) { setCurrentPage(currentPage - 1); }
    };


    const indexOfLastProduct = currentPage * productsPerPage;
    const indexOfFirstProduct = indexOfLastProduct - productsPerPage;
    const currentProducts = sortedProducts.slice(indexOfFirstProduct, indexOfLastProduct);

    const updateUrl = (category, media, sort, genre, available) => {
        const urlParams = new URLSearchParams();

        if (category !== 'any category') {
            urlParams.set('category', category);
        }

        if (media !== 'all formats') {
            urlParams.set('media', media);
        }
        if (sort !== 'default') {
            urlParams.set('sort', sort);
        }
        if(genre !== 'any genre'){
            urlParams.set('genre', genre);
        }
        if(available !== 'all'){
            urlParams.set('available', available);
        }

        navigate(`?${urlParams.toString()}`);
    };

    return (
        <div>
            <div className="entete-allProducts">
                <h1>All products</h1>
                <div>
                    {role === "Administrator" &&
                        <Link className="text-light" to="/add-product"><button id='add-item'>Add Item</button></Link>
                    }
                </div>
            </div>
               
            
            <FilterMenu
                selectedSortOption={selectedSortOption}
                handleSortChange={handleSortChange}
                selectedGenreFilterOption={selectedGenreFilterOption}
                handleGenreFilterChange={handleGenreFilterChange}
                selectedFormatFilterOption={selectedFormatFilterOption}
                handleFormatFilterChange={handleFormatFilterChange}
                selectedCategoryFilterOption={selectedCategoryFilterOption}
                handleCategoryFilterChange={handleCategoryFilterChange}
                selectedAvailabilityFilterOption={selectedAvailabilityFilterOption}
                handleAvailabilityFilterChange={handleAvailabilityFilterChange}
                ArtistGenre={ArtistGenre}
            />
            {currentProducts.length > 0 ? (
            <div>
                <ProductList title="All products" products={currentProducts} isHomeGallery={false}/>
                    {Math.ceil(sortedProducts.length / productsPerPage) > 1 && (
                        <Pagination
                            currentPage={currentPage}
                            totalPages={Math.ceil(sortedProducts.length / productsPerPage)}
                            setCurrentPage={setCurrentPage}
                            prevPage={prevPage}
                            nextPage={nextPage}
                        />
                    )}
            </div>) : (
                <div className="no-results">
                    <h1>No matching results!</h1>
                </div>
            )}
        </div>
    );
}

export default ProductGallery;