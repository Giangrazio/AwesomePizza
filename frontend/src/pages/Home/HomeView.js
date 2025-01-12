import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from '../../axiosConfig';
import './HomeView.css';
import OrderSummary from '../../components/Order/OrderSummary';
import Header from '../../components/Header/Header';

const HomePage = () => {
  const navigate = useNavigate();
  const [orderCode, setOrderCode] = useState('');
  const [orderDetails, setOrderDetails] = useState(null);
  const [error, setError] = useState('');

  const handleCheckOrderStatus = async () => {
    try {
      const response = await axios.get(`/api/order/bycode/${orderCode}`);
      setOrderDetails(response.data);
      setError('');
      setOrderCode('');
    } catch (error) {
      setOrderDetails(null);
      setError('Order not found or invalid code.');
    }
  };

  return (
    <>
    <Header/>
    <div className="homepage-container">
      <div className="button-container">
        <button onClick={() => navigate('/customer')}>Order Pizza</button>
        <button onClick={() => navigate('/chef')}>Manage Orders</button>
      </div>
      <div className="order-summary-container">
        <h2>Check Your Order Details</h2>
        <input
          type="text"
          placeholder="Enter your order code"
          value={orderCode}
          onChange={(e) => setOrderCode(e.target.value)}
        />
        <button onClick={handleCheckOrderStatus}>Check Order</button>
        {error && <p className="error">{error}</p>}
        {orderDetails && (
          <OrderSummary order={orderDetails}/>
        )}
      </div>
    </div>
    </>
  );
};

export default HomePage;