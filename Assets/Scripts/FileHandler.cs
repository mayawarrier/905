using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/// <summary>
/// FileHandler class manages the creation and saving/loading of files and directories.
/// Filepaths used can use "[]" to refer to the Application.dataPath/_data/ (assets/_data/) folder as a shorthand.
/// Filepaths can also use "[^]" to refer to the Application.persistenDataPath/_data/ folder instead.
/// Supports custom file formats. File extension need not be specified.
/// Example: "[]nameOfFile" or "[^]fileName" are valid filepaths.
/// </summary>
public static class FileHandler {

	/* Need to ensure that data being saved is serializable */

	private const string fileExt = ".xml"; // Default File Extention if none specified.

	/// <summary>
	/// Reads data from file.
	/// </summary>
	/// <returns>The loaded file if read was successful, <c>null</c> or <c>0</c> otherwise.</returns>
	/// <param name="filePath">File path.</param>
	/// <typeparam name="T">The type of data to read.</typeparam>
	public static T ReadFile<T> (string filePath) 
		where T: class
	{
		filePath = FixFilePath(filePath);
		if (!File.Exists (filePath)) {
			Debug.LogError ("Filepath ' " + filePath + " ' does not exist");
			return default(T);
		}
		if (filePath.Contains (".xml"))
			return ReadFromXml<T> (filePath);
		else 
			return ReadCustomFile<T> (filePath);
	}

	/// <summary>
	/// Reads data from custom file format.
	/// </summary>
	/// <returns>The loaded file if read was successful, <c>null</c> or <c>0</c> otherwise.</returns>
	/// <param name="filePath">File path.</param>
	/// <typeparam name="T">The type of data to read.</typeparam>
	public static T ReadCustomFile<T> (string filePath) 
		where T: class
	{
		filePath = FixFilePath (filePath);
		if (!File.Exists (filePath)) {
			Debug.LogError ("Filepath ' " + filePath + " ' does not exist");
			return default(T);
		}
		try {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream fs = new FileStream(filePath, FileMode.Open);
			T obj = bf.Deserialize(fs) as T;
			fs.Close();
			return obj;
		} catch (Exception e) {
			Debug.LogError (e.Message);
			return default(T);
		}
	}

	/// <summary>
	/// Reads from an xml file.
	/// </summary>
	/// <returns>The loaded file if read was successful, <c>null</c> or <c>0</c> otherwise.</returns>
	/// <param name="filePath">File path to load data from.</param>
	/// <typeparam name="T">The type of data to read.</typeparam>
	public static T ReadFromXml<T> (string filePath) 
		where T: class
	{
		filePath = FixFilePath (filePath);
		if (!File.Exists (filePath)) {
			Debug.LogError ("Filepath ' " + filePath + " ' does not exist");
			return default(T);
		}
		try {
			XmlSerializer xs = new XmlSerializer (typeof(T));
			FileStream fs = new FileStream (filePath, FileMode.Open);
			T obj = xs.Deserialize (fs) as T;
			fs.Close ();
			return obj;
		} catch (Exception e) {
			Debug.LogError (e.Message);
			//obj = temp;
			return default(T);
		}
	}

	/// <summary>
	/// Reads from a txt file.
	/// </summary>
	/// <returns>String list of lines if read was successful, <c>null</c> otherwise.</returns>
	/// <param name="filePath">File path to load data from.</param>
	public static List<string> ReadFromTxt (string filePath) {
		filePath = FixFilePath (filePath);
		//List<string> temp = data;
		if (!File.Exists (filePath)) {
			Debug.LogError ("Filepath ' " + filePath + " ' does not exist");
			return null;
		}
		try {
			StreamReader sr = new StreamReader(filePath);
			string rawContent = sr.ReadToEnd();
			sr.Dispose();

			char[] splitChars = { '\n', '\r' };
			string[] lines = rawContent.Split(splitChars);
			List<string> data = lines.ToList();
			return data;
		} catch (Exception e) {
			Debug.LogError (e.Message);
			//data = temp;
			return null;
		}
	}

