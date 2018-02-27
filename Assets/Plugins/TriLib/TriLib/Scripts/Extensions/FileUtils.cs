using System;
using System.IO;

namespace TriLib {
    /// <summary>
    /// Contains IO helper functions.
    /// </summary>
	public static class FileUtils
    {
		/// <summary>
		/// Gets the file directory.
		/// </summary>
		/// <returns>The file directory.</returns>
		/// <param name="filename">Filename.</param>
        public static string GetFileDirectory(string filename) {
            var lastDash = filename.LastIndexOf('/');
            return filename.Substring(0, lastDash);
        }

		/// <summary>
		/// Gets the filename without extension.
		/// </summary>
		/// <returns>The filename without extension.</returns>
		/// <param name="filename">Filename.</param>
        public static string GetFilenameWithoutExtension(string filename) {
            var lastDash = filename.LastIndexOf('/');
            var lastDot = filename.LastIndexOf('.');
            return filename.Substring(lastDash, lastDot);
        }

		/// <summary>
		/// Gets the path filename.
		/// </summary>
		/// <returns>The filename.</returns>
		/// <param name="path">Path.</param>
		public static string GetFilename (string path) {
			var filename = Path.GetFileName (path);
			if (path == filename) {
				var indexOfBackslash = path.LastIndexOf ("\\");
				if (indexOfBackslash >= 0) {
					return path.Substring (indexOfBackslash + 1);
				}
				var indexOfSlash = path.LastIndexOf ("/");
				if (indexOfSlash >= 0) {
					return path.Substring (indexOfSlash + 1);
				}
				return path;
			}
			return filename;
		}

		/// <summary>
		/// Synchronously loads the file data.
		/// </summary>
		/// <returns>The file data.</returns>
		/// <param name="filename">Filename.</param>
		public static byte[] LoadFileData(string filename) {
			try {
				if (filename == null) {
					return new byte[0];
				}
				return File.ReadAllBytes(filename.Replace('\\', '/'));
			} catch (Exception) {
				return new byte[0];
			}
		}
    }
}

