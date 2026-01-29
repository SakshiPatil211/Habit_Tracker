import api from "./axios";

export const getHabits = () => api.get("/habits");

export const getTodayLogs = () =>
  api.get("/habits/logs/today"); // optional
