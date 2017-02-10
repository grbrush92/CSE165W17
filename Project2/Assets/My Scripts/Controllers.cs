using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEditor;





public class Controllers : MonoBehaviour {

    //object to be spawned 
    public GameObject prefab;
    public GameObject prefab_desk;
    public GameObject prefab_locker;
    public GameObject prefab_board;
    public GameObject prefab_tv;
    public GameObject prefab_cab;
    public GameObject parent;
    public GameObject grabbedObj;
    private GameObject storeGrabbedObj;
    public OVRCameraRig camera;

    public OVRPlayerController pointFinger;
    public OVRCameraRig handCam;
    public Transform player;

    public LayerMask grabMask;
    public OVRInput.Controller controller;

    //timer for teleport
    public float timer_teleport = 0.0f;
    public float timer_spawn = 0.0f;
    public float timer_changeObj = 0.0f;
    public float timer_select = 0.0f;

    public float sphereRadius;

    public LineRenderer leftLine;
    public LineRenderer rightLine;
    public float laserWidth;
    public float laserMaxLength;

    private bool bGrab = true;
    private bool bStart = true;
    private bool ready = true;
    private bool bTeleport = true;
    private bool first = true;
    private Vector3 newLocation, oldLocation;
    private bool select, butA, butB, clear, grab1, grab2, startButton;

    public GameObject recordObject;
    private StreamWriter file;
    private GameObject[] allObjects;
    private string[] thing = new string[256];
    private Vector3 lastUp, newUp, tVec, eVec;



    //prefab choice 
    // 0 - chair   1 - locker   2 - storage  3 - desk  4 - TV   5 - wb
    int obj = 0;

    //useless  - for debug
    int x = 9;

    bool grabbing = false;

    class ChangedObject
    {
        Renderer renderer;
        GameObject obj;
        Color oldColor;
        public ChangedObject(GameObject x, Renderer y) {
            // save the 
            renderer = y;
            obj = x;
            oldColor = y.material.color;
        }

        // reset
        // update

    }
    //ChangedObject[] oldObjects = new ChangedObject[20];
    GameObject[] oldObjects = new GameObject[20];
    int numChanged = 0;
    public Material new_mat;


    // Use this for initialization
    void Start () {
        Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        leftLine.SetPositions(initLaserPositions);
        leftLine.startWidth = laserWidth;
        leftLine.endWidth = laserWidth;
        rightLine.SetPositions(initLaserPositions);
        rightLine.startWidth = laserWidth;
        rightLine.endWidth = laserWidth;
        ReadFile();
        lastUp = newUp = handCam.transform.up;
        tVec = eVec = new Vector3(0, 0, 0);

        




    }

    void RecordData()
    {
        //Debug.Log(Transform.)
        Debug.Log("saving shit");
        allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        //save shit
        file = new StreamWriter("data.txt");

        foreach(GameObject go in allObjects)
        {
            if (go.name.Contains("DeskObj") || go.name.Contains("3DTVObj") || go.name.Contains("CabinetObj") || go.name.Contains("ChairObj") || go.name.Contains("locker") || go.name.Contains("WhiteBoardObj"))
            {
                recordObject = go;
                file.WriteLine(Time.time + " " + recordObject.name + " " + recordObject.transform.position.x + " " + recordObject.transform.position.y + " " + recordObject.transform.position.z + " " + recordObject.transform.eulerAngles.x + " " + recordObject.transform.eulerAngles.y + " " + recordObject.transform.eulerAngles.z);
            }
                
        }
        file.Close();
        Debug.Log("done saving");

        
    }
	
