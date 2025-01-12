import React, { useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import './OrderSummary.css';
import axios from '../../axiosConfig';

const OrderSummary = ({ order }) => {
  const navigate = useNavigate();
  const location = useLocation();
  const [orderDetails, setOrderDetails] = useState(order);
  const [error, setError] = useState('');

  const handleCheckOrderStatus = async () => {
    try {
      const response = await axios.get(`/api/order/bycode/${orderDetails.uniqueCode}`);
      setOrderDetails(response.data);
      setError('');
    } catch (error) {
      setOrderDetails(null);
      setError('Order not found or invalid code.');
    }
  };

  const calculateTotalCost = () => {
    return orderDetails.orderProducts.reduce((total, op) => total + (op.product.price * op.quantity), 0).toFixed(2);
  };

  const setClassStatus = (status) => {
    switch (status) {
      case 0:
        return 'button-inpending'; // InPending
      case 1:
        return 'button-inpreparation'; // InPreparation
      case 2:
        return 'button-completed'; // Completed
      case 3:
        return 'button-delivered'; // Delivered
      case 4:
        return 'button-cancelled'; // Cancelled
      default:
        return ''; // Default case if status is not recognized
    }
  };

  return (
    <> 
    <div className="order-summary-container">
        {order.id &&
        <>
            <h2>Order Summary</h2>
            <p><strong>Order Date:</strong> {new Date(orderDetails.orderDate).toLocaleDateString()}</p>
            <p><strong>Customer Name:</strong> {orderDetails.customerName}</p>
            <p><strong>Email:</strong> {orderDetails.customerEmail}</p>
            <p><strong>Phone:</strong> {orderDetails.customerPhone}</p>
            <p><strong>Delivery Address:</strong> {orderDetails.deliveryAddress}</p>
        </>
      }
      <h2>Pizzas Ordered:</h2>
      <ul>
        {orderDetails.orderProducts.map(product => (
          <li key={product.productId}>
            {product.product.name} - Quantity: {product.quantity} - total: {(product.quantity * product.product.price).toFixed(2)}€
            <br></br>
            Note: {product.note}
          </li>
        ))}
      </ul>
      <h2>Total Cost: {calculateTotalCost()}€</h2>
      {order.id &&
        <>
            <p className="order-status"><strong>Status:</strong> <label className={setClassStatus(parseInt(order.status, 10))}>{order.statusDisplay}</label></p>
            {location.pathname !== '/' && (
                <button onClick={() => navigate('/')}>Back to Home</button>
            )}
            <button onClick={handleCheckOrderStatus}>Refresh</button>
            {error && <p className="error">{error}</p>}
        </>
      }
    </div>
    </>
  );
};

export default OrderSummary;