export const formatDDMMYYYYHHMM = (dateInput) => {
  let d = new Date(dateInput);
  if (isNaN(d)) d = new Date(Date.now());

  // Manually add 7 hours (in milliseconds) to the UTC time
  // 7 hours * 60 minutes * 60 seconds * 1000 milliseconds
  const jakartaTime = new Date(d.getTime() + 7 * 60 * 60 * 1000);

  // Use getUTC... methods so the browser doesn't try to change it back
  const day = jakartaTime.getUTCDate().toString().padStart(2, "0");
  const month = (jakartaTime.getUTCMonth() + 1).toString().padStart(2, "0");
  const year = jakartaTime.getUTCFullYear();

  // getUTCHours() natively returns 0-23
  const hours = jakartaTime.getUTCHours().toString().padStart(2, "0");
  const minutes = jakartaTime.getUTCMinutes().toString().padStart(2, "0");

  return `${day}/${month}/${year} ${hours}:${minutes}`;
};
