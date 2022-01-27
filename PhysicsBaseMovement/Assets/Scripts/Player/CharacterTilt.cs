using UnityEngine;

namespace HzrController {
	public class CharacterTilt : MonoBehaviour
	{
        [SerializeField] private InputController _input;
        [SerializeField] private float _maxTiltAngle = 15.0f;
        [SerializeField] private float _tiltSmoothTime = 2.5f;
        private Vector3 euler;

        private void Update()
        {
            TiltCharacter();
        }
        
        private void TiltCharacter()
        {
            float horizontal = _input.look.x;

            if(_input.move != Vector2.zero)
            {
                euler = Vector3.Lerp(euler, new Vector3(0.0f, 0.0f, horizontal * -_maxTiltAngle), _tiltSmoothTime * Time.deltaTime);
            }
            else
            {
                euler = Vector3.Lerp(euler, Vector3.zero, _tiltSmoothTime * Time.deltaTime);
            }

            transform.localEulerAngles = euler;
        }
    }
}
