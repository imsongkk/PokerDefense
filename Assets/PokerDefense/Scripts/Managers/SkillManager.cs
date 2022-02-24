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
    public UnityEvent<float, float> TimeStopStarted, EarthQuakeStarted, FireHoleStarted; // 스킬 지속시간, 스킬 쿨타임
    public UnityEvent TimeStopFinished, EarthQuakeFinished, FireHoleFinished;

    public bool IsCoolTimeTimeStop 
    { 
        get => timeStopSkillData.isInCoolTime;  
        private set => timeStopSkillData.isInCoolTime = value; 
    }
    public bool IsCoolTimeEarthQuake
    {
        get => earthQuakeSkillData.isInCoolTime;
        private set => earthQuakeSkillData.isInCoolTime = value;
    }
    public bool IsCoolTimeFireHole
    {
        get => fireHoleSkillData.isInCoolTime;
        private set => fireHoleSkillData.isInCoolTime = value;
    }

    SkillData timeStopSkillData, fireHoleSkillData, earthQuakeSkillData, meteoSkillData;

    public void InitSkillManager()
    {
        InitSkillData();

        TimeStopStarted.AddListener(StartTimeStop);
        TimeStopFinished.AddListener(FinishTimeStop);

        EarthQuakeStarted.AddListener(StartEarthQuake);
        EarthQuakeFinished.AddListener(FinishEarthQuake);

        FireHoleStarted.AddListener(StartFireHole);
        FireHoleFinished.AddListener(FinishFireHole);
    }

    private void InitSkillData()
    {
        GameManager.Data.SkillDataDict.TryGetValue("TimeStop", out timeStopSkillData);
        GameManager.Data.SkillDataDict.TryGetValue("FireHole", out fireHoleSkillData);
        GameManager.Data.SkillDataDict.TryGetValue("EarthQuake", out earthQuakeSkillData);
        GameManager.Data.SkillDataDict.TryGetValue("Meteo", out meteoSkillData);
    }

    public void UseTimeStopSkill()
    {
        // TODO : 데이터 가져오기
        float skillTime = timeStopSkillData.skillTime;
        float coolTime = timeStopSkillData.coolTime;
        TimeStopStarted.Invoke(skillTime, coolTime);
    }

    public void UseMeteoSkill()
    {

    }

    public void UseEarthQuakeSkill()
    {

    }

    public void UseFireHoleSkill()
    {

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

    private void StartTimeStop(float skillTime, float coolTime)
    {
        StartCoroutine(SetTimer(skillTime, TimeStopFinished));
        StartCoroutine(SetTimer(coolTime, () => { IsCoolTimeTimeStop = false; }));

        IsCoolTimeTimeStop = true;
        // TODO : 시간이 멈추었다는 UI 작업
        // TODO : UI_InGameScene의 timeText 홀딩
    }

    private void FinishTimeStop()
    {

    }

    private void StartEarthQuake(float skillTime, float coolTime)
    {
        StartCoroutine(SetTimer(skillTime, EarthQuakeFinished));

        // TODO : 실제 지진 처럼 grid 흔들기
        // TODO : 데이터 가져오기
        float earthQuakeSlow = 100f;
    }

    private void FinishEarthQuake()
    {

    }

    private void StartFireHole(float skillTime, float coolTime)
    {
        StartCoroutine(SetTimer(skillTime, FireHoleFinished));

        // TODO : 실제 지진 처럼 grid 흔들기
        // TODO : 데이터 가져오기
        /*
        float earthQuakeSlow = 100f;
        foreach (var enemy in enemies)
        {
            enemy.OnSlow(time, earthQuakeSlow);
        }
        */
    }

    private void FinishFireHole()
    {

    }
}
