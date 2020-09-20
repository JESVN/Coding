using UnityEngine;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
#if  UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.U2D;
using UnityEngine.UI;
/// <summary>
/// 图集打包，注意看Betches即draw call个数，用图集加载664张贴图的draw call数比用Resources加减32张贴图的draw call数还要少!!!
/// </summary>
public class AtlasLoad : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private GameObject _load; //加载遮罩
    private GameObject _game; //item
    private string _relativePath = "Assets";
    private string _aggregateProgress; //总进度
    private string _fileProgress; //当前文件进度
    private string _aggregateProgressText; //当前总进度text
    private string atFile; //当前文件进度text

    private GUIStyle _style = new GUIStyle(); //GUI样式

    // Start is called before the first frame update
    void Start()
    {
        _style.fontSize = 30;
        _style.alignment = TextAnchor.UpperCenter; //GUI.Lable标签居中
        _style.normal.textColor = Color.green;
        _game = Resources.Load<GameObject>("Atlas/Item");
    }

    // Update is called once per frame
    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width / 2 - 150 / 2 - 100, 0, 150, 100), "普通加载"))
        {
            LoadMethod("Assets/Resources/1LoadingPage/", "*.jpg", "*.png");
        }

        ;
        if (GUI.Button(new Rect(Screen.width / 2 - 150 / 2 + 100, 0, 150, 100), "图集加载"))
        {
            AtlasLoadMethod("Assets/Resources/Atlas/PngAtlas.spriteatlas");
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 150 / 2 + 300, 0, 150, 100), "重新加载场景"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        GUI.Label(new Rect(Screen.width / 2 - 800, 0, 0, 0), _aggregateProgressText + _aggregateProgress, _style);
        GUI.Label(new Rect(Screen.width / 2 - 450, 0, 0, 0), atFile + _fileProgress, _style);
    }

    void DesRes()
    {
        for (int i = 0; i < _parent.childCount; i++)
        {
            Destroy(_parent.GetChild(i).gameObject);
        }
    }

    void LoadMethod(string basePath, params string[] filters)
    {
        DesRes();
#if UNITY_EDITOR
        float progress = 0;
        float fileProgress = 0;
        int count = 0;
        _aggregateProgressText = "当前总进度:";
        atFile = "当前文件进度:";
        _load.SetActive(true);
        ToolLoom.RunAsync(() =>
        {
            var dIf = new DirectoryInfo(basePath);
            var info = dIf.GetDirectories("*", SearchOption.AllDirectories);
            foreach (var VARIABLE in info)
            {
                string[] filePaths = filters.SelectMany(f =>
                    Directory.GetFiles(_relativePath + Regex.Split(VARIABLE.FullName, _relativePath)[1], f)).ToArray();
                fileProgress = 0;
                foreach (var VARIABLEPath in filePaths)
                {
                    ToolLoom.QueueOnMainThread(() =>
                    {
                        Instantiate(_game, _parent).GetComponent<Image>().sprite =
                            ResLoad.AssetDatabaseMethod<Sprite>(VARIABLEPath);
                    });
                    fileProgress++;
                    count++;
                    _fileProgress = (fileProgress / filePaths.Length * 1f).ToString("f2");
                }

                progress++;
                _aggregateProgress = (progress / info.Length * 1f).ToString("f2");
            }

            ToolLoom.QueueOnMainThread(() =>
            {
                _load.SetActive(false);
                _aggregateProgressText = "普通图片加载完成";
                atFile = null;
                _fileProgress = null;
                Debug.Log($"{count}");
            });
        });
#else
        Sprite[] sprites = Resources.LoadAll<Sprite>("1LoadingPage/");
        foreach (var VARIABLE in sprites)
        {
            Instantiate(_game, _parent).GetComponent<Image>().sprite = VARIABLE;
        }
                  _load.SetActive(false);
                _aggregateProgressText = "普通图片加载完成";
                atFile = null;
                _fileProgress = null;
#endif
    }
    void AtlasLoadMethod(string path)
    {
        DesRes();
        float progress = 0;
        int count = 0;
        _aggregateProgressText = "当前总进度:";
        atFile = null;
        _load.SetActive(true);
        ToolLoom.RunAsync(() =>
        {
            ToolLoom.QueueOnMainThread(() =>
            {
#if UNITY_EDITOR
                var srpiteAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(path);
                #else
                 var srpiteAtlas = Resources.Load<SpriteAtlas>("Atlas/PngAtlas");
#endif
                var sprites = new Sprite[srpiteAtlas.spriteCount];
                srpiteAtlas.GetSprites(sprites);
                foreach (var sprite in sprites)
                {
                    Instantiate(_game, _parent).GetComponent<Image>().sprite = sprite;
                    progress++;
                    count++;
                    _aggregateProgress = (progress / sprites.Length * 1f).ToString("f2");
                }
            });
            ToolLoom.QueueOnMainThread(() =>
            {
                _load.SetActive(false);
                _aggregateProgressText = "图集加载完成";
                Debug.Log($"{count}");
            });
        });
    }
}