import React from 'react';
import { GoogleMap, LoadScript } from '@react-google-maps/api';

function GoogleMapComponent() {
    const mapStyles = {
        height: '300px',
        width: '50%',
    };
    
    const defaultCenter = {
        lat: 46.829853, 
        lng: -71.254028, 
    };

    return (
        <LoadScript googleMapsApiKey={process.env.REACT_APP_GOOGLE_MAPS_API_KEY}>
            <GoogleMap mapContainerStyle={mapStyles} center={defaultCenter} zoom={10} ></GoogleMap>
        </LoadScript>
    );
}

export default GoogleMapComponent;