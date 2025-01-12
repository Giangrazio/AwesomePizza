import React from 'react';
import { useNavigate } from 'react-router-dom';
import { jwtDecode } from 'jwt-decode';
import './Header.css';

const Header = () => {
    const navigate = useNavigate();
    const token = localStorage.getItem('jwtToken');
    let userName = '';

    if (token) {
        try {
        const decodedToken = jwtDecode(token);
        userName = decodedToken?.NickName || ''; // Assicurati che il campo corrisponda a quello nel token
        } catch (error) {
        console.error('Failed to decode token:', error);
        }
    }

    const handleLogout = () => {
        localStorage.removeItem('jwtToken');
        navigate('/login');
    };

    return (
        <div className="header-container">
            <div className="logo-container"
                onClick={() => navigate('/')}>
                <img
                src={`${process.env.PUBLIC_URL}/logo512.png`}
                
                alt="Awesome Pizza Logo"
                className="logo"
                />
                <h1>Awesome Pizza</h1>
            </div>
            {token && (
                <div className="user-info">
                    <p>Welcome, {userName}</p><br></br>
                    <button onClick={handleLogout} className="logout-button">Logout</button>
                </div>
            )}
        </div>
    )
}

export default Header;