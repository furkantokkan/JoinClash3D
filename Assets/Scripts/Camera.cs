using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public float cameraDistance = 3f;
    public float cameraHeight = 6f;
    public float cameraFollowSpeed = 15f;
    float cameraZ;
    void LateUpdate()
    {
        if (GameController.instance.armyList.Count != 0)
        {
            cameraZ = GameController.instance.armyList[0].transform.position.z - cameraDistance;
        }
        else
        {
            cameraZ = transform.position.z;
        }
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, cameraHeight,cameraZ), cameraFollowSpeed * Time.deltaTime);
    }

}
