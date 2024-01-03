import { Container, Row } from 'reactstrap';
import { Link } from 'react-router-dom';
import Product from './Product';

const ProductList = ({ title, products = [], isHomeGallery = false }) => {
    const formattedTitle = title.replace(/\s+/g, '-').toLowerCase();

    return (
        <section className={`product-list section-${formattedTitle}`}>
            <Container>
                {isHomeGallery && (
                    <div className="section-category" style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                        <h1>{title}</h1>
                        <h3>
                            <Link to={`/products?category=${title.split(' ')[0]}&media=${title.split(' ')[1]}&sort=${title.split(' ')[2]}&genre=${title.split(' ')[3]}&available=${title.split(' ')[4]}`}>
                                Click for more {'->'}
                            </Link>
                        </h3>
                    </div>
                )}
                {!isHomeGallery}
                <Row>
                    {products.map(product => <Product key={product.id} product={product} />)}
                </Row>
            </Container>
        </section>
    );
};

export default ProductList;