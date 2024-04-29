using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteBehavior : MonoBehaviour
{
    public Vector3 destination = Vector3.zero;
    private Rigidbody rb;
    [SerializeField] float speed;
    [SerializeField] GameObject blastRadius;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * 15f, ForceMode.VelocityChange);
    }

    void FixedUpdate()
    {
        if (destination == Vector3.zero) return;
        rb.MovePosition(Vector3.MoveTowards(transform.position, destination, speed * Time.fixedDeltaTime * GlobalParams.speedScale));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Road"))
        {
            // Vector3 pos = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            // Debug.Log(pos);
            // StartCoroutine(DelayExplosion(pos - Vector3.up * 0.5f)); // have to account for object size
            StartCoroutine(DelayExplosion()); // have to account for object size
        }   
    }

    IEnumerator DelayExplosion()
    {
        yield return new WaitForSeconds(0.25f);
        Vector3 dynamitePos = transform.position;
        dynamitePos.y = 0;
        Instantiate(blastRadius, dynamitePos, Quaternion.identity);
        Destroy(gameObject);
    }
}
