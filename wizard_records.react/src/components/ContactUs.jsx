import React, { Component } from 'react';
import { Container, Form, FormGroup, Label, Input, Alert, Button } from 'reactstrap';
//import '../styles/ContactUs.css';

class ContactForm extends Component {
    constructor(props) {
        super(props);
        this.state = {
            formData: {
                name: '',
                email: '',
                description: '',
            },
            errors: {},
        };
    }

    handleSubmit = async (event) => {
        event.preventDefault();
        const errors = {};

        if (!this.state.formData.name.trim()) {
            errors.name = "Name is required.";
        }
        if (!this.state.formData.email.trim()) {
            errors.email = "Email is required.";
        }
        if (!this.state.formData.description.trim()) {
            errors.description = "A description is required.";
        }

        this.setState({ errors });

        if (!Object.keys(errors).length) {
            console.log('Form is valid!');
        }
    }

    handleInputChange = (e) => {
        const { name, value } = e.target;
        this.setState((prevState) => ({
            formData: {
                ...prevState.formData,
                [name]: value,
            },
        }));
    };

    render() {
        return (
            <section className="contact-form">
                <Container>
                    <h2>Contact Us</h2>
                    <Form onSubmit={this.handleSubmit}>
                        <FormGroup>
                            <Label for="name">Name</Label>
                            <Input
                                type="text"
                                name="name"
                                id="name"
                                placeholder="Enter your name"
                                value={this.state.formData.name}
                                onChange={this.handleInputChange}
                            />
                            {this.state.errors.name && <Alert color="danger">{this.state.errors.name}</Alert>}
                        </FormGroup>
                        <FormGroup>
                            <Label for="email">Email</Label>
                            <Input
                                type="email"
                                name="email"
                                id="email"
                                placeholder="Enter your email"
                                value={this.state.formData.email}
                                onChange={this.handleInputChange}
                            />
                            {this.state.errors.email && <Alert color="danger">{this.state.errors.email}</Alert>}
                        </FormGroup>
                        <FormGroup>
                            <Label for="description">Description</Label>
                            <Input
                                type="textarea"
                                name="description"
                                id="description"
                                placeholder="Describe your request"
                                value={this.state.formData.description}
                                onChange={this.handleInputChange}
                            />
                            {this.state.errors.description && <Alert color="danger">{this.state.errors.description}</Alert>}
                        </FormGroup>
                        <Button type="submit">Submit</Button>
                    </Form>
                </Container>
            </section>
        );
    }
}

export default ContactForm;

