﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public static bool startFight;
    public Material blue;
    public Material red;
    public Material grey;

    public GameObject ground;

    public List<GameObject> armyList = new List<GameObject>();
    public List<GameObject> enemyList = new List<GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
