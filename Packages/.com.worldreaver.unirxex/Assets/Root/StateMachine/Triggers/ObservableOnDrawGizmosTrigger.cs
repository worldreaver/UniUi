using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;


namespace ExtraUniRx.StateMachines
{
    [DisallowMultipleComponent]
    public class ObservableOnDrawGizmosTrigger : ObservableTriggerBase
    {
        #region variable

        private Subject<Unit> _onDrawGizmos;

        #endregion

        #region property

        public IObservable<Unit> OnDrawGizmosAsObservable()
        {
            return _onDrawGizmos ?? (_onDrawGizmos = new Subject<Unit>());
        }

        #endregion

        #region method

        private void OnDrawGizmos()
        {
            _onDrawGizmos?.OnNext(default(Unit));
        }

        protected override void RaiseOnCompletedOnDestroy()
        {
            _onDrawGizmos?.OnCompleted();
        }

        #endregion
    }
}