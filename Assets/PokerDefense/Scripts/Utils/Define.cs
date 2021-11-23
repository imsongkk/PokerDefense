using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokerDefense.Utils
{
    public class Define
    {
        public enum Scene
        {
            Unknown,
            Loading,
            Main,
            InGame,
        }

        public enum UIEvent
        {
            Click,
            Drag,
        }

        public enum MouseEvent
        {
            Press,
            Click,
            Drag,
            PointerDown,
            PointerUp
        }

        public enum AttackType
        {
            AD,
            AP,
        }
    }
}
