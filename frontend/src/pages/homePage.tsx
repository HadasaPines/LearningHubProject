import React from "react";
import { Link } from "react-router-dom";

const HomePage: React.FC = () => {
  return (
    <div className="p-6 text-center">
      <h1 className="text-3xl font-bold mb-4">
        Welcome to Learning Hub!
      </h1>
      <p className="mb-6">
        The system for managing private lessons.
      </p>

      <div className="flex justify-center gap-4">
        <Link to="/login">
          <button className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700">
            Login
          </button>
        </Link>
        <Link to="/register">
          <button className="bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700">
            Register
          </button>
        </Link>
      </div>
    </div>
  );
};

export default HomePage;
