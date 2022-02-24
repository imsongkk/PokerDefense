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

    public bool IsCoolTimeTimeStop { get; private set; } = false;
    public bool IsCoolTimeEarthQuake { get; private set; } = false;
    public bool IsCoolTimeFireHole { get; private set; } = false;

    public void InitSkillManager()
    {
        TimeStopStarted.AddListener(StartTimeStop);
        TimeStopFinished.AddListener(FinishTimeStop);

        EarthQuakeStarted.AddListener(StartEarthQuake);
        EarthQuakeFinished.AddListener(FinishEarthQuake);

        FireHoleStarted.AddListener(StartFireHole);
        FireHoleFinished.AddListener(FinishFireHole);
    }

    public void UseTimeStopSkill()
    {
        // TODO : 데이터 가져오기
        float skillTime = 5f;
        float coolTime = 10f;
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
