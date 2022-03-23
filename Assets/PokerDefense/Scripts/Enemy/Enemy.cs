using PokerDefense.Data;
using PokerDefense.Managers;
using PokerDefense.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using static PokerDefense.Utils.Define;

namespace PokerDefense.Enemies
{
    public class Enemy : MonoBehaviour
    {
        [Flags]
        public enum Debuff
        {
            Slow = 1,
            Weak = 2,
            EarthQuake = 4,
            TimeStop = 8
        }

        public struct DebuffData
        {
            public Debuff debuff;          // 디버프 종류
            public float debuffTime;       // 디버프 시간
            public float debuffPercent;    // 디버프 강도
        }

        public class EnemyIndivData
        {
            public EnemyIndivData(Enemy owner, float speed, float hp, string name, bool isBoss, int damage, EnemyType enemyType)
            {
                this.owner = owner;

                Speed = speed;
                Hp = hp;
                IsBoss = isBoss;
                Damage = damage;
                Name = name;
                EnemyType = enemyType;
                enemyDebuff = 0;

                debuffTime.Add(Enemy.Debuff.Slow, 0);
                debuffTime.Add(Enemy.Debuff.Weak, 0);
            }

            Enemy owner;

            public float Speed { get; private set; }
            public float Hp { get; private set; }
            public string Name { get; private set; }
            public bool IsBoss { get; private set; }
            public int Damage { get; private set; }
            public EnemyType EnemyType { get; private set; }

            private float additionalDamage;
            private float slowPercent;

            public float AdditionalDamage
            {
                get { return additionalDamage; }
                private set { additionalDamage = value; }
            }

            public float SlowPercent
            {
                get { return slowPercent; }
                private set
                {
                    //* 변수 변경 시 Speed에 즉시 적용
                    slowPercent = value;
                    Speed = owner.enemyOriginData.moveSpeed * (1 - (slowPercent / 100));
                }
            }
            public Enemy.Debuff enemyDebuff;
            public Dictionary<Enemy.Debuff, float> debuffTime;

            public void OnDamage(float damage)
                => Hp -= (damage + additionalDamage);

            public void OnSlow(float percent)
            {
                //* 가해진 디버프의 강도가 기존의 슬로우 정도보다 강할 경우에만 적용
                SlowPercent = ((SlowPercent < percent) ? percent : SlowPercent);
            }

            public void OnSlowResume()
            {
                SlowPercent = 0;
            }

            public void OnWeak(float percent)
            {
                AdditionalDamage = ((AdditionalDamage < percent) ? percent : AdditionalDamage);
            }

            public void OnWeakResume()
            {
                AdditionalDamage = 0;
            }
        }
        EnemyData enemyOriginData;
        public EnemyIndivData enemyIndivData { get; private set; }

        public Dictionary<Debuff, UnityEvent<float>> debuffEvents;     // 디버프 적용 함수 목록
        public Dictionary<Debuff, UnityEvent> debuffStopEvents; // 디버프 해제 함수 목록

        [SerializeField] Transform hpBarGroup, hitText;
        [SerializeField] Image hpBarImage;

        protected int curIndex = 0;

        public static List<Transform> wayPoints;
        public static Transform endPoint;

        private float originScaleX;

        Vector3 moveDirection;

        bool died = false;

        private void Update()
        {
            //MoveHpBar();
            //hitText.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0f, 0.1f, 0f));

            //* 디버프 시간 측정 및 디버프 해제
            if ((enemyIndivData.enemyDebuff & Debuff.Slow) != 0)
            {
                if (enemyIndivData.debuffTime[Debuff.Slow] > 0)
                {
                    enemyIndivData.debuffTime[Debuff.Slow] -= Time.deltaTime;
                }
                else
                {
                    enemyIndivData.enemyDebuff &= ~Debuff.Slow;
                    enemyIndivData.debuffTime[Debuff.Slow] = 0;
                    debuffStopEvents[Debuff.Slow].Invoke();
                }
            }
            if ((enemyIndivData.enemyDebuff & Debuff.Weak) != 0)
            {
                if (enemyIndivData.debuffTime[Debuff.Weak] > 0)
                {
                    enemyIndivData.debuffTime[Debuff.Weak] -= Time.deltaTime;
                }
                else
                {
                    enemyIndivData.enemyDebuff &= ~Debuff.Weak;
                    enemyIndivData.debuffTime[Debuff.Weak] = 0;
                    debuffStopEvents[Debuff.Weak].Invoke();
                }
            }
        }

