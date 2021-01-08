using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Movement : MonoBehaviour
{
    
    private static float runSpeed = 175f;
    private float localPlayerZ;
    public static Vector3 moveForce;
    public static bool onTheEdge = false;
    private Animator anim;

    private bool canMove;
    internal bool startMove;
    Rigidbody rb;

    private Vector3 lastMousePos;
    private Vector3 lastTransform;

    public float sensitivity = 0.16f, clampDelta = 42f;
    public float turnSpeed = 15;
    private float getPositionSpeed = 15f;
    public float maxX = 3.7f;

    public float minXClaimOffset = 0.3f;
    public float maxXClaimOffset = 1f;

    public float minZClaimOffset = -1.5f;
    public float maxZClaimOffset = 1.5f;


    public float zOffset = 0.3f;

    Vector3 targetPos;


    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        GetComponent<AICombat>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<NavMeshAgent>().enabled = false;

    }
    private void Start()
    {
       GameController.instance.armyList.Add(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {

        if (TouchButton.IsPressing())
        {
            canMove = true;
            anim.SetBool("Run", true);
        }
        else
        {
            canMove = false;
            anim.SetBool("Run", false);
        }

        if (canMove)
        {
            if (this.gameObject == GameController.instance.armyList[0].gameObject)
            {
                startMove = true;
            }

            if (startMove)
            {
                localPlayerZ += transform.position.z;
                transform.position = new Vector3(transform.position.x, transform.position.y, (transform.position.z) + runSpeed * 0.025f * Time.deltaTime);
            }



        }


        TurnThePlayer();

    }

    private void FixedUpdate()
    {

        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;
            lastTransform = transform.position;
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 direction = lastMousePos - Input.mousePosition;
            lastMousePos = Input.mousePosition;
            lastTransform = transform.position;
            direction = new Vector3(direction.x, 0, 0);
            moveForce = Vector3.ClampMagnitude(direction, clampDelta);

            if (transform.position.x >= maxX)
            {
                if (moveForce.x < 0)
                {
                    //right
                    onTheEdge = true;
                }
                else
                {
                    onTheEdge = false;
                }
            }
            else if (transform.position.x <= -maxX)
            {
                if (moveForce.x > 0)
                {
                    //left
                    onTheEdge = true;
                }
                else
                {
                    onTheEdge = false;
                }
            }

            if (!onTheEdge)
            {
                rb.AddForce((-moveForce * sensitivity - rb.velocity / 5f), ForceMode.VelocityChange);
            }

        }

    }
    private void OnDisable()
    {
        if (!GameController.startFight)
        {
            GameController.instance.armyList.Remove(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Natural")
        {
            // AddClaimOffset(other);
            other.gameObject.name = "Army Member";
            other.gameObject.GetComponent<Movement>().enabled = true;
            other.gameObject.GetComponent<Movement>().startMove = true;
            other.gameObject.GetComponent<PlayerSkin>().SetMaterial(GameController.instance.blue);
            other.gameObject.gameObject.tag = "Player";
        }
    }

    void TurnThePlayer()
    {
        if (!onTheEdge)
        {
            if (transform.position.x > lastTransform.x && canMove)
            {
                //right
                print("Right");
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 18, 0), Time.deltaTime * turnSpeed);
            }
            else if (transform.position.x < lastTransform.x && canMove)
            {
                //left
                print("Left");
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, -18, 0), Time.deltaTime * turnSpeed);
            }
            else if (transform.position.x == lastTransform.x)
            {
                //midle
                print("Midle");
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * turnSpeed);
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * turnSpeed);
            }
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }
    }
    
    void AddClaimOffset(Collision other)
    {

        if (!onTheEdge)
        {
            if (other.transform.position.x > transform.position.x)
            {
                //on the right
                zOffset = (float)Random.Range(minZClaimOffset, maxZClaimOffset);

                targetPos = Vector3.Lerp(transform.position, new Vector3(other.transform.localPosition.x + (float)Random.Range(minXClaimOffset, maxXClaimOffset),
                  other.transform.position.y, other.transform.position.z + zOffset), getPositionSpeed * Time.deltaTime);

                other.transform.position = targetPos;
            }
            else if (other.transform.position.x < transform.position.x)
            {
                //on the left
                zOffset = (float)Random.Range(minZClaimOffset, maxZClaimOffset);

                targetPos = Vector3.Lerp(transform.position, new Vector3(other.transform.localPosition.x + (float)Random.Range(-minXClaimOffset, -maxXClaimOffset),
                    other.transform.position.y, other.transform.position.z + zOffset), getPositionSpeed * Time.deltaTime);

                other.transform.position = targetPos;
            }
            other.gameObject.GetComponent<Movement>().startMove = true;
        }
        else
        {

            if (other.transform.position.x > transform.position.x)
            {
                //on the right
                zOffset = (float)Random.Range(minZClaimOffset, maxZClaimOffset);

                targetPos = new Vector3(other.transform.localPosition.x + (float)Random.Range(-minXClaimOffset, -maxXClaimOffset),
                     other.transform.position.y, other.transform.position.z + zOffset);

                other.transform.position = targetPos;
            }
            else if (other.transform.position.x < transform.position.x)
            {
                //on the left
                zOffset = (float)Random.Range(minZClaimOffset, maxZClaimOffset);

                targetPos = new Vector3(other.transform.localPosition.x + (float)Random.Range(minXClaimOffset, maxXClaimOffset),
                    other.transform.position.y, other.transform.position.z + zOffset);

                other.transform.position = targetPos;
            }
            other.gameObject.GetComponent<Movement>().startMove = true;
        }
    }
}
