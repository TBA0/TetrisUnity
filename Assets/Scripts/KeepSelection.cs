using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeepSelection : MonoBehaviour
{
	GameObject lastSelected;

	void Update()
	{
		if (EventSystem.current.currentSelectedGameObject == null)
		{
			if (lastSelected.gameObject.activeSelf && lastSelected.GetComponent<Button>() != null && lastSelected.GetComponent<Button>().interactable)
			{
				EventSystem.current.SetSelectedGameObject(lastSelected);
			}
		}
		else
		{
			lastSelected = EventSystem.current.currentSelectedGameObject;
		}
	}
}
