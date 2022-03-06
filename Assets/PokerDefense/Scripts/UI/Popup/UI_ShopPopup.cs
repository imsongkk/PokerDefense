using UnityEngine;
using PokerDefense.Utils;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using PokerDefense.Managers;

namespace PokerDefense.UI.Popup
{
    public class UI_ShopPopup : UI_Popup
    {
        public class Item
        {
            public Item(string itemName, int price)
            {
                ItemName = itemName;
                Price = price;
            }
            public string ItemName { get; private set; }
            public int Price { get; private set; }
        }

        enum GameObjects
        {
            BackButton,

            ChancePurchaseButton,
        }

        List<Item> itemList = new List<Item>();

        private void Start()
            => Init();

        public override void Init()
        {
            base.Init();
            BindObjects();

            InitShopData();
        }

        private void InitShopData()
        {
            // TODO : 아이템 데이터 테이블에 맞게 초기화
        }

        private void BindObjects()
        {
            Bind<GameObject>(typeof(GameObjects));

            GameObject backButton = GetObject((int)GameObjects.BackButton);
            AddUIEvent(backButton, (a) => ClosePopupUI(), Define.UIEvent.Click);
            AddButtonAnim(backButton);

            GameObject chancePurchaseButton = GetObject((int)GameObjects.ChancePurchaseButton);
            AddUIEvent(chancePurchaseButton, OnClickChancePurchaseButton, Define.UIEvent.Click);
            AddButtonAnim(chancePurchaseButton);
        }

        private void OnClickChancePurchaseButton(PointerEventData evt)
        {
            GameManager.Data.ShopDataDict.TryGetValue(nameof(GameManager.Round.Chance), out var price);

            // TODO : 아이템 데이터 테이블에 맞게 초기화
            if (GameManager.Round.Gold >= price)
            {
                GameManager.Round.Chance++;
                GameManager.Round.Gold -= price;
            }
            else
            {
                GameManager.UI.ShowPopupUI<UI_ShopErrorPopup>();
            }
        }
    }
}
