using UnityEngine;
using ca.HenrySoftware;
using ca.HenrySoftware.Rage;
[RequireComponent(typeof(Pool))]
public class Effects : MonoBehaviour
{
	public CanvasGroup GroupText;
	public ModelEffectAnimation Block;
	public ModelEffectAnimation Box;
	public ModelEffectAnimation Bubble;
	public ModelEffectAnimation Circle;
	public ModelEffectAnimation Claw;
	public ModelEffectAnimation Consume;
	public ModelEffectAnimation Dark;
	public ModelEffectAnimation Earth;
	public ModelEffectAnimation Electric;
	public ModelEffectAnimation Explode;
	public ModelEffectAnimation Fire;
	public ModelEffectAnimation Footprints;
	public ModelEffectAnimation Glint;
	public ModelEffectAnimation Heal;
	public ModelEffectAnimation Ice;
	public ModelEffectAnimation Lightning;
	public ModelEffectAnimation Nuclear;
	public ModelEffectAnimation Poison;
	public ModelEffectAnimation Puff;
	public ModelEffectAnimation Shield;
	public ModelEffectAnimation Slash;
	public ModelEffectAnimation Sparks;
	public ModelEffectAnimation SplatterBlood;
	public ModelEffectAnimation SplatterSlime;
	public ModelEffectAnimation Square;
	public ModelEffectAnimation Star;
	public ModelEffectAnimation Teleport;
	public ModelEffectAnimation Touch;
	public ModelEffectAnimation Warp;
	public ModelEffectAnimation Water;
	public ModelEffectAnimation Web;
	Pool _pool;
	Material _materialNormal;
	Material _materialAdditive;
	void Awake()
	{
		_materialNormal = new Material(Shader.Find("Sprites/Default")) { color = Color.white };
		_materialAdditive = new Material(Shader.Find("Custom/Additive")) { color = Color.white };
		_pool = GetComponent<Pool>();
	}
	void Start()
	{
		GroupText.alpha = 0f;
		Begin();
	}
	void Begin()
	{
		Ease.Go(this, 0f, 1f, 1f, (p) => GroupText.alpha = p, Continue0, EaseType.Linear);
	}
	void Continue0()
	{
		Ease.Go(this, 1f, 0f, 1f, (p) => GroupText.alpha = p);
	}
	void Finish()
	{
		StopAllCoroutines();
		Ease.GoAlpha(this, 1f, 0f, 1f, null, null, EaseType.Linear);
		Ease.Go(this, 1f, 0f, 1f, (p) => GroupText.alpha = p, Begin, EaseType.Linear);
	}
	[ContextMenu("TriggerBlock")]
	public void TriggerBlock()
	{
		TriggerBlock(Vector2.zero);
	}
	public void TriggerBlock(Vector2 p)
	{
		Trigger(Block, p);
	}
	[ContextMenu("TriggerBox")]
	public void TriggerBox(bool alternate = false)
	{
		TriggerBox(alternate, Vector2.zero);
	}
	public void TriggerBox(bool alternate, Vector2 p)
	{
		Trigger(Box, p, true, alternate);
	}
	[ContextMenu("TriggerBubble")]
	public void TriggerBubble()
	{
		TriggerBubble(Vector2.zero);
	}
	public void TriggerBubble(Vector2 p)
	{
		Trigger(Bubble, p);
	}
	[ContextMenu("TriggerCircle")]
	public void TriggerCircle()
	{
		TriggerCircle(Vector2.zero);
	}
	public void TriggerCircle(Vector2 p)
	{
		Trigger(Circle, p);
	}
	[ContextMenu("TriggerClaw")]
	public void TriggerClaw(bool alternate = false)
	{
		TriggerClaw(alternate, Vector2.zero);
	}
	public void TriggerClaw(bool alternate, Vector2 p)
	{
		Trigger(Claw, p, true, alternate);
	}
	[ContextMenu("TriggerConsume")]
	public void TriggerConsume(float type = 0f)
	{
		TriggerConsume(type, Vector2.zero);
	}
	public void TriggerConsume(float type, Vector2 p)
	{
		Trigger(Consume, p, false, false, true, type);
	}
	[ContextMenu("TriggerDark")]
	public void TriggerDark()
	{
		TriggerDark(Vector2.zero);
	}
	public void TriggerDark(Vector2 p)
	{
		Trigger(Dark, p);
	}
	[ContextMenu("TriggerEarth")]
	public void TriggerEarth()
	{
		TriggerEarth(Vector2.zero);
	}
	public void TriggerEarth(Vector2 p)
	{
		Trigger(Earth, p);
	}
	[ContextMenu("TriggerElectric")]
	public void TriggerElectric()
	{
		TriggerElectric(Vector2.zero);
	}
	public void TriggerElectric(Vector2 p)
	{
		Trigger(Electric, p);
	}
	[ContextMenu("TriggerExplode")]
	public void TriggerExplode(float type = 0f)
	{
		TriggerExplode(type, Vector2.zero);
	}
	public void TriggerExplode(float type, Vector2 p)
	{
		Trigger(Explode, p, false, false, true, type);
	}
	[ContextMenu("TriggerFire")]
	public void TriggerFire()
	{
		TriggerFire(Vector2.zero);
	}
	public void TriggerFire(Vector2 p)
	{
		Trigger(Fire, p);
	}
	[ContextMenu("TriggerGlint")]
	public void TriggerGlint()
	{
		TriggerGlint(Vector2.zero);
	}
	public void TriggerGlint(Vector2 p)
	{
		Trigger(Glint, p);
	}
	[ContextMenu("TriggerHeal")]
	public void TriggerHeal(bool alternate = false)
	{
		TriggerHeal(alternate, Vector2.zero);
	}
	public void TriggerHeal(bool alternate, Vector2 p)
	{
		Trigger(Heal, p, true, alternate);
	}
	[ContextMenu("TriggerIce")]
	public void TriggerIce()
	{
		TriggerIce(Vector2.zero);
	}
	public void TriggerIce(Vector2 p)
	{
		Trigger(Ice, p);
	}
	[ContextMenu("TriggerLightning")]
	public void TriggerLightning()
	{
		TriggerLightning(Vector2.zero);
	}
	public void TriggerLightning(Vector2 p)
	{
		Trigger(Lightning, p);
	}
	[ContextMenu("TriggerNuclear")]
	public void TriggerNuclear()
	{
		TriggerNuclear(Vector2.zero);
	}
	public void TriggerNuclear(Vector2 p)
	{
		Trigger(Nuclear, p);
	}
	[ContextMenu("TriggerPoison")]
	public void TriggerPoison()
	{
		TriggerPoison(Vector2.zero);
	}
	public void TriggerPoison(Vector2 p)
	{
		Trigger(Poison, p);
	}
	[ContextMenu("TriggerPuff")]
	public void TriggerPuff()
	{
		TriggerPuff(Vector2.zero);
	}
	public void TriggerPuff(Vector2 p)
	{
		Trigger(Puff, p);
	}
	[ContextMenu("TriggerShield")]
	public void TriggerShield()
	{
		TriggerShield(Vector2.zero);
	}
	public void TriggerShield(Vector2 p)
	{
		Trigger(Shield, p);
	}
	[ContextMenu("TriggerSlash")]
	public void TriggerSlash(float type = 0f)
	{
		TriggerSlash(type, Vector2.zero);
	}
	public void TriggerSlash(float type, Vector2 p)
	{
		Trigger(Slash, p, false, false, true, type);
	}
	[ContextMenu("TriggerSparks")]
	public void TriggerSparks()
	{
		TriggerSparks(Vector2.zero);
	}
	public void TriggerSparks(Vector2 p)
	{
		Trigger(Sparks, p);
	}
	[ContextMenu("TriggerSplatterBlood")]
	public void TriggerSplatterBlood()
	{
		TriggerSplatterBlood(Vector2.zero);
	}
	public void TriggerSplatterBlood(Vector2 p)
	{
		Trigger(SplatterBlood, p);
	}
	[ContextMenu("TriggerSplatterSlime")]
	public void TriggerSplatterSlime(Color? color = null)
	{
		TriggerSplatterSlime(color, Vector2.zero);
	}
	public void TriggerSplatterSlime(Color? color, Vector2 p)
	{
		Trigger(SplatterSlime, p, false, false, false, 0f, true, color);
	}
	[ContextMenu("TriggerSquare")]
	public void TriggerSquare(bool alternate = false)
	{
		TriggerSquare(alternate, Vector2.zero);
	}
	public void TriggerSquare(bool alternate, Vector2 p)
	{
		Trigger(Square, p, true, alternate);
	}
	[ContextMenu("TriggerStar")]
	public void TriggerStar()
	{
		TriggerStar(Vector2.zero);
	}
	public void TriggerStar(Vector2 p)
	{
		Trigger(Star, p);
	}
	[ContextMenu("TriggerTeleport")]
	public void TriggerTeleport()
	{
		TriggerTeleport(Vector2.zero);
	}
	public void TriggerTeleport(Vector2 p)
	{
		Trigger(Teleport, p);
	}
	[ContextMenu("TriggerTouch")]
	public void TriggerTouch()
	{
		TriggerTouch(Vector2.zero, null);
	}
	public void TriggerTouch(Vector2 p, Color? color = null)
	{
		Trigger(Touch, p, false, false, false, 0, true, color);
	}
	[ContextMenu("TriggerWarp")]
	public void TriggerWarp()
	{
		TriggerWarp(Vector2.zero);
	}
	public void TriggerWarp(Vector2 p)
	{
		Trigger(Warp, p);
	}
	[ContextMenu("TriggerWater")]
	public void TriggerWater()
	{
		TriggerWater(Vector2.zero);
	}
	public void TriggerWater(Vector2 p)
	{
		Trigger(Water, p);
	}
	[ContextMenu("TriggerWeb")]
	public void TriggerWeb()
	{
		TriggerWeb(Vector2.zero);
	}
	public void TriggerWeb(Vector2 p)
	{
		Trigger(Web, p);
	}
	public static int AnimatorTrigger = Animator.StringToHash("Trigger");
	public static int AnimatorAlternate = Animator.StringToHash("Alternate");
	public static int AnimatorType = Animator.StringToHash("Type");
	public void Trigger(ModelEffectAnimation model, Vector2 p,
		bool setAlternate = false, bool alternate = false,
		bool setType = false, float type = 0f,
		bool setColor = false, Color? color = null)
	{
		var o = _pool.Enter();
		o.transform.localPosition = new Vector3(p.x, p.y, transform.localPosition.z);
		o.transform.localScale = Vector3.one;
		var spriteRenderers = o.GetComponentsInChildren<SpriteRenderer>();
		foreach (var spriteRenderer in spriteRenderers)
		{
			spriteRenderer.color = (setColor && color != null && color.HasValue) ? color.Value : Color.white;
			spriteRenderer.material = model.Blend ? _materialAdditive : _materialNormal;
		}
		spriteRenderers[1].transform.localPosition = model.BackOffset;
		spriteRenderers[1].sortingOrder = -1;
		spriteRenderers[2].transform.localPosition = model.ForeOffset;
		spriteRenderers[2].sortingOrder = 2;
		spriteRenderers[0].transform.localPosition = model.Offset;
		spriteRenderers[0].sortingOrder = 1;
		var animator = o.GetComponentInChildren<Animator>();
		animator.runtimeAnimatorController = model.Controller;
		if (setAlternate)
			animator.SetBool(AnimatorAlternate, alternate);
		if (setType)
			animator.SetFloat(AnimatorType, type);
		animator.SetTrigger(AnimatorTrigger);
	}
	public void Exit(GameObject o)
	{
		// handled by stateRemove when animaion done
		_pool.Exit(o);
	}
}
