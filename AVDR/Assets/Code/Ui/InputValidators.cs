using System;
using UnityEngine;

namespace TMPro {

    /// <summary>
    /// Input validator for positive integers and zero.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "InputValidator - Positive Int.asset",
        menuName = "TextMeshPro/Input Validators/PositiveInt",
        order = 100)]
    public class PosIntZeroValidator : TMP_InputValidator {
        public override char Validate(ref string text, ref int pos, char ch)
        {
            if(ch >= '0' && ch <= '9') {
                text += ch;
                pos += 1;
                return ch;
            }
            return (char)0;
        }
    }
}