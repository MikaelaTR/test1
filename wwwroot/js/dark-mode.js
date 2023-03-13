const prefersDarkScheme = window.matchMedia("(prefers-color-scheme: light)");
const currentTheme = localStorage.getItem("theme");

if (currentTheme === "dark" || (!currentTheme && prefersDarkScheme.matches)) {
    document.documentElement.classList.add("dark");
} else {
    document.documentElement.classList.remove("dark");
}

function toggleDarkMode() {
    const currentTheme = localStorage.getItem("theme");
    if (currentTheme === "dark") {
        localStorage.setItem("theme", "light");
        document.documentElement.classList.add("light");
        document.documentElement.classList.remove("dark");
    } else {
        localStorage.setItem("theme", "dark");
        document.documentElement.classList.remove("light");
        document.documentElement.classList.add("dark");
    }
}

document.addEventListener("DOMContentLoaded", function () {
    const darkModeToggle = document.getElementById("dark-mode-toggle");
    darkModeToggle.addEventListener("click", toggleDarkMode);
});
