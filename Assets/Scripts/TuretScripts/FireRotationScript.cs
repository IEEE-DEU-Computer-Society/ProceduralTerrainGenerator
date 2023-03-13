using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireRotationScript : MonoBehaviour
{
    public GameObject enemy;

    public Rigidbody2D rgb;

    public float force = 8;



    // Start is called before the first frame update
    void Start()
    {
        
        //fire
       

        

    }

    // Update is called once per frame
    void Update()
    {


    }

    //InVisable Bullet
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "enemy")
        {


            StartCoroutine(Hit());
            this.GetComponent<SpriteRenderer>().enabled = false;


        }

    }

    //Enemy Hit
    IEnumerator Hit()
    {
        enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(enemy.transform.position.x - transform.position.x ,
            enemy.transform.position.y - transform.position.y );
        yield return new WaitForSeconds(0.2f);
        enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        Destroy(this.gameObject);


    }

    

}
    

