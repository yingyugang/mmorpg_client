using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace BlueNoah.IO
{
    public class FileManager
    {
        public static bool Exists(string path)
        {
            return File.Exists(path);
        }

        public static void CreateDirectoryIfNotExisting(string path){
            if(!DirectoryExisting(path)){
                CreateDirectory(path);
            }
        }

        public static void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public static bool DirectoryExisting(string path)
        {
            return Directory.Exists(path);
        }

        public static void DeleteEmptyDirectory(string path){
            Directory.Delete(path);
        }

        public static void DeleteDirectoryWithSubDirctorysAndFiles(string path){
            Directory.Delete(path, true);
        }

        public static void WriteAllBytes(string filePath, byte[] bytes)
        {
            try
            {
                string dir = Path.GetDirectoryName(filePath);

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
#if UNITY_IOS
                UnityEngine.iOS.Device.SetNoBackupFlag(dir);
#endif
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            try
            {
                File.WriteAllBytes(filePath, bytes);
            }
            catch (Exception e)
            {
                throw e;
            }

#if UNITY_IOS
            UnityEngine.iOS.Device.SetNoBackupFlag(filePath);
#endif
        }

        public static void AppendAllText(string filePath, string str)
        {
            try
            {
                string dir = Path.GetDirectoryName(filePath);

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
#if UNITY_IOS
                    UnityEngine.iOS.Device.SetNoBackupFlag(dir);
#endif
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            try
            {
                File.AppendAllText(filePath, str);
            }
            catch (Exception e)
            {
                throw e;
            }

#if UNITY_IOS
            UnityEngine.iOS.Device.SetNoBackupFlag(filePath);
#endif
        }

        public static void WriteString(string filePath, string contents)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(contents);
            WriteAllBytes(filePath, bytes);
#if UNITY_IOS
            UnityEngine.iOS.Device.SetNoBackupFlag(filePath);
#endif
        }

        public static string ReadString(string filePath)
        {
            string contents;
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(filePath);
                contents = reader.ReadToEnd();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
            }

            return contents;
        }

        public static byte[] ReadBytes(string filePath)
        {
            StreamReader reader = null;
            byte[] buffer = null;

            try
            {
                reader = new StreamReader(filePath);
                Stream stream = reader.BaseStream;

                if (stream.CanRead)
                {
                    buffer = new byte[(int)stream.Length];
                    stream.Read(buffer, 0, (int)stream.Length);
                    stream.Close();
                    stream.Dispose();
                    stream = null;
                }

                reader.Close();
                reader.Dispose();
                reader = null;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
            }

            return buffer;
        }

        public static FileInfo GetFileInfo(string filePath){
            return new FileInfo(filePath);
        }

        public static string GetFileHash(string filePath)
        {
            string hashcode = "";
            var fileInfo = new FileInfo(filePath);

            if (fileInfo.Exists)
            {
                FileStream filestream = null;

                try
                {
                    fileInfo.Refresh();
                    filestream = fileInfo.Open(FileMode.Open, FileAccess.ReadWrite);
                    byte[] bytes = System.Security.Cryptography.SHA1.Create().ComputeHash(filestream);
                    //this is why the hashcode is not same as server generated.
                    hashcode = BitConverter.ToString(bytes).Replace("-", "").ToLower();
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (filestream != null)
                    {
                        filestream.Close();
                        filestream.Dispose();
                    }

                    filestream = null;
                }
            }

            return hashcode;
        }

        public static void CopyFile(string filePath, string destFilePath)
        {
            var fileInfo = new FileInfo(filePath);
            fileInfo.CopyTo(destFilePath, true);
#if UNITY_IOS
            UnityEngine.iOS.Device.SetNoBackupFlag(destFilePath);
#endif
        }

        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                Debug.LogError(string.Format("Folder <color=green>{0}</color> is not existing.",sourceDirName));
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }


        public static void DeleteFile(string filePath)
        {
            var fileInfo = new FileInfo(filePath);

            if (fileInfo.Exists)
            {
                try
                {
                    fileInfo.Delete();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public static void DeleteFolder(string filePath)
        {
            Directory.Delete(filePath, true);
        }

        public static string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            return Directory.GetFiles(path, searchPattern, searchOption);
        }

        public static string[] GetDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            return Directory.GetDirectories(path, searchPattern, searchOption);
        }

        public static string AssetDataPathToSystemPath(string path){
            path = path.Remove(path.IndexOf("Assets",StringComparison.CurrentCulture),6);
            path = Application.dataPath + path;
            return path;
        }

        public static string SystemPathToAssetDataPath(string path){
            if(path.IndexOf(Application.dataPath,StringComparison.CurrentCulture)!=-1){
                path = "Assets" + path.Remove(0,Application.dataPath.Length);
            }
            return path;
        }

        //e.g. "Assets/Resources/Images/image1" ==> "/Images/image1".
        public static string AssetDataPathToResourcesPath(string path)
        {
            if (path.IndexOf("/Resources/", StringComparison.CurrentCulture) != -1)
            {
                string resourcesPath = path.Substring(path.IndexOf("/Resources/", StringComparison.CurrentCulture) + "/Resources/".Length);
                resourcesPath = resourcesPath.Remove(resourcesPath.LastIndexOf(".", StringComparison.CurrentCulture));
                return resourcesPath;
            }
            return path;
        }

        //e.g. "xxxxx/xxxxxx/image1" ==> "image1".
        public static string GetFileNameFromPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;
            string fileName = path;
            if (path.LastIndexOf("/", StringComparison.CurrentCulture) != -1)
            {
                int index = path.LastIndexOf("/", StringComparison.CurrentCulture) + 1;
                fileName = path.Substring(index);
            }
            return fileName;
        }

        //e.g. "xxxxx/image1.jpg" ==> "image1"
        public static string GetFileMain(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;
            if (fileName.LastIndexOf(".", StringComparison.CurrentCulture) != -1)
            {
                int index = fileName.LastIndexOf(".", StringComparison.CurrentCulture);
                fileName = fileName.Substring(0, index);
            }
            return fileName;
        }

        //e.g. "xxxxx/image1.jpg" ==> "jpg"
        public static string GetFilePattern(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;
            if (fileName.LastIndexOf(".", StringComparison.CurrentCulture) != -1)
            {
                int index = fileName.LastIndexOf(".", StringComparison.CurrentCulture) + 1;
                fileName = fileName.Substring(index);
            }
            return fileName;
        }

    }
}