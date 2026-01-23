export default function ThemeToggle() {
  const toggle = () => {
    const html = document.documentElement;
    const theme = html.getAttribute("data-bs-theme");
    html.setAttribute("data-bs-theme", theme === "dark" ? "light" : "dark");
  };

  return (
    <button
      className="btn btn-outline-secondary btn-sm position-fixed top-0 end-0 m-3"
      onClick={toggle}
    >
      🌙 / ☀️
    </button>
  );
}
