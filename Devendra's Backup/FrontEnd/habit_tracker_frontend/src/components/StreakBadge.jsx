import { useEffect, useState } from "react";
import { getHabitStreak } from "../api/habitApi";

export default function StreakBadge({ habitId, refreshKey }) {
  const [streak, setStreak] = useState(null);

  useEffect(() => {
    let isMounted = true;

    const loadStreak = async () => {
      const res = await getHabitStreak(habitId);
      if (isMounted) setStreak(res.data);
    };

    loadStreak();

    return () => {
      isMounted = false;
    };
  }, [habitId, refreshKey]); // ✅ REQUIRED

  if (!streak) return null;

  return (
    <div className="streak-badge">
      🔥 {streak.currentStreak} | 🏆 {streak.longestStreak}
    </div>
  );
}
