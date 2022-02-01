using UnityEngine;

namespace HzrController
{
    [CreateAssetMenu(fileName = "NewWeapon", menuName = "New Weapon")]
    public class WeaponScriptableObject : ScriptableObject
    {
        [Header("Common Info")]
        public string weaponName;
        public float damage;
        public EWeaponType eWeaponType;
    }
}
