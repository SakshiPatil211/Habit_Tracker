import api from "./axios";

export const getDashboardSummary = () => api.get("/api/dashboard/summary");
export const getTodayHabits = () => api.get("/api/dashboard/today-habits");
export const getTopStreaks = () => api.get("/api/dashboard/top-streaks");
