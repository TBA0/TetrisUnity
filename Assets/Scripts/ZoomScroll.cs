using UnityEngine;
using UnityEngine.InputSystem;

public class ZoomScroll : MonoBehaviour
{
    public RectTransform canvas;

	void Update()
    {
		if (Mouse.current.scroll.ReadValue().y != 0)
		{
			Debug.Log(Mouse.current.scroll.ReadValue().y);
			canvas.sizeDelta += new Vector2(100, 100);
		}
	}
}
