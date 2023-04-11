using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class GetFilePath : Editor
{
	private static List<string> _config_path;

	public static List<string> GetConfig(string path, string suffix_name)
	{
		GetFilePath._config_path = new List<string>();
		string path2 = Application.dataPath + path;
		GetFilePath.GetFolder(path2, suffix_name);
		return GetFilePath._config_path;
	}

	private static void GetFolder(string path, string suffix_name)
	{
		GetFilePath.GetFiles(path, suffix_name);
		string[] directories = Directory.GetDirectories(path);
		bool flag = directories.Length != 0;
		if (flag)
		{
			string[] array = directories;
			for (int i = 0; i < array.Length; i++)
			{
				string path2 = array[i];
				GetFilePath.GetFolder(path2, suffix_name);
			}
		}
	}

	private static void GetFiles(string dir, string suffix_name)
	{
		string[] files = Directory.GetFiles(dir, "*." + suffix_name);
		bool flag = files.Length != 0;
		if (flag)
		{
			string[] array = files;
			for (int i = 0; i < array.Length; i++)
			{
				string item = array[i];
				GetFilePath._config_path.Add(item);
			}
		}
	}
}
