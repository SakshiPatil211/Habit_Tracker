import { useState } from "react";
import api from "../api/axios";
import Alert from "../components/Alert";
import { required, minLen } from "../utils/validators";

export default function Signup() {
  const [form, setForm] = useState({
    firstName: "",
    middleName: "",
    lastName: "",
    username: "",
    email: "",
    mobileNumber: "",
    dob: "",
    password: "",
    confirmPassword: ""
  });

  const [fieldErr, setFieldErr] = useState({});
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  const submit = async (e) => {
    e.preventDefault();
    setError("");
    setSuccess("");

    // 🔍 Client-side validation
    const errs = {
      firstName: required(form.firstName),
      lastName: required(form.lastName),
      username: required(form.username),
      email: required(form.email),
      mobileNumber: required(form.mobileNumber),
      dob: required(form.dob),
      password: minLen(form.password, 6),
      confirmPassword:
        form.password !== form.confirmPassword
          ? "Passwords do not match"
          : ""
    };

    setFieldErr(errs);
    if (Object.values(errs).some(v => v)) return;

    try {
      await api.post("/auth/signup", form);
      setSuccess("Account created successfully. Please login.");
      setForm({
        firstName: "",
        middleName: "",
        lastName: "",
        username: "",
        email: "",
        mobileNumber: "",
        dob: "",
        password: "",
        confirmPassword: ""
      });
    } catch (err) {
      setError(err.response?.data?.message || "Signup failed");
    }
  };

  return (
    <div className="container mt-5">
      <div className="row justify-content-center">
        <div className="col-12 col-sm-11 col-md-8 col-lg-6">

          <div className="card shadow-sm">
            <div className="card-body">
              <h3 className="text-center mb-4">Create Account</h3>

              <form onSubmit={submit} noValidate>

                {/* First / Middle / Last Name */}
                <div className="row">
                  <div className="col-md-4 mb-3">
                    <input
                      className={`form-control ${fieldErr.firstName ? "is-invalid" : ""}`}
                      placeholder="First Name"
                      value={form.firstName}
                      onChange={e => setForm({ ...form, firstName: e.target.value })}
                    />
                    <div className="invalid-feedback">{fieldErr.firstName}</div>
                  </div>

                  <div className="col-md-4 mb-3">
                    <input
                      className="form-control"
                      placeholder="Middle Name (optional)"
                      value={form.middleName}
                      onChange={e => setForm({ ...form, middleName: e.target.value })}
                    />
                  </div>

                  <div className="col-md-4 mb-3">
                    <input
                      className={`form-control ${fieldErr.lastName ? "is-invalid" : ""}`}
                      placeholder="Last Name"
                      value={form.lastName}
                      onChange={e => setForm({ ...form, lastName: e.target.value })}
                    />
                    <div className="invalid-feedback">{fieldErr.lastName}</div>
                  </div>
                </div>

                {/* Username */}
                <div className="mb-3">
                  <input
                    className={`form-control ${fieldErr.username ? "is-invalid" : ""}`}
                    placeholder="Username"
                    value={form.username}
                    onChange={e => setForm({ ...form, username: e.target.value })}
                  />
                  <div className="invalid-feedback">{fieldErr.username}</div>
                </div>

                {/* Email */}
                <div className="mb-3">
                  <input
                    type="email"
                    className={`form-control ${fieldErr.email ? "is-invalid" : ""}`}
                    placeholder="Email"
                    value={form.email}
                    onChange={e => setForm({ ...form, email: e.target.value })}
                  />
                  <div className="invalid-feedback">{fieldErr.email}</div>
                </div>

                {/* Mobile */}
                <div className="mb-3">
                  <input
                    className={`form-control ${fieldErr.mobileNumber ? "is-invalid" : ""}`}
                    placeholder="Mobile Number"
                    value={form.mobileNumber}
                    onChange={e => setForm({ ...form, mobileNumber: e.target.value })}
                  />
                  <div className="invalid-feedback">{fieldErr.mobileNumber}</div>
                </div>

                {/* DOB */}
                <div className="mb-3">
                  <input
                    type="date"
                    className={`form-control ${fieldErr.dob ? "is-invalid" : ""}`}
                    value={form.dob}
                    onChange={e => setForm({ ...form, dob: e.target.value })}
                  />
                  <div className="invalid-feedback">{fieldErr.dob}</div>
                </div>

                {/* Password */}
                <div className="mb-3">
                  <input
                    type="password"
                    className={`form-control ${fieldErr.password ? "is-invalid" : ""}`}
                    placeholder="Password"
                    value={form.password}
                    onChange={e => setForm({ ...form, password: e.target.value })}
                  />
                  <div className="invalid-feedback">{fieldErr.password}</div>
                </div>

                {/* Confirm Password */}
                <div className="mb-4">
                  <input
                    type="password"
                    className={`form-control ${fieldErr.confirmPassword ? "is-invalid" : ""}`}
                    placeholder="Confirm Password"
                    value={form.confirmPassword}
                    onChange={e => setForm({ ...form, confirmPassword: e.target.value })}
                  />
                  <div className="invalid-feedback">{fieldErr.confirmPassword}</div>
                </div>

                <button className="btn btn-success w-100">
                  Sign Up
                </button>
              </form>

              <Alert type="danger" message={error} />
              <Alert type="success" message={success} />

              <div className="text-center mt-3">
                <small>
                  Already have an account? <a href="/">Login</a>
                </small>
              </div>

            </div>
          </div>

        </div>
      </div>
    </div>
  );
}
