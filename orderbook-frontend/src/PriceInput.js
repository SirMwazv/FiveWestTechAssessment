import React, { useState, useEffect, useCallback } from "react";
import axios from "axios";
import { debounce } from "lodash";

export default function PriceInput() {
  const [usdtAmount, setUsdtAmount] = useState("");
  const [zarPrice, setZarPrice] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const fetchPrice = useCallback(
    debounce(async (amount) => {
      if (amount === "") {
        setZarPrice(null);
        return;
      }
      setLoading(true);
      setError(null);
      try {
        const response = await axios.get(`http://localhost:5124/api/price`, {
          params: { usdtQuantity: amount },
        });
        setZarPrice(response.data.price_in_zar);
      } catch (err) {
        console.error("Error fetching price:", err);
        setError("Unable to fetch price. Please try again.");
      } finally {
        setLoading(false);
      }
    }, 300),
    []
  );

  useEffect(() => {
    fetchPrice(usdtAmount);
  }, [usdtAmount, fetchPrice]);

  const handleInputChange = (e) => {
    setUsdtAmount(e.target.value);
  };

  return (
    <div className="bg-gray-100 p-6 rounded-lg shadow-md max-w-md mx-auto mt-10">
      <div className="mb-4">
        <label className="block text-gray-700 text-sm font-medium mb-2" htmlFor="usdtAmount">
          USDT amount to buy:
        </label>
        <input
          type="number"
          id="usdtAmount"
          value={usdtAmount}
          onChange={handleInputChange}
          className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
        />
      </div>
      <div className="flex justify-between items-center">
        <span className="text-gray-700 text-lg">Price:</span>
        <span className="text-green-500 text-2xl font-bold">
          {loading ? "Loading..." : zarPrice ? zarPrice.toFixed(4) : "0.0000"}
        </span>
      </div>
      <div className="bg-red-500 p-4">Test Background Color</div>

      {error && <div className="text-red-500 mt-2 text-sm">{error}</div>}
    </div>
  );
}