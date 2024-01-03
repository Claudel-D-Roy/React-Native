import React, { useState, useEffect } from 'react';
import '../styles/OrderPage.css';
import { jwtDecode as jwt_decode } from 'jwt-decode';
import { API_BASE_URL } from './utils/config'
import axios from 'axios';
import Swal from 'sweetalert2';
import { useNavigate } from 'react-router-dom';


const OrderPage = () => {
    const [user, GetUser] = useState();
   const navigate = useNavigate();
    const [orderData, setOrderData] = useState({
        firstName: '',
        lastName: '',
        email: '',
        phone: '', // Champ pour le numéro de téléphone
        address: '',
        city: '',
        zipCode: '',
        country: 'Canada',
        province: '', // Champ pour la province
        // Ajoutez d'autres champs si nécessaire
    });
    const [errors, setErrors] = useState({});
    const [loading, setLoading] = useState(false);
    const [role, setRole] = useState(null);
    const [orderId, setOrderId] = useState(null);
    const [error, setError] = useState(null);

 
   
useEffect(() => {
    if (orderId !== null) {
        navigate(`/checkout/${orderId}`);
    }
  }, [orderId, navigate]);

    const provinces = [
        "Alberta", "British Columbia", "Manitoba", "New Brunswick", "Newfoundland and Labrador",
        "Nova Scotia", "Ontario", "Prince Edward Island", "Quebec", "Saskatchewan", "Yukon"
    ];

    //get token
    useEffect(() => {
        var token = sessionStorage.getItem('userToken');
        var tokenGuest = sessionStorage.getItem('guestToken');
        if (token) {
            var decodedToken = jwt_decode(token);
            var id = decodedToken["id"];
            GetUser(id);
            
            //call bd 
            axios.get(`${API_BASE_URL}/order/orders/userInfo/${id}`)
                .then((response) => {
                    if (response.status === 200) {
                        console.log(response.data)
                        var data = response.data;
                        setOrderData({
                            ...orderData,
                            firstName: data.firstName,
                            lastName: data.lastName,
                            email: data.email,
                            phone: data.phone,
                            address: data.address,
                            province: provinces[data.province],
                            city: data.city,
                            zipCode: data.zipCode,
                        });
                    } else {
                        throw Error(`Failed to fetch previous orders with status: ${response.status}`);
                    }
                })
                .catch((error) => {
                    console.error(error);
                    throw error;
                });
              

        } else if (tokenGuest) {
            var decodedTokenGuest = jwt_decode(tokenGuest);
            GetUser(decodedTokenGuest["id"]);
            setRole(decodedTokenGuest["role"]);
        } else {
            GetUser("Undefined");
        }
    }, []);


    const checkEmail = async (email) => {
       

            var response =  await axios.get(`${API_BASE_URL}/order/order/checkemail/${email}`);
            return response.data;
        
    }

    const validate = async (data) => {
        let errors = {};

        // Vérification du nom complet
        if (!data.firstName.trim()) errors.firstName = "The first name is required";
        if (!data.lastName.trim()) errors.lastName = "The last name is required";
  
        // Vérification de l'email
        if (!data.email.trim()) {
            errors.email = "Email required";
        } else if (!data.email.includes('@')) {
            errors.email = "Invalid Email";
        } else if(role === "Guest"){
            try {
                var emailExists = await checkEmail(data.email);
                if (emailExists) {
                    errors.email = "Email already exists";
                }
            } catch (error) {
                console.error("Error checking email:", error);
                
            }
        }

     
        // Vérification de l'adresse
        if (!data.address.trim()) errors.address = "adress required";

        // Vérification de la ville
        if (!data.city.trim()) errors.city = "town required";

        // Vérification du code postal
        const postalCodeRegex = /^[A-Za-z]\d[A-Za-z](?:\d)?[A-Za-z]\d$/;
        if (!data.zipCode.trim()) {
            errors.zipCode = "Postal Code required";
        } else if (!postalCodeRegex.test(data.zipCode)) {
            errors.zipCode = "Postal code invalid";
        }

        
        // Vérification du pays
        if (!data.country.trim()) errors.country = "Country required";

        return errors;
    };

    const handleProvinceChange = (event) => {
        const { value } = event.target;
        setOrderData({ ...orderData, province: value });
    };

    const handleInputChange = (event) => {
        const { name, value } = event.target;
        setOrderData({ ...orderData, [name]: value });
    };

    const handleSubmit = async (event) => {
        event.preventDefault();
        const formErrors = await validate(orderData);

        if (Object.keys(formErrors).length === 0) {
            var responseCart = await axios.get(`${API_BASE_URL}/Order/OrderInfo/${user}`);
            const responseData = responseCart.data;

            const orderInfoHtml = `
            <div style="display:flex;">
                <div style="width: 100%">
                <h1>User info</h1>
                <p>First Name: ${orderData.firstName}</p>
                <p>Last Name: ${orderData.lastName}</p>
                <p>Email: ${orderData.email}</p>
                <p>Phone: ${orderData.phone}</p>
                <p>Address: ${orderData.address}</p>
                <p>City: ${orderData.city}</p>
                <p>Country: ${orderData.country}</p>
                <p>Province: ${orderData.province}</p>
                <p>Zip Code: ${orderData.zipCode}</p>
                </div>
                <div style="width:100%">
                <h1>Items info</h1>
                ${responseData.items.map(item => `
                    <div>
                    <p>Album Title: ${item.album.title}</p>
                    <p>Quantity: ${item.quantity}</p>
                    <p>Price: ${item.album.price}</p>
                    </div>
                `).join('')}
                <p>Total Before Taxes: ${responseData.totalAvTaxes}</p>
                <p>Total Taxes: ${responseData.totalTaxes}</p>
                <p>Total After Taxes: ${responseData.totalApTaxes}</p>
                </div>
            </div>
            `;

            // Show SweetAlert modal with custom content
            const result = await Swal.fire({
                title: 'Confirm Order',
                html: orderInfoHtml,
                icon: 'question',
                showCancelButton: true,
                confirmButtonText: 'Confirm',
                cancelButtonText: 'Cancel',
            });

            if (result.isConfirmed) {

                setLoading(true);

                const response = await axios.post(`${API_BASE_URL}/Order/Order`, {
                    userId: user,
                    TotalAvTaxes : responseData.totalAvTaxes,
                    TotalTaxes : responseData.totalTaxes,
                    TotalApTaxes : responseData.totalApTaxes,
                    firstName: orderData.firstName,
                    lastName: orderData.lastName,
                    email: orderData.email,
                    phone: orderData.phone,
                    address: orderData.address,
                    city: orderData.city,
                    country: orderData.country,
                    province: orderData.province,
                    zipCode: orderData.zipCode,
                });


                if (response.status === 200) {
                    
                    setOrderId(response.data.orderId);
                }
              
          
                
                setLoading(false);
            } else {
                document.location.href = '/cart';
            }
        } else {
            setErrors(formErrors);
        }
    };


    if (loading) return <div>Chargement...</div>;
    if (error) return <div>Erreur: {error.message}</div>;

    return (
        <form onSubmit={handleSubmit} className="form-container">
            <div className="OP-grid-container">
                <div className="grid-column">
                    <div className="form-field">
                        <label htmlFor="firstName">First Name:</label>
                        <input
                            type="text"
                            className="order-form-field"
                            id="firstName"
                            name="firstName"
                            value={orderData.firstName}
                            onChange={handleInputChange}
                        />
                    </div>
                    <div className="form-field">
                        <label htmlFor="lastName">Last Name:</label>
                        <input
                            type="text"
                            className="order-form-field"
                            id="lastName"
                            name="lastName"
                            value={orderData.lastName}
                            onChange={handleInputChange}
                        />
                    </div>
                    <div className="form-field">
                        <label htmlFor="email">Email:</label>
                        <input
                            type="email"
                            className="order-form-field"
                            id="email"
                            name="email"
                            value={orderData.email}
                            onChange={handleInputChange}
                        />
                    </div>
                </div>
                <div className="grid-column">
                    <div className="form-field">
                        <label htmlFor="phone">Phone:</label>
                        <input
                            type="tel"
                            className="order-form-field"
                            id="phone"
                            name="phone"
                            value={orderData.phone}
                            onChange={handleInputChange}
                        />
                    </div>
                    <div className="form-field">
                        <label htmlFor="address">Address:</label>
                        <input
                            type="text"
                            className="order-form-field"
                            id="address"
                            name="address"
                            value={orderData.address}
                            onChange={handleInputChange}
                        />
                    </div>
                    <div className="form-field">
                        <label htmlFor="city">City:</label>
                        <input
                            type="text"
                            className="order-form-field"
                            id="city"
                            name="city"
                            value={orderData.city}
                            onChange={handleInputChange}
                        />
                    </div>
                </div>
                <div className="grid-column">
                    <div className="form-field">
                        <label htmlFor="country">Country:</label>
                        <input
                            type="text"
                            className="order-form-field"
                            id="country"
                            name="country"
                            value="Canada" // Toujours afficher Canada
                            readOnly // Rendre le champ en lecture seule
                            style={{ backgroundColor: '#e0e0e0' }} // Style en ligne pour la couleur de fond grise
                        />
                    </div>
                    <div className="form-field">
                        <label htmlFor="province">Province:</label>
                        <select className="order-form-field" id="province" name="province" value={orderData.province} onChange={handleProvinceChange}>
                            <option value="">Select a province</option>
                            {provinces.map((province, index) => (
                            <option key={index} value={province}>{province}</option>
                            ))}
                        </select>
                    </div>
                    <div className="form-field">
                        <label htmlFor="zipCode">ZIP Code:</label>
                        <input
                            className="order-form-field"
                            type="text"
                            id="zipCode"
                            name="zipCode"
                            value={orderData.zipCode}
                            onChange={handleInputChange}
                        />
                    </div>
                </div>
            </div>
            <div className="order-form-button">
                {errors.firstName && <div className="error">{errors.firstName}</div>}
                {errors.lastName && <div className="error">{errors.lastName}</div>}
                {errors.email && <div className="error">{errors.email}</div>}
                {errors.address && <div className="error">{errors.address}</div>}
                {errors.city && <div className="error">{errors.city}</div>}
                {errors.zipCode && <div className="error">{errors.zipCode}</div>}
                {errors.country && <div className="error">{errors.country}</div>}                
                <button type="submit" className="primary-button" >Send</button>
            </div>
        </form>
    );
};

export default OrderPage;
