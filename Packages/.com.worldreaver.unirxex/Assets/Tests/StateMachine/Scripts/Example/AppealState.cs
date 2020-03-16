using ExtraUniRx.StateMachines;
using UnityEngine;
using UnityEngine.UI;
using UniRx;


[DisallowMultipleComponent]
public class AppealState : State
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
        // Play appeal animation
        BeginStream.Subscribe(_ => _animator.Play("Appeal"));

        // Change the language of the tutorial
        BeginStream.Subscribe(_ => _tutorialText.text =
            (int)IdleState.TRANSITION_TO_APPEAL_DURATION + "It has transitioned to the appeal state because seconds have passed");

        // Transition to the standby state when the play of the appeal animation is completed
        UpdateStream.Where(_ => _animator.IsCompleted(Animator.StringToHash("Appeal")))
            .Subscribe(_ => Transition<IdleState>());
        
        // Transit to driving state when left-clicked
        UpdateStream.Where(_ => Input.GetMouseButtonDown(0))
            .Subscribe(_ => Transition<RunState>());
    }

    #endregion
}