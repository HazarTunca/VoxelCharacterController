using UnityEngine;

namespace HzrController
{
    [RequireComponent(typeof(InputController))]
    public class PlayerController : Character
    {
        [Header("Camera")] [Space(10)]
        [SerializeField] private Transform _cam;

        [Header("Movement Smooth Times")] [Space(10)]
        [SerializeField] private float _directionSmoothTime = 2.5f;
        [SerializeField] private float _speedStartSmoothTime = 10.0f;
        [SerializeField] private float _speedStopSmoothTime = 5.0f;
        [SerializeField] private float _speedAirborneSmoothTime = 0.5f;
        [SerializeField] private float _rotationSmoothTime = 5.0f;
        [HideInInspector] public Vector3 moveDir;

        [Header("Animation")] [Space(10)]
        [SerializeField] private Animator _animator;

        private int _speedAnimVar;
        private int _isJumpingAnimVar;
        private int _isFallingAnimVar;

        // Movement
        private InputController _input;
        private Vector3 _smoothDir;
        private float _speed;
        private float _targetAngle;
        private float _lastPosY;
        private bool _isJumping;
        private bool _isFalling;

        protected override void Awake()
        {
            base.Awake();

            _input = GetComponent<InputController>();
        }

        private void Start()
        {
            _speedAnimVar = Animator.StringToHash("Speed");
            _isJumpingAnimVar = Animator.StringToHash("isJumping");
            _isFallingAnimVar = Animator.StringToHash("isFalling");
        }

        protected override void Update()
        {
            base.Update();

            RotatePlayer();
            Move();
            Jump();
            PlayBaseAnimations();
        }

        private void Move()
        {
            float targetSpeed = 0.0f;
            if (_input.move != Vector2.zero)
            {
                targetSpeed = _input.sprint ? _sprintSpeed : _walkSpeed;

                // calculate rotation
                _targetAngle = Mathf.Atan2(_input.move.x, _input.move.y) * Mathf.Rad2Deg + _cam.eulerAngles.y;
            }

            moveDir = Quaternion.Euler(0.0f, _targetAngle, 0.0f) * Vector3.forward;
            _smoothDir = Vector3.Lerp(_smoothDir, moveDir, _directionSmoothTime * Time.deltaTime);

            // start moving and stop moving speeds
            if (_isGrounded)
            {
                if (_input.move != Vector2.zero) _speed = Mathf.Lerp(_speed, targetSpeed, _speedStartSmoothTime * Time.deltaTime);
                else if (_input.move == Vector2.zero) _speed = Mathf.Lerp(_speed, targetSpeed, _speedStopSmoothTime * Time.deltaTime);
            }
            else
                _speed = Mathf.Lerp(_speed, targetSpeed, _speedAirborneSmoothTime * Time.deltaTime);

            // move
            _controller.Move(_smoothDir.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);            
        }

        private void RotatePlayer()
        {
            if (_controller.velocity.magnitude > _threshold)
            {
                // calculate rotating player towards velocity 
                Quaternion velocityRotation = Quaternion.LookRotation(_controller.velocity);
                Quaternion desiredRotation = Quaternion.Euler(0.0f, velocityRotation.eulerAngles.y, 0.0f);
                Quaternion smoothRotate = Quaternion.Lerp(transform.rotation, desiredRotation, _rotationSmoothTime * Time.deltaTime);

                // rotate player
                transform.rotation = smoothRotate;
            }
        }

        private void Jump()
        {
            if (!_isGrounded)
            {
                if (transform.position.y > _lastPosY) _isJumping = true;
                else _isFalling = true;

                _lastPosY = transform.position.y;

                _input.jump = false;
                return;
            }

            _isJumping = false;
            _isFalling = false;

            if (_input.jump)
            {
                _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
            }
        }

        private void PlayBaseAnimations()
        {
            _animator.SetFloat(_speedAnimVar, _speed);
            _animator.SetBool(_isJumpingAnimVar, _isJumping);
            _animator.SetBool(_isFallingAnimVar, _isFalling);
        }
    }
}