using PokerDefense.UI.Popup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PokerDefense.Managers;
using UnityEngine.EventSystems;
using PokerDefense.Utils;
using static PokerDefense.Utils.Define;

namespace PokerDefense.UI.Popup
{
    public class UI_Poker : UI_Popup
    {
        private enum GameObjects
        {
            CardList,
            CardDeck,
            PokerButton,
            ConfirmButton,
        }

        GameObject cardDeck;
        UI_CardItem[] cardItems = new UI_CardItem[5];
        bool isPokerDrawed = false;

        private Transform cardHand;

        private List<Sprite>[] cardsSpriteList;
        private List<(CardShape shape, int number)> cardList;

        private void Start()
            => Init();

        public override void Init()
        {
            base.Init();
            GameManager.Poker.SetUIPoker(this);

            BindObjects();

            cardsSpriteList = GameManager.Poker.cardsSpriteList;
            cardList = GameManager.Poker.CardList;

        }

        private void BindObjects()
        {
            Bind<GameObject>(typeof(GameObjects));

            PokerUIReset();

            cardDeck = GetObject((int)GameObjects.CardDeck);

            GameObject pokerButton = GetObject((int)GameObjects.PokerButton);
            AddUIEvent(pokerButton, OnClickPokerButton, Define.UIEvent.Click);
            AddButtonAnim(pokerButton);

            GameObject confirmButton = GetObject((int)GameObjects.ConfirmButton);
            AddUIEvent(confirmButton, OnClickConfirmButton, Define.UIEvent.Click);
            AddButtonAnim(confirmButton);
        }

        private void InitCardItems(Transform parent)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                if (cardItems[i] == null)   // UI_Poker를 처음 불러올 때 초기화
                {
                    UI_CardItem cardItem = Util.GetOrAddComponent<UI_CardItem>(parent.GetChild(i).gameObject);
                    cardItems[i] = cardItem;
                    cardItem.SetUIPoker(this);
                }
                // 모든 카드를 Joker Z(디폴트 스프라이트)로 교체
                InstantiateCard(i, cardItems[i], (CardShape.Joker, 0));
            }
        }

        private void InstantiateCard(int index, UI_CardItem card, (CardShape shape, int number) cardTuple)
        {
            card.InitCard(index, cardTuple.number, cardTuple.shape);
        }

        public void InstantiateCardIndex(int index)
        {
            //TODO 카드뽑기 애니메이션
            InstantiateCard(index, cardItems[index], cardList[index]);
        }

        public void PokerUIReset()
        {
            Transform cardListTransform = GetObject((int)GameObjects.CardList).transform;
            InitCardItems(cardListTransform);
        }

        private void OnClickPokerButton(PointerEventData evt)
        {
            /* RoundState, 찬스 개수에 따라 눌릴지 안눌릴지 결정 */

            GameManager.Poker.GetHand((int index) =>
            {
                InstantiateCardIndex(index);
                isPokerDrawed = true;
            }
            );
        }

        private void OnClickConfirmButton(PointerEventData evt)
        {
            if (!isPokerDrawed)
            {
                GameManager.UI.ShowPopupUI<UI_PokerErrorPopup>();
                return;
            }

            // 포커 패 확정!
            // RoundManager에게 Poker State 종료 알리기
            GameManager.Round.BreakState();
            ClosePopupUI();
        }

        private void OnClickResetButton(PointerEventData evt)
        {
            GameManager.Poker.ResetDeque(() =>
            {
                PokerUIReset();
            });
        }
    }
}