import { useEffect, useState } from "react";
import {
  getDashboardSummary,
  getTodayHabits,
  getTopStreaks,
} from "../api/dashboardApi";
import "./Dashboard.css";

const Dashboard = () => {
  const [summary, setSummary] = useState(null);
  const [todayHabits, setTodayHabits] = useState([]);
  const [topStreaks, setTopStreaks] = useState([]);

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchDashboard = async () => {
      try {
        setLoading(true);

        const [summaryRes, todayRes, streaksRes] = await Promise.all([
          getDashboardSummary(),
          getTodayHabits(),
          getTopStreaks(),
        ]);

        setSummary(summaryRes.data);
        setTodayHabits(todayRes.data);
        setTopStreaks(streaksRes.data);

      } catch (err) {
        setError("Failed to load dashboard data");
      } finally {
        setLoading(false);
      }
    };

    fetchDashboard();
  }, []);

  if (loading) return <div className="loading">Loading...</div>;
  if (error) return <div className="error">{error}</div>;

  return (
    <div className="dashboard-container">
      <h1>Dashboard</h1>

      {/* ===== SUMMARY CARDS ===== */}
      <div className="summary-cards">
        <div className="card">
          <h3>Total Habits</h3>
          <p>{summary?.totalHabits}</p>
        </div>

        <div className="card">
          <h3>Completed Today</h3>
          <p>{summary?.completedToday}</p>
        </div>

        <div className="card">
          <h3>Pending Today</h3>
          <p>{summary?.pendingToday}</p>
        </div>

        <div className="card">
          <h3>Longest Streak</h3>
          <p>{summary?.longestStreak}</p>
        </div>
      </div>

      {/* ===== TODAY HABITS ===== */}
      <div className="section">
        <h2>Today Habits</h2>
        {todayHabits.length === 0 ? (
          <p>No habits for today.</p>
        ) : (
          <div className="habit-list">
            {todayHabits.map((h) => (
              <div key={h.habitId} className="habit-item">
                <span>{h.habitName}</span>
                <span className={h.isCompleted ? "done" : "pending"}>
                  {h.isCompleted ? "Done" : "Pending"}
                </span>
              </div>
            ))}
          </div>
        )}
      </div>

      {/* ===== TOP STREAKS ===== */}
      <div className="section">
        <h2>Top Streaks</h2>
        {topStreaks.length === 0 ? (
          <p>No streaks yet.</p>
        ) : (
          <div className="streak-list">
            {topStreaks.map((s, index) => (
              <div key={index} className="streak-item">
                <span>{s.habitName}</span>
                <span>{s.streakDays} days</span>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
};

export default Dashboard;
