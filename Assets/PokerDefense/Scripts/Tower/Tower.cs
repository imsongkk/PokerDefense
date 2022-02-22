using UnityEngine;
using PokerDefense.Data;
using PokerDefense.Managers;
using static PokerDefense.Managers.TowerManager;
using System.Collections;
using System.Collections.Generic;
using PokerDefense.Utils;
using System;

namespace PokerDefense.Towers
{
    public abstract class Tower : MonoBehaviour
    {
        public class TowerIndivData
        {
            public TowerIndivData(Tower owner, int topCard, int damageLevel, 
                int speedLevel, int rangeLevel, int criticalLevel, int rareNess, int price, 
                bool isHidden, TowerType towerType, string towerName, int index)
            {
                GameManager.Data.TowerUpgradeDataDict.TryGetValue(towerName, out towerUpgradeData);

                this.owner = owner;
                TopCard = topCard;
                RareNess = rareNess;
                Price = price;
                IsHidden = isHidden;
                TowerType = towerType;
                TowerName = towerName;
                Index = index;

                DamageLevel = damageLevel;
                SpeedLevel = speedLevel;
                RangeLevel = rangeLevel;
                CriticalLevel = criticalLevel;

                owner.attackDelay = new WaitForSeconds(Speed);
            }

            Tower owner;
            TowerUpgradeData towerUpgradeData;

            int damageLevel, speedLevel, rangeLevel, criticalLevel;
            int maxDamageLevel, maxSpeedLevel, maxRangeLevel, maxCriticalLevel;

            public float Damage { get; private set; }
            public int DamageLevel 
            { 
                get => damageLevel; 
                set 
                { 
                    damageLevel = value;
                    //if(damageLevel == maxDamageLevel)
                        // TODO : UI 업데이트
                    Damage = towerUpgradeData.attackDamageTable[damageLevel];
                    Debug.Log(Damage);
                    UpdatePrice();
                } 
            }
            public float Speed { get; private set; }
            public int SpeedLevel
            {
                get => speedLevel;
                set
                {
                    speedLevel = value;
                    // TODO
                    Speed = towerUpgradeData.attackSpeedTable[speedLevel];
                    UpdatePrice();
                }
            }
            public float Range { get; private set; }
            public int RangeLevel
            {
                get => rangeLevel;
                set
                {
                    rangeLevel = value;
                    // TODO
                    Range = towerUpgradeData.attackRangeTable[rangeLevel];
                    owner.rangeCollider.radius = Range; // 실제 사거리 변경
                    owner.attackRangeCircle.localScale = new Vector2(Range * 2, Range * 2);
                    UpdatePrice();
                }
            }
            public float Critical { get; private set; }
            public int CriticalLevel
            {
                get => criticalLevel;
                set
                {
                    criticalLevel = value;
                    // TODO
                    Critical = towerUpgradeData.attackCriticalTable[criticalLevel];
                    UpdatePrice();
                }
            }
            public int TopCard { get; private set; }
            public int RareNess { get; private set; }
            public int Price { get; private set; }
            public bool IsHidden { get; private set; }
            public TowerType TowerType { get; private set; }
            public string TowerName { get; private set; }
            public int Index { get; private set; }

            public void UpgradeDamage(int currentLevel)
            {
                DamageLevel = currentLevel;
                UpdatePrice();
            }
            public void UpgradeSpeed(int currentLevel)
            {
                SpeedLevel = currentLevel;
                UpdatePrice();
            }
            public void UpgradeRange(int currentLevel)
            {
                RangeLevel = currentLevel;
                UpdatePrice();
            }
            public void UpgradeCritical(int currentLevel)
            {
                CriticalLevel = currentLevel;
                UpdatePrice();
            }

            private void UpdatePrice()
            {
                // TODO : 알맞게 가격 upgrade
            }
        }
        protected TowerUniqueData towerUniqueData; // 족보로 결정되는 불변한 수치들
        public TowerIndivData towerIndivData { get; private set; } // 타워 개개인이 가지고 있는 데이터

