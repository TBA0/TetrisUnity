using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class Menu : MonoBehaviour
{
    private int OptionsIndex = 0;
    public UnityEngine.UI.Text Arrow { get; set; }
    private Input m_PlayerInput;
    private ButtonControl b_Submit;
    private ButtonControl b_Move;

    private void Awake()
    {
        m_PlayerInput = new Input();
        m_PlayerInput.Enable();

        b_Submit = (ButtonControl)m_PlayerInput.UI.Submit.controls[0];
        b_Move = (ButtonControl)m_PlayerInput.UI.Navigate.controls[0];
        Arrow = GetComponentsInChildren<UnityEngine.UI.Text>()[0];
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((m_PlayerInput.UI.Submit.activeControl != null) && ((ButtonControl)m_PlayerInput.UI.Submit.activeControl != b_Submit))
        {
            b_Submit = (ButtonControl)m_PlayerInput.UI.Submit.activeControl;
        }
        if ((m_PlayerInput.UI.Navigate.activeControl != null) && ((ButtonControl)m_PlayerInput.UI.Navigate.activeControl != b_Move))
        {
            b_Move = (ButtonControl)m_PlayerInput.UI.Navigate.activeControl;
        }

        if (b_Move.wasPressedThisFrame)
        {
            string str = "";
            if (OptionsIndex < 2) OptionsIndex++;
            else OptionsIndex = 0;
            for (int i = 0; i < OptionsIndex; i++)
            {
                str += "\n\n";
            }
            str += "→";
            Arrow.text = str;
        }
    }
}
