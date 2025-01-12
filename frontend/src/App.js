import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import CustomerView from './pages/Customer/CustomerView';
import ChefView from './pages/Chef/ChefView';
import HomePage from './pages/Home/HomeView';
import Login from './pages/Login/LoginView';
import ProtectedRoute from './components/Login/ProtectedRoute';

const App = () => (
  <Router>
    <Routes>
      <Route path="/" element={<HomePage />} />
      <Route path="/login" element={<Login />} />
      <Route path="/customer" element={<CustomerView />} />
      <Route path="/chef" element={
        <ProtectedRoute>
          <ChefView />
        </ProtectedRoute>} />
    </Routes>
  </Router>
);

export default App;