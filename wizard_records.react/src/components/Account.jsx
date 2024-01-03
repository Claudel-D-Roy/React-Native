import React, { useState, useEffect } from 'react';
import { Container, Form, FormGroup, Label, Input, Button } from 'reactstrap';
import { Formik, Field, ErrorMessage } from 'formik';
import * as Yup from 'yup';
import { API_BASE_URL } from './utils/config';
import {  useNavigate } from 'react-router-dom';
import { Province } from './utils/constants';
import { jwtDecode as jwt_decode } from 'jwt-decode';
import swal from 'sweetalert2';

import axios from 'axios';
import '../styles/Account.css';

//PLACER LE NAVIGATE SI PAS DE GUEST TOKEN


const loginSchema = Yup.object().shape({
    Email: Yup.string().required('Email is required'),
    Password: Yup.string().required('Password is required')
});

const registerSchema = Yup.object().shape({
    UserName: Yup.string().required('Username is required'),
    FirstName: Yup.string().required('First name is required'),
    LastName: Yup.string().required('Last name is required'),
    Email: Yup.string().email('Invalid email').required('Email is required'),
    Password: Yup.string().required('Password is required').matches(
      /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,})/,
      "Must Contain 8 Characters, One Uppercase, One Lowercase, One Number and One Special Case Character"
    ),
    Confirm: Yup.string()
        .oneOf([Yup.ref('Password'), null], 'Passwords must match')
        .required('Password confirmation is required'),
    PhoneNumber: Yup.string().required('Phone number is required'),
    AddressNum: Yup.number().required('Address number is required'),
    StreetName: Yup.string().required('Street name is required'),
    City: Yup.string().required('City is required'),
    Province: Yup.number()
      .oneOf(Province.map((_, index) => index), 'Invalid province')
      .required('Province is required'),
    PostalCode: Yup.string().required('Postal code is required').matches(/^[A-Za-z]\d[A-Za-z]\d[A-Za-z]\d$/, "postal code invalid"),
});

