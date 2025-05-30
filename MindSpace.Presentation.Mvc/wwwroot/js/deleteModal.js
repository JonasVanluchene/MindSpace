const idInput = document.querySelector("[name='Id']");
const titleSpan = document.querySelector('#entry-title');
const deleteButtons = document.querySelectorAll(".btn-delete");

deleteButtons.forEach(button => {
    button.addEventListener("click", (e) => {
        const id = button.getAttribute('data-id');
        const title = button.getAttribute('data-title');

        titleSpan.textContent = title;
        idInput.value = id;
        
    });
});