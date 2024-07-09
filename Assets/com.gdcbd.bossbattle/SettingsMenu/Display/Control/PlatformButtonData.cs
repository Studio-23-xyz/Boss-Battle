using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Studio23.SS2.UI.Misc
{
    [System.Flags]
    public enum PlatformButtonData
    {
        Steam = 1,
        XBox_PC = 2,
        XBox_Console = 4,
        PlayStation = 8 ,
        Nintendo = 16,
        Default = 32,
    }
}
