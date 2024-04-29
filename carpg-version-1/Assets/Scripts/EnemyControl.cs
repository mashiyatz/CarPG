using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
 
    private Rigidbody rb;
    [SerializeField] private float sideMaxSpeed;
    [SerializeField] private float fwdMaxSpeed;
    private StateManager manager;
    [SerializeField] private GameObject projectilePrefab;
    // 
    public Vector3 currentSpeed;
    private bool isAttacking;
    private EnemySpawner spawner;

    private void Awake()
    {
        manager = GameObject.Find("StateManager").GetComponent<StateManager>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spawner = transform.parent.GetComponent<EnemySpawner>();
        isAttacking = false;
        // currentSpeed = new Vector3(fwdMaxSpeed, 0, sideMaxSpeed);
    }

    private void FixedUpdate()
    {
        rb.velocity = currentSpeed * GlobalParams.speedScale;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Enemy"))
        {
            currentSpeed = -currentSpeed;
            // if (manager.CurrentState == StateManager.STATE.EVALUATE) Destroy(gameObject);
        } else if (collision.transform.CompareTag("Bound"))
        {
            if (collision.transform.name == "Side") currentSpeed.z *= -1;
            if (collision.transform.name == "Front") currentSpeed.x *= -1;
        } else if (collision.collider.CompareTag("Obstacle"))
        {
            Destroy(gameObject); 
        } 


    }

    private int CoinFlip()
    {
        if (Random.Range(0, 100) > 50) return 1; else return -1;
    }

    private int ChooseShotDirection()
    {
        if (transform.position.x > spawner.player.transform.position.x) return -1;
        else return 1;
    }

    IEnumerator AttackLaunchProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity, transform);
        projectile.GetComponent<ProjectileBehavior>().projectileSpeed *= ChooseShotDirection();
        
        yield return new WaitForSecondsRealtime(4.0f);
        
    }

    void Update()
    {
        // attack
        if (transform.position.x > -20 && !GetComponent<Collider>().enabled)
        {
            GetComponent<Collider>().enabled = true;
            currentSpeed = new Vector3(Random.Range(fwdMaxSpeed/2, fwdMaxSpeed), 0, Random.Range(sideMaxSpeed/2, sideMaxSpeed));
        }

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
