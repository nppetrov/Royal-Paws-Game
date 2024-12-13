using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class GlobalButtonHover : MonoBehaviour
{
	private void Start()
	{
		ApplyHoverEffectToAllButtons();
	}

	private void ApplyHoverEffectToAllButtons()
	{
		Button[] buttons = FindObjectsByType<Button>(FindObjectsSortMode.None);
		foreach (Button button in buttons)
		{
			EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
			if (trigger == null)
			{
				trigger = button.gameObject.AddComponent<EventTrigger>();
			}

			AddHoverEvent(trigger, button.transform);
		}
	}

	private void AddHoverEvent(EventTrigger trigger, Transform buttonTransform)
	{
		Vector3 originalScale = buttonTransform.localScale;

		EventTrigger.Entry pointerEnter = new EventTrigger.Entry
		{
			eventID = EventTriggerType.PointerEnter
		};
		pointerEnter.callback.AddListener((data) =>
			buttonTransform.DOScale(originalScale * 1.1f, 0.2f).SetEase(Ease.OutBack)
		);
		trigger.triggers.Add(pointerEnter);

		EventTrigger.Entry pointerExit = new EventTrigger.Entry
		{
			eventID = EventTriggerType.PointerExit
		};
		pointerExit.callback.AddListener((data) =>
			buttonTransform.DOScale(originalScale, 0.2f).SetEase(Ease.OutBack)
		);
		trigger.triggers.Add(pointerExit);
	}
}
