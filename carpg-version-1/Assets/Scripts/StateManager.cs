using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StateManager : MonoBehaviour
{
    public enum STATE { DRIVE, ATTACK, EVALUATE }
    public STATE currentState {
        get
        {
            return _currentState;
        }
        set 
        { 
            if (value == STATE.DRIVE) timer = countdown;
            _currentState = value;
            phaseTextbox.text = currentState.ToString();
        }
    
    }
    [SerializeField] private Image gauge;
    [SerializeField] private TextMeshProUGUI phaseTextbox;

    [SerializeField] private float countdown;
    private float timer;
    private static STATE _currentState;

    void Start()
    {
        timer = countdown;
        currentState = STATE.DRIVE;
        phaseTextbox.text = currentState.ToString();
    }

    void Update()
    {
        if (currentState == STATE.DRIVE)
        {
            timer -= Time.deltaTime;
            gauge.fillAmount = Mathf.Lerp(0, 1, 1 - timer / countdown);
            if (timer < 0)
            {
                currentState = STATE.ATTACK;
            }
        } else if (currentState == STATE.ATTACK)
        {
            // wait for action
        } else if (currentState == STATE.EVALUATE)
        {
            // wait for animations to finish
        }
    }
}
