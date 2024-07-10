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
        public delegate void OnActionMapEvent(InputMaps actionMap);

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
       
        public OnActionMapEvent OnActionMapChanged;
        private InputMaps _currentActionMap;
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
         

       
        
        #region UI_Settings
        public OnActionEvent OnHoldSubmitActionCompleted;
        public OnActionEvent OnHoldResetActionCompleted;
        public OnActionEvent OptionLeft;
        public OnActionEvent OptionRight;
        public OnActionEvent OptionBack;
        public OnActionEvent OptionSubmit;
        private float _sliderValue;
        public void OnLeftAction(InputAction.CallbackContext context)
        {
            if (context.performed)
                OptionLeft?.Invoke();
            
        }
        public void OnRightAction(InputAction.CallbackContext context)
        {
            if (context.performed)
                OptionRight?.Invoke();
          
        }
        public void OnBackAction(InputAction.CallbackContext context)
        {
            if (context.performed)
                OptionBack?.Invoke();
            
        }
        public void OnSubmitAction(InputAction.CallbackContext context)
        {
            if (context.performed)
                OptionSubmit?.Invoke();
            
        }
        public void OnSliderButtonInfo(InputAction.CallbackContext callbackContext)
        {
            _sliderValue = callbackContext.ReadValue<float>();
        }
        public float GetSliderValue()
        {
            return _sliderValue;
        }
        
        #endregion
       
        public void ChangeInputMap(InputMaps actionMap)
        {
            _currentActionMap = actionMap;
            playerInput.SwitchCurrentActionMap(actionMap.ToString());
            OnActionMapChanged?.Invoke(_currentActionMap);
            Debug.Log($"Action Map switch to : {_currentActionMap.ToString()}");
        }
        
    }
    public enum InputMaps
    {
        Player,
        UI,
        Interaction,
        Cheat
    }
}