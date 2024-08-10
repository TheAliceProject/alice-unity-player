<?php
$directory = 'uploads/';
$files = array_diff(scandir($directory), array('..', '.'));

echo json_encode($files);
?>
