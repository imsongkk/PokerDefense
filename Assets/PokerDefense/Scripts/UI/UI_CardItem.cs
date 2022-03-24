using PokerDefense.Managers;
using PokerDefense.Utils;
using static PokerDefense.Utils.Define;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PokerDefense.UI
{
    public class UI_CardItem : UI_Base
    {
        [SerializeField] Image cardImage;
        [SerializeField] int number;
        [SerializeField] CardShape shape = CardShape.Null;
        [SerializeField] int cardIndex;
        [SerializeField] GameObject rerollButton;
        [SerializeField] GameObject chanceButton;

        private void Start()
            => Init();

        public override void Init()
            => BindObjects();

        private void BindObjects()
        {
            AddUIEvent(gameObject, OnClickCardItem, Define.UIEvent.Click);
            AddUIEvent(rerollButton, OnClickRerollButton, Define.UIEvent.Click);
            AddUIEvent(chanceButton, OnClickChanceButton, Define.UIEvent.Click);
        }

        private void OnClickCardItem(PointerEventData evt)
        {
            // 아직 뽑지 않은 패일 경우 시행하지 않음
            if (shape == CardShape.Null) return;

        }
        
        private void OnClickRerollButton(PointerEventData evt)
        {
            Debug.Log($"{shape.ToString()} {number} 터치됨");

            rerollButton.SetActive(false);
            chanceButton.SetActive(true);

            InGameManager.Poker.ChangeCard(cardIndex, SetCardInfo);
        }

        private void OnClickChanceButton(PointerEventData evt)
        {
            Debug.Log($"{shape.ToString()} {number} 터치됨");

            InGameManager.Poker.ChangeCard(cardIndex, SetCardInfo);
        }

        public void InitCard(int index, (CardShape shape, int number) tuple)
        {
            this.cardIndex = index;
            SetCardInfo(tuple);
        }

        private void SetCardInfo((CardShape shape, int number) tuple)
        {
            shape = tuple.shape;
            number = tuple.number;
            RefreshImage();
        }

        public void OnPokerDrawed((CardShape shape, int number) tuple)
        {
            // 처음 포커 Draw 됐을 때
            // 이미 index는 초기화 완료
            rerollButton.SetActive(true);
            SetCardInfo(tuple);
        }

        private void RefreshImage() // Poker 패에 맞는 스프라이트 가져오기
            => cardImage.sprite = InGameManager.Poker.GetSprite((shape, number));
    }
}
