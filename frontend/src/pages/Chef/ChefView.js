import React, { useState, useEffect } from 'react';
import axios from '../../axiosConfig';
import './ChefView.css';
import Header from '../../components/Header/Header';

const ChefView = () => {
  const [orders, setOrders] = useState([]);
  const [expandedOrderId, setExpandedOrderId] = useState(null);

  useEffect(() => {
    // Fetch orders from the API
    fetchOrders();
  }, []);

  const fetchOrders = () => {
    axios.get('/api/order')
      .then(response => setOrders(response.data))
      .catch(error => console.error('Error fetching orders:', error));
  }

  const handleExpand = (orderId) => {
    setExpandedOrderId(expandedOrderId === orderId ? null : orderId);
  };

  const handleUpdateStatus = async (orderId, status) => {
    try {
      await axios.patch(`/api/order/${orderId}/updateStatus`, status, 
        { headers: { 'Content-Type': 'application/json', 'accept': '*/*' } }
      );
      fetchOrders();
    } catch (error) {
      console.error('Error updating order status:', error);
      alert('There was an error updating your order. Please try again.');
    }
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
    <Header/>
    <div className="chef-view-container">
      <h3>Manage Orders</h3>
      <table className="orders-table">
        <thead>
          <tr>
            <th>ID</th>
            <th style={{minWidth: '100px'}}>Order code</th>
            <th style={{minWidth: '100px'}}>N° Pizzas</th>
            <th style={{minWidth: '200px'}}>Customer</th>
            <th style={{minWidth: '250px'}}>Delivery Address</th>
            <th style={{minWidth: '150px'}}>Status</th>
            <th style={{width: '140px'}}>Actions</th>
          </tr>
        </thead>
        <tbody>
          {orders.map(order => (
            <React.Fragment key={order.id}>
              <tr>
                <td>{order.id}</td>
                <td>{order.uniqueCode}</td>
                <td>{order.orderProducts.length} <br></br>
                  <button className='expand-reduce' onClick={() => handleExpand(order.id)}>
                    {expandedOrderId === order.id ? '▲ Reduce' : '▼ Expand'}
                  </button>
                </td>
                <td>{order.customerName}<br></br>{order.customerPhone}</td>
                <td>{order.deliveryAddress}</td>
                <td><label className={setClassStatus(parseInt(order.status, 10))}>{order.statusDisplay}</label></td>
                <td>
                  <button 
                    className='action button-inpreparation' 
                    onClick={() => handleUpdateStatus(order.id, 1)}
                    disabled={parseInt(order.status, 10) === 1}
                  >In preparation</button>
                  <button 
                    className='action button-completed' 
                    onClick={() => handleUpdateStatus(order.id, 2)}
                    disabled={parseInt(order.status, 10) === 2}
                    >Complete
                  </button>
                  <button 
                    className='action button-delivered' 
                    onClick={() => handleUpdateStatus(order.id, 3)}
                    disabled={parseInt(order.status, 10) === 3}
                    >Delivered
                  </button>
                  <button 
                    className='action button-cancelled' 
                    onClick={() => handleUpdateStatus(order.id, 4)}
                    disabled={parseInt(order.status, 10) === 4}
                    >Cancelled
                  </button>
                </td>
              </tr>
              {expandedOrderId === order.id && (
                <tr className="order-details-row">
                  <td colSpan="6">
                    <div className="order-details-expandible">
                      <h3>Products:</h3>
                      <ul>
                        {order.orderProducts.map(product => (
                          <li key={product.productId}>
                            {product.product.name} - Quantity: {product.quantity}<br></br>
                            Note: {product.note}
                          </li>
                        ))}
                      </ul>
                    </div>
                  </td>
                </tr>
              )}
            </React.Fragment>
          ))}
        </tbody>
      </table>
    </div>
    </>
  );
};

export default ChefView;