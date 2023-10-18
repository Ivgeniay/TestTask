using DG.Tweening;
using UnityEngine;

namespace CodeBase.IteractableObjects
{
    public interface IGrabHolder
    {
        public Vector3 GrabPosition { get; }
        public float MoveDuration { get; }
        public Ease MoveAnimation { get; }
    }
}
