import React from 'react';
import { Container, Row, Col } from 'reactstrap';
import '../styles/Home.css';
import '../styles/Fonts.css';

const Partners = () => {
    return (
        <section className="partners">
            <Container>
                <h4>Our Partners</h4>
                <Row style={{ display: 'flex', justifyContent: 'center' }}>
                    <Col md={2}>
                        <img src={require("../assets/images/partners/adisq.png")} alt="Adisq logo" />
                    </Col>
                    <Col md={2}>
                        <img src={require("../assets/images/partners/desjardins.png")} alt="Desjardins logo" />
                    </Col>
                    <Col md={2}>
                        <img src={require("../assets/images/partners/belle_province.png")} alt="Belle Province logo" />
                    </Col>
                    <Col md={2}>
                        <img src={require("../assets/images/partners/vice.png")} alt="Vice logo" />
                    </Col>
                    <Col md={2}>
                        <img src={require("../assets/images/partners/space_x.png")} alt="Space X logo" />
                    </Col>
                </Row>
            </Container>
        </section>
    )    
}

export default Partners;