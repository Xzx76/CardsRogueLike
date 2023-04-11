using System;
using System.IO;
using System.Reflection;
using UnityEditor;

public class InvokeCsToCall : Editor
{
	public static void InvokeMethod()
	{
		string suffix_name = "cs";
		CommonProperty.cs_path.Clear();
		CommonProperty.cs_path = GetFilePath.GetConfig(CommonProperty.invoke_cs_path, suffix_name);
		for (int i = 0; i < CommonProperty.cs_path.Count; i++)
		{
			string class_name = Path.GetFileName(CommonProperty.cs_path[i]).Replace(".cs", "");
			InvokeCsToCall.ReflectionInvokeCsForSerializable(class_name);
		}
	}

	private static void ReflectionInvokeCsForSerializable(string class_name)
	{
		Type type = null;
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		for (int i = 0; i < assemblies.Length; i++)
		{
			Assembly assembly = assemblies[i];
			bool flag = assembly.GetType(class_name) != null;
			if (flag)
			{
				type = assembly.GetType(class_name);
				break;
			}
		}
		MethodInfo method = type.GetMethod(CommonProperty.method_name);
		object obj = Activator.CreateInstance(type);
		method.Invoke(obj, null);
	}
}