    void ReadFile()
    {
        allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        for (int q = 0; q < allObjects.Length;q++)
        {
            if (allObjects[q].name.Contains("DeskObj") || allObjects[q].name.Contains("3DTVObj") || allObjects[q].name.Contains("CabinetObj") || allObjects[q].name.Contains("ChairObj") || allObjects[q].name.Contains("locker") || allObjects[q].name.Contains("WhiteBoardObj"))
            {
                if (allObjects[q] != null)
                    Destroy(allObjects[q]);
            }
        }
        string[] lines = System.IO.File.ReadAllLines("data.txt");

        Debug.Log("contents of data.txt");
        foreach(string line in lines)
        {
            
            int x = 0;
            foreach (string token in line.Split())
            {
                Debug.Log(x + "  " + token);
                thing[x] = token;
                x++;
            }
            if (thing[2].Contains("("))
            {
                tVec = new Vector3(float.Parse(thing[3]), float.Parse(thing[4]), float.Parse(thing[5]));
                eVec = new Vector3(float.Parse(thing[6]), float.Parse(thing[7]), float.Parse(thing[8]));
            }
            else
            {
                tVec = new Vector3(float.Parse(thing[2]), float.Parse(thing[3]), float.Parse(thing[4]));
                eVec = new Vector3(float.Parse(thing[5]), float.Parse(thing[6]), float.Parse(thing[7]));
            }
            
            //Instantiate(myObj, player.transform.localPosition, parent.transform.localRotation);
            Debug.Log(thing[1]);
            if (thing[1].Contains("DeskObj"))
            {
                Debug.Log("entered DeskObj spawn");
                GameObject sudo;
                sudo = Instantiate(Resources.Load("DeskObj", typeof(GameObject))) as GameObject;
                sudo.transform.position = tVec;
                sudo.transform.eulerAngles = eVec;
            }
            else if (thing[1].Contains("3DTVObj"))
            {
                GameObject sudo;
                sudo = Instantiate(Resources.Load("3DTVObj", typeof(GameObject))) as GameObject;
                sudo.transform.position = tVec;
                sudo.transform.eulerAngles = eVec;
            }
            else if (thing[1].Contains("CabinetObj"))
            {
                GameObject sudo;
                sudo = Instantiate(Resources.Load("CabinetObj", typeof(GameObject))) as GameObject;
                sudo.transform.position = tVec;
                sudo.transform.eulerAngles = eVec;
            }
            else if (thing[1].Contains("ChairObj"))
            {
                GameObject sudo;
                sudo = Instantiate(Resources.Load("ChairObj", typeof(GameObject))) as GameObject;
                sudo.transform.position = tVec;
                sudo.transform.eulerAngles = eVec;
            }
            else if (thing[1].Contains("WhiteBoardObj"))
            {
                GameObject sudo;
                sudo = Instantiate(Resources.Load("WhiteBoardObj", typeof(GameObject))) as GameObject;
                sudo.transform.position = tVec;
                sudo.transform.eulerAngles = eVec;
            }
            else if (thing[1].Contains("locker"))
            {
                GameObject sudo;
                sudo = Instantiate(Resources.Load("locker", typeof(GameObject))) as GameObject;
                sudo.transform.position = tVec;
                sudo.transform.eulerAngles = eVec;
            }
            x = 0;
        }
    }


	// Update is called once per frame
	void Update () {
        select = OVRInput.Get(OVRInput.RawButton.RIndexTrigger) && !(OVRInput.Get(OVRInput.RawButton.RHandTrigger));
        butA = OVRInput.Get(OVRInput.RawButton.A);
        butB = OVRInput.Get(OVRInput.RawButton.B);
        clear = OVRInput.Get(OVRInput.RawButton.RHandTrigger);
        grab1 = OVRInput.Get(OVRInput.RawButton.RIndexTrigger);
        grab2 = OVRInput.Get(OVRInput.RawButton.RHandTrigger);
        bool save = OVRInput.Get(OVRInput.RawButton.LThumbstick);
        bool fly = OVRInput.Get(OVRInput.RawButton.RThumbstick);
        startButton = OVRInput.Get(OVRInput.RawButton.Start);
        newUp = handCam.transform.up;
        if (startButton == false)
        {
            bStart = true;
        }
        spawnObject();
        switchObject();
        //selectObject();
        teleport();

        if (startButton && bStart)
        {
            bGrab = !bGrab;
            bStart = false;
        }
        if (bGrab)
        {
            if (grab1 && grab2 && !grabbing)
            {
                storeGrabbedObj = grabbedObj;
                grabObject();
            }
            else
            {
                dropObject();
                
            }
                
        }
        else
        {
                selectObject();
        }
        if(save)
        {
            Debug.Log("save shit please");
            RecordData();
        }
        if(fly)
        {
            itsABirdItsAPlaneIts();
        }

        lastUp = newUp;
    }

    void itsABirdItsAPlaneIts()
    {
        player.transform.position += (handCam.rightHandAnchor.transform.rotation * Vector3.forward * 0.1f);
        
    }

