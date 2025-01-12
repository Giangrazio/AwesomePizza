import React from 'react';
import { Navigate } from 'react-router-dom';

const ProtectedRoute = ({ children }) => {
  const token = localStorage.getItem('jwtToken');

  if (!token) {
    // Se il token non è presente, reindirizza alla pagina di login
    return <Navigate to="/login" />;
  }

  return children;
};

export default ProtectedRoute;