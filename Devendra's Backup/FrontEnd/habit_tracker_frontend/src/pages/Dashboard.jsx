import { useEffect, useState } from "react";
import api from "../api/axios";
import "./Dashboard.css";

export default function Dashboard() {
  const [habits, setHabits] = useState([]);
  const [categories, setCategories] = useState([]);
  const [selectedCategory, setSelectedCategory] = useState(null);
  const [habitName, setHabitName] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  // ✅ FUNCTION FIRST (VERY IMPORTANT)
  const loadDashboard = async () => {
    try {
      setLoading(true);
      setError("");

      // habits may be empty → NOT an error
      const habitsReq = api.get("/habits").catch(() => ({ data: [] }));
      const categoriesReq = api.get("/habit-categories");

      const [hRes, cRes] = await Promise.all([
        habitsReq,
        categoriesReq
      ]);

      setHabits(hRes.data || []);
      setCategories(cRes.data || []);
    } catch (err) {
      console.error(err);
      setError("Failed to load dashboard");
    } finally {
      setLoading(false);
    }
  };

  // ✅ useEffect AFTER function
  useEffect(() => {
    loadDashboard();
  }, []);

  // ✅ CREATE HABIT
  const createHabit = async () => {
    if (!habitName.trim() || !selectedCategory) return;

    try {
      await api.post("/habits", {
        habitName,
        categoryId: selectedCategory.categoryId
      });

      setHabitName("");
      setSelectedCategory(null);
      loadDashboard();
    } catch (err) {
      console.error(err);
      setError("Failed to create habit");
    }
  };

  if (loading) {
    return <div className="dashboard">Loading...</div>;
  }

  return (
    <div className="dashboard">
      <h1>📊 Dashboard</h1>

      {/* ================= NO HABITS ================= */}
      {habits.length === 0 && (
        <>
          <h2>No habits yet</h2>
          <p>Select a category to start</p>

          <div className="category-list">
            {categories.map((cat) => (
              <button
                key={cat.categoryId}
                className={
                  selectedCategory?.categoryId === cat.categoryId
                    ? "category active"
                    : "category"
                }
                onClick={() => setSelectedCategory(cat)}
              >
                {cat.categoryName}
              </button>
            ))}
          </div>

          {selectedCategory && (
            <div className="add-habit">
              <input
                placeholder="Enter habit name"
                value={habitName}
                onChange={(e) => setHabitName(e.target.value)}
              />
              <button onClick={createHabit}>Add Habit</button>
            </div>
          )}
        </>
      )}

      {/* ================= HABITS LIST ================= */}
      {habits.length > 0 && (
        <div className="habit-list">
          {habits.map((h) => (
            <div key={h.habitId} className="habit-card">
              {h.habitName}
            </div>
          ))}
        </div>
      )}

      {error && <p className="error">{error}</p>}
    </div>
  );
}
