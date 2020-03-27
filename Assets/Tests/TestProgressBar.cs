using UnityEngine;
using Worldreaver.UniUI;

public class TestProgressBar : MonoBehaviour
{
    public UniProgressBar progressBar;

    private void Start()
    {
        progressBar.Initialized(0, 100);
    }

    public void Decrease()
    {
        progressBar.DecreaseGuard(30);
    }
    
    public void ResetProgress()
    {
        progressBar.ResetProgress();
    }
}