using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float fwdMaxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float sideMaxSpeed;
    private Rigidbody rb;
    private StateManager manager;

    [SerializeField] private GameObject projectilePrefab;

    private void Awake()
    {
        manager = GameObject.Find("StateManager").GetComponent<StateManager>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (manager.currentState != StateManager.STATE.DRIVE)
        {
            if (manager.currentState != StateManager.STATE.EVALUATE)
            {
                rb.velocity = Vector3.zero;
            }
            return;
        }

        Vector3 direction = Vector3.zero;
        direction.x = Input.GetAxis("Vertical");
        direction.z = -Input.GetAxis("Horizontal");
        direction.Normalize();

        Vector3 velocity = rb.velocity;
        velocity += direction * acceleration * Time.fixedDeltaTime;

        if (velocity.x > fwdMaxSpeed) velocity.x = fwdMaxSpeed;
        else if (velocity.x < -fwdMaxSpeed) velocity.x = -fwdMaxSpeed;
        if (velocity.z > sideMaxSpeed) velocity.z = sideMaxSpeed;
        else if (velocity.z < -sideMaxSpeed) velocity.z = -sideMaxSpeed;
        rb.velocity = velocity;
    }

    IEnumerator AttackBash()
    {
        Vector3 bashForce = new Vector3(0, 0, 25f);
        bool directionPicked = false;
        // tooltip to pick direction

        while (!directionPicked)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { directionPicked = true; rb.AddForce(bashForce, ForceMode.VelocityChange); }
            else if (Input.GetKeyDown(KeyCode.RightArrow)) { directionPicked = true; rb.AddForce(-bashForce, ForceMode.VelocityChange); }
            yield return null;
        }
        
        yield return new WaitForSecondsRealtime(0.4f);
        rb.velocity = Vector3.zero; rb.angularVelocity = Vector3.zero;
        yield return new WaitForSecondsRealtime(1.0f);
        manager.currentState = StateManager.STATE.DRIVE;
    }

    IEnumerator AttackLaunchProjectile()
    {
        bool directionPicked = false;
        
        while (!directionPicked)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) { directionPicked = true; Instantiate(projectilePrefab, transform.position, Quaternion.identity, transform); }
            else if (Input.GetKeyDown(KeyCode.DownArrow)) { 
                directionPicked = true;
                GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity, transform);
                projectile.GetComponent<ProjectileBehavior>().projectileSpeed *= -1;
            }
            yield return null;
        }
        
        yield return new WaitForSecondsRealtime(4.0f);
        manager.currentState = StateManager.STATE.DRIVE;
    }

    void Update()
    {
        Vector3 currentPos = transform.position;

        if (currentPos.x > GlobalParams.boundaryPosX)
        {
            currentPos.x = GlobalParams.boundaryPosX;
            rb.velocity = Vector3.zero;
        }
        else if (currentPos.x < -GlobalParams.boundaryPosX)
        {
            currentPos.x = -GlobalParams.boundaryPosX;
            rb.velocity = Vector3.zero;
        }

        if (currentPos.z > GlobalParams.boundaryPosZ)
        {
            currentPos.z = GlobalParams.boundaryPosZ;
            rb.velocity = Vector3.zero;
        }
        else if (currentPos.z < -GlobalParams.boundaryPosZ)
        {
            currentPos.z = -GlobalParams.boundaryPosZ;
            rb.velocity = Vector3.zero;
        }

        transform.position = currentPos;

        if (manager.currentState == StateManager.STATE.ATTACK) {
            if (Input.GetKeyDown(KeyCode.U))
            {
                print("do projectile attack");
                StartCoroutine(AttackLaunchProjectile());
                manager.currentState = StateManager.STATE.EVALUATE;
            } else if (Input.GetKeyDown(KeyCode.I))
            {
                print("do side bash attack");
                StartCoroutine(AttackBash());
                manager.currentState = StateManager.STATE.EVALUATE;
            }
        }
    }
}
