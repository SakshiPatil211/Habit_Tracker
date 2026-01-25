import "./Auth.css";
import { useState } from "react";
import api from "../api/axios";
import { Link } from "react-router-dom";  // <-- add this

const Register = () => {
  const [formData, setFormData] = useState({
    firstName: "",
    middleName: "",
    lastName: "",
    username: "",
    mobileNumber: "",
    email: "",
    password: "",
    dob: ""
  });

  const [errors, setErrors] = useState({});
  const [successMsg, setSuccessMsg] = useState("");
  const [errorMsg, setErrorMsg] = useState("");


  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const validate = () => {
    const err = {};

    if (!/^[A-Za-z]+$/.test(formData.firstName))
      err.firstName = "First name should contain only letters";

    if (!/^[A-Za-z]+$/.test(formData.lastName))
      err.lastName = "Last name should contain only letters";

    if (formData.username.length < 4)
      err.username = "Username must be at least 4 characters";

    if (!/^\d{10}$/.test(formData.mobileNumber))
      err.mobile = "Mobile must be 10 digits";

    if (!/\S+@\S+\.\S+/.test(formData.email))
      err.email = "Invalid email format";

    if (formData.password.length < 6)
      err.password = "Password must be at least 6 characters";

    // if (!formData.dob)
    //   err.dob = "Date of birth is required";

    setErrors(err);
    return Object.keys(err).length === 0;
  };

  const handleSubmit = async (e) => {
  e.preventDefault();

  setSuccessMsg("");
  setErrorMsg("");

  if (!validate()) return;

  const payload = {
    firstName: formData.firstName,
    middleName: formData.middleName || null,
    lastName: formData.lastName,
    username: formData.username,
    email: formData.email,
    mobileNumber: formData.mobileNumber,   // ✅ FIXED
    password: formData.password,
    dob: formData.dob ? formData.dob : null 
  };

  try {
    await api.post("/api/auth/signup", payload);

    setSuccessMsg("Registration successful! Please login.");

    setFormData({
      firstName: "",
      middleName: "",
      lastName: "",
      username: "",
      mobileNumber: "",
      email: "",
      password: "",
      dob: ""
    });
  } catch (error) {
    if (error.response?.data?.error) {
      const firstError = Object.values(error.response.data.errors)[0][0];
      setErrorMsg(firstError);
      //setErrorMsg(error.response.data.error);
    } else {
      setErrorMsg("Registration failed. Try again later.");
    }
  }
};


  return (
    <div className="auth-container">
      <div className="auth-box">
        <h2>Create Account</h2>

        {/* {successMsg && <p className="success">{successMsg}</p>} */}
        {errorMsg && <p className="error">{errorMsg}</p>}

        {!successMsg && (
    <form onSubmit={handleSubmit}>
    <input
      name="firstName"
      placeholder="First Name"
      value={formData.firstName}
      onChange={handleChange}
    />
    <span className="error">{errors.firstName}</span>

    <input
      name="middleName"
      placeholder="Middle Name"
      value={formData.middleName}
      onChange={handleChange}
    />

    <input
      name="lastName"
      placeholder="Last Name"
      value={formData.lastName}
      onChange={handleChange}
    />
    <span className="error">{errors.lastName}</span>

    <input
      name="username"
      placeholder="Username"
      value={formData.username}
      onChange={handleChange}
    />
    <span className="error">{errors.username}</span>

    <input
      name="mobileNumber"
      placeholder="Mobile Number"
      value={formData.mobileNumber}
      onChange={handleChange}
    />
    <span className="error">{errors.mobile}</span>

    <input
      name="email"
      placeholder="Email"
      value={formData.email}
      onChange={handleChange}
    />
    <span className="error">{errors.email}</span>

    <input
      type="password"
      name="password"
      placeholder="Password"
      value={formData.password}
      onChange={handleChange}
    />
    <span className="error">{errors.password}</span>

    <input
      type="date"
      name="dob"
      value={formData.dob}
      onChange={handleChange}
    />
    <span className="error">{errors.dob}</span>

    <button type="submit">Register</button>
  </form>
)}


        <p className="switch-auth">
          Already have an account? <a href="/login">Login</a>
        </p>
      </div>
    </div>
  );
};

export default Register;  