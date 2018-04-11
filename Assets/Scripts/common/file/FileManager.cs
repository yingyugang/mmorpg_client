using System;
using System.IO;
using UnityEngine;
using System.Text;
using System.Collections.Generic;

public class FileManager
{
	public static bool FileExists (string path)
	{
		return File.Exists (path);
	}

	public static bool DirectoryExists (string path)
	{
		return Directory.Exists (path);
	}

	public static void CreateDirectory(string path){
		Directory.CreateDirectory (path);
	}

	public static void CreateFile(string path){
		if(!File.Exists(path)){
			File.Create (path).Close();
		}
	}

	public static void WriteAllBytes (string filePath, byte[] bytes)
	{
		try {
			string dir = Path.GetDirectoryName (filePath);
			if (!Directory.Exists (dir)) {
				Directory.CreateDirectory (dir);
				#if  UNITY_IOS
				UnityEngine.iOS.Device.SetNoBackupFlag (dir);
				#endif
			}
		} catch (System.Exception e) {
			Debug.Log ("IO error." + e.Message);
		}
		try {
			File.WriteAllBytes (filePath, bytes);
		} catch (System.Exception e) {
			Debug.Log ("IO error ." + e.Message);
		}
		#if  UNITY_IOS
		UnityEngine.iOS.Device.SetNoBackupFlag (filePath);
		#endif
	}

	public static void AppendAllText (string filePath, string str)
	{
		try {
			string dir = Path.GetDirectoryName (filePath);
			if (!Directory.Exists (dir)) {
				Directory.CreateDirectory (dir);
				#if  UNITY_IOS
				UnityEngine.iOS.Device.SetNoBackupFlag (dir);
				#endif
			}
		} catch (System.Exception e) {
			Debug.Log ("IO error." + e.Message);
		}
		try {
			File.AppendAllText (filePath, str);
		} catch (System.Exception e) {
			Debug.Log ("IO error ." + e.Message);
		}
		#if  UNITY_IOS
		UnityEngine.iOS.Device.SetNoBackupFlag (filePath);
		#endif
	}

	public static void WriteString (string filePath, string contents)
	{
		byte[] bytes = Encoding.UTF8.GetBytes (contents);
		FileManager.WriteAllBytes (filePath, bytes);
		#if  UNITY_IOS
		UnityEngine.iOS.Device.SetNoBackupFlag (filePath);
		#endif
	}

	public static string ReadString (string filePath)
	{
		string contents = "";
		StreamReader reader = null;
		try {
			reader = new StreamReader (filePath);
			contents = reader.ReadToEnd ();	
		} catch (System.Exception e) {
			Debug.Log ("IO Read error ." + e.Message);
		} finally {
			if (reader != null) {
				reader.Close ();
				reader.Dispose ();
				reader = null;
			}
		}
		return contents;
	}

	public static byte[] ReadBytes (string filePath)
	{
		StreamReader reader = null;
		byte[] buffer = null;
		try {
			reader = new StreamReader (filePath);
			Stream stream = reader.BaseStream;
			if (stream.CanRead) {
				buffer = new byte[(int)stream.Length];
				stream.Read (buffer, 0, (int)stream.Length);
				stream.Close ();
				stream.Dispose ();
				stream = null;
			}
			reader.Close ();
			reader.Dispose ();
			reader = null;

		} catch (System.Exception e) {
			Debug.Log ("IO Read error ." + e.Message);
		} finally {
			if (reader != null) {
				reader.Close ();
				reader.Dispose ();
				reader = null;
			}
		}
		return buffer;
	}

	public static string GetFileHash (string filePath)
	{
		string hashcode = "";
		FileInfo info = new FileInfo (filePath);
		if (info.Exists) {
			FileStream filestream = null;
			try {
				info.Refresh ();
				filestream = info.Open (FileMode.Open, FileAccess.ReadWrite);
				byte[] bytes = System.Security.Cryptography.SHA1.Create ().ComputeHash (filestream);
				hashcode = BitConverter.ToString (bytes).Replace ("-", "").ToLower ();
			} catch (Exception ex) {
				throw ex;
			} finally {
				if (filestream != null) {
					filestream.Close ();
					filestream.Dispose ();
				}
				filestream = null;
			}
		}
		return hashcode;
	}

	public static void CopyFile (string filePath, string destFilePath)
	{
		if (destFilePath.LastIndexOf ("/") != -1) {
			string dirctory = destFilePath.Substring (0, destFilePath.LastIndexOf ("/"));
			if (!DirectoryExists(dirctory)) {
				CreateDirectory (dirctory);
			}
		}
		FileInfo info = new FileInfo (filePath);
		info.CopyTo (destFilePath, true);
		#if  UNITY_IOS
		UnityEngine.iOS.Device.SetNoBackupFlag (destFilePath);
		#endif
	}

	public static string GetFileNameFromPath(string filePath){
		if (filePath.LastIndexOf ("/") != -1) {
			string dirctory = filePath.Substring (filePath.LastIndexOf ("/") + 1);
			if(dirctory.LastIndexOf(".")!=-1){
				dirctory = dirctory.Substring (0,dirctory.LastIndexOf ("."));
			}
			return dirctory;
		}
		return filePath;
	}

	public static string GetDirectoryNameFromPath(string directoryPath){
		if(directoryPath.LastIndexOf("/")!=-1){
			string directory = directoryPath.Substring (directoryPath.LastIndexOf ("/") + 1);
			return directory;
		}
		return directoryPath;
	}

	public static void DeleteFile (string filePath)
	{
		FileInfo info = new FileInfo (filePath);
		if (info.Exists) {
			try {
				info.Delete ();
			} catch (Exception ex) {
				throw ex;
			} 
		}
	}

	public static void DeleteFolder (string filePath)
	{
		Directory.Delete (filePath, true);
	}

	public static string[] GetFiles (string path, string searchPattern, SearchOption searchOption)
	{
		return Directory.GetFiles (path, searchPattern, searchOption);
	}

	public static string[] GetDirectories (string path, string searchPattern, SearchOption searchOption)
	{
		return Directory.GetDirectories (path, searchPattern, searchOption);
	}
}