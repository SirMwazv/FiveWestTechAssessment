import React from "react";

function PriceDisplay({ zarPrice }) {
    return (
        <div className="bg-green-100 p-4 mt-5">
            <h2 className="text-xl font-semibold">ZAR Price:</h2>
            <p className="text-2xl">{zarPrice.toFixed(2)} ZAR</p>
        </div>
    );
}

export default PriceDisplay;
