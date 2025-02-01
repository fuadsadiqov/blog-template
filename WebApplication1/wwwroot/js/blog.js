document.addEventListener("DOMContentLoaded", function () {
    const tagContainer = document.getElementById("tagContainer");
    const tagInput = document.getElementById("tagInput");
    const blogTagList = document.getElementById("blogTagList");
    const tagList = document.querySelectorAll("[data-tag-name]");
    const addNewTagElement = document.createElement("p");
    const tagInputElement = document.getElementsByName("tags")[0];
    let isClickInside = false;
    const initBlogTags = document.getElementById("blogTags")?.textContent;
    
    addNewTagElement.className = "tag-item p-1 border rounded cursor-pointer text-primary";
    addNewTagElement.style.display = "none";
    addNewTagElement.addEventListener("click", handleNewTagCreation);
    tagContainer.appendChild(addNewTagElement);

    if(initBlogTags){
        setInitBlogTagList(initBlogTags)
    }
   
    tagInput.addEventListener("focus", () => {
        tagContainer.classList.remove("d-none");
    });
    
    tagList.forEach((tag) => {
        tag.addEventListener("click", () => addTagToSelection(tag.textContent, tag.getAttribute("data-tag-name")));
    })

    tagContainer.addEventListener("mousedown", () => {
        isClickInside = true;
    });

    document.addEventListener("mousedown", (event) => {
        setTimeout(() => {
            if (!tagContainer.contains(event.target) && event.target !== tagInput && !isClickInside) {
                tagContainer.classList.add("d-none");
            }
            isClickInside = false;
        }, 100);
    });

    tagInput.addEventListener("keyup", (event) => {
        const searchText = event.target.value.toLowerCase();
        let hasMatch = false;

        tagList.forEach(tag => {
            const match = tag.textContent.toLowerCase().includes(searchText);
            tag.classList.toggle("d-none", !match);
            if (match) hasMatch = true;
        });

        if (!hasMatch && searchText.length > 0) {
            addNewTagElement.textContent = `Add "${tagInput.value}"`;
            addNewTagElement.style.display = "block";
        } else {
            addNewTagElement.style.display = "none";
        }

        if (event.key === "Enter" && !hasMatch && searchText.length > 0) {
            handleNewTagCreation();
        }
    });

    function addTagToSelection(tagText, tagId) {
        if (!document.getElementById(`${tagId}`)) {
            const selectedTag = document.createElement("div");
            selectedTag.className = "selected-tag";
            selectedTag.id = `${tagId}`;
            selectedTag.textContent = tagText;

            selectedTag.addEventListener("click", () => {
                selectedTag.remove()
                setIdToTagInput(blogTagList);
            });
            
            blogTagList.appendChild(selectedTag);
            
            setIdToTagInput(blogTagList);
        }

        tagInput.value = "";
        tagContainer.classList.add("d-none");
    }

    async function handleNewTagCreation() {
        const newTagText = tagInput.value.trim();
        if (!newTagText) return;

        try {
            const response = await fetch("/Tag/Add", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ name: newTagText }),
            });

            if (!response) throw new Error("Failed to create tag");

            const newTag = await response.json();
            const newTagElement = document.createElement("p");
            newTagElement.className = "tag-item cursor-pointer";
            newTagElement.setAttribute("data-tag-name", newTag.id);
            newTagElement.textContent = newTagText;

            newTagElement.addEventListener("click", () => addTagToSelection(newTagText, newTag.id));

            tagContainer.appendChild(newTagElement);

            addNewTagElement.style.display = "none";
            tagInput.value = "";

            addTagToSelection(newTagText, newTag.id);
        } catch (error) {
            console.error("Error creating tag:", error);
        }
    }
    
    function setIdToTagInput(blogTagList){
        let value = ""
        for (var i = 0; i < blogTagList.children.length; i++) {
            value += blogTagList.children[i].id  + ",";
        }
        tagInputElement.value = value;
    }
    
    function setInitBlogTagList(){
        const initBlogTagArray = initBlogTags.split(",").filter(bT => bT.length > 0);
        for(let i = 0; i < tagList.length; i++){
            if(initBlogTagArray.includes(tagList[i].getAttribute("data-tag-name"))){
                addTagToSelection(tagList[i].textContent, tagList[i].getAttribute("data-tag-name"));
            }
        }
        setIdToTagInput(blogTagList);
    }
});