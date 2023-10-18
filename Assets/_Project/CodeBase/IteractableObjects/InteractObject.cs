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
            numberText.text = Key.ToString();
        }
    }
}
