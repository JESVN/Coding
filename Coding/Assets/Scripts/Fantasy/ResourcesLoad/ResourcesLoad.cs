using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
#if UNITY_EDITOR
using UnityEditor;
#endif
/// <summary>
/// 资源加载方式
/// </summary>
public class ResourcesLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(ResoucesMethod());
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            AssetDatabaseMethod();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(wwwMethod());
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(UnityWebMethod());
        }
    }

    /// <summary>
    /// Resources方式加载(不带文件后缀)，资源必须放在Resources文件夹下，该文件夹资源会随包打出，会增大包体，资源小则放心使用
    /// </summary>
    /// <returns></returns>
    private IEnumerator ResoucesMethod()
    {
        var textAuto = Resources.Load<GameObject>("TextForithm/TextAuto"); //同步
        Instantiate(textAuto);
        var request = Resources.LoadAsync<GameObject>("UniRx_UniTask_Thread/bar"); //异步
        yield return request;
        Instantiate(request.asset);
    }

    /// <summary>
    /// AssetDatabase方式加载(需要文件后缀)，可加载Assets目录下的任何资源。仅Editor模式下有效
    /// </summary>
    private void AssetDatabaseMethod()
    {
#if UNITY_EDITOR
        var sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/UI/2MainMenu/Btn_Character_h.png");
        new GameObject("bg_Progress").AddComponent<SpriteRenderer>().sprite = sprite;
        var game = AssetDatabase.LoadAssetAtPath<GameObject>(
            "Assets/Effect/EpicToonFX/Prefabs/Combat/Blood/Green/GreenBloodExplosionLooping.prefab");
        Instantiate(game);
#else
        var sprite = Resources.Load<Sprite>("Png/Btn_Character_h");
        new GameObject("bg_Progress").AddComponent<SpriteRenderer>().sprite = sprite;
        var game = Resources.Load<GameObject>(
            "Effect/GreenBloodExplosionLooping");
        Instantiate(game);
#endif
    }

    /// <summary>
    /// WWW方式加载(本地需要全路径加载和文件后缀)，用于加载server或者local的ab资源或者texture,video等少数非prefab资源
    /// </summary>
    /// <returns></returns>
    private IEnumerator wwwMethod()
    {
#if UNITY_EDITOR
        var www = new WWW(Application.dataPath + "/Resources/Png/Btn_User_h.png");
#else
                var www = new WWW(Application.dataPath + "/Btn_User_h.png");//读取外部图片
#endif
        yield return www;
        if (string.IsNullOrEmpty(www.error))
        {
            var teture = www.texture;
            var sprite = Sprite.Create(teture, new Rect(0, 0, teture.width, teture.height), new Vector2(0.5f, 0.5f));
            new GameObject("Btn_User_h").AddComponent<SpriteRenderer>().sprite = sprite;
        }
        else
            Debug.Log($"{www.error}");
    }

    /// <summary>
    /// UnityWebRequest方式加载(本地需要全路径加载和文件后缀)，用于加载server或者local的ab资源或者texture,video等少数非prefab资源
    /// </summary>
    /// <returns></returns>
    private IEnumerator UnityWebMethod()
    {
        
#if UNITY_EDITOR
        var web = UnityWebRequest.Get(Application.dataPath + "/Resources/Png/Sci-fi-Circle_Large.png");
#else
              var web = UnityWebRequest.Get(Application.dataPath + "/Sci-fi-Circle_Large.png");//读取外部图片
#endif
        var _download = new DownloadHandlerTexture(true);
        web.downloadHandler = _download;
        yield return web.SendWebRequest();
        if (string.IsNullOrEmpty(web.error))
        {
            var sprite = Sprite.Create(_download.texture,
                new Rect(0, 0, _download.texture.width, _download.texture.height), new Vector2(0.5f, 0.5f));
            new GameObject("Sci-fi-Circle_Large1").AddComponent<SpriteRenderer>().sprite = sprite;
        }
        else
            Debug.Log($"{web.error}");
    }
}