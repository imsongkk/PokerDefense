using PokerDefense.Data;
using PokerDefense.Managers;
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
            // TODO : 쿨타임 UI 띄우기
            return false;
        }
        else if(GameManager.Round.CurrentState != RoundManager.RoundState.PLAY)
        {
            Debug.Log("지금은 스킬을 사용할 수 없습니다");
            // TODO : UI 띄우기
            return false;
        }
        else if(skillData[skillIndex].skillCost > GameManager.Round.Gold)
        {
            Debug.Log("코스트 부족");
            // TODO : 코스트 부족 UI 띄우기
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


        switch(skillIndex)
        {

        }
        // TODO : 시간이 멈추었다는 UI 작업
        // TODO : UI_InGameScene의 timeText 홀딩
    }

    private void SetCoolTime(int skillIndex, bool setCoolTime)
        => skillData[skillIndex].isInCoolTime = setCoolTime;
}
