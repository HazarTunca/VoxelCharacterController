using UnityEngine;

namespace PBM
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
        protected float _gravity = -9.81f;
        protected float _verticalVelocity;
        private float _terminalVelocity = 50.0f;

        // Acceleration tilt
        private Vector3 _vel;
        private Vector3 _lastVel;
        private Vector3 _acc;

        protected virtual void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        protected virtual void Update()
        {
            CheckGround();

            ApplyGravity();
            CalculateAcc();
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

        protected virtual void Die()
        {
            Debug.Log("you ded!");
        }

        private void CalculateAcc()
        {
            _vel = transform.InverseTransformVector(_controller.velocity);
            _acc = (_vel - _lastVel) / Time.fixedDeltaTime;
            _lastVel = _vel;

            if (Mathf.Abs(_acc.x) < _threshold && Mathf.Abs(_acc.z) < _threshold) _acc = Vector3.zero;
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