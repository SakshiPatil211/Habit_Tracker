import { useLocation, useNavigate } from "react-router-dom";
import { useState } from "react";
import api from "../api/axios";
import "./Auth.css";

const ResetPassword = () => {
  const location = useLocation();
  const navigate = useNavigate();

  const [email, setEmail] = useState(location.state?.email || "");
  const [otp, setOtp] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setSuccess("");

    try {
      await api.post("/api/auth/reset-password", {
        email,
        otp,
        newPassword,
        confirmPassword
      });

      setSuccess("Password reset successful. Please login.");

      setTimeout(() => navigate("/login"), 1500);
    } catch {
      setError("Invalid OTP or password mismatch");
    }
  };

  return (
    <div className="auth-container">
      <div className="auth-box">
        <h2>Reset Password</h2>

        {error && <p className="error">{error}</p>}
        {success && <p className="success">{success}</p>}

        <form onSubmit={handleSubmit}>
          <input type="email" value={email} readOnly />
          <input
            type="text"
            placeholder="Enter OTP"
            value={otp}
            onChange={(e) => setOtp(e.target.value)}
            required
          />
          <input
            type="password"
            placeholder="New Password"
            value={newPassword}
            onChange={(e) => setNewPassword(e.target.value)}
            required
          />
          <input
            type="password"
            placeholder="Confirm Password"
            value={confirmPassword}
            onChange={(e) => setConfirmPassword(e.target.value)}
            required
          />
          <button type="submit">Reset Password</button>
        </form>

        <p className="switch-auth">
          Back to <a onClick={() => navigate("/login")}>Login</a>
        </p>
      </div>
    </div>
  );
};

export default ResetPassword;
