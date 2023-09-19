using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCursor : MonoBehaviour
{
    void Start()
    {
		if (!Cursor.visible)
		{
			Cursor.visible = true;
		}
	}
	private void Awake()
	{
		if (!Cursor.visible)
		{
			Cursor.visible = true;
		}
	}
}
