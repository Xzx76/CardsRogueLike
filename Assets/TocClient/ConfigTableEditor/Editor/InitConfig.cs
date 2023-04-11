using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class InitConfig : Editor
{
	public static void ProduceFiles(string path = "")
	{
		CommonProperty.config_path.Clear();
		bool flag = path == "";
		if (flag)
		{
			CommonProperty.config_path = GetFilePath.GetConfig("/", "txt");
		}
		else
		{
			path = path.Replace("Assets", Application.dataPath).Replace('\\', '/');
			CommonProperty.config_path.Add(path);
		}
		string path2 = Application.dataPath + CommonProperty.base_cs_path;
		bool flag2 = !Directory.Exists(path2);
		if (flag2)
		{
			Directory.CreateDirectory(path2);
		}
		string path3 = Application.dataPath + CommonProperty.invoke_cs_path;
		bool flag3 = !Directory.Exists(path3);
		if (flag3)
		{
			Directory.CreateDirectory(path3);
		}
		string path4 = Application.dataPath + CommonProperty.so_path;
		bool flag4 = !Directory.Exists(path4);
		if (flag4)
		{
			Directory.CreateDirectory(path4);
		}
		ProduceSerializableCs.StartProduce();
		ProduceInvokeCsForSerializable.StartProduce();
	}

	public static void ProduceScriptableObject()
	{
		InvokeCsToCall.InvokeMethod();
		AssetDatabase.SaveAssets();
		AssetDatabase.DeleteAsset("Assets" + CommonProperty.invoke_cs_path);
		AssetDatabase.Refresh();
	}
}
