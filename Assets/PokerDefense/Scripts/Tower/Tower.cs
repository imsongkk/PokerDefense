using UnityEngine;
using PokerDefense.Data;
using PokerDefense.Managers;
using static PokerDefense.Managers.TowerManager;

namespace PokerDefense.Towers
{
    public class Tower : MonoBehaviour
    {
        TowerData towerData;

        SpriteRenderer spriteRenderer;

        bool isInit = false;

        TowerType towerType;
        protected int topCard;          // 탑 버프
        protected float towerDamage;    // 계산된 실제 대미지
        protected int finalPrice;       // 탑 버프 및 업그레이드에 의해 결정된 최종 가격


        private void Start()
            => Init();

        private void Init()
        {

        }

        public void InitTower(string towerName)
        {
            GameManager.Data.TowerDataDict.TryGetValue(towerName, out towerData);
            if (towerData == null) return;

            InitTowerSprite();

            isInit = true;
        }

        private void Update()
        {
            if (!isInit) return;

            Attack();
        }

        private void InitTowerSprite()
        {
            // TODO : towerName으로 Sprite 불러오기
        }

        public void SetTowerSettings(TowerType type, int topCard)
        {
            //TODO towerType 결정
            //TODO topcard 결정
            this.towerType = type;
            this.topCard = topCard;
        }


        private void Attack()
        {

        }

        protected virtual void DamageCalculate()
        {

        }
    }
}