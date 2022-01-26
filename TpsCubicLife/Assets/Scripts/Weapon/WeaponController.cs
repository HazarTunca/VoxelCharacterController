using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EWeaponType
{
    none,
    melee,
    longRange
}
public enum EMeleeWeapon
{
    knife,
    sword
}
public enum ELongRangedWeapon
{
    bow
}

[RequireComponent(typeof(Animator))]
public class WeaponController : MonoBehaviour
{
    [Header("Weapon info")]
    [SerializeField] private Transform _handGrip;
    [SerializeField] private float _throwForce = 15.0f;

    [Space(10)]
    [SerializeField] private Vector3 _bowRotForGrip;
    [SerializeField] private Vector3 _bowPosForGrip;

    [Space(10)]
    [SerializeField] private Vector3 _swordRotForGrip;
    [SerializeField] private Vector3 _swordPosForGrip;

    [Space(10)]
    [SerializeField] private Vector3 _knifeRotForGrip;
    [SerializeField] private Vector3 _knifePosForGrip;

    // weapon info
    private Transform _holdingWeapon;
    private Transform _takeableWeapon;
    private IWeapon _weaponScript;

    [Header("Animations")] [Space(10)]
    [SerializeField] private AnimatorOverrideController _normalAOC;
    [SerializeField] private AnimatorOverrideController _swordAOC;
    [SerializeField] private AnimatorOverrideController _knifeAOC;
    [SerializeField] private AnimatorOverrideController _bowAOC;
    private Animator _animator;
    private int _canComboAnim;

    // attack
    [Header("Attack")] [Space(10)]
    public bool canAttack = true;
    public bool canCollide;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        _canComboAnim = Animator.StringToHash("canCombo");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeWeapon();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropWeapon();
        }

        if (Input.GetMouseButtonDown(0)) Attack();
    }

    #region animation

    private void Attack()
    {
        if (_holdingWeapon == null || _weaponScript == null) return;
        if (!canAttack) return;

        canAttack = false;
        _weaponScript.Attack();
    }

    public void EnterAttackState() => _animator.SetBool(_canComboAnim, false);
    public void EnableCombo() => canAttack = true;
    public void DisableCombo() => canAttack = false;

    // give damage
    public void EnableMeleeCollision() => canCollide = true;
    public void DisableMeleeCollision() => canCollide = false;
    public void Shoot()
    {
        if (_holdingWeapon == null) return;

        _holdingWeapon.GetComponent<ILongRangedWeapon>().Shoot();
    }

    private void SetAnimations()
    {
        AnimatorOverrideController newAOC = _normalAOC;
        if (_holdingWeapon != null)
        {
            // is melee
            IMeleeWeapon melee = null;
            _holdingWeapon.TryGetComponent(out melee);

            ILongRangedWeapon longRanged = null;
            _holdingWeapon.TryGetComponent(out longRanged);

            if (melee != null)
            {
                switch (melee.eMeleeWeapon)
                {
                    case EMeleeWeapon.knife:
                        newAOC = _knifeAOC;
                        break;
                    case EMeleeWeapon.sword:
                        newAOC = _swordAOC;
                        break;
                }
            }
            else if(longRanged != null)
            {
                switch (longRanged.eLongRangedWeapon)
                {
                    case ELongRangedWeapon.bow:
                        newAOC = _bowAOC;
                        break;
                }
            }
        }
        _animator.runtimeAnimatorController = newAOC;
    }

    #endregion

    private void TakeWeapon()
    {
        if (_takeableWeapon == null) return;
        if (_holdingWeapon != null) DropWeapon();

        _holdingWeapon = _takeableWeapon;

        // set the variables
        _weaponScript = _holdingWeapon.GetComponent<IWeapon>();
        _weaponScript.animator = _animator;
        _weaponScript.weaponController = this;

        // rigidbody
        Rigidbody rb = _holdingWeapon.GetComponent<Rigidbody>();
        rb.isKinematic = true;

        // Set parent
        _holdingWeapon.SetParent(_handGrip);
        _holdingWeapon.localPosition = Vector3.zero;
        _holdingWeapon.localRotation = Quaternion.identity;
        SetHoldingWeaponRotPos();

        // set collider
        _holdingWeapon.GetComponent<Collider>().isTrigger = true;

        SetAnimations();
        _takeableWeapon = null;
    }

    public void DropWeapon()
    {
        if (_holdingWeapon == null) return;

        // Reset variables
        _weaponScript.animator = _animator;
        _weaponScript.weaponController = this;

        // rigidbody
        Rigidbody rb = _holdingWeapon.GetComponent<Rigidbody>();
        rb.isKinematic = false;

        // Set parent
        _holdingWeapon.SetParent(null);
        

        // set collider
        _holdingWeapon.GetComponent<Collider>().isTrigger = false;

        // add force
        rb.AddForce(transform.forward * _throwForce, ForceMode.Impulse);

        _holdingWeapon = null;
        _weaponScript = null;
        SetAnimations();
    }

    private void SetHoldingWeaponRotPos()
    {
        if (_holdingWeapon == null) return;

        IMeleeWeapon melee = null;
        _holdingWeapon.TryGetComponent(out melee);

        ILongRangedWeapon longRanged = null;
        _holdingWeapon.TryGetComponent(out longRanged);

        if(melee != null)
        {
            switch (melee.eMeleeWeapon)
            {
                case EMeleeWeapon.knife:
                    _holdingWeapon.localPosition = _knifePosForGrip;
                    _holdingWeapon.localRotation = Quaternion.Euler(_knifeRotForGrip);
                    break;
                case EMeleeWeapon.sword:
                    _holdingWeapon.localPosition = _swordPosForGrip;
                    _holdingWeapon.localRotation = Quaternion.Euler(_swordRotForGrip);
                    break;
            }
        }
        else if(longRanged != null)
        {
            switch (longRanged.eLongRangedWeapon)
            {
                case ELongRangedWeapon.bow:
                    _holdingWeapon.localPosition = _bowPosForGrip;
                    _holdingWeapon.localRotation = Quaternion.Euler(_bowRotForGrip);
                    break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon") && other.transform != _holdingWeapon)
        {
            _takeableWeapon = other.transform;
            return;
        }

        _takeableWeapon = null;
    }
    private void OnTriggerExit(Collider other) => _takeableWeapon = null;
}
