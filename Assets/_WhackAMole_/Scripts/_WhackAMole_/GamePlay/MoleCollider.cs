using System;
using UnityEngine;

namespace WhackAMole
{
    public class MoleCollider : MonoBehaviour 
    {
        public event Action OnHammerEnter;
        public bool DetectCollision;
        
        [SerializeField] private Collider _collider;

        private void OnTriggerEnter(Collider other)
        {
            if(DetectCollision && other.gameObject.layer == (int)Layers.Hammer)
            {
                DetectCollision = false;
                OnHammerEnter?.Invoke();
            }
        }
    }
}