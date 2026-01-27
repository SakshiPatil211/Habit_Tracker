// import { useEffect, useState } from "react";
// import api from "../api/axios";
// import { useNavigate } from "react-router-dom";

// const AddHabit = () => {
//   const navigate = useNavigate();

//   const [categories, setCategories] = useState([]);
//   const [categoryId, setCategoryId] = useState("");
//   const [habitName, setHabitName] = useState("");
//   const [startDate, setStartDate] = useState("");
//   const [error, setError] = useState("");
//   const [success, setSuccess] = useState("");


//   // 🔹 Fetch categories from DB
//   useEffect(() => {
//     api.get("api/categories")
//       .then((res) => setCategories(res.data))
//       .catch(() => setError("Failed to load categories"));
//   }, []);

//   const handleSubmit = async (e) => {
//     e.preventDefault();

//     if (!categoryId || !habitName || !startDate) {
//       setError("All fields are required");
//       return;
//     }

//     const habitData = {
//         categoryId: Number(categoryId),
//         habitName: habitName,
//         description: "Daily habit",
//         startDate: startDate
//     };


//     // try {
//     //   await api.post("api/habits", habitData);
//     //   navigate("/dashboard");
//     // } catch (err) {
//     //   console.error(err);
//     //   setError("Failed to add habit");
//     // }

//     try {
//         await api.post("/api/habits", habitData);

//         setSuccess("Habit added successfully ✅");
//         setError("");

//         // clear form
//         setHabitName("");
//         setCategoryId("");
//         setStartDate("");

//     } catch (err) {
//         console.error(err);
//         setSuccess("");
//         setError("Failed to add habit");
//     }

//   };

//   return (
//     <div className="page-container">
//       <h1>Add Habit</h1>

//       {error && <p style={{ color: "red" }}>{error}</p>}

//       <form onSubmit={handleSubmit}>
//         {/* Category */}
//         <label>Category</label>
//         <select
//           value={categoryId}
//           onChange={(e) => setCategoryId(e.target.value)}
//         >
//           <option value="">Select Category</option>
//             {categories.map((cat) => (
//                 <option key={cat.categoryId} value={cat.categoryId}>
//                 {cat.categoryName}
//           </option>
// ))}

//         </select>

//         {/* Habit Name */}
//         <label>Habit Name</label>
//         <input
//           type="text"
//           placeholder="Enter habit name"
//           value={habitName}
//           onChange={(e) => setHabitName(e.target.value)}
//         />

//         {/* Frequency */}
//         <label>Frequency</label>
//         <input type="text" value="Daily" disabled />

//         {/* Start Date */}
//         <label>Start Date</label>
//         <input
//           type="date"
//           value={startDate}
//           onChange={(e) => setStartDate(e.target.value)}
//         />

//         <button type="submit">Add Habit</button>
//         {success && <p style={{ color: "green" }}>{success}</p>}
//         {error && <p style={{ color: "red" }}>{error}</p>}

//       </form>
//     </div>
//   );
// };

// export default AddHabit;


import { useEffect, useState } from "react";
import api from "../api/axios";
import { useNavigate } from "react-router-dom";
import "./AddHabit.css";

const AddHabit = () => {
  const navigate = useNavigate();

  const [categories, setCategories] = useState([]);
  const [categoryId, setCategoryId] = useState("");
  const [habitName, setHabitName] = useState("");
  const [startDate, setStartDate] = useState("");
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  // 🔹 Fetch categories from DB on component mount
  useEffect(() => {
    api.get("/api/categories")
      .then((res) => setCategories(res.data))
      .catch(() => setError("Failed to load categories"));
  }, []);

  // 🔹 Validate form fields
  const validateForm = () => {
    if (!categoryId) {
      setError("Please select a category");
      return false;
    }

    if (!habitName) {
      setError("Habit name is required");
      return false;
    }

    if (habitName.length < 3) {
      setError("Habit name must be at least 3 characters long");
      return false;
    }

    if (!startDate) {
      setError("Please select a start date");
      return false;
    }

    const today = new Date().toISOString().split("T")[0]; // format YYYY-MM-DD
    if (startDate < today) {
      setError("Start date cannot be in the past");
      return false;
    }

    // Clear previous error if all validations pass
    setError("");
    return true;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    // 🔹 Check validations before submitting
    if (!validateForm()) return;

    const habitData = {
      categoryId: Number(categoryId),
      habitName: habitName,
      description: "Daily habit",
      startDate: startDate,
    };

    try {
      await api.post("/api/habits", habitData);

      setSuccess("Habit added successfully ✅");
      setError("");

      // Clear form after successful submission
      setHabitName("");
      setCategoryId("");
      setStartDate("");

      // Optionally redirect to dashboard after 2 seconds
      // setTimeout(() => navigate("/dashboard"), 2000);

    } catch (err) {
      console.error(err);
      setSuccess("");
      setError("Failed to add habit. Please try again.");
    }
  };

  return (
    <div className="page-container">
      <h1>Add Habit</h1>

      {/* Display error or success messages */}
      {error && <p style={{ color: "red" }}>{error}</p>}
      {success && <p style={{ color: "green" }}>{success}</p>}

      <form onSubmit={handleSubmit}>
        {/* Category Selection */}
        <label>Category</label>
        <select
          value={categoryId}
          onChange={(e) => setCategoryId(e.target.value)}
        >
          <option value="">Select Category</option>
          {categories.map((cat) => (
            <option key={cat.categoryId} value={cat.categoryId}>
              {cat.categoryName}
            </option>
          ))}
        </select>

        {/* Habit Name Input */}
        <label>Habit Name</label>
        <input
          type="text"
          placeholder="Enter habit name"
          value={habitName}
          onChange={(e) => setHabitName(e.target.value)}
        />

        {/* Frequency (fixed) */}
        <label>Frequency</label>
        <input type="text" value="Daily" disabled />

        {/* Start Date Input */}
        <label>Start Date</label>
        <input
          type="date"
          value={startDate}
          onChange={(e) => setStartDate(e.target.value)}
        />

        {/* Submit Button */}
        <button type="submit">Add Habit</button>
      </form>
    </div>
  );
};

export default AddHabit;