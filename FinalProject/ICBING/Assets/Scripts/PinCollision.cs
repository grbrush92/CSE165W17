using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinCollision : MonoBehaviour {

    public Audio playAudio;

    // Use this for initialization
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name.Contains("BowlingBall"))
        {
            Debug.Log("Crash");
            playAudio.setCrash();
        }

        //checkReached = true;
        //Debug.Log("collision with: " + other.gameObject.name);
        //if (end == false)
        //{
        //    if (other.gameObject.tag == "Environment")
        //    {
        //        con.collisionEV();
        //        lcp.collisionEV();
        //        //Debug.Log("collision with: " + other.gameObject.name);
        //        playAudio.setCrash();
        //    }
        //}

    }
}
