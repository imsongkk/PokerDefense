using UnityEngine;
using PokerDefense.Data;
using static PokerDefense.Managers.TowerManager;
using PokerDefense.Managers;

namespace PokerDefense.Towers
{
    public class Tower : MonoBehaviour
    {
        TowerData towerData;

        SpriteRenderer spriteRenderer;
        TowerType towerType;
        
        bool isInit = false;
        
        protected float towerDamage;    // 계산된 실제 대미지
        

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
            InitTowerType();

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

        private void InitTowerType()
        {
            // TODO : towerType으로 TowerType 결정
        }


        private void Attack()
        {

        }

        protected virtual void DamageCalculate()
        {

        }
    }
}