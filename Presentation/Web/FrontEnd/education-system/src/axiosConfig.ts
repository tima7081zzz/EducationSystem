import axios from 'axios';
import baseUrl from './config';
import { useNavigate } from 'react-router-dom';

axios.defaults.baseURL = baseUrl;
axios.defaults.withCredentials = true;

// Add a response interceptor
axios.interceptors.response.use(
  response => {
    // Any status code that lie within the range of 2xx cause this function to trigger
    return response;
  },
  error => {
    // Any status codes that falls outside the range of 2xx cause this function to trigger
    const { status } = error.response;
    if (status === 401) {
      // If 401 error, redirect to login page
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export default axios;
