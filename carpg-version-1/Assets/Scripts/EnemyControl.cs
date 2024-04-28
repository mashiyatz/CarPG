using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    // consider making a base class that can be overridden for controls

    private Rigidbody rb;
    [SerializeField] private float sideMaxSpeed;
    private StateManager manager;
    [SerializeField] private GameObject projectilePrefab;
    // 
    private Vector3 currentSpeed;
    private bool isAttacking;

    private void Awake()
    {
        manager = GameObject.Find("StateManager").GetComponent<StateManager>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isAttacking = false;
        currentSpeed = new Vector3(0, 0, sideMaxSpeed);
    }

    private void FixedUpdate()
    {
        rb.velocity = currentSpeed * GlobalParams.speedScale;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            currentSpeed = -currentSpeed;
            // note that bullet goes after bash -- is this worth keeping?
            if (manager.CurrentState == StateManager.STATE.EVALUATE) Destroy(gameObject);
        } else if (collision.transform.CompareTag("Bound"))
        {
            currentSpeed = -currentSpeed;
        }


    }

    private int CoinFlip()
    {
        if (Random.Range(0, 100) > 50) return 1; else return -1;
    }

    IEnumerator AttackLaunchProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity, transform);
        projectile.GetComponent<ProjectileBehavior>().projectileSpeed *= CoinFlip();
        
        yield return new WaitForSecondsRealtime(4.0f);
        
    }

        void Update()
    {
        // attack

        if (manager.CurrentState == StateManager.STATE.WAIT && !isAttacking)
        {
            StartCoroutine(AttackLaunchProjectile());
            isAttacking = true;
        }
        else if (manager.CurrentState == StateManager.STATE.DRIVE && isAttacking)
        {
            isAttacking = false;
        }

        // 
    }
}
