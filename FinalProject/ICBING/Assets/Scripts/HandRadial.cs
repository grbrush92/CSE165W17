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
    public GameObject player;
    //public OVRInput.Controller controller;
    public OVRCameraRig cam;
    public OvrAvatar avatar;
    public GameObject selector;
    public float selectionRadius;
    public GameObject pallet;
    public GameObject modMenu;
    public LayerMask grabMask;
    public LineRenderer lzr;
    public GameObject BowlingBall;
    public GameObject BasketBall;
    public SpawnPins spScript;
    public GameObject cursor;
    private GameObject tpCursor;
    private float scaleFactor;
    private float massFactor;
    public TextMesh size;
    public TextMesh mass;
    private float bowlScale;
    private float bowlMass;
    private float basketScale;
    private float basketMass;
    bool B = false;
    bool Y = false;
    bool Rt = false;
    bool Lt = false;
    bool grabRt = false;
    bool grabLt = false;
    bool grabY = false;
    bool grabB = false;
    bool spawn = false;
    bool hover = false;
    bool teleported = false;
    bool once = false;
    private Quaternion lastRotation, currentRotation;
    private float maxCursorDistance = 0.2f;
    GameObject lastObj = null;
    Vector3 ballSpawn = new Vector3(0.3f, 0.3f, -3.5f);
    private GameObject grabbedObj;
    private GameObject curBall;
    public Audio playAudio;


    // Use this for initialization
    void Start()
    {
        lzr.enabled = false;
        tpCursor = Instantiate(cursor);
        tpCursor.SetActive(false);
        curBall = Instantiate(BowlingBall, ballSpawn, new Quaternion(0, 0, 0, 0));
        bowlScale = curBall.transform.localScale.x;
        bowlMass = curBall.GetComponent<Rigidbody>().mass;
        basketScale = 0.2f;
        basketMass = 2.0f;
        playAudio.setStart();

    }

    // Update is called once per frame
    void Update()
    {
        if (grabbedObj != null)
        {
            lastRotation = currentRotation;
            currentRotation = cam.rightHandAnchor.transform.rotation;
            //currentRotation = OVRInput.GetLocalControllerRotation(rHand);
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
            if (!grabB)
            {
                grabB = true;
                //selector.SetActive(true);
            }
            else
            {
                //selector.SetActive(false);
            }
        }
        else
        {
            selector.SetActive(false);
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
            modMenu.SetActive(false);
        }

        if (Rt)
        {
            if (!grabRt && !Lt)
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
            teleported = false;
            once = false;
        }

        if (Lt)
        {
            if (!grabLt)
            {
                
                grabLt = true;
                //tpCursor.SetActive(true);
                //teleport();
                //spScript.removePins();
                //spScript.spawnPins();
            }
            else if (grabLt && Rt)
            {
                if (!teleported)
                    teleport();
            }
        }
        else
        {
            tpCursor.SetActive(false);
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
        //if (grabB)
        //{

        //}
        //else
        //{
        //    selector.transform.position = cam.rightHandAnchor.transform.position;
        //    selector.transform.rotation = cam.rightHandAnchor.transform.rotation;
        //}

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
                if (hits[closest].transform.name == "Bowling_Pin_HighRez")
                {
                    grabbedObj = hits[closest].transform.gameObject;
                }
                else if (hits[closest].transform.root == null)
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
                    //if (lastObj != null)
                    //{
                    //    lastObj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    //}
                    //hit.collider.gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                    if (grabRt)
                    {
                        if (spawn == false)
                        {
                            Destroy(curBall);
                            curBall = Instantiate(BowlingBall, ballSpawn, new Quaternion(0, 0, 0, 0));
                            curBall.GetComponent<Rigidbody>().mass = bowlMass;
                            curBall.transform.localScale = new Vector3(bowlScale, bowlScale, bowlScale);
                            curBall.GetComponent<Rigidbody>().mass = bowlMass;
                            size.text = bowlScale.ToString();
                            mass.text = ((int)curBall.GetComponent<Rigidbody>().mass).ToString();
                            spawn = true;
                        }
                    }
                }
                else if (hit.collider.gameObject.tag == "BasketBall")
                {

                    lastObj = hit.collider.gameObject;
                    //if (lastObj != null)
                    //{
                    //    lastObj.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                    //}
                    //hit.collider.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    if (grabRt)
                    {
                        if (spawn == false)
                        {
                            Destroy(curBall);
                            curBall = Instantiate(BasketBall, ballSpawn, new Quaternion(0, 0, 0, 0));
                            curBall.GetComponent<Rigidbody>().mass = basketMass;
                            curBall.transform.localScale = new Vector3(basketScale, basketScale, basketScale);
                            curBall.GetComponent<Rigidbody>().mass = basketMass;
                            size.text = basketScale.ToString();
                            mass.text = ((int)curBall.GetComponent<Rigidbody>().mass).ToString();
                            spawn = true;
                        }
                    }
                }
                else if (hit.collider.gameObject.tag == "BowlingPin")
                {
                    lastObj = hit.collider.gameObject;
                    //if (lastObj != null)
                    //{
                    //    lastObj.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
                    //}
                    //hit.collider.gameObject.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);

                    if (grabRt)
                    {
                        if (spawn == false)
                        {
                            //Instantiate(BasketBall, ballSpawn, new Quaternion(0, 0, 0, 0));
                            spScript.removePins();
                            spScript.spawnPins();
                            //curBall = Instantiate(curBall.gameObject, ballSpawn, new Quaternion(0, 0, 0, 0));
                            spawn = true;
                        }
                    }
                }
                else if (hit.collider.gameObject.tag == "Settings")
                {
                    if (grabRt && !once)
                    {
                        once = true;
                        pallet.SetActive(false);
                        modMenu.SetActive(true);
                        Debug.Log(curBall.name);
                        if (curBall.name.Contains("BowlingBall"))
                        {
                            bowlScale = curBall.transform.localScale.x;
                            size.text = bowlScale.ToString();
                            mass.text = ((int)curBall.GetComponent<Rigidbody>().mass).ToString();
                        }
                        else if (curBall.name.Contains("BasketBall"))
                        {
                            basketScale = curBall.transform.localScale.x;
                            size.text = basketScale.ToString();
                            mass.text = ((int)curBall.GetComponent<Rigidbody>().mass).ToString();
                        }

                        //lastScale = curBall.transform.localScale.x;
                        //size.text = ((int)lastScale).ToString();
                        //mass.text = ((int)curBall.GetComponent<Rigidbody>().mass).ToString();
                    }
                    
                }
                else if (hit.collider.gameObject.tag == "ScaleUp")
                {
                    //Debug.Log("size up");
                    if (grabRt && !once)
                    {
                        //Debug.Log("triggered");
                        once = true;

                        if (curBall.name.Contains("BowlingBall"))
                        {
                            bowlScale = curBall.transform.localScale.x + 0.1f;
                            curBall.transform.localScale = new Vector3(bowlScale, bowlScale, bowlScale);
                            size.text = bowlScale.ToString();
                            //mass.text = ((int)curBall.GetComponent<Rigidbody>().mass).ToString();
                        }
                        else if (curBall.name.Contains("BasketBall"))
                        {
                            basketScale = curBall.transform.localScale.x + 0.1f;
                            curBall.transform.localScale = new Vector3(basketScale, basketScale, basketScale);
                            size.text = basketScale.ToString();
                            //mass.text = ((int)curBall.GetComponent<Rigidbody>().mass).ToString();
                        }

                        //lastScale = curBall.transform.localScale.x + 0.1f;
                        //curBall.transform.localScale = new Vector3(lastScale, lastScale, lastScale);
                        //size.text = lastScale.ToString();
                    }
                }
                else if (hit.collider.gameObject.tag == "ScaleDown")
                {
                    if (grabRt && !once)
                    {
                        once = true;

                        if (curBall.name.Contains("BowlingBall"))
                        {
                            bowlScale = curBall.transform.localScale.x - 0.1f;
                            curBall.transform.localScale = new Vector3(bowlScale, bowlScale, bowlScale);
                            size.text = bowlScale.ToString();
                            //mass.text = ((int)curBall.GetComponent<Rigidbody>().mass).ToString();
                        }
                        else if (curBall.name.Contains("BasketBall"))
                        {
                            basketScale = curBall.transform.localScale.x - 0.1f;
                            curBall.transform.localScale = new Vector3(basketScale, basketScale, basketScale);
                            size.text = basketScale.ToString();
                            //mass.text = ((int)curBall.GetComponent<Rigidbody>().mass).ToString();
                        }

                        //lastScale = curBall.transform.localScale.x - 0.1f;
                        //curBall.transform.localScale = new Vector3(lastScale, lastScale, lastScale);
                        //size.text = lastScale.ToString();
                    }
                }
                else if (hit.collider.gameObject.tag == "MassUp")
                {
                    if (grabRt && !once)
                    {
                        once = true;

                        if (curBall.name.Contains("BowlingBall"))
                        {
                            bowlMass = curBall.GetComponent<Rigidbody>().mass + 1f;
                            //curBall.transform.localScale = new Vector3(bowlScale, bowlScale, bowlScale);
                            curBall.GetComponent<Rigidbody>().mass = bowlMass;
                            //size.text = bowlScale.ToString();
                            mass.text = ((int)curBall.GetComponent<Rigidbody>().mass).ToString();
                        }
                        else if (curBall.name.Contains("BasketBall"))
                        {
                            basketMass = curBall.GetComponent<Rigidbody>().mass + 1f;
                            //curBall.transform.localScale = new Vector3(bowlScale, bowlScale, bowlScale);
                            curBall.GetComponent<Rigidbody>().mass = basketMass;
                            //size.text = basketScale.ToString();
                            mass.text = ((int)curBall.GetComponent<Rigidbody>().mass).ToString();
                        }

                        //lastMass = curBall.GetComponent<Rigidbody>().mass + 1.0f;
                        //curBall.GetComponent<Rigidbody>().mass = lastMass;
                        //mass.text = ((int)lastMass).ToString();
                    }
                }
                else if (hit.collider.gameObject.tag == "MassDown")
                {
                    if (grabRt && !once)
                    {
                        once = true;

                        if (curBall.name.Contains("BowlingBall"))
                        {
                            bowlMass = curBall.GetComponent<Rigidbody>().mass - 1f;
                            //curBall.transform.localScale = new Vector3(bowlScale, bowlScale, bowlScale);
                            curBall.GetComponent<Rigidbody>().mass = bowlMass;
                            //size.text = bowlScale.ToString();
                            mass.text = ((int)curBall.GetComponent<Rigidbody>().mass).ToString();
                        }
                        else if (curBall.name.Contains("BasketBall"))
                        {
                            basketMass = curBall.GetComponent<Rigidbody>().mass - 1f;
                            //curBall.transform.localScale = new Vector3(bowlScale, bowlScale, bowlScale);
                            curBall.GetComponent<Rigidbody>().mass = basketMass;
                            //size.text = basketScale.ToString();
                            mass.text = ((int)curBall.GetComponent<Rigidbody>().mass).ToString();
                        }

                        //float tempMass = curBall.GetComponent<Rigidbody>().mass - 1.0f;
                        //curBall.GetComponent<Rigidbody>().mass = lastMass;
                        //mass.text = ((int)lastMass).ToString();
                    }
                }
                else if (hit.collider.gameObject.tag == "Return")
                {
                    if (grabRt && !once)
                    {
                        once = true;
                        pallet.SetActive(true);
                        modMenu.SetActive(false);
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
                    //if (lastObj.tag == "BowlingBall")
                    //    lastObj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    //else if (lastObj.tag == "BasketBall")
                    //    lastObj.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                    //else if (lastObj.tag == "BowlingPin")
                    //    lastObj.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
                    lastObj = null;
                }

            }

        }
        else if (grabLt)
        {
            showCursor();
            //if (grabRt)
            //{
            //    Debug.Log("Before tele");
            //    teleport();
            //}
        }
        else
        {
            //selector.SetActive(true);
            lzr.enabled = false;
            if (grabB)
            {
                gogo();
            }
            else
            {
                regularMove();
            }
            
        }
    }

    void showCursor()
    {
        Ray ray = new Ray(cam.rightHandAnchor.position, cam.rightHandAnchor.rotation * Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.tag == "Ground")
            {
                tpCursor.SetActive(true);
                tpCursor.transform.position = hit.point;
                tpCursor.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                //if (grabRt)
                //{
                //    player.transform.position = hit.point + new Vector3(0, 1, 0);
                //}
            }
            else
            {
                tpCursor.SetActive(false);
            }
        }
    }

    void teleport()
    {
        Ray ray = new Ray(cam.rightHandAnchor.position, cam.rightHandAnchor.rotation * Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.tag == "Ground")
            {
                Debug.Log("next tele");
                player.transform.position = hit.point + new Vector3(0, 1.5f, 0);
            }
        }
        teleported = true;
    }

    void regularMove()
    {
        selector.transform.position = cam.rightHandAnchor.transform.position;
        selector.transform.rotation = cam.rightHandAnchor.transform.rotation;
    }

    public void resetBall()
    {
        curBall.transform.position = ballSpawn;
        curBall.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        curBall.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
    }
}
