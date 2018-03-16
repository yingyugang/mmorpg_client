using System;
using System.IO;
using System.Text;

public static class FileManager
{
    public static bool Exists(string path)
    {
        return File.Exists(path);
    }

    public static bool DirectoryExists(string path)
    {
        return Directory.Exists(path);
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
        FileManager.WriteAllBytes(filePath, bytes);
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

    public static bool CheckIfCSVExist()
    {
        if (FileManager.DirectoryExists(PathConstant.CLIENT_CSV_PATH))
        {
            string[] stringArray = FileManager.GetFiles(PathConstant.CLIENT_CSV_PATH, "*", SearchOption.TopDirectoryOnly);
            if (stringArray.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

	public static void CreateDirectory(string path){
		Directory.CreateDirectory (path);
	}
}