    void grabObject()
    {
        grabbing = true;

        RaycastHit[] hits;
        hits = Physics.SphereCastAll(handCam.rightHandAnchor.transform.position, sphereRadius, handCam.rightHandAnchor.transform.forward, 0f, grabMask);
        
        if(hits.Length > 0)
        {
            int closest = 0;

            for(int i = 0; i < hits.Length; i++)
            {
                if(hits[i].distance < hits[closest].distance)
                {
                    closest = i;
                }
            }

            grabbedObj = hits[closest].transform.root.transform.gameObject;
            grabbedObj.GetComponent<Rigidbody>().isKinematic = true;
            grabbedObj.transform.position = handCam.rightHandAnchor.transform.position + handCam.rightHandAnchor.transform.rotation * Vector3.forward;
            grabbedObj.transform.parent = handCam.rightHandAnchor.transform;
        }


    }

    void dropObject()
    {

        
        if(grabbing)
        {
            grabbedObj.transform.parent = null;
            grabbedObj.GetComponent<Rigidbody>().isKinematic = false;
            //print(OVRInput.GetLocalControllerVelocity(controller));
            grabbedObj.GetComponent<Rigidbody>().velocity = OVRInput.GetLocalControllerVelocity(controller);
            grabbedObj = storeGrabbedObj;
        }
        grabbing = false;
    }

