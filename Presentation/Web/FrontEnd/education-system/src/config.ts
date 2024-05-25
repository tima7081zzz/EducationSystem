let baseUrl = '';

if (process.env.NODE_ENV === 'production') {
  baseUrl = 'https://your-production-url.com';
} else {
  baseUrl = 'http://localhost:5000/';
}

export default baseUrl;
