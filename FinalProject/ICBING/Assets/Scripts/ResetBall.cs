using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBall : MonoBehaviour {

    public HandRadial radial;

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name.Contains("BowlingBall"))
        {
            radial.resetBall();
        }

        if (other.gameObject.name.Contains("BasketBall"))
        {
            radial.resetBall();
        }
    }

}
