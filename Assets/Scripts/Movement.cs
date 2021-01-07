﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private static float runSpeed = 175f;
    public static float playerZ;
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


    private float zOffset;

    Vector3 targetPos;


    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
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
        }
        else
        {
            canMove = false;
        }

        if (canMove)
        {
            if (this.gameObject == GameController.instance.armyList[0].gameObject)
            {
                playerZ += runSpeed * 0.025f * Time.deltaTime;
                startMove = true;
            }

            if (startMove)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, playerZ + zOffset);
            }

            TurnThePlayer();

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
        GameController.instance.armyList.Remove(this.gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.tag == "Natural")
        {
            AddClaimOffset(other);
            other.gameObject.name = "Army Member";
            other.collider.gameObject.GetComponent<Movement>().enabled = true;
            other.collider.gameObject.GetComponent<PlayerSkin>().SetMaterial(GameController.instance.blue);
            other.collider.gameObject.gameObject.tag = "Player";
        }
        if (other.collider.gameObject.tag == "Player")
        {
            targetPos = transform.position;
        }
    }
    void TurnThePlayer()
    {
        if (!onTheEdge)
        {
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

                targetPos =  new Vector3(other.transform.localPosition.x + (float)Random.Range(minXClaimOffset, maxXClaimOffset),
                    other.transform.position.y, other.transform.position.z + zOffset);

                other.transform.position = targetPos;
            }
            other.gameObject.GetComponent<Movement>().startMove = true;
        }
    }
}
