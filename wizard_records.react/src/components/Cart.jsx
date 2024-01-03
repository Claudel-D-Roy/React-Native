import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { API_BASE_URL } from './utils/config';
import { jwtDecode as jwt_decode } from 'jwt-decode';
import { useNavigate } from 'react-router-dom';
import '../styles/Cart.css';
import Swal from 'sweetalert2'

const fetchDataForCart = (user) => {
    if (user) {
        return axios.get(`${API_BASE_URL}/cart/user/${user}`)
            .then((response) => {
                if (response.status === 200) {
                    const cart = response.data;

                    const transformedCart = {
                        user: cart.user,
                        cartId: cart.cartId,
                        albums: cart.cartItems.map((item) => ({
                            id: item.album.albumId,
                            cover: item.album.imageFilePath === "" ? "default.webp" : item.album.imageFilePath,
                            media: item.album.media === 0 ? "VinylBase.png" : "CDBase.png",
                            artistName: item.album.artistName,
                            albumTitle: item.album.title,
                            isUsed: item.album.isUsed,
                            price: item.album.price.toFixed(2),
                            quantity: item.quantity
                        }))
                    };


                    return transformedCart;
                } else {
                    throw Error(`Failed to fetch cart with status: ${response.status}`);
                }
            })
            .catch((error) => {
                console.error(error);
                throw error;
            });
    }
};

function calculateTotal(cart) {
    let total = 0;
    cart.map((cart) => (total += cart.price * cart.quantity));
    return total.toFixed(2);
}

function calculateTotalAlbum(album, quantity) {
    let total = 0;
    total = album * quantity;
    return total.toFixed(2);
}

function GetImgageSRC(cart) {
    const coverImg = require(`../assets/images/covers/${cart.cover}`);
    return coverImg;
}

const AddAlbumToCart = async (album, cart, setCart, setTotalPanier) => {
    try {
        const reponse = await axios.post(`${API_BASE_URL}/cart/add/${cart.cartId}/${album}`);
        if (reponse.status === 200) {
            console.log('Album added successfully');

            const updatedCart = { ...cart };
            updatedCart.albums = updatedCart.albums.map((item) => {
                if (item.id === album) {
                    item.quantity = item.quantity + 1;
                }
                return item;
            }).filter((item) => item.quantity !== 0);
            updatedCart.albums = updatedCart.albums.filter((item) => item !== null);
            setCart(updatedCart);
            setTotalPanier(calculateTotal(updatedCart.albums));
        }
    }
    catch (error) { 
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'You cannot add more of this album!',
        });
    }
}

const deleteAlbumFromCart = async (album, cart, setCart, setTotalPanier) => {
    try {
        const response = await axios.delete(`${API_BASE_URL}/cart/delete/${cart.cartId}/${album}`);
        if (response.status === 200) {
            console.log('Album deleted successfully');

            const updatedCart = { ...cart };
            updatedCart.albums = updatedCart.albums.map((item) => {
                if (item.id === album) {
                    item.quantity = item.quantity - 1;
                }
                return item;
            });

            updatedCart.albums = updatedCart.albums.filter((item) => item.quantity !== 0);

            setCart(updatedCart);
            setTotalPanier(calculateTotal(updatedCart.albums));
        } else {
            console.error('Failed to delete album with status:', response.status);
          
        }
    } catch (error) {
        console.error('Error deleting album:', error.message);
    }
};

const SetAlbumInCart = async (event, album, cart, setCart, setTotalPanier) => {
    const value = event.target.value;
    console.log(value);
        try {
            const response = await axios.put(`${API_BASE_URL}/cart/update/${cart.cartId}/${album}`);
            if (response.status === 200) {
                console.log('Album quantity changed successfully');

                const updatedCart = { ...cart };
                updatedCart.albums = updatedCart.albums.map((item) => {
                    if (item.id === album) {
                        if(Number(value) === 0)
                            item.quantity = 1;
                        else
                            item.quantity = Number(value);
                    }
                    return item;
                });

                setCart(updatedCart);
                setTotalPanier(calculateTotal(updatedCart.albums));
            } else {
                console.error('Failed to change album quantity with status:', response.status);
            }
        } catch (error) {
            console.error('Error changing album quantity:', error.message);
    }
    if (Number(value) === 0) {
        console.log("please delete");
        deleteAlbumFromCart(album, cart, setCart, setTotalPanier);
    }
};

