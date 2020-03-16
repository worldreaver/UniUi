using UnityEngine;
using UnityEngine.UI;
using UniRx;
using ExtraUniRx.StateMachines;


[DisallowMultipleComponent]
public class RunState : State
{
    #region variable

    [Header("Cache")]

    [SerializeField]
    private Animator _animator = null;

    [SerializeField]
    private Text _tutorialText = null;

    #endregion

    #region event

    private void Reset()
    {
        _animator = transform.GetComponent<Animator>();
    }

    private void Start()
    {
        // Play the running animation
        BeginStream.Subscribe(_ => _animator.Play("Run"));

        // Change the language of the tutorial
        BeginStream.Subscribe(_ => _tutorialText.text = "Transit to wait state with left click");

        // Transit to wait state when left clicked
        UpdateStream.Where(_ => Input.GetMouseButtonDown(0))
            .Subscribe(_ => Transition<IdleState>());
    }

    #endregion
}