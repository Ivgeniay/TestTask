using Mirror;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Components.Effects
{
    internal class MaterialEffectComponent : BaseEffectComponent
    {
        [SerializeField] private Renderer[] renderers;
        [SerializeField] private Color newMaterialColor;
        private List<Color> colors = new List<Color>();

        private void Awake() {
            if (renderers.Length == 0) return;

            foreach (var renderer in renderers) 
                foreach (var material in renderer.materials) 
                    colors.Add(material.color);
        }

        public override void StartEffect() {
            OnEffectStarted?.Invoke();

            foreach (var renderer in renderers) 
                foreach (var material in renderer.materials) 
                    material.color = newMaterialColor;
        }

        public void StopEffect() {
            OnEffectEnded?.Invoke();

            int counter = 0;
            foreach (var renderer in renderers) 
                foreach (var material in renderer.materials) {
                    material.color = colors[counter];
                    counter += 1;
                }
            
        }
    }
}
