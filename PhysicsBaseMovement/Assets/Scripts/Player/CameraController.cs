using UnityEngine;

namespace HzrController
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private InputController _input;
        [SerializeField] private Transform _target;

        [Header("Camera Follow")]
        [Space(10)]
        [SerializeField] private float _followSmoothTime = 3.5f;
        [SerializeField] private Vector3 _lookOffset = new Vector3(0.0f, 2.0f, -6.5f);

        [Header("Camera Rotate")]
        [Space(10)]
        [SerializeField] private float _rotateSmoothTime = 8.0f;
        [SerializeField] private float _sensitivity = 2.0f;
        [SerializeField] private float _topClamp = 80.0f;
        [SerializeField] private float _bottomClamp = -60.0f;

        private float _x;
        private float _y;
        private Vector3 _vel;

        private void Start()
        {
            SetCameraFixedPosition();
        }

        private void LateUpdate()
        {
            MoveCamera();
            RotateCamera();
        }

        private void MoveCamera()
        {
            transform.position = Vector3.SmoothDamp(transform.position, _target.position, ref _vel, _followSmoothTime * Time.fixedDeltaTime);
        }

        private void RotateCamera()
        {
            _x += _input.look.x * _sensitivity;
            _y -= _input.look.y * _sensitivity;

            _x = ClampAngle(_x, float.MinValue, float.MaxValue);
            _y = ClampAngle(_y, _bottomClamp, _topClamp);

            Quaternion wantedRot = Quaternion.Euler(_y, _x, 0.0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, wantedRot, _rotateSmoothTime * Time.deltaTime);
        }

        private void SetCameraFixedPosition()
        {
            Transform cam = transform.GetChild(0);
            cam.localPosition = _lookOffset;
        }

        private float ClampAngle(float x, float min, float max)
        {
            if (x > 360) x -= 360;
            else if (x < -360) x += 360;
            return Mathf.Clamp(x, min, max);
        }
    }
}
