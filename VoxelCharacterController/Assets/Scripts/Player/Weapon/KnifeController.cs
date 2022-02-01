using UnityEngine;

namespace HzrController {
    public class KnifeController : MonoBehaviour, IWeapon
    {
        [field: SerializeField] public WeaponScriptableObject weapon { get; set; }
        [field: SerializeField] public Animator animator { get; set; }
        [field: SerializeField] public Vector3 gripPosition { get; set; }
        [field: SerializeField] public Vector3 gripRotation { get; set; }

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

            if (animator.GetCurrentAnimatorStateInfo(1).IsName("Empty"))
            {
                animator.Play("KnifeAttack_1", 1, 0.02f);
            }
            else if (animator.GetCurrentAnimatorStateInfo(1).IsName("KnifeAttack_1"))
            {
                animator.Play("KnifeAttack_2", 1, 0.04f);
            }
            else if (animator.GetCurrentAnimatorStateInfo(1).IsName("KnifeAttack_2"))
            {
                animator.Play("KnifeAttack_1", 1, 0.04f);
            }
        }
    }
}
