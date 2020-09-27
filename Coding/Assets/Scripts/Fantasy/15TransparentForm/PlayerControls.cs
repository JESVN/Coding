using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Collections.Generic;
using System.Collections;
public class PlayerControls : MonoBehaviour
{
    public static PlayerControls Instance;

    private float _maxY;
    private float _minY;
    
    private float _maxX;
    private float _minX;
    
    [SerializeField] private GameObject[] _effect;
    [SerializeField] private GameObject[] _attack;
    [SerializeField]private Transform target;
    
    private Rigidbody _rigidbody;
    private  GameObject s_aroundCols;
    
    private float _speed = 1f;
    private float _attackSpeed = 2000f;
    
    private bool _isMove;
    private bool _iSpace;
    
    private Vector3 dir;
    private Vector3 self;

    private List<GameObject> _memPool = new List<GameObject>();
    private List<GameObject> _attackMemPool = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        Instance=this;
        self = transform.position;
        CreateAroundColliders();
        _rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(EffLoad());
        this.UpdateAsObservable().Subscribe(_ =>
        {
            //移动
            if (_isMove)
            {
                _rigidbody.freezeRotation = true;
                _rigidbody.freezeRotation = false;
                float step = _speed * Time.deltaTime;
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, dir, step);
                if (Vector3.Distance(transform.localPosition, dir) < 0.1f)
                {
                    transform.localRotation = Quaternion.Euler(0, 180, 0);
                    _isMove = false;
                }
            }

            //爆炸
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (!_isMove)
                    _isMove = true;
                dir = MouseToWorld(transform.position.z);
                transform.LookAt(dir);
                StartCoroutine(DesGame(Range(_effect, _memPool)));
            }
            //持续特效
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _iSpace = !_iSpace;
            }
            //攻击
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                GameObject projectile = Range(_attack, _attackMemPool);
                projectile.transform
                    .LookAt(target.position); //Sets the projectiles rotation to look at the point clicked
                projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward*_attackSpeed);
            }

            if (transform.position.y > _maxY||transform.position.y<_minY||transform.position.x > _maxX||transform.position.x<_minX)
            {
                transform.position = self;
            }
        });
    }
    /// <summary>
    /// 击中处理
    /// </summary>
    public void AttackHand(Vector3 target)
    {
        //_rigidbody.AddForce(-target*1000f);
    }
    /// <summary>
    /// 前后判断(返回值为正时,目标在自己的前方,反之在自己的后方)
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private float GetFrontBack(Vector3 target)
    {
        Vector3 dir = target - transform.position; //位置差，方向     
        return Vector3.Dot(transform.forward, dir);
    }
    /// <summary>
    /// 左右判断(返回值为正时,目标在自己的右方,反之在自己的左方)
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private float GetLeftRight(Vector3 target)
    {
        return Vector3.Cross(transform.forward, target).y;
    }
    /// <summary>
    ///持续特效加载
    /// </summary>
    /// <returns></returns>
    private IEnumerator EffLoad()
    {
        while (true)
        {
            if (_iSpace)
            {
                GameObject projectile = Range(_attack, _attackMemPool);
                projectile.transform
                    .LookAt(MouseToWorld(15)); //Sets the projectiles rotation to look at the point clicked
                projectile.GetComponent<Rigidbody>().AddForce(Input.mousePosition*100f);
                yield return new WaitForSeconds(0.5f);
            }
            yield return null;
        }
    }
    /// <summary>
    /// 鼠标转世界坐标
    /// </summary>
    /// <returns></returns>
    private Vector3 MouseToWorld(float z)
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
            z));
    }

    /// <summary>
    /// 随机创建
    /// </summary>
    private GameObject Range(GameObject[] gameObjects, List<GameObject> gamesList)
    {
        int index = Random.Range(0, gameObjects.Length);
        GameObject game = InsGame(gameObjects[index], gamesList);
        game.transform.position = MouseToWorld(15);
        game.transform.rotation = Quaternion.identity;
        return game;
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="name"></param>
    private GameObject InsGame(GameObject game, List<GameObject> gamesList)
    {
        GameObject _rnGame = null;
        for (int i = 0; i < gamesList.Count; i++)
        {
            if (gamesList[i]!= null)
            {
                if (gamesList[i].name.Contains(game.name) && !gamesList[i].activeInHierarchy)
                {
                    gamesList[i].SetActive(true);
                    _rnGame = gamesList[i];
                    break;
                }
            }
            else
            {
                gamesList.Remove(gamesList[i]);
            }
        }
        if (_rnGame == null)
        {
            _rnGame = Instantiate(game);
            gamesList.Add(_rnGame);
        }

        return _rnGame;
    }

    /// <summary>
    /// 销毁(隐藏)
    /// </summary>
    /// <param name="game"></param>
    private IEnumerator DesGame(GameObject game)
    {
        yield return new WaitForSeconds(game.GetComponent<ParticleSystem>().main.duration);
        game.SetActive(false);
    }
    /// <summary>
    /// 计算边框
    /// </summary>
    public  void CreateAroundColliders()
    {
	    if(s_aroundCols != null) return;
	    s_aroundCols = new GameObject("aroundCols");
        s_aroundCols.transform.position =Vector3.zero;
        s_aroundCols.transform.localScale = Vector3.one;
        s_aroundCols.transform.localEulerAngles = Vector3.zero;
 
        Vector3 top = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height, 4));
        Vector3 bottom = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, 0f, 4));
        Vector3 left = Camera.main.ScreenToWorldPoint(new Vector3(0f, Screen.height/2f, 4));
        Vector3 right = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height/2f, 4));
        float width = Vector3.Distance(left, right);
        float height = Vector3.Distance(bottom, top);

        _maxY = top.y;
        _minY=bottom.y;
        _maxX = right.x;
        _minX=left.x;
        PhysicMaterial pmat = new PhysicMaterial();
        pmat.dynamicFriction = 0f;      //动摩擦力//
        pmat.staticFriction = 0f;       //静摩擦力//
        pmat.bounciness = 1f;       //弹力//
        pmat.frictionCombine = PhysicMaterialCombine.Minimum;       //接触物体之间的摩擦力计算//
        pmat.bounceCombine = PhysicMaterialCombine.Maximum;         //接触物体之间的弹力计算//
 
        createOnCollider("top", s_aroundCols.transform, Vector3.zero, top, new Vector3(width, 0.05f, 0.05f), pmat);
        createOnCollider("bottom", s_aroundCols.transform, Vector3.zero, bottom, new Vector3(width, 0.05f, 0.05f), pmat);
        createOnCollider("left", s_aroundCols.transform, Vector3.zero, left, new Vector3(0.05f, height, 0.05f), pmat);
        createOnCollider("right", s_aroundCols.transform, Vector3.zero, right, new Vector3(0.05f, height, 0.05f), pmat);
    }
    /// <summary>
    /// 创建物体
    /// </summary>
    /// <param name="name"></param>
    /// <param name="parent"></param>
    /// <param name="pos"></param>
    /// <param name="center"></param>
    /// <param name="size"></param>
    /// <param name="mat"></param>
    private  void createOnCollider(string name, Transform parent, Vector3 pos, Vector3 center, Vector3 size, PhysicMaterial mat)
    {
        GameObject col = new GameObject(name);
        col.transform.parent = parent;
        col.transform.position = pos;
        BoxCollider box = col.AddComponent<BoxCollider>();
        box.center = center;
        box.size = size;
        box.material = mat;
    }
}