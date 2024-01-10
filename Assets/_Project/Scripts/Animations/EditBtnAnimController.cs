using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Clock.Animations
{
    internal class EditBtnAnimController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Image image;

        private int OnHowerTr = Animator.StringToHash("OnHower");
        private int OnBtnDownTr = Animator.StringToHash("OnBtnDown");
        private int OnBtnUpTr = Animator.StringToHash("OnBtnUp");
        private int OnExitTr = Animator.StringToHash("OnExit");

        private bool isEditMode = false;

        public void OnPointerEnter(PointerEventData eventData)
        {
            animator.SetTrigger(OnHowerTr);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            animator.SetTrigger(OnExitTr);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            animator.SetTrigger(OnBtnDownTr); 
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            animator.SetTrigger(OnBtnUpTr);
        }

        public void OnClick()
        {
            isEditMode = !isEditMode;
            image.color = isEditMode == true ? new Color32(192, 247, 251, 255) : Color.white;
        }
    }
}
