using System;
using System.Collections.Generic;
using UnityEditor;

public class CommonProperty : Editor
{
	public static List<string> config_path = new List<string>();

	public static List<string> cs_path = new List<string>();

	public static List<string[]> config_data = new List<string[]>();

	public static string struct_name = "ConfigStruct";

	public static string struct_instance_name = "configstruct";

	public static string list_name = "Data";

	public static string method_name = "ProduceSerializable";

	public static string separator = ";";

	public static string invoke_cs_path = "/Editor/__TempEditorConfigInvoke";

	public static string base_cs_path = "/Scripts/ScriptableObjectOfConfig";
	

	public static string so_path = "/Resources/ScriptableObjectOfConfig";
}
