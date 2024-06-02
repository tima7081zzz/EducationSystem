import axios from 'axios';
import baseUrl from './config';
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

axios.defaults.baseURL = baseUrl;
axios.defaults.withCredentials = true;

// Add a response interceptor
axios.interceptors.response.use(
  response => {
    // Any status code that lies within the range of 2xx cause this function to trigger
    return response;
  },
  error => {
    // Any status codes that falls outside the range of 2xx cause this function to trigger
    const { status } = error.response;

    if (status === 401) {
      // If 401 error, redirect to login page
      window.location.href = '/login';
    } else if (status === 404) {
      // Show light-red toast for 404 errors
      toast.error('Not found', {
        position: "top-center",
        className: 'toast-light-red'
      });
    } else if (status === 403) {
      // Show light-red toast for 403 errors
      toast.error('Forbidden', {
        position: "top-center",
        className: 'toast-light-red'
      });
    } else {
      // Show light-red toast for other errors
      toast.error('Request failed', {
        position: "top-center",
        className: 'toast-light-red'
      });
    }

    //return Promise.reject(error);
  }
);

export default axios;
