export default function Alert({ type, message }) {
  if (!message) return null;

  return (
    <div className={`alert alert-${type} mt-3`} role="alert">
      {message}
    </div>
  );
}
