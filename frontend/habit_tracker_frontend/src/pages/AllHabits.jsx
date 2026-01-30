import { useEffect, useState } from "react";
import api from "../api/axios";
import "./AllHabits.css";

const AllHabits = () => {
  // -------------------- STATES --------------------
  const [habits, setHabits] = useState([]);
  const [categories, setCategories] = useState([]); // ✅ Needed for edit dropdown
  const [loading, setLoading] = useState(true);

  // Track which habit is in edit mode
  const [editingHabitId, setEditingHabitId] = useState(null);

  // Form data for editing
  const [editForm, setEditForm] = useState({
    habitName: "",
    categoryId: 0
  });

  // -------------------- API CALLS --------------------

  // Fetch all habits
  const fetchHabits = async () => {
    try {
      const res = await api.get("/api/habits");
      setHabits(res.data);
    } catch (err) {
      console.error("Error fetching habits", err);
    } finally {
      setLoading(false);
    }
  };

  // ✅ Fetch categories (required for edit)
  const fetchCategories = async () => {
    try {
      const res = await api.get("/api/categories");
      setCategories(res.data);
    } catch (err) {
      console.error("Error fetching categories", err);
    }
  };

  // Toggle habit completed / active
  const toggleHabit = async (habitId) => {
    try {
      await api.post(`/api/habits/${habitId}/toggle`);
      fetchHabits(); // refresh list
    } catch (err) {
      console.error("Toggle failed", err);
    }
  };

  // Delete habit
  const deleteHabit = async (habitId) => {
    if (!window.confirm("Are you sure you want to delete this habit?")) return;

    try {
      await api.delete(`/api/habits/${habitId}`);
      fetchHabits();
    } catch (err) {
      console.error("Delete failed", err);
    }
  };

  // -------------------- EDIT LOGIC --------------------

  // Start editing a habit
    const startEdit = (habit) => {
        // Find matching category using categoryName
        const matchedCategory = categories.find(
            (c) => c.categoryName === habit.categoryName
        );

        setEditingHabitId(habit.habitId);

        setEditForm({
            habitName: habit.habitName,
            categoryId: matchedCategory?.categoryId || 0
        });
    };


    // Save updated habit
    const saveEdit = async (habitId) => {
    if (!editForm.categoryId) {
        alert("Please select a category");
        return;
    }

    try {
        console.log("Update payload:", editForm);

        await api.put(`/api/habits/${habitId}`, {
            habitName: editForm.habitName,
            categoryId: editForm.categoryId
        });

            setEditingHabitId(null);
            fetchHabits();
        } catch (err) {
            console.error(err.response?.data);
            alert(err.response?.data?.error || "Update failed");
        }
    };


  // Cancel edit mode
  const cancelEdit = () => {
    setEditingHabitId(null);
  };

  // -------------------- EFFECT --------------------
  useEffect(() => {
    fetchHabits();
    fetchCategories(); // ✅ must load categories
  }, []);

  if (loading) return <p>Loading habits...</p>;

  // -------------------- UI --------------------
  return (
    <div className="habits-container">
      <h2>All Habits</h2>

      <table className="habits-table">
        <thead>
          <tr>
            <th>Category</th>
            <th>Habit Name</th>
            <th>Status</th>
            <th>Toggle</th>
            <th>Actions</th>
          </tr>
        </thead>

        <tbody>
          {habits.length === 0 ? (
            <tr>
              <td colSpan="5">No habits found</td>
            </tr>
          ) : (
            habits.map((habit) => (
              <tr key={habit.habitId}>
                {/* Category name (read-only) */}
                <td>{habit.categoryName}</td>

                {/* Habit name + Category edit */}
                <td>
                  {editingHabitId === habit.habitId ? (
                    <>
                      {/* Edit habit name */}
                      <input
                        value={editForm.habitName}
                        onChange={(e) =>
                          setEditForm({
                            ...editForm,
                            habitName: e.target.value
                          })
                        }
                      />

                      {/* ✅ Category dropdown */}
                      <select
                        value={editForm.categoryId}
                        onChange={(e) =>
                          setEditForm({
                            ...editForm,
                            categoryId: Number(e.target.value)
                          })
                        }
                      >
                        {categories.map((cat) => (
                          <option
                            key={cat.categoryId}
                            value={cat.categoryId}
                          >
                            {cat.categoryName}
                          </option>
                        ))}
                      </select>
                    </>
                  ) : (
                    habit.habitName
                  )}
                </td>

                {/* Status */}
                <td>
                    {habit.status === "DONE" ? "Completed" : "Pending"}
                </td>

                {/* Toggle switch */}
                <td>
                  <label className="switch">
                    <input
                      type="checkbox"
                      checked={habit.isCompleted}
                      onChange={() => toggleHabit(habit.habitId)}
                    />
                    <span className="slider"></span>
                  </label>
                </td>

                {/* Actions */}
                <td>
                  {editingHabitId === habit.habitId ? (
                    <>
                      <button
                        className="edit-btn"
                        onClick={() => saveEdit(habit.habitId)}
                      >
                        Save
                      </button>
                      <button
                        className="delete-btn"
                        onClick={cancelEdit}
                      >
                        Cancel
                      </button>
                    </>
                  ) : (
                    <>
                      <button
                        className="edit-btn"
                        onClick={() => startEdit(habit)}
                      >
                        Edit
                      </button>
                      <button
                        className="delete-btn"
                        onClick={() =>
                          deleteHabit(habit.habitId)
                        }
                      >
                        Delete
                      </button>
                    </>
                  )}
                </td>
              </tr>
            ))
          )}
        </tbody>
      </table>
    </div>
  );
};

export default AllHabits;
