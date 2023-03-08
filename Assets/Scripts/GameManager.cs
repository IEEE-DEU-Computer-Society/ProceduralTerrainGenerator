using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager ClassScript = null;

    public bool zoneIn = false;

    private void Awake()
    {
        if (ClassScript == null)
            ClassScript = this;
    }




}