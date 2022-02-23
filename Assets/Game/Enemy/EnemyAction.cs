using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class EnemyAction : MonoBehaviour
{
    public float animationTime;
    public string attackName;
    public GameObject processorPrefab;
    public CallForInterjectionsGameEvent interjectionEvent;

    public virtual void Execute()
    {

    }
}
