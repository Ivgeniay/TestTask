using CodeBase.PlayerBehaviour;
using System.Collections;
using TMPro;
using UnityEngine;

namespace CodeBase.UI
{
    internal class HelpLable : MonoBehaviour
    {
        [SerializeField] private IteracteComponent interactComp;
        [SerializeField] private TextMeshProUGUI lable;

        [SerializeField] private float appearTime = 1.0f;
        [SerializeField] private float waitTime = 2.0f;
        [SerializeField] private float disappearTime = 1.0f;

        private bool isUsed = false;

        private void Awake()
        {
            interactComp.OnTakeGrabableEvent += OnTakeGrabableHandler;
        }

        private void OnTakeGrabableHandler(IteractableObjects.GrabbableObject obj)
        {
            if (!isUsed)
            {
                isUsed = true;
                StartCoroutine(AnimateText(appearTime, waitTime, disappearTime));
            }
        }

        private IEnumerator AnimateText(float appearTime, float waitTime, float disappearTime)
        {
            float startTime = Time.time;
            float endTime = startTime + appearTime;
            while (Time.time < endTime)
            {
                float progress = (Time.time - startTime) / appearTime;
                lable.alpha = Mathf.Lerp(0f, 1f, progress);
                yield return null;
            }

            yield return new WaitForSeconds(waitTime);
             
            startTime = Time.time;
            endTime = startTime + disappearTime;
            while (Time.time < endTime)
            {
                float progress = (Time.time - startTime) / disappearTime;
                lable.alpha = Mathf.Lerp(1f, 0f, progress);
                yield return null;
            }
             
            lable.alpha = 0f;
        }
    }
}
