using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour, IWeapon, IMeleeWeapon
{
    [field: SerializeField] public EMeleeWeapon eMeleeWeapon { get; set; }
    [field: SerializeField] public float Damage { get; set; }

    public WeaponController weaponController { get; set; }
    public Animator animator { get; set; }

    public void Attack()
    {
        if (animator.GetCurrentAnimatorStateInfo(1).IsName("SwordAttack_1"))
        {
            animator.SetBool("canCombo", true);
        }
        else if (animator.GetCurrentAnimatorStateInfo(1).IsName("SwordAttack_2"))
        {
            animator.SetBool("canCombo", true);
        }
        else
        {
            animator.Play("SwordAttack_1", 1, 0.02f);
            animator.SetBool("canCombo", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (weaponController == null) return;
        if (!weaponController.canCollide) return;

        Debug.Log("collide with: " + other.name);
    }
}
