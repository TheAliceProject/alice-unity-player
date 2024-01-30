using System;
using System.Collections;
using System.IO;
using UnityEngine.Networking;

namespace Alice.Storage {
    /**
     * A common layer to read in file like assets
     *  - Directly from the file system
     *  - Nested inside a compressed file
     *  - Over the network
     */
    public class StorageReader {
        private Stream m_ReadStream;
        private readonly ExceptionHandler m_ExceptionHandler;
        
        public delegate void SuccessHandler(Stream stream);
        public delegate void ExceptionHandler(Exception e);

        public static IEnumerator Read(string resourceLocation, SuccessHandler successHandler, ExceptionHandler exceptionHandler) {
            var reader = new StorageReader(exceptionHandler);
            yield return reader.Load(resourceLocation);
            successHandler(reader.m_ReadStream);
        }

        private StorageReader(ExceptionHandler exceptionHandler) {
            m_ExceptionHandler = exceptionHandler;
        }

        private IEnumerator Load(string resourceLocation) {
            if (resourceLocation.StartsWith("jar:") || resourceLocation.StartsWith("http:")) {
                // Use UnityWebRequest for web and when reading from inside a compressed file
                yield return LoadAsWebRequest(resourceLocation);
            } else {
                yield return LoadAsFile(resourceLocation);
            }
        }

        private IEnumerator LoadAsFile(string fileName) {
            if (!File.Exists(fileName)) {
                HandleException(new FileNotFoundException(fileName));
            }
            m_ReadStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            yield return null;
        }

        private IEnumerator LoadAsWebRequest(string request) {
            var www = new UnityWebRequest(request) {downloadHandler = new DownloadHandlerBuffer()};
            yield return www.SendWebRequest();
            m_ReadStream = new MemoryStream(www.downloadHandler.data);
        }

        private void HandleException(Exception e) {
            m_ReadStream?.Close();
            m_ExceptionHandler(e);
            throw e;
        }
    }
}