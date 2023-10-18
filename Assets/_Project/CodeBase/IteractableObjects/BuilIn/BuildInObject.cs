using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace CodeBase.IteractableObjects
{
    public class BuildInObject : InteractObject, IGrabHolder
    {
        public UnityEvent OnBuiltInEvent;

        [SerializeField] private Material disactivatedMat;
        [SerializeField] private Material promtMat;
        private Material activatedMat;

        [SerializeField] private Renderer rend;

        [Header("Get GrabInteractable Object")]
        [SerializeField] private float moveDuration = 0.15f;
        [SerializeField] private Ease moveAnimation = Ease.InElastic;

        [SerializeField] public string contactType;

        private GrabbableObject grabbableObject;

        public bool IsEmpty { get => grabbableObject == null; }
        public Vector3 GrabPosition { get => transform.localPosition; }
        public float MoveDuration { get => moveDuration; }
        public Ease MoveAnimation { get => moveAnimation; }



        protected override void Awake()
        {
            base.Awake();

            if (rend == null)
                throw new Exception($"Rendere is null {gameObject.name}");

            activatedMat = rend.material;
            SetActive(ActiveType.Diactivate);
        }

        internal void SetActive(ActiveType active)
        {
            switch (active)
            {
                case ActiveType.Diactivate:
                    rend.material = disactivatedMat;
                    break;

                case ActiveType.Promt:
                    rend.material = promtMat;
                    break;

                case ActiveType.Activate:
                    rend.material = activatedMat;
                    break;
            }
        }
        internal void SetUp(GrabbableObject grabbableObject)
        {
            if (!grabbableObject) return;

            grabbableObject.OnSetEvent += OnSetGrabbableHandler;
            grabbableObject.transform.SetParent(transform);
            grabbableObject.Grab(this, true);
        }
        private void OnSetGrabbableHandler(GrabbableObject obj)
        {
            obj.OnSetEvent -= OnSetGrabbableHandler;
            grabbableObject = obj;
            SetActive(ActiveType.Activate);
            grabbableObject.gameObject.SetActive(false);
            OnBuiltInEvent?.Invoke();
        }
    }
}
