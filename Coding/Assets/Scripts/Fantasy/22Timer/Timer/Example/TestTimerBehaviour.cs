using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UnityTimer.Examples
{
    public class TestTimerBehaviour : MonoBehaviour
    {
        #region Unity Inspector Fields

        [Header("Controls")] public InputField DurationField;

        public Button StartTimerButton;
        public Button CancelTimerButton;
        public Button PauseTimerButton;
        public Button ResumeTimerButton;

        public Toggle IsLoopedToggle;
        public Toggle UseGameTimeToggle;

        public Slider TimescaleSlider;

        public Text NeedsRestartText;

        [Header("Stats Texts")] public Text TimeElapsedText;
        public Text TimeRemainingText;
        public Text PercentageCompletedText;
        public Text PercentageRemainingText;

        public Text NumberOfLoopsText;
        public Text IsCancelledText;
        public Text IsCompletedText;
        public Text IsPausedText;
        public Text IsDoneText;
        public Text UpdateText;

        #endregion

        private int _numLoops;
        private Timer _testTimer;
        private Timer loopOrTimer;
        private int updateCallLoops;
        private void Awake()
        {
            ResetState();
        }

        private void ResetState()
        {
            _numLoops = 0;
            CancelTestTimer();
        }

        public void StartTestTimer()
        {
            ResetState();

            // this is the important code example bit where we register a new timer
            _testTimer = Timer.Register(
                duration: GetDurationValue(),
                onComplete: () => _numLoops++,
                onUpdate: secondsElapsed =>
                {
                    UpdateText.text = string.Format("Timer ran update callback: {0:F2} seconds", secondsElapsed);
                },
                isLooped: IsLoopedToggle.isOn,
                useRealTime: !UseGameTimeToggle.isOn);

            CancelTimerButton.interactable = true;
        }

        public void CancelTestTimer()
        {
            Timer.Cancel(_testTimer);
            CancelTimerButton.interactable = false;
            NeedsRestartText.gameObject.SetActive(false);
        }

        public void PauseTestTimer()
        {
            Timer.Pause(_testTimer);
        }

        public void ResumeTestTimer()
        {
            Timer.Resume(_testTimer);
        }
        /// <summary>
        /// 延迟执行方法
        /// </summary>
        /// <param name="duration"></param>
        public void DeferredExecutionMethod(float duration)
        {
            loopOrTimer=Timer.Register(duration, () => Debug.Log("Hello World:"+duration));
        }
        /// <summary>
        /// 延迟执行方法(循环执行)
        /// </summary>
        /// <param name="duration"></param>
        public void DeferredExecutionMethodLoop(float duration)
        {
            loopOrTimer=Timer.Register(duration, () => Debug.Log("Hello World"),null,true);
        }
        /// <summary>
        /// 取消执行
        /// </summary>
        public void CancelLoopOrTimer()
        {
            Timer.Cancel(loopOrTimer);
        }
        /// <summary>
        /// 销毁
        /// </summary>
        public void OnDestroyThis()
        {
            Destroy(gameObject);
        }
        /// <summary>
        /// 对象绑定
        /// </summary>
        /// <param name="duration"></param>
        public void ObjectBinding(float duration)
        {
            //法一
            // this.AttachTimer(duration, () => {
            //     Debug.Log("Hello World对象绑定");
            // },null,true);
            //法二
            loopOrTimer=Timer.Register(duration, () => Debug.Log("Hello World对象绑定"),null,true,false,this);
        }
        /// <summary>
        /// Update回调(对象绑定且在duration时间不停执行onUpdate内容，直至结束)
        /// </summary>
        /// <param name="duration"></param>
        public void UpdateCall(float duration)
        {
            loopOrTimer=Timer.Register(duration,onComplete: () =>
            {
                Debug.Log("updateCallLoops:" + updateCallLoops);
            }, secondsElapsed =>{
                updateCallLoops++;
                Debug.Log($"secondsElapsed:{secondsElapsed}");
                Debug.Log($"updateCallLoops:{updateCallLoops}");
            });
        }
        /// <summary>
        /// Update回调(对象绑定且在duration时间不停执行onUpdate内容，直至销毁)
        /// </summary>
        /// <param name="duration"></param>
        public void UpdateCallLoop(float duration)
        {
            loopOrTimer=Timer.Register(duration,onComplete: () =>
            {
                Debug.Log("updateCallLoops:" + updateCallLoops);
            }, secondsElapsed =>{
                updateCallLoops++;
                Debug.Log($"secondsElapsed:{secondsElapsed}");
                Debug.Log($"updateCallLoops:{updateCallLoops}");
            },true,false,this);
        }
        private void Update()
        {
            if (_testTimer == null)
            {
                return;
            }

            Time.timeScale = TimescaleSlider.value;
            _testTimer.isLooped = IsLoopedToggle.isOn;

            TimeElapsedText.text = string.Format("Time elapsed: {0:F2} seconds", _testTimer.GetTimeElapsed());
            TimeRemainingText.text = string.Format("Time remaining: {0:F2} seconds", _testTimer.GetTimeRemaining());
            PercentageCompletedText.text = string.Format("Percentage completed: {0:F4}%",
                _testTimer.GetRatioComplete()*100);
            PercentageRemainingText.text = String.Format("Percentage remaining: {0:F4}%",
                _testTimer.GetRatioRemaining()*100);
            NumberOfLoopsText.text = string.Format("# Loops: {0}", _numLoops);
            IsCancelledText.text = string.Format("Is Cancelled: {0}", _testTimer.isCancelled);
            IsCompletedText.text = string.Format("Is Completed: {0}", _testTimer.isCompleted);
            IsPausedText.text = String.Format("Is Paused: {0}", _testTimer.isPaused);
            IsDoneText.text = string.Format("Is Done: {0}", _testTimer.isDone);

            PauseTimerButton.interactable = !_testTimer.isPaused;
            ResumeTimerButton.interactable = _testTimer.isPaused;

            NeedsRestartText.gameObject.SetActive(ShouldShowRestartText());
        }

        private bool ShouldShowRestartText()
        {
            return !_testTimer.isDone && // the timer is in progress and...
                   ((UseGameTimeToggle.isOn && _testTimer.usesRealTime) || // we switched to real-time or
                    (!UseGameTimeToggle.isOn && !_testTimer.usesRealTime) || // we switched to game-time or
                    Mathf.Abs(GetDurationValue() - _testTimer.duration) >= Mathf.Epsilon); // our duration changed
        }

        private float GetDurationValue()
        {
            float duration;
            return float.TryParse(DurationField.text, out duration) ? duration : 0;
        }
    }
}