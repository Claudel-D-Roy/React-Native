import React, { useState } from 'react';
import { Container, Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink, Input, Button } from 'reactstrap';
import { Link } from 'react-router-dom';
import { useNavigate } from "react-router-dom";
import '../styles/NavMenu.css';

const NavMenu = () => {
    const navigate = useNavigate();
    const [collapsed, setCollapsed] = useState(true);
    const [searchQuery, setSearchQuery] = useState('');

    const toggleNavbar = () => {
        setCollapsed(!collapsed);
    };

    const handleSearchChange = (event) => {
        setSearchQuery(event.target.value);
    };

    const handleSearchSubmit = () => {
        // Get the search query from the state
        const query = searchQuery;

        // Navigate to the search results page with the query as a parameter
        navigate(`/search?query=${query}`);
    };

    return (
        <header id="container">
            {/* Display for desktop/tablets */}
            <div className="desktop">
                <div className="container">
                    <div className="row justify-content-md-center">
                        <div className="col-md-auto">
                            <NavbarBrand tag={Link} to="/"><img src={require("../assets/images/logo/WizardRecords.png")} alt="logo" style={{ maxHeight: '50px', maxWidth: '250px' }} /></NavbarBrand>
                        </div>
                        <div className="col col-lg">
                            <div className="input-group">
                                <Input
                                    className="search-input"
                                    type="text"
                                    placeholder="Search products"
                                    value={searchQuery}
                                    onChange={handleSearchChange}
                                />
                                <div className="input-group-append">
                                    <Button color="primary" onClick={handleSearchSubmit}>
                                        Search
                                    </Button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
                    <section className="hero-banner">
                        <Container>
                            <p className="slogan">"Where your Crate-digging adventures begin"</p>
                        </Container>
                    </section>
                    <NavbarToggler onClick={toggleNavbar} className="mr-2" />
                    <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
                        <ul className="navbar-nav flex-grow">
                            <NavItem>
                                <NavLink tag={Link} className="text-light" to="/">Home</NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink tag={Link} className="text-light" to="/products">Products</NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink tag={Link} className="text-light" to="/cart">Cart</NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink tag={Link} className="text-light" to="/history">Purchase History</NavLink>
                                </NavItem>
                            <NavItem>
                                <NavLink tag={Link} className="text-light" to="/account">Account</NavLink>
                            </NavItem>
                        </ul>
                    </Collapse>
                </Navbar>
            </div>
            {/* Display for small screens */}
            <div className="mobile">
                <div className="container">
                    <div className="row justify-content-md-center">
                        <div className="col-md-auto">
                            <NavbarBrand tag={Link} to="/"><img src={require("../assets/images/logo/WizardRecords.png")} alt="logo" style={{ maxHeight: '50px', maxWidth: '250px' }} /></NavbarBrand>
                        </div>
                        <section className="hero-banner">
                            <Container>
                                <p className="slogan">"From yesterday's vinyls to today's hits."</p>
                            </Container>
                        </section>
                        <div className="col col-lg">
                            <div className="input-group">
                                <Input
                                    type="text"
                                    placeholder="search products"
                                    value={searchQuery}
                                    onChange={handleSearchChange}
                                />
                                <div className="input-group-append">
                                    <Button color="primary" onClick={handleSearchSubmit}>
                                        Search
                                    </Button>
                                </div>
                            </div>
                            <div id="menu">
                                <Navbar className="navbar-expand-sm navbar-toggleable-sm box-shadow mb-3" light>
                                    <NavbarToggler onClick={toggleNavbar} className="mr-2" />
                                    <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
                                        <ul className="navbar-nav flex-grow">
                                            <NavItem>
                                                <NavLink tag={Link} className="text-light" to="/">Home</NavLink>
                                            </NavItem>
                                            <NavItem>
                                                <NavLink tag={Link} className="text-light" to="/products">Products</NavLink>
                                            </NavItem>
                                            <NavItem>
                                                <NavLink tag={Link} className="text-light" to="/cart">Cart</NavLink>
                                            </NavItem>
                                            <NavItem>
                                <NavLink tag={Link} className="text-light" to="/history">Purchase History</NavLink>
                                </NavItem>
                                            <NavItem>
                                                <NavLink tag={Link} className="text-light" to="/account">Account</NavLink>
                                            </NavItem>
                                           
                                        </ul>
                                    </Collapse>
                                </Navbar>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </header>
    );
}

export default NavMenu;