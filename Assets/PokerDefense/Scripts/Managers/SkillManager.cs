using PokerDefense.Data;
using PokerDefense.Managers;
using PokerDefense.UI.Popup;
using PokerDefense.Utils;
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

    public void SkillClicked(int skillIndex)
    {
        if (!CheckSkillUseable(skillIndex))
            return;

        switch (skillIndex)
        {
            case 0: //TimeStop
                {
                    GameManager.SystemText.SetSystemMessage(Define.SystemMessage.TimeStopSkillUse);
                    UseSkill(skillIndex);
                    // TODO : UI_InGameScene�� timeText Ȧ��
                }
                break;
            case 1: //FireHole
                {
                    var popup = GameManager.UI.ShowPopupUI<UI_SkillRangePopup>();
                    popup.InitSkillRangePopup(skillIndex, (rangeScreenPos) =>
                    {
                        skillStarted[skillIndex].AddListener((a, b) => { SpawnFireHole(rangeScreenPos, skillData[skillIndex]); });
                        GameManager.SystemText.SetSystemMessage(Define.SystemMessage.FireHoleSkillUse);
                        UseSkill(skillIndex);
                    });
                }
                break;
            case 2: //EarthQuake
                {
                    GameManager.SystemText.SetSystemMessage(Define.SystemMessage.EarthQuakeSkillUse);
                    UseSkill(skillIndex);
                }
                break;
            case 3: //Meteo
                {
                    var popup = GameManager.UI.ShowPopupUI<UI_SkillRangePopup>();
                    popup.InitSkillRangePopup(skillIndex, (rangeScreenPos) =>
                    {
                        skillStarted[skillIndex].AddListener((a, b) => { SpawnMeteo(rangeScreenPos, skillData[skillIndex]); });
                        GameManager.SystemText.SetSystemMessage(Define.SystemMessage.MeteoSkillUse);
                        UseSkill(skillIndex);
                    });
                }
                break;
        }
    }

    private void SpawnFireHole(Vector2 rangeScreenPos, SkillData skillData)
    {
        StartCoroutine(SpawnFireHoleCoroutine(rangeScreenPos, skillData));
    }

    private void SpawnMeteo(Vector2 rangeScreenPos, SkillData skillData)
    {
        var enemyList = GameManager.Round.GetEnemyInRange(rangeScreenPos, skillData.skillRange);
        foreach (var enemy in enemyList)
            enemy.OnDamage(skillData.skillDamage);
    }

    private void UseSkill(int skillIndex)
    {
        float skillTime = skillData[skillIndex].skillTime;
        float coolTime = skillData[skillIndex].coolTime;

        GameManager.Inventory.Gold -= skillData[skillIndex].skillCost; // ��ų �ڽ�Ʈ �Һ�
        skillStarted[skillIndex]?.Invoke(skillTime, coolTime);
    }

    private bool CheckSkillUseable(int skillIndex)
    {
        if (skillData[skillIndex].isInCoolTime)
        {
            GameManager.SystemText.SetSystemMessage(Define.SystemMessage.IsCoolTime);
            Debug.Log("��Ÿ�� �Դϴ�");
            // TODO : ��Ÿ�� ���� �ý��� �޼��� ����
            return false;
        }
        else if (GameManager.Round.CurrentState != RoundManager.RoundState.PLAY)
        {
            GameManager.SystemText.SetSystemMessage(Define.SystemMessage.NotPlayState);
            Debug.Log("������ ��ų�� ����� �� �����ϴ�");
            // TODO : �ý��� �޼��� ����
            return false;
        }
        else if (skillData[skillIndex].skillCost > GameManager.Inventory.Gold)
        {
            GameManager.SystemText.SetSystemMessage(Define.SystemMessage.NotEnoughCost);
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
    }

    IEnumerator SpawnFireHoleCoroutine(Vector2 fireHoleScreenPos, SkillData fireHoleskillData)
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

        while (accumulatedTic < skillTime)
        {
            var enemyList = GameManager.Round.GetEnemyInRange(fireHoleScreenPos, skillRange);
            foreach (var enemy in enemyList)
                enemy.OnDamage(skillDamage);

            accumulatedTic += skillTic;
            yield return skillTicDelay;
        }
        fireHole.SetActive(false);
    }

    private void SetCoolTime(int skillIndex, bool setCoolTime)
        => skillData[skillIndex].isInCoolTime = setCoolTime;
}
