using UnityEngine;
using PokerDefense.Utils;
using PokerDefense.Managers;
using UnityEngine.UI;

namespace PokerDefense.UI.Popup
{
    public class UI_ShopPopup : UI_Popup
    {
        Transform content;
        ScrollRect scrollRect;

        enum GameObjects
        {
            BackButton,
            Content,
            ScrollView,
        }

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
            var list = GameManager.Data.ShopItemList;
            foreach (var itemId in list)
            {
                GameManager.Data.ItemDataDict.TryGetValue(itemId, out var itemData);
                UI_ShopItem shopItem = GameManager.Resource.Instantiate("UI/UI_ShopItem", content).GetComponent<UI_ShopItem>();
                shopItem.InitItem(this, itemData);
            }
        }

        private void BindObjects()
        {
            Bind<GameObject>(typeof(GameObjects));

            GameObject backButton = GetObject((int)GameObjects.BackButton);
            AddUIEvent(backButton, (a) => ClosePopupUI(), Define.UIEvent.Click);
            AddButtonAnim(backButton);

            content = GetObject((int)GameObjects.Content).transform;
            scrollRect = GetObject((int)GameObjects.ScrollView).GetComponent<ScrollRect>();
        }

        public ScrollRect GetScrollRect()
            => scrollRect;
    }
}
