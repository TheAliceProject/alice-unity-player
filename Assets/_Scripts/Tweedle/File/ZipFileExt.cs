using ICSharpCode.SharpZipLib.Zip;
using UnityEngine;
using System.IO;

namespace Alice.Tweedle.File {
    public static class ZipFileExt {
        public static string ReadEntry(this ZipFile zipFile, string location)
        {
            ZipEntry entry = zipFile.GetEntry(location);
            if (entry == null)
            {
                Debug.Log("Did not find entry for: " + location);
            }
            return ReadEntry(zipFile, entry);
        }

        public static string ReadEntry(this ZipFile zipFile, ZipEntry entry)
        {
            Stream entryStream = zipFile.GetInputStream(entry);
            using (var reader = new StreamReader(entryStream)) {
                return reader.ReadToEnd();
            }
        }

        public static byte[] ReadDataEntry(this ZipFile zipFile, string location)
        {
            ZipEntry entry = zipFile.GetEntry(location);
            if (entry == null)
            {
                var dirPath = Path.GetDirectoryName(location);
                var dirEntry =  zipFile.GetEntry(dirPath);

                Debug.Log("Did not find entry for: " + location + " dir entry " + dirPath);
                return null;
            }
            return ReadDataEntry(zipFile, entry);
        }

        public static byte[] ReadDataEntry(this ZipFile zipFile, ZipEntry entry)
        {
            Stream entryStream = zipFile.GetInputStream(entry);
            using (var reader = new BinaryReader(entryStream)) {
                return reader.ReadBytes((int)entry.Size);
            }
        }
    }
}