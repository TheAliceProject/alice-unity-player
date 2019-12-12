using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using TMPro;

namespace Alice.Tweedle.Parse
{
public class VRBrowser : MonoBehaviour
    {
        public VRBrowserButton fileButton;
        public Transform root;
        public TextMeshProUGUI currentDirectory;
        public ScrollRect rect;
        public Sprite worldSprite;

        private string pwd;
        private List<string> tempDirs = new List<string>();
        // Use this for initialization
        void Start()
        {
            pwd = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            LoadFilesInPath(pwd);
        }

        void Update()
        {
            if(Input.GetAxis("RightThumbstickUpDown") > 0.75f)
                rect.verticalNormalizedPosition += (0.15f / root.childCount);
            else if(Input.GetAxis("RightThumbstickUpDown") < -0.75f)
                rect.verticalNormalizedPosition -= (0.15f / root.childCount);
        }

        public void Navigate(string path)
        {
            if (path.Contains(".."))
            {
                if (pwd.Length <= 3)
                    return;
                if (Directory.GetParent(pwd) != null)
                {
                    string par = Directory.GetParent(pwd).ToString();
                    if (par.Length == 2)
                    {
                        par += "/";
                        pwd = par;
                    }
                    else
                        pwd = Directory.GetParent(pwd).ToString();
                    LoadFilesInPath(pwd);
                }
            }
            else
            {
                try
                {
                    System.IO.Directory.GetDirectories(pwd + path);
                }
                catch (Exception e)
                {
                    return;
                }

                LoadFilesInPath(pwd + path);
                pwd += path;
            }
        }

        public void LoadFilesInPath(string path)
        {
            currentDirectory.text = path;

            for (int j = 0; j < root.childCount; j++)
                Destroy(root.GetChild(j).gameObject);

            // Resize scroll view based on number of items to display
            int numItems = 0;
            foreach (string file in System.IO.Directory.GetFiles(path)){
                if (file.Contains(".a3w") || file.Contains(".A3W"))
                    numItems++;
            }
            numItems += GetNormalDirectories(path).Length;
            numItems++; // ".."
            root.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 45f * (float)numItems + 10f);

            // get directories, plus ".."
            int i = 1;
            VRBrowserButton button = InstantiateBrowserButton();
            button.transform.localPosition = new Vector3(button.transform.localPosition.x, -25f, button.transform.localPosition.z);
            button.label.text = "..";
            foreach (string dir in GetNormalDirectories(path))
            {
                button = InstantiateBrowserButton();
                button.transform.localPosition = new Vector3(button.transform.localPosition.x, -45f * i - 25f, button.transform.localPosition.z);
                int lastSlash = Mathf.Max(dir.LastIndexOf('/'), dir.LastIndexOf('\\'));
                button.label.text = dir.Substring(lastSlash);
                i++;
            }

            foreach (string file in System.IO.Directory.GetFiles(path))
            {
                if (file.Contains(".a3w") || file.Contains(".A3W"))
                {
                    button = InstantiateBrowserButton();
                    button.SetSpriteLabel(worldSprite); // Otherwise folder by default
                    button.transform.localPosition = new Vector3(button.transform.localPosition.x, -45f * i - 25f, button.transform.localPosition.z);
                    button.label.text = Path.GetFileName(file);
                    i++;
                }
            }
        }

        private string[] GetNormalDirectories(string path)
        {
            tempDirs.Clear();
            foreach (string dir in System.IO.Directory.GetDirectories(path))
            {
                // Ignore certain special files
                FileInfo pathInfo = new FileInfo(dir);
                if (pathInfo.Attributes.ToString().Contains("ReparsePoint") || pathInfo.Attributes.ToString().Contains("System"))
                    continue;
                tempDirs.Add(dir);
            }
            return tempDirs.ToArray();
        }

        public void ChooseFileOrDirectory(string path)
        {
            if (path.Contains("/") || path.Contains("\\") || path.Contains(".."))
                Navigate(path);
            else if(path.Contains(".a3w") || path.Contains(".A3W"))
                WorldObjects.GetParser().OpenWorld(pwd + "/" + path);
            else
                Debug.LogError("Invalid path: " + path);
        }

        private VRBrowserButton InstantiateBrowserButton()
        {
            VRBrowserButton button = Instantiate(fileButton, root);
            button.SetBrowser(this);
            return button;
        }
    }
}
