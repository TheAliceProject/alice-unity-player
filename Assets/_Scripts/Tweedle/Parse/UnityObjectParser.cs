using System.IO;
using UnityEngine;

using ICSharpCode.SharpZipLib.Zip;
using Alice.Tweedle.VM;
using Alice.Tweedle.File;

namespace Alice.Tweedle.Parse
{
    public class UnityObjectParser : MonoBehaviour
    {
        static string project_ext = "a3p";
        public bool dumpTypeOutlines = false;
        public GameObject canvas;

        private TweedleSystem m_System;
        private VirtualMachine m_VM;
        private Coroutine m_QueueProcessor;

        public void SetCameraType(int type)
        {
            CameraType.SetCameraType(type);
        }

        public void Select(string path)
        {
            string zipPath = path;

            Camera.main.backgroundColor = Color.clear;
            canvas.SetActive(false);

            if (Player.Unity.SceneGraph.Exists) {
                Player.Unity.SceneGraph.Current.Clear();
            }

            m_System?.Unload();
            if (m_QueueProcessor != null)
            {
                StopCoroutine(m_QueueProcessor);
            }

            m_System = new TweedleSystem();
            JsonParser.ParseZipFile(m_System, zipPath);
            m_System.Link();

            if (dumpTypeOutlines)
            {
                m_System.DumpTypes();
            }

            m_System.QueueProgramMain(m_VM);

            StartQueueProcessing();
        }

        // Use this for MonoBehaviour initialization
        void Start()
        {
            m_VM = new VirtualMachine();
        }

        private void StartQueueProcessing()
        {
            m_QueueProcessor = StartCoroutine(m_VM.ProcessQueue());
        }
    }
}
