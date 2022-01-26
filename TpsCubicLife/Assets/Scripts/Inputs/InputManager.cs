using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    [HideInInspector] public Vector2 move;
    [HideInInspector] public Vector2 look;
    [HideInInspector] public bool jump;

    public bool lockMouse = true;

    private void Start()
    {
        LockMouse();
    }

    public void LockMouse()
    {
        if (lockMouse)
        {
            Cursor.lockState = CursorLockMode.Locked;
            return;
        }

        Cursor.lockState = CursorLockMode.None;
    }

    public void OnMove(InputValue value) => MoveAction(value.Get<Vector2>());
    public void OnLook(InputValue value) => LookAction(value.Get<Vector2>());
    public void OnJump(InputValue value) => JumpAction(value.isPressed);

    private void MoveAction(Vector2 newValue) => move = newValue;
    private void LookAction(Vector2 newValue) => look = newValue;
    private void JumpAction(bool newValue) => jump = newValue;
}
