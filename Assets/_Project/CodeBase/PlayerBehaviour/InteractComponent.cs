using CodeBase.IteractableObjects;
using DG.Tweening;
using System;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

namespace CodeBase.PlayerBehaviour
{
    internal class IteracteComponent : MonoBehaviour, IGrabHolder
    {
        public event Action<GrabbableObject> OnTakeGrabableEvent;

        [field: SerializeField] private Transform grabPosition; 
        [Space(10)]
        [Header("Interact")]
        [SerializeField] private LayerMask interacLayerMask; 

        [Header("Grab")]
        [SerializeField] private float grabDistance = 2.5f;
        [SerializeField] private float moveDuration = 0.15f;
        [SerializeField] private Ease moveAnimation = Ease.InElastic;
        [SerializeField] private float throwForce = 800;
        private GrabbableObject GrabbableObject { get; set; }
        private BuildInObject BuildInObject { get; set; }

        public Vector3 GrabPosition { get => grabPosition.localPosition; }
        public float MoveDuration { get => moveDuration; }
        public Ease MoveAnimation { get => moveAnimation; }


#region Mono
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!GrabbableObject && !BuildInObject) TryToGrab();
                else if (GrabbableObject && !BuildInObject) Throw();
                else if (GrabbableObject && BuildInObject) SetGrabToBuild();
            }

            if (GrabbableObject)
            {
                float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
                if (scrollWheel != 0)
                {
                    Quaternion deltaRotation = Quaternion.Euler(Vector3.up * scrollWheel * 5000 * Time.deltaTime);
                    var rb = GrabbableObject.GetRigidbody();
                    rb.MoveRotation(rb.rotation * deltaRotation);
                    //GrabbableObject.transform.rotation = GrabbableObject.transform.rotation * deltaRotation;
                }
            }

            SeekBuilInObject();
        }

#endregion
#region Grab
        private void TryToGrab()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, grabDistance, interacLayerMask))
            {
                var grabbable = hit.transform.GetComponent<GrabbableObject>();
                if (grabbable)
                    Grab(grabbable);
            }
        }

        private void Throw()
        {
            GrabbableObject?.transform.SetParent(null);
            GrabbableObject?.Grab(this, false);
            GrabbableObject.GetRigidbody().AddForce(Camera.main.transform.forward * throwForce);
            GrabbableObject = null;
        }

        private void Grab(GrabbableObject grabbableObject)
        {
            GrabbableObject = grabbableObject;
            GrabbableObject?.transform.SetParent(transform);
            GrabbableObject?.Grab(this, true);
            OnTakeGrabableEvent?.Invoke(grabbableObject);
        }
#endregion
#region BuildIn
        private void SeekBuilInObject()
        {
            if (!GrabbableObject)
            {
                if (BuildInObject) BuilIdIsSeeked(null);
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, grabDistance, interacLayerMask))
            {
                var builIn = hit.transform.GetComponent<BuildInObject>();
                if (builIn != null)
                {
                    BuilIdIsSeeked(builIn);
                }
            }
            else
            {
                BuilIdIsSeeked(null);
            }
        }

        private void BuilIdIsSeeked(BuildInObject buildInObject)
        {
            if (buildInObject == null)
            {
                if (!BuildInObject) return;
                BuildInObject.SetActive(ActiveType.Diactivate);
                BuildInObject = null;
                return;
            }

            if (!buildInObject.IsEmpty) return;
            if (buildInObject.contactType != GrabbableObject.GetType().Name) return;
            if (buildInObject.Key != GrabbableObject.Key) return;
            if (BuildInObject == buildInObject) return;

            BuildInObject?.SetActive(ActiveType.Diactivate);
            BuildInObject = buildInObject;
            BuildInObject.SetActive(ActiveType.Promt);
        } 

#endregion
#region SetUp
        private void SetGrabToBuild()
        {
            BuildInObject.SetUp(GrabbableObject);
            GrabbableObject = null;
        }

#endregion
    }
}
