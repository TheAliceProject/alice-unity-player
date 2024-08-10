<?php
if ($_SERVER['REQUEST_METHOD'] == 'POST') {
    $filename = $_POST['filename'];
    $filepath = 'uploads/' . $filename;

    if (file_exists($filepath)) {
        if (unlink($filepath)) {
            echo "File '$filename' deleted successfully.";
        } else {
            echo "Error deleting file '$filename'.";
        }
    } else {
        echo "File '$filename' not found.";
    }
} else {
    echo "Invalid request method.";
}
?>