import { useState } from "react";
import api from "../api/axios";
import Alert from "../components/Alert";
import { required } from "../utils/validators";

export default function ForgotPassword() {
  const [identifier, setIdentifier] = useState("");
  const [channel, setChannel] = useState("SMS");

  const [fieldErr, setFieldErr] = useState("");
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  const submit = async (e) => {
    e.preventDefault();
    setError("");
    setSuccess("");

    // 🔍 Validation
    const err = required(identifier);
    setFieldErr(err);
    if (err) return;

    try {
      await api.post("/auth/forgot-password", {
        identifier,
        channel
      });
      setSuccess("OTP sent successfully. Please check your phone/email.");
    } catch (err) {
      setError(err.response?.data?.message || "Failed to send OTP");
    }
  };

  return (
    <div className="container mt-5">
      <div className="row justify-content-center">
        <div className="col-12 col-sm-10 col-md-6 col-lg-4">

          <div className="card shadow-sm">
            <div className="card-body">
              <h4 className="text-center mb-4">Forgot Password</h4>

              <form onSubmit={submit} noValidate>

                {/* Identifier */}
                <div className="mb-3">
                  <input
                    className={`form-control ${fieldErr ? "is-invalid" : ""}`}
                    placeholder="Email or Mobile Number"
                    value={identifier}
                    onChange={(e) => setIdentifier(e.target.value)}
                  />
                  <div className="invalid-feedback">
                    {fieldErr}
                  </div>
                </div>

                {/* Channel */}
                <div className="mb-4">
                  <select
                    className="form-select"
                    value={channel}
                    onChange={(e) => setChannel(e.target.value)}
                  >
                    <option value="SMS">Send via SMS</option>
                    <option value="EMAIL">Send via Email</option>
                  </select>
                </div>

                <button className="btn btn-warning w-100">
                  Send OTP
                </button>
              </form>

              <Alert type="danger" message={error} />
              <Alert type="success" message={success} />

              <div className="text-center mt-3">
                <small>
                  Remembered your password? <a href="/">Login</a>
                </small>
              </div>

            </div>
          </div>

        </div>
      </div>
    </div>
  );
}
