using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCast : MonoBehaviour {

    public Transform target;
    void Update()
    {
            Vector3 fwd = transform.TransformDirection(Vector3.forward);

            if (Physics.Raycast(transform.position, fwd, 100000))
                print("There is something in front of the object!");
    }
}
