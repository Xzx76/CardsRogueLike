using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEditor;
using UnityEngine;
using ProtoBuf;
using System;
using Excel;
using LitJson;

namespace TocClient
	{
	public class ConfigEditorWindows : EditorWindow
	{
		Rect rect;
		private string _fileName;
		private string path;
		private bool isConverting = false; 
		private bool usePopup = true;
		private bool useDrag = false;
		private UnityEngine.Object source;
		private Vector2 scrollview_2 = Vector2.zero;
		private static ConfigEditorWindows ctew;
		private GUIStyle StyleCustom;
		private GUIStyle StyleCustom1;
		/// <summary>
		/// 数据
		/// </summary>
		private DataSet mResultSet;
		//支持的数据类型
		private string[] typePopup = new string[]
	    {
		"string",
		"string[]",
		"int",
		"int[]",
		"bool",
		"bool[]",
		"float",
		"float[]"
		};

		int optionsIndex = 0;
		public string[] options = new string[] { "选择配置文件" };

		[SerializeField]
		private List<string[]> configchar = new List<string[]>();

		/// <summary>
		/// support float write
		/// </summary>
		static void registerFloat()
		{
			void Exporter(float obj, JsonWriter writer)
			{
				writer.Write(obj);
			}
			JsonMapper.RegisterExporter((ExporterFunc<float>) Exporter);
			void ExporterArray(float[] obj, JsonWriter writer)
			{
				for (int i = 0; i < obj.Length; i++)
				{
					writer.Write(obj[i]);	
				}
				
			}
			JsonMapper.RegisterExporter((ExporterFunc<float[]>) ExporterArray);
		}
		[MenuItem("Tools/ConfigEditor")]
		private static void Init()
		{
			ConfigEditorWindows.ctew = (ConfigEditorWindows)EditorWindow.GetWindow(typeof(ConfigEditorWindows), false, "ConfigEditor");
			ConfigEditorWindows.ctew.autoRepaintOnSceneChange = true;
		}
		private void OnEnable()
		{
			this.StyleCustom = new GUIStyle();
			this.StyleCustom.fontStyle = FontStyle.Bold;
			this.StyleCustom.fontSize = 24;
			this.StyleCustom.normal.textColor = Color.blue;
			this.StyleCustom.alignment = TextAnchor.MiddleCenter;
			this.StyleCustom1 = new GUIStyle();
			this.StyleCustom1.fontStyle = FontStyle.Bold;
			GetConfigFiles();
		}
		private void OnGUI()
        {
			EditorGUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.Label("ConfigEditor", this.StyleCustom, new GUILayoutOption[]{GUILayout.ExpandWidth(true)});
			EditorGUILayout.EndVertical();
            EditorGUILayout.BeginHorizontal();
            {
                rect = EditorGUILayout.GetControlRect(GUILayout.Width(500));
                path = EditorGUI.TextField(rect, "Excl File", path);
                if ((Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragExited) && rect.Contains(Event.current.mousePosition))
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                    if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
                    {
                        path = DragAndDrop.paths[0];
						Debug.Log("-------"+path);
                    }
                }
                bool flag2 = GUILayout.Button("LoadExcel", new GUILayoutOption[]
                {
                    GUILayout.MaxWidth(200f)
                });
                if (flag2)
                {
                    LoadExcel(path);
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                optionsIndex = EditorGUILayout.Popup("选择配置文件(Excel)", optionsIndex, options, new GUILayoutOption[] { GUILayout.MaxWidth(500f) });
                //this.source = EditorGUILayout.ObjectField("Config File", this.source, typeof(TextAsset), true, new GUILayoutOption[] { GUILayout.MaxWidth(500f) });
                bool flag2 = GUILayout.Button("LoadExcel", new GUILayoutOption[]
                {
                    GUILayout.MaxWidth(200f)
                });
                if (flag2)
                {
                    //LoadBytes(options[optionsIndex]);
                    LoadExcel(options[optionsIndex]);

                }
            }
            EditorGUILayout.EndHorizontal();
            if (configchar.Count>0)
            {
				GUI_ShowConfigData();
			}

			GUILayout.FlexibleSpace();
			bool flag5 = GUILayout.Button("(1)Create script file", new GUILayoutOption[]
			{
				GUILayout.Height(30f)
			});
			if (flag5)
			{
				_fileName = GetFileNameFromPath(path);
                if (_fileName.Length>0)
                {
					ScriptCreatFunction.CreatFuncN(configchar, _fileName);
					AssetDatabase.Refresh();					
				}
                else
                {
					Debug.LogError("配置文件名称获取失败！");
                }			

			}
			//GUILayout.FlexibleSpace();
			bool flag6 = GUILayout.Button("(2)Save as json file", new GUILayoutOption[]
			{
				GUILayout.Height(30f)
			});
			if (flag6)
			{
				//注册float 类型，由于导出cs文件以后会导致重新编译，所以每次保存的时候重新注册write
				registerFloat();
				_fileName = GetFileNameFromPath(path);
				if (_fileName.Length > 0)
				{
					Cfg_Serializable.ClassInstance(configchar, _fileName);
					AssetDatabase.Refresh();
					GetConfigFiles();
				}
				else
				{
					Debug.LogError("配置文件名称获取失败！");
				}

			}

			if (GUILayout.Button("save all json file"))
			{
				for (int i = 0; i < options.Length; i++)
				{
					LoadExcel(options[i]);
					_fileName = GetFileNameFromPath(path);
					if (_fileName.Length > 0)
					{
						Cfg_Serializable.ClassInstance(configchar, _fileName);
						AssetDatabase.Refresh();
						GetConfigFiles();
					}
				}
			}
			GUILayout.Space(20f);
		}
		
		private void LoadExcel(string excelFile)
		{
			path = excelFile.Replace(@"\","/");
			
			configchar.Clear();
			FileStream mStream = File.Open(excelFile, FileMode.Open, FileAccess.Read);
			IExcelDataReader mExcelReader = ExcelReaderFactory.CreateOpenXmlReader(mStream);
			mResultSet = mExcelReader.AsDataSet();

			//判断Excel文件中是否存在数据表
			if (mResultSet.Tables.Count < 1)
				return;
			//默认读取第一个数据表
			DataTable mSheet = mResultSet.Tables[0];
			//判断数据表内是否存在数据
			if (mSheet.Rows.Count < 1)
				return;
			//获取代对表
			Dictionary<string, string> daidui;
			if (mResultSet.Tables.Count < 3)
				daidui = new Dictionary<string, string>();
			else
				daidui = Daidui(mResultSet.Tables[2]);
			//获取需要代对的列index
			List<int> needRep = DaiduiCol(mSheet);
			//获取需要或略的列
			List<int> rejectCol = RejectCol(mSheet);
			List<int> rejectRows = RejectRows(mSheet);

			//读取数据表行数和列数
			int rowCount = mSheet.Rows.Count;
			int colCount = mSheet.Columns.Count;

            for (int i = 0; i < rowCount; i++)
			{
				int rejectNum = 0;
				string[] row = new string[colCount- rejectCol.Count];
                for (int j = 0; j < colCount; j++)
                {
					if (rejectCol.Contains(j))
                    {
						rejectNum++;
						continue;
					}
					string str = mSheet.Rows[i][j].ToString();
                    if (needRep.Contains(j) &&i>2)
                    {
                        if (daidui.TryGetValue(str.Trim(),out string val))
                        {
							str = val;
                        }
					}
					row[j- rejectNum] = str;					
				}

				configchar.Add(row);
            }
			Debug.Log(GetFileNameFromPath(path)+ ":"+configchar.Count);
			
		}
		private string[] FieldSplit(string val)
		{
			string[] result = new string[2];
			if (val == null || val.Length < 4)
			{
				result[0] = "";
				result[1] = "string";
			}
			else
			{
				int index1 = val.IndexOf("[");
				int index2 = val.IndexOf("][");
				result[0] = val.Substring(0, index1);
				result[1] = val.Substring(index2 + 2, val.Length - 1);
			}

			return result;
		}

		//需要代对的列
		private List<int> DaiduiCol(DataTable mSheet)
        {
			List<int> lst = new List<int>();
            for (int i = mSheet.Columns.Count-1; i >=0 ; i--)
            {
                if (mSheet.Rows[2][i].ToString().IndexOf('$')>=0)
                {
					lst.Add(i);
				}
            }
			return lst;
        }
		private List<int> RejectCol(DataTable mSheet)
		{
			List<int> lst = new List<int>();
			for (int i = mSheet.Columns.Count - 1; i >= 0; i--)
			{
				if (mSheet.Rows[2][i].ToString().Trim().Length <= 0)
				{
					lst.Add(i);

					Debug.LogFormat("或略第{0}列数据",i);
				}
			}
			return lst;
		}
		private List<int> RejectRows(DataTable mSheet)
		{
			List<int> lst = new List<int>();
			for (int i = mSheet.Rows.Count - 1; i > 2; i--)
			{
				if (mSheet.Rows[i][1].ToString().Trim().Length <= 0)
				{
					lst.Add(i);

					Debug.LogFormat("或略第{0}行数据", i);
				}
			}
			return lst;
		}
		private Dictionary<string, string> Daidui(DataTable mSheet)
		{
			Dictionary<string, string> kval = new Dictionary<string, string>();
			if (mSheet.Rows.Count < 1)
				return kval;
			//读取数据表行数和列数
			int rowCount = mSheet.Rows.Count;
			int colCount = mSheet.Columns.Count;
			for (int i = 0; i < rowCount; i++)
			{
				for (int j = 0; j < colCount; j++)
				{
					string val = mSheet.Rows[i][j].ToString();
					if (val.Length > 0)
					{
						val = val.Replace("：", ":");
						string[] arr = val.Split(':');
						if (arr.Length >= 2)
						{
							kval[arr[0]] = arr[1];
						}
					}
				}
			}
			return kval;
		}
		private string GetFileNameFromPath(string path)
        {
			string[] arr= path.Split('/');
            if (arr!=null)
            {
				string file = arr[arr.Length - 1];
				string[] arr2=file.Split('.');
                if (arr2!=null)
                {
					string name = arr2[0];					
					return name.Substring(0, 1).ToUpper() + name.Substring(1); 
                }
			}
			return "";

		}
		private void GetConfigFiles()
        {
			string fullPath = "Assets/";  //路径

			//获取指定路径下面的所有资源文件  
			if (Directory.Exists(fullPath))
			{
				DirectoryInfo direction = new DirectoryInfo(fullPath);
				FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);

				//Debug.Log(files.Length);
				List<string> fileNames = new List<string>();

				for (int i = 0; i < files.Length; i++)
				{
					//if (files[i].Name.EndsWith(".meta"))
					//{
					//	continue;
					//}
                    if (files[i].Name.EndsWith(".xlsx"))
                    {
						//Debug.Log("Name:" + files[i].Name);  //打印出来这个文件架下的所有文件
						string fileName = files[i].FullName;
						fileName=fileName.Substring(fileName.IndexOf("Assets"));
						fileNames.Add(fileName);                             //Debug.Log( "FullName:" + files[i].FullName );  
															 //Debug.Log( "DirectoryName:" + files[i].DirectoryName );  
					}

				}
				options = fileNames.ToArray();
			}
		}
		private void GUI_ShowConfigData()
		{
			this.scrollview_2 = EditorGUILayout.BeginScrollView(this.scrollview_2, new GUILayoutOption[]
			{
			GUILayout.ExpandHeight(false)
			});
			EditorGUILayout.BeginVertical("Box", new GUILayoutOption[0]);
			EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(84f);
			for (int i = 0; i < this.configchar[0].Length; i++)
			{
				bool flag = i == 0;
				if (flag)
				{
					GUILayout.Label("", new GUILayoutOption[]
					{
					GUILayout.MinWidth(100f)
					});
				}
				else
				{
					bool flag2 = GUILayout.Button("Remove", new GUILayoutOption[]
					{
					GUILayout.MinWidth(100f)
					});
					if (flag2)
					{
						for (int j = 0; j < this.configchar.Count; j++)
						{
							this.configchar[j] = ConfigTableEditorFunction.DeleteInArray(this.configchar[j], i);
						}
					}
				}
			}
			EditorGUILayout.EndHorizontal();
			for (int k = 0; k < this.configchar.Count; k++)
			{
				EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
				bool flag3 = k == 0;
				if (flag3)
				{
					GUILayout.Box(new GUIContent("Remarks", "只是对当前字段的说明，没有具体作用。\r\n\r\nThere is no specific effect on the description of the current field."), new GUILayoutOption[]
					{
					GUILayout.Width(80f)
					});
				}
				else
				{
					bool flag4 = k == 1;
					if (flag4)
					{
						GUILayout.Box(new GUIContent("FieldType", "字段类型，只支持当前8种类型，并且不能为空。\r\n\r\nThe field type supports only the current 8 types, and can not be empty."), new GUILayoutOption[]{GUILayout.Width(80f)});
					}
					else
					{
						bool flag5 = k == 2;
						if (flag5)
						{
							GUILayout.Box(new GUIContent("FieldName", "字段名字，对应在脚本中使用的字段名，不能为空或全数字，并且不能与其他字段名字重复。\r\n\r\nField names, corresponding to the field names used in the script, cannot be null or full numbers, and cannot be duplicated with other field names."), new GUILayoutOption[]
							{
							GUILayout.Width(80f)
							});
						}
						else
						{
							bool flag6 = GUILayout.Button("Remove", new GUILayoutOption[]
							{
							GUILayout.Width(80f)
							});
							if (flag6)
							{
								this.configchar.RemoveAt(k);
							}
						}
					}
				}
				bool flag7 = k + 1 <= this.configchar.Count;
				if (flag7)
				{
					bool flag8 = k != 1;
					if (flag8)
					{
						for (int l = 0; l < this.configchar[k].Length; l++)
						{
							GUILayout.BeginVertical(new GUILayoutOption[0]);
							GUILayout.Space(4f);
							this.configchar[k][l] = EditorGUILayout.TextField(this.configchar[k][l], new GUILayoutOption[]{GUILayout.MinWidth(100f)});
							GUILayout.EndVertical();
						}
					}
					else
					{
						for (int m = 0; m < this.configchar[k].Length; m++)
						{
							for (int n = 0; n < this.typePopup.Length; n++)
							{
								bool flag9 = this.configchar[k][m] == this.typePopup[n];
								if (flag9)
								{
									GUILayout.BeginVertical(new GUILayoutOption[0]);
									GUILayout.Space(5f);
									int num = EditorGUILayout.Popup(n, this.typePopup, new GUILayoutOption[]{GUILayout.MinWidth(100f)});
									GUILayout.EndVertical();
									this.configchar[k][m] = this.typePopup[num];
								}
							}
						}
					}
				}
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndScrollView();
			GUILayout.Space(20f);
			EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
			bool flag10 = GUILayout.Button("Add New Row", new GUILayoutOption[]
			{
			GUILayout.Height(30f)
			});
			if (flag10)
			{
				string[] item = new string[this.configchar[0].Length];
				this.configchar.Add(item);
			}
			GUILayout.Space(30f);
			bool flag11 = GUILayout.Button("Add New Column", new GUILayoutOption[]
			{
			GUILayout.Height(30f)
			});
			if (flag11)
			{
				for (int num2 = 0; num2 < this.configchar.Count; num2++)
				{
					bool flag12 = num2 != 1;
					if (flag12)
					{
						this.configchar[num2] = ConfigTableEditorFunction.AddInArray(this.configchar[num2], "");
					}
					else
					{
						this.configchar[num2] = ConfigTableEditorFunction.AddInArray(this.configchar[num2], "string");
					}
				}
			}
			EditorGUILayout.EndHorizontal();
		}
	}
}

