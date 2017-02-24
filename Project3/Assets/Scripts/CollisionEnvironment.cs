using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEnvironment : MonoBehaviour {

    //public delegate void CollisionAction();
    //public static event CollisionAction onCol;
    public Controls con;
    public LoadCheckPoints lcp;
    private float time = 0;
    private bool end = false;
    public Audio playAudio;
    //private bool checkReached = false;
    //private void Update()
    //{
    //    if (checkReached == true)
    //    {
    //        time += Time.deltaTime;
    //        if (time < 1.0f)
    //        {
    //            lcp.displayCheck("Checkpoint Reacehed");
    //        }
    //        else
    //        {
    //            lcp.displayCheck("");
    //            time = 0;
    //            checkReached = false;
    //        }
    //    }

    //}

    private void OnTriggerEnter(Collider other)
    {
        //checkReached = true;
        //Debug.Log("collision with: " + other.gameObject.name);
        if (end == false)
        {
            if (other.gameObject.tag == "Environment")
            {
                con.collisionEV();
                lcp.collisionEV();
                //Debug.Log("collision with: " + other.gameObject.name);
                playAudio.setCrash();
            }
        }
        
    }

    public void raceEnd()
    {
        end = true;
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
