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
            public Item(string itemName, int price, int uid)
            {
                ItemName = itemName;
                Price = price;
            }
            public string ItemName { get; private set; }
            public int Price { get; private set; }
            public int Uid { get; private set; }
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
            // TODO : ������ ������ ���̺� �°� �ʱ�ȭ
            // TODO : ������ uid�� �´� ��������Ʈ ���ҽ� ��������
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
            // TODO : �����丵 �ʿ�
            //GameManager.Data.ShopDataDict.TryGetValue(nameof(GameManager.Round.Chance), out var price);

            string itemName = "chance";
            GameManager.Data.ItemIndexDict.TryGetValue(itemName, out int itemId);
            GameManager.Data.ItemDataDict.TryGetValue(itemId, out var itemData);

            int price = itemData.price;

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
