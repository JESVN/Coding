public class Receiver5:Receiver6,IReceive
{
    public string a = "Receiver5";
    public void Receive(string code)
    {
        UnityEngine.Debug.Log($"{code},{a}");
    }
}
