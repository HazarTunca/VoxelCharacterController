using UnityEngine;

namespace HzrController
{
	public enum EWeaponType
    {
		knife,
		sword,
		bow,
    }

	[RequireComponent(typeof(InputController))]
	public class WeaponController : MonoBehaviour
	{
        [SerializeField] private Transform _handGrip;
        [SerializeField] private float _throwForce = 25.0f;
        private Transform _weapon;
        private Rigidbody _weaponRb;
        private Collider _weaponCol;
        private IWeapon _weaponInfo;

        [Header("Animation")]
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimatorOverrideController _normalAnimatorOC;
        [SerializeField] private AnimatorOverrideController _knifeAnimatorOC;
        [SerializeField] private AnimatorOverrideController _swordAnimatorOC;
        [SerializeField] private AnimatorOverrideController _bowAnimatorOC;

        private InputController _input;

        private void Awake()
        {
            _input = GetComponent<InputController>();
        }

        private void Update()
        {
            if (_input.drop)
            {
                DropWeapon();
                _input.drop = false;
            }

            if (_input.attack)
            {
                _weaponInfo?.Attack();
                _input.attack = false;
            }
        }

        public void WearWeapon(Transform weapon)
        {
            if (_weapon != null) DropWeapon();

            _weapon = weapon;
            _weaponInfo = _weapon.GetComponent<IWeapon>();

            // parenting
            _weapon.SetParent(_handGrip);
            _weapon.localPosition = _weaponInfo.gripPosition;
            _weapon.localEulerAngles = _weaponInfo.gripRotation;

            // disable physics
            _weaponRb = _weapon.GetComponent<Rigidbody>();
            _weaponRb.isKinematic = true;

            // enable trigger
            _weaponCol = _weapon.GetComponent<Collider>();
            _weaponCol.isTrigger = true;

            SetAnimatorOC();
        }

		private void DropWeapon()
        {
            if (_weapon == null) return;

            // unparenting
            _weapon.SetParent(null);
            _weaponInfo = null;

            // disable trigger
            _weaponCol.isTrigger = false;
            _weaponCol = null;

            // throw
            _weaponRb.isKinematic = false;
            Vector3 throwDir = transform.forward + Vector3.up;
            _weaponRb.AddForce(throwDir * _throwForce * Time.deltaTime, ForceMode.Impulse);
            _weaponRb.AddTorque(_weapon.right * _throwForce * Time.deltaTime, ForceMode.Impulse);
            _weaponRb = null;

            _weapon = null;

            SetAnimatorOC();
        }

        private void SetAnimatorOC()
        {
            AnimatorOverrideController newAnimatorOC = _normalAnimatorOC;
            if (_weaponInfo == null)
            {
                _animator.runtimeAnimatorController = newAnimatorOC;
                return;
            }

            EWeaponType weaponType = _weaponInfo.weapon.eWeaponType;
            switch (weaponType)
            {
                case EWeaponType.knife:
                    newAnimatorOC = _knifeAnimatorOC;
                    break;
                case EWeaponType.sword:
                    newAnimatorOC = _swordAnimatorOC;
                    break;
                case EWeaponType.bow:
                    newAnimatorOC = _bowAnimatorOC;
                    break;
                default:
                    newAnimatorOC = _normalAnimatorOC;
                    break;
            }

            _animator.runtimeAnimatorController = newAnimatorOC;
        }
	}
}
