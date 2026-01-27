import { useState } from "react";
import { logHabit } from "../api/habitApi";

export default function HabitItem({ habit, onUpdated }) {
  const [checked, setChecked] = useState(false);
  const [loading, setLoading] = useState(false);

  const handleChange = async (e) => {
    if (loading) return;

    const isChecked = e.target.checked;
    setChecked(isChecked);
    setLoading(true);

    try {
      await logHabit(habit.habitId, {
        logDate: new Date().toISOString().split("T")[0],
        status: isChecked ? "Done" : "Missed",
      });

      onUpdated(); // ✅ ONLY user-triggered
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="habit-item">
      <input
        type="checkbox"
        checked={checked}
        onChange={handleChange}
      />
      <span>{habit.habitName}</span>
    </div>
  );
}
