using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;


/*
* Inherit this State class and define various states
*
*
* Various processes are described using the streams of Begin, End, Update, and LateUpdate
*
* ex)
* BeginStream.Subscribe (_ => Debug.Log ("State begin"));
* EndStream.Subscribe (_ => Debug.Log ("State end"));
* UpdateStream.Subscribe (_ => Debug.Log ("State update"));
* ...
*
* Use the Transition function to transition to another state
* ex)
* UpdateStream.Where (_ => Input.GetMouseButtonDown (0))
* .Subscribe (_ => Transition (typeof (OtherState)));
*
* Generic version)
* UpdateStream.Where (_ => Input.GetMouseButtonDown (0))
* .Subscribe (_ => Transition <OtherState> ());
*/


namespace ExtraUniRx.StateMachines
{
    /// <summary>
    /// State class
    /// </summary>
    [RequireComponent(typeof(StateMachine))]
    public abstract class State : MonoBehaviour
    {
        #region variable

        /// <summary>
        /// State machine
        /// </summary>
        public StateMachine StateMachine { get; set; }

        /// <summary>
        /// State start stream
        /// </summary>
        private readonly Subject<Unit> _begin = new Subject<Unit>();

        /// <summary>
        /// State end stream
        /// </summary>
        private readonly Subject<Unit> _end = new Subject<Unit>();

        #endregion

        #region event stream

        /// <summary>
        /// State start stream
        /// </summary>
        public IObservable<Unit> BeginStream => _begin.AsObservable();

        /// <summary>
        /// State end stream
        /// </summary>
        public IObservable<Unit> EndStream => _end.AsObservable();

        /// <summary>
        /// Update stream
        /// </summary>
        public IObservable<Unit> UpdateStream => StateStream(this.UpdateAsObservable());

        /// <summary>
        ///FixedUpdate stream
        /// </summary>
        public IObservable<Unit> FixedUpdateStream => StateStream(this.FixedUpdateAsObservable());

        /// <summary>
        /// LateUpdate stream
        /// </summary>
        public IObservable<Unit> LateUpdateStream => StateStream(this.LateUpdateAsObservable());

        /// <summary>
        /// OnDrawGizmos stream
        /// </summary>
        public IObservable<Unit> OnDrawGizmosStream => StateStream(this.OnDrawGizmosAsObservable());

        /// <summary>
        /// OnGUI stream
        /// </summary>
        public IObservable<Unit> OnGUIStream => StateStream(this.OnGUIAsObservable());

        /// <summary>
        /// The stream through which messages flow only while this state is the current state
        /// </summary>
        protected IObservable<T> StateStream<T>(IObservable<T> source)
        {
            return source
                // Since BeginStream is OnNext
                .SkipUntil(BeginStream)
                // Until EndStream is OnNext
                .TakeUntil(EndStream)
                .RepeatUntilDestroy(gameObject)
                .Publish()
                .RefCount();
        }

        #endregion

        #region method

        /// <summary>
        /// State start notification (not called except for StateMachine)
        /// </summary>
        public void StateBegin()
        {
            _begin.OnNext(default(Unit));
        }

        /// <summary>
        /// State end notification (Basically not called except for StateMachine)
        /// </summary>
        public void StateEnd()
        {
            _end.OnNext(default(Unit));
        }

        /// <summary>
        /// State transition reservation
        /// </summary>
        protected void Transition<T>() where T : State
        {
            StateMachine.Transition<T>();
        }

        #endregion
    }
}