	/// <summary>
	/// Saves data to file.
	/// Creates file if file doesn't exist.
	/// Overwrites file if file already exists.
	/// </summary>
	/// <returns><c>true</c>, if file was saved successfully, <c>false</c> otherwise.</returns>
	/// <param name="filePath">File path.</param>
	/// <param name="obj">Object to save.</param>
	/// <typeparam name="T">The type of object to save.</typeparam>
	public static bool SaveFile<T> (string filePath, T obj) {
		filePath = FixFilePath (filePath);
		bool isValid = ValidateDirectory (filePath);
		if (!isValid)
			return false;
		if (filePath.Contains (".xml"))
			return SaveToXml<T> (filePath, obj);
		else
			return SaveToCustomFile<T> (filePath, obj);
	}

	/// <summary>
	/// Saves data to custom file format.
	/// Creates file if file doesn't exist.
	/// Overwrites file if file already exists.
	/// </summary>
	/// <returns><c>true</c>, if file was saved successfully, <c>false</c> otherwise.</returns>
	/// <param name="filePath">File path to save data to.</param>
	/// <param name="obj">Object to save.</param>
	/// <typeparam name="T">The type of object to save.</typeparam>
	public static bool SaveToCustomFile<T> (string filePath, T obj) {
		filePath = FixFilePath (filePath);
		bool isValid = ValidateDirectory (filePath);
		if (!isValid)
			return false;
		try {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream fs = new FileStream(filePath, FileMode.Create);
			bf.Serialize(fs, obj);
			fs.Close();
			return true;
		} catch (Exception e) {
			Debug.LogError (e.Message);
			return false;
		}
	}

	/// <summary>
	/// Saves data to an xml file.
	/// Creates file if file doesn't exist.
	/// Overwrites file if file already exists.
	/// </summary>
	/// <returns><c>true</c>, if file was saved successfully, <c>false</c> otherwise.</returns>
	/// <param name="filePath">File path to save data to.</param>
	/// <param name="obj">Object to save into file.</param>
	/// <typeparam name="T">The type parameter of object.</typeparam>
	public static bool SaveToXml<T> (string filePath, T obj) {
		filePath = FixFilePath (filePath);
		bool isValid = ValidateDirectory (filePath);
		if (!isValid)
			return false;
		try {
			XmlSerializer xs = new XmlSerializer(typeof(T));
			FileStream fs = new FileStream(filePath, FileMode.Create);
			xs.Serialize(fs, obj);
			fs.Close ();
			return true;
		} catch (Exception e) {
			Debug.LogError (e.Message);
			return false;
		}
	}

	/// <summary>
	/// Saves string list provided into a txt file.
	/// Creates file if file doesn't exist.
	/// Overwrites file if file already exists.
	/// </summary>
	/// <returns><c>true</c>, if file was saved successfully, <c>false</c> otherwise.</returns>
	/// <param name="filePath">File path to save data to.</param>
	/// <param name="data">String list to save into file.</param>
	public static bool SaveToTxt (string filePath, List<string> data) {
		filePath = FixFilePath (filePath, ".txt");
		try {
			StreamWriter sw = new StreamWriter(filePath);
			for (int i = 0; i < data.Count; i++) {
				sw.WriteLine(data[i]);
			}
			sw.Close();
			return true;
		} catch (Exception e) {
			Debug.LogError (e.Message);
			return false;
		}
	}

	/// <summary>
	/// Fixes the file path to appropriate format.
	/// </summary>
	/// <returns>The fixed file path.</returns>
	/// <param name="filePath">File path.</param>
	/// <param name="ext">File extension to add to end of filepath if filepath does not already contain an extension. Uses default (.xml) if not specified</param>
	static string FixFilePath(string filePath, string ext = fileExt) {
		string savePath = Application.dataPath + "/_data/";
		string savePath2 = Application.persistentDataPath + "/_data/";
		filePath = filePath.Replace ("[]", savePath);
		filePath = filePath.Replace ("[^]", savePath2);
		if (!filePath.Contains ("."))
			filePath += ext;
		return filePath;
	}

