using UnityEngine;

namespace HzrController {
    public class BowController : MonoBehaviour, IWeapon
    {
        [field: SerializeField] public WeaponScriptableObject weapon { get; set; }
        [field: SerializeField] public Animator animator { get; set; }
        [field: SerializeField] public Vector3 gripPosition { get; set; }
        [field: SerializeField] public Vector3 gripRotation { get; set; }

        [SerializeField] private Transform _handGrip;
        [SerializeField] private Transform _arrow;
        [SerializeField] private float _projectileForce = 2500.0f;
        private Animator _bowAnimator;
        private Transform _createdArrow;

        private void Start()
        {
            _bowAnimator = GetComponent<Animator>();
        }

        private void Update()
        {
            ShootProjectile();
        }

        public void Attack()
        {
            if (!PlayerWeaponAnimationController.CanCombo)
            {
                if (animator.GetCurrentAnimatorStateInfo(1).IsName("Empty"))
                {
                    PlayerWeaponAnimationController.CanCombo = true;
                }

                return;
            }
            PlayerWeaponAnimationController.CanCombo = false;

            _createdArrow = Instantiate(_arrow, _handGrip.position, Quaternion.Euler(_handGrip.rotation.x, -_handGrip.rotation.y, _handGrip.rotation.z), _handGrip);
            if (animator.GetCurrentAnimatorStateInfo(1).IsName("Empty"))
            {
                animator.Play("BowShoot", 1, 0.02f);
                _bowAnimator.Play("Shoot", 0, 0.02f);
            }
            else if (animator.GetCurrentAnimatorStateInfo(1).IsName("BowShoot"))
            {
                animator.Play("BowShoot", 1, 0.02f);
                _bowAnimator.Play("Shoot", 0, 0.02f);
            }
        }

        public void ShootProjectile()
        {
            if (!PlayerWeaponAnimationController.CanShoot) return;

            Rigidbody rb = _createdArrow.GetComponent<Rigidbody>();
            ProjectileInfo arrowInfo = _createdArrow.GetComponent<ProjectileInfo>();
            arrowInfo.damage = weapon.damage;

            _createdArrow.SetParent(null);

            rb.isKinematic = false;
            rb.AddForce(transform.forward * _projectileForce * Time.deltaTime, ForceMode.Impulse);

            _createdArrow = null;
            PlayerWeaponAnimationController.CanShoot = false;
        }
    }
}
