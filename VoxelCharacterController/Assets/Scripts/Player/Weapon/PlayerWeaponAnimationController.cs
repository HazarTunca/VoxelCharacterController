using UnityEngine;

namespace HzrController {
	public class PlayerWeaponAnimationController : MonoBehaviour
	{
        private static bool _canCombo = true;
        public static bool CanCombo { get => _canCombo; set => _canCombo = value; }

        private static bool _canShoot;
        public static bool CanShoot { get => _canShoot; set => _canShoot = value; }

        public void DisableComboEvent() => _canCombo = false;
        public void EnableComboEvent() => _canCombo = true;
        public void ShootProjectileEvent() => _canShoot = true;
    }
}
