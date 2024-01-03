import React, { useState } from 'react';
import { Container, Form, FormGroup, Label, Input, Button } from 'reactstrap';
import { Media, Category, ArtistGenre, AlbumGenre, Grade } from './utils/constants'
import { API_BASE_URL } from './utils/config'
import axios from 'axios';

function AddProductForm() {
    const [product, setProduct] = useState({
        artistName: '',
        albumTitle: '',
        artistGenre: '',
        albumGenre: '',
        labelName: '',
        price: '',
        isUsed: '', // Used or new
        media: '',
        quantity: '',
        imageFilePath: '',
        mediaGrade: 8,
        sleeveGrade: 8,
        catalogNumber: '',
        matrixNumber: '',
        comments: ''
    });

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const response = await axios.post(`${API_BASE_URL}/crud/create`, {
                ArtistName: product.artistName,
                Title: product.albumTitle,
                ArtistGenre: product.artistGenre,
                AlbumGenre: product.albumGenre,
                LabelName: product.labelName,
                Price: parseFloat(product.price),
                IsUsed: product.isUsed,
                Media: product.media,
                Quantity: parseInt(product.quantity),
                ImageFilePath: product.imageFilePath,
                MediaGrade: product.mediaGrade,
                SleeveGrade: product.sleeveGrade,
                CatalogNumber: product.catalogNumber,
                MatrixNumber: product.matrixNumber,
                Comments: product.comments,
            });

            if (response.status === 200) {
                console.log('Album created:', response.data);
                alert('Album successfully created!');

                setProduct({
                    artistName: '',
                    albumTitle: '',
                    artistGenre: '',
                    albumGenre: '',
                    labelName: '',
                    price: '',
                    isUsed: '',
                    media: '',
                    quantity: '',
                    imageFilePath: '',
                    mediaGrade: '',
                    sleeveGrade: '',
                    catalogNumber: '',
                    matrixNumber: '',
                    comments: ''
                });
            } else {
                console.error('Error creating the album.');
                alert('Error creating the album. Please try again.');
            }
        } catch (error) {
            console.error('Error sending the product', error);
            alert('Error sending the product. Please check your input and try again.');
        }
    };

    const handleInputChange = (e, fieldName) => {
        let value = e.target.value;
      
        switch (fieldName) {
          case 'isUsed':
            value = value === 'true';
            break;
          case 'artistGenre':
          case 'albumGenre':
          case 'media':
          case 'mediaGrade':
          case 'sleeveGrade':
          case 'quantity':
            value = parseInt(value, 10);
            break;
          case 'price':
            value = parseFloat(value);
            if (isNaN(value)) value = "";
            break;
          default:
            break;
        }
      
        console.log("Setting", fieldName, "to", value);  // For debugging
        setProduct(prevProduct => ({ ...prevProduct, [fieldName]: value }));
      };

    return (
        <Container className="add-product-form">
            <h2>Add new product</h2>
            <Form onSubmit={handleSubmit}>
                <FormGroup>
                    <Label for="artistName">Artist name</Label>
                    <Input
                        type="text"
                        name="artistName"
                        id="artistName"
                        placeholder="Artist name"
                        value={product.artistId}
                        onChange={(e) => handleInputChange(e, 'artistName')}
                    />
                </FormGroup>
                <FormGroup>
                    <Label for="albumTitle">Album title</Label>
                    <Input
                        type="text"
                        name="albumTitle"
                        id="albumTitle"
                        placeholder="Album title"
                        value={product.albumTitle}
                        onChange={(e) => handleInputChange(e, 'albumTitle')}
                    />
                </FormGroup>
                <FormGroup>
                    <Label for="artistGenre">Artist genre</Label>
                    <Input
                        type="select"
                        name="artistGenre"
                        id="artistGenre"
                        value={product.artistGenre}
                        onChange={(e) => handleInputChange(e, 'artistGenre')}
                    >
                        <option value="">Select the Artist genre</option>
                        {ArtistGenre.map((genre, index) => (
                            <option key={index} value={genre.value}>
                                {genre.label}
                            </option>
                        ))}
                    </Input>
                </FormGroup>
                <FormGroup>
                    <Label for="albumGenre">Album genre</Label>
                    <Input
                        type="select"
                        name="albumGenre"
                        id="albumGenre"
                        value={product.albumGenre}
                        onChange={(e) => handleInputChange(e, 'albumGenre')}
                    >
                        <option value="">Select the Album genre</option>
                        {AlbumGenre.map((genre, index) => (
                            <option key={index} value={genre.value}>
                                {genre.label}
                            </option>
                        ))}
                    </Input>
                </FormGroup>
                <FormGroup>
                    <Label for="labelName">Label name</Label>
                    <Input
                        type="text"
                        name="labelName"
                        id="labelName"
                        placeholder="Label name"
                        value={product.labelName}
                        onChange={(e) => handleInputChange(e, 'labelName')}
                    />
                </FormGroup>
                <FormGroup>
                    <Label for="price">Price</Label>
                    <Input
                        type="number"
                        name="price"
                        id="price"
                        placeholder="Price"
                        value={product.price}
                        onChange={(e) => handleInputChange(e, 'price')}
                    />
                </FormGroup>
                <FormGroup>
                    <Label for="isUsed">Category</Label>
                    <Input
                        type="select"
                        name="isUsed"
                        id="isUsed"
                        value={product.isUsed.toString()} // Convert the boolean value to a string for the dropdown
                        onChange={(e) => handleInputChange(e, 'isUsed')}
                    >
                        <option value="">Select a category</option>
                        {Category.map((category, index) => (
                            <option key={index} value={category.value}>
                                {category.label}
                            </option>
                        ))}
                    </Input>
                </FormGroup>
                <FormGroup>
                    <Label for="media">Media</Label>
                    <Input
                        type="select"
                        name="media"
                        id="media"
                        value={product.media}
                        onChange={(e) => handleInputChange(e, 'media')}
                    >
                        <option value="">Select a media type</option>
                        {Media.map((media, index) => (
                            <option key={index} value={media.value}>
                                {media.label}
                            </option>
                        ))}
                    </Input>
                </FormGroup>
                <FormGroup>
                    <Label for="quantity">Quantity</Label>
                    <Input
                        type="number"
                        name="quantity"
                        id="quantity"
                        placeholder="Quantity"
                        value={product.quantity}
                        onChange={(e) => handleInputChange(e, 'quantity')}
                    />
                </FormGroup>
                <FormGroup>
                    <Label for="imageFilePath">Image file path</Label>
                    <Input
                        type="text"
                        name="imageFilePath"
                        id="imageFilePath"
                        placeholder="File name with extension only"
                        value={product.imageFileName}
                        onChange={(e) => handleInputChange(e, 'imageFilePath')}
                    />
                </FormGroup>
                <FormGroup>
                    <Label for="mediaGrade">Media grade</Label>
                    <Input
                        type="select"
                        name="mediaGrade"
                        id="mediaGrade"
                        value={product.mediaGrade}
                        onChange={(e) => handleInputChange(e, 'mediaGrade')}
                    >
                        <option value={8}>Select grade for the album's condition</option> {/* Default to None */}
                        {Grade.map((grade, index) => (
                            <option key={index} value={grade.value}>
                                {grade.label}
                            </option>
                        ))}
                    </Input>
                </FormGroup>
                <FormGroup>
                    <Label for="sleeveGrade">Sleeve grade</Label>
                    <Input
                        type="select"
                        name="sleeveGrade"
                        id="sleeveGrade"
                        value={product.sleeveGrade}
                        onChange={(e) => handleInputChange(e, 'sleeveGrade')}
                    >
                        <option value={8}>Select grade for the sleeve's condition</option> {/* Default to None */}
                        {Grade.map((grade, index) => (
                            <option key={index} value={grade.value}>
                                {grade.label}
                            </option>
                        ))}
                    </Input>
                </FormGroup>
                <FormGroup>
                    <Label for="catalogNumber">Catalog number</Label>
                    <Input
                        type="text"
                        name="catalogNumber"
                        id="catalogNumber"
                        placeholder="For used products ONLY!"
                        value={product.catalogNumber}
                        onChange={(e) => handleInputChange(e, 'catalogNumber')}
                    />
                </FormGroup>
                <FormGroup>
                    <Label for="matrixNumber">Matrix number</Label>
                    <Input
                        type="text"
                        name="matrixNumber"
                        id="matrixNumber"
                        placeholder="For used products ONLY!"
                        value={product.matrixNumber}
                        onChange={(e) => handleInputChange(e, 'matrixNumber')}
                    />
                </FormGroup>
                <FormGroup>
                    <Label for="comments">Comments</Label>
                    <Input
                        type="textarea"
                        name="comments"
                        id="comments"
                        placeholder="Comments"
                        value={product.comments}
                        onChange={(e) => handleInputChange(e, 'comments')}
                    />
                </FormGroup>
                <Button type="submit">Add product</Button>
            </Form>
        </Container>
    );
}

export default AddProductForm;
