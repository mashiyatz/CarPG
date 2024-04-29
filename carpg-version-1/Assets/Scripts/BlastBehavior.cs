using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastBehavior : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
        Destroy(gameObject, 0.25f);
    }
}
