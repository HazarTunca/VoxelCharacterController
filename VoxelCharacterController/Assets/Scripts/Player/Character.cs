using UnityEngine;

namespace HzrController
{
    [RequireComponent(typeof(CharacterController))]
    public abstract class Character : MonoBehaviour
    {
        [Header("Character")]
        [SerializeField] protected CharacterController _controller;
        [SerializeField] protected float _health;
        [SerializeField] protected float _walkSpeed;
        [SerializeField] protected float _sprintSpeed;
        [SerializeField] protected float _jumpHeight;

        [Header("Check Ground")] [Space(10)]
        [SerializeField] protected bool _isGrounded;
        [SerializeField] protected float _distanceToGround;
        [SerializeField] protected float _groundCheckRadius = 0.3f;
        [SerializeField] protected float _groundCheckPosOffset;
        [SerializeField] protected LayerMask _groundLayers;
        protected RaycastHit _hit;

        protected const float _threshold = 0.1f;

        // gravity
        protected float _gravity = -24.0f;
        protected float _verticalVelocity;
        private float _terminalVelocity = 50.0f;

        protected virtual void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        protected virtual void Update()
        {
            CheckGround();
            ApplyGravity();
        }

        protected void CheckGround()
        {
            Vector3 groundCheckPos = new Vector3(transform.position.x, transform.position.y - _groundCheckPosOffset, transform.position.z);
            _isGrounded = Physics.CheckSphere(groundCheckPos, _groundCheckRadius, _groundLayers, QueryTriggerInteraction.Ignore);
        }

        private void ApplyGravity()
        {
            if (_isGrounded & _verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += _gravity * Time.deltaTime;
            }
        }

        public void ApplyDamage(float amount)
        {
            _health -= amount;
            if (_health <= 0.0f) Die();
        }

        protected virtual void Die()
        {
            Debug.Log("you ded!");
        }


// debug
        private void OnDrawGizmosSelected()
        {
            Vector3 groundCheckPos = new Vector3(transform.position.x, transform.position.y - _groundCheckPosOffset, transform.position.z);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(groundCheckPos, _groundCheckRadius);
        }
    }
}