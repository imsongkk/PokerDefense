using PokerDefense.Managers;
using PokerDefense.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using PokerDefense.Data;
using PokerDefense.UI.Popup;

namespace PokerDefense.UI
{
    public class UI_ShopItem : UI_Base, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        enum GameObjects
        {
            PurchaseButton,
        }

        [SerializeField] TextMeshProUGUI goldCountText;
        [SerializeField] Image itemImage;

        private ItemData itemData;
        UI_ShopPopup ui_ShopPopup;
        ScrollRect scrollRect;

        private void Start()
            => Init();

        public override void Init()
            => BindObjects();

        private void BindObjects()
        {
            Bind<GameObject>(typeof(GameObjects));

            GameObject purchaseButton = GetObject((int)GameObjects.PurchaseButton);
            AddUIEvent(purchaseButton, OnClickPurchaseButton, Define.UIEvent.Click);
            AddButtonAnim(purchaseButton);
        }

        public void InitItem(UI_ShopPopup _ui_ShopPopup, ItemData _itemData)
        {
            // TODO : 스프라이트 가져오는거 리팩토링 필요(임시)
            itemData = _itemData;
            ui_ShopPopup = _ui_ShopPopup;
            scrollRect = ui_ShopPopup.GetScrollRect();

            itemImage.sprite = GameManager.Resource.Load<Sprite>($"Sprites/Items/{itemData.itemId}");
            goldCountText.text = itemData.price.ToString();
        }

        private void OnClickPurchaseButton(PointerEventData evt)
        {
            GameManager.Inventory.OnClickPuchase(itemData.itemId, 1);
            scrollRect.OnBeginDrag(evt);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            ((IBeginDragHandler)scrollRect).OnBeginDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ((IEndDragHandler)scrollRect).OnEndDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            ((IDragHandler)scrollRect).OnDrag(eventData);
        }
    }
}
