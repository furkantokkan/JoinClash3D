using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkin : MonoBehaviour
{
    public GameObject shield;
    public GameObject helmet;
    public GameObject armor;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMaterial(Material material)
    {
        shield.gameObject.GetComponent<MeshRenderer>().material = material;
        helmet.gameObject.GetComponent<MeshRenderer>().material = material;
        armor.gameObject.GetComponent<SkinnedMeshRenderer>().material = material;
    }
}