    void selectObject()
    {
        
        //print(OVRInput.Get(OVRInput.RawButton.A));
        if (butA == false)
            ready = true;
        //if (butB == false)
        //    grabbedObj.GetComponent<Rigidbody>().freezeRotation = false;
        if (select)
        {
            //actual teleport code here...
            Ray ray = new Ray(handCam.rightHandAnchor.transform.position, handCam.rightHandAnchor.transform.rotation * Vector3.forward);
            RaycastHit hit;
            RaycastHit[] hitA;
            hitA = Physics.RaycastAll(handCam.rightHandAnchor.transform.position, handCam.rightHandAnchor.transform.rotation * Vector3.forward);
            //newLocation = hit.point - handCam.rightHandAnchor.transform.position;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Vector3 pos = new Vector3(0, 0, 0);
                foreach (RaycastHit tH in hitA)
                {
                    
                    if (tH.collider.name == "Ground" || tH.collider.gameObject.name.Contains("WhiteBoard") || tH.collider.name.Contains("room"))
                    {
                        rightLine.enabled = true;
                        rightLine.SetPosition(0, handCam.rightHandAnchor.transform.position);
                        rightLine.SetPosition(1, hit.point);
                        pos = tH.point;
                    }
                }
                //if (hit.collider.name == "Ground")
                //    newLocation = hit.point;
                //rightLine.enabled = true;
                //rightLine.SetPosition(0, handCam.rightHandAnchor.transform.position);
                //rightLine.SetPosition(1, hit.point);



                // create class to saved color (gameobject, renderer)           
                

                if (butA && (ready == true))
                {
                    if (hit.collider.gameObject.tag == "Chair")
                    {
                        hit.collider.gameObject.transform.root.gameObject.transform.GetComponent<ParticleSystem>().Play();
                        oldObjects[numChanged] = hit.collider.gameObject;
                        numChanged++;
                        print("hit chair");
                        if (first)
                            grabbedObj.transform.position = hit.collider.gameObject.transform.root.gameObject.transform.position;
                        else
                        {
                            if (hit.collider.gameObject.transform.root.gameObject.transform.parent == null)
                                hit.collider.gameObject.transform.root.gameObject.transform.parent = grabbedObj.transform;
                        }
                        first = false;
                        
                        
                        
                    }
                    else if (hit.collider.gameObject.tag == "Desk")
                    {
                        hit.collider.gameObject.transform.root.gameObject.transform.GetComponent<ParticleSystem>().Play();
                        oldObjects[numChanged] = hit.collider.gameObject.transform.root.gameObject;
                        numChanged++;
                        if (first)
                            grabbedObj.transform.position = hit.collider.gameObject.transform.root.gameObject.transform.position;
                        else
                        {
                            if (hit.collider.gameObject.transform.root.gameObject.transform.parent == null)
                                hit.collider.gameObject.transform.root.gameObject.transform.parent = grabbedObj.transform;
                        }
                        first = false;
                        
                    }
                    else if (hit.collider.gameObject.tag == "Cabinet")
                    {
                        hit.collider.gameObject.transform.GetComponent<ParticleSystem>().Play();
                        oldObjects[numChanged] = hit.collider.gameObject;
                        numChanged++;
                        print("hit cabinet");
                        if (first)
                            grabbedObj.transform.position = hit.collider.gameObject.transform.root.gameObject.transform.position;
                        else
                        {
                            if (hit.collider.gameObject.transform.root.gameObject.transform.parent == null)
                                hit.collider.gameObject.transform.root.gameObject.transform.parent = grabbedObj.transform;
                        }
                        first = false;
                        
                    }
                    else if (hit.collider.gameObject.tag == "Locker")
                    {
                        hit.collider.gameObject.transform.root.gameObject.transform.GetComponent<ParticleSystem>().Play();
                        oldObjects[numChanged] = hit.collider.gameObject;
                        numChanged++;
                        if (first)
                            grabbedObj.transform.position = hit.collider.gameObject.transform.root.gameObject.transform.position;
                        else
                        {
                            if (hit.collider.gameObject.transform.root.gameObject.transform.parent == null)
                                hit.collider.gameObject.transform.root.gameObject.transform.parent = grabbedObj.transform;
                        }
                        first = false;
                        //grabbedObj = hit.collider.gameObject;
                        
                    }
                    else if (hit.collider.gameObject.tag == "TV")
                    {
                        hit.collider.gameObject.transform.root.gameObject.transform.GetComponent<ParticleSystem>().Play();
                        oldObjects[numChanged] = hit.collider.gameObject.transform.root.gameObject;
                        numChanged++;
                        if (first)
                            grabbedObj.transform.position = hit.collider.gameObject.transform.root.gameObject.transform.position;
                        else
                        {
                            if (hit.collider.gameObject.transform.root.gameObject.transform.parent == null)
                                hit.collider.gameObject.transform.root.gameObject.transform.parent = grabbedObj.transform;
                        }
                        first = false;
                        //grabbedObj = hit.collider.gameObject.transform.root.gameObject;
                        
                        

                    }
                    else if (hit.collider.gameObject.tag == "Board")
                    {
                        hit.collider.gameObject.transform.root.gameObject.transform.GetComponent<ParticleSystem>().Play();
                        if (first)
                            grabbedObj.transform.position = hit.collider.gameObject.transform.root.gameObject.transform.position;
                        else
                        {
                            if (hit.collider.gameObject.transform.root.gameObject.transform.parent == null)
                                hit.collider.gameObject.transform.root.gameObject.transform.parent = grabbedObj.transform;
                        }
                        first = false;
                        //grabbedObj = hit.collider.gameObject;
                        oldObjects[numChanged] = hit.collider.gameObject;
                        numChanged++;
                    }
                    else
                    {
                        //grabbedObj = null;
                    }
                    ready = false;
                }
                else if (butB)
                {
                    if (grabbedObj.transform != null)
                    {
                        grabbedObj.transform.position = pos + new Vector3(0, 1, 0);
                        double numerator = Vector3.Dot(Vector3.Cross(newUp, handCam.rightHandAnchor.transform.position), Vector3.Cross(lastUp, handCam.rightHandAnchor.transform.position));
                        double denominator = Vector3.Magnitude(Vector3.Cross(newUp, handCam.rightHandAnchor.transform.position)) * Vector3.Magnitude(Vector3.Cross(lastUp, handCam.rightHandAnchor.transform.position));
                        double theta = Math.Acos(numerator / denominator);
                        //grabbedObj.transform.RotateAround(grabbedObj.transform.position, new Vector3(0,1,0), (float)theta);
                    }
                        

                }
                oldLocation = newLocation;
            }
            else
            {
                rightLine.enabled = false;
            }
        }
        else if (clear)
        {
            for (int i = 0; i < numChanged; i++)
            {
                if (oldObjects[i] != null)
                {
                    oldObjects[i].gameObject.GetComponent<ParticleSystem>().Pause();
                    oldObjects[i].gameObject.GetComponent<ParticleSystem>().Clear();
                }
                oldObjects[i] = null;
                
            }
            numChanged = 0;
            print("entering clear");
            if (grabbedObj.transform.childCount != 0)
            {
                
                grabbedObj.transform.DetachChildren();
            }
                
            first = true;
            ready = true;
        }
        else
        {
            rightLine.enabled = false;
        }
        
    }

    //hold left trigger
    void teleport()
    {
        bool triggered = OVRInput.Get(OVRInput.RawButton.LIndexTrigger);
        bool handTrig = OVRInput.Get(OVRInput.RawButton.LHandTrigger);
        if (handTrig == false)
            bTeleport = true;
        if (triggered)
        {
            leftLine.enabled = true;

            //actual teleport code here...
            Ray ray = new Ray(handCam.leftHandAnchor.transform.position, handCam.leftHandAnchor.transform.rotation * Vector3.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.name == "Ground")
                {
                    leftLine.SetPosition(0, handCam.leftHandAnchor.transform.position);
                    leftLine.SetPosition(1, hit.point);
                    if (handTrig && (bTeleport == true))
                    {
                        player.transform.position = hit.point + new Vector3(0, 2, 0);
                        bTeleport = false;
                    }
                    //viewCamera.transform.position = hit.point + new Vector3(0, 4, 0);
                    //player.transform.position = hit.point + new Vector3(0, 3.5f, 0);

                    //Debug.DrawleftLine(handCam.leftHandAnchor.transform.position, hit.point, Color.red);
                    Debug.Log("teleport");
                }
                else
                {
                    leftLine.enabled = false;
                }

            }

            print(triggered);
            timer_teleport = 0.0f;
        }
        else if(triggered)
        {
            leftLine.enabled = true;
            Ray ray = new Ray(handCam.leftHandAnchor.transform.position, handCam.leftHandAnchor.transform.rotation * Vector3.forward);
            RaycastHit hit;



            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.name == "Ground")
                {
                    leftLine.SetPosition(0, handCam.leftHandAnchor.transform.position);
                    leftLine.SetPosition(1, hit.point);
                }

            }
            timer_teleport += Time.deltaTime;
        }
        else
        {
            leftLine.enabled = false;
            timer_teleport = 0.0f;
        }
    }
    
    //hold X button
    void spawnObject()
    {
        GameObject myObj = prefab;

        bool spawn = OVRInput.Get(OVRInput.RawButton.X);

        int choice = obj;

        if (spawn && timer_spawn > 2.0f)
        {
            switch (obj) {

                case 0:
                    myObj = prefab;
                    //Instantiate(Prefab, vector3, quaternion.identity);
                    
                    //prefab.transform.position = player.transform.localPosition;



                    break;
                case 1:
                    myObj = prefab_desk;
                    //Instantiate(Prefab, vector3, quaternion.identity);
                    //Instantiate(prefab_desk, new Vector3(2, 2, 2), Quaternion.identity);
                    break;
                case 2:
                    myObj = prefab_locker;
                    //Instantiate(Prefab, vector3, quaternion.identity);
                    //Instantiate(prefab_locker, new Vector3(2, 2, 2), Quaternion.identity);
                    break;
                case 3:
                    myObj = prefab_board;
                    //Instantiate(Prefab, vector3, quaternion.identity);
                    //Instantiate(prefab_board, new Vector3(2, 2, 2), Quaternion.identity);
                    break;
                case 4:
                    myObj = prefab_tv;
                    //Instantiate(Prefab, vector3, quaternion.identity);
                    //Instantiate(prefab_tv, new Vector3(2, 2, 2), Quaternion.identity);
                    break;
                case 5:
                    myObj = prefab_cab;
                    //Instantiate(prefab_cab, new Vector3(2, 2, 2), Quaternion.identity);
                    break;
            }

            Instantiate(myObj, player.transform.localPosition, parent.transform.localRotation);



            print(spawn);
            timer_spawn = 0.0f;
        }
        else if (spawn)
        {
            timer_spawn += Time.deltaTime;
        }
        else
        {
            timer_spawn = 0.0f;
        }
    }

    //hold Y button 
    int switchObject()
    {
        bool change = OVRInput.Get(OVRInput.RawButton.Y);

        if (change && timer_changeObj > 2.0f)
        {
            if(obj < 5)
            {
                obj++;
            }
            else
            {
                obj = 0;
            }

            print(change);
            timer_changeObj = 0.0f;
        }
        else if (change)
        {
            timer_changeObj += Time.deltaTime;
        }
        else
        {
            timer_changeObj = 0.0f;
        }


        return obj;
    }



}
