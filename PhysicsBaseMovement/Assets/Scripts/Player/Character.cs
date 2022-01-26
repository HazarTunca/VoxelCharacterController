using UnityEngine;

namespace PBM
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Character : MonoBehaviour
    {
        [Header("Character")]
        [SerializeField] protected Rigidbody _rb;
        [SerializeField] protected float _health;
        [SerializeField] protected float _walkSpeed;
        [SerializeField] protected float _sprintSpeed;
        [SerializeField] protected float _jumpForce;

        [Header("Check Ground")]
        [Space(10)]
        [SerializeField] protected bool _isGrounded;
        [SerializeField] protected float _distanceToGround;
        [SerializeField] protected float _groundCheckRadius = 0.3f;
        [SerializeField] protected float _groundCheckPosOffset;
        [SerializeField] protected LayerMask _groundLayers;
        protected RaycastHit _hit;
        protected Vector3 _groundCheckPos;

        protected const float _threshold = 0.1f;

        // Acceleration tilt
        private Vector3 _vel;
        private Vector3 _lastVel;
        private Vector3 _acc;

        protected virtual void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        protected virtual void FixedUpdate()
        {
            CheckGround();
            CalculateDistanceToGround();
            CalculateAcc();
            TiltCharacter();
        }

        protected void CheckGround()
        {
            _groundCheckPos = new Vector3(transform.position.x, transform.position.y + _groundCheckPosOffset, transform.position.z);
            _isGrounded = Physics.CheckSphere(_groundCheckPos, _groundCheckRadius, _groundLayers, QueryTriggerInteraction.Ignore);
        }

        protected void CalculateDistanceToGround()
        {
            if (_isGrounded)
            {
                _distanceToGround = 0.0f;
                return;
            }
            Physics.Raycast(_groundCheckPos, -transform.up, out _hit);

            if (_hit.transform != null) _distanceToGround = Vector3.Distance(_groundCheckPos, _hit.point);
        }

        protected virtual void Die()
        {
            Debug.Log("you ded!");
        }

        private void TiltCharacter()
        {
            float zRot = _acc.x;
            float xRot = _acc.z;

            _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, Quaternion.Euler(_rb.rotation.x, _rb.rotation.y, zRot), 8.0f));
        }

        private void CalculateAcc()
        {
            _vel = transform.InverseTransformVector(_rb.velocity);
            _acc = (_vel - _lastVel) / Time.fixedDeltaTime;
            _lastVel = _vel;

            if (Mathf.Abs(_acc.x) < _threshold && Mathf.Abs(_acc.z) < _threshold) _acc = Vector3.zero;
        }
        // debug
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_groundCheckPos, _groundCheckRadius);
        }
    }
}