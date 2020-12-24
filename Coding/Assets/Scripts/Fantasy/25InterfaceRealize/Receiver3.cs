using UnityEngine;
public class Receiver3 :IReceive
{
    public string a = "Receiver3";
    public void Receive(string code)
    {
         Debug.Log($"{code},{a}");
    }
}
