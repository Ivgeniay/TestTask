using UnityEngine;

namespace Units.Player
{
    internal interface IControlable {
        public void Move(Vector2 moveDirection);
        public void ViewRotate(Vector2 moveDelta);
    }
}
