import { useState } from "react";
import api from "../api/axios";
import { required } from "../utils/validators";
import Alert from "../components/Alert";

export default function Login() {
  const [form, setForm] = useState({ username: "", password: "" });
  const [error, setError] = useState("");
  const [fieldErr, setFieldErr] = useState({});

  const submit = async (e) => {
    e.preventDefault();
    setError("");

    const errs = {
      username: required(form.username),
      password: required(form.password)
    };

    setFieldErr(errs);
    if (errs.username || errs.password) return;

    try {
      await api.post("/auth/login", form);
      window.location.href = "/dashboard";
    } catch (err) {
      setError(err.response?.data?.message || "Login failed");
    }
  };

  return (
    <div className="container mt-5">
      <div className="row justify-content-center">
        <div className="col-12 col-sm-10 col-md-6 col-lg-4">
          
          <div className="card shadow-sm">
            <div className="card-body">
              <h3 className="text-center mb-4">Login</h3>

              <form onSubmit={submit} noValidate>
                {/* Username */}
                <div className="mb-3">
                  <input
                    className={`form-control ${
                      fieldErr.username ? "is-invalid" : ""
                    }`}
                    placeholder="Username"
                    value={form.username}
                    onChange={(e) =>
                      setForm({ ...form, username: e.target.value })
                    }
                  />
                  <div className="invalid-feedback">
                    {fieldErr.username}
                  </div>
                </div>

                {/* Password */}
                <div className="mb-3">
                  <input
                    type="password"
                    className={`form-control ${
                      fieldErr.password ? "is-invalid" : ""
                    }`}
                    placeholder="Password"
                    value={form.password}
                    onChange={(e) =>
                      setForm({ ...form, password: e.target.value })
                    }
                  />
                  <div className="invalid-feedback">
                    {fieldErr.password}
                  </div>
                </div>

                <button className="btn btn-primary w-100">
                  Login
                </button>
              </form>

              <Alert type="danger" message={error} />

              <div className="text-center mt-3">
                <a href="/forgot">Forgot password?</a>
              </div>

              <div className="text-center mt-2">
                <small>
                  Don’t have an account? <a href="/signup">Sign up</a>
                </small>
              </div>
            </div>
          </div>

        </div>
      </div>
    </div>
  );
}
