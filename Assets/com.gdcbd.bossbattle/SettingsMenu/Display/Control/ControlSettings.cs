
using Studio23.SS2.Settings.Control.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Processors;

namespace Studio23.SS2.Settings.Core
{
    public class ControlSettings : MonoBehaviour
    {
        [SerializeField] private InputActionReference _inputActionReference;
        [SerializeField] private float _mouseSensitivityMultiplier;
        [SerializeField] private float _controllerSensitivityMultiplier;

        [SerializeField] private ControlSettingsConfiguration _controlSettingsConfiguration;


        public void Initialize(ControlSettingsConfiguration data)
        {
            InvertXAxis(data.IsInvertX);
            InvertYAxis(data.IsInvertY);
            UpdateMouseSensitivity(data.MouseSensitivity);
            UpdateControllerSensitivity(data.ControllerSensitivity);
        }

        /// <summary>
        /// Invert X axis
        /// </summary>
        /// <param name="axisState"></param>
        public void InvertXAxis(int axisState)
        {
            _inputActionReference.action.ApplyParameterOverride((InvertVector2Processor p) => p.invertX, axisState > 0);
        }

        /// <summary>
        /// Invert Y axis
        /// </summary>
        /// <param name="axisState"></param>
        public void InvertYAxis(int axisState)
        {
            _inputActionReference.action.ApplyParameterOverride((InvertVector2Processor p) => p.invertY, axisState > 0);
        }

        /// <summary>
        /// Change mouse sensitivity
        /// </summary>
        /// <param name="mouseSensitivity"></param>
        public void UpdateMouseSensitivity(float mouseSensitivity)
        {
            UpdateSensitivity(GetKeyBoardGroupName(), _mouseSensitivityMultiplier * mouseSensitivity);
        }

        /// <summary>
        /// Change controller sensitivity
        /// </summary>
        /// <param name="controllerSensitivity"></param>
        public void UpdateControllerSensitivity(float controllerSensitivity)
        {
            UpdateSensitivity(GetGamePadGroupName(), _controllerSensitivityMultiplier * controllerSensitivity);
        }

        private void UpdateSensitivity(string maskingGroup, float sensitivity)
        {
            _inputActionReference.action.ApplyParameterOverride("scaleVector2:x", sensitivity, InputBinding.MaskByGroups(maskingGroup));
            _inputActionReference.action.ApplyParameterOverride("scaleVector2:y", sensitivity, InputBinding.MaskByGroups(maskingGroup));
        }

        private string GetKeyBoardGroupName()
        {
            var keyboardGroupsName = string.Empty;
            foreach (var binding in _inputActionReference.action.bindings)
            {
                var groups = binding.groups;
                groups = groups.ToLower();
                if (groups.Contains("keyboard") | groups.Contains("mouse"))
                {
                    if (!string.IsNullOrEmpty(keyboardGroupsName)) keyboardGroupsName += ';';
                    keyboardGroupsName += groups;
                }
            }
            return keyboardGroupsName;
        }

        private string GetGamePadGroupName()
        {
            var gamepadGroupsName = string.Empty;
            foreach (var binding in _inputActionReference.action.bindings)
            {
                var groups = binding.groups;
                groups = groups.ToLower();
                if (groups.Contains("keyboard") | groups.Contains("mouse")) continue;
                if (!string.IsNullOrEmpty(gamepadGroupsName)) gamepadGroupsName += ';';
                gamepadGroupsName += groups;
            }
            return gamepadGroupsName;
        }

        public int ReturnDefaultInvertX() => _controlSettingsConfiguration.IsInvertX;
        public int ReturnDefaultInvertY() => _controlSettingsConfiguration.IsInvertY;
        public float ReturnDefaultMouseSensitivity() => _controlSettingsConfiguration.MouseSensitivity;
        public float ReturnDefaultControllerSensitivity() => _controlSettingsConfiguration.ControllerSensitivity;

    }

}
