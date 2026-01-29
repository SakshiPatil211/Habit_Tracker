import api from "./axios";

export const getHabits = () => {
  return api.get("/habits");
};

export const toggleHabit = (habitId) => {
  return api.put(`/habits/${habitId}/toggle`);
};

export const deleteHabit = (habitId) => {
  return api.delete(`/habits/${habitId}`);
};
