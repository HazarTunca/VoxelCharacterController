using UnityEngine;

namespace PBM
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

        // others
        private InputController _input;
        Vector3 _smoothDir;
        private float _speed;
        private float _targetDirection;

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

            // calculate rotation
            if (_input.move != Vector2.zero)
            {
                _targetDirection = Mathf.Atan2(_input.move.x, _input.move.y) * Mathf.Rad2Deg + _cam.eulerAngles.y;
            }

            //Vector3 moveDir = transform.forward * _speed;
            Vector3 movedir = Quaternion.Euler(0.0f, _targetDirection, 0.0f) * Vector3.forward;

            // check for direction !!!!!!!!!!!!!!!!!!!!!!!!!! do something with it
            if (_controller.velocity.magnitude > _threshold)
                _smoothDir = Vector3.Lerp(_smoothDir, movedir, _directionSmoothTime * Time.deltaTime);
            else
                _smoothDir = movedir;

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
                Quaternion smoothRotate = Quaternion.Lerp(transform.rotation, desiredRotation, _rotationSmoothTime);

                // rotate player
                transform.rotation = smoothRotate;
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
                _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
            }
        }
    }
}