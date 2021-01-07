using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public static float runSpeed = 175f;
    public static float playerZ;
    public static Vector3 moveForce;
    public static bool onTheEdge = false;
    private Animator anim;

    private bool canMove;
    Rigidbody rb;

    private Vector3 lastMousePos;
    private Vector3 lastTransform;
    public float sensitivity = 0.16f, clampDelta = 42f;
    public float turnSpeed = 15;

    public float maxX = 3.7f;

 
    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (TouchButton.IsPressing())
        {
            canMove = true;
        }
        else
        {
            canMove = false;
        }

        if (canMove)
        {

            playerZ += runSpeed * 0.025f * Time.deltaTime;

            transform.position = new Vector3(transform.position.x, transform.position.y, playerZ);

            if (transform.position.x > lastTransform.x)
            {
                //right
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 15, 0), turnSpeed * Time.deltaTime);
            }
            else if (transform.position.x < lastTransform.x)
            {
                //left
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -15, 0), turnSpeed * Time.deltaTime);
            }
            else if (transform.position.x == lastTransform.x)
            {
                //midle
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), turnSpeed * Time.deltaTime);
            }
        }

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

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.tag == "Natural")
        {
            other.collider.gameObject.GetComponent<Movement>().enabled = true;
            other.collider.gameObject.GetComponent<PlayerColor>().SetMaterial(GameController.instance.blue);
            other.collider.gameObject.gameObject.tag = "Player";
        }
    }
}
