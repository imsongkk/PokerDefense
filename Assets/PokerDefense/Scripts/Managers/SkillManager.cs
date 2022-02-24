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
    /// 각 스킬에게 부여된 index는 json데이터를 확인할 것
    /// </summary>
    /// 
    public UnityEvent<float, float>[] skillStarted; // 스킬 지속시간, 스킬 쿨타임
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

        GameManager.Round.Gold -= skillData[skillIndex].skillCost; // 스킬 코스트 소비
        skillStarted[skillIndex]?.Invoke(skillTime, coolTime);
    }

    public bool CheckSkillUse(int skillIndex)
    {
        if (skillData[skillIndex].isInCoolTime)
        {
            Debug.Log("쿨타임 입니다");
            // TODO : 쿨타임 부족 시스템 메세지 띄우기
            return false;
        }
        else if(GameManager.Round.CurrentState != RoundManager.RoundState.PLAY)
        {
            Debug.Log("지금은 스킬을 사용할 수 없습니다");
            // TODO : 시스템 메세지 띄우기
            return false;
        }
        else if(skillData[skillIndex].skillCost > GameManager.Round.Gold)
        {
            Debug.Log("코스트 부족");
            // TODO : 코스트 부족 시스템 메세지 띄우기
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
                    // TODO : 시스템 메세지
                    // TODO : UI_InGameScene의 timeText 홀딩
                }
                break;
            case 1: //FireHole
                {
                    // TODO : 시스템 메세지
                    var popup = GameManager.UI.ShowPopupUI<UI_SkillRangePopup>();
                    popup.InitSkillRangePopup(skillIndex, (rangeScreenPos) =>
                    {
                        StartCoroutine(SpawnFireHole(rangeScreenPos, skillData[skillIndex]));
                    });
                }
                break;
            case 2: //EarthQuake
                {
                    // TODO : 시스템 메세지
                    // TODO : 상단에 지속시간 UI 띄우기
                }
                break;
            case 3: //Meteo
                {
                    // TODO : 시스템 메세지
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
