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
    mobile: "",
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

    if (!/^\d{10}$/.test(formData.mobile))
      err.mobile = "Mobile must be 10 digits";

    if (!/\S+@\S+\.\S+/.test(formData.email))
      err.email = "Invalid email format";

    if (formData.password.length < 6)
      err.password = "Password must be at least 6 characters";

    if (!formData.dob)
      err.dob = "Date of birth is required";

    setErrors(err);
    return Object.keys(err).length === 0;
  };

  const handleSubmit = async (e) => {
  e.preventDefault();

  setSuccessMsg("");
  setErrorMsg("");

  if (!validate()) return;

  try {
    await api.post("/auth/register", formData);

    setSuccessMsg("Registration successful! Please login.");

    // optional: clear form
    setFormData({
      firstName: "",
      middleName: "",
      lastName: "",
      username: "",
      mobile: "",
      email: "",
      password: "",
      dob: ""
    });

  } catch (error) {
    if (error.response && error.response.data) {
      setErrorMsg(error.response.data.message);
    } else {
      setErrorMsg("Registration failed. Try again later.");
    }
  }
};


  return (
    <div className="auth-container">
      <div className="auth-box">
        <h2>Create Account</h2>

        {successMsg && <p className="success">{successMsg}</p>}
        {errorMsg && <p className="error">{errorMsg}</p>}

        <form onSubmit={handleSubmit}>
          <input name="firstName" placeholder="First Name" onChange={handleChange} />
          <span className="error">{errors.firstName}</span>

          <input name="middleName" placeholder="Middle Name" onChange={handleChange} />

          <input name="lastName" placeholder="Last Name" onChange={handleChange} />
          <span className="error">{errors.lastName}</span>

          <input name="username" placeholder="Username" onChange={handleChange} />
          <span className="error">{errors.username}</span>

          <input name="mobile" placeholder="Mobile Number" onChange={handleChange} />
          <span className="error">{errors.mobile}</span>

          <input name="email" placeholder="Email" onChange={handleChange} />
          <span className="error">{errors.email}</span>

          <input type="password" name="password" placeholder="Password" onChange={handleChange} />
          <span className="error">{errors.password}</span>

          <input type="date" name="dob" onChange={handleChange} />
          <span className="error">{errors.dob}</span>

          <button type="submit">Register</button>
        </form>

        <p className="switch-auth">
          Already have an account? <a href="/login">Login</a>
        </p>
      </div>
    </div>
  );
};

export default Register;  