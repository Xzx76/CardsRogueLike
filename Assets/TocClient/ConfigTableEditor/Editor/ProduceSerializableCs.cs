using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ProduceSerializableCs : Editor
{
	public static void StartProduce()
	{
		for (int i = 0; i < CommonProperty.config_path.Count; i++)
		{
			ProduceSerializableCs.ProduceBaseCs(CommonProperty.config_path[i]);
		}
	}

	private static void ProduceBaseCs(string filepath)
	{
		CommonProperty.config_data.Clear();
		CommonProperty.config_data = AnalysisConfig.GetConfigData(filepath);
		string text = Path.GetFileName(filepath).Replace(".txt", "");
		string text2 = "using UnityEngine;\nusing System.Collections;\nusing System.Collections.Generic;\nusing System;\n";
		string text3 = "\npublic class Config_" + text + " : ScriptableObject";
		string text4 = "\n{\n\t[Serializable]\n\tpublic struct " + CommonProperty.struct_name + "\n\t{";
		string text5 = ProduceSerializableCs.ProduceStructCell();
		string text6 = string.Concat(new string[]
		{
			"\n\t}\n\tpublic List<",
			CommonProperty.struct_name,
			"> ",
			CommonProperty.list_name,
			" = new List<",
			CommonProperty.struct_name,
			">();\n}"
		});
		string cs_content = string.Concat(new string[]
		{
			text2,
			text3,
			text4,
			text5,
			text6
		});
		ProduceSerializableCs.SaveToCs(cs_content, text);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
	}

	private static string ProduceStructCell()
	{
		string text = "";
		for (int i = 0; i < CommonProperty.config_data[0].Length; i++)
		{
			string str = string.Concat(new string[]
			{
				"\n\t\tpublic ",
				CommonProperty.config_data[1][i],
				" ",
				CommonProperty.config_data[2][i],
				";"
			});
			text += str;
		}
		return text;
	}

	private static void SaveToCs(string cs_content, string cs_name)
	{
		string path = string.Concat(new string[]
		{
			Application.dataPath,
			CommonProperty.base_cs_path,
			"/Config_",
			cs_name,
			".cs"
		});
		bool flag = File.Exists(path);
		if (flag)
		{
			File.Delete(path);
		}
		File.WriteAllText(path, cs_content, Encoding.UTF8);
	}
}
