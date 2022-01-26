using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILongRangedWeapon
{
    public ELongRangedWeapon eLongRangedWeapon { get; set; }
    public Transform Projectile { get; set; }
    public Transform ProjectileGrip { get; set; }
    public float ShootForce { get; set; }
    void Shoot();
}
