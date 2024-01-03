import React, { Component } from 'react';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { Layout } from '../../components/Layout';
import '../../styles/Common.css';
import { loadStripe } from '@stripe/stripe-js';
import { Elements } from '@stripe/react-stripe-js';
import CheckoutScreen from '../CheckoutScreen';
const stripePromise = loadStripe('pk_test_51OCShKEZSxcrg8JwcK6ZA4f6KKCjFJU4MPA5Gx2T6nOcdVFGqPdmcEGYbaJqxiCWXE3d3lTQhV8Ntt7iKV1BySeO00yEcr8YI8');



export default class App extends Component {
  static displayName = App.name;
  
  render() {
    return (
      <BrowserRouter>
        <Elements stripe={stripePromise}>
          <Layout>
            <Routes>
              {AppRoutes.map((route, index) => {
                const { element, ...rest } = route;
                return <Route key={index} {...rest} element={element} />;
              })}
              <Route
                path="/checkout/:orderId"
                element={
                  <CheckoutScreen />
                }
              />
            </Routes>
          </Layout>
        </Elements>
      </BrowserRouter>
    );
  }
}