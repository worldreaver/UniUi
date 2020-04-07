using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Worldreaver.Utility;

namespace Worldreaver.UniUI
{
    [RequireComponent(typeof(Image))]
    public class UniButton : Button, IButton, IAffect
    {
        #region Property

#pragma warning disable 0649
        [SerializeField] private EPivot pivot = EPivot.MiddleCenter;
        [SerializeField] private bool isMotion;
        [SerializeField] private EUIMotionType motionType = EUIMotionType.Immediate;
        [SerializeField] private bool isAffectToSelf = true;
        [SerializeField] private RectTransform affectObject;
        [SerializeReference] private IMotion _motion;
#pragma warning restore 0649

        #endregion

        #region Implementation of IButton

        public EPivot Pivot => pivot;
        public bool IsMotion => isMotion;
        public bool IsRelease { get; private set; }
        public bool IsPrevent { get; private set; }

        public EUIMotionType MotionType
        {
            get => motionType;
            private set => motionType = value;
        }

        #endregion

        #region Implementation of IAffect

        public Vector3 DefaultScale { get; set; }
        public bool IsAffectToSelf => isAffectToSelf;

        public RectTransform AffectObject => IsAffectToSelf ? targetGraphic.rectTransform : affectObject;

        #endregion

        #region Overrides of UIBehaviour

        protected override void Start()
        {
            base.Start();
            InitializeMotion();
            DefaultScale = AffectObject.localScale;
        }

        #region Overrides of Selectable

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            //use Invoke to call method to avoid warning "Send Message cannot be called during Awake, or OnValidate...
            Invoke(nameof(RefreshPivot), 0.1f);
            Invoke(nameof(InitializeMotion), 0.1f);
        }

        protected void RefreshPivot()
        {
            switch (Pivot)
            {
                case EPivot.LowerLeft:
                    Util.SetPivot(targetGraphic.rectTransform, Vector2.zero);
                    break;
                case EPivot.LowerCenter:
                    Util.SetPivot(targetGraphic.rectTransform, new Vector2(0.5f, 0));
                    break;
                case EPivot.LowerRight:
                    Util.SetPivot(targetGraphic.rectTransform, Vector2.right);
                    break;
                case EPivot.MiddleLeft:
                    Util.SetPivot(targetGraphic.rectTransform, new Vector2(0f, 0.5f));
                    break;
                case EPivot.MiddleCenter:
                    Util.SetPivot(targetGraphic.rectTransform, new Vector2(0.5f, 0.5f));
                    break;
                case EPivot.MiddleRight:
                    Util.SetPivot(targetGraphic.rectTransform, new Vector2(1f, 0.5f));
                    break;
                case EPivot.UpperLeft:
                    Util.SetPivot(targetGraphic.rectTransform, Vector2.up);
                    break;
                case EPivot.UpperCenter:
                    Util.SetPivot(targetGraphic.rectTransform, new Vector2(0.5f, 1));
                    break;
                case EPivot.UpperRight:
                    Util.SetPivot(targetGraphic.rectTransform, Vector2.one);
                    break;
                default:
                    Util.SetPivot(targetGraphic.rectTransform, new Vector2(0.5f, 0.5f));
                    break;
            }
        }

#endif
        public override void OnPointerExit(PointerEventData eventData)
        {
            if (IsRelease) return;
            base.OnPointerExit(eventData);
            IsPrevent = true;
            OnPointerUp(eventData);
        }

        #endregion

        #region Overrides of Button

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            IsRelease = false;
            IsPrevent = false;
            if (IsMotion) _motion?.MotionDown(DefaultScale, AffectObject);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (IsRelease) return;
            base.OnPointerUp(eventData);
            IsRelease = true;
            if (IsMotion) _motion?.MotionUp(DefaultScale, AffectObject);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (IsRelease && IsPrevent) return;
            base.OnPointerClick(eventData);
        }

        #endregion

        protected void InitializeMotion()
        {
            if (_motion == null)
            {
                switch (MotionType)
                {
                    case EUIMotionType.Immediate:
                        _motion = new MotionImmediate();
                        break;
                    case EUIMotionType.CurveDownCurveUp:
                        _motion = new MotionCurveCurve();
                        break;
                    case EUIMotionType.CurveDownEaseUp:
                        _motion = new MotionCurveEase();
                        break;
                    case EUIMotionType.EaseDownCurveUp:
                        _motion = new MotionEaseCurve();
                        break;
                    case EUIMotionType.EaseDownEaseUp:
                        _motion = new MotionEaseEase();
                        break;
                    case EUIMotionType.UniformCurveDownCurveUp:
                        _motion = new UniformMotionCurveCurve();
                        break;
                    case EUIMotionType.UniformCurveDownEaseUp:
                        _motion = new UniformMotionCurveEase();
                        break;
                    case EUIMotionType.UniformEaseDownCurveUp:
                        _motion = new UniformMotionEaseCurve();
                        break;
                    case EUIMotionType.UniformEaseDownEaseUp:
                        _motion = new UniformMotionEaseEase();
                        break;
                }
            }
            else
            {
                switch (MotionType)
                {
                    case EUIMotionType.Immediate when _motion.GetType() != typeof(MotionImmediate):
                        _motion = new MotionImmediate();
                        break;
                    case EUIMotionType.CurveDownCurveUp when _motion.GetType() != typeof(MotionCurveCurve):
                        _motion = new MotionCurveCurve();
                        break;
                    case EUIMotionType.CurveDownEaseUp when _motion.GetType() != typeof(MotionCurveEase):
                        _motion = new MotionCurveEase();
                        break;
                    case EUIMotionType.EaseDownCurveUp when _motion.GetType() != typeof(MotionEaseCurve):
                        _motion = new MotionEaseCurve();
                        break;
                    case EUIMotionType.EaseDownEaseUp when _motion.GetType() != typeof(MotionEaseEase):
                        _motion = new MotionEaseEase();
                        break;
                    case EUIMotionType.UniformCurveDownCurveUp when _motion.GetType() != typeof(UniformMotionCurveCurve):
                        _motion = new UniformMotionCurveCurve();
                        break;
                    case EUIMotionType.UniformCurveDownEaseUp when _motion.GetType() != typeof(UniformMotionCurveEase):
                        _motion = new UniformMotionCurveEase();
                        break;
                    case EUIMotionType.UniformEaseDownCurveUp when _motion.GetType() != typeof(UniformMotionEaseCurve):
                        _motion = new UniformMotionEaseCurve();
                        break;
                    case EUIMotionType.UniformEaseDownEaseUp when _motion.GetType() != typeof(UniformMotionEaseEase):
                        _motion = new UniformMotionEaseEase();
                        break;
                }
            }
        }

        #endregion
    }
}