using UnityEngine;

namespace PBM
{
    [RequireComponent(typeof(InputController))]
    public class PlayerController : Character
    {
        [Header("Camera")]
        [Space(10)]
        [SerializeField] private Transform _cam;

        [Header("Movement Smooth Times")]
        [Space(10)]
        [SerializeField] private float _speedStartSmoothTime = 10.0f;
        [SerializeField] private float _speedStopSmoothTime = 5.0f;
        [SerializeField] private float _speedAirborneSmoothTime = 0.5f;
        [SerializeField] private float _whileRotationSmoothTime = 0.125f;
        [SerializeField] private float _startRotationSmoothTime = 0.025f;

        // others
        private InputController _input;
        private float _speed;
        private float _targetRotation;
        private float _rotationVel;
        private float _rotationSmoothTime;
        private float _rotation;

        protected override void Awake()
        {
            base.Awake();

            _input = GetComponent<InputController>();
        }

        protected override void Update()
        {
            base.Update();

            Move();
            Jump();
            RotatePlayer();
        }

        private void Move()
        {
            float targetSpeed = 0.0f;
            if (_input.move != Vector2.zero) targetSpeed = _input.sprint ? _sprintSpeed : _walkSpeed;

            // start moving and stop moving speeds
            if (_input.move != Vector2.zero && _isGrounded) _speed = Mathf.Lerp(_speed, targetSpeed, _speedStartSmoothTime * Time.deltaTime);
            else if (_input.move == Vector2.zero && _isGrounded) _speed = Mathf.Lerp(_speed, targetSpeed, _speedStopSmoothTime * Time.deltaTime);
            else if (!_isGrounded) _speed = Mathf.Lerp(_speed, targetSpeed, _speedAirborneSmoothTime * Time.deltaTime);

            Vector3 moveDir = transform.forward * _speed;

            // move
            _controller.Move(new Vector3(moveDir.x, _controller.velocity.y, moveDir.z) * Time.deltaTime + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        }

        private void RotatePlayer()
        {
            if (_input.move != Vector2.zero)
            {
                // rotation smooth time calculation
                Vector3 vel = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z);
                if (vel.magnitude < _threshold) _rotationSmoothTime = _startRotationSmoothTime;
                else _rotationSmoothTime = Mathf.Lerp(_rotationSmoothTime, _whileRotationSmoothTime, 4.0f * Time.deltaTime);

                // calculate rotation
                _targetRotation = Mathf.Atan2(_input.move.x, _input.move.y) * Mathf.Rad2Deg + _cam.transform.eulerAngles.y;
                _rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVel, _rotationSmoothTime);

                // rotate player
                transform.rotation = Quaternion.Euler(0.0f, _rotation, 0.0f);
            }
            else
            {
                // decrease smooth time
                _rotationSmoothTime = Mathf.Lerp(_rotationSmoothTime, _startRotationSmoothTime, 8.0f * Time.deltaTime);
            }
        }

        private void Jump()
        {
            if (!_isGrounded)
            {
                _input.jump = false;
                return;
            }

            if (_input.jump)
            {
                _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity * Time.deltaTime);
            }
        }
    }
}