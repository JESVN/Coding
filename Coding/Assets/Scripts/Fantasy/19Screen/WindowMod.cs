using System;
using System.Runtime.InteropServices;
using UnityEngine;
public class WindowMod : MonoBehaviour 
{
    #region 程序分屏，Unity支持最多8个拓展屏
    // void Start()
    // {
    //     All();
    // }
    // private void All()
    // {
    //     Debug.Log(Display.displays.Length);//连接主机的屏幕个数，在编辑器上只显示一个
    //     for(int i=0;i<Display.displays.Length;i++)
    //     {
    //         Display.displays[i].Activate();//激活连接主机的所有屏幕，并且激活之后不能再失活
    //         Screen.SetResolution(Display.displays[i].renderingWidth, Display.displays[i].renderingHeight, true);
    //     }
    // }
    #endregion
    #region 调用WinAPI,效果强大(用这个脚本，可以使Unity3D窗口全屏，没有标题栏，通过更改screenPosition的值，还可以使窗口直接在第二个屏幕上启动（x=0, y=0, width=1920, height=1080），或者窗口跨越两个屏（x=0, y=0, width=3840, height=1080）。  Windows系统会记录每个软件的窗口大小和位置，记录在注册表的\HKEY_CURRENT_USER\Software\xxx\yyy 位置，xxx是Unity3D在build设置中的Company Name，yyy是在Build设置中的Product Name。所以如果有时候窗口大小有问题，可以先备份注册表，再删除xxx项。建议每个项目的Product Name不要用默认值，否则打包出来的软件都会对应到注册表里相同的项)
    // public Rect screenPosition;
    // [DllImport("user32.dll")]
    // static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);
    // [DllImport("user32.dll")]
    // static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    // [DllImport("user32.dll")]
    // static extern IntPtr GetActiveWindow();
    // const uint SWP_SHOWWINDOW = 0x0040;
    // const int GWL_STYLE = -16;
    // const int WS_BORDER = 1;
    // private int i = 0;
    // void Start()
    // {
    //     screenPosition=new Rect(Screen.width,0,0,0);//在第二个屏幕上显示,在第三个屏幕上显示则为Screen.width*2，第四个Screen.width*3，以此类推，实际上就是改变程序的坐标
    //     SetWindowLong(GetActiveWindow(), GWL_STYLE, WS_BORDER);
    //     SetWindowPos(GetActiveWindow(), -1, (int)screenPosition.x, (int)screenPosition.y, (int)screenPosition.width, (int)screenPosition.height, SWP_SHOWWINDOW);
    // }

    #endregion
    
    
    #region Unity自带的API，可以改变程序坐标，使其在指定显示屏显示

    void Start()
    {
        Display.main.Activate();
        Display.main.SetParams(Screen.width, Screen.height, Screen.width, 0);
    }

    #endregion
}