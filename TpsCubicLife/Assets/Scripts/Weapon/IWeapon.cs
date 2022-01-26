using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public WeaponController weaponController { get; set; }
    public Animator animator { get; set; }
    public float Damage { get; set; }
    public void Attack();
}
