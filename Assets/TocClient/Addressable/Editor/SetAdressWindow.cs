using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

public class SetAdressWindow : EditorWindow
{
    private static AddressableAssetSettings setting;
    private static List<PathInfo> path = new List<PathInfo>();
    /// <summary>
    /// 目标平台
    /// </summary>
    private BuildTarget target;

    private BuildEnvironment environment;
    /// <summary>
    /// 版本信息
    /// </summary>
    private string version = string.Empty;
    /// <summary>
    /// 远程资源地址
    /// </summary>
    public string url;
    /// <summary>
    /// 标记是否有打包报错信息
    /// </summary>
    private bool isBuildSuccess = true;

    private bool isChooseTarget = false;

    [MenuItem("Tools/BuildTools/SetAdressWindow")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(SetAdressWindow));
        setting = AssetDatabase.LoadAssetAtPath<AddressableAssetSettings>("Assets/AddressableAssetsData/AddressableAssetSettings.asset");
    }

    SetAdressWindow()
    {
    }
    private void OnEnable()
    {
        this.titleContent = new GUIContent("Set Address Window");
        isChooseTarget = false;
        version = Application.version;
        setting = AssetDatabase.LoadAssetAtPath<AddressableAssetSettings>("Assets/AddressableAssetsData/AddressableAssetSettings.asset");
        url = PlayerPrefs.GetString(setting.profileSettings.GetProfileName(setting.activeProfileId) + "_BuildUrl", @"http://[PrivateIpAddress]:[HostingServicePort]");
        environment = (BuildEnvironment)System.Enum.Parse(typeof(BuildEnvironment), PlayerPrefs.GetString("BuildEnvironment", BuildEnvironment.Debug.ToString()));

        //path.Add(new PathInfo());
        //path.Add(new PathInfo());

        #region 读取已保存的路径
        path.Clear();
        m_dataDir = $"{Application.dataPath.Substring(0, Application.dataPath.Length - 6)}/tempDataPath";
        m_savePath = $"{m_dataDir}/AdressEditor.dat";
        if (File.Exists(m_savePath))
        {
            var str = File.ReadAllText(m_savePath).Split(',');
            for (int i = 0; i < str.Length; i++)
            {
                if (!string.IsNullOrEmpty(str[i]))
                {
                    path.Add(new PathInfo() { path = str[i] });
                }
            }
        }
        #endregion

    }
    private void OnDisable()
    {
        PlayerPrefs.SetString(environment.ToString() + "_BuildUrl", url);
        PlayerPrefs.SetString("BuildEnvironment", environment.ToString());
    }

    #region 自动标记
    public static void MarkStart(bool isAdd)
    {
        List<DirectoryInfo> rootDirs = new List<DirectoryInfo>();
        List<string> groups = new List<string>();
        groups.Add("Built In Data");
        groups.Add("Default Local Group");
        for (int i = 0; i < path.Count; i++)
        {
            DirectoryInfo dir = new DirectoryInfo(Application.dataPath + path[i].path);
            rootDirs.Add(dir);
            string _path = path[i].path.Replace("/AddressableAssets/", "");
            if (_path.Contains("/"))
            {
                string[] arr = _path.Split('/');
                if (arr.Length >= 2)
                {
                    string groupName = arr[0] + "_" + arr[1];
                    groups.Add(groupName);
                    var group = setting.FindGroup(groupName);
                    if (group == null)
                    {
                        group = setting.CreateGroup(groupName, false, false, false, new List<AddressableAssetGroupSchema> { setting.DefaultGroup.Schemas[0], setting.DefaultGroup.Schemas[1] });
                    }
                    MarkFiles(group, dir);
                }
            }
            else
            {

                DirectoryInfo[] dirs = dir.GetDirectories();
                for (int j = 0; j < dirs.Length; j++)
                {
                    string groupName = _path + "_" + dirs[j].Name;
                    groups.Add(groupName);
                    var _group = setting.FindGroup(groupName);
                    if (_group == null)
                    {
                        _group = setting.CreateGroup(groupName, false, false, false, new List<AddressableAssetGroupSchema> { setting.DefaultGroup.Schemas[0], setting.DefaultGroup.Schemas[1] });
                    }
                    MarkFiles(_group, dirs[j]);
                }
            }

        }
        if (!isAdd)
        {
            var settingGroups = setting.groups;
            for (int i = 0; i < settingGroups.Count; i++)
            {
                var name = settingGroups[i].name;
                if (!groups.Contains(name))
                {
                    setting.RemoveGroup(settingGroups[i]);
                }
            }
        }
        
    }

    public static void MarkFiles(AddressableAssetGroup group, DirectoryInfo dir)
    {
        if (dir.Name == "SpriteAtlas")
            return;
        DirectoryInfo[] dirs = dir.GetDirectories();
        for (int i = 0; i < dirs.Length; i++)
        {
            MarkFiles(group, dirs[i]);
        }
        FileInfo[] files = dir.GetFiles();
        Debug.Log("开始标记：" + group.Name);
        if (files != null && files.Length > 0)
        {
            foreach (var file in files)
            {
                if (file.Extension != ".meta")
                {
                    var assetPath = file.FullName.Replace(@"\", @"/").Replace(Application.dataPath, "Assets");

                    var guid = AssetDatabase.AssetPathToGUID(assetPath);
                    int index = file.Name.IndexOf(".");
                    string address = file.Name.Remove(index, file.Name.Length - index);
                    var entry = setting.CreateOrMoveEntry(guid, group);
                    if (entry.address != address)
                    {
                        entry.SetAddress(address);
                    }
                    assetPath = assetPath.Replace("Assets/AddressableAssets/", "");
                    index = assetPath.LastIndexOf('/');
                    assetPath = assetPath.Remove(index, assetPath.Length - index);
                    var label = assetPath.Split('/').ToList();
                    List<string> oldLabels = new List<string>();
                    foreach (var item in entry.labels)
                    {
                        if (!label.Contains(item))
                            oldLabels.Add(item);
                    }
                    for (int i = 0; i < oldLabels.Count; i++)
                    {
                        entry.SetLabel(oldLabels[i], false);
                        setting.RemoveLabel(oldLabels[i]);
                    }
                    for (int i = 1; i < label.Count; i++)
                    {
                        var _label = label[i];
                        if (!setting.GetLabels().Contains(_label))
                        {
                            setting.AddLabel(_label);
                        }
                        entry.SetLabel(_label, true);
                    }
                }
            }

        }

    }
    #endregion

    #region GUI
    private void OnGUI()
    {
        //------选择设置标记路径---------
        for (int i = 0; i < path.Count; i++)
        {
            GUILayout.BeginHorizontal();
            path[i].rect = EditorGUILayout.GetControlRect(GUILayout.Width(400));
            path[i].path = EditorGUI.TextField(path[i].rect, path[i].path);

            if ((Event.current.type == EventType.DragExited) &&
             path[i].rect.Contains(Event.current.mousePosition))
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
                {
                    path[i].path = DragAndDrop.paths[0].Remove(0, 6);
                }
            }
            if (GUILayout.Button("删除", GUILayout.Width(80)))
            {
                path.RemoveAt(i);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(5);           
        }
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("增加路径", GUILayout.Width(80)))
        {
            path.Add(new PathInfo());
        }

        if (GUILayout.Button("重新标记资源地址"))
        {
            MarkStart(false);
            EditorUtility.DisplayDialog("自动标记", "自动标记成功", "确定");
            saveData();
        }
        if (GUILayout.Button("增量标记资源地址"))
        {
            MarkStart(true);
            EditorUtility.DisplayDialog("增量标记", "增量标记成功", "确定");
            saveData();
        }
        GUILayout.EndHorizontal();
        //----
    }

    #endregion
    
    #region 保存文件路径

    
    private string m_dataDir;

    private string m_savePath;
    /// <summary>
    /// save path
    /// </summary>
    void saveData()
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < path.Count; i++)
        {
            stringBuilder.Append(path[i].path);
            if (i < path.Count - 1)
            {
                stringBuilder.Append(",");
            }
        }

        if (!Directory.Exists(m_dataDir))
        {
            Directory.CreateDirectory(m_dataDir);
        }
        File.WriteAllText(m_savePath, stringBuilder.ToString());
    }

    #endregion
}
