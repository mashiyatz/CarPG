using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PassengerControl : MonoBehaviour
{
    public GlobalParams.Archetype passengerType;
    public GameObject projectilePrefab;
    public Coroutine attack;
    private StateManager manager;
    private PlayerControl player;

    private void Awake()
    {
        manager = GameObject.Find("StateManager").GetComponent<StateManager>();
        player = GameObject.Find("PlayerCar").GetComponent<PlayerControl>();
        GetComponent<Image>().color = GlobalParams.typeToColor.GetValueOrDefault(passengerType);
    }

    private void Start()
    {
        if (transform.GetSiblingIndex() == 0) passengerType = GlobalParams.Archetype.DRIVER;
        GetComponent<Image>().color = GlobalParams.typeToColor.GetValueOrDefault(passengerType);
    }

    public void SetPlayer(PlayerControl p)
    {
        player = p;
    }

    public void SetPassengerType(GlobalParams.Archetype type)
    {
        passengerType = type;
        GetComponent<Image>().color = GlobalParams.typeToColor.GetValueOrDefault(type);
    }

    public void Attack()
    {
        manager.CurrentState = StateManager.STATE.EVALUATE;
        attack = StartCoroutine(GlobalParams.typeToAttack.GetValueOrDefault(passengerType));
    }

    private void Update()
    {
        if (transform.GetSiblingIndex() == 0 && passengerType != GlobalParams.Archetype.DRIVER)
        {
            passengerType = GlobalParams.Archetype.DRIVER;
            GetComponent<Image>().color = GlobalParams.typeToColor.GetValueOrDefault(passengerType);
        }
    }

    /// Attacks

    IEnumerator Bullet()
    {
        GlobalParams.attackDirection = GlobalParams.Direction.None;
        player.directionIcons[0].SetActive(true);
        player.directionIcons[1].SetActive(true);

        while (GlobalParams.attackDirection == GlobalParams.Direction.None) yield return null;

        if (GlobalParams.attackDirection == GlobalParams.Direction.FORWARD) Instantiate(projectilePrefab, player.transform.position, Quaternion.identity, player.transform); 
        else if (GlobalParams.attackDirection == GlobalParams.Direction.BACK)
        {
            GameObject projectile = Instantiate(projectilePrefab, player.transform.position, Quaternion.identity, player.transform);
            projectile.GetComponent<ProjectileBehavior>().projectileSpeed *= -1;
        }

        player.directionIcons[0].SetActive(false);
        player.directionIcons[1].SetActive(false);
        manager.CurrentState = StateManager.STATE.WAIT;
        yield return new WaitForSecondsRealtime(0.25f);
        manager.CurrentState = StateManager.STATE.DRIVE;
        
    }

    IEnumerator Bash()
    {
        Vector3 bashForce = new Vector3(0, 0, 25f);
        GlobalParams.attackDirection = GlobalParams.Direction.None;
        player.directionIcons[2].SetActive(true);
        player.directionIcons[3].SetActive(true);


        while (GlobalParams.attackDirection == GlobalParams.Direction.None) yield return null;

        if (GlobalParams.attackDirection == GlobalParams.Direction.LEFT) player.GetComponent<Rigidbody>().AddForce(bashForce, ForceMode.VelocityChange);
        else if (GlobalParams.attackDirection == GlobalParams.Direction.RIGHT) player.GetComponent<Rigidbody>().AddForce(-bashForce, ForceMode.VelocityChange);

        player.directionIcons[2].SetActive(false);
        player.directionIcons[3].SetActive(false);
        yield return new WaitForSecondsRealtime(0.4f);
        player.GetComponent<Rigidbody>().velocity = Vector3.zero; player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        manager.CurrentState = StateManager.STATE.WAIT;
        yield return new WaitForSecondsRealtime(0.25f);
        manager.CurrentState = StateManager.STATE.DRIVE;     
    }

    IEnumerator Jump()
    {
        Animator anim = player.GetComponent<Animator>();
        anim.Play("Jump");
        StartCoroutine(player.HaveTemporaryImmunity(1.0f));
        manager.CurrentState = StateManager.STATE.WAIT;
        yield return new WaitForSecondsRealtime(0.25f);
        manager.CurrentState = StateManager.STATE.DRIVE;
    }


}
