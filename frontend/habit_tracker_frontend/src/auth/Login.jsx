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
    const response = await api.post("/api/auth/login", {
      username,
      password
    });

    // backend response
    const { token, username: name, role, userId, expiresAt } = response.data;

    // store everything
    localStorage.setItem("token", token);
    localStorage.setItem("username", name);
    localStorage.setItem("role", role);
    localStorage.setItem("userId", userId);
    localStorage.setItem("expiresAt", expiresAt);

    navigate("/dashboard");

  } catch (err) {
    if (err.response?.data?.message) {
      setError(err.response.data.message);
    } else {
      setError("Invalid username or password");
    }
  }
};


    // ✅ HARDCODED LOGIN
    // if (username === "admin" && password === "admin123") {
    //     localStorage.setItem("token", "dummy-jwt-token"); // fake token
    //     navigate("/dashboard");
    // } else {
    //     setError("Invalid username or password");
    // }

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

        <div className="auth-footer">
          <Link to="/forgot-password" className="forgot-link">
            Forgot Password?
          </Link>

          <p>
            Don’t have an account?
            <Link to="/register"> Register</Link>
          </p>
        </div>

      </div>
    </div>
  );
};

export default Login;
