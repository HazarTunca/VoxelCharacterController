using UnityEngine;

namespace HzrController {
    public class SwordController : MonoBehaviour, IWeapon
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
                animator.Play("SwordAttack_1", 1, 0.02f);
            }
            else if (animator.GetCurrentAnimatorStateInfo(1).IsName("SwordAttack_1")){
                animator.Play("SwordAttack_2", 1, 0.02f);
            }
            else if (animator.GetCurrentAnimatorStateInfo(1).IsName("SwordAttack_2")){
                animator.Play("SwordAttack_3", 1, 0.02f);
            }
        }
    }
}
