using PokerDefense.Managers;
using PokerDefense.Scene;
using PokerDefense.UI.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokerDefense.Scene
{
    public class MainScene : BaseScene
    {
        private void Start()
            => Init();

        protected override void Init()
        {
            base.Init();
            GameManager.UI.ShowSceneUI<UI_MainScene>();
        }
    }
}