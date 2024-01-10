using System;
using UnityEngine;

namespace Clock.Entities.Arrows
{
    [Serializable]
    internal class ArrowController
    {
        public event Action<object, int, float> OnPointerMovedEvent;

        [SerializeField] private ArrowView arrowView;
        [SerializeField] private int circlePart = 60;
        [SerializeField] private float currentPart = 0;

        public ArrowController(ArrowView arrowView, int circlePart)
        { 
            this.arrowView = arrowView;
            this.circlePart = circlePart;
            arrowView.OnPointerMovedEvent += OnPointerMovedHandler;
        }

        public void SetPosition(int real, int fraction)
        {
            real = real > circlePart ? real % circlePart : real;
            currentPart = real + Mathf.InverseLerp(0, 60, fraction);
            float rotationAngle = 360f * currentPart / circlePart;
            arrowView.RotateArrow(rotationAngle);
        }

        private void OnPointerMovedHandler(object sender, float angle) =>
            OnPointerMovedEvent?.Invoke(sender, circlePart, angle);
    }
}
