using PokerDefense.Data;
using PokerDefense.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PokerDefense.Utils.Define;

public class EnemyIndivData
{
    public EnemyIndivData(float speed, float hp, string name, bool isBoss, int damage, EnemyType enemyType)
    {
        Speed = speed;
        Hp = hp;
        IsBoss = isBoss;
        Damage = damage;
        Name = name;
        EnemyType = enemyType;
    }

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
        // 슬로우 능력 가진 타워에게
        Speed *= percent / 100;
    }
}

public class Enemy : MonoBehaviour
{
    EnemyData enemyOriginData;
    public EnemyIndivData EnemyIndivData { get; private set; }

    [SerializeField] Transform hpBarGroup, hitText;
    [SerializeField] Image hpBarImage;

    protected int curIndex = 0;

    public static List<Transform> wayPoints;
    public static Transform endPoint;

    Vector3 moveDirection;

    private void Update()
    {
        hpBarGroup.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0f, 0.2f, 0f));
        //hitText.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0f, 0.1f, 0f));
    }

    private void Start()
        => Init();

    private void Init()
        => StartCoroutine(Move());
    

    public void InitEnemy(string enemyName, EnemyData enemyOriginData)
    {
        EnemyIndivData = new EnemyIndivData(enemyOriginData.moveSpeed, enemyOriginData.hp, 
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
                moveDirection = (wayPoints[(curIndex + 1) % wayPoints.Count].position - wayPoints[curIndex].position).normalized;
                transform.Translate(moveDirection * EnemyIndivData.Speed * Time.deltaTime);
                yield return null;
            }
        }
    }

    public void Die()
    {
        Debug.Log($"{EnemyIndivData.Name} died");

        if(EnemyIndivData.IsBoss)
        {
            // TODO : 보스 죽음 팝업
        }
        GameManager.Round.OnEnemyDied();
        Destroy(gameObject);
    }

    public void OnDamage(float damage) 
    {
        Debug.Log($"{EnemyIndivData.Name} got {damage} damaged");

        EnemyIndivData.OnDamage(damage);
        if (EnemyIndivData.Hp < 0)
            Die();
        RefreshHpBar();
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

    private void RefreshHpBar()
    {
        hpBarImage.fillAmount = (EnemyIndivData.Hp > 0 ? EnemyIndivData.Hp : 0) / enemyOriginData.hp;
    }

    private void RefreshHitText()
    {

    }
}
