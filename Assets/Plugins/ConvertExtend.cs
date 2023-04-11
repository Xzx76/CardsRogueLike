using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace TocClient
{
	/// <summary>
	/// 格式转换扩展
	/// </summary>
    public static class ConvertExtend
	{
		static char[] m_SeParator = new char[2] { '，', ',' };

		public static byte ToByte(this bool value)
        {
			return Convert.ToByte(value);
        }
		public static int ToInt32(this object value)
		{
			return Convert.ToInt32(value);
		}
		public static uint ToUInt32(this object value)
		{
			return Convert.ToUInt32(value);
		}
		public static bool Approximately(this float value,float target)
        {
			return target - value < 0.01f && target - value > -0.01;

		}
		/// <summary>
		/// 转为浮点数
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static float ToFloat(this string value)
		{
			if (value == null || value.Trim().Length <= 0)
				return 0f;
			float v = 0;
			if (float.TryParse(value, out v) == false)
			{
				Debug.LogFormat("value:{0} can not convert to float", value);
			}
			return v;
		}
		/// <summary>
		/// 转为短整型（short）
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Int16 ToInt16(this string value)
		{
			Int16 v = 0;
			if (Int16.TryParse(value, out v) == false)
			{
				Debug.LogError(string.Format("value = {0} can not convert to int16", value));
			}
			return v;
		}
		/// <summary>
		/// 转为整型
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Int32 ToInt32(this string value)
		{
			if (value == null || value.Trim().Length <= 0)
				return 0;
            if (int.TryParse(value, out int v) == false)
            {
                Debug.LogError(string.Format("value = {0} can not convert to int", value));
            }
            return v;
		}
		/// <summary>
		/// 转为长整型(long)
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Int64 ToInt64(this string value)
		{
			Int64 v = 0;
			if (Int64.TryParse(value, out v) == false)
			{
				Debug.LogError(string.Format("value = {0} can not convert to int64", value));
			}
			return v;
		}

		public static UInt16 ToUInt16(this string value)
		{
			UInt16 v = 0;
			if (UInt16.TryParse(value, out v) == false)
			{
				Debug.LogError(string.Format("value = {0} can not convert to uint16", value));
			}
			return v;
		}
		public static byte ToByte(this string value)
		{
			byte v = 0;
			if (byte.TryParse(value, out v) == false)
			{
				Debug.LogError(string.Format("value = {0} can not convert to byte", value));
			}
			return v;
		}
		public static UInt32 ToUInt32(this string value)
		{
			UInt32 v = 0;
			if (UInt32.TryParse(value, out v) == false)
			{
				Debug.LogError(string.Format("value = {0} can not convert to uint32", value));
			}
			return v;
		}

		public static UInt64 ToUInt64(this string value)
		{
			UInt64 v = 0;
			if (UInt64.TryParse(value, out v) == false)
			{
				Debug.LogError(string.Format("value = {0} can not convert to uint64", value));
			}
			return v;
		}

		public static bool ToBool(this string value)
		{
			string s = value.ToLower();
			return s.Equals("true");
		}
		public static bool TryToBool(this string vals)
		{
			string value = vals.Trim().ToLower();
			return value.Equals("true") || value.Equals("1") || value.Equals("t");
		}
		public static int[] ToIntArrry(this string vals,params char[] sep)
		{
			int[] vs = null;
            
			if (vals != null)
			{
				if (sep == null || sep.Length <= 0)
					sep = new char[] { ',' };

				string[] s_arr = vals.Split(sep);
				int len = s_arr.Length;
				vs = new int[len];
				for (int i = 0; i < len; i++)
				{
					vs[i] = int.Parse(s_arr[i]);
				}
			}
			return vs;
		}

		public static Color ToColor3(this int color)
		{
			float r = (float)(color >> 16) / 255f;
			float g = (float)(color >> 8 & 255) / 255f;
			float b = (float)(color & 255) / 255f;
			return new Color(r, g, b);
		}

		public static Color ToColor4(this int color)
		{
			float a = (float)(color >> 24) / 255f;
			float r = (float)(color >> 16 & 255) / 255f;
			float g = (float)(color >> 8 & 255) / 255f;
			float b = (float)(color & 255) / 255f;
			return new Color(r, g, b, a);
		}

		public static Vector2 ToVector2(this string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return Vector2.zero;
			}
			string[] array = value.Split(m_SeParator);
			return new Vector2(array[0].ToFloat(), array[1].ToFloat());
		}

		public static Vector3 ToVector3(this string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return Vector3.zero;
			}
			string[] array = value.Split(m_SeParator);
			return new Vector3(array[0].ToFloat(), array[1].ToFloat(), array[2].ToFloat());
		}

		public static Vector4 ToVector4(this string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return Vector3.zero;
			}
			string[] array = value.Split(m_SeParator);
			return new Vector4(array[0].ToFloat(), array[1].ToFloat(), array[2].ToFloat(), array[3].ToFloat());
		}

		public static List<int> ToList(this string value)
		{
			List<int> list = new List<int>();
			if (string.IsNullOrEmpty(value))
			{
				return null;
			}
			string[] array = value.Split(m_SeParator);
			for (int i = 1; i < array.Length - 1; i++)
			{
				int v = 0;
				int.TryParse(array[i], out v);
				list.Add(v);
			}
			return list;
		}
		public static Int16 GetInt16(this XmlElement element, string name)
		{
			string s = element.GetAttribute(name);
			if (string.IsNullOrEmpty(s)) return 0;
            Int16.TryParse(s, out short v);
            return v;
		}

		public static Int32 GetInt32(this XmlElement element, string name)
		{
			string s = element.GetAttribute(name);
			if (string.IsNullOrEmpty(s)) return 0;
            int.TryParse(s, out int v);
			return v;
		}

		public static Int64 GetInt64(this XmlElement element, string name)
		{
			string s = element.GetAttribute(name);
			if (string.IsNullOrEmpty(s)) return 0;
			long.TryParse(s, out long v);
			return v;
		}

		public static UInt16 GetUInt16(this XmlElement element, string name)
		{
			string s = element.GetAttribute(name);
			if (string.IsNullOrEmpty(s)) return 0;
			UInt16.TryParse(s, out UInt16 v);
			return v;
		}

		public static UInt32 GetUInt32(this XmlElement element, string name)
		{
			string s = element.GetAttribute(name);
			if (string.IsNullOrEmpty(s)) return 0;
			UInt32.TryParse(s, out UInt32 v);
			return v;
		}

		public static UInt64 GetUInt64(this XmlElement element, string name)
		{
			string s = element.GetAttribute(name);
			if (string.IsNullOrEmpty(s)) return 0;
			UInt64.TryParse(s, out UInt64 v);
			return v;
		}

		public static Vector2 GetVector2(this XmlElement element, string name)
		{
			string s = element.GetAttribute(name);
			if (string.IsNullOrEmpty(s)) return Vector3.zero;
			return ToVector2(s);
		}

		public static Vector3 GetVector3(this XmlElement element, string name)
		{
			string s = element.GetAttribute(name);
			if (string.IsNullOrEmpty(s)) return Vector3.zero;
			return ToVector3(s);
		}

		public static Vector4 GetVector4(this XmlElement element, string name)
		{
			string s = element.GetAttribute(name);
			if (string.IsNullOrEmpty(s)) return Vector4.zero;
			return ToVector4(s);
		}

		/// <summary>
		/// string转换
		/// </summary>
		/// <param name="name">(1,2,3,4,5,6)</param>
		/// <returns>List<int></returns>
		public static List<int> GetListForParentheses(this XmlElement element, string name)
		{			
			string s = element.GetAttribute(name);
			if (string.IsNullOrEmpty(s)) return null;
			return ToList(s);
		}

		public static bool GetBool(this XmlElement element, string name)
		{
			string s = element.GetAttribute(name);
			if (string.IsNullOrEmpty(s)) return false;
			return s == "1" || s.ToLower() == "true";
		}

		public static List<int> GetListForInt(this XmlElement element, string name)
		{
			List<int> list = new List<int>();
			string s = element.GetAttribute(name);
			if (string.IsNullOrEmpty(s)) return list;
			string[] array = s.Split(',');
			for (int i = 0; i < array.Length; i++)
			{
                int.TryParse(array[i], out int v);
                list.Add(v);
			}
			return list;
		}
       
        public static List<float> GetListForFloat(this XmlElement element, string name)
		{
			List<float> list = new List<float>();
			string s = element.GetAttribute(name);
			if (string.IsNullOrEmpty(s)) return list;
			string[] array = s.Split(',');
			for (int i = 0; i < array.Length; i++)
			{
                float.TryParse(array[i], out float v);
                list.Add(v);
			}
			return list;
		}

		public static float GetFloat(this XmlElement element, string name)
		{
			string s = element.GetAttribute(name);
			if (string.IsNullOrEmpty(s)) return 0;
            float.TryParse(s, out float v);
            return v;
		}

		public static string GetString(this XmlElement element, string name)
		{
			string s = element.GetAttribute(name);
			if (string.IsNullOrEmpty(s)) return string.Empty;
			return s;
		}

		public static T GetEnum<T>(this XmlElement element, string name)
		{
			string s = element.GetAttribute(name);
			if (string.IsNullOrEmpty(s)) return default(T);
			return (T)Enum.Parse(typeof(T), s);
		}
	}
}
