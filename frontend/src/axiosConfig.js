import axios from 'axios';

const isLocal = window.location.hostname === 'localhost';

axios.defaults.baseURL = isLocal
    ? process.env.REACT_APP_API_URL_LOCAL ?? "http://localhost:3002"
    : process.env.REACT_APP_API_URL_EXTERNAL;

axios.interceptors.request.use(
    (config) => {
      const token = localStorage.getItem('jwtToken');
      if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      }
      return config;
    },
    (error) => Promise.reject(error)
  );

export default axios;