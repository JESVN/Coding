using System.Threading;
using System.Threading.Tasks;
using IngameDebugConsole;
using UnityEngine;
public class UnityIngameDebugConsoleTest
{
    [ConsoleMethod("TestLog","这是一条测试ToolLoom.RunAsync的Log")]
    public static void TestAsyncLog()
    {
        ToolLoom.RunAsync(() =>
        {
            Debug.Log($"{"这是一个测试常规ToolLoom.RunAsync的Log--3秒后会Log加密通话"}");
            Thread.Sleep(3000);
            Debug.Log("这是一个测试常规的Log--别比别比，别比巴伯"); 
        });
    }
    [ConsoleMethod("TestAsyncLog","这是一条测试await/async的Log")]
    public static async void TestAsyncLog2()
    {
        Debug.Log($"{"这是一个测试常规的await/async的Log--3秒后会Log加密通话"}");
        await Task.Delay(3000);
        Debug.Log("这是一个测试常规的Log--别比别比，别比巴伯"); 
    }
}
