using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Playables;

public class ZoneScript : MonoBehaviour
{
    public GameObject bullet;

    public Transform bulletTrans;
    

    private float timer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        
            timer += Time.deltaTime;

            
        
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag== "enemy")
        {
            
            
            if (timer > 2)
            {
                timer = 0;
                SpwanBullet();
            }
            
        }
            
       
       
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Dusman cikti!!");
        GameManager.ClassScript.zoneIn = false;
    }
    public void SpwanBullet()
    {
       
        Instantiate(bullet, bulletTrans.position, Quaternion.identity);
        

    }
}