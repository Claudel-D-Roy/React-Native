import React, { useEffect, useState } from 'react';
import { API_BASE_URL } from './utils/config';
import { Link } from 'react-router-dom';
import { useParams, useNavigate } from 'react-router-dom';
import { AlbumGenre, ArtistGenre, Grade } from './utils/constants';
import axios from 'axios';
import '../styles/Detail.css';
import '../styles/Home.css';
import { jwtDecode as jwt_decode } from 'jwt-decode';
import Swal from 'sweetalert2';

const Detail = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const [user, SetUser] = useState([]);
    const [role, setRole] = useState([]);
    const [product, setProduct] = useState(null);
    const [editedProduct, setEditedProduct] = useState({});
    const [isEditing, setIsEditing] = useState(false);
    const [showSuccessPopup, setShowSuccessPopup] = useState(false);
 

    //get token
    useEffect(() => {
        var token = sessionStorage.getItem('userToken');
        var tokenGuest = sessionStorage.getItem('guestToken');
        if(token)
        {
            var decodedToken = jwt_decode(token);
            setRole(decodedToken["role"]);
            SetUser(decodedToken["id"]);
        }
        else if(tokenGuest){
            var decodedTokenGuest = jwt_decode(tokenGuest);
            setRole(decodedTokenGuest["role"]);
            SetUser(decodedTokenGuest["id"]);
        }else{
            console.log(`Personne est logger`);
            SetUser(null);
        }
    }, []);

    const fetchDataForDetail = async  (id) => {
        try {
            const response = await axios.get(`${API_BASE_URL}/album/${id}`);
            if (response.status === 200) {
                const album = response.data;
            
                AlbumGenre.map((genre) => (genre.value === album.albumGenre) && (album.albumGenre = genre.label))
                ArtistGenre.map((genre) => (genre.value === album.artistGenre) && (album.artistGenre = genre.label))
                Grade.map((grade) => (grade.value === album.mediaGrade) && (album.mediaGrade = grade.label))
                Grade.map((grade) => (grade.value === album.sleeveGrade) && (album.sleeveGrade = grade.label))

                let imagePath;
                try {
                    imagePath = require(`../assets/images/covers/${album.imageFilePath}`);
                } catch (error) {
                    console.error(`Error requiring image file for ${album.imageFilePath}`, error);
                    imagePath = require('../assets/images/covers/default.webp');
                }
              
                const productData = {
                    albumId: album.albumId,
                    imageFilePath: imagePath,
                    albumLabel: album.labelName,
                    media: album.media === 0 ? 'Vinyl' : 'CD',
                    artistName: album.artistName,
                    albumTitle: album.title,
                    isUsed: album.isUsed === false ? 'New' : 'Used',
                    price: album.price.toFixed(2),
                    quantity: album.quantity,
                    mediaGrade : formatGrade(album.mediaGrade),
                    sleeveGrade : formatGrade(album.sleeveGrade),
                    catalogNumber : album.catalogNumber === '' || album.catalogNumber === 'None' ? 'None' : album.catalogNumber,
                    matrixNumber : album.matrixNumber === '' || album.matrixNumber === 'None' ? 'None' : album.matrixNumber,
                    artistGenre : album.artistGenre,
                    albumGenre  : album.albumGenre,
                   
                };

                setProduct(productData);

            } else {
                throw new Error(`Failed to fetch album with status: ${response.status}`);
            }
        } catch (error) {
            console.error('Error:', error.message);
        }
    };

    const deleteProduct = async () => {
        try {
            const response = await axios.delete(`${API_BASE_URL}/crud/delete/${product.albumId}`);
            if (response.status === 200) {
                console.log('Album deleted successfully');
                navigate('/products');
            } else {
                console.error('Failed to delete album with status:', response.status);
            }
        } catch (error) {
            console.error('Error deleting album:', error);
        }
    };

    const editProduct = () => {
        setEditedProduct(product);
        setIsEditing(true);
    };

    const updateProduct = async () => {
        try {
            console.log(editedProduct);
            const albumUpdate = {
                Title: editedProduct.albumTitle,
                Quantity: editedProduct.quantity,
                Price: editedProduct.price,
                AlbumGenre: editedProduct.albumGenre,
                ArtistGenre: editedProduct.artistGenre,
                MediaGrade: editedProduct.mediaGrade,
                SleeveGrade: editedProduct.sleeveGrade,
                CatalogNumber: editedProduct.catalogNumber,
                MatrixNumber: editedProduct.matrixNumber,
                ArtistName: editedProduct.artistName,
                LabelName: editedProduct.albumLabel,
                IsUsed: editedProduct.isUsed,
                Media: editedProduct.media
            };

            console.log(albumUpdate);

            if(albumUpdate.Price < 0)
            {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'The price must be greater than 0!',
                });
                return;
            }

            if(albumUpdate.Quantity < 0)
            {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'The quantity must be greater than 0!',
                });
                return;
            }


            const response = await axios.put(`${API_BASE_URL}/crud/update/${product.albumId}`, albumUpdate, {
                headers: {
                    'Content-Type': 'application/json',
                },
            });
            if (response.status === 200) {
                console.log('Album updated successfully');
                setIsEditing(false);
                fetchDataForDetail(id);
            } else {
                console.error('Failed to update album with status:', response.status);
            }
        } catch (error) {
            console.error('Error updating album:', error);
        }
    };

    const AddToCart = async () => {
        try {   
            const cart = {
                albumId: product.albumId,
                cartId: 0
            };
                        
           //Faire une comparaison pour le userId = user du token          
            
            console.log(user);

           if(user !== null){
             //Creation panier
             const creationPanier = await axios.post(`${API_BASE_URL}/cart/createpanier/${user}`, cart, {
            headers: {
             'Content-Type': 'application/json',
            },
            });
            if(creationPanier.status === 200)
            {
                console.log('Cart Creer');
                cart.cartId = creationPanier.data.cartId;
            }
            else {
                console.error('Echec de creation du panier avec le statut:', creationPanier.status);
            }

            //Album
            const addToCart = await axios.post(`${API_BASE_URL}/cart/add/${cart.cartId}/${cart.albumId}`, cart, {
            headers: {
                'Content-Type': 'application/json',
            },
            });
            if (addToCart.status === 200) {
            console.log('Album added to cart successfully');
            setShowSuccessPopup(true);
            Swal.fire({
                icon: 'success',
                title: 'Item Added to Cart!',
                text: 'The item has been added to your cart successfully.',
                
            }).then(() => {
                setShowSuccessPopup(false); // Réactive le bouton après la fermeture du pop-up
            });

            }
            else {
            console.error('Failed to add album to cart with status:', addToCart.status);
            }
         } 
         else {

            //CREATION USER
            const creationUser = await axios.post(`${API_BASE_URL}/cart/create`, cart, {
                headers: {
                   'Content-Type': 'application/json',
               },
           });
           if (creationUser.status === 200) {
           console.log('User Creer');

            sessionStorage.setItem('guestToken', JSON.stringify(creationUser.data.token));
             var token = sessionStorage.getItem('guestToken');
            var decodedToken = jwt_decode(token);
            cart.userId = decodedToken["id"];
            SetUser(decodedToken["id"]);
            console.log(user);

            }
           else {
           console.error('Echec de creation du user avec le statut:', creationUser.status);
           }


          
           const creationPanier = await axios.post(`${API_BASE_URL}/cart/createpanier/${cart.userId}`, cart, {
            headers: {
             'Content-Type': 'application/json',
            },
            });
            if(creationPanier.status === 200)
            {
                console.log('Cart Creer');
                cart.cartId = creationPanier.data.cartId;
            }
            else {
                console.error('Echec de creation du panier avec le statut:', creationPanier.status);
            }

            //Album
            const addToCart = await axios.post(`${API_BASE_URL}/cart/add/${cart.cartId}/${cart.albumId}`, cart, {
            headers: {
                'Content-Type': 'application/json',
            },
            });
            if (addToCart.status === 200) {
            console.log('Album added to cart successfully');
            setShowSuccessPopup(true);
            Swal.fire({
                icon: 'success',
                title: 'Item Added to Cart!',
                text: 'The item has been added to your cart successfully.',
                
            }).then(() => {
                setShowSuccessPopup(false); // Réactive le bouton après la fermeture du pop-up
            });

            }
            else {
            console.error('Failed to add album to cart with status:', addToCart.status);
            }



         }          
             }
             catch (error) {
                console.error('Error adding album to cart:', error);
            }

            
    };    

    useEffect(() => {
        fetchDataForDetail(id);
    }, [id]);

    if (!product) {
        return <div>Loading product details...</div>;
    }


    return (
        <div className="detail-container">
            <div className="detail-image">
            <img src={product?.imageFilePath || 'default.webp'}
                 alt={`${product?.albumTitle || 'Default'} cover`} />
            </div>
            <div className="detail-content">
                {isEditing ? (
                    <div className="input-container" >

                       
                        <label htmlFor="albumTitle" className="detail-label">Album Title:</label>
                        <input
                            type="text"
                            className="detail-input"
                            value={editedProduct.albumTitle}
                            onChange={(e) => setEditedProduct({ ...editedProduct, albumTitle: e.target.value })}
                        />
                        <label htmlFor="quantity" className="detail-label">Quantity:</label>
                        <input
                            type="number"
                            className="detail-input"
                            value={editedProduct.quantity}
                            onChange={(e) => setEditedProduct({ ...editedProduct, quantity: e.target.value })}
                        />
                        <label htmlFor="price" className="detail-label">Price:</label>
                        <input
                            type="number"
                            className="detail-input"
                            value={editedProduct.price}
                            onChange={(e) => setEditedProduct({ ...editedProduct, price: e.target.value })}
                        />
                        <label htmlFor="albumGenre" className="detail-label">Album Genre:</label>
                         <input
                            type="text"
                            className="detail-input"
                            value={editedProduct.albumGenre}
                            onChange={(e) => setEditedProduct({ ...editedProduct, albumGenre: e.target.value })}
                        />
                        <label htmlFor="artistGenre" className="detail-label">Artist Genre:</label>
                         <input
                            type="text"
                            className="detail-input"
                            value={editedProduct.artistGenre}
                            onChange={(e) => setEditedProduct({ ...editedProduct, artistGenre: e.target.value })}
                        />
                        <label htmlFor="mediaGrade" className="detail-label">Media Grade:</label>
                        <input
                            type="text"
                            className="detail-input"
                            value={editedProduct.mediaGrade}
                            onChange={(e) => setEditedProduct({ ...editedProduct, mediaGrade: e.target.value })}
                        />
                       

               

                        <label htmlFor="sleeveGrade" className="detail-label">Sleeve Grade:</label>
                        <input
                            type="text"
                            className="detail-input"
                            value={editedProduct.sleeveGrade}
                            onChange={(e) => setEditedProduct({ ...editedProduct, sleeveGrade: e.target.value })}
                        />
                        <label htmlFor="catalogNumber" className="detail-label">Catalog Number:</label>
                        <input
                            type="text"
                            className="detail-input"
                            value={editedProduct.catalogNumber}
                            onChange={(e) => setEditedProduct({ ...editedProduct, catalogNumber: e.target.value })}
                        />
                        <label htmlFor="albumTitle" className="detail-label">Album Title:</label>
                        <input
                            type="text"
                            className="detail-input"
                            value={editedProduct.MatrixNumber}
                            onChange={(e) => setEditedProduct({ ...editedProduct, MatrixNumber: e.target.value })}
                        />
                        <label htmlFor="MatrixNumber" className="detail-label">Matrix Number:</label>
                        <input
                            type="text"
                            className="detail-input"
                            value={editedProduct.artistName}
                            onChange={(e) => setEditedProduct({ ...editedProduct, artistName: e.target.value })}
                        />
                        <label htmlFor="albumLabel" className="detail-label">Album Label:</label>
                        <input
                            type="text"
                            className="detail-input"
                            value={editedProduct.albumLabel}
                            onChange={(e) => setEditedProduct({ ...editedProduct, albumLabel: e.target.value })}
                        />
                        <label htmlFor="isUsed" className="detail-label">is Used?:</label>
                          <input
                            type="text"
                            className="detail-input"
                            value={editedProduct.isUsed}
                            onChange={(e) => setEditedProduct({ ...editedProduct, isUsed: e.target.value })}
                        />
                        <label htmlFor="Media" className="detail-label">Media:</label>
                          <input
                            type="text"
                            className="detail-input"
                            value={editedProduct.Media}
                            onChange={(e) => setEditedProduct({ ...editedProduct, Media: e.target.value })}
                        />
                        
                      
                       
                        <div className="save-cancel-container">
                            <button className="button-save" onClick={updateProduct}>Save</button>
                            <button className="button-cancel" onClick={() => setIsEditing(false)}>Cancel</button>
                        </div>
                    </div>
                ) : (
                    <div>
                        <Link to={`/artist/${product.artistName}`}><h1 className='artist-link'>{product.artistName}</h1></Link>
                        <h2 className="title-label-container">"{product.albumTitle}" ({product.albumLabel})</h2>
                        <div className="detail-information">
                            <br />
                            <p><i>Section</i> : {product.isUsed} {product.media}
                            <br />
                            <i>Genre</i> : {product.artistGenre} / {product.albumGenre}
                            <br />
                            <i>Media Condition</i> : {product.mediaGrade}
                            <br />
                            <i>Sleeve Condition</i> : {product.sleeveGrade} 
                            <br />
                            <i>Catalog Number</i> : {product.catalogNumber} 
                            <br />
                            <i>Matrix Number</i> : {product.matrixNumber}
                            <br />
                            <i>Availability</i> : <b>{product.quantity > 0 ? `There ${product.quantity === 1 ? 'is' : 'are'} currently ${product.quantity} copie${product.quantity === 1 ? '' : 's'} available` : 'Sorry! This item is currently OUT OF STOCK.'}</b></p>
                        </div>
                        {role === "Administrator" &&
                        <div className="edit-delete-container">
                            <button className="button-edit" onClick={editProduct}>Edit</button>
                            <button className="button-delete" onClick={deleteProduct}>Delete</button>
                        </div>
                        }
                    </div>
                )}
                <div className="price-cart-container">
                    <p className="detail-price">${product.price}</p>
                    <button className={`button-cart ${product.quantity === 0 ? 'button-unavailable' : ''}`} onClick={AddToCart} disabled={showSuccessPopup}>{product.quantity === 0 ? 'Out of stock' : 'Add to cart'}</button>
               
                </div>
            </div>
    </div>
    );
};

const formatGrade = (grade) => {
    const gradeMap = {
        'VG_PLUS' : 'VG+',
        'G_PLUS!' : 'G+!',
    };
    return gradeMap[grade] || grade;
}

export default Detail;