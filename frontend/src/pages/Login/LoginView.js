import React, { useState } from 'react';
import axios from '../../axiosConfig';
import { useNavigate } from 'react-router-dom';
import Header from '../../components/Header/Header';
import './LoginView.css'

const Login = () => {
  const [nickName, setNickName] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();

    try {
      const response = await axios.post('/api/auth/login', 
        { nickName, password, },
        { headers: { 'Content-Type': 'application/json', 'accept': 'text/plain' } }
      );

      // Estrai il token JWT dall'header della risposta
      const token = response.headers['x-token'];
      if (token) {
        localStorage.setItem('jwtToken', token); // Salva il token nel localStorage
        navigate('/chef'); // Reindirizza alla pagina protetta
      }
    } catch (error) {
      console.error('Login failed:', error);
      alert('Login failed. Please check your credentials.');
    }
  };

  return (
    <><Header/>
    <div className="login-container">
      <h1>Login</h1>
      <form onSubmit={handleLogin} className="login-form">
        <div className="form-group">
          <label>NickName:</label>
          <input
            type="text"
            value={nickName}
            onChange={(e) => setNickName(e.target.value)}
            required
          />
        </div>
        <div className="form-group">
          <label>Password:</label>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        <button type="submit" className="login-button">Login</button>
      </form>
    </div>
    </>
  );
};

export default Login;