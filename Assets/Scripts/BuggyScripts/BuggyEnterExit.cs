using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BuggyEnterExit : MonoBehaviour
{
    public MonoBehaviour BuggyController2;
    private bool Candrive;
    public Transform Buggy;
    public Transform Player;
    public GameObject PlayerCam;
    public GameObject BuggyCam;
    
    void Start()
    {
        BuggyController2.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Candrive)
        {
            BuggyController2.enabled = true;

            // Parentlama
            
            Player.transform.SetParent(Buggy);
            Player.gameObject.SetActive(false);

            // Camera

            PlayerCam.transform.SetParent(Buggy);
            PlayerCam.gameObject.SetActive(false);
            BuggyCam.gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            BuggyController2.enabled = false;

            // Unparent


            Player.transform.SetParent(null);
            Player.gameObject.SetActive(true);

            // Sürmüyorken Camera doğrulama
            
            PlayerCam.transform.SetParent(null);
            Player.transform.Translate(0, 0, 0);
            PlayerCam.gameObject.SetActive(true);
            BuggyCam.gameObject.SetActive(false);
        }
        
        if (BuggyController2.enabled == true)
        {
            BuggyCam.transform.eulerAngles = new Vector3 (0, 0, 0);
        }
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Candrive = true;
            PlayerCam.transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Candrive = false;
        }
    }
}