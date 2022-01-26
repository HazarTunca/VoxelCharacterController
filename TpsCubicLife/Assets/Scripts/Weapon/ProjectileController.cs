using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float damage;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collide with: " + collision.transform.name);

        // if detects enemy or smt give damage and destroy
        Destroy(gameObject, 3.5f);
    }
}