	/// <summary>
	/// Checks if a directory exists. If it doesn't, attempts to create the directory.
	/// </summary>
	/// <returns><c>true</c>, if directory or file was found or created, <c>false</c> otherwise.</returns>
	/// <param name="path">Directory or File Path.</param>
	static bool ValidateDirectory (string path) {
		string dPath = path;
		if (Directory.Exists (path) || File.Exists (path))
			return true;
		if (path.Contains (".")) {
			dPath = "";
			string[] parts = path.Split ('/');
			foreach (string part in parts) {
				if (!part.Contains(".")) dPath += part + "/";
			}
			if (Directory.Exists (dPath))
				return true;
		}
		if (dPath != "" && !dPath.Contains (".")) {
			try {
				Directory.CreateDirectory(dPath);
				return true;
			} catch (Exception e) {
				Debug.LogError (e.Message);
				return false;
			}
		}
		Debug.LogError ("Invalid Directory: " + dPath);
		return false;
	}

	/* Some helper functions that other scripts could use if needed */

	/// <summary>
	/// Checks if file exists.
	/// </summary>
	/// <returns><c>true</c> if file exists, <c>false</c> otherwise.</returns>
	/// <param name="filePath">File path.</param>
	public static bool FileExists(string filePath) {
		filePath = FixFilePath (filePath);
		return File.Exists (filePath);
	}

	/// <summary>
	/// Checks if directory exists.
	/// </summary>
	/// <returns><c>true</c>, if directory exists, <c>false</c> otherwise.</returns>
	/// <param name="path">Directory Path.</param>
	public static bool DirectoryExists(string path) {
		string savePath = Application.dataPath + "/_data/";
		string savePath2 = Application.persistentDataPath + "/_data/";
		path = path.Replace("[]", savePath);
		path = path.Replace ("[^]", savePath2);
		string dPath = path;
		if (path.Contains (".")) {
			dPath = "";
			string[] parts = path.Split ('/');
			foreach (string part in parts) {
				if (!part.Contains(".")) dPath += part + "/";
			}
		}
		return Directory.Exists (path);
	}

	/// <summary>
	/// Creates a file.
	/// </summary>
	/// <returns><c>true</c>, if file was created or exists, <c>false</c> otherwise.</returns>
	/// <param name="filePath">File path to create.</param>
	/// <param name="overwrite">If set to <c>true</c>, will overwrite file if it exists. By default is false.</param>
	public static bool CreateFile(string filePath, bool overwrite = false) {
		filePath = FixFilePath (filePath);
		if (File.Exists (filePath)) {
			if (overwrite) {
				File.Delete (filePath);
			} else {
				Debug.Log ("File Already Exists");
				return true;
			}
		}
		try {
			File.Create(filePath);
			return true;
		} catch (Exception e) {
			Debug.LogError (e.Message);
			return false;
		}
	}

	/// <summary>
	/// Creates the directory.
	/// </summary>
	/// <returns><c>true</c>, if directory was created or exists, <c>false</c> otherwise.</returns>
	/// <param name="path">Directory path to create.</param>
	public static bool CreateDirectory(string path) {
		string savePath = Application.dataPath + "/_data/";
		string savePath2 = Application.persistentDataPath + "/_data/";
		path = path.Replace("[]", savePath);
		path = path.Replace ("[^]", savePath2);
		string dPath = path;
		if (path.Contains (".")) {
			dPath = "";
			string[] parts = path.Split ('/');
			foreach (string part in parts) {
				if (!part.Contains(".")) dPath += part + "/";
			}
		}
		if (Directory.Exists (dPath)) {
			Debug.Log ("Directory already exists");
			return true;
		}
		try {
			Directory.CreateDirectory(dPath);
			return true;
		} catch (Exception e) {
			Debug.LogError (e.Message);
			return false;
		}
	}

	/// <summary>
	/// Deletes the specified file.
	/// </summary>
	/// <returns><c>true</c>, if file was deleted, <c>false</c> otherwise.</returns>
	/// <param name="filePath">File path.</param>
	public static bool DeleteFile(string filePath) {
		filePath = FixFilePath (filePath);
		if (!File.Exists (filePath)) {
			Debug.LogError ("File ' " + filePath + " ' does not exist");
			return false;
		}
		File.Delete (filePath);
		return true;
	}
}