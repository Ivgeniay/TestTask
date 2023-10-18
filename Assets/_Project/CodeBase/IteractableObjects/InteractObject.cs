using TMPro;
using UnityEngine;

namespace CodeBase.IteractableObjects
{
    public class InteractObject : MonoBehaviour
    {
        [field: SerializeField] internal int Key { get; private set; }
        [SerializeField] private TextMeshPro numberText;

        protected virtual void Awake()
        {
            if (!numberText) throw new System.Exception($"Interaction's UI element is null");
            else numberText.text = Key.ToString();
        }
    }
}
