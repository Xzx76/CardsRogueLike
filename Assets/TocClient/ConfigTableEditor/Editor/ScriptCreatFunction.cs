using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.Reflection;

namespace TocClient
	{
	public class ScriptCreatFunction : Editor
	{
		const string PUBLIC = "public";
		public static string base_cs_path = "/Scripts/ScriptableObjectOfConfig";

		public static void CreatFunc(List<string[]> datas,string name)
        {
			Debug.Log("----------=="+name);
			string[] typ = datas[1];
			string[] member = datas[2];
			int memberCount = typ.Length;

			StringBuilder builder = new StringBuilder();
			builder.AppendLine("using System;");
			builder.AppendLine("using System.Collections.Generic;");
			builder.AppendLine("using ProtoBuf;");
			builder.AppendLine("");
			builder.AppendLine("namespace TocClient {");
			builder.AppendLine("    [ProtoContract]");
			builder.AppendLine("    public class Cfg_" + name);
			builder.AppendLine("    {");
			for (int i = 0; i < memberCount; i++)
			{
				string field = member[i].Replace("$","");
				builder.AppendLine("        [ProtoMember(" + (i+1)+")]");
				builder.AppendLine(string.Format("        public {0} {1}", typ[i], field.Substring(0, 1).ToUpper() + field.Substring(1)) + "{get;set;}");
			}
			
			builder.AppendLine("    }");
			//builder.AppendLine();
			//builder.AppendLine("    [ProtoContract]");
			//builder.AppendLine("    public class Cfg_" + name + "Reader");
			//builder.AppendLine("    {");
			//builder.AppendLine("        [ProtoMember(1)]");
			//builder.AppendLine("        public List<Cfg_" + name + "> lst = new List<Cfg_" + name + ">();");
			//builder.AppendLine("    }");
			builder.AppendLine("}");
			

			SaveToCs(builder.ToString(),name);
		}
		public static void CreatFuncN(List<string[]> datas, string name)
		{
			Debug.Log("----------==" + name);
			string[] typ = datas[1];
			string[] member = datas[2];
			int memberCount = typ.Length;

			StringBuilder builder = new StringBuilder();
			builder.AppendLine("using System;");
			builder.AppendLine("using System.Collections.Generic;");
			builder.AppendLine("");
			builder.AppendLine("namespace TocClient {");
			builder.AppendLine("    public class Cfg_" + name);
			builder.AppendLine("    {");
			for (int i = 0; i < memberCount; i++)
			{
				string field = member[i].Replace("$", "");
				builder.AppendLine(string.Format("        public {0} {1}", typ[i], field.Substring(0, 1).ToUpper() + field.Substring(1)) + "{get;set;}");
			}

			builder.AppendLine("    }");
			builder.AppendLine("}");


			SaveToCs(builder.ToString(), name);
		}

		private static string SaveToCs(string cs_content, string cs_name)
		{
			string classname = string.Concat(new string[] { "Cfg_",cs_name});

			string path = string.Concat(new string[]{Application.dataPath,base_cs_path,"/",classname,".cs"});
			Debug.Log(path);
			bool flag = File.Exists(path);
			if (flag)
			{
				File.Delete(path);
			}
			File.WriteAllText(path, cs_content, Encoding.UTF8);

			//Debug.Log(path);
			
			return classname;
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

	
}

