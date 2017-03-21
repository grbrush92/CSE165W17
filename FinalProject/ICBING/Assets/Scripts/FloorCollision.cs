using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCollision : MonoBehaviour {

    public Audio playAudio;

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name.Contains("BowlingBall"))
        {
            playAudio.setRoll();
            Debug.Log("Enter");
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

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("BowlingBall"))
        {
            Debug.Log("Exit");
            playAudio.setStop();
        }
    }
}
