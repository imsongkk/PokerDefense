using UnityEngine;
using PokerDefense.Utils;
using UnityEngine.EventSystems;
using PokerDefense.Managers;

namespace PokerDefense.UI.Popup
{
    public class UI_HorseSelectPopup : UI_Popup
    {
        enum GameObjects
        {
            HorseOneButton,
            HorseTwoButton,
            HorseThreeButton,
            HorseFourButton,
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

            GameObject one = GetObject((int)GameObjects.HorseOneButton);
            AddUIEvent(one, HorseOneButton, Define.UIEvent.Click);
            AddButtonAnim(one);

            GameObject two = GetObject((int)GameObjects.HorseTwoButton);
            AddUIEvent(two, HorseTwoButton, Define.UIEvent.Click);
            AddButtonAnim(two);

            GameObject three = GetObject((int)GameObjects.HorseThreeButton);
            AddUIEvent(three, HorseThreeButton, Define.UIEvent.Click);
            AddButtonAnim(three);

            GameObject four = GetObject((int)GameObjects.HorseFourButton);
            AddUIEvent(four, HorseFourButton, Define.UIEvent.Click);
            AddButtonAnim(four);
        }

        private void HorseOneButton(PointerEventData evt) { GameManager.Round.Horse = 0; ClosePopupUI(); }
        private void HorseTwoButton(PointerEventData evt) { GameManager.Round.Horse = 1; ClosePopupUI(); }
        private void HorseThreeButton(PointerEventData evt) { GameManager.Round.Horse = 2; ClosePopupUI(); }
        private void HorseFourButton(PointerEventData evt) { GameManager.Round.Horse = 3; ClosePopupUI(); }
    }
}
