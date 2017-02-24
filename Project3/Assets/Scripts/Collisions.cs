using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions : MonoBehaviour {

    //public delegate void CollisionAction();
    //public static event CollisionAction onCol;
    public LoadCheckPoints lcp;
    private float time;
    private bool checkReached;

    private void Start()
    {
        time = 0.0f;
        checkReached = true;
    }


    private void Update()
    {
        //Debug.Log(checkReached);
        if (checkReached == true)
        {
            time += Time.deltaTime;
            if (time < 1.0f)
            {
                //Debug.Log("Display");
                lcp.displayCheck("Checkpoint Reached");
            }
            else if (time >= 1.0f)
            {
                lcp.displayCheck("");
                time = 0.0f;
                //Debug.Log("checkReached set to false");
                checkReached = false;
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        checkReached = true;
        //Debug.Log("collision with: " + other.gameObject.name);
        if (other.gameObject.tag == "CP")
        {
            lcp.collisionCP();
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    //Physics.IgnoreCollision(collision.collider, this.gameObject.GetComponent<Collider>());
    //    //collision.gameObject.GetComponent<Rigidbody>().Sleep();
    //    Debug.Log("collision with: " + collision.gameObject.name);
    //    if (collision.gameObject.name == "LMHeadMountedRig")
    //    {
    //        lcp.weCollided();
    //    }
    //}
}
