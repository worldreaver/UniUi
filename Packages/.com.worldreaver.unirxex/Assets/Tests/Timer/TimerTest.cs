using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Worldreaver.Timer;

public class TimerTest : MonoBehaviour
{
    public Button btnStart;
    public Button btnPause;
    public Button btnResume;
    public Button btnGetCurrentRemainTime;
    public Button btnGetCurrentElapsedTime;
    public Button btnIsPlaying;
    public Button btnIncreaseTime;
    public Button btnDecreaseTime;
    public InputField inputTime;
    public InputField inputTimeChange;
    public Text txtTime;
    public Text txtLog;
    public bool useTimeScale;

    private Timer _timer;

    private void Start()
    {
        btnStart.onClick.AddListener(StartTime);
        btnPause.onClick.AddListener(Pause);
        btnResume.onClick.AddListener(Resume);
        btnGetCurrentRemainTime.onClick.AddListener(GetCurrentRemainTime);
        btnGetCurrentElapsedTime.onClick.AddListener(GetCurrentElapsedTime);
        btnIsPlaying.onClick.AddListener(IsPlaying);
        btnIncreaseTime.onClick.AddListener(Add5);
        btnDecreaseTime.onClick.AddListener(Minus5);
    }

    private void StartTime()
    {
        _timer = useTimeScale ? new Timer() : new Timer(new UnscaledTimeScheduler());

        _timer.StartedAsObservable.Subscribe(_ => Debug.Log("Start"));
        _timer.FinishedAsObservable.Subscribe(_ =>
        {
            Debug.Log("Complete!");
            _timer.Stop();
        });
        _timer.StoppedTimeAsObservable.Subscribe(_ => Debug.Log("HAHA it is completed"));
        _timer.PausedTimeAsObservable.Subscribe(_ => Debug.Log("pause"));
        _timer.ResumedAsObservable.Subscribe(_ => Debug.Log("Resume"));
        _timer.Start(int.Parse(inputTime.text));
        //_timer.ElapsedTimeAsObservable.Subscribe(_ => Debug.Log($" Elaspsed :{_}"));
        _timer.RemainTimeAsObservable.Subscribe(_ =>
        {
            txtTime.text = _ + "";
            Debug.Log($" Remain :{_}");
        });
        _timer.RemainTimeAsObservable.Select(it => Mathf.CeilToInt(it)).DistinctUntilChanged().Subscribe(_ => Debug.Log($" Remain :{_}")).AddTo(this);

        _timer.RemainTimeAsObservable.Subscribe(time => this.Render(time, _timer.CurrentFinishTime)).AddTo(this);
        _timer.IsPlayingAsObservable.Subscribe(_ => Debug.LogWarning("IsPlayingAsObservable : " + _));
    }

    private void Render(float time,
        float finishTime)
    {
        var ratio = finishTime > 0 ? time / finishTime : 1f;
        Debug.Log($"ratio :{ratio}");
    }


    public void Pause()
    {
        _timer.Pause();
    }

    public void Resume()
    {
        _timer.Resume();
    }

    public void GetCurrentRemainTime()
    {
        txtLog.text = $"CurrentRemainTime: {_timer.GetRemainTime()}";
        Debug.LogWarning($"CurrentRemainTime: {_timer.GetRemainTime()}");
    }

    public void GetCurrentElapsedTime()
    {
        txtLog.text = $"CurrentElapsedTime: {_timer.GetElapsedTime()}";
        Debug.LogWarning($"CurrentElapsedTime: {_timer.GetElapsedTime()}");
    }

    public void IsPlaying()
    {
        txtLog.text = $"IsPlaying: {_timer.IsPlaying}";
        Debug.LogWarning($"IsPlaying: {_timer.IsPlaying}");
    }

    public void Add5()
    {
        _timer.IncreaseTime(int.Parse(inputTimeChange.text));
    }

    public void Minus5()
    {
        _timer.DecreaseTime(int.Parse(inputTimeChange.text));
    }
}