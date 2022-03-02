using UnityEngine;
using PokerDefense.Utils;

namespace PokerDefense.UI.Popup
{
    public class UI_ShopPopup : UI_Popup
    {
        enum GameObjects
        {
            BackButton,
        }

        private void Start()
            => Init();

        public override void Init()
        {
            base.Init();

            BindObjects();
        }

        private void BindObjects()
        {
            Bind<GameObject>(typeof(GameObjects));

            GameObject backButton = GetObject((int)GameObjects.BackButton);
            AddUIEvent(backButton, (a) => ClosePopupUI(), Define.UIEvent.Click);
            AddButtonAnim(backButton);
        }
    }
}
