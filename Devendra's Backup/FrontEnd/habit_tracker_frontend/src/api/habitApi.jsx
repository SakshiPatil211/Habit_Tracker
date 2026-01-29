import api from "./axios";

export const getHabits = () => api.get("/habits");

export const logHabit = (habitId, payload) =>
  api.post(`/habits/${habitId}/logs`, payload);

export const getHabitStreak = (habitId) =>
  api.get(`/habits/${habitId}/streak`);
