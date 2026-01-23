export const required = (v) => !v ? "This field is required" : "";
export const minLen = (v, n) =>
  v.length < n ? `Minimum ${n} characters` : "";