        private void Awake()
        {
            originScaleX = transform.localScale.x;

            //* 스킬 목록
            GameManager.Data.SkillIndexDict.TryGetValue("TimeStop", out var timeStopSkillIndex);
            GameManager.Skill.skillStarted[timeStopSkillIndex].AddListener((stopTime, nouse) =>
            {
                enemyIndivData.enemyDebuff ^= Debuff.TimeStop;
                enemyIndivData.OnSlow(100);
            });
            GameManager.Skill.skillFinished[timeStopSkillIndex].AddListener(() =>
            {
                enemyIndivData.enemyDebuff &= ~Debuff.TimeStop;
                if ((enemyIndivData.enemyDebuff & (Debuff.EarthQuake ^ Debuff.Slow)) == 0) enemyIndivData.OnSlowResume();
            });

            GameManager.Data.SkillIndexDict.TryGetValue("EarthQuake", out var earthQuakeSkillIndex);
            GameManager.Skill.skillStarted[earthQuakeSkillIndex].AddListener((slowTime, nouse) =>
            {
                enemyIndivData.enemyDebuff ^= Debuff.EarthQuake;
                GameManager.Data.SkillDataDict.TryGetValue(earthQuakeSkillIndex, out var skillData);
                enemyIndivData.OnSlow(skillData.slowPercent);
            });
            GameManager.Skill.skillFinished[earthQuakeSkillIndex].AddListener(() =>
            {
                enemyIndivData.enemyDebuff &= ~Debuff.EarthQuake;
                if ((enemyIndivData.enemyDebuff & (Debuff.TimeStop ^ Debuff.Slow)) == 0) enemyIndivData.OnSlowResume();
            });

            //* 발동 이벤트 추가
            debuffEvents.Add(Debuff.Slow, new UnityEvent<float>());
            debuffEvents[Debuff.Slow].AddListener(SlowEnemy);
            debuffEvents.Add(Debuff.Weak, new UnityEvent<float>());
            debuffEvents[Debuff.Weak].AddListener(WeakEnemy);

            debuffStopEvents.Add(Debuff.Slow, new UnityEvent());
            debuffStopEvents[Debuff.Slow].AddListener(StopSlowEnemy);
            debuffStopEvents.Add(Debuff.Weak, new UnityEvent());
            debuffStopEvents[Debuff.Weak].AddListener(StopWeakEnemy);
        }

        private void Start()
            => Init();

        private void Init()
        {
            MoveHpBar(); // 1프레임 방지
            StartCoroutine(Move());
        }

        public void InitEnemy(string enemyName, EnemyData enemyOriginData)
        {
            enemyIndivData = new EnemyIndivData(this, enemyOriginData.moveSpeed, enemyOriginData.hp,
                enemyName, enemyOriginData.isBoss, enemyOriginData.damage, enemyOriginData.enemyType);
            this.enemyOriginData = enemyOriginData;

            RefreshHpBar();
        }

        IEnumerator Move()
        {
            if (wayPoints.Count >= 2)
            {
                while (curIndex < wayPoints.Count)
                {
                    MoveHpBar();

                    moveDirection = (wayPoints[(curIndex + 1) % wayPoints.Count].position - wayPoints[curIndex].position).normalized;
                    if (Util.GetNearFourDirection(moveDirection) == Vector2.right)
                        transform.localScale = new Vector2(originScaleX * -1, transform.localScale.y);
                    else if (Util.GetNearFourDirection(moveDirection) == Vector2.left)
                        transform.localScale = new Vector2(originScaleX, transform.localScale.y);

                    transform.Translate(moveDirection * enemyIndivData.Speed * Time.deltaTime);
                    yield return null;
                }
            }
        }

