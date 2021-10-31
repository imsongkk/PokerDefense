using PokerDefense.Managers;
using PokerDefense.UI.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PokerDefense.Scene
{
    public class LoadingScene : BaseScene
    {
        private void Start()
            => Init();

        protected override void Init()
        {
            base.Init();
            SceneType = Utils.Define.Scene.Loading;
            GameManager.UI.ShowSceneUI<UI_LoadingScene>("UI_LoadingScene");
        }
    }
}
