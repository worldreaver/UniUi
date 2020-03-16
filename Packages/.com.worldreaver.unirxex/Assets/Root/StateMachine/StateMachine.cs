using UnityEngine;
using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;


namespace ExtraUniRx.StateMachines
{
    /// <summary>
    /// State machine
    /// </summary>
    public class StateMachine : MonoBehaviour
    {
        #region variable

        [SerializeField, Tooltip("Auto start FSM via Unity Start() function")]
        public bool autoStart = true;

        /// <summary>
        /// State map
        /// </summary>
        private readonly Dictionary<Type, State> _stateMap = new Dictionary<Type, State>();

        /// <summary>
        /// Current state
        /// </summary>
        [SerializeField, Tooltip("Set initial state here")]
        private State currentState = null;

        /// <summary>
        /// Transition destination state
        /// </summary>
        private Type _nextState = null;

        #endregion

        #region property

        public State CurrentState => currentState;

        #endregion

        #region event

        private void Awake()
        {
            foreach (var state in GetComponents<State>())
            {
                Register(state);
            }
        }

        private void Start()
        {
            if (autoStart)
            {
                StartFsm();
            }
        }

        #endregion

        #region method

        /// <summary>
        /// Start the state machine
        /// </summary>
        public void StartFsm()
        {
            if (currentState == null)
            {
                Debug.LogError("Plese assign Current State via Hierachy or call SetFirstState");
            }

            currentState.StateBegin();

            // Perform transition processing if transition destination state exists
            this.LateUpdateAsObservable()
                .Where(_ => _nextState != null)
                .Subscribe(_ =>
                {
                    // Exit current state
                    currentState.StateEnd();

                    // Start next state
                    currentState = _stateMap[_nextState];
                    currentState.StateBegin();

                    // Initialize transition destination with null
                    _nextState = null;
                });
        }

        /// <summary>
        /// Register Initial State
        /// </summary>
        /// <param name="firstState">Initial state</param>
        public void SetFirstState(State firstState)
        {
            currentState = firstState;
        }

        /// <summary>
        /// Register State
        /// </summary>
        /// <param name="state">State to register</param>
        public void Register(State state)
        {
            _stateMap.Add(state.GetType(), state);
            state.StateMachine = this;
        }

        /// <summary>
        /// State transition reservation
        /// </summary>
        public void Transition<T>() where T : State
        {
            _nextState = typeof(T);
        }

        #endregion
    }
}
