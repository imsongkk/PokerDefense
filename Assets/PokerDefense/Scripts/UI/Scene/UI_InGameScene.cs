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
            Bottom,
            HeartText,
            GoldText,
            RoundText,
            ChanceText,
        }

        TextMeshProUGUI heartText, goldText, roundText, chanceText;
        GameObject bottomUIObject;

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
            chanceText = GetObject((int)GameObjects.ChanceText).GetComponent<TextMeshProUGUI>();

            GameObject menuButton = GetObject((int)GameObjects.MenuButton);
            AddUIEvent(menuButton, OnClickMenuButton, Define.UIEvent.Click);
            AddButtonAnim(menuButton);

            bottomUIObject = GetObject((int)GameObjects.Bottom);
        }

        private void OnClickMenuButton(PointerEventData evt)
        {
            GameManager.UI.ShowPopupUI<UI_InGameMenuPopup>();
        }

        public void ActivateBottomUI()
        {
            bottomUIObject.SetActive(true);
        }

        public void InitText(int round, int heart, int gold, int chance)
        {
            SetRoundText(round);
            SetHeartText(heart);
            SetGoldText(gold);
            SetChanceText(chance);
        }

        public void SetHorseIndex(int index)
        {
            GameManager.Horse.RunHorse(index);
            GameManager.Round.BreakState();
        }

        public void SetRoundText(int round)
            => roundText.text = $"Round : {round}";
        public void SetHeartText(int count)
            => heartText.text = count.ToString();
        public void SetGoldText(int count)
            => goldText.text = count.ToString();
        public void SetChanceText(int count)
            => chanceText.text = count.ToString();
    }
}