        public void Die()
        {
            died = true;

            Debug.Log($"{enemyIndivData.Name} died");

            if (enemyIndivData.IsBoss)
            {
                // TODO : 보스 죽음 팝업
            }
            GameManager.Round.EnemyKillCount++;
            Destroy(gameObject);
        }

        public void OnHit(float damage, DebuffData debuffData, BuffStackDelegate buffStack)
        {
            Debug.Log($"{enemyIndivData.Name} got {damage} damaged");

            enemyIndivData.OnDamage(damage);
            if (enemyIndivData.Hp < 0 && !died)
                Die();
            RefreshHpBar();

            DebuffEnemy(debuffData);
            buffStack.Invoke();
        }

        public void OnDamage(float damage)
        {
            //! Deprecated
            Debug.Log($"{enemyIndivData.Name} got {damage} damaged");

            enemyIndivData.OnDamage(damage);
            if (enemyIndivData.Hp < 0 && !died)
                Die();
            RefreshHpBar();
        }

        public void DebuffEnemy(DebuffData debuffData)
        {
            DebuffEnemy(debuffData.debuff, debuffData.debuffTime, debuffData.debuffPercent);
        }

        public void DebuffEnemy(Debuff debuff, float debuffTime, float debuffPercent)
        {
            //* 디버프 적용
            enemyIndivData.enemyDebuff ^= debuff;
            enemyIndivData.debuffTime[debuff] += debuffTime;

            UnityEvent<float> debuffEvent;
            debuffEvents.TryGetValue(debuff, out debuffEvent);
            debuffEvent.Invoke(debuffPercent);
        }

        //* 각 디버프 적용 함수 목록: debuffEvents에 추가
        private void SlowEnemy(float debuffPercent)
        {
            // debuffPercent만큼의 비율로 속도 감소. 중첩되지 않음
            enemyIndivData.OnSlow(debuffPercent);
        }

        private void WeakEnemy(float debuffPercent)
        {
            // debuffPercent만큼의 비율로 데미지 증가. 중첩되지 않음
            enemyIndivData.OnWeak(debuffPercent);
        }

        //* 각 디버프 적용 해제 함수 목록: debuffStopEvents에 추가
        private void StopSlowEnemy()
        {
            // 지진 및 시간정지 스킬이 사용되지 않는 동안에만 슬로우 해제
            if ((enemyIndivData.enemyDebuff & (Debuff.EarthQuake ^ Debuff.TimeStop)) == 0) enemyIndivData.OnSlowResume();
        }

        private void StopWeakEnemy()
        {
            enemyIndivData.OnWeakResume();
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("WayPoint"))
                curIndex++;
            else if (collision.gameObject.CompareTag("EndPoint"))
            {
                if (curIndex + 2 >= wayPoints.Count)
                    Destroy(gameObject);
                GameManager.Round.EnemyEndPointCount++;
            }
        }

        public bool IsInRange(Vector2 screenSpaceOffset, float range)
        {
            float pixelsPerUnitInScreenSpace = Util.GetPixelsPerUnitInScreenSpace();
            Vector2 enemyScreenPos = Camera.main.WorldToScreenPoint(transform.position);
            float distance = Vector2.Distance(screenSpaceOffset, enemyScreenPos);

            if (distance <= range * pixelsPerUnitInScreenSpace)
                return true;
            return false;
        }

        private void MoveHpBar()
        {
            hpBarGroup.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0f, 0.2f, 0f));
        }

        private void RefreshHpBar()
        {
            hpBarImage.fillAmount = (enemyIndivData.Hp > 0 ? enemyIndivData.Hp : 0) / enemyOriginData.hp;
        }

        private void RefreshHitText()
        {

        }
    }

}