function CartPage() {
    const navigate = useNavigate();
    const [user, GetUser] = useState();
    const [role, GetRole] = useState();
    const [cart, setCart] = useState([]);
    const [totalPanier, setTotalPanier] = useState(0);

    //get token
    useEffect(() => {
        var token = sessionStorage.getItem('userToken');
        var tokenGuest = sessionStorage.getItem('guestToken');
        if (token) {
            var decodedToken = jwt_decode(token);
            GetUser(decodedToken["id"]);

        } else if (tokenGuest) {
            var decodedTokenGuest = jwt_decode(tokenGuest);
            GetUser(decodedTokenGuest["id"]);
            GetRole(decodedTokenGuest["role"]);
        }
        else {
            GetUser("Undefined");
        }
    }, []);

    useEffect(() => {
        if (user) {
            fetchDataForCart(user)
                .then((data) => {
                    setCart(data);
                    setTotalPanier(calculateTotal(data.albums));
                })
                .catch((error) => {
                    console.error("Failed to fetch cart data:", error);
                });
        }
    }, [user]);

    const goToCheckout = async () => {
        if (role === "Guest") {
            // Afficher la boîte de dialogue pour le choix de l'utilisateur
            const result = await Swal.fire({
                title: 'Welcome Guest!',
                text: 'Do you want to stay as a Guest, log in, or register?',
                icon: 'question',
                showCancelButton: true,
                confirmButtonText: 'Stay Guest',
                cancelButtonText: 'Log In',
                showCloseButton: true,
                cancelButtonAriaLabel: 'Log In',
                cancelButtonColor: '#3085d6',
                reverseButtons: true,
                // Ajout d'un troisième bouton pour s'enregistrer
                showDenyButton: true,
                denyButtonText: 'Register',
            });

            // Traiter le choix de l'utilisateur
            if (result.isConfirmed) {
                // L'utilisateur a choisi de rester en tant qu'invité
                navigate('/order');
            } else if (result.dismiss === Swal.DismissReason.cancel) {
                // L'utilisateur a choisi de se connecter
                navigate('/account?isLogin=true');
            } else {
                // L'utilisateur a choisi de s'enregistrer
                navigate('/account?isLogin=false');
            }
        } else {
            // Utilisateur non invité, rediriger directement vers /order
            navigate('/order');
        }
    };

    if (cart.albums === undefined) {
        return (
            <div>
                <h2>Your cart</h2>
                <button className="previous-orders-button" onClick={() => navigate("/previous_orders")}>Previous Orders</button>
                <h3>Your cart is empty</h3>
            </div>
        );
    }
    else {
        return (
            <div className="cart-container">
                <h2 className="cart-heading">Your cart</h2>
                <button className="previous-orders-button" onClick={() => navigate("/previous_orders")}>Previous Orders</button>
                {cart.albums ? (
                    <ul className="cart-list">
                        {cart.albums.map((album) => (
                            <li className="cart-item" key={album.id}>
                                <div className="cart-item-image">
                                    <img src={GetImgageSRC(album)} alt={`${cart.album} cover`} />
                                </div>
                                <div className="cart-item-details">
                                    <div>Album name: {album.albumTitle}</div>
                                    <div>Artist: {album.artistName}</div>
                                    <div>
                                        <div className="Quantity-inputs-div">Quantity:
                                            <button id="qi1" className="Quantity-inputs" onClick={() => deleteAlbumFromCart(album.id, cart, setCart, setTotalPanier)}>
                                                -
                                            </button>
                                            <select id="qi2" className="Quantity-inputs" value={Number(album.quantity)} onChange={(e) => SetAlbumInCart(e, album.id, cart, setCart, setTotalPanier)}>
                                                {(() => {
                                                    console.log(album);
                                                    let rows = [];
                                                    for (let i = 0; i <= album.quantity; i++) {
                                                        rows.push(<option key={Number(i)} value={Number(i)}> {Number(i)}</option>);
                                                    }
                                                    return rows;
                                                })()}
                                            </select>
                                            <button id="qi3" className="Quantity-inputs" onClick={() => AddAlbumToCart(album.id, cart, setCart, setTotalPanier)}>
                                                +
                                            </button>
                                        </div>
                                    </div>
                                    <div>Sub-total: {calculateTotalAlbum(album.price, album.quantity)} $</div>
                                    <div>Taxes: {(Math.round(((calculateTotalAlbum(album.price, album.quantity) * 1.15) - calculateTotalAlbum(album.price, album.quantity)) * 100) / 100).toFixed(2)} $</div>
                                    <div>Total: {(Math.round((calculateTotalAlbum(album.price, album.quantity) * 1.15) * 100) / 100).toFixed(2)} $</div>
                                </div>
                            </li>
                        ))}
                    </ul>
                ) : (
                    <div>
                        <h3 className="cart-message">Your cart is empty</h3>
                    </div>
                )}
                <div className="flexDiv">
                    <h3 className="cart-total">Cart total : {totalPanier} $</h3>
                    <button className="checkout-button" onClick={() => goToCheckout()} >Go to checkout</button>
                </div>
            </div>
        );
    }
}

export default CartPage;
