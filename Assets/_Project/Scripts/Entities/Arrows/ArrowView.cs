using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.EventSystems;
using System.Collections;
using Clock.Entities.Clocks;

namespace Clock.Entities
{
    internal class ArrowView : MonoBehaviour, IGameModeDependence, IDragHandler, IDropHandler
    {
        public event Action<object, float> OnPointerMovedEvent;

        [SerializeField] private AnalogueController analogueController;
        [SerializeField] private float angle;
        [SerializeField] private RectTransform rectTransform;


        private Tween tween;
        private Core core { get => Core.Instance; }
        private GameModeService gameModeService = null;
        private bool isDragged = false;

        public void RotateArrow(float angle, bool ignoreDragged = false)
        {
            if (!ignoreDragged)
                if (isDragged) return;

            if (tween != null) tween.Kill();
            tween = rectTransform
                .DORotate(new Vector3(0f, 0f, -angle), 0.3f)
                .OnComplete(OnAnimationComplete);
        }

        private IEnumerator Start()
        {
            yield return core.IsLoaded;
            gameModeService = core.GetService<GameModeService>();
            gameModeService.Register(this);
        }

        private void OnEnable()
        {
            gameModeService?.Register(this);
        }

        private void OnDisable()
        {
            tween?.Kill();
            OnAnimationComplete();
            gameModeService?.Unregister(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (gameModeService.CurrentGameMode != GameMode.Edit) return;

            isDragged = true;
            Vector3 vector = eventData.position - new Vector2(rectTransform.position.x, rectTransform.position.y);
            int sign = Math.Sign(eventData.position.x - rectTransform.position.x);
            var angle = Vector3.Angle(vector, Vector2.up) * sign;
            RotateArrow(angle, true);
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (gameModeService.CurrentGameMode != GameMode.Edit) return;

            OnPointerMovedEvent?.Invoke(this, rectTransform.rotation.eulerAngles.z);
        }

        private void OnAnimationComplete()
        {
            if (gameModeService.CurrentGameMode != GameMode.Edit) return;

            if (isDragged) OnPointerMovedEvent?.Invoke(this, rectTransform.rotation.eulerAngles.z);
            isDragged = false;
        }

        public void OnGameStateChange(GameMode currentGameMode) { }
    }
}
