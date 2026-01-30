import { useState } from "react";
import axios from "axios";

export default function Register() {
  const [form, setForm] = useState({
    firstName: "",
    middleName: "",
    lastName: "",
    username: "",
    email: "",
    mobileNumber: "",
    password: "",
    dob: ""
  });

  const handleChange = (e) => {
    setForm({
      ...form,
      [e.target.name]: e.target.value
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      const payload = {
        firstName: form.firstName,
        middleName: form.middleName || null,
        lastName: form.lastName,
        username: form.username,
        email: form.email,
        mobileNumber: form.mobileNumber,
        password: form.password,
        dob: form.dob ? form.dob : null
      };

      await axios.post(
        "http://localhost:5000/api/auth/register",
        payload,
        {
          headers: { "Content-Type": "application/json" }
        }
      );

      alert("✅ Registered successfully");
    } catch (err) {
      console.error(err.response?.data);
      alert(JSON.stringify(err.response?.data?.errors, null, 2));
    }
  };

  return (
    <div style={{ maxWidth: "400px", margin: "auto" }}>
      <h2>Register</h2>

      <form onSubmit={handleSubmit}>
        <input
          name="firstName"
          placeholder="First Name"
          onChange={handleChange}
          required
        />

        <input
          name="middleName"
          placeholder="Middle Name (optional)"
          onChange={handleChange}
        />

        <input
          name="lastName"
          placeholder="Last Name"
          onChange={handleChange}
          required
        />

        <input
          name="username"
          placeholder="Username"
          onChange={handleChange}
          required
        />

        <input
          name="email"
          type="email"
          placeholder="Email"
          onChange={handleChange}
          required
        />

        <input
          name="mobileNumber"
          placeholder="Mobile Number"
          onChange={handleChange}
          required
        />

        <input
          name="password"
          type="password"
          placeholder="Password"
          onChange={handleChange}
          required
        />

        <input
          name="dob"
          type="date"
          onChange={handleChange}
        />

        <button type="submit">Register</button>
      </form>
    </div>
  );
}
