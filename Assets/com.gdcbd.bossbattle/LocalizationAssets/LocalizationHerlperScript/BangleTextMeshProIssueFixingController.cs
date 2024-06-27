using System.Collections.Generic;
using System.Linq;
using com.gdcbd.bossbattle.utility;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Studio23.SS2
{
    public class BangleTextMeshProIssueFixingController : PersistentMonoSingleton<BangleTextMeshProIssueFixingController>
    {
        public string CharacterPrefixFix;
        public List<ReplaceCharacterData> CharacterToReplace;
        public string CharacterToIgnore;


        public string FixBangleOrder(string stringToFix)
        {
            if (LocalizationSettings.SelectedLocale.Identifier.Code.Equals("bn") == false) return stringToFix;

            var fixedText = FixTextOrder(stringToFix);
            return fixedText;
        }

        private string FixTextOrder(string inputText)
        {
            List<char> newString = new List<char>();

            foreach (var c in inputText)
            {

                Debug.Log($"Found character {c} and ascii code {(int)c}");

                if (IsCharacterMatched(CharacterPrefixFix,c))
                {
                    var indent = FindSwapIndex(newString);
                    newString.Insert(indent, c);
                }
                else if (CharacterToReplace.FirstOrDefault(x => x.CharacterToReplace.Equals(c)) != null)
                {
                    var characterMatched = CharacterToReplace.FirstOrDefault(x => x.CharacterToReplace.Equals(c));
                    var indent = FindSwapIndex(newString);
                    newString.Add(characterMatched.ReplacedSuffixCharacter);
                    newString.Insert(indent, characterMatched.ReplacedPrefixCharacter);
                }
                else
                {
                    newString.Add(c);
                }
            }

            return new string(newString.ToArray());
        }

        

        private bool IsCharacterMatched(string checkedAgainst, char c)
        {
            if(checkedAgainst.Contains(c)) return true;
            return false;
        }

        private int FindSwapIndex(List<char> sourceText)
        {
            var indent = 0;
            var startingIndex = sourceText.Count - 1;

            for (int i = startingIndex; i >= 0; i--)
            {
                if (i == 0) break;
                if (IsCharacterMatched(CharacterToIgnore, sourceText[i])) continue;
                if (IsCharacterMatched(CharacterToIgnore, sourceText[i - 1]) && !IsCharacterMatched(CharacterPrefixFix, sourceText[i - 1])) continue;
                //if (sourceText[i] > 2500) continue;
                //if (sourceText[i - 1] > 2500 && !IsCharacterMatched(CharacterPrefixFix, sourceText[i - 1])) continue;
                indent = i;
                break;
            }

            return indent;
        }

        protected override void Initialize()
        {
            
        }
    }


    [System.Serializable]
    public class ReplaceCharacterData
    {
        public char CharacterToReplace;
        public char ReplacedPrefixCharacter;
        public char ReplacedSuffixCharacter;
    }
}
