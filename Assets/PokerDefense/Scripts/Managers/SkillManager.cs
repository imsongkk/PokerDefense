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
                    // TODO : UI_InGameScene의 timeText 홀딩
                }
                break;
            case 1: //FireHole
                {
                    var popup = GameManager.UI.ShowPopupUI<UI_SkillRangePopup>();
                    popup.InitSkillRangePopup(skillIndex, (rangeScreenPos) =>
                    {
						skillStarted[skillIndex].AddListener((a, b)=>{ SpawnFireHole(rangeScreenPos, skillData[skillIndex]); });
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

        GameManager.Round.Gold -= skillData[skillIndex].skillCost; // 스킬 코스트 소비
        skillStarted[skillIndex]?.Invoke(skillTime, coolTime);
    }

    private bool CheckSkillUseable(int skillIndex)
    {
        if (skillData[skillIndex].isInCoolTime)
        {
            GameManager.SystemText.SetSystemMessage(Define.SystemMessage.IsCoolTime);
            Debug.Log("쿨타임 입니다");
            // TODO : 쿨타임 부족 시스템 메세지 띄우기
            return false;
        }
        else if (GameManager.Round.CurrentState != RoundManager.RoundState.PLAY)
        {
            GameManager.SystemText.SetSystemMessage(Define.SystemMessage.NotPlayState);
            Debug.Log("지금은 스킬을 사용할 수 없습니다");
            // TODO : 시스템 메세지 띄우기
            return false;
        }
        else if (skillData[skillIndex].skillCost > GameManager.Round.Gold)
        {
            GameManager.SystemText.SetSystemMessage(Define.SystemMessage.NotEnoughCost);
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

        while(accumulatedTic < skillTime)
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
