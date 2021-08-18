using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PokerDefense.Managers;

namespace PokerDefense.UI.Scene
{
    public class UI_Scene : UI_Base
    {
        public override void Init()
        {
            GameManager.UI.SetCanvas(gameObject, false);
        }
    }
}
