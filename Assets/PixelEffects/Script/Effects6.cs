using System.Collections;
using UnityEngine;
public class Effects6 : MonoBehaviour
{
	Transform _t;
	Effects _effects;
	void Awake()
	{
		_t = transform;
		_effects = GetComponentInParent<Effects>();
	}
	void Start()
	{
		StartCoroutine(Test());
	}
	IEnumerator Test()
	{
		yield return new WaitForSeconds(.333f);
		while (true)
		{
			if (Random.value > .5f)
				_effects.TriggerPoison(_t.localPosition);
			else
				_effects.TriggerDark(_t.localPosition);
			yield return new WaitForSeconds(Random.Range(.5f, 1f));
		}
	}
}
