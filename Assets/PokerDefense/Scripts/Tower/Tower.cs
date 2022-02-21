using UnityEngine;
using PokerDefense.Data;
using PokerDefense.Managers;
using static PokerDefense.Managers.TowerManager;
using System.Collections;
using System.Collections.Generic;
using PokerDefense.Utils;

namespace PokerDefense.Towers
{
    public class TowerIndivData
    {
        public TowerIndivData(int topCard, float damage, float speed, float range, 
            int rareNess, int price, bool isHidden, TowerType towerType, string towerName, int index)
        {
            TopCard = topCard;
            Damage = damage;
            Speed = speed;
            Range = range;
            RareNess = rareNess;
            Price = price;
            IsHidden = isHidden;
            TowerType = towerType;
            TowerName = towerName;
            Index = index;
        }
        public int TopCard { get; private set; }
        public float Damage { get; private set; }
        public float Speed { get; private set; }
        public float Range { get; private set; }
        public int RareNess { get; private set; }
        public int Price { get; private set; }
        public bool IsHidden { get; private set; }
        public TowerType TowerType { get; private set; }
        public string TowerName { get; private set; }

        public int Index { get; private set; }

        public void UpgradeDamage(int newDamage)
        {
            Damage = newDamage;
            UpdatePrice();
        }
        public void UpgradeSpeed(int newSpeed)
        {
            Speed = newSpeed;
            UpdatePrice();
        }
        public void UpgradeRange(int newRange)
        {
            Range = newRange;
            UpdatePrice();
        }

        private void UpdatePrice()
        {
            // TODO : 알맞게 가격 upgrade
        }
    }

    public abstract class Tower : MonoBehaviour
    {
        protected TowerData towerLowData; // 족보로 결정되는 값들
        public TowerIndivData TowerIndivData { get; private set; } // 타워 개개인이 가지고 있는 데이터

        protected CircleCollider2D rangeCollider;
        protected WaitForSeconds attackDelay;
        protected List<Enemy> enemies = new List<Enemy>();
        protected Animator animator;

        private void Awake()
        {
            animator = transform.GetChild(0).GetComponent<Animator>();
        }

        protected void StartAttacking(object sender, System.EventArgs e)
        {
            StartCoroutine(Attacking());
        }

        protected void StopAttacking(object sender, System.EventArgs e)
        {
            StopCoroutine(Attacking());
        }

        protected IEnumerator Attacking()
        {
            while(true)
            {
                if(enemies.Count > 0)
                {
                    Attack();
                    yield return attackDelay;
                }
                yield return null;
            }
        }

        public void InitTower(string towerName, TowerType towerType, int topCard, int index) // lowData로 타워 처음 생성
        {
            GameManager.Data.TowerDataDict.TryGetValue(towerName, out towerLowData);
            if (towerLowData == null) return;

            GameManager.Round.RoundStarted += StartAttacking;
            GameManager.Round.RoundFinished += StopAttacking;

            attackDelay = new WaitForSeconds(towerLowData.attackSpeed);
            TowerIndivData = new TowerIndivData(topCard, towerLowData.damage, 
                towerLowData.attackSpeed, towerLowData.attackRange, towerLowData.rareNess,
                towerLowData.basePrice, towerLowData.isHidden, towerType, towerName, index);

            SetRangeCollider();
        }

        public void InitTower(TowerSaveData towerSaveData) // 세이브 된 데이터로 생성
        {
            TowerData modifiedTowerData = towerSaveData.towerData;

            GameManager.Round.RoundStarted += StartAttacking;
            GameManager.Round.RoundFinished += StopAttacking;

            attackDelay = new WaitForSeconds(modifiedTowerData.attackSpeed);
            TowerIndivData = new TowerIndivData(towerSaveData.topCard, modifiedTowerData.damage, 
                modifiedTowerData.attackSpeed, modifiedTowerData.attackRange, modifiedTowerData.rareNess,
                modifiedTowerData.basePrice, modifiedTowerData.isHidden, towerSaveData.towerType, 
                towerSaveData.towerName, towerSaveData.towerIndex);

            SetRangeCollider();
        }

        protected virtual void Attack()
        {
            if (enemies.Count == 0) return;

            Enemy target = enemies[0];
            Define.EnemyType enemyType = target.EnemyIndivData.EnemyType;
            TowerType towerType = TowerIndivData.TowerType;
            float calculatedDamage = Define.CalculateDamage(towerType, enemyType, TowerIndivData.Damage);

            enemies[0].OnDamage(calculatedDamage); // 몬스터에게 실제 데미지 전달
            SetAnimAttack(); // 애니메이션 스타트
        }

        // 객체마다 다른 공격 방식

        protected virtual void DamageCalculate()
        {
            // Deprecated 
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Enemy"))
            {
                enemies.Add(collision.gameObject.GetComponent<Enemy>());
            }
        }

        protected void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                enemies.RemoveAt(0); // FIFO
            }
        }

        protected void SetAnimAttack()
        {
            animator.SetBool("Attack", true);
        }

        public void UpgradeDamage(int newDamage)
            => TowerIndivData.UpgradeDamage(newDamage);
        public void UpgradeSpeed(int newSpeed)
            => TowerIndivData.UpgradeSpeed(newSpeed);
        public void UpgradeRange(int newRange)
        {
            TowerIndivData.UpgradeRange(newRange);
            rangeCollider.radius = TowerIndivData.Range;
        }

        protected void SetRangeCollider()
        {
            rangeCollider = gameObject.GetComponent<CircleCollider2D>();
            rangeCollider.radius = TowerIndivData.Range;
        }
    }
}