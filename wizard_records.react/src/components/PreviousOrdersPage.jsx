import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { API_BASE_URL } from './utils/config';
import { jwtDecode as jwt_decode } from 'jwt-decode';
import { useNavigate } from 'react-router-dom';
import '../styles/Cart.css';
import '../styles/PreviousOrderPage.css';

const OrderState = {
    0: 'Confirmed',
    1: 'Paid',
    2: 'Canceled',
    3: 'In Preparation',
    4: 'In Delivery',
    5: 'Delivered',
    6: 'Returned'
};

const fetchPreviousOrders = (userId, role) => {
    if (role === "Administrator") {
        return axios.get(`${API_BASE_URL}/order/orders/all`)
            .then((response) => {
                if (response.status === 200) {
                    return response.data;
                } else {
                    throw Error(`Failed to fetch all orders with status: ${response.status}`);
                }
            })
            .catch((error) => {
                console.error(error);
                throw error;
            });
    } else {
        return axios.get(`${API_BASE_URL}/order/orders/user/${userId}`)
            .then((response) => {
                if (response.status === 200) {
                    return response.data;
                } else {
                    throw Error(`Failed to fetch previous orders with status: ${response.status}`);
                }
            })
            .catch((error) => {
                console.error(error);
                throw error;
            });
    }
};

function PreviousOrdersPage() {
    const navigate = useNavigate();
    const [user, setUser] = useState();
    const [role, setRole] = useState();
    const [previousOrders, setPreviousOrders] = useState([]);

    useEffect(() => {
    const token = sessionStorage.getItem('userToken');
    const tokenGuest = sessionStorage.getItem('guestToken');

    if (token) {
        const decodedToken = jwt_decode(token);
        setUser(decodedToken["id"]);
        setRole(decodedToken["role"]);
    } else if (tokenGuest) {
        const decodedTokenGuest = jwt_decode(tokenGuest);
        setUser(decodedTokenGuest["id"]);
        setRole(decodedTokenGuest["role"]);
    } else {
        setUser("Undefined");
    }
}, []);


    useEffect(() => {
    if (user) {
        fetchPreviousOrders(user, role)
            .then((data) => {
                setPreviousOrders(data);
            })
            .catch((error) => {
                console.error("Failed to fetch previous orders:", error);
            });
    }
}, [user, role]);


    const cancelOrder = async (orderId) => {
        try {
            const response = await axios.put(`${API_BASE_URL}/order/orders/cancel/${orderId}`);
            if (response.status === 200) {
                // Refresh the list of orders after cancellation
                fetchPreviousOrders(user, role)
                    .then((data) => {
                        setPreviousOrders(data);
                    })
                    .catch((error) => {
                        console.error("Failed to fetch previous orders:", error);
                    });
                console.log('Order canceled successfully');
            } else {
                console.error('Failed to cancel order with status:', response.status);
            }
        } catch (error) {
            console.error('Error canceling order:', error.message);
        }
    };


     const groupedOrders = {};
    previousOrders.forEach((order) => {
        const userName = order.userName || "Undefined";
        if (!groupedOrders[userName]) {
            groupedOrders[userName] = [];
        }
        groupedOrders[userName].push(order);
    });

    return (
        <div>
            <h1>Previous Orders</h1>
            {Object.keys(groupedOrders).length > 0 ? (
                Object.keys(groupedOrders).map((userName) => (
                    <div key={userName}>
                        {role === "Administrator" && <h2>{userName}</h2>}
                        <ul className="POP-grid-container">
                            {groupedOrders[userName].map((order) => (
                                <li key={order.OrderId} className="card POP-card grid-column">
                                    <div className="test">
                                        <div>
                                            <strong>Order Number:</strong> {order.orderId}
                                        </div>
                                        <div>
                                            <strong>Cart Items:</strong>
                                            <ul>
                                                {order.cartItems.map((cartItem) => (
                                                    <li key={cartItem.album.albumId}>
                                                        {cartItem.album.title} - Quantity: {cartItem.quantity} <br />
                                                        State: {OrderState[order.state]}
                                                            
                                                    </li>
                                                    
                                                ))}
                                            </ul>
                                        </div>
                                        {(order.state === 0 || order.state === 3) && (
                                            <div className="btn-container">
                                                <button className="cancel-button" onClick={() => cancelOrder(order.orderId)}>
                                                    Cancel Order
                                                </button>
                                            </div>
                                        )}
                                        {order.state === 2 && (
                                            <div className="btn-container">
                                                <button className="cancel-button-disabled" disabled>
                                                    Order Canceled
                                                </button>
                                            </div>
                                        )}
                                    </div>
                                </li>
                            ))}
                        </ul>
                    </div>
                ))
            ) : (
                <h3>No previous orders found</h3>
            )}
            <button className="previous-orders-button" onClick={() => navigate("/cart")}>Back to Cart</button>
        </div>
    );
}

export default PreviousOrdersPage;

