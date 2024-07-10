using Studio23.SS2.AudioSystem.fmod.Extensions;
using System.Collections;
using System.Collections.Generic;
using com.gdcbd.bossbattle;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace Studio23.SS2.UIKit.Components
{
    public class Rotator : MonoBehaviour, ISelectHandler, IDeselectHandler

    {
        [SerializeField] private TextMeshProLocalizer _labeledText;
        [SerializeField] private TextMeshProUGUI _displayText;
        [SerializeField] private Button _leftButton;
        [SerializeField] private Button _rightButton;
        [SerializeField] private bool _makeUpdateImmedietly;
        [SerializeField] private int _selectedIndex;
        private IList _data;
        public UnityEvent OnSelectIndex;
        public UnityEvent<int> SelectedIndexUpdated;
        private LocalizedStringTable _localizedStringTable;


        void OnEnable()
        {
            UpdateLocalizeOptions(null);
        }

        void OnDisable()
        {
            OnDeselect(null);
        }

        void OnDestroy()
        {
            LocalizationSettings.SelectedLocaleChanged -= UpdateLocalizeOptions;
        }


        private void Start()
        {
            LocalizationSettings.SelectedLocaleChanged += UpdateLocalizeOptions;
            _leftButton.onClick.AddListener(DecreaseIndex);
            _rightButton.onClick.AddListener(IncreaseIndex);
        }

        public void InitializeData(IList data, int selectIndex = 0)
        {
            _selectedIndex = selectIndex;
            _data = data;
            SelectedIndexUpdateAction(false);
        }

        public void InitializeData(LocalizedString localString,LocalizedStringTable stringTable, int selectIndex = 0)
        {
            _selectedIndex = selectIndex;
            
            _labeledText.SetText(localString);
            _localizedStringTable = stringTable;
            UpdateLocalizeOptions(null);
            SelectedIndexUpdateAction(false);
        }

        public void InitializeData(LocalizedString localString, IList data, int selectIndex = 0)
        {
            _selectedIndex = selectIndex;
            _labeledText.SetText(localString);
            _data = data;
            SelectedIndexUpdateAction(false);
        }


        private void ShowText()
        {
            _displayText.text = _data[_selectedIndex].ToString();
            OnSelectIndex?.Invoke();
        }

        private void PlayEmitter()
        {
            TryGetComponent<CustomStudioEventEmitter>(out CustomStudioEventEmitter emitter);
            if (emitter != null) emitter.Play();
        }

        public void IncreaseIndex()
        {
            if (_selectedIndex < _data.Count - 1)
            {
                _selectedIndex++;
            }
            else
            {
                _selectedIndex = 0;
            }
            SelectedIndexUpdateAction();
            PlayEmitter();
        }


        public void DecreaseIndex()
        {
            if (_selectedIndex > 0)
            {
                _selectedIndex--;
            }
            else
            {
                _selectedIndex = _data.Count - 1;
            }
            SelectedIndexUpdateAction();
            PlayEmitter();
        }

        public void SelectedIndexUpdateAction(bool checkLiveStatus = true)
        {
            
            ShowText();
            if (checkLiveStatus)
                if (!_makeUpdateImmedietly) return;
            ApplyAction();
        }

        private void ApplyAction()
        {
            SelectedIndexUpdated?.Invoke(_selectedIndex);
        }

        public void OnSelect(BaseEventData eventData)
        {
            InputManager.Instance.OptionLeft += DecreaseIndex; // TODO : Have assign latter
            InputManager.Instance.OptionRight += IncreaseIndex;
        }

        public void OnDeselect(BaseEventData eventData)
        {
            InputManager.Instance.OptionLeft -= DecreaseIndex;
            InputManager.Instance.OptionRight -= IncreaseIndex;
        }

        public void UpdateLocalizeOptions(Locale locale)
        {
            if(_localizedStringTable == null) return;
            var localizedData = new List<string>();
            var stringTableEntries = _localizedStringTable.GetTable().Values;
            foreach (var tableEntry in stringTableEntries)
            {
                localizedData.Add(tableEntry.LocalizedValue);
            }

            _data = localizedData;
            ShowText();
        }
    }
}
