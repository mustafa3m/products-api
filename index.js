const express = require('express');
const mysql = require('mysql');

const app = express();
const port = 8080;

// MySQL connection configuration
const db = mysql.createConnection({
  host: process.env.DB_HOST,
  user: process.env.DB_USER,
  password: process.env.DB_PASSWORD,
  database: process.env.DB_NAME
});

// Connect to MySQL
db.connect(err => {
  if (err) {
    console.error('Error connecting to MySQL:', err);
    process.exit(1);
  }
  console.log('Connected to MySQL');
});

// Endpoint to get all products
app.get('/api/products', (req, res) => {
  db.query('SELECT * FROM Products', (err, results) => {
    if (err) {
      console.error('Error fetching products:', err);
      res.status(500).send('Internal Server Error');
      return;
    }
    res.json(results);
  });
});

// Endpoint to get a product by ID
app.get('/api/products/:id', (req, res) => {
  const productId = req.params.id;
  db.query('SELECT * FROM Products WHERE id = ?', [productId], (err, results) => {
    if (err) {
      console.error('Error fetching product:', err);
      res.status(500).send('Internal Server Error');
      return;
    }
    if (results.length === 0) {
      res.status(404).send('Product not found');
      return;
    }
    res.json(results[0]);
  });
});

// Health check endpoint
app.get('/api/health', (req, res) => {
  res.send('API OK');
});

// Start the server
app.listen(port, () => {
  console.log(`API server running on port ${port}`);
});