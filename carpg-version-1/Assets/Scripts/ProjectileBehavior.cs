using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    private Rigidbody rb;
    public float projectileSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.right * projectileSpeed;
        StartCoroutine(DestroyAfterWait());
    }

    IEnumerator DestroyAfterWait()
    {
        yield return new WaitForSeconds(4.0f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        
    }

    void Update()
    {
        
    }
}
