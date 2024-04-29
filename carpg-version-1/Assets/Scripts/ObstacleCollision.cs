using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = -Vector3.right * 10f;
        
    }

    private void FixedUpdate()
    {
        // rb.position = 2f * -Vector3.right * Time.fixedDeltaTime;
        rb.MovePosition(Vector3.MoveTowards(rb.position, rb.position - Vector3.right, GlobalParams.speedScale * 25f * Time.fixedDeltaTime));
        if (rb.position.x < -25) Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("Projectile"))
        {
            Destroy(gameObject);
        }
    }
}
