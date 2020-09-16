using System.Collections;
using UnityEngine;
public class Manager : MonoBehaviour
{
    private enum EUpdateType
    {
        StandardUpdate,
        CoroutinesUpdate,
        OptimizeUpdate,
        UniRxUpdate,
    }
    private EUpdateType _eUpdateType;
    private int _count=3000;
    private GameObject _game;
    private bool _isCreat;
    private GUIStyle _style = new GUIStyle();
    private string _message;
    // Start is called before the first frame update
    void Start()
    {
        _style.fontSize = 30;
        _style.normal.textColor = Color.green;
        _game = Resources.Load<GameObject>("Updates/Item");
        StartCoroutine(IECreatCube());
    }
    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2-600, 0, 300, 300), _message, _style);
        if (GUI.Button(new Rect(50, 50, 150, 70), "StandardUpdate"))
        {
            _message = "标准Update执行";
            Creat(EUpdateType.StandardUpdate);
        }
        if (GUI.Button(new Rect(50, 170, 150, 70), "CoroutinesUpdate"))
        {
            _message = "协程执行";
            Creat(EUpdateType.CoroutinesUpdate);
        }
        if (GUI.Button(new Rect(50, 290, 150, 70), "OptimizeUpdate"))
        {
            _message = "优化Update执行";
            Creat(EUpdateType.OptimizeUpdate);
        }
        if (GUI.Button(new Rect(50, 410, 150, 70), "UniRxUpdate"))
        {
            _message = "UniRx执行";
            Creat(EUpdateType.UniRxUpdate);
        }
        if (GUI.Button(new Rect(50, 530, 150, 70), "CreatCube"))
        {
            _isCreat=!_isCreat;
        }
        if (GUI.Button(new Rect(50, 650, 150, 70), "Destroy"))
        {
            DestroyItem();
        }
    }

    private void DestroyItem()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
    private void Creat(EUpdateType eUpdateType)
    {
        DestroyItem();
        for (int i = 0; i < _count; i++)
        {
            GameObject game = Instantiate(_game, transform);
            game.transform.position=Random.insideUnitSphere*13f;
            switch (eUpdateType)
            {
                case EUpdateType.StandardUpdate:
                    game.AddComponent<ItemZoomStandard_Update>();
                    break;
                case EUpdateType.CoroutinesUpdate:
                    game.AddComponent<ItemZoomStandard_Update>()._IsCoroutines=true;
                    break;
                case EUpdateType.OptimizeUpdate:
                    game.AddComponent<ItemZoomOptimizeUpdates_Update>();
                    break;
                case EUpdateType.UniRxUpdate:
                    game.AddComponent<ItemZoomUniRx_Update>();
                    break;
            }
        }
    }
    /// <summary>
    /// 创建方块
    /// </summary>
    /// <returns></returns>
    private IEnumerator IECreatCube()
    {
        while (true)
        {
            if (_isCreat)
            {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obj.AddComponent<Rigidbody>();
                obj.GetComponent<Renderer>().material.color = Color.green;
                obj.name = "Cube";
                obj.transform.position = new Vector3(0, 20, 0);
                yield return new WaitForSeconds(0.1f);
            }
            yield return null;
        }
    }
}
