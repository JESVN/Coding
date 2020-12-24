using UnityEngine;
public class Receiver4:IReceive
{
    public string a = "Receiver4";
    public void Receive(string code)
    {
         Debug.Log($"{code},{a}");
    }
}
