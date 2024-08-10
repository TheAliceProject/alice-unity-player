### README: Setting Up Apache Server for Uploading and Deleting `.a3w` Files and Configuring Unity

---

#### **Part 1: Setting Up an Apache Server with PHP**

1. **Install Apache and PHP:**
   - On **Ubuntu/Debian**:
     ```bash
     sudo apt update
     sudo apt install apache2 php libapache2-mod-php
     ```
   - On **Windows**:
     - Download and install XAMPP from [Apache Friends](https://www.apachefriends.org/index.html), which includes Apache, PHP, and MySQL.

2. **Configure the Apache Server:**
   - Start the Apache server.
   - On **Linux**, use:
     ```bash
     sudo systemctl start apache2
     ```
   - On **Windows**, open the XAMPP control panel and start the Apache service.

3. **Set Up the Directory Structure:**
   - Navigate to the Apache root directory (usually `/var/www/html` on Linux or `C:\xampp\htdocs` on Windows).
   - Create a folder for your project, e.g., `file_manager`.
   - Inside the project folder, create the following structure:
     ```
     /file_manager
     ├── upload.php
     ├── delete.php
     ├── list_files.php
     ├── list_files_delete.php
     └── uploads/
     ```
   - The `uploads` directory will store the uploaded `.a3w` files.

4. **Create PHP Scripts:**

   - **`upload.php`:** This script handles file uploads.
   - **`delete.php`:** This script handles file deletion.
   - **`list_files.php`:** This script returns a JSON list of files in the `uploads/` directory.
   - **`list_files_delete.php`:** This script does the same thing but the return is formatted for delete dropdown.


2. **Inspector Setup in Unity:**
   - Find the FileDownloader game object on the scene.
   - Replace `http://yourserver.com/` with your actual server URL.
   - To find your server address: open Command Prompt (cmd), run ipconfig, and find the "IPv4 Address" under your active network connection (e.g., 192.168.1.2).

4. **Testing:**
   - Build the Unity project for Android.
   - Upload it to the headset using SideQuest.
   - Play and DO NOT click on the "Restore App" option.
   - The script should automatically download all `.a3w` files from the server.