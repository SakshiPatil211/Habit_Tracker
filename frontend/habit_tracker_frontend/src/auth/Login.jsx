import { useState } from "react";
import api from "../api/axios";
import { useNavigate, Link } from "react-router-dom";
import "./Auth.css";

const Login = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const validate = () => {
    if (!username || !password) {
      setError("Username and password are required");
      return false;
    }
    return true;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");

    if (!validate()) return;

    try {
      const response = await api.post("/auth/login", {
        username,
        password
      });

      localStorage.setItem("token", response.data.token);
      navigate("/dashboard");

    } catch (err) {
      if (err.response && err.response.data) {
        setError(err.response.data.message);
      } else {
        setError("Something went wrong. Try again later.");
      }
    }

    // ✅ HARDCODED LOGIN
    // if (username === "admin" && password === "admin123") {
    //     localStorage.setItem("token", "dummy-jwt-token"); // fake token
    //     navigate("/dashboard");
    // } else {
    //     setError("Invalid username or password");
    // }
  };

  return (
    <div className="auth-container">
      <div className="auth-box">
        <h2>Login</h2>

        <form onSubmit={handleSubmit}>
          <input
            placeholder="Username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
          />

          <input
            type="password"
            placeholder="Password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />

          {error && <p className="error">{error}</p>}

          <button type="submit">Login</button>
        </form>

        <p className="switch-auth">
          <Link to="/forgot-password">Forgot Password?</Link>
        </p>

        <p className="switch">
          Don’t have an account? 
          <Link to="/register"> Register</Link>
        </p>
      </div>
    </div>
  );
};

export default Login;
