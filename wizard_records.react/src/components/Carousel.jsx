import React, { useState, useEffect, useCallback } from 'react';
import { BiSolidLeftArrow, BiSolidRightArrow } from 'react-icons/bi';
import '../styles/Carousel.css';

const Carousel = () => {
    const [currentIndex, setCurrentIndex] = useState(0);
    const images = [
        { src: require("../assets/images/carousel/PubImage4.jpg"), alt: "Karl Tremblay" },
        { src: require("../assets/images/carousel/PubImage1.jpg"), alt: "New Rock Arrivals" },
        { src: require("../assets/images/carousel/PubImage2.jpg"), alt: "Get Together 2023" },
        { src: require("../assets/images/carousel/PubImage3.jpg"), alt: "Weekly Jazz Recommendations" },
    ];
    const length = images.length;

    const next = useCallback(() => {
        if (currentIndex < (length - 1)) {
            setCurrentIndex((prevState) => prevState + 1);
        } else {
            setCurrentIndex(0);
        }
    }, [currentIndex, length]);

    const prev = () => {
        if (currentIndex > 0) {
            setCurrentIndex((prevState) => prevState - 1);
        } else {
            setCurrentIndex(length - 1);
        }
    };

    useEffect(() => {
        const interval = setInterval(() => {
            next();
        }, 7000);

        return () => {
            clearInterval(interval);
        };
    }, [next]);

    return (
        <div className="carousel-container">
            <div className="carousel-wrapper">
                <button onClick={prev} className="left-arrow">
                    <BiSolidLeftArrow className="arrow" />
                </button>
                <div className="carousel-content-wrapper">
                    <div className="carousel-content" style={{ transform: `translateX(-${currentIndex * 100}%)` }}>
                        {images.map((image, index) => (
                            <img key={index} src={image.src} alt={image.alt || 'carousel-image'} />
                        ))}
                    </div>
                </div>
                <button onClick={next} className="right-arrow">
                    <BiSolidRightArrow className="arrow" />
                </button>
            </div>
        </div>
    );
}

export default Carousel;
