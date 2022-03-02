using UnityEngine;
using PokerDefense.Utils;
using UnityEngine.EventSystems;
using PokerDefense.Managers;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

namespace PokerDefense.UI.Popup
{
    public class UI_HorseSelectPopup : UI_Popup
    {
        TMP_InputField bettingInput;

        enum GameObjects
        {
            HorseOneButton,
            HorseTwoButton,
            HorseThreeButton,
            HorseFourButton,

            BettingInput,

            UpButton,
            DownButton,
            ConfirmButton,
            CancelButton,
        }

        int? horseIndex = null;
        int? bettingPrice = 0;
        int maxBettingPrice;

        private int? HorseIndex
        {
            get => horseIndex;
            set
            {
                if(horseIndex.HasValue)
                {
                    ResetButtonImage(horseIndex.Value);
                    HighLightButtonImage(value.Value);
                }
                else
                    HighLightButtonImage(value.Value);

                horseIndex = value;
            }
        }

        List<GameObject> horseButtons = new List<GameObject>();

        private void Start()
            => Init();

        public override void Init()
        {
            base.Init();
            BindObjects();

            maxBettingPrice = GameManager.Round.Gold;

            bettingInput.onValueChanged.AddListener(CheckBettingPrice);
        }

        private void BindObjects()
        {
            Bind<GameObject>(typeof(GameObjects));

            bettingInput = GetObject((int)GameObjects.BettingInput).GetComponent<TMP_InputField>();
            
            GameObject one = GetObject((int)GameObjects.HorseOneButton);
            AddUIEvent(one, (e) => HorseIndex = 0, Define.UIEvent.Click);
            AddButtonAnim(one);
            horseButtons.Add(one);

            GameObject two = GetObject((int)GameObjects.HorseTwoButton);
            AddUIEvent(two, (e) => HorseIndex = 1, Define.UIEvent.Click);
            AddButtonAnim(two);
            horseButtons.Add(two);

            GameObject three = GetObject((int)GameObjects.HorseThreeButton);
            AddUIEvent(three, (e) => HorseIndex = 2, Define.UIEvent.Click);
            AddButtonAnim(three);
            horseButtons.Add(three);

            GameObject four = GetObject((int)GameObjects.HorseFourButton);
            AddUIEvent(four, (e) => HorseIndex = 3, Define.UIEvent.Click);
            AddButtonAnim(four);
            horseButtons.Add(four);

            GameObject upButton = GetObject((int)GameObjects.UpButton);
            AddUIEvent(upButton, OnClickUpButton, Define.UIEvent.Click);
            AddButtonAnim(upButton);

            GameObject downButton = GetObject((int)GameObjects.DownButton);
            AddUIEvent(downButton, OnClickDownButton, Define.UIEvent.Click);
            AddButtonAnim(downButton);

            GameObject confirmButton = GetObject((int)GameObjects.ConfirmButton);
            AddUIEvent(confirmButton, OnClickConfirmButton, Define.UIEvent.Click);
            AddButtonAnim(confirmButton);

            GameObject cancelButton = GetObject((int)GameObjects.CancelButton);
            AddUIEvent(cancelButton, OnClickCancelButton, Define.UIEvent.Click);
            AddButtonAnim(cancelButton);
        }

        private void OnClickUpButton(PointerEventData evt)
        {
            if (bettingPrice.Value >= maxBettingPrice) return;
            bettingInput.text = (++bettingPrice).ToString();
        }

        private void OnClickDownButton(PointerEventData evt)
        {
            if (bettingPrice.Value <= 0) return;
            bettingInput.text = (--bettingPrice).ToString();
        }

        private void OnClickConfirmButton(PointerEventData evt)
        {
            if (horseIndex == null)
            {
                GameManager.UI.ShowPopupUI<UI_HorseErrorPopup>();
                return;
            }
            if (!bettingPrice.HasValue || bettingPrice.Value <= 0)
            {
                GameManager.UI.ShowPopupUI<UI_BettingErrorPopup>();
                return;
            }

            GameManager.Horse.OnBettingFinished(horseIndex, bettingPrice);
            ClosePopupUI();
        }

        private void OnClickCancelButton(PointerEventData evt)
        {
            GameManager.Horse.OnBettingFinished(null, null);
            ClosePopupUI();
        }

        private void CheckBettingPrice(string priceText)
        {
            int price = 0;
            if( ! int.TryParse(priceText, out price))
            {
                return;
                // 숫자만 입력
            }

            if(price > maxBettingPrice)
                price = maxBettingPrice;
            else if(price < 0)
                price = 0;

            bettingInput.text = price.ToString();
        }

        private void HighLightButtonImage(int horseIndex)
            => horseButtons[horseIndex].GetComponent<Image>().color = Color.red;
        private void ResetButtonImage(int horseIndex)
            => horseButtons[horseIndex].GetComponent<Image>().color = Color.white;
    }
}
