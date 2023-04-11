using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ProduceInvokeCsForSerializable : Editor
{
	public static void StartProduce()
	{
		for (int i = 0; i < CommonProperty.config_path.Count; i++)
		{
			ProduceInvokeCsForSerializable.ProduceCallCs(CommonProperty.config_path[i]);
		}
	}

	private static void ProduceCallCs(string filepath)
	{
		CommonProperty.config_data.Clear();
		CommonProperty.config_data = AnalysisConfig.GetConfigData(filepath);
		string text = Path.GetFileName(filepath).Replace(".txt", "");
		string text2 = "using UnityEngine;\nusing System.Collections;\nusing UnityEngine.EventSystems;\nusing System.Collections.Generic;\nusing System;";
		string text3 = "\n\npublic class InvokeConfig_" + text;
		string text4 = "\n{\n\tpublic static void " + CommonProperty.method_name + "()\n\t{";
		string text5 = string.Concat(new string[]
		{
			"\n\t\tConfig_",
			text,
			" common_class_name = (Config_",
			text,
			")UnityEditor.AssetDatabase.LoadAssetAtPath(\"Assets",
			CommonProperty.so_path,
			"/",
			text,
			".asset\", typeof(Config_",
			text,
			"));"
		});
		string text6 = "\n\t\tif (common_class_name == null)\n\t\t{";
		string text7 = "\n\t\t\tcommon_class_name = ScriptableObject.CreateInstance<Config_" + text + ">();";
		string text8 = string.Concat(new string[]
		{
			"\n\t\t\tUnityEditor.AssetDatabase.CreateAsset(common_class_name, \"Assets",
			CommonProperty.so_path,
			"/",
			text,
			".asset\");\n\t\t}"
		});
		string text9 = "\n\t\telse\n\t\t{";
		string text10 = "\n\t\t\tcommon_class_name." + CommonProperty.list_name + ".Clear();\n\t\t}";
		string text11 = "\n\t\tList<string[]> config_data = new List<string[]>();";
		string text12 = "\n\t\tconfig_data.Clear();\n\t\tconfig_data = AnalysisConfig.GetConfigData (Application.dataPath + \"/\"+ \"" + filepath.Replace(Application.dataPath + "/", "").Replace('\\', '/') + "\");";
		string text13 = "\n\t\tfor (int i = 3; i < config_data.Count; i++) {";
		string text14 = string.Concat(new string[]
		{
			"\n\t\t\tConfig_",
			text,
			".",
			CommonProperty.struct_name,
			" ",
			CommonProperty.struct_instance_name,
			" = new Config_",
			text,
			".",
			CommonProperty.struct_name,
			"();"
		});
		string text15 = ProduceInvokeCsForSerializable.ProduceForCell();
		string text16 = string.Concat(new string[]
		{
			"\n\t\t\tcommon_class_name.",
			CommonProperty.list_name,
			".Add (",
			CommonProperty.struct_instance_name,
			");\n\t\t}"
		});
		string text17 = "\n\t\tUnityEditor.EditorUtility.SetDirty(common_class_name);";
		string text18 = "\n\t\tUnityEditor.AssetDatabase.SaveAssets();";
		string text19 = "\n\t\tUnityEditor.AssetDatabase.Refresh();";
		string text20 = "\n\t}\n}";
		string cs_content = string.Concat(new string[]
		{
			text2,
			text3,
			text4,
			text5,
			text6,
			text7,
			text8,
			text9,
			text10,
			text11,
			text12,
			text13,
			text14,
			text15,
			text16,
			text17,
			text18,
			text19,
			text20
		});
		ProduceInvokeCsForSerializable.SaveToCs(cs_content, text);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
	}

	private static string ProduceForCell()
	{
		string text = "";
		for (int i = 0; i < CommonProperty.config_data[0].Length; i++)
		{
			string text2 = "";
			string text3 = "";
			bool flag = false;
			string text4 = "";
			string text5 = "";
			bool flag2 = CommonProperty.config_data[1][i] == "int";
			if (flag2)
			{
				text2 = "ConversionString.StringToInt (";
				text3 = ")";
			}
			else
			{
				bool flag3 = CommonProperty.config_data[1][i] == "float";
				if (flag3)
				{
					text2 = "ConversionString.StringToFloat (";
					text3 = ")";
				}
				else
				{
					bool flag4 = CommonProperty.config_data[1][i] == "bool";
					if (flag4)
					{
						text2 = "ConversionString.StringToBool (";
						text3 = ")";
					}
					else
					{
						bool flag5 = CommonProperty.config_data[1][i] == "int[]";
						if (flag5)
						{
							text4 = "ConversionString.ArrayStringToInt(";
							text5 = ",new string[1]{\"" + CommonProperty.separator + "\"});";
							flag = true;
						}
						else
						{
							bool flag6 = CommonProperty.config_data[1][i] == "float[]";
							if (flag6)
							{
								text4 = "ConversionString.ArrayStringToFloat(";
								text5 = ",new string[1]{\"" + CommonProperty.separator + "\"});";
								flag = true;
							}
							else
							{
								bool flag7 = CommonProperty.config_data[1][i] == "bool[]";
								if (flag7)
								{
									text4 = "ConversionString.ArrayStringToBool(";
									text5 = ",new string[1]{\"" + CommonProperty.separator + "\"});";
									flag = true;
								}
								else
								{
									bool flag8 = CommonProperty.config_data[1][i] == "string[]";
									if (flag8)
									{
										text4 = "ConversionString.ArrayStringToString(";
										text5 = ",new string[1]{\"" + CommonProperty.separator + "\"});";
										flag = true;
									}
								}
							}
						}
					}
				}
			}
			bool flag9 = !flag;
			string str;
			if (flag9)
			{
				str = string.Concat(new object[]
				{
					"\n\t\t\t",
					CommonProperty.struct_instance_name,
					".",
					CommonProperty.config_data[2][i],
					" = ",
					text2,
					"config_data [i] [",
					i,
					"]",
					text3,
					";"
				});
			}
			else
			{
				str = string.Concat(new object[]
				{
					"\n\t\t\t",
					CommonProperty.struct_instance_name,
					".",
					CommonProperty.config_data[2][i],
					" = ",
					text4,
					"config_data [i] [",
					i,
					"]",
					text5
				});
			}
			text += str;
		}
		return text;
	}

	private static void SaveToCs(string cs_content, string cs_name)
	{
		string path = string.Concat(new string[]
		{
			Application.dataPath,
			CommonProperty.invoke_cs_path,
			"/InvokeConfig_",
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
