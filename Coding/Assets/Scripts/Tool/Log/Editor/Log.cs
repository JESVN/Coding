using UnityEditor;
using UnityEngine;
public class Log : Editor
{
    /// <summary>
    /// 拓展Hierarchy右键
    /// </summary>
    [MenuItem("GameObject/Log", false,0)]
    public static void ExpandLog()
    {
        if (FindObjectOfType<IssueRemoveLog>())
            return;
        GameObject log = new GameObject("[issue]RemoveLog");
        log.AddComponent<IssueRemoveLog>();
    }
    #region 重写Hierarchy右键

    // /// <summary>
    // /// 重写Hierarchy右键
    // /// </summary>
    // [MenuItem("Assets/重写Hierarchy右键", false,0
    // )]
    // public static void RewriteHierarchy()
    // {
    //     if (GameObject.Find("[issue]RemoveLog"))
    //         return;
    //     GameObject log = new GameObject("[issue]RemoveLog");
    //     log.AddComponent<IssueRemoveLog>();
    // }
    //
    // [InitializeOnLoadMethod]
    // private static void StartInitializeOnLoadMethod()
    // {
    //     EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;//hierarchy面板绘制GUI的委托，可以自己添加gui委托
    // }
    //
    // static void OnHierarchyGUI(int instanceID, Rect selectionRect)//Unity hierarchy绘制的时候传递的2个参数
    // {
    //     if (Event.current != null && Event.current.button == 1 && Event.current.type <= EventType.MouseUp) //右键，点击向上
    //     {
    //         Vector2 mousePosition = Event.current.mousePosition;
    //         EditorUtility.DisplayPopupMenu(new Rect(mousePosition.x, mousePosition.y, 0, 0), "Assets/",null);//在鼠标的位置弹出菜单，菜单的路径
    //          
    //     }
    // }

    #endregion
}