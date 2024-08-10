<?php
    // Check if the connection is secure (HTTPS)
    if (!empty($_SERVER['HTTPS']) && ('on' == $_SERVER['HTTPS'])) {
        $uri = 'https://'; // If HTTPS, set the URI scheme to 'https://'
    } else {
        $uri = 'http://'; // If not HTTPS, set the URI scheme to 'http://'
    }
    $uri .= $_SERVER['HTTP_HOST']; // Append the host (domain) to the URI
    header('Location: '.$uri.'/dashboard/'); // Redirect to the /dashboard/ path
    exit; // Terminate the script
?>
Something is wrong with the XAMPP installation :-(
