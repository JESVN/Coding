using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DataTest))]
public class DataTestInspector : Editor
{
    private DataTest _dataTest;

    void OnEnable()
    {
        //获取当前编辑自定义Inspector的对象
        _dataTest = (DataTest) target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField("是否分包");
        _dataTest.IsEnCode = EditorGUILayout.Toggle("IsEnCode",_dataTest.IsEnCode,GUILayout.MinWidth(1));
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        
        EditorGUILayout.LabelField("发送内容");
        _dataTest._Type=EditorGUILayout.IntField("_Type", (byte)_dataTest._Type, GUILayout.MinWidth(1));
        _dataTest._Area=EditorGUILayout.IntField("_Area", (byte)_dataTest._Area, GUILayout.MinWidth(1));
        _dataTest._command=EditorGUILayout.IntField("_command",(byte)_dataTest._command, GUILayout.MinWidth(1));
        EditorGUILayout.LabelField("_data");
        _dataTest._data = EditorGUILayout.TextArea(_dataTest._data, GUILayout.MinHeight(100), GUILayout.MinWidth(100));

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("缓冲区大小");
        _dataTest.lenght = EditorGUILayout.Slider("lenght", _dataTest.lenght, 10, 1024);

        EditorGUILayout.EndVertical();
    }
}