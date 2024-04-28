using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    private Rigidbody rb;
    private StateManager manager;
    public GameObject dustParticles;
    public Transform carBody;

    [SerializeField] private GameObject projectilePrefab;

    [SerializeField] private int lives;
    [SerializeField] private Transform livesContainer;
    [SerializeField] private GameObject livesIconPrefab;
    public GameObject[] directionIcons; // fwd, back, left, right
    private Vector3 targetPos;
    private int collisionCount = 0;
    private bool isImmune = false;

    private void Awake()
    {
        manager = GameObject.Find("StateManager").GetComponent<StateManager>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void LoseLife()
    {
        if (livesContainer.childCount > 0)
        {
            Destroy(livesContainer.GetChild(0).gameObject);
        } 
        if (livesContainer.childCount <= 0)
        {
            Application.Quit();
        }
    }

    public IEnumerator HaveTemporaryImmunity(float duration = 0.5f)
    {
        isImmune = true;
        yield return new WaitForSeconds(duration);
        isImmune = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isImmune) return;
        if (!collision.collider.CompareTag("Road"))
        {
            manager.cameraFX.StartShake();
            StartCoroutine(HaveTemporaryImmunity());
            manager.DoubleLag();
            collisionCount += 1;
            if (collisionCount == 3)
            {
                collisionCount = 0;
                LoseLife();
            }
        } 
    }

    private void FixedUpdate()
    {
        float speed = rb.velocity.magnitude * GlobalParams.speedScale;
        speed += acceleration * Time.fixedDeltaTime * GlobalParams.speedScale;

        if (speed > maxSpeed) speed = maxSpeed;

        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out RaycastHit hit, 100))
        {
            if (manager.CurrentState != StateManager.STATE.ATTACK && manager.CurrentState != StateManager.STATE.EVALUATE)
            {
                targetPos = hit.point;
                targetPos.y = rb.position.y;
            }
            rb.MovePosition(Vector3.MoveTowards(transform.position, targetPos, speed * Time.fixedDeltaTime * GlobalParams.speedScale));
        }

    }

    void Update()
    {
        // UpdateDirectionVector();
        if (Physics.Raycast(carBody.transform.position, Vector3.down, out RaycastHit hit, 3f))
        {
            if (hit.collider.CompareTag("Road")) dustParticles.SetActive(true);
            else dustParticles.SetActive(false);
        }
    }
}
