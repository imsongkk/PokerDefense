using PokerDefense.Data;
using PokerDefense.Managers;
using PokerDefense.UI.Popup;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    /// <summary>
    /// �� ��ų���� �ο��� index�� json�����͸� Ȯ���� ��
    /// </summary>
    /// 
    public UnityEvent<float, float>[] skillStarted; // ��ų ���ӽð�, ��ų ��Ÿ��
    public UnityEvent[] skillFinished;

    SkillData[] skillData;
    [SerializeField] GameObject fireHole;

    int skillCount;

    public void InitSkillManager()
    {
        InitSkillData();

        for (int i = 0; i < skillCount; i++)
        {
            int lambdaCapture = i;
            skillStarted[i].AddListener((skillTime, coolTime) => { StartSkill(lambdaCapture, skillTime, coolTime); });
        }
    }

    private void InitSkillData()
    {
        skillCount = GameManager.Data.SkillDataDict.Count;
        skillData = new SkillData[skillCount];
        skillStarted = new UnityEvent<float, float>[skillCount];
        skillFinished = new UnityEvent[skillCount];

        for (int i = 0; i < skillCount; i++)
        {
            skillStarted[i] = new UnityEvent<float, float>();
            skillFinished[i] = new UnityEvent();
            skillData[i] = new SkillData();
            GameManager.Data.SkillDataDict.TryGetValue(i, out skillData[i]);
        }
    }

    public void UseSkill(int skillIndex)
    {
        float skillTime = skillData[skillIndex].skillTime;
        float coolTime = skillData[skillIndex].coolTime;

        GameManager.Round.Gold -= skillData[skillIndex].skillCost; // ��ų �ڽ�Ʈ �Һ�
        skillStarted[skillIndex]?.Invoke(skillTime, coolTime);
    }

    public bool CheckSkillUse(int skillIndex)
    {
        if (skillData[skillIndex].isInCoolTime)
        {
            Debug.Log("��Ÿ�� �Դϴ�");
            // TODO : ��Ÿ�� ���� �ý��� �޼��� ����
            return false;
        }
        else if(GameManager.Round.CurrentState != RoundManager.RoundState.PLAY)
        {
            Debug.Log("������ ��ų�� ����� �� �����ϴ�");
            // TODO : �ý��� �޼��� ����
            return false;
        }
        else if(skillData[skillIndex].skillCost > GameManager.Round.Gold)
        {
            Debug.Log("�ڽ�Ʈ ����");
            // TODO : �ڽ�Ʈ ���� �ý��� �޼��� ����
            return false;
        }
        return true;
    }

    IEnumerator SetTimer(float time, UnityEvent FinishEvent)
    {
        yield return new WaitForSeconds(time);
        FinishEvent?.Invoke();
    }

    IEnumerator SetTimer(float time, Action FinishAction)
    {
        yield return new WaitForSeconds(time);
        FinishAction?.Invoke();
    }

    private void StartSkill(int skillIndex, float skillTime, float coolTime)
    {
        SetCoolTime(skillIndex, true);

        StartCoroutine(SetTimer(skillTime, skillFinished[skillIndex]));
        StartCoroutine(SetTimer(coolTime, () => { SetCoolTime(skillIndex, false); }));

        ActIndivSkill(skillIndex);
    }

    private void ActIndivSkill(int skillIndex)
    {
        switch (skillIndex)
        {
            case 0: //TimeStop
                {
                    // TODO : �ý��� �޼���
                    // TODO : UI_InGameScene�� timeText Ȧ��
                }
                break;
            case 1: //FireHole
                {
                    // TODO : �ý��� �޼���
                    var popup = GameManager.UI.ShowPopupUI<UI_SkillRangePopup>();
                    popup.InitSkillRangePopup(skillIndex, (rangeScreenPos) =>
                    {
                        StartCoroutine(SpawnFireHole(rangeScreenPos, skillData[skillIndex]));
                    });
                }
                break;
            case 2: //EarthQuake
                {
                    // TODO : �ý��� �޼���
                    // TODO : ��ܿ� ���ӽð� UI ����
                }
                break;
            case 3: //Meteo
                {
                    // TODO : �ý��� �޼���
                    var popup = GameManager.UI.ShowPopupUI<UI_SkillRangePopup>();
                    popup.InitSkillRangePopup(skillIndex, (rangeScreenPos) => 
                    {
                        var enemyList = GameManager.Round.GetEnemyInRange(rangeScreenPos, skillData[skillIndex].skillRange);
                        foreach(var enemy in enemyList)
                            enemy.OnDamage(skillData[skillIndex].skillDamage);
                    });
                }
                break;
        }
    }

    IEnumerator SpawnFireHole(Vector2 fireHoleScreenPos, SkillData fireHoleskillData)
    {
        float skillTime = fireHoleskillData.skillTime;
        float skillDamage = fireHoleskillData.skillDamage;
        float skillTic = fireHoleskillData.skillTic;
        float skillRange = fireHoleskillData.skillRange;

        float accumulatedTic = 0f;
        WaitForSeconds skillTicDelay = new WaitForSeconds(skillTic);

        fireHole.transform.position = Camera.main.ScreenToWorldPoint(new Vector2(fireHoleScreenPos.x, fireHoleScreenPos.y));
        fireHole.transform.localScale = new Vector2(2 * skillRange, 2 * skillRange);
        fireHole.SetActive(true);

        while(accumulatedTic < skillTime)
        {
            var enemyList = GameManager.Round.GetEnemyInRange(fireHoleScreenPos, skillRange);
            foreach (var enemy in enemyList)
                enemy.OnDamage(skillDamage);

            accumulatedTic += skillTic;
            yield return skillTicDelay;
        }
        yield return skillTicDelay;
        fireHole.SetActive(false);
    }

    private void SetCoolTime(int skillIndex, bool setCoolTime)
        => skillData[skillIndex].isInCoolTime = setCoolTime;
}