function Account() {
    const navigate = useNavigate(); 
    const [isLogin, setIsLogin] = useState(true);
    const [isLoggedIn, setLoggedIn] = useState(false);
    const [user, GetUser] = useState();
    const [cart, setCart] = useState();
    const [userNew, SetUser] = useState();
    const [role , setRole] = useState();

  

   //get token
   useEffect(() => {
    var token = sessionStorage.getItem('userToken');
    var tokenGuest = sessionStorage.getItem('guestToken');
    if(token)
    {
        var decodedToken = jwt_decode(token);
        GetUser(decodedToken["id"]);

    }else if(tokenGuest){     
      var decodedTokenGuest = jwt_decode(tokenGuest);
      GetUser(decodedTokenGuest["id"]);
      setRole(decodedTokenGuest["role"]);

    }
    else {
      GetUser("Undefined");
      
    }
}, []);



useEffect(() => {
  const fetchCart = async () => {
     try {
        const carts = await axios.get(`${API_BASE_URL}/cart/user/${user}`);
        if (carts.status === 200) {
           setCart(carts.data);  
           console.log('Panier ajouté à la variable');
        }
     } catch (error) {
        console.error('Erreur lors de la récupération du panier:', error.message);
     }
  };

  if (role === 'Guest') {
     fetchCart();
  }
}, [role, user]);


useEffect(() => {
  if (role === 'Guest' && userNew !== undefined && cart !== undefined) {
     addCartToUser(cart);
  }
}, [role, userNew, cart]);


async function addCartToUser(cart) {
  if (userNew !== null) {
    try {
      // Vérifier si le panier existe pour l'utilisateur
      const usercart = await axios.get(`${API_BASE_URL}/cart/user/${userNew}`);

      if (usercart.data && usercart.data.cartId) {
        console.log('Le panier existe déjà');
        var OldCart = usercart.data;

        for (const album of cart.cartItems) {
          try {

            if(album.quantity === 1)
            {
              const response = await axios.post(`${API_BASE_URL}/cart/add/${OldCart.cartId}/${album.album.albumId}`);
              if (response.status === 200) {
                console.log('Album ajouté au panier');
              } else {
                console.error('Échec de l\'ajout de l\'album au panier avec le statut :', response.status);
              }
            }else {
              for(let i = 0; i < album.quantity; i++)
              {
                const response = await axios.post(`${API_BASE_URL}/cart/add/${OldCart.cartId}/${album.album.albumId}`);
                if (response.status === 200) {
                  console.log('Album ajouté au panier');
                } else {
                  console.error('Échec de l\'ajout de l\'album au panier avec le statut :', response.status);
                }
              }

            }
           
          } catch (error) {
            console.error('Erreur lors de l\'ajout de l\'album au panier :', error.message);
          }
        }
        navigate('/');
      }
       
    } catch (error) {

      console.log('Le panier n\'existe pas');
      // Si le panier n'existe pas, le créer
      const creationPanier = await axios.post(`${API_BASE_URL}/cart/createpanier/${userNew}`, cart, {
        headers: {
          'Content-Type': 'application/json',
        },
      });

      if (creationPanier.status === 200) {
        console.log('Panier créé');
        var newPanier = creationPanier.data;

        for (const album of cart.cartItems) {
          try {
            if(album.quantity === 1)
            {
              const response = await axios.post(`${API_BASE_URL}/cart/add/${newPanier.cartId}/${album.album.albumId}`);
              if (response.status === 200) {
                console.log('Album ajouté au panier');
              } else {
                console.error('Échec de l\'ajout de l\'album au panier avec le statut :', response.status);
              }
            }else {
              for(let i = 0; i < album.quantity; i++)
              {
                const response = await axios.post(`${API_BASE_URL}/cart/add/${newPanier.cartId}/${album.album.albumId}`);
                if (response.status === 200) {
                  console.log('Album ajouté au panier');
                } else {
                  console.error('Échec de l\'ajout de l\'album au panier avec le statut :', response.status);
                }
              }

            }
          } catch (error) {
            console.error('Erreur lors de l\'ajout de l\'album au panier :', error.message);
          }
        }
        navigate('/');
      } else {
        console.error('Échec de la création du panier avec le statut :', creationPanier.status);
      }
    }
    
  }
  navigate('/');
}
  


    useEffect(() => {
      
    const urlSearchParams = new URLSearchParams(window.location.search);
    const isLoginParam = urlSearchParams.get('isLogin');
    
    if (isLoginParam === 'true') {
        setIsLogin(true);
    } else if (isLoginParam === 'false') {
        setIsLogin(false);
    }
    else {
        setIsLogin(true);
      }

    },[setIsLogin]);

   
  
  


    const initLoginValues = {
        Email: '',
        Password: '',
      };
    
    const initRegisterValues = {
        UserName: '',
        FirstName: '',
        LastName: '',
        Email: '',
        Password: '',
        Confirm: '',
        PhoneNumber: '',
        AddressNum: '',
        StreetName: '',
        City: '',
        PostalCode: '',
        Province: ''
    }, [formData, setFormData] = useState(initRegisterValues);

    useEffect(() => {
        var token = sessionStorage.getItem('userToken');
        if(token)
        {
            setLoggedIn(true);  
            sessionStorage.removeItem('guestToken');
        }
    },[]);




    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData(prevState => ({ ...prevState, [name]: value }));
    };

    const handleLogout = async () => {     
        try {
          const response = await axios.post(`${API_BASE_URL}/account/logout`);
          if (response.status === 200) {
            setIsLogin(false);
            sessionStorage.removeItem('userToken');
            swal.fire({
              title:"Success!", 
              html: "You have been successfully logged out!", 
              icon: "success"
            });
            navigate('/');
          } else {
            swal.fire({
              title: "An error has occurred during the logout process...", 
              html: "If the error persists, you can close this window to disconnect this person.", 
              icon: "error"
            });
          }
        } catch (error) {
          console.error("Logout error:", error);
          swal.fire({
            title: "An error has occurred during the logout process...", 
            html: "If the error persists, you can close this window to disconnect this person.", 
            icon: "error"
          });
        }
      };

    // const handleSubmit = async (e) => {
    const handleSubmit = async (values, actions) => {
        console.log("handleSubmit triggered!");
        console.log("Form values:", values);
        
       

        try {
            let response;

            if (isLogin) {
                response = await axios.post(`${API_BASE_URL}/account/login`, {
                    Email: values.Email,
                    Password: values.Password
                });
            } else {
              const { Confirm, ...rest } = values;
              const registrationData = {
                ...rest,
                Province: parseInt(rest.Province, 10)
              };

              response = await axios.post(`${API_BASE_URL}/account/register`, registrationData);
              if (response.status === 200){
                  swal.fire({
                    title: "Success!", 
                    html: "You have been successfully registered!", 
                    icon: "success"
                  });
                  setIsLogin(true);
              }
            }
            if (response.data.token) {
                sessionStorage.setItem('userToken', JSON.stringify(response.data.token));
                swal.fire({
                  title: "Success!", 
                  html: "You have been successfully logged in!", 
                  icon: "success"
                });
                setLoggedIn(true);              
                sessionStorage.removeItem('guestToken');
                var token = sessionStorage.getItem('userToken');
                if(token)
                {
                    var decodedToken = jwt_decode(token);
                    SetUser(decodedToken["id"]);
                }
                if(role !== 'Guest'){
                  navigate('/');}
               
            }

        } catch (error) {
            console.error("Authentication error:", error);
            swal.fire({
              title: "Authentication failed", 
              html: "Authentication failed, please try again, your email or username may already be in use. ", 
              icon: "error"
            });
            actions.setErrors({ general: 'Authentication failed. Please try again.' });
        }

        actions.setSubmitting(false);
    };
    

    //A modifier pour pouvoir se logout: ne doit pas utiliser isLoggedIn car la variable est reset a chaque fois qu'on reviens sur la page
    //devrait plutot regarder si le token est existant
    if (isLoggedIn) {
        return (
            <div className='logout-div'>
                <h1>Do you want to log out?</h1>
                <Button className="btn-submit" type="submit" onClick={() => handleLogout()}>
                    Logout
                </Button>
            </div>
        )
    }

    return (
        <section className={isLogin ? "login-form" : "register-form"}>
          <Container className="account-container">
            <h1>{isLogin ? 'Login' : 'Register'}</h1>
            <Formik
              initialValues={isLogin ? initLoginValues : initRegisterValues}
              validationSchema={isLogin ? loginSchema : registerSchema}
              onSubmit={handleSubmit}
            >
              {({ handleSubmit, isSubmitting }) => (
                <Form onSubmit=
                {handleSubmit}>
                  {!isLogin && (
                    <>
                      <FormGroup>
                        <Label for="UserName">Username</Label>
                        <Field name="UserName" as={Input} placeholder="Username" className="field"/>
                        <ErrorMessage name="UserName" component="div" className="error-message"/>
                      </FormGroup>
                      <FormGroup>
                        <Label for="FirstName">First Name</Label>
                        <Field name="FirstName" as={Input} placeholder="First Name" className="field"/>
                        <ErrorMessage name="FirstName" component="div" className="error-message"/>
                      </FormGroup>
                      <FormGroup>
                        <Label for="LastName">Last Name</Label>
                        <Field name="LastName" as={Input} placeholder="Last Name" className="field"/>
                        <ErrorMessage name="LastName" component="div" className="error-message"/>
                      </FormGroup>
                      <FormGroup>
                        <Label for="Email">Email</Label>
                        <Field name="Email" type="email" as={Input} placeholder="Email" className="field"/>
                        <ErrorMessage name="Email" component="div" className="error-message"/>
                      </FormGroup>
                      <FormGroup>
                        <Label for="Password">Password</Label>
                        <Field name="Password" type="password" as={Input} placeholder="Password" className="field"/>
                        <ErrorMessage name="Password" component="div" className="error-message"/>
                      </FormGroup>
                      <FormGroup>
                        <Label for="Confirm">Confirm password</Label>
                        <Field name="Confirm" type="password" as={Input} placeholder="Confirm password" className="field"/>
                        <ErrorMessage name="Confirm" component="div" className="error-message"/>
                      </FormGroup>
                      <FormGroup>
                        <Label for="PhoneNumber">Phone number</Label>
                        <Field name="PhoneNumber" type="string" as={Input} placeholder="Phone number" className="field"/>
                        <ErrorMessage name="PhoneNumber" component="div" className="error-message"/>
                      </FormGroup>
                      <FormGroup>
                        <Label for="AddressNum">Civic address number</Label>
                        <Field name="AddressNum" type="number" as={Input} placeholder="Civic address number" className="field"/>
                        <ErrorMessage name="AddressNum" component="div" className="error-message"/>
                      </FormGroup>
                      <FormGroup>
                        <Label for="StreetName">Street name</Label>
                        <Field name="StreetName" type="string" as={Input} placeholder="Street name" className="field"/>
                        <ErrorMessage name="StreetName" component="div" className="error-message"/>
                      </FormGroup>
                      <FormGroup>
                        <Label for="City">City</Label>
                        <Field name="City" type="string" as={Input} placeholder="City, town, etc." className="field"/>
                        <ErrorMessage name="City" component="div" className="error-message"/>
                      </FormGroup>
                      <FormGroup>
                        <Label for="PostalCode">Postal code</Label>
                        <Field name="PostalCode" type="string" as={Input} placeholder="Postal code" className="field"/>
                        <ErrorMessage name="PostalCode" component="div" className="error-message"/>
                      </FormGroup>
                      <FormGroup>
                        <Label for="Province">Province</Label><br />
                        <Field name="Province" as="select" className="field" id="provMenu">
                          <option value="">Select a Province</option>
                          {Province.map((province, index) => (
                            <option key={index} value={index}>
                              {province.value}
                            </option>
                          ))}
                        </Field>
                        <ErrorMessage name="Province" component="div" className="error-message"/>
                      </FormGroup>
                    </>
                  )}
                  {isLogin && (
                    <>
                        <FormGroup>
                            <Label for="Email">Email</Label>
                            <Field name="Email" type={isLogin ? "text" : "email"} as={Input} placeholder="Email" className="field"/>
                            <ErrorMessage name="Email" component="div" className="error-message"/>
                        </FormGroup>
                        <FormGroup>
                            <Label for="Password">Password</Label>
                            <Field name="Password" type="password" as={Input} placeholder="Password" className="field"/>
                            <ErrorMessage name="Password" component="div" className="error-message"/>
                        </FormGroup>
                    </>
                  )}
                  <Button className="btn-submit" type="submit" disabled={isSubmitting}>
                    {isLogin ? 'Login' : 'Register'}
                  </Button>
                  <Button type="button" onClick={() => setIsLogin(!isLogin)}>
                    Switch to {isLogin ? 'Register' : 'Login'}
                  </Button>
                </Form>
              )}
            </Formik>
          </Container>
        </section>
    );
}


export default Account;
