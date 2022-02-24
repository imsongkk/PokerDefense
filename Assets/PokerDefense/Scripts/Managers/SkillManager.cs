using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public UnityEvent<float, float> TimeStopStarted, EarthQuakeStarted, FireHoleStarted; // ��ų ���ӽð�, ��ų ��Ÿ��
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
        // TODO : ������ ��������
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
        // TODO : �ð��� ���߾��ٴ� UI �۾�
        // TODO : UI_InGameScene�� timeText Ȧ��
    }

    private void FinishTimeStop()
    {

    }

    private void StartEarthQuake(float skillTime, float coolTime)
    {
        StartCoroutine(SetTimer(skillTime, EarthQuakeFinished));

        // TODO : ���� ���� ó�� grid ����
        // TODO : ������ ��������
        float earthQuakeSlow = 100f;
    }

    private void FinishEarthQuake()
    {

    }

    private void StartFireHole(float skillTime, float coolTime)
    {
        StartCoroutine(SetTimer(skillTime, FireHoleFinished));

        // TODO : ���� ���� ó�� grid ����
        // TODO : ������ ��������
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
