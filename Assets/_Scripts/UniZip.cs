using System;
using System.IO;
using System.IO.Compression;
using System.Text;

public class UniZip
{
    public delegate void ProgressDelegate(string sMessage);

    private static void CompressFile(string sDir, string sRelativePath, GZipStream zipStream)
    {
        //Compress file name
        char[] chars = sRelativePath.ToCharArray();
        zipStream.Write(BitConverter.GetBytes(chars.Length), 0, sizeof(int));
        foreach (char c in chars)
            zipStream.Write(BitConverter.GetBytes(c), 0, sizeof(char));

        //Compress file content
        byte[] bytes = File.ReadAllBytes(Path.Combine(sDir, sRelativePath));
        zipStream.Write(BitConverter.GetBytes(bytes.Length), 0, sizeof(int));
        zipStream.Write(bytes, 0, bytes.Length);
    }

    public static void CompressDirectory(string sInDir, string sOutFile, ProgressDelegate progress)
    {
        string[] sFiles = Directory.GetFiles(sInDir, "*.*", SearchOption.AllDirectories);
        int iDirLen = sInDir[sInDir.Length - 1] == Path.DirectorySeparatorChar ? sInDir.Length : sInDir.Length + 1;

        using (FileStream outFile = new FileStream(sOutFile, FileMode.Create, FileAccess.Write, FileShare.None))
        using (GZipStream str = new GZipStream(outFile, CompressionMode.Compress))
            foreach (string sFilePath in sFiles)
            {
                string sRelativePath = sFilePath.Substring(iDirLen);
                if (progress != null)
                    progress(sRelativePath);
                CompressFile(sInDir, sRelativePath, str);
            }
    }

    private static bool DecompressFile(string sDir, GZipStream zipStream, ProgressDelegate progress)
    {
        //zipStream.Close();
        //Decompress file name
        byte[] bytes = new byte[sizeof(int)];
        int Readed = zipStream.Read(bytes, 0, sizeof(int));
        if (Readed < sizeof(int))
            return false;

        int iNameLen = BitConverter.ToInt32(bytes, 0);
        bytes = new byte[sizeof(char)];
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < iNameLen; i++)
        {
            zipStream.Read(bytes, 0, sizeof(char));
            char c = BitConverter.ToChar(bytes, 0);
            sb.Append(c);
        }
        string sFileName = sb.ToString();
        if (progress != null)
            progress(sFileName);

        //Decompress file content
        bytes = new byte[sizeof(int)];
        zipStream.Read(bytes, 0, sizeof(int));
        int iFileLen = BitConverter.ToInt32(bytes, 0);

        bytes = new byte[iFileLen];
        zipStream.Read(bytes, 0, bytes.Length);

        string sFilePath = Path.Combine(sDir, sFileName);
        string sFinalDir = Path.GetDirectoryName(sFilePath);
        if (!Directory.Exists(sFinalDir))
            Directory.CreateDirectory(sFinalDir);

        using (FileStream outFile = new FileStream(sFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            outFile.Write(bytes, 0, iFileLen);

        return true;
    }


    public static void DecompressToDirectory(string sCompressedFile, string sDir, ProgressDelegate progress)
    {
        using (FileStream inFile = new FileStream(sCompressedFile, FileMode.Open, FileAccess.Read, FileShare.None))
        using (GZipStream zipStream = new GZipStream(inFile, CompressionMode.Decompress, true))
            while (DecompressFile(sDir, zipStream, progress)) ;
    }

    public static void Compress(string directoryPath)
    {
        DirectoryInfo directorySelected = new DirectoryInfo(directoryPath);
        foreach (FileInfo fileToCompress in directorySelected.GetFiles())
        {
            using (FileStream originalFileStream = fileToCompress.OpenRead())
            {
                if ((File.GetAttributes(fileToCompress.FullName) &
                   FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
                {
                    using (FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".gz"))
                    {
                        using (GZipStream compressionStream = new GZipStream(compressedFileStream,
                           CompressionMode.Compress))
                        {
                            StreamExtensions.CopyTo(originalFileStream, compressionStream);
                            //originalFileStream.CopyTo(compressionStream);
                        }
                    }
                    FileInfo info = new FileInfo(directoryPath + "\\" + fileToCompress.Name + ".gz");
                    Console.WriteLine("Compressed {0} from {1} to {2} bytes.",
                    fileToCompress.Name, fileToCompress.Length.ToString(), info.Length.ToString());
                }

            }
        }
    }

    private static void Decompress(FileInfo fileToDecompress)
    {
        using (FileStream originalFileStream = fileToDecompress.OpenRead())
        {
            string currentFileName = fileToDecompress.FullName;
            string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

            using (FileStream decompressedFileStream = File.Create(newFileName))
            {
                using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                {
                    StreamExtensions.CopyTo(decompressionStream, decompressedFileStream);
                    //decompressionStream.CopyTo(decompressedFileStream);
                    Console.WriteLine("Decompressed: {0}", fileToDecompress.Name);
                }
            }
        }
    }

    public static void DecompressFolder(string directoryPath)
    {
        DirectoryInfo directorySelected = new DirectoryInfo(directoryPath);
        foreach (FileInfo fileToDecompress in directorySelected.GetFiles("*.txt"))
        {
            Decompress(fileToDecompress);
        }
    }
}

public static class StreamExtensions
{
    public static void CopyTo(this Stream input, Stream output)
    {
        byte[] buffer = new byte[16 * 1024];
        int bytesRead;
        while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
        {
            output.Write(buffer, 0, bytesRead);
        }
    }
}