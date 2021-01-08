using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AICombat : MonoBehaviour
{
    public enum State
    {
        Idle,
        Charge,
        Attack,
        None
    }

    [Header("Attack Settings")]
    public float attackDistance = 2f;
    public float attackRate = 1f;
    [Header("Movement Settings")]
    public float chargeDistance = 8f;
    public float turnSpeed = 10f;
    public float moveSpeed = 3f;

    private Transform target;

    private Animator anim;
    private NavMeshAgent agent;

    private float currentAttackTime;

    private State currentState = State.None;

    private List<GameObject> attackList = new List<GameObject>();

    float targetDistance;
    private void Awake()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Movement>().enabled = false;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        agent.avoidancePriority = 1;
        agent.stoppingDistance = attackDistance;
        if (gameObject.tag == "Enemy")
        {
            GameController.instance.enemyList.Add(this.gameObject);
        }
        else if (gameObject.tag == "Player")
        {
            GameController.instance.armyList.Add(this.gameObject);
        }

        UpdateList();
    }

    // Update is called once per frame
    void Update()
    {
        SetTarget();
        SetState();
        ExecuteState();
    }

    private void OnDisable()
    {
        if (gameObject.tag == "Enemy")
        {
            GameController.instance.enemyList.Remove(this.gameObject);
        }
        else if (gameObject.tag == "Player")
        {
            GameController.instance.armyList.Remove(this.gameObject);
        }
    }

    void UpdateList()
    {
        if (this.gameObject.tag == "Enemy")
        {
            attackList = GameController.instance.armyList;
        }
        else if (this.gameObject.tag == "Player")
        {
            attackList = GameController.instance.enemyList;
        }
    }

    void SetTarget()
    {
        targetDistance = float.MaxValue;

        foreach (GameObject army in attackList)
        {
            float distanceToEnemy = Vector3.Distance(army.transform.position, this.transform.position);

            if (distanceToEnemy < targetDistance)
            {
                targetDistance = distanceToEnemy;
                target = army.transform;
            }
        }
    }

    void SetState()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (distanceToTarget <= chargeDistance && distanceToTarget > attackDistance)
        {
            currentState = State.Charge;
        }
        else if (distanceToTarget <= attackDistance)
        {
            currentState = State.Attack;
        }
        else if (distanceToTarget > attackDistance && distanceToTarget > chargeDistance)
        {
            currentState = State.Idle;
        }

    }

    void ExecuteState()
    {
        switch (currentState)
        {
            case State.Idle:
                target = null;
                anim.SetBool("Run", false);
                agent.isStopped = true;
                break;
            case State.Charge:
                Charge();
                break;
            case State.Attack:
                Attack();
                break;
            case State.None:
                break;
        }
    }

    void Attack()
    {
        if (target != null && currentState != State.Charge || currentState != State.Idle)
        {
            agent.isStopped = true;
            anim.SetBool("Run", false);
            LookAtTheEnemy();
            if (currentAttackTime >= attackRate)
            {
                print("Attack");
                anim.SetTrigger("Attack");
                currentAttackTime = 0f;
            }
            else
            {
                currentAttackTime += Time.deltaTime;
            }
        }
    }
    void Charge()
    {
        if (target != null && currentState != State.Attack || currentState != State.Idle)
        {
            print("Charge");
            LookAtTheEnemy();
            anim.SetBool("Run", true);
            agent.isStopped = false;
            agent.speed = moveSpeed;
            agent.SetDestination(target.position);
        }
    }

    void LookAtTheEnemy()
    {
        Vector3 targetLookPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetLookPosition - transform.position),
            turnSpeed * Time.deltaTime);
    }
}
