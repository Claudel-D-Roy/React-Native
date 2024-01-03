import React, { Component } from 'react';
import Carousel from "../components/Carousel";
import Partners from './Partners';
import HomeGallery from './HomeGallery';
import '../styles/Home.css';
import '../styles/Fonts.css';
import '../styles/Common.css';

class Home extends Component {
    render() {
        return (
            <div>
                <Carousel />
                <HomeGallery />
                <Partners />
            </div>
        );
    }
}

export default Home;