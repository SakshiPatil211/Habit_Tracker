import { useState } from "react";
import api from "../api/axios";
import { Link } from "react-router-dom";
import "./Auth.css";

const ForgotPassword = () => {
  const [username, setUsername] = useState("");
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    setMessage("");
    setError("");

    try {
      // Replace this API endpoint with your backend endpoint
      await api.post("/auth/forgot-password", { username });

      setMessage("Password reset link sent to your email.");
    } catch (err) {
      setError("User not found or something went wrong.");
    }
  };

  return (
    <div className="auth-container">
      <div className="auth-box">
        <h2>Forgot Password</h2>

        {message && <p className="success">{message}</p>}
        {error && <p className="error">{error}</p>}

        <form onSubmit={handleSubmit}>
          <input
            placeholder="Enter your username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
          />
          <button type="submit">Send Reset Link</button>
        </form>

        <p className="switch-auth">
          Back to <Link to="/login">Login</Link>
        </p>
      </div>
    </div>
  );
};

export default ForgotPassword;
