using UnityEngine;
using DentedPixel;
public class TweenFont : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        Player.SetActive(false);
        LeanTween.alpha(gameObject,0f,1f).setRecursive(true).setLoopPingPong();
        Invoke("Show",1f);
    }
    void Show()
    {
        Player.SetActive(true);
        Destroy(gameObject,3f);
    }
}
