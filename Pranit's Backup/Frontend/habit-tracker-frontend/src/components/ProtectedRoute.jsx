import { Navigate } from "react-router-dom";
import { useEffect, useState } from "react";
import { checkAuth } from "../api/auth";

export default function ProtectedRoute({ children }) {
  const [loading, setLoading] = useState(true);
  const [user, setUser] = useState(null);

  useEffect(() => {
    checkAuth().then(u => {
      setUser(u);
      setLoading(false);
    });
  }, []);

  if (loading) return <div className="text-center mt-5">Loading...</div>;
  if (!user) return <Navigate to="/" />;

  return children;
}
