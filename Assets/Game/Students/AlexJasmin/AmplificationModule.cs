using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class AmplificationModule : Relic
{
    GameObject player;
    private void Awake()
    {
        player = FindObjectOfType<Player>().gameObject;
    }
}