import React, { useState, useEffect } from 'react';
import axios from '../../axiosConfig';
import './CustomerView.css';
import OrderSummary from '../../components/Order/OrderSummary';
import Header from '../../components/Header/Header';

const CustomerView = () => {
  const [products, setProducts] = useState([]);
  const [orderProducts, setOrderProducts] = useState([]);
  const [showPersonalInfoForm, setShowPersonalInfoForm] = useState(false);
  const [customerName, setCustomerName] = useState('');
  const [customerEmail, setCustomerEmail] = useState('');
  const [customerPhone, setCustomerPhone] = useState('');
  const [deliveryAddress, setDeliveryAddress] = useState('');
  const [order, setOrder] = useState(null);
  const [preOrderSubmit, setPreOrderSubmit] = useState(null);

  useEffect(() => {
    // Fetch available products (pizzas) from the API
    axios.get('/api/menu/products')
      .then(response => setProducts(response.data))
      .catch(error => console.error('Error fetching products:', error));
  }, []);

  const handleQuantityChange = (product, delta) => {
    setOrderProducts(prevOrderProducts => {
      const existingProduct = prevOrderProducts.find(op => op.productId === product.id);
      if (existingProduct) {
        // Update quantity if product already in order
        const newQuantity = existingProduct.quantity + delta;
        return prevOrderProducts.map(op =>
          op.productId === product.id ? { ...op, quantity: Math.max(newQuantity, 0) } : op
        );
      } else {
        // Add new product to order with initial quantity
        return [...prevOrderProducts, { productId: product.id, quantity: Math.max(delta, 0), product, note: '' }];
      }
    });
  };

  const handleNotesChange = (productId, note) => {
    setOrderProducts(prevOrderProducts => {
      return prevOrderProducts.map(op =>
        op.productId === productId ? { ...op, note } : op
      );
    });
  };

  const handleProceedToCheckout = () => {
    // Filtra i prodotti per includere solo quelli con quantità maggiore di zero
    const filteredOrderProducts = orderProducts.filter(op => op.quantity > 0);
  
    // Verifica se ci sono prodotti da ordinare
    if (filteredOrderProducts.length === 0) {
      alert('Please select at least one product with a quantity greater than zero.');
      return;
    }
    setShowPersonalInfoForm(true);

    const orderData = {
        customerName,
        customerEmail,
        customerPhone,
        deliveryAddress,
        orderProducts: filteredOrderProducts
      };
      setPreOrderSubmit(orderData);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
  
    // Filtra i prodotti per includere solo quelli con quantità maggiore di zero
    const filteredOrderProducts = orderProducts.filter(op => op.quantity > 0);
  
    // Verifica se ci sono prodotti da ordinare
    if (filteredOrderProducts.length === 0) {
      alert('Please select at least one product with a quantity greater than zero.');
      return;
    }
  
    const orderData = {
      customerName,
      customerEmail,
      customerPhone,
      deliveryAddress,
      orderProducts: filteredOrderProducts
    };
  
    try {
      const response = await axios.post('/api/order', orderData);
      setOrder(response.data);
      
      alert('Order placed successfully!');
    } catch (error) {
      console.error('Error placing order:', error);
      alert('There was an error placing your order. Please try again.');
    }
  };

  return (
    <>
    <Header/>
    <div className="customer-view-container">
      {!order ? !showPersonalInfoForm ? (
        <>
          <h2>Select Your Pizzas</h2>
          {products.map(product => (
            <div key={product.id} className="product-selection">
              <img src={product.imageUrl} alt={product.name} className="product-image" />
              <div className="product-details">
                <h3>{product.name}</h3>
                <p>{product.description}</p>
                <p><strong>Price:</strong> {product.price}€</p>
                <div className="quantity-control">
                  <label>Quantity:</label>
                  <button onClick={() => handleQuantityChange(product, -1)}>-</button>
                  <input
                      type="number"
                      value={orderProducts.find(op => op.productId === product.id)?.quantity || 0}
                      readOnly
                  />
                  <button onClick={() => handleQuantityChange(product, 1)}>+</button>
                </div>
                <div className="notes-control">
                  <label>Notes for Chef:</label>
                  <input
                    type="text"
                    value={orderProducts.find(op => op.productId === product.id)?.note || ''}
                    onChange={(e) => handleNotesChange(product.id, e.target.value)}
                  />
                </div>
              </div>
            </div>
          ))}
          <button onClick={handleProceedToCheckout} disabled={products.length === 0}>Proceed to Checkout</button>
        </>
      ) : (
        <form onSubmit={handleSubmit}>
          <h1>Enter Your Information</h1>
          <div className="form-group">
            <label>Name:</label>
            <input
              type="text"
              value={customerName}
              onChange={(e) => setCustomerName(e.target.value)}
              required
            />
          </div>
          <div className="form-group">
            <label>Email:</label>
            <input
              type="email"
              value={customerEmail}
              onChange={(e) => setCustomerEmail(e.target.value)}
              required
            />
          </div>
          <div className="form-group">
            <label>Phone:</label>
            <input
              type="tel"
              value={customerPhone}
              onChange={(e) => setCustomerPhone(e.target.value)}
              required
            />
          </div>
          <div className="form-group">
            <label>Delivery Address:</label>
            <input
              type="textarea"
              value={deliveryAddress}
              onChange={(e) => setDeliveryAddress(e.target.value)}
              required
            />
          </div>
          <OrderSummary order={preOrderSubmit}/>
          <button onClick={() => setShowPersonalInfoForm(false)}>Back</button>
          <button type="submit">Send Order</button>
        </form>
      ) : (
        <OrderSummary order={order}/>
      )}
    </div>
    </>
  );
};

export default CustomerView;