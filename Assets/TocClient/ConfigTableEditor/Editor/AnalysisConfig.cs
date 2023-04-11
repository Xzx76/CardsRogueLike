using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;

public class AnalysisConfig : Editor
{
	public static List<string[]> GetConfigData(string filepath)
	{
		List<string[]> list = new List<string[]>();
		string text = File.ReadAllText(filepath, Encoding.UTF8);
		string[] array = text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < array.Length; i++)
		{
			string[] item = array[i].Split(new char[]
			{
				'\t'
			});
			list.Add(item);
		}
		return list;
	}
}
