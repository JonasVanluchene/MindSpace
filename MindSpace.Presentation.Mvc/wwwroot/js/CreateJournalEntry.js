document.addEventListener('DOMContentLoaded', function () {
    const tagBadges = document.getElementById('tag-badges');
    const selectedTagInputs = document.getElementById('selected-tag-inputs');
    const newTagInput = document.querySelector('input[name="NewTagName"]');
    const addNewTagBtn = document.getElementById('add-new-tag');
    const tagCheckboxes = document.querySelectorAll('.tag-checkbox');

    let selectedTags = new Map(); // id -> name
    let pendingNewTag = null; // Store new tag name until form submission

    function updateDisplay() {
        // Update badges
        tagBadges.innerHTML = '';

        // Add badges for selected existing tags
        selectedTags.forEach((name, id) => {
            const badge = document.createElement('span');
            badge.className = 'badge bg-primary me-2 mb-2';
            badge.innerHTML = `
                ${name} 
                <button type="button" class="btn-close btn-close-white ms-1" 
                        onclick="removeTag('${id}')" aria-label="Remove ${name}">
                </button>
            `;
            tagBadges.appendChild(badge);
        });

        // Add badge for pending new tag
        if (pendingNewTag) {
            const badge = document.createElement('span');
            badge.className = 'badge bg-success me-2 mb-2';
            badge.innerHTML = `
                ${pendingNewTag} (new)
                <button type="button" class="btn-close btn-close-white ms-1" 
                        onclick="removePendingTag()" aria-label="Remove ${pendingNewTag}">
                </button>
            `;
            tagBadges.appendChild(badge);
        }

        // Update hidden inputs for existing tags
        selectedTagInputs.innerHTML = '';
        selectedTags.forEach((name, id) => {
            const input = document.createElement('input');
            input.type = 'hidden';
            input.name = 'SelectedTagIds';
            input.value = id;
            selectedTagInputs.appendChild(input);
        });

        // Show/hide selected tags section
        const hasAnyTags = selectedTags.size > 0 || pendingNewTag;
        document.getElementById('selected-tags-display').style.display = hasAnyTags ? 'block' : 'none';
    }

    // Remove existing tag
    window.removeTag = function (tagId) {
        selectedTags.delete(tagId);
        updateDisplay();

        // Uncheck corresponding checkbox
        const checkbox = document.querySelector(`input[value="${tagId}"]`);
        if (checkbox) checkbox.checked = false;
    };

    // Remove pending new tag
    window.removePendingTag = function () {
        pendingNewTag = null;
        newTagInput.value = '';
        updateDisplay();
    };

    // Handle existing tag selection
    tagCheckboxes.forEach(checkbox => {
        checkbox.addEventListener('change', function () {
            const tagId = this.value;
            const tagName = this.getAttribute('data-tag-name');

            if (this.checked) {
                selectedTags.set(tagId, tagName);
            } else {
                selectedTags.delete(tagId);
            }
            updateDisplay();
        });
    });

    // Handle new tag addition
    function addNewTag() {
        const newTagName = newTagInput.value.trim();
        if (newTagName) {
            // Check if tag name already exists in selected tags
            const existingTagNames = Array.from(selectedTags.values()).map(name => name.toLowerCase());
            const existingCheckboxes = Array.from(tagCheckboxes).map(cb => cb.getAttribute('data-tag-name').toLowerCase());

            if (existingTagNames.includes(newTagName.toLowerCase()) ||
                existingCheckboxes.includes(newTagName.toLowerCase())) {
                // Show error feedback
                newTagInput.classList.add('is-invalid');
                showFeedbackMessage('Tag already exists!', 'error');
                setTimeout(() => newTagInput.classList.remove('is-invalid'), 2000);
                return;
            }

            pendingNewTag = newTagName;
            newTagInput.classList.add('is-valid');
            setTimeout(() => newTagInput.classList.remove('is-valid'), 1000);
            updateDisplay();
        }
    }

    // Show feedback messages
    function showFeedbackMessage(message, type = 'success') {
        const alertClass = type === 'error' ? 'alert-danger' : 'alert-success';
        const alertDiv = document.createElement('div');
        alertDiv.className = `alert ${alertClass} alert-dismissible fade show mt-2`;
        alertDiv.innerHTML = `
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        `;

        // Insert after the new tag input group
        const inputGroup = newTagInput.closest('.input-group');
        inputGroup.parentNode.insertBefore(alertDiv, inputGroup.nextSibling);

        // Auto-remove after 3 seconds
        setTimeout(() => {
            if (alertDiv && alertDiv.parentNode) {
                alertDiv.remove();
            }
        }, 3000);
    }

    // Event listeners
    addNewTagBtn.addEventListener('click', addNewTag);

    newTagInput.addEventListener('keypress', function (e) {
        if (e.key === 'Enter') {
            e.preventDefault();
            addNewTag();
        }
    });

    // Clear validation classes on input
    newTagInput.addEventListener('input', function () {
        this.classList.remove('is-valid', 'is-invalid');
    });

    // Form submission validation
    const form = document.querySelector('form');
    form.addEventListener('submit', function (e) {
        // If there's a pending new tag, make sure the NewTagName field has the value
        if (pendingNewTag && !newTagInput.value.trim()) {
            newTagInput.value = pendingNewTag;
        }
    });

    // Initial display update
    updateDisplay();

    // Initialize any pre-selected tags (for edit scenarios)
    tagCheckboxes.forEach(checkbox => {
        if (checkbox.checked) {
            const tagId = checkbox.value;
            const tagName = checkbox.getAttribute('data-tag-name');
            selectedTags.set(tagId, tagName);
        }
    });
    updateDisplay();
});