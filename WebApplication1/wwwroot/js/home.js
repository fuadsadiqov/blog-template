document.addEventListener("DOMContentLoaded", function () {
    let elements = document.querySelectorAll(".lastNewsItem");

    elements.forEach(el => {
    let words = el.innerHTML.split(" ");
        if (words.length > 1) {
    words[words.length - 1] = `<span class="last-word">${words[words.length - 1]}</span>`;
    el.innerHTML = words.join(" ");
        }
    });
});