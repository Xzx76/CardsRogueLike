using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace TocClient
{
	public class Cfg_Serializable
	{
		public static void ClassInstance(List<string[]> datas, string class_name)
		{
			//string classname = string.Concat(new string[] { "TocClient.", class_name });
			string classname = string.Concat(new string[] { "TocClient.Cfg_", class_name });
			int len = datas.Count;
			string[] types = datas[1];
			List<object> lst = new List<object>();
			for (int i = 3; i < len; i++)
            {
				string[] data = datas[i];

				Type type = Type.GetType(classname, true, true);
				var temp = Activator.CreateInstance(type);
				PropertyInfo[] infos = type.GetProperties();
                for (int j = 0; j < infos.Length; j++)
                {
					var val= ConvertValue(types[j], data[j]);
					infos[j].SetValue(temp, val);
				}				
				lst.Add(temp);
			}
			Debug.Log(lst.Count);
   //         foreach (var item in lst)
			//{
			//	Debug.Log(item.GetType().Name);

			//}
			ClassWriteJsonFile(lst,"Config_" + class_name);
		}

		public static string base_bin_path = "/AddressableAssets/Local/Config";
		public static void ClassWriteJsonFile<T>(T value, string filename)
		{
			//ProtolBuffer protolBuffer = new ProtolBuffer();
			//string m_filepath = subDir;
			string m_filepath = string.Concat(new string[] { Application.dataPath, base_bin_path });

			if (!Directory.Exists(m_filepath))
			{
				Directory.CreateDirectory(m_filepath);
			}
			m_filepath = string.Concat(new string[] { m_filepath, "/", filename, ".json" });
			string jsonStr = JsonMapper.ToJson(value);
			File.WriteAllText(m_filepath, jsonStr);
			

		}
		public static int ClassWriteFile<T>(T value, string filename)
		{
			//ProtolBuffer protolBuffer = new ProtolBuffer();
			//string m_filepath = subDir;
			string m_filepath = string.Concat(new string[] { Application.dataPath, base_bin_path});

			if (!Directory.Exists(m_filepath))
			{
				Directory.CreateDirectory(m_filepath);
			}
			m_filepath = string.Concat(new string[] { m_filepath, "/", filename, ".bytes" });
			byte[] bytes = ProtolBuffer.Serialize(value);
			int len = bytes.Length;
			FileStream fs = File.Create(m_filepath);
			fs.Write(bytes, 0, len);
			fs.Close();
			Debug.Log(m_filepath);
			return len;

		}

		public static object ConvertValue(string key,string val)
        {
			string typeKey = key.ToLower();
            object reVal = typeKey switch
            {
                "int" => val.ToInt32(),
				"float" => val.ToFloat(),
				"bool" => val.TryToBool(),
				"string[]" => val.Split(','),
				"int[]" => val.ToIntArrry(','),
				"float[]" => GetFloats(val.Split(',')),
				"bool[]" => GetBools(val.Split(',')),
				_ => val,
            };
            return reVal;
        }

		public static float[] GetFloats(string[] vals)
		{
			float[] vs = null;
			if (vals != null)
			{
				vs = new float[vals.Length];
				for (int i = 0; i < vals.Length; i++)
				{
					float.TryParse(vals[i],out vs[i]);
				}
			}
			return vs;
		}
		public static bool[] GetBools(string[] vals) 
		{
			bool[] vs = null;
			if (vals != null)
			{
				vs = new bool[vals.Length];
				for (int i = 0; i < vals.Length; i++)
				{
                    if (vals[i].Trim()=="true"|| vals[i].Trim()=="1" || vals[i].Trim() == "T")
						vs[i] = true;
					else
						vs[i] = false;
					//bool.TryParse(vals[i], out vs[i]);
				}
			}
			return vs;
		}
		
	}
}

