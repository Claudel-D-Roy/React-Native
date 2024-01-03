import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { Footer } from './Footer';
import NavMenu from './NavMenu';
import '../styles/Layout.css'

export class Layout extends Component {
    static displayName = Layout.name;

    render() {
        return (
            <div className="bg">
                <NavMenu />
                    <Container tag="main">
                        {this.props.children}
                    </Container>
                <Footer />
            </div>
        );
    }
}