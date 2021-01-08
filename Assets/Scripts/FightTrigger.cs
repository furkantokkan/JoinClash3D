using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class FightTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameController.startFight = true;
            for (int i = 0; i < GameController.instance.armyList.Count; i++)
            {
                GameController.instance.armyList[i].gameObject.GetComponent<Movement>().enabled = false;
                GameController.instance.armyList[i].gameObject.GetComponent<Rigidbody>().isKinematic = true;
                GameController.instance.armyList[i].gameObject.GetComponent<NavMeshAgent>().enabled = true;
                GameController.instance.armyList[i].gameObject.GetComponent<AICombat>().enabled = true;
                if (i == GameController.instance.armyList.Count)
                {
                    enabled = false;
                }
            }
        }
    }
}
