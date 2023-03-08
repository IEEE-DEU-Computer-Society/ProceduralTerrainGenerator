using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireRotationScript : MonoBehaviour
{
    public GameObject enemy;

    public Rigidbody2D rgb;

    public float force;
    // Start is called before the first frame update
    void Start()
    {
        
      
        Vector3 direction = enemy.transform.position - transform.position;
        rgb.velocity = new Vector2(direction.x, direction.y).normalized * force;

        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,rot+90);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "enemy")
        {
            StartCoroutine(Hit());
           
            
            Debug.Log("Do somthing!!");
        }
        
    }

    IEnumerator Hit()
    {
        enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0);
        yield return new WaitForSeconds(0.2f);
        enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);


    }
}
