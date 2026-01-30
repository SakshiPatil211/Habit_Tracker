import { useEffect, useState } from "react";
import { getHabits } from "../api/habitApi";
import HabitItem from "../components/HabitItem";
import StreakBadge from "../components/StreakBadge";

export default function HabitsPage() {
  const [habits, setHabits] = useState([]);
  const [refreshKey, setRefreshKey] = useState(0);

  useEffect(() => {
    getHabits().then((res) => setHabits(res.data));
  }, []);

  return (
    <div>
      <h2>My Habits</h2>

      {habits.map((habit) => (
        <div key={habit.habitId}>
          <HabitItem
            habit={habit}
            onUpdated={() => setRefreshKey((k) => k + 1)}
          />
          <StreakBadge habitId={habit.habitId} refreshKey={refreshKey} />
        </div>
      ))}
    </div>
  );
}
