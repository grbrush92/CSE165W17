using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEditor;


public class HandRadial : MonoBehaviour
{

    public OVRInput.Controller lHand;
    public OVRInput.Controller rHand;
    //public OVRInput.Controller controller;
    public OVRCameraRig cam;
    public OvrAvatar avatar;
    public GameObject selector;
    public float selectionRadius;
    public GameObject pallet;
    public LayerMask grabMask;
    public LineRenderer lzr;
    public GameObject BowlingBall;
    public GameObject BasketBall;
    bool B = false;
    bool Y = false;
    bool Rt = false;
    bool Lt = false;
    bool grabRt = false;
    bool grabLt = false;
    bool grabY = false;
    bool grabB = false;
    bool spawn = false;
    private Quaternion lastRotation, currentRotation;
    private float maxCursorDistance = 0.2f;
    GameObject lastObj = null;
    Vector3 ballSpawn = new Vector3(0.3f, 0.3f, -2);
    private GameObject grabbedObj;
    

    // Use this for initialization
    void Start()
    {
        lzr.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbedObj != null)
        {
            lastRotation = currentRotation;
            currentRotation = grabbedObj.transform.rotation;
        }
        ControllerInput();
        ControllerOutput();
        
    }

    void ControllerInput()
    {
        B = OVRInput.Get(OVRInput.RawButton.B);
        Y = OVRInput.Get(OVRInput.RawButton.Y);
        Rt = OVRInput.Get(OVRInput.RawButton.RIndexTrigger);
        Lt = OVRInput.Get(OVRInput.RawButton.LIndexTrigger);

        if (B)
        {
            grabB = true;
        }
        else
        {
            grabB = false;
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
                grabStuff();
            else if (grabRt)
            {
                if (grabbedObj != null)
                {
                    grabbedObj.transform.position = selector.transform.position;
                    grabbedObj.transform.rotation = selector.transform.rotation;
                }
            }

        }
        else
        {
            dropStuff();
            grabRt = false;
            spawn = false;
        }

        if (Lt)
        {
            if (!grabLt)
            {
                grabLt = true;
            }
        }
        else
        {
            grabLt = false;
        }
    }

    void gogo()
    {
        Vector3 dist = cam.rightHandAnchor.transform.position - cam.centerEyeAnchor.transform.position;
        //Vector3 dist = cam.rightHandAnchor.rotation * Vector3.forward;
        float distance2 = Vector3.Magnitude(dist);
        if (distance2 > 0.65f)
        {
            selector.transform.position = cam.rightHandAnchor.transform.position + (cam.rightHandAnchor.rotation * Vector3.forward * Mathf.Pow((distance2 - 0.65f) * 25, 2));
            selector.SetActive(true);
        }
        else
        {
            selector.transform.position = cam.rightHandAnchor.transform.position;
            selector.SetActive(false);
        }
        selector.transform.rotation = cam.rightHandAnchor.transform.rotation;
    }

    void grabStuff()
    {
        grabRt = true;
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(selector.transform.position, selectionRadius, selector.transform.forward, 0f, grabMask);

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
                if (hits[closest].transform.root == null)
                    grabbedObj = hits[closest].transform.gameObject;
                else
                    grabbedObj = hits[closest].transform.root.transform.gameObject;
                if (!grabbedObj.GetComponent<Rigidbody>().isKinematic)
                    grabbedObj.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }

    void dropStuff()
    {
        if (grabRt && (grabbedObj != null))
        {
            grabbedObj.GetComponent<Rigidbody>().isKinematic = false;
            grabbedObj.GetComponent<Rigidbody>().velocity = OVRInput.GetLocalControllerVelocity(rHand) * 2;
            grabbedObj.GetComponent<Rigidbody>().angularVelocity = getAngularVelocity();
            grabbedObj = null;
        }
    }

    Vector3 getAngularVelocity()
    {
        Quaternion delta = currentRotation * Quaternion.Inverse(lastRotation);
        return new Vector3(Mathf.DeltaAngle(0, delta.eulerAngles.x), Mathf.DeltaAngle(0, delta.eulerAngles.y), Mathf.DeltaAngle(0, delta.eulerAngles.z));
    }

    void ControllerOutput()
    {
        
        if (grabY)
        {
            selector.SetActive(false);
            lzr.enabled = true;
            Ray ray = new Ray(cam.rightHandAnchor.position, cam.rightHandAnchor.rotation * Vector3.forward);
            //Vector3 p = cam.rightHandAnchor.rotation * cam.rightHandAnchor.forward;
            lzr.SetPosition(0, cam.rightHandAnchor.position);
            lzr.SetPosition(1, ray.origin + ray.direction.normalized * maxCursorDistance);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {

                if (hit.collider.gameObject.tag == "BowlingBall")
                {
                    
                    lastObj = hit.collider.gameObject;
                    if (lastObj != null)
                    {
                        lastObj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    }
                    hit.collider.gameObject.transform.localScale = new Vector3(0.2f,0.2f,0.2f);
                    if (grabRt)
                    {
                        if (spawn == false)
                        {
                            Instantiate(BowlingBall, ballSpawn, new Quaternion(0, 0, 0, 0));
                            spawn = true;
                        }
                            
                        Debug.Log("bowlingball");
                        
                    }
                }
                if (hit.collider.gameObject.tag == "BasketBall")
                {
                    
                    lastObj = hit.collider.gameObject;
                    if (lastObj != null)
                    {
                        lastObj.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                    }
                    hit.collider.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    if (grabRt)
                    {
                        if (spawn == false)
                        {
                            Instantiate(BasketBall, ballSpawn, new Quaternion(0, 0, 0, 0));
                            spawn = true;
                        }

                        Debug.Log("bowlingball");

                    }
                }
                //else
                //{
                //    Debug.Log("paskldfja;");
                //    if (lastObj != null)
                //    {
                //        Debug.Log(";");
                //        lastObj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                //        lastObj = null;
                //    }

                //}
            }
            else
            {
                if (lastObj != null)
                {
                    if (lastObj.tag == "BowlingBall")
                        lastObj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    else if (lastObj.tag == "BasketBall")
                        lastObj.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                    lastObj = null;
                }

            }

        }
        else
        {
            selector.SetActive(true);
            lzr.enabled = false;
            gogo();
        }
    }
}
