using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Experimental.GraphView;

public class StateManager : MonoBehaviour
{
    public enum STATE { DRIVE, ATTACK, EVALUATE, WAIT }
    public Transform buttonParent;
    private float lagScale = 1.0f;
    [SerializeField] private Renderer roadRenderer;
    public CameraShake cameraFX;

    public STATE CurrentState
    {
        get
        {
            return _currentState;
        }
        set 
        {
            if (value == STATE.DRIVE)
            {
                timer = countdown;
                lagScale = 1.0f;
            }


            if (value == STATE.ATTACK)
            {
                foreach (Button button in buttonParent.GetComponentsInChildren<Button>()) button.enabled = true;
                timer = slowMotionCountdown;
            }
            else foreach (Button button in buttonParent.GetComponentsInChildren<Button>()) button.enabled = false;


            if (value == STATE.ATTACK || value == STATE.EVALUATE)
            {
                GlobalParams.speedScale = 0.2f;
                roadRenderer.material.SetFloat("_lagScale", 0.5f);
            }
            else
            {
                GlobalParams.speedScale = 1.0f;
                roadRenderer.material.SetFloat("_lagScale", 10);
            }
           
            _currentState = value;
            Debug.Log(value);
            // phaseTextbox.text = CurrentState.ToString();
        }
    
    }
    [SerializeField] private Image gauge;
    // [SerializeField] private TextMeshProUGUI phaseTextbox;

    [SerializeField] private float countdown;
    [SerializeField] private float slowMotionCountdown;
    private float timer;
    private static STATE _currentState;

    void Start()
    {
        // Camera.main.eventMask = inputLayerMask;
        roadRenderer.material.SetFloat("_lagScale", 10);
        cameraFX = GetComponent<CameraShake>();
        timer = countdown;
        CurrentState = STATE.DRIVE;
        // phaseTextbox.text = CurrentState.ToString();
    }

    public void DoubleLag()
    {
        lagScale *= 0.5f;
    }

    public void ChangeAttackDirection(GlobalParams.Direction dir)
    {
        // 1 is FWD 
        // 2 is BACK
        // 3 is RIGHT
        // 4 is LEFT
        // 0 is None
        GlobalParams.attackDirection = dir;
    }

    void Update()
    {
        if (CurrentState == STATE.DRIVE)
        {
            timer -= Time.deltaTime * lagScale;
            gauge.fillAmount = Mathf.Lerp(0, 1, 1 - timer / countdown);
            if (timer < 0)
            {
                CurrentState = STATE.ATTACK;
            }
        } else if (CurrentState == STATE.ATTACK)
        {
            timer -= Time.deltaTime;
            gauge.fillAmount = Mathf.Lerp(1, 0, 1 - timer / slowMotionCountdown);
            if (timer < 0)
            {
                CurrentState = STATE.WAIT;
            }
            // wait for action
        } else if (CurrentState == STATE.EVALUATE)
        {
            timer -= Time.deltaTime;
            gauge.fillAmount = Mathf.Lerp(1, 0, 1 - timer / slowMotionCountdown);
            if (timer < 0)
            {
                CurrentState = STATE.WAIT;
            }
        } else if (CurrentState == STATE.WAIT)
        {

        }
    }
}
