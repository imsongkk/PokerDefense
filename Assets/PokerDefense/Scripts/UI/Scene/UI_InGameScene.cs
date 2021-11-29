using PokerDefense.UI.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PokerDefense.Managers;

namespace PokerDefense.UI.Scene
{
    public class UI_InGameScene : UI_Scene
    {
        enum GameObjects
        {
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
