using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    // consider making a base class that can be overridden for controls

    private Rigidbody rb;
    // [SerializeField] private float fwdMaxSpeed;
    [SerializeField] private float sideMaxSpeed;
    private StateManager manager;

    // 
    private Vector3 currentSpeed;

    private void Awake()
    {
        manager = GameObject.Find("StateManager").GetComponent<StateManager>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        currentSpeed = new Vector3(0, 0, sideMaxSpeed);
    }

    private void FixedUpdate()
    {
        if (manager.currentState != StateManager.STATE.DRIVE)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        rb.velocity = currentSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player")) currentSpeed = -currentSpeed;
    }


    void Update()
    {
        Vector3 currentPos = transform.position;

        /*        if (currentPos.x > GlobalParams.boundaryPosX) currentPos.x = GlobalParams.boundaryPosX;
                else if (currentPos.x < -GlobalParams.boundaryPosX) currentPos.x = -GlobalParams.boundaryPosX;*/
        if (currentPos.z > GlobalParams.boundaryPosZ)
        {
            currentPos.z = GlobalParams.boundaryPosZ;
            currentSpeed = -currentSpeed;
        }
        else if (currentPos.z < -GlobalParams.boundaryPosZ)
        {
            currentPos.z = -GlobalParams.boundaryPosZ;
            currentSpeed = -currentSpeed;
        }
        transform.position = currentPos;
    }
}
