using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Worldreaver.Utility;

namespace Worldreaver.UniUI
{
    [RequireComponent(typeof(Image))]
    public class UniTap : Toggle, IToggle, IToggleAffect
    {
        #region Property

#pragma warning disable 649
        [SerializeField] private EPivot pivot = EPivot.MiddleCenter;
        [SerializeField] private List<RectTransform> activeObjects = new List<RectTransform>();
        [SerializeField] private List<RectTransform> deactiveObjects = new List<RectTransform>();
        [SerializeField] private bool isMotion;
        [SerializeField] private bool allowMotionDisable;
        [SerializeField] private bool isSwitchSprite;
        [SerializeField] private Sprite activeSprite;
        [SerializeField] private Sprite deactiveSprite;
        [SerializeField] private bool isAffectToSelf = true;
        [SerializeField] private Image affectObject;
        [SerializeField] private bool isExpan;
        [SerializeField] private bool isExpanLayout;
        [SerializeField] private LayoutElement layoutElement;
        [SerializeField] private Vector2 valueExpand = Vector2.one;
        [SerializeField] private Vector2 valueFlexible = new Vector2(-1, -1);
        [SerializeField] private Vector3 selectedScale;
        [SerializeField] private Vector3 unSelectedScale;
        [SerializeField] private EUIMotionType motionType = EUIMotionType.Immediate;
        [SerializeField] private EUIMotionType motionTypeDisable = EUIMotionType.Immediate;
        [SerializeReference] private IMotion _motion;
        [SerializeReference] private IMotion _motionDisable;
        private IDisposable _disposablePointDown;
        private IDisposable _disposablePointUp;
#pragma warning restore 649

        #endregion

        #region Implementation of IToggle

        public EPivot Pivot => pivot;
        public bool AllowMotionDisable => allowMotionDisable;
        public bool IsRelease { get; private set; }
        public bool IsPrevent { get; private set; }
        public bool IsSwitchSprite => isSwitchSprite;
        public bool IsMotion { get => isMotion; set => isMotion = value; }

        public List<RectTransform> ActiveObjects { get => activeObjects; set => activeObjects = value; }

        public List<RectTransform> DeactiveObjects { get => deactiveObjects; set => deactiveObjects = value; }

        public Sprite ActiveSprite { get => activeSprite; set => activeSprite = value; }

        public Sprite DeactiveSprite { get => deactiveSprite; set => deactiveSprite = value; }

        public Image TargetGraphicImage => targetGraphic as Image;
        public Image GraphicImage => graphic as Image;
        public EUIMotionType MotionType => motionType;

        public EUIMotionType MotionTypeDisable { get => motionTypeDisable; private set => motionTypeDisable = value; }

        public IMotion Motion => _motion;
        public IMotion MotionDisable => _motionDisable;

        #endregion

        #region Implementation of IToggleAffect

        public Vector3 DefaultScale { get; set; }
        public bool IsAffectToSelf => isAffectToSelf;

        public RectTransform AffectObject
        {
            get
            {
                if (IsAffectToSelf)
                {
                    var targetGraphic1 = targetGraphic;
                    return targetGraphic1 == null ? null : targetGraphic1.rectTransform;
                }

                return affectObject == null ? null : affectObject.rectTransform;
            }
        }

        public bool IsExpan => isExpan;
        public Vector2 ValueExpand => valueExpand;
        public Vector2 ValueFlexible => valueFlexible;
        public LayoutElement LayoutElement => layoutElement;
        public Vector3 SelectedScale => selectedScale;
        public Vector3 UnSelectedScale => unSelectedScale;

        #endregion

        #region Overrides of Toggle

