import React, { Component } from 'react';
import '../styles/Footer.css';

export class Footer extends Component {
    render() {
        return (
            <footer className="footer">
                <div className="container">
                    <div className="row">
                        <div className="footer-col">
                            <h4>company</h4>
                            <ul>
                                <li><a href="/aboutus">about us</a></li>
                                <li><a href="https://github.com/thatscringebro/TPWebWizards">our services</a></li>
                                <li><a href="https://github.com/thatscringebro/TPWebWizards">privacy policy</a></li>
                                <li><a href="/contactus">Contact us</a></li>
                            </ul>
                        </div>
                        <div className="footer-col">
                            <h4>get help</h4>
                            <ul>
                                <li><a href="https://github.com/thatscringebro/TPWebWizards">FAQ</a></li>
                                <li><a href="https://github.com/thatscringebro/TPWebWizards">shipping</a></li>
                                <li><a href="https://github.com/thatscringebro/TPWebWizards">returns</a></li>
                                <li><a href="/previous_orders">order status</a></li>
                                <li><a href="https://stripe.com/docs">payment options</a></li>
                            </ul>
                        </div>
                        <div className="footer-col">
                            <h4>online shop</h4>
                            <ul>
                                <li><a href="/products?media=vinylOnly">Vinyls</a></li>
                                <li><a href="/products?media=cdOnly">Compact discs</a></li>
                            </ul>
                        </div>
                        <div className="footer-col">
                            <h4>follow us</h4>
                            <div className="social-links">
                                <a href="https://facebook.com"><img src={require("../assets/images/social/facebook.png")} alt="Facebook" /></a>
                                <a href="https://twitter.com"><img src={require("../assets/images/social/twitterX.png")} alt="Twitter" /></a>
                                <a href="https://instagram.com"><img src={require("../assets/images/social/instagram.png")} alt="Instagram" /></a>
                                <a href="https://pinterest.com"><img src={require("../assets/images/social/pinterest.png")} alt="Pinterest" /></a>
                            </div>
                        </div>
                    </div>
                </div>
            </footer>
        );
    }
}

