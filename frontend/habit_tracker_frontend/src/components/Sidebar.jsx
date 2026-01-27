import { NavLink } from "react-router-dom";
import "./Sidebar.css";

const Sidebar = () => {
  return (
    <aside className="sidebar">
      <h2>Habit Tracker</h2>

      <nav>
        <NavLink to="/dashboard">Dashboard</NavLink>
        <NavLink to="/habits">All Habits</NavLink>
        <NavLink to="/add-habit">➕ Add Habit</NavLink>
      </nav>
    </aside>
  );
};

export default Sidebar;
