using System;
using UnityEngine;

namespace Game
{
    public class ISkill : MonoBehaviour
    {
        public virtual event Action<IDamageble> OnHit;
    }
}