        protected override void Start()
        {
            base.Start();
            InitializeMotion();
            DefaultScale = AffectObject.localScale;
            unSelectedScale = DefaultScale;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            onValueChanged.AddListener(OnValueChanged);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            onValueChanged.RemoveListener(OnValueChanged);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (!IsAffectToSelf && AffectObject != null)
            {
                unSelectedScale = AffectObject.localScale;
            }

            Invoke(nameof(FindObjectAffect), 0.1f);
            Invoke(nameof(RefreshPivot), 0.1f);
            Invoke(nameof(FindLayoutElement), 0.1f);
            Invoke(nameof(InitializeMotion), 0.1f);
        }

        private void FindObjectAffect()
        {
            if (IsAffectToSelf) return;
            if (affectObject == null)
            {
                affectObject = GetComponentsInChildren<Image>(true).FirstOrDefault(_ => _.gameObject != gameObject);
            }
        }

        private void FindLayoutElement()
        {
            if (!IsExpan) return;
            if (!isExpanLayout) return;
            if (layoutElement == null)
            {
                layoutElement = GetComponentInChildren<LayoutElement>(true);
            }

            if (layoutElement == null) return;
            LayoutElement.flexibleWidth = valueFlexible.x;
            LayoutElement.flexibleHeight = valueFlexible.y;
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

        #region Overrides of Selectable

        public override void OnPointerDown(
            PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            IsRelease = false;
            IsPrevent = false;
            if (!IsMotion) return;
            if (!interactable && AllowMotionDisable)
            {
                _motionDisable?.MotionDown(DefaultScale, AffectObject);
                return;
            }

            _motion?.MotionDown(DefaultScale, AffectObject);
        }

        public override void OnPointerUp(
            PointerEventData eventData)
        {
            if (IsRelease) return;
            base.OnPointerUp(eventData);
            IsRelease = true;
            if (!IsMotion) return;
            if (!interactable && AllowMotionDisable)
            {
                _motionDisable?.MotionUp(DefaultScale, AffectObject);
                return;
            }

            _motion?.MotionUp(DefaultScale, AffectObject);
        }

        public override void OnPointerExit(
            PointerEventData eventData)
        {
            if (IsRelease) return;
            base.OnPointerExit(eventData);
            IsPrevent = true;
            OnPointerUp(eventData);
        }

        public override void OnPointerClick(
            PointerEventData eventData)
        {
            if (IsRelease && IsPrevent) return;
            base.OnPointerClick(eventData);
        }

        #endregion

        #endregion

        private void Refresh()
        {
            for (int i = 0; i < ActiveObjects.Count; i++)
            {
                ActiveObjects[i].gameObject.SetActive(isOn);
            }

            for (int i = 0; i < DeactiveObjects.Count; i++)
            {
                DeactiveObjects[i].gameObject.SetActive(!isOn);
            }

            if (IsSwitchSprite)
            {
                if (IsAffectToSelf)
                {
                    TargetGraphicImage.sprite = isOn ? ActiveSprite : DeactiveSprite;
                }
                else
                {
                    if (affectObject != null)
                    {
                        affectObject.sprite = isOn ? ActiveSprite : DeactiveSprite;
                    }
                }
            }

            if (!IsExpan) return;


            if (isExpanLayout && LayoutElement != null)
            {
                if (isOn)
                {
                    if (LayoutElement.flexibleWidth >= 0)
                    {
                        LayoutElement.flexibleWidth = ValueExpand.x;
                    }

                    if (LayoutElement.flexibleHeight >= 0)
                    {
                        LayoutElement.flexibleHeight = ValueExpand.y;
                    }
                }
                else
                {
                    if (LayoutElement.flexibleWidth >= 0)
                    {
                        LayoutElement.flexibleWidth = valueFlexible.x;
                    }

                    if (LayoutElement.flexibleHeight >= 0)
                    {
                        LayoutElement.flexibleHeight = valueFlexible.y;
                    }
                }
            }

            if (IsAffectToSelf) return;
            if (AffectObject != null)
            {
                DefaultScale = isOn ? SelectedScale : UnSelectedScale;
                AffectObject.localScale = DefaultScale;
            }
        }

        private void OnValueChanged(
            bool isOn)
        {
            Refresh();
        }

        protected void InitializeMotion()
        {
            if (!IsMotion) return;

            if (_motion == null)
            {
                switch (MotionType)
                {
                    case EUIMotionType.Immediate:
                        _motion = new MotionImmediate();
                        break;
                    case EUIMotionType.NormalCurve:
                        _motion = new MotionCurve();
                        break;
                    case EUIMotionType.NormalEase:
                        _motion = new MotionEase();
                        break;
                    case EUIMotionType.UniformCurve:
                        _motion = new UniformMotionCurve();
                        break;
                    case EUIMotionType.UniformEase:
                        _motion = new UniformMotionEase();
                        break;
                    case EUIMotionType.LateCurve:
                        _motion = new LateMotionCurve();
                        break;
                    case EUIMotionType.LateEase:
                        _motion = new LateMotionEase();
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
                    case EUIMotionType.NormalCurve when _motion.GetType() != typeof(MotionCurve):
                        _motion = new MotionCurve();
                        break;
                    case EUIMotionType.NormalEase when _motion.GetType() != typeof(MotionEase):
                        _motion = new MotionEase();
                        break;
                    case EUIMotionType.UniformCurve when _motion.GetType() != typeof(UniformMotionCurve):
                        _motion = new UniformMotionCurve();
                        break;
                    case EUIMotionType.UniformEase when _motion.GetType() != typeof(UniformMotionEase):
                        _motion = new UniformMotionEase();
                        break;
                    case EUIMotionType.LateCurve when _motion.GetType() != typeof(LateMotionCurve):
                        _motion = new LateMotionCurve();
                        break;
                    case EUIMotionType.LateEase when _motion.GetType() != typeof(LateMotionEase):
                        _motion = new LateMotionEase();
                        break;
                }
            }

            if (!AllowMotionDisable) return;

            if (_motionDisable == null)
            {
                switch (MotionTypeDisable)
                {
                    case EUIMotionType.Immediate:
                        _motionDisable = new MotionImmediate();
                        break;
                    case EUIMotionType.NormalCurve:
                        _motionDisable = new MotionCurve();
                        break;
                    case EUIMotionType.NormalEase:
                        _motionDisable = new MotionEase();
                        break;
                    case EUIMotionType.UniformCurve:
                        _motionDisable = new UniformMotionCurve();
                        break;
                    case EUIMotionType.UniformEase:
                        _motionDisable = new UniformMotionEase();
                        break;
                    case EUIMotionType.LateCurve:
                        _motionDisable = new LateMotionCurve();
                        break;
                    case EUIMotionType.LateEase:
                        _motionDisable = new LateMotionEase();
                        break;
                }
            }
            else
            {
                switch (MotionTypeDisable)
                {
                    case EUIMotionType.Immediate when _motionDisable.GetType() != typeof(MotionImmediate):
                        _motionDisable = new MotionImmediate();
                        break;
                    case EUIMotionType.NormalCurve when _motionDisable.GetType() != typeof(MotionCurve):
                        _motionDisable = new MotionCurve();
                        break;
                    case EUIMotionType.NormalEase when _motionDisable.GetType() != typeof(MotionEase):
                        _motionDisable = new MotionEase();
                        break;
                    case EUIMotionType.UniformCurve when _motionDisable.GetType() != typeof(UniformMotionCurve):
                        _motionDisable = new UniformMotionCurve();
                        break;
                    case EUIMotionType.UniformEase when _motionDisable.GetType() != typeof(UniformMotionEase):
                        _motionDisable = new UniformMotionEase();
                        break;
                    case EUIMotionType.LateCurve when _motionDisable.GetType() != typeof(LateMotionCurve):
                        _motionDisable = new LateMotionCurve();
                        break;
                    case EUIMotionType.LateEase when _motionDisable.GetType() != typeof(LateMotionEase):
                        _motionDisable = new LateMotionEase();
                        break;
                }
            }
        }
    }
}