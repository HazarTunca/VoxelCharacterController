using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowController : MonoBehaviour, IWeapon, ILongRangedWeapon
{
    [field: SerializeField] public ELongRangedWeapon eLongRangedWeapon { get; set; }
    [field: SerializeField] public float Damage { get; set; }
    [field: SerializeField] public float ShootForce { get; set; }

    [field: SerializeField] public Transform Projectile { get; set; }
    [field: SerializeField] public Transform ProjectileGrip { get; set; }

    public WeaponController weaponController { get; set; }
    public Animator animator { get; set; }

    public void Attack()
    {
        if (animator.GetCurrentAnimatorStateInfo(1).IsName("BowShoot"))
        {
            animator.Play("BowShootCombo", 1, 0.02f);
        }
        else if(animator.GetCurrentAnimatorStateInfo(1).IsName("BowShootCombo"))
        {
            animator.Play("BowShootCombo", 1, 0.02f);
        }
        else
        {
            animator.Play("BowShoot", 1, 0.02f);
        }
    }

    public void Shoot()
    {
        // create projectile
        Transform arrow = Instantiate(Projectile, ProjectileGrip.position, ProjectileGrip.rotation);
        ProjectileController controller = arrow.gameObject.AddComponent<ProjectileController>();
        controller.damage = Damage;

        // shoot Projectile
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce(transform.forward * ShootForce, ForceMode.Impulse);
    }
}
