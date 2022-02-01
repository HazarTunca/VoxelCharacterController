using UnityEngine;

namespace HzrController
{
    public interface IWeapon
    {
        public WeaponScriptableObject weapon { get; set; }
        public Animator animator { get; set; }
        public Vector3 gripPosition { get; set; }
        public Vector3 gripRotation { get; set; }
        public void Attack();
    }
}
