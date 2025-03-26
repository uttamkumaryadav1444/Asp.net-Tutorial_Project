document.addEventListener("DOMContentLoaded", function () {
    const fileInput = document.querySelector("input[type='file']");
    if (fileInput) {
        fileInput.addEventListener("change", function (event) {
            const file = event.target.files[0];
            console.clear();

            if (file) {
                console.log("File selected:");
                console.log("Name:", file.name);
                console.log("Size:", file.size, "bytes");
                console.log("Type:", file.type);

                const allowedTypes = ["image/jpeg", "image/png", "image/gif"];
                const maxSize = 5 * 1024 * 1024; // 5 MB

                if (!allowedTypes.includes(file.type)) {
                    console.error("Unsupported file type:", file.type);
                    alert("Please upload a valid image file (JPG, PNG, GIF).");
                    fileInput.value = "";
                    return;
                }

                if (file.size > maxSize) {
                    console.error("File too large:", file.size);
                    alert("File size must not exceed 5 MB.");
                    fileInput.value = "";
                    return;
                }

                console.log("File is valid.");
            } else {
                console.log("No file selected.");
            }
        });
    } else {
        console.error("File input element not found!");
    }
});
