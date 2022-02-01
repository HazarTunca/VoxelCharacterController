using UnityEngine;

namespace HzrController {
	[RequireComponent(typeof(InputController), typeof(WeaponController))]
	public class LootItem : MonoBehaviour
	{
		[SerializeField] private float _lootDistance = 5.0f;
        private Transform _cam;
		private RaycastHit _hit;

		private InputController _input;
		private WeaponController _weaponController;

        private void Awake()
        {
			_input = GetComponent<InputController>();
            _weaponController = GetComponent<WeaponController>();
            _cam = Camera.main.transform;
        }

        private void Update()
        {
            if (_input.loot)
            {
                CreateRay();
                _input.loot = false;
            }
        }

        private void CreateRay()
        {
            bool isHit = Physics.Raycast(_cam.position, _cam.forward, out _hit, _lootDistance);
            if (!isHit) return;

            Transform item = _hit.transform;
            if (item.CompareTag("Weapon")) _weaponController.WearWeapon(item);
        }
    }
}
