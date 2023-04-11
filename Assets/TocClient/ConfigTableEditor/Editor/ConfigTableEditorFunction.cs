using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ConfigTableEditorFunction : Editor
{
	public static string[] AddInArray(string[] arr, string s = "")
	{
		bool flag = arr == null;
		string[] result;
		if (flag)
		{
			result = null;
		}
		else
		{
			List<string> list = arr.ToList<string>();
			list.Add(s);
			result = list.ToArray();
		}
		return result;
	}

	public static string[] DeleteInArray(string[] arr, int index)
	{
		bool flag = arr == null || index < 0 || arr.Length - 1 < index;
		string[] result;
		if (flag)
		{
			result = null;
		}
		else
		{
			for (int i = index; i < arr.Length - 1; i++)
			{
				arr[i] = arr[i + 1];
			}
			arr = arr.Take(arr.Length - 1).ToArray<string>();
			result = arr;
		}
		return result;
	}

	public static int GetIndex(string path, string[] options)
	{
		int result;
		for (int i = 0; i < options.Length; i++)
		{
			bool flag = options[i] == path;
			if (flag)
			{
				result = i;
				return result;
			}
		}
		result = 0;
		return result;
	}

	public static void ConfigReader(ref List<string[]> configchar, TextAsset source)
	{
		configchar.Clear();
		string text = source.text;
		string[] array = text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < array.Length; i++)
		{
			string[] item = array[i].Split(new char[]
			{
				'\t'
			});
			configchar.Add(item);
		}
	}

	public static void SaveConfig(List<string[]> configchar, string path)
	{
		path = Application.dataPath + "/" + path.Replace('\\', '/').Replace("Assets/", "");
		bool flag = File.Exists(path);
		if (flag)
		{
			string text = "";
			for (int i = 0; i < configchar.Count; i++)
			{
				for (int j = 0; j < configchar[i].Length; j++)
				{
					bool flag2 = j < configchar[i].Length - 1;
					if (flag2)
					{
						text = text + configchar[i][j] + "\t";
					}
					else
					{
						text = text + configchar[i][j] + "\r\n";
					}
				}
			}
			File.Delete(path);
			File.WriteAllText(path, text, Encoding.UTF8);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			EditorUtility.DisplayDialog("Tip", "File saved successfully\r\n\r\n文件保存成功", "close");
		}
	}

	public static void CreateNewConfig(string path)
	{
		path = Application.dataPath + "/" + path.Replace('\\', '/');
		string a = Path.GetFileName(path).Replace(".txt", "");
		bool flag = a != "";
		if (flag)
		{
			bool flag2 = File.Exists(path);
			if (flag2)
			{
				EditorUtility.DisplayDialog("Error", "File rename, failed to create\r\n\r\n文件名重名，创建失败", "close");
			}
			else
			{
				string directoryName = Path.GetDirectoryName(path);
				bool flag3 = !Directory.Exists(directoryName);
				if (flag3)
				{
					Directory.CreateDirectory(directoryName);
				}
				string contents = "id/编号\tname/名字\r\nint\tstring[]\r\nid\tname\r\n1\tExample;例子";
				File.WriteAllText(path, contents, Encoding.UTF8);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
				EditorUtility.DisplayDialog("Tip", "Create success\r\n\r\n创建成功", "close");
			}
		}
		else
		{
			EditorUtility.DisplayDialog("Error", "File name error, failed to create\r\n\r\n文件名错误，创建失败", "close");
		}
	}

	public static bool CheckConfig(List<string[]> configchar)
	{
		string[] array = new string[]
		{
			"string",
			"string[]",
			"int",
			"int[]",
			"float",
			"float[]",
			"bool",
			"bool[]"
		};
		bool flag = configchar == null;
		bool result;
		if (flag)
		{
			result = false;
		}
		else
		{
			bool flag2 = configchar.Count < 3;
			if (flag2)
			{
				result = false;
			}
			else
			{
				bool flag3 = false;
				for (int i = 0; i < configchar[1].Length; i++)
				{
					for (int j = 0; j < array.Length; j++)
					{
						bool flag4 = configchar[1][i] == array[j];
						if (flag4)
						{
							flag3 = true;
							break;
						}
					}
					bool flag5 = !flag3;
					if (flag5)
					{
						result = false;
						return result;
					}
				}
				result = true;
			}
		}
		return result;
	}

	public static bool CheckConvertConfig(string path = "")
	{
		List<string> list = new List<string>();
		bool flag = path == "";
		if (flag)
		{
			list = GetFilePath.GetConfig("/", "txt");
		}
		else
		{
			list.Add(path);
		}
		string[] array = new string[]
		{
			" ",
			"\u3000",
			"+",
			"-",
			"——",
			"`",
			"·",
			"~",
			"!",
			"！",
			"@",
			"#",
			"$",
			"￥",
			"%",
			"^",
			"……",
			"&",
			"*",
			"(",
			")",
			"{",
			"}",
			"：",
			"；",
			":",
			";",
			"|",
			"\\",
			"'",
			"\"",
			"‘",
			"’",
			"“",
			"”",
			",",
			"<",
			"《",
			".",
			">",
			"》",
			"/",
			"?",
			"？",
			"，",
			"。"
		};
		string[] array2 = new string[]
		{
			"0",
			"1",
			"2",
			"3",
			"4",
			"5",
			"6",
			"7",
			"8",
			"9"
		};
		int i = 0;
		bool result;
		while (i < list.Count)
		{
			TextAsset source = (TextAsset)AssetDatabase.LoadAssetAtPath(list[i].Replace(Application.dataPath, "Assets").Replace('\\', '/'), typeof(TextAsset));
			List<string[]> list2 = new List<string[]>();
			ConfigTableEditorFunction.ConfigReader(ref list2, source);
			bool flag2 = list2.Count > 3;
			if (flag2)
			{
				for (int j = 0; j < list2[2].Length; j++)
				{
					bool flag3 = list2[2][j] == "";
					if (flag3)
					{
						EditorUtility.DisplayDialog("Error", "Configuring table third lines with empty strings\r\n\r\n配置表第三行有空字符串\r\n\r\n" + list[i] + "\r\n\r\nConfigErrorField: " + list2[2][j], "close");
						result = false;
						return result;
					}
					for (int k = 0; k < array2.Length; k++)
					{
						bool flag4 = list2[2][j][0].ToString() == array2[k];
						if (flag4)
						{
							EditorUtility.DisplayDialog("Error", "The configuration table third row has an irregular named field\r\n\r\n配置表第三行有非规则命名字段\r\n\r\n" + list[i] + "\r\n\r\nConfigErrorField: " + list2[2][j], "close");
							result = false;
							return result;
						}
					}
					for (int l = 0; l < array.Length; l++)
					{
						bool flag5 = list2[2][j].Contains(array[l]);
						if (flag5)
						{
							EditorUtility.DisplayDialog("Error", "The configuration table third row has an irregular named field\r\n\r\n配置表第三行有非规则命名字段\r\n\r\n" + list[i] + "\r\n\r\nConfigErrorField: " + list2[2][j], "close");
							result = false;
							return result;
						}
					}
					for (int m = j + 1; m < list2[2].Length; m++)
					{
						bool flag6 = list2[2][j] == list2[2][m];
						if (flag6)
						{
							EditorUtility.DisplayDialog("Error", "The configuration table third row has a duplicate name field\r\n\r\n配置表第三行有重名字段\r\n\r\n" + list[i] + "\r\n\r\nConfigErrorField: " + list2[2][j], "close");
							result = false;
							return result;
						}
					}
				}
				i++;
				continue;
			}
			EditorUtility.DisplayDialog("Error", "Configuration table error\r\n\r\n配置表格式错误\r\n" + list[i], "close");
			result = false;
			return result;
		}
		result = true;
		return result;
	}
}
