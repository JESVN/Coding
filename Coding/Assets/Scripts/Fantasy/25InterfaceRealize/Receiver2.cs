using UnityEngine;
public class Receiver2 : MonoBehaviour,IReceive
{
    public void Receive(string code)
    {
         Debug.Log($"{code},{gameObject}");
    }
}