        protected CircleCollider2D rangeCollider;
        protected WaitForSeconds attackDelay;
        protected List<Enemy> enemies = new List<Enemy>();
        protected Animator animator;
        protected Transform attackRangeCircle;
        protected float originScaleX;

        private void Awake()
        {
            animator = transform.GetChild(0).GetComponent<Animator>();
            attackRangeCircle = transform.GetChild(1);
            rangeCollider = gameObject.GetComponent<CircleCollider2D>();
            originScaleX = transform.localScale.x;
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

        public void InitTower(string towerName, TowerType towerType, int topCard, int index) // 타워 처음 생성
        {
            // 더미 세이브 데이터 생성

            TowerSaveData towerSaveData = new TowerSaveData();
            towerSaveData.towerName = towerName;
            towerSaveData.attackDamageLevel = 0;
            towerSaveData.attackSpeedLevel = 0;
            towerSaveData.attackRangeLevel = 0;
            towerSaveData.attackCriticalLevel = 0;
            towerSaveData.towerIndex = index;
            towerSaveData.towerType = towerType;
            towerSaveData.topCard = topCard;

            InitTower(towerSaveData);
        }

        public void InitTower(TowerSaveData towerSaveData) // 세이브 된 데이터로 생성
        {
            string towerName = towerSaveData.towerName;
            GameManager.Data.TowerUniqueDataDict.TryGetValue(towerName, out towerUniqueData);
            if (towerUniqueData == null) return;

            GameManager.Round.RoundStarted += StartAttacking;
            GameManager.Round.RoundFinished += StopAttacking;

            towerIndivData = new TowerIndivData(this, towerSaveData.topCard, towerSaveData.attackDamageLevel,
                towerSaveData.attackSpeedLevel, towerSaveData.attackRangeLevel, towerSaveData.attackCriticalLevel,
                towerUniqueData.rareNess, towerUniqueData.basePrice, towerUniqueData.isHidden, towerSaveData.towerType, 
                towerSaveData.towerName, towerSaveData.towerIndex);

            //SetRangeCollider();
        }

        protected virtual void Attack()
        {
            if (enemies.Count == 0) return;

            Enemy target = enemies[0];
            Define.EnemyType enemyType = target.EnemyIndivData.EnemyType;
            TowerType towerType = towerIndivData.TowerType;
            float calculatedDamage = Define.CalculateDamage(towerType, enemyType, towerIndivData.Damage);

            target.OnDamage(calculatedDamage); // 몬스터에게 실제 데미지 전달
            SetAnimAttack(target); // 애니메이션 스타트
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

        protected void SetAnimAttack(Enemy target)
        {
            Vector2 towerDirection = Util.GetNearTwoDirection(transform.position, target.transform.position);

            if (towerDirection == Vector2.right)
                transform.localScale = new Vector2(originScaleX * -1, transform.localScale.y);
            else if (towerDirection == Vector2.left)
                transform.localScale = new Vector2(originScaleX, transform.localScale.y);

            animator.SetBool("Attack", true);
        }

        public void DestroyTower(Action afterDestroyAction)
        {
            UnityEngine.Object.Destroy(gameObject);
            afterDestroyAction?.Invoke();
        }

        public void UpgradeDamageLevel()
            => towerIndivData.DamageLevel = towerIndivData.DamageLevel + 1;
        public void UpgradeSpeedLevel()
            => towerIndivData.SpeedLevel = towerIndivData.SpeedLevel + 1;
        public void UpgradeRangeLevel()
            => towerIndivData.RangeLevel = towerIndivData.RangeLevel + 1;
        public void UpgradeCriticalLevel()
            => towerIndivData.CriticalLevel = towerIndivData.CriticalLevel + 1;

        protected void SetRangeCollider()
        {
            rangeCollider = gameObject.GetComponent<CircleCollider2D>();
            rangeCollider.radius = towerIndivData.Range;
            attackRangeCircle.localScale = new Vector2(towerIndivData.Range * 2, towerIndivData.Range * 2);
        }

        public void HighlightRangeCircle()
            => attackRangeCircle.gameObject.SetActive(true);

        public void ResetRangeCircle()
            => attackRangeCircle.gameObject.SetActive(false);
    }
}