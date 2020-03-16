using UnityEngine;
using UnityEngine.UI;
using UniRx;
using ExtraUniRx.StateMachines;


[DisallowMultipleComponent]
public class IdleState : State
{
    #region variable

    [Header("Parameter")]

    //Time to transition to appeal state
    public const float TRANSITION_TO_APPEAL_DURATION = 3f;


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
        // Play idle animation
        BeginStream.Subscribe(_ => _animator.Play("Idle"));

        // Change the language of the tutorial
        BeginStream.Subscribe(_ => _tutorialText.text = "Transit to driving state with left click");


        float counter = 0f;

        // Transit to appeal state after n seconds
        UpdateStream.Do(_ => counter += Time.deltaTime)
            .Where(count => counter > TRANSITION_TO_APPEAL_DURATION)
            .Subscribe(_ => Transition<AppealState>());

        // Transit to driving state when left-clicked
        UpdateStream.Where(_ => Input.GetMouseButtonDown(0))
            .Subscribe(_ => Transition<RunState>());

        // Reset counter when performing state transition
        EndStream.Subscribe(_ => counter = 0f);
    }

    #endregion
}