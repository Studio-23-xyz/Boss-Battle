using System;
using com.gdcbd.bossbattle.utility;
using UnityEngine;
using UnityEngine.InputSystem;

namespace com.gdcbd.bossbattle
{
    public class InputManager : PersistentMonoSingleton<InputManager>
    {
        [SerializeField] private PlayerInput playerInput;


        private Vector2 _moveInput;

        public delegate void OnActionEvent();

        public OnActionEvent MovePressedAction;
        public OnActionEvent MoveReleaseAction;

        public OnActionEvent JumpPressedAction;
        public OnActionEvent JumpReleaseAction;

        public OnActionEvent SprintPressedAction;
        public OnActionEvent SprintReleasedAction;

        public OnActionEvent FirePressedAction;
        public OnActionEvent FireReleasedAction;

        public OnActionEvent DashPressedAction;
        public OnActionEvent DashReleasedAction;

        public OnActionEvent CrouchPressedAction;
        public OnActionEvent CrouchReleasedAction;

        protected override void Initialize()
        {
        }

        public void GetMovementInput(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
            OnMovingAction(context);
        }

        private void OnMovingAction(InputAction.CallbackContext context)
        {
            if (context.performed)
                MovePressedAction?.Invoke();
            else if (context.canceled)
                MoveReleaseAction?.Invoke();
        }

        public Vector2 Move()
        {
            return _moveInput;
        }

        public void OnJumpAction(InputAction.CallbackContext context)
        {
            if (context.performed)
                JumpPressedAction?.Invoke();
            else if (context.canceled)
                JumpReleaseAction?.Invoke();
        }

        public void OnSprintAction(InputAction.CallbackContext context)
        {
            if (context.performed)
                SprintPressedAction?.Invoke();
            else if (context.canceled)
                SprintReleasedAction?.Invoke();
        }

        public void OnFireAction(InputAction.CallbackContext context)
        {
            if (context.performed)
                FirePressedAction?.Invoke();
            else if (context.canceled)
                FireReleasedAction?.Invoke();
        }

        public void OnDashAction(InputAction.CallbackContext context)
        {
            if (context.performed)
                DashPressedAction?.Invoke();
            else if (context.canceled)
                DashReleasedAction?.Invoke();
        }

        public void OnCrouchAction(InputAction.CallbackContext context)
        {
            if (context.performed)
                CrouchPressedAction?.Invoke();
            else if (context.canceled)
                CrouchReleasedAction?.Invoke();
        }
    }
}