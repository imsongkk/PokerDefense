using UnityEngine;
using TMPro;
using PokerDefense.Managers;
using PokerDefense.Utils;
using UnityEngine.EventSystems;
using PokerDefense.UI.Popup;

namespace PokerDefense.UI.Scene
{
    public class UI_InGameScene : UI_Scene
    {
        enum GameObjects
        {
            MenuButton,
            HeartText,
            GoldText,
            RoundText,
        }

        TextMeshProUGUI heartText, goldText, roundText;

        private void Start()
            => Init();

        public override void Init()
        {
            base.Init();

            Bind<GameObject>(typeof(GameObjects));
            BindObject();

            GameManager.Round.SetUIIngameScene(this);
        }

        private void BindObject()
        {
            heartText = GetObject((int)GameObjects.HeartText).GetComponent<TextMeshProUGUI>();
            goldText = GetObject((int)GameObjects.GoldText).GetComponent<TextMeshProUGUI>();
            roundText = GetObject((int)GameObjects.RoundText).GetComponent<TextMeshProUGUI>();

            GameObject menuButton = GetObject((int)GameObjects.MenuButton);
            AddUIEvent(menuButton, OnClickMenuButton, Define.UIEvent.Click);
            AddButtonAnim(menuButton);
        }

        private void OnClickMenuButton(PointerEventData evt)
        {
            GameManager.UI.ShowPopupUI<UI_InGameMenuPopup>();
        }

        public void InitText(int heart, int gold, int round)
        {
            heartText.text = heart.ToString();
            goldText.text = gold.ToString();
            roundText.text = round.ToString();
        }

        public void SetHeartText(int count)
            => heartText.text = count.ToString();
        public void SetGoldText(int count)
            => goldText.text = count.ToString();
        public void SetRoundText(int round)
            => roundText.text = $"Round : {round}";
    }
}
