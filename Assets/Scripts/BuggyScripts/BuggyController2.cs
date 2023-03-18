 using UnityEngine;
 using System.Collections;
 
 public class BuggyController2 : MonoBehaviour {
 
     public float acceleration;
     public float steering;
     private Rigidbody2D rbBuggy;
 
     void Start () {
         rbBuggy = GetComponent<Rigidbody2D>();
     }
 
     void FixedUpdate () {
         float h = -Input.GetAxis("Horizontal");
         float v = Input.GetAxis("Vertical");
 
         Vector2 speed = transform.up * (v * acceleration);
         rbBuggy.AddForce(speed);
 
         float direction = Vector2.Dot(rbBuggy.velocity, rbBuggy.GetRelativeVector(Vector2.up));
         if(direction >= 0.0f) {
             rbBuggy.rotation += h * steering * (rbBuggy.velocity.magnitude / 5.0f);
             //rb.AddTorque((h * steering) * (rb.velocity.magnitude / 10.0f));
         } else {
             rbBuggy.rotation -= h * steering * (rbBuggy.velocity.magnitude / 5.0f);
             //rb.AddTorque((-h * steering) * (rb.velocity.magnitude / 10.0f));
         }
 
         Vector2 forward = new Vector2(0.0f, 0.5f);
         float steeringRightAngle;
         if(rbBuggy.angularVelocity > 0) {
             steeringRightAngle = -90;
         } else {
             steeringRightAngle = 90;
         }
 
         Vector2 rightAngleFromForward = Quaternion.AngleAxis(steeringRightAngle, Vector3.forward) * forward;
         Debug.DrawLine((Vector3)rbBuggy.position, (Vector3)rbBuggy.GetRelativePoint(rightAngleFromForward), Color.green);
 
         float driftForce = Vector2.Dot(rbBuggy.velocity, rbBuggy.GetRelativeVector(rightAngleFromForward.normalized));
 
         Vector2 relativeForce = (rightAngleFromForward.normalized * -1.0f) * (driftForce * 10.0f);
 
 
         Debug.DrawLine((Vector3)rbBuggy.position, (Vector3)rbBuggy.GetRelativePoint(relativeForce), Color.red);
 
         rbBuggy.AddForce(rbBuggy.GetRelativeVector(relativeForce));
     }
 }