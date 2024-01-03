import React, { useState, useEffect } from 'react';
import { CardElement, useStripe, useElements } from '@stripe/react-stripe-js';
import axios from 'axios';
import { useParams } from 'react-router-dom';
import { API_BASE_URL } from './utils/config';

const CheckoutScreen = () => {
  const { orderId } = useParams();
  const stripe = useStripe();
  const elements = useElements();
  const [nameOnCard, setNameOnCard] = useState('');
  const [billingAddress, setBillingAddress] = useState('');
  const [phoneNumber, setPhoneNumber] = useState('');
  const [cardError, setCardError] = useState('');

  const handleCardChange = (event) => {
    setCardError(event.error ? event.error.message : '');
  };

  const handleFormSubmit = async (event) => {
    event.preventDefault();

    if (!nameOnCard || !billingAddress || !phoneNumber) {
      console.error("Name on card, billing address, and phone number are required.");
      return;
    }

    try {
      const cardElement = elements.getElement(CardElement);

      const { token, error } = await stripe.createToken(cardElement);

      if (error) {
        setCardError(error.message);
        return;
      }

      await processPayment(token);
    } catch (error) {
      console.error("Error in form submission:", error);
      document.location.href = `/failed/${orderId}`;
    }
  };

  const processPayment = async (token) => {
    try {
      const paymentData = {
        NameOnCard: nameOnCard,
        BillingAddress: billingAddress,
        PhoneNumber: phoneNumber,
        Token: token.id,
        Last4: "",
        DateNow: "",
      };

      const response = await axios.post(`${API_BASE_URL}/charge/${orderId}`, paymentData);

      console.log("Payment Success:", response);
      document.location.href = '/confirmation';
    } catch (error) {
      console.error("Payment processing error:", error);
      document.location.href = `/failed/${orderId}`;
    }
  };

  useEffect(() => {
    if (elements) {
      const cardElement = elements.getElement(CardElement);

      const handleChange = (event) => {
        handleCardChange(event);
      };

      cardElement.addEventListener('change', handleChange);

      return () => {
        cardElement.removeEventListener('change', handleChange);
      };
    }
  }, [elements]);

  return (
    <section className="container mt-4">
      <div className="row">
        <aside className="col-lg-6">
          <div className="card shadow-sm p-4">
            <h3 className="h5 mb-3">Payment</h3>
            <form onSubmit={handleFormSubmit}>
              <div className="mb-3">
                <label htmlFor="nameOnCard" className="form-label">
                  Name on Card
                </label>
                <input
                  type="text"
                  className="form-control"
                  id="nameOnCard"
                  placeholder="Name on Card"
                  value={nameOnCard}
                  onChange={(e) => setNameOnCard(e.target.value)}
                  required
                />
              </div>

              <div className="mb-3">
                <label htmlFor="billingAddress" className="form-label">
                  Billing Address
                </label>
                <input
                  type="text"
                  className="form-control"
                  id="billingAddress"
                  placeholder="Billing Address"
                  value={billingAddress}
                  onChange={(e) => setBillingAddress(e.target.value)}
                  required
                />
              </div>

              <div className="mb-3">
                <label htmlFor="phoneNumber" className="form-label">
                  Phone Number
                </label>
                <input
                  type="tel"
                  className="form-control"
                  id="phoneNumber"
                  placeholder="Phone Number"
                  value={phoneNumber}
                  onChange={(e) => setPhoneNumber(e.target.value)}
                  required
                />
              </div>

              <div className="mb-3">
                <label htmlFor="creditCard" className="form-label">
                  Credit Card
                </label>
                <CardElement className="form-control" options={{ hidePostalCode: true }} onChange={handleCardChange} />
                <div role="alert" className="text-danger">
                  {cardError}
                </div>
              </div>

              <button type="submit" className="btn btn-primary" disabled={!stripe} id="submit-button">
                Buy now
              </button>
            </form>
          </div>
        </aside>
      </div>
    </section>
  );
};

export default CheckoutScreen;