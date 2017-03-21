using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneScript : MonoBehaviour {

    public HandRadial radial;

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("0");

        if (other.gameObject.tag == "Lane1")
        {
            Debug.Log("1");
            radial.setLane1();
        }
        if (other.gameObject.tag == "Lane2")
        {
            Debug.Log("2");
            radial.setLane2();
        }
        if (other.gameObject.tag == "Lane3")
        {
            Debug.Log("3");
            radial.setLane3();
        }
    }
}
