using MonKey;
using UnityEngine;
public class MonKeyCommanderTest : MonoBehaviour
{
    [Command("ETEditor_RsyncEditor","Rsync同步",Category = "ETEditor")]
    private static void ShowWindow()
    {
        Debug.Log($"{"MonKeyCommanderTest"}");
    }

}
