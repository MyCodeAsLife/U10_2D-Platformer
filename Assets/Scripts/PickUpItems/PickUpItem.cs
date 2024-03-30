using System;
using UnityEngine;

namespace Game
{
    public abstract class PickUpItem : MonoBehaviour, IInteractive
    {
        public abstract event Action PickedUp;
    }
}