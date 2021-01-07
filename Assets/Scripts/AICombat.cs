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
    private void Awake()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Movement>().enabled = false;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        agent.avoidancePriority = Random.Range(1, 51);

        if (gameObject.tag == "Enemy")
        {
            GameController.instance.enemyList.Add(this.gameObject);
        }
        else if (gameObject.tag == "Player")
        {
            GameController.instance.armyList.Add(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
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

    void SetState()
    {
        if (this.gameObject.tag == "Enemy")
        {
            attackList = GameController.instance.armyList;
        }
        else if (this.gameObject.tag == "Player")
        {
            attackList = GameController.instance.enemyList;
        }

        foreach (GameObject army in attackList)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, army.transform.position);

            if (distanceToEnemy > attackDistance && distanceToEnemy <= chargeDistance)
            {
                currentState = State.Charge;
                target = null;
                target = army.transform;
            }
            else if (distanceToEnemy <= attackDistance + 0.15f)
            {
                if (target == null)
                {
                    target = army.transform;
                }
                else
                {
                    currentState = State.Attack;
                }
            }
            else
            {
                currentState = State.Idle;
            }
        }
    }

    void ExecuteState()
    {
        switch (currentState)
        {
            case State.Idle:
                agent.isStopped = true;
                target = null;
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
        if (target != null)
        {
            agent.isStopped = true;
            anim.SetBool("Run", false);
            Vector3 targetLookPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetLookPosition - transform.position),
                turnSpeed * Time.deltaTime);
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
        if (target != null)
        {
            print("Charge");
            anim.SetBool("Run", true);
            agent.isStopped = false;
            agent.speed = moveSpeed;
            agent.SetDestination(target.position);
        }
    }
}
