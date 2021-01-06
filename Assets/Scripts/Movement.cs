using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public static float runSpeed = 175f;
    public static float slideSpeed = 50f;
    public static float playerZ;
    public static float horizontalMove;

    private Animator anim;

    private bool canMove;
    Rigidbody rb;

    float playerToPointDistance;
    private Vector3 mousePos;
    private Vector3 targetPos;
 
    void Start()
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
            mousePos = Input.mousePosition;
            mousePos.z = playerZ + 25;
            var temp = Camera.main.ScreenToWorldPoint(mousePos);

            playerZ += runSpeed * 0.025f * Time.deltaTime;
            
            if (Mathf.Abs(temp.x - transform.position.x) <= 0.7f)
            {
                horizontalMove = transform.position.x;
            }
            else
            {
                horizontalMove += Mathf.Sign(temp.x) * slideSpeed * 0.025f * Time.deltaTime;
            }

            targetPos = new Vector3(horizontalMove, transform.position.y, playerZ);

            transform.position = targetPos;
        }

    }
}
