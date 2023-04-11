using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Excel;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System;

public class ExcelUtility
{
	/// <summary>
	/// 表格数据集合
	/// </summary>
	private DataSet mResultSet;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="excelFile">Excel file.</param>
	public ExcelUtility (string excelFile)
	{
		FileStream mStream = File.Open (excelFile, FileMode.Open, FileAccess.Read);
		IExcelDataReader mExcelReader = ExcelReaderFactory.CreateOpenXmlReader (mStream);
		mResultSet = mExcelReader.AsDataSet ();
	}
			
	/// <summary>
	/// 转换为实体类列表
	/// </summary>
	public List<T> ConvertToList<T> ()
	{
        //判断Excel文件中是否存在数据表
        if (mResultSet.Tables.Count < 1)
            return null;
        //默认读取第一个数据表
        DataTable mSheet = mResultSet.Tables [0];

        //判断数据表内是否存在数据
        if (mSheet.Rows.Count < 1)
            return null;

        //读取数据表行数和列数
        int rowCount = mSheet.Rows.Count;
		int colCount = mSheet.Columns.Count;
		//准备一个列表以保存全部数据
		List<T> list = new List<T> ();
				
		//读取数据
		for (int i=0; i<rowCount; i++) 
		{
			//创建实例
			Type t = typeof(T);
			ConstructorInfo ct = t.GetConstructor (System.Type.EmptyTypes);
			T target = (T)ct.Invoke (null);
			for (int j=1; j<colCount; j++) 
			{
				//读取第1行数据作为表头字段
				string field = mSheet.Rows [0] [j].ToString ();
				object value = mSheet.Rows [i] [j];
				//设置属性值
				SetTargetProperty (target, field, value);
			}
					
			//添加至列表
			list.Add (target);
		}
				
		return list;
	}

	

	/// <summary>
	/// 导出为Xml
	/// </summary>
	public void ConvertToXml (string XmlFile)
	{
		//判断Excel文件中是否存在数据表
		if (mResultSet.Tables.Count < 1)
			return;

		//默认读取第一个数据表
		DataTable mSheet = mResultSet.Tables[0];

		//判断数据表内是否存在数据
		if (mSheet.Rows.Count < 1)
			return;

		//读取数据表行数和列数
		int rowCount = mSheet.Rows.Count;
		int colCount = mSheet.Columns.Count;

		//创建一个StringBuilder存储数据
		StringBuilder stringBuilder = new StringBuilder ();
		//创建Xml文件头
		stringBuilder.Append ("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
		stringBuilder.Append("\r\n");
		stringBuilder.Append ("<RECORDS>");
		stringBuilder.Append("\r\n");

		//创建根节点
		//stringBuilder.Append ("RECORDS");

		//读取数据
		for (int i = 1; i < rowCount; i++) {
			//创建子节点
			//stringBuilder.Append ("<RECORDS>");
			//stringBuilder.Append ("\r\n");
			stringBuilder.Append("<RECORD");
			for (int j = 0; j < colCount; j++) {
                if(mSheet.Rows[1][j].ToString()=="")
                {
                    continue;
                }
				stringBuilder.Append (" "+mSheet.Rows [0][j].ToString () + "=");
				stringBuilder.Append ("\'"+ mSheet.Rows [i] [j].ToString ()+"\'");
				//stringBuilder.Append ("</" + mSheet.Rows [0] [j].ToString () + ">");
				//stringBuilder.Append ("\r\n");
				//stringBuilder.Append("</RECORDS>");
			}
			//使用换行符分割每一行
			//stringBuilder.Append ("  RECORD SceneNavMesh");
			stringBuilder.Append("/>");
			stringBuilder.Append ("\r\n");
		}
		//闭合标签
		stringBuilder.Append ("</RECORDS>");
		//写入文件
		using (FileStream fileStream = new FileStream(XmlFile, FileMode.Create, FileAccess.Write)) {
			using (TextWriter textWriter = new StreamWriter(fileStream,Encoding.GetEncoding("utf-8"))) {
				textWriter.Write (stringBuilder.ToString ());
			}
		}
	}

	/// <summary>
	/// 设置目标实例的属性
	/// </summary>
	private void SetTargetProperty (object target, string propertyName, object propertyValue)
	{
		//获取类型
		Type mType = target.GetType ();
		//获取属性集合
		PropertyInfo[] mPropertys = mType.GetProperties ();
		foreach (PropertyInfo property in mPropertys) {
			if (property.Name == propertyName) {
				property.SetValue (target, Convert.ChangeType (propertyValue, property.PropertyType), null);
			}
		}
	}
}

