import React, { useEffect, useState } from 'react';

const Confirmation= () => {
  const [countdown, setCountdown] = useState(5);
  
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
    };
  }, [countdown]);

  return (
    <section className="row bg-light rounded">
      <div className="col p-2">
        <h2>Confirmation</h2>
        <p>Your purchase was confirmed!</p>

        <div className="float-right">
          <span className="text-muted">You will be redirected in <span id="countdown">{countdown}</span> seconds...</span>
          <button onClick={() => document.location.href = '/'} className="btn btn-success">Return</button>
        </div>
      </div>
    </section>
  );
};

export default Confirmation;