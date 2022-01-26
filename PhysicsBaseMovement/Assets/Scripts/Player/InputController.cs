using UnityEngine;

namespace PBM
{
    public class InputController : MonoBehaviour
    {
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;

        public bool mouseLock = true;

        private void Start()
        {
            LockMouseToggle();
        }

        private void Update()
        {
            // movement
            float keyboard_x = Input.GetAxisRaw("Horizontal");
            float keyboard_y = Input.GetAxisRaw("Vertical");
            move = new Vector2(keyboard_x, keyboard_y).normalized;

            // look
            float mouse_x = Input.GetAxis("Mouse X");
            float mouse_y = Input.GetAxis("Mouse Y");
            look = new Vector2(mouse_x, mouse_y);

            if (Input.GetKeyDown(KeyCode.Space)) jump = true;

            if (Input.GetKey(KeyCode.LeftShift)) sprint = true;
            else sprint = false;
        }

        public void LockMouseToggle()
        {
            if (mouseLock)
            {
                Cursor.lockState = CursorLockMode.Locked;
                return;
            }

            Cursor.lockState = CursorLockMode.None;
        }
    }
}