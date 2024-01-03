import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { API_BASE_URL } from './utils/config';
import { jwtDecode as jwt_decode } from 'jwt-decode';




function History() {

    const [user, setUser] = useState();
    const [history, setHistory] = useState([]);
    
    
    
    useEffect(() => {
        const token = sessionStorage.getItem('userToken');
        const tokenGuest = sessionStorage.getItem('guestToken');

        if (token) {
            const decodedToken = jwt_decode(token);
            setUser(decodedToken["id"]);
        } else if (tokenGuest) {
            const decodedToken = jwt_decode(tokenGuest);
            setUser(decodedToken["id"]);
        } }, []);




    const fetchHistory = (user) => 
    {
        return axios.get(`${API_BASE_URL}/payment/${user}`).then((responce) => {
       
        if(responce.status === 200)
        {
            return responce.data;
        }
        else 
        {
            throw Error('Failed to fetch payment history with status: ${responce.status}');
        }
        }).catch((error) => {
            console.error(error);
            throw error;
        });
    
    };





    useEffect(() => {
        if (user) {
            fetchHistory(user)
                .then((data) => {
                    setHistory(data);
                })
                .catch((error) => {
                    console.error(error);
                });
        }
    }, [user]);





    return (
        <div className="container">
            <div className="row">
                <div className="col-12">
                    <h2>Payment History</h2>
                    {history.length > 0 ? (
                        <table className="table table-striped">
                            <thead>
                                <tr>
                                    <th>Payment Date</th>
                                    <th>Name on card</th>
                                    <th>Billing Address</th>
                                    <th>Phone Number</th>
                                    <th>Last four number on credit card</th>
                                </tr>
                            </thead>
                            <tbody>
                                {history.map((payment) => (
                                    <tr key={payment.paymentId}>
                                        <td>{payment.dateNow}</td>
                                        <td>{payment.nameOnCard}</td>
                                        <td>{payment.billingAddress}</td>
                                        <td>{payment.phoneNumber}</td>
                                        <td>**** **** **** **** {payment.last4}</td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    ) : (
                        <p>No payment history available</p>
                    )}
                </div>
            </div>
        </div>
    );

}
export default History;