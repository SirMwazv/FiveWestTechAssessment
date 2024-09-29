import React, { useState } from "react";
import axios from "axios";
import PriceDisplay from "./PriceDisplay";

function App() {
  const [usdtAmount, setUsdtAmount] = useState("");
  const [zarPrice, setZarPrice] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  // Handle the form submission to request the ZAR price from API
  const handleSubmit = async (e) => {
    e.preventDefault(); // Prevent default form submission
    setLoading(true);
    setError(null);

    try {
      // Make the GET request to the C# API endpoint
      const response = await axios.get(`http://localhost:5124/api/price`, {
        params: { usdtQuantity: usdtAmount }
      });


      // Set the price from the API response
      setZarPrice(response.data.price_in_zar);
    } catch (err) {
      console.error("Error fetching price:", err);
      setError("Unable to fetch price. Please try again later.");
    } finally {
      setLoading(false);
    }
  };

  return (
      <div className="container mx-auto mt-10">
        <h1 className="text-2xl font-bold mb-5">USDT amount to buy</h1>
        <form onSubmit={handleSubmit} className="mb-5">
          <label className="block text-lg mb-2" htmlFor="usdtAmount">
            USDT amount to buy:
          </label>
          <input
              type="number"
              id="usdtAmount"
              value={usdtAmount}
              onChange={(e) => setUsdtAmount(e.target.value)}
              className="border p-2 w-full mb-4"
              required
          />
          <button
              type="submit"
              className="bg-blue-500 text-white p-2 w-full"
              disabled={loading}
          >
            {loading ? "Calculating..." : "Get ZAR Price"}
          </button>
        </form>

        {/* Conditionally display the price or error */}
        {zarPrice && <PriceDisplay zarPrice={zarPrice} />}
        {error && <div className="text-red-500">{error}</div>}
      </div>
  );
}

export default App;
