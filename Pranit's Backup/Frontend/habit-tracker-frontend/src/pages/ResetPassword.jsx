import { useState } from "react";
import api from "../api/axios";
import Alert from "../components/Alert";
import { required, minLen } from "../utils/validators";

export default function ResetPassword() {
  const [form, setForm] = useState({
    identifier: "",
    otp: "",
    newPassword: "",
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
      identifier: required(form.identifier),
      otp: required(form.otp),
      newPassword: minLen(form.newPassword, 6),
      confirmPassword:
        form.newPassword !== form.confirmPassword
          ? "Passwords do not match"
          : ""
    };

    setFieldErr(errs);
    if (Object.values(errs).some(v => v)) return;

    try {
      await api.post("/auth/reset-password", form);
      setSuccess("Password reset successful. Please login.");
      setForm({
        identifier: "",
        otp: "",
        newPassword: "",
        confirmPassword: ""
      });
    } catch (err) {
      setError(err.response?.data?.message || "Reset failed");
    }
  };

  return (
    <div className="container mt-5">
      <div className="row justify-content-center">
        <div className="col-12 col-sm-10 col-md-6 col-lg-4">

          <div className="card shadow-sm">
            <div className="card-body">
              <h4 className="text-center mb-4">Reset Password</h4>

              <form onSubmit={submit} noValidate>

                {/* Identifier */}
                <div className="mb-3">
                  <input
                    className={`form-control ${fieldErr.identifier ? "is-invalid" : ""}`}
                    placeholder="Email or Mobile"
                    value={form.identifier}
                    onChange={e =>
                      setForm({ ...form, identifier: e.target.value })
                    }
                  />
                  <div className="invalid-feedback">
                    {fieldErr.identifier}
                  </div>
                </div>

                {/* OTP */}
                <div className="mb-3">
                  <input
                    className={`form-control ${fieldErr.otp ? "is-invalid" : ""}`}
                    placeholder="OTP"
                    value={form.otp}
                    onChange={e =>
                      setForm({ ...form, otp: e.target.value })
                    }
                  />
                  <div className="invalid-feedback">
                    {fieldErr.otp}
                  </div>
                </div>

                {/* New Password */}
                <div className="mb-3">
                  <input
                    type="password"
                    className={`form-control ${fieldErr.newPassword ? "is-invalid" : ""}`}
                    placeholder="New Password"
                    value={form.newPassword}
                    onChange={e =>
                      setForm({ ...form, newPassword: e.target.value })
                    }
                  />
                  <div className="invalid-feedback">
                    {fieldErr.newPassword}
                  </div>
                </div>

                {/* Confirm Password */}
                <div className="mb-4">
                  <input
                    type="password"
                    className={`form-control ${fieldErr.confirmPassword ? "is-invalid" : ""}`}
                    placeholder="Confirm Password"
                    value={form.confirmPassword}
                    onChange={e =>
                      setForm({ ...form, confirmPassword: e.target.value })
                    }
                  />
                  <div className="invalid-feedback">
                    {fieldErr.confirmPassword}
                  </div>
                </div>

                <button className="btn btn-success w-100">
                  Reset Password
                </button>
              </form>

              <Alert type="danger" message={error} />
              <Alert type="success" message={success} />

              <div className="text-center mt-3">
                <small>
                  Back to <a href="/">Login</a>
                </small>
              </div>

            </div>
          </div>

        </div>
      </div>
    </div>
  );
}
