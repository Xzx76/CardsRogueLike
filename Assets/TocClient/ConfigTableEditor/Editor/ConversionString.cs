using System;
using UnityEditor;

public class ConversionString : Editor
{
	public static int StringToInt(string s)
	{
		int num;
		bool flag = int.TryParse(s, out num);
		int result;
		if (flag)
		{
			result = num;
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public static float StringToFloat(string s)
	{
		float num;
		bool flag = float.TryParse(s, out num);
		float result;
		if (flag)
		{
			result = num;
		}
		else
		{
			result = 0f;
		}
		return result;
	}

	public static bool StringToBool(string s)
	{
		bool flag2;
		bool flag = bool.TryParse(s, out flag2);
		return flag && flag2;
	}

	public static int[] ArrayStringToInt(string s, string[] split_symbol)
	{
		bool flag = s != "";
		int[] result;
		if (flag)
		{
			string[] array = s.Split(split_symbol, StringSplitOptions.RemoveEmptyEntries);
			int[] array2 = new int[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = ConversionString.StringToInt(array[i]);
			}
			result = array2;
		}
		else
		{
			result = new int[0];
		}
		return result;
	}

	public static float[] ArrayStringToFloat(string s, string[] split_symbol)
	{
		bool flag = s != "";
		float[] result;
		if (flag)
		{
			string[] array = s.Split(split_symbol, StringSplitOptions.RemoveEmptyEntries);
			float[] array2 = new float[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = ConversionString.StringToFloat(array[i]);
			}
			result = array2;
		}
		else
		{
			result = new float[0];
		}
		return result;
	}

	public static bool[] ArrayStringToBool(string s, string[] split_symbol)
	{
		bool flag = s != "";
		bool[] result;
		if (flag)
		{
			string[] array = s.Split(split_symbol, StringSplitOptions.RemoveEmptyEntries);
			bool[] array2 = new bool[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = ConversionString.StringToBool(array[i]);
			}
			result = array2;
		}
		else
		{
			result = new bool[0];
		}
		return result;
	}

	public static string[] ArrayStringToString(string s, string[] split_symbol)
	{
		bool flag = s != "";
		string[] result;
		if (flag)
		{
			result = s.Split(split_symbol, StringSplitOptions.RemoveEmptyEntries);
		}
		else
		{
			result = new string[0];
		}
		return result;
	}
}
