using UnityEngine;
using PokerDefense.Utils;

namespace PokerDefense.UI.Popup
{
    public class UI_HorseErrorPopup : UI_Popup
    {
        enum GameObjects
        {
            ConfirmButton,
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

            GameObject confirmButton = GetObject((int)GameObjects.ConfirmButton);
            AddUIEvent(confirmButton, (a) => ClosePopupUI(), Define.UIEvent.Click);
            AddButtonAnim(confirmButton);
        }
    }
}
