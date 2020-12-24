using UnityEngine;
public class Receiver1:MonoBehaviour,IReceive
{
    public void Receive(string code)
    {
        Debug.Log($"{code},{gameObject}");
    }
}
