<?php
// Check if the request method is POST
if ($_SERVER['REQUEST_METHOD'] == 'POST') {
    // Check if a file has been uploaded
    if (isset($_FILES['file'])) {
        $errors = []; // Initialize an array to store any errors
        $path = 'uploads/'; // Define the upload directory
        $extensions = ['a3w']; // Allowed file extensions

        // Retrieve file information
        $file_name = $_FILES['file']['name'];
        $file_tmp = $_FILES['file']['tmp_name'];
        $file_type = $_FILES['file']['type'];
        $file_size = $_FILES['file']['size'];
        
        // Extract the file extension
        $parts = explode('.', $file_name);
        $file_ext = strtolower(end($parts));

        // Define the full path for the uploaded file
        $file = $path . basename($file_name);

        // Check if the file extension is allowed
        if (!in_array($file_ext, $extensions)) {
            $errors[] = 'Extension not allowed: ' . $file_name . ' ' . $file_type;
        }

        // Check if the file size exceeds the limit (10 MB)
        if ($file_size > 10485760) {
            $errors[] = 'File size exceeds limit: ' . $file_name . ' ' . $file_type;
        }

        // If there are no errors, move the uploaded file to the specified directory
        if (empty($errors)) {
            move_uploaded_file($file_tmp, $file);
            echo 'File uploaded successfully.';
        } else {
            // If there are errors, display them
            foreach ($errors as $error) {
                echo $error . '<br>';
            }
        }
    }
}
?>