using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEditor;


public class HandRadial : MonoBehaviour {

    public OVRInput.Controller lHand;
    public OVRInput.Controller rHand;
    //public OVRInput.Controller controller;
    public OVRCameraRig cam;
    public OvrAvatar avatar;
    public GameObject selector;
    public float selectionRadius;
    public GameObject pallet;
    public LayerMask grabMask;
    bool B = false;
    bool Y = false;
    bool Rt = false;
    bool Lt = false;
    bool grabRt = false;
    bool grabLt = false;
    bool grabY = false;
    bool grabB = false;

    private GameObject grabbedObj;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        ControllerInput();
        gogo();

    }

    void ControllerInput()
    {
        B = OVRInput.Get(OVRInput.RawButton.B);
        Y = OVRInput.Get(OVRInput.RawButton.Y);
        Rt = OVRInput.Get(OVRInput.RawButton.RIndexTrigger);
        Lt = OVRInput.Get(OVRInput.RawButton.LIndexTrigger);

        if (B && !grabB)
        {
            //Debug.Log("grabbing");
            grabStuff();
            //if (!grabB)
            //{
            //    grabB = true;
            //    Debug.Log("B pressed");
            //    grabStuff();
            //}

        }
        else
        {
            dropStuff();
            //grabB = false;
            
        }

        if (Y)
        {
            if (!grabY)
            {
                grabY = true;
                Debug.Log("Y pressed");
                pallet.SetActive(true);
            }

        }
        else
        {
            grabY = false;
            pallet.SetActive(false);
        }

        if (Rt)
        {
            if (!grabRt)
            {
                grabRt = true;
                Debug.Log("Rt pressed");
            }

        }
        else
        {
            grabRt = false;
        }

        if (Lt)
        {
            if (!grabLt)
            {
                grabLt = true;
                Debug.Log("Lt pressed");
            }

        }
        else
        {
            grabLt = false;
        }
    }

    void gogo()
    {
        Vector3 handPos = cam.rightHandAnchor.transform.position;
        Vector3 dir = cam.rightHandAnchor.transform.position - cam.rightHandAnchor.forward;
        Vector3 dist = cam.rightHandAnchor.transform.position - cam.centerEyeAnchor.transform.position;

        float distance2 = Vector3.Magnitude(dist);
        if (distance2 > 0.45f)
        {
            selector.transform.position = cam.rightHandAnchor.transform.position + (dist * Mathf.Pow((distance2 - 0.45f) * 10, 2));
        }
        else
        {
            selector.transform.position = cam.rightHandAnchor.transform.position;
        }
        selector.transform.rotation = cam.rightHandAnchor.transform.rotation;
    }

    void grabStuff()
    {
        grabB = true;
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(selector.transform.position,selectionRadius,selector.transform.forward, 0f, grabMask);

        if (hits.Length > 0)
        {
            int closest = 0;

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].distance < hits[closest].distance)
                {
                    closest = i;
                }
            }

            if ((hits[closest].transform.gameObject.tag == "Grabbable") && (grabbedObj == null))
            {
                //Debug.Log("Grabbing: " + hits[closest].transform.gameObject.name);
                if (hits[closest].transform.root == null)
                    grabbedObj = hits[closest].transform.gameObject;
                else
                    grabbedObj = hits[closest].transform.root.transform.gameObject;
                //grabbedObj = hits[closest].transform.gameObject;
                if (!grabbedObj.GetComponent<Rigidbody>().isKinematic)
                    grabbedObj.GetComponent<Rigidbody>().isKinematic = true;
                
                //grabbedObj.transform.parent = cam.rightHandAnchor.transform;
            }

        }
        if (grabbedObj != null)
        {
            grabbedObj.transform.position = selector.transform.position;
            grabbedObj.transform.rotation = selector.transform.rotation;
        }
        
    }

    void dropStuff()
    {
        if (grabB && (grabbedObj != null))
        {
           // grabbedObj.transform.parent = null;
            grabbedObj.GetComponent<Rigidbody>().isKinematic = false;
            //print(OVRInput.GetLocalControllerVelocity(controller));
            grabbedObj.GetComponent<Rigidbody>().velocity = OVRInput.GetLocalControllerVelocity(rHand);
            //grabbedObj = storeGrabbedObj;
            grabbedObj = null;
        }
        grabB = false;
    }
}
