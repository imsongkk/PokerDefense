using PokerDefense.Data;
using PokerDefense.Managers;
using PokerDefense.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PokerDefense.Utils.Define;

public class Enemy : MonoBehaviour
{
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
        }

        Enemy owner;

        public float Speed { get; private set; }
        public float Hp { get; private set; }
        public string Name { get; private set; }
        public bool IsBoss { get; private set; }
        public int Damage { get; private set; }
        public EnemyType EnemyType { get; private set; }

        public void OnDamage(float damage)
            => Hp -= damage;

        public void OnSlow(float time, float percent)
        {
            Speed *= 1 - (percent / 100);
        }

        public void OnSlowResume()
        {
            Speed = owner.enemyOriginData.moveSpeed;
        }
    }
    EnemyData enemyOriginData;
    public EnemyIndivData enemyIndivData { get; private set; }

    [SerializeField] Transform hpBarGroup, hitText;
    [SerializeField] Image hpBarImage;

    protected int curIndex = 0;

    public static List<Transform> wayPoints;
    public static Transform endPoint;

    private float originScaleX;

    Vector3 moveDirection;

    private void Update()
    {
        //MoveHpBar();
        //hitText.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0f, 0.1f, 0f));
    }

    private void Awake()
    {
        originScaleX = transform.localScale.x;

        GameManager.Data.SkillIndexDict.TryGetValue("TimeStop", out var timeStopSkillIndex);
        GameManager.Skill.skillStarted[timeStopSkillIndex].AddListener((stopTime, nouse) => enemyIndivData.OnSlow(stopTime, 100));
        GameManager.Skill.skillFinished[timeStopSkillIndex].AddListener(()=> { enemyIndivData.OnSlowResume(); });

        GameManager.Data.SkillIndexDict.TryGetValue("EarthQuake", out var earthQuakeSkillIndex);
        GameManager.Skill.skillStarted[earthQuakeSkillIndex].AddListener((slowTime, nouse) =>
        {
            GameManager.Data.SkillDataDict.TryGetValue(earthQuakeSkillIndex, out var skillData);
            enemyIndivData.OnSlow(slowTime, skillData.slowPercent);
        });
        GameManager.Skill.skillFinished[earthQuakeSkillIndex].AddListener(()=> { enemyIndivData.OnSlowResume(); });
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
                if(Util.GetNearFourDirection(moveDirection) == Vector2.right)
                    transform.localScale = new Vector2(originScaleX * -1, transform.localScale.y);
                else if (Util.GetNearFourDirection(moveDirection) == Vector2.left)
                    transform.localScale = new Vector2(originScaleX , transform.localScale.y);

                transform.Translate(moveDirection * enemyIndivData.Speed * Time.deltaTime);
                yield return null;
            }
        }
    }

    public void Die()
    {
        Debug.Log($"{enemyIndivData.Name} died");

        if(enemyIndivData.IsBoss)
        {
            // TODO : 보스 죽음 팝업
        }
        GameManager.Round.OnEnemyDied();
        Destroy(gameObject);
    }

    public void OnDamage(float damage) 
    {
        Debug.Log($"{enemyIndivData.Name} got {damage} damaged");

        enemyIndivData.OnDamage(damage);
        if (enemyIndivData.Hp < 0)
            Die();
        RefreshHpBar();
    }

    public void OnSlow(float time, float percent)
    {
        enemyIndivData.OnSlow(time, percent);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("WayPoint"))
            curIndex++;
        else if (collision.gameObject.CompareTag("EndPoint"))
        {
            if (curIndex + 2 >= wayPoints.Count)
                Destroy(gameObject);
            GameManager.Round.OnEnemyGetEndPoint();
        }
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
