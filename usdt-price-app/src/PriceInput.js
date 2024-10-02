import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './PriceInput.css'; // Import your CSS file

const apiUrl = process.env.REACT_APP_API_URL || 'http://localhost:5002';
const PriceInput = () => {
  const [usdtAmount, setUsdtAmount] = useState('');
  const [price, setPrice] = useState(null);

  // Effect to fetch price when usdtAmount changes
  useEffect(() => {
    const fetchPrice = async () => {
      if (usdtAmount) {
        try {
            const response = await axios.get(`${apiUrl}/api/price?usdtQuantity=${usdtAmount}`);
            setPrice(response.data.price_in_zar); 
       } catch (error) {
          console.error('Error fetching price:', error);
        }
      } else {
        setPrice(null); // Reset price if input is empty
      }
    };

    fetchPrice();
  }, [usdtAmount]); // Depend on usdtAmount

  const handleInputChange = (event) => {
    setUsdtAmount(event.target.value);
  };

  return (
    <div className="price-input-container">
      <div className="heading-container">
        <h4>USDT amount to buy</h4>
        <h4>Price:</h4>
      </div>
      <div className="input-price-container">
        <input
          type="number"
          value={usdtAmount}
          onChange={handleInputChange}
          placeholder="Enter USDT amount"
        />
        {price !== null && (
          <span className="price-value">{price}</span>
        )}
      </div>
    </div>
  );
};

export default PriceInput;
