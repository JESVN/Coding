using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
namespace Fantasy.UniRx_UniTask_Thread
{
    public class Threading : MonoBehaviour
    {
        private Text _countText;
        private bool _isActive=true;
        private int _codeing = 0;
        /// <summary>
        /// Rx Property属性数据响应器
        /// </summary>
        private ReactiveProperty<int> hp;
        // Start is called before the first frame update
        void Start()
        {
            _countText = GameObject.Find("Canvas/Text").GetComponent<Text>();
            //注：RX中订阅的通知，首次也会执行，不过不影响不用深入探究，不然纯粹浪费时间
            
            //MicroCooutine是一种高效、快速的协同工作，不用在Update函数中执行判断，或许大量的逻辑运算在这里执行更为合适
            MainThreadDispatcher.StartUpdateMicroCoroutine(IEWorker());
            
            //达到某一条件后再执行某事(只执行一次)
            GetIsActive();
            //万能数据监控，本对象的数据监控
            this.ObserveEveryValueChanged(t => t.transform.position).Subscribe((xx) =>
            {
                Debug.Log($"{xx}+++do...sth...xx");
            });
            //万能数据监控，本对象的数据监控
            this.ObserveEveryValueChanged(t =>t._codeing).Subscribe((x) =>
            {
                Debug.Log($"{x}+++do...sth...x");
            });
            //初始化属性数据响应器
            hp = new IntReactiveProperty(100);
            //订阅通知,ObserveOnMainThread表示在主线程执行,若去掉此方法而且是由子线程通知，则会报错
            hp.ObserveOnMainThread().Subscribe((x) =>
            {
                if (_countText != null)
                    _countText.text = x.ToString();
            }).AddTo(this);;
            //万能数据监控，本对象的数据监控(这种数据按键监听合适判断是否按下，不能判断长按)
            this.ObserveEveryValueChanged(t => Input.GetKeyDown(KeyCode.P)).Subscribe((xx) =>
            {
                if (xx)
                {
                    hp.Value+=10;
                }
            });
            //UniRx中的开启线程
            Observable.Start(() =>
            {
                var watch = new Stopwatch();
                watch.Start();
                while (true)
                {
                    hp.Value++;
                    Debug.Log($"{hp.Value}");
                    if (hp.Value >= 120)
                        break;
                    Thread.Sleep(100);
                }
                return watch.ElapsedTicks.ToString();
            }).ObserveOnMainThread().Subscribe(x =>//ObserveOnMainThread 切换回主线程执行
            {
                Debug.Log($"完成，耗时：{x}");
                //async方法执行 UniTask异步编程
#pragma warning disable 4014
                DemoAsync();
#pragma warning restore 4014
            }).AddTo(this);
            //鼠标双击检测
            var clickStream = Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0));
            clickStream.Buffer(clickStream.Throttle(TimeSpan.FromMilliseconds(250)))
                .Where(xs => xs.Count >= 2)
                .Subscribe(xs => Debug.Log("DoubleClick Detected! Count:" + xs.Count)).AddTo(this);

