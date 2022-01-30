using UnityEngine;

namespace HzrController {
	public class PlayerTilt : MonoBehaviour
	{
        [Header("Reqirements")]
        [SerializeField] private InputController _input;
        [SerializeField] private PlayerController _playerController;

        [Header("Tilting")]
        [SerializeField] private float _maxTiltAngle = 20.0f;
        [SerializeField] private float _minTiltAngle = 1.5f;
        [SerializeField] private float _tiltDivider = 2.5f;
        [SerializeField] private float _tiltSmoothTime = 10.0f;

        // tilt privates
        private Vector3 _euler;
        private float _turnAngle;
        private float _tiltAngle;

        private const float _threshold = 0.1f;

        private void Update()
        {
            TiltCharacter();
        }
        
        private void TiltCharacter()
        {
            // Calculate turn angle
            _turnAngle = Vector3.SignedAngle(transform.forward, _playerController.moveDir, Vector3.up);
            float angle = -_turnAngle;
            if (_turnAngle <= _threshold && _turnAngle >= -_threshold) angle = 0.0f;

            if (Mathf.Abs(angle) > _minTiltAngle) _tiltAngle = angle;
            else _tiltAngle = 0.0f;

            if (_tiltAngle > _maxTiltAngle) _tiltAngle = _maxTiltAngle;
            else if (_tiltAngle < -_maxTiltAngle) _tiltAngle = -_maxTiltAngle;

            // rotate character
            if (_input.move != Vector2.zero)
            {
                _euler = Vector3.Lerp(_euler, new Vector3(0.0f, 0.0f, _tiltAngle / _tiltDivider), _tiltSmoothTime * Time.deltaTime);
            }
            else
            {
                _euler = Vector3.Lerp(_euler, Vector3.zero, _tiltSmoothTime * Time.deltaTime);
            }

            transform.localEulerAngles = _euler;
        }
    }
}
