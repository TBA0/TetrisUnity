using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ZoomScroll : MonoBehaviour
{
    public GameObject canvas;

	void Update()
    {
		if (Mouse.current.scroll.ReadValue().y != 0)
		{
			canvas.transform.localScale += new Vector3(1, 1, 1);
		}
    }
}