            //键盘按钮
            Observable.EveryUpdate().Subscribe(xs =>
            {
                if (Input.GetKeyDown(KeyCode.L))
                {
                    Debug.Log("单击L键");
                }
            }).AddTo(this);
        }
        /// <summary>
        /// 不停判断按键
        /// </summary>
        /// <returns></returns>
        IEnumerator IEWorker()
        {
            while(true)
            {
                if (Input.GetKeyDown(KeyCode.G))
                {
                    hp.Value+=10;
                }
                else if (Input.GetKey(KeyCode.G))
                {
                    hp.Value+=10;
                }
                yield return null;
            }
        }
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                TaskMethod();
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                UniTaskMethod();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                _isActive=!_isActive;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                hp.Value+=10;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                _codeing+=10;
            }
        }
        /// <summary>
        /// 此方法可用于达到某一条件后再执行某事(只执行一次)
        /// </summary>
        private async void GetIsActive()
        {
            await UniTask.WaitUntilValueChanged(this, x => x._isActive);
            //do...sth
            Debug.Log($"{_isActive}+++do...sth");
        }
        
        /// <summary>
        /// Task方式实现
        /// </summary>
        private async void TaskMethod()
        {
            await Task.Run(() =>
            {
                Endless("C#传统方式开启线程");
            });
        }
        /// <summary>
        /// UniTask方式实现
        /// </summary>
        private async void UniTaskMethod()
        {
            //await UniTask.SwitchToMainThread();
            await UniTask.Run(() =>
            {
                Endless("UniTask方式开启线程");
            });
        }
        /// <summary>
        /// 死循环，需要开启线程处理，否则会卡住主线程，从而造成卡顿
        /// </summary>
        /// <param name="message">调用者消息通知</param>
        private void Endless(string message)
        {
            int count = 0;
            while (true)
            {
                if (count >= 10)
                    break;
                count++;
                try
                {
                    ToMainThread(() =>
                    {
                        if (_countText != null)
                            _countText.text = count.ToString();
                    });
                }
                catch (Exception e)
                {
                    Debug.LogError($"{e.Message}");
                    break;
                }
                Debug.Log($"{count}");
                Thread.Sleep(1000);
            }
            Debug.Log($"{message}");
        }
        /// <summary>
        /// 代码片段切换回主线程执行
        /// </summary>
        /// <param name="action"></param>
        /// <exception cref="Exception"></exception>
        private async void ToMainThread(Action action)
        {
            if (action == null)
                throw new Exception("切换主线程委托不能为空");
            await UniTask.SwitchToMainThread();
            action();
            await UniTask.SwitchToThreadPool();
        }
        private async UniTask<string> DemoAsync()
        {
            Instantiate(await Resources.LoadAsync<GameObject>("UniRx_UniTask_Thread/bar").ToUniTask(Progress.Create<float>(x=>Debug.Log(x))));
            Debug.Log($"完成");
            // You can await Unity's AsyncObject
            var asset = await Resources.LoadAsync<TextAsset>("foo");
            var txt = (await UnityWebRequest.Get("https://...").SendWebRequest()).downloadHandler.text;
            await SceneManager.LoadSceneAsync("scene2");

            // .WithCancellation enables Cancel, GetCancellationTokenOnDestroy synchornizes with lifetime of GameObject
            var asset2 = await Resources.LoadAsync<TextAsset>("bar")
                .WithCancellation(this.GetCancellationTokenOnDestroy());

            // .ToUniTask accepts progress callback(and all options), Progress.Create is a lightweight alternative of IProgress<T>
            var asset3 = await Resources.LoadAsync<TextAsset>("baz")
                .ToUniTask(Progress.Create<float>(x => Debug.Log(x)));

            // await frame-based operation like coroutine
            await UniTask.DelayFrame(100);

            // replacement of yield return new WaitForSeconds/WaitForSecondsRealtime
            await UniTask.Delay(TimeSpan.FromSeconds(10), ignoreTimeScale: false);

            // yield any playerloop timing(PreUpdate, Update, LateUpdate, etc...)
            await UniTask.Yield(PlayerLoopTiming.PreLateUpdate);

            // replacement of yield return null
            await UniTask.Yield();
            await UniTask.NextFrame();

            // replacement of WaitForEndOfFrame(same as UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate))
            await UniTask.WaitForEndOfFrame();

            // replacement of yield return new WaitForFixedUpdate(same as UniTask.Yield(PlayerLoopTiming.FixedUpdate))
            await UniTask.WaitForFixedUpdate();

            // replacement of yield return WaitUntil
            await UniTask.WaitUntil(() => _isActive == false);

            // special helper of WaitUntil
            await UniTask.WaitUntilValueChanged(this, x => x._isActive);

            // You can await IEnumerator coroutine
            await FooCoroutineEnumerator();

            // You can await standard task
            await Task.Run(() => 100);

            // Multithreading, run on ThreadPool under this code
            await UniTask.SwitchToThreadPool();

            /* work on ThreadPool */

            // return to MainThread(same as `ObserveOnMainThread` in UniRx)
            await UniTask.SwitchToMainThread();

            // get async webrequest
            async UniTask<string> GetTextAsync(UnityWebRequest req)
            {
                var op = await req.SendWebRequest();
                return op.downloadHandler.text;
            }

            var task1 = GetTextAsync(UnityWebRequest.Get("http://google.com"));
            var task2 = GetTextAsync(UnityWebRequest.Get("http://bing.com"));
            var task3 = GetTextAsync(UnityWebRequest.Get("http://yahoo.com"));

            // concurrent async-wait and get result easily by tuple syntax
            var (google, bing, yahoo) = await UniTask.WhenAll(task1, task2, task3);

            // shorthand of WhenAll, tuple can await directly
            var (google2, bing2, yahoo2) = await (task1, task2, task3);

            // You can handle timeout easily
            await GetTextAsync(UnityWebRequest.Get("http://unity.com")).Timeout(TimeSpan.FromMilliseconds(300));

            // return async-value.(or you can use `UniTask`(no result), `UniTaskVoid`(fire and forget)).
            return (asset as TextAsset)?.text ?? throw new InvalidOperationException("Asset not found");
        }

        private IEnumerator FooCoroutineEnumerator()
        {
            yield return new WaitForSeconds(1f);
        }
    }
}