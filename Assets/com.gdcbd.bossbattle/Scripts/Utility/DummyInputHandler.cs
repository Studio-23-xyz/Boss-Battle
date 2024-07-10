using System;
using System.Collections;
using System.Collections.Generic;
using com.gdcbd.bossbattle;
using UnityEngine;

public class DummyInputHandler : MonoBehaviour
{
   public InputMaps _inputMapSetOnStart = InputMaps.Player;
   private void Start()
   {
      InputManager.Instance.ChangeInputMap(_inputMapSetOnStart);
   }
}
