import axios from 'axios';
import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { API_BASE_URL } from './utils/config';

const PaymentFailed = () => {
  const { orderId } = useParams();
  const [countdown, setCountdown] = useState(5);
  const [isCleanupExecuted, setIsCleanupExecuted] = useState(false);

  useEffect(() => {
    const intervalId = setInterval(() => {
      setCountdown(prevCountdown => prevCountdown - 1);

      if (countdown <= 0) {
        clearInterval(intervalId);

        document.location.href = '/';
      }
    }, 1000);

    return () => {
      clearInterval(intervalId);
      if (!isCleanupExecuted) {
        setIsCleanupExecuted(true);
        cancelOrder(orderId);
      }
    };
  }, [countdown, orderId, isCleanupExecuted]);

  const cancelOrder = async (orderId) => {
    await axios.put(`${API_BASE_URL}/order/orders/cancel/${orderId}`);
  };

  return (
    <section className="row bg-light rounded">
      <div className="col p-2">
        <h2>Checkout failed...</h2>
        <p>Sorry, your purchase couldn't be completed... try later.</p>

        <div className="float-right">
          <span className="text-muted">You will be redirected in <span id="countdown">{countdown}</span> seconds...</span>
          <button onClick={() => document.location.href = '/'} className="btn btn-success">Return</button>
        </div>
      </div>
    </section>
  );
};

export default PaymentFailed;
