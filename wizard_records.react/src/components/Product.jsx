import { Col, Card, CardImg, CardBody, CardTitle, CardSubtitle } from 'reactstrap';
import { Link } from 'react-router-dom';

const Product = ({ product }) => {
    let coverImageSrc, formatImageSrc;
    
    try {
        formatImageSrc = require(`../assets/images/format/${product.media}`);
    } catch (err) {
        console.error(`Error requiring format image for ${product.media}`, err);
    }

    try {
        const coverImageFile = require(`../assets/images/covers/${product.cover}`);

        if (coverImageFile) {
            coverImageSrc = coverImageFile;
        }
    } catch (err) {
        console.error(`Error requiring cover image for ${product.cover}`, err);
        coverImageSrc = require('../assets/images/covers/default.webp');
    }

    return (
        <Col md={4} className="d-flex mb-4">
            <Link to={`/detail/${product.id}`} className="card-href card-width">
                <Card className="h-100">
                    <CardImg top className="card-img-format" src={formatImageSrc} alt={product.media} />
                    <CardImg top className="card-img-cover" src={coverImageSrc} alt={product.cover} />
                    <CardBody className="card-body">
                        <div className="card-info">
                            <CardTitle className="card-artist">{product.artistName}</CardTitle>
                            <CardSubtitle className="card-album">{product.albumTitle}</CardSubtitle>
                        </div>
                        <div className="card-divider"></div>
                        <div className="card-purchase">
                            <CardTitle className="card-price"><b>${product.price}</b></CardTitle>
                            <CardSubtitle className={`card-basket ${product.quantity === 0 ? 'card-unavailable' : ''}`}>{product.quantity === 0 ? 'Sold Out' : 'Add to cart'}</CardSubtitle>
                        </div>
                    </CardBody>
                </Card>
            </Link>
        </Col>
    );
};

export default Product;