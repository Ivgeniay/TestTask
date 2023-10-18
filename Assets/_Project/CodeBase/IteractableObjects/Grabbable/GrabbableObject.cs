using CodeBase.PlayerBehaviour;
using DG.Tweening;
using System;
using UnityEngine;

namespace CodeBase.IteractableObjects
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class GrabbableObject : InteractObject
    {
        internal event Action<GrabbableObject> OnSetEvent; 
        private Rigidbody rb;

        protected override void Awake()
        {
            base.Awake();
            rb = GetComponent<Rigidbody>();
        }

        internal void Grab(IGrabHolder interacteComponent, bool isGrab)
        {
            rb.isKinematic = isGrab;
            if(interacteComponent != null && isGrab)
            {
                transform
                .DOLocalMove(interacteComponent.GrabPosition, interacteComponent.MoveDuration)
                .SetEase(interacteComponent.MoveAnimation)
                .OnComplete(() => OnSetEvent?.Invoke(this)); ;
            }
        }

        internal Rigidbody GetRigidbody() => rb;
    }
}
