using System;
using System.Collections;
using System.Threading;
using com.gdcbd.bossbattle;
using Cysharp.Threading.Tasks;
 
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace Studio23.SS2.UIKit.Components
{
    public class LabeledSlider : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private TextMeshProLocalizer LabelText;
        [SerializeField] private TextMeshProUGUI DisplayText;
        [SerializeField] Slider _slider;
        [SerializeField] private bool _makeUpdateImmedietly;
        public UnityEvent<float> SliderValueUpdated;
        private CancellationTokenSource _sliderCancelToken;
        public float Value => _slider.value;

        void Start()
        {
            _slider.onValueChanged.AddListener((_ => { SelectedIndexUpdateAction(); }));
        }

        void OnDisable()
        {
            OnDeselect(null);
        }


        public void InitializeData(string label, float minValue=0, float maxValue=1, float value = 0)
        {
            //LabelText.text=label;
            _slider.value=value;
            _slider.minValue=minValue;
            _slider.maxValue=maxValue;
            SelectedIndexUpdateAction(false);
        }

        public void InitializeData(LocalizedString LabelTextLabelText, float minValue = 0, float maxValue = 1, float value = 0)
        {
            LabelText.SetText(LabelTextLabelText);
            _slider.value = value;
            _slider.minValue = minValue;
            _slider.maxValue = maxValue;
            SelectedIndexUpdateAction(false);
        }


        private void ShowText()
        {
            DisplayText.text = _slider.value.ToString("0.0");
        }


        public void SelectedIndexUpdateAction(bool checkLiveStatus = true)
        {
            ShowText();
            if (checkLiveStatus)
                if (!_makeUpdateImmedietly) return;
            ApplyAction(_slider.value);
        }

        private void ApplyAction(float value)
        {
            SliderValueUpdated?.Invoke(_slider.value);
        }

        public void OnSelect(BaseEventData eventData)
        {
            _sliderCancelToken = new CancellationTokenSource();
            UpdateSlider();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            _sliderCancelToken?.Cancel();
        }

        private async void UpdateSlider()
        {
            while (!_sliderCancelToken.IsCancellationRequested)
            {
                var sliderVal = InputManager.Instance.GetSliderValue();
                _slider.value += sliderVal * Time.unscaledDeltaTime;
                Debug.Log("Pause Slider : "+ _slider.value);
                _slider.value = Mathf.Clamp(_slider.value, _slider.minValue, _slider.maxValue);
                await UniTask.Yield();

            }
        }
    }

}
