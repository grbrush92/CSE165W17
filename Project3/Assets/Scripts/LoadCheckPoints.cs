using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadCheckPoints : MonoBehaviour {

    public GameObject checkPointPreFab;
    public GameObject player;
    public Camera cam;
    public GameObject arrow;
    public TextMesh text;
    public TextMesh textBack;
    public LineRenderer laZr;
    public LineRenderer laZr2;
    public CollisionEnvironment ce;
    public float scaleFactor;
    private bool firstLine = true;
    private bool secondLine = false;
    private GameObject[] cpArray;
    private int index;
    private int count;
    private int currentCP;
    private int dist;
    private float gameTime;
    public TextMesh gameTimeText;
    public TextMesh gameTimeTextBack;
    public TextMesh finishTime;
    public TextMesh headsUp;
    public Audio playAudio;
    private bool stopTime;
    Vector3 oldCP;
    //private TextMesh text;

    private void OnEnable()
    {
        //Collisions.onCol += weCollided;
    }



    // Use this for initialization
    void Start () {
        index = 0;
        count = 0;
        gameTime = 0.0f;
        currentCP = 1;
        dist = 0;
        laZr.enabled = true;
        laZr2.enabled = true;
        stopTime = false;

        finishTime.GetComponent<ParticleSystem>().Pause();

        StreamReader trackIn = File.OpenText("Competition-track.txt");
        string line;
        //count # of lines to be read
        while ((line = trackIn.ReadLine()) != null)
        {
            string[] items = line.Split(' ');
            count++;
        }
        Debug.Log("Count: " + count);
        trackIn.Close();
        trackIn = File.OpenText("Competition-track.txt");

        //create array of objs
        cpArray = new GameObject[count];

        //read in line contents
        while ((line = trackIn.ReadLine()) != null)
        {
            GameObject tempObj;
            SphereCollider tempSC;
            string[] items = line.Split(' ');
            tempObj = Instantiate(checkPointPreFab, new Vector3(float.Parse(items[0]) * 0.0254f, float.Parse(items[1]) * 0.0254f, float.Parse(items[2]) * 0.0254f), Quaternion.identity);
            
            //Collisions c = tempObj.GetComponent("Collisions") as Collisions;
            //c.lcp = this;

            tempSC = tempObj.GetComponent<SphereCollider>();
            tempSC.radius = (30.0f * scaleFactor * 12.0f / 5.0f);
            cpArray[index] = tempObj;
            if (firstLine == true)
            {
                
                player.transform.position = new Vector3(float.Parse(items[0]) * 0.0254f, float.Parse(items[1]) * 0.0254f, float.Parse(items[2]) * 0.0254f);
                oldCP = player.transform.position;
                firstLine = false;
                secondLine = true;
                Destroy(cpArray[index].gameObject);
            }
            //if (secondLine == true)
            //{
            //    Debug.Log("secondLine");
            //    //Vector3 temp = new Vector3(int.Parse(items[0]), player.transform.position.y, int.Parse(items[2]));
            //    //player.transform.LookAt(tempObj.transform);
            //    //cam.transform.LookAt(tempObj.transform);
            //    secondLine = false;
            //}
            //Debug.Log(int.Parse(items[0]) + ", " + int.Parse(items[1]) + ", " + int.Parse(items[2]));
            index++;
        }
        //Debug.Log(Vector3.Distance(cpArray[0].transform.position, player.transform.position));

        /* ------------------------------------------------------ */
        /* initialize */
        Behaviour halo = cpArray[currentCP].GetComponent("Halo") as Behaviour;
        halo.enabled = true;
        SphereCollider sphere = cpArray[currentCP].GetComponent<SphereCollider>();
        sphere.enabled = true;
        

        
    }

    private void Update()
    {
        wayFind();
    }

    public void displayTime()
    {
        if (stopTime == false)
        {
            gameTime += Time.deltaTime;
            gameTimeText.text = Math.Round(gameTime, 2) + "\ts";
            gameTimeTextBack.text = Math.Round(gameTime, 2) + "\ts";
        }
        

        if (stopTime == true)
        {
            finishTime.text =  Math.Round(gameTime, 2) + "\ts";
            gameTimeText.text = "";
            gameTimeTextBack.text = "";
            finishTime.GetComponent<ParticleSystem>().Play();
        }
    }

    public void displayCheck(String textIn)
    {
        headsUp.text = textIn;
    }

    private void wayFind()
    {
        if (currentCP < count)
        {
            arrow.transform.LookAt(cpArray[currentCP].transform);
            arrow.transform.Rotate(Vector3.right * 90);
            dist = (int)((Vector3.Distance(cpArray[currentCP].transform.position, player.transform.position) / 12.0f) / scaleFactor);
            laZr.SetPosition(0, cpArray[currentCP].transform.position);
            laZr.SetPosition(1, (cam.transform.position - new Vector3(0,2,0)));
            laZr2.SetPosition(0, cpArray[currentCP].transform.position);
            laZr2.SetPosition(1, oldCP);
            //laZr2.SetPosition(1, cpArray[(currentCP - 1)].transform.position);
            text.text = dist + " ft";
            textBack.text = dist + " ft";
        }
        else
        {
            stopTime = true;
            text.text = "";
            textBack.text = "";
            GameObject[] z = GameObject.FindGameObjectsWithTag("Arrow");
            foreach (GameObject a in z)
            {
                a.SetActive(false);
            }
        }
        
        
    }

    public void collisionCP()
    {
        Behaviour halo = cpArray[currentCP].GetComponent("Halo") as Behaviour;
        halo.enabled = false;
        SphereCollider sphere = cpArray[currentCP].GetComponent<SphereCollider>();
        sphere.enabled = false;
        oldCP = cpArray[currentCP].transform.position;
        Destroy(cpArray[currentCP].gameObject);
        //Debug.Log("collision!!!!");
        
        
        currentCP++;
        Debug.Log("currentCP: " + currentCP);
        if (currentCP < count)
        {
            /* sound for checkpoint reached */
            playAudio.setCP();
            halo = cpArray[currentCP].GetComponent("Halo") as Behaviour;
            halo.enabled = true;
            sphere = cpArray[currentCP].GetComponent<SphereCollider>();
            sphere.enabled = true;
            
        }
        else
        {
            /* play end sound */
            playAudio.setFinish();
            laZr.enabled = false;
            laZr2.enabled = false;
            ce.raceEnd();
        }
            
    }

    public void collisionEV()
    {
        player.transform.position = oldCP;
    }

    public void test()
    {
        Debug.Log("test");
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.name == "Player")
    //    {
    //        Debug.Log("collision!!!!");
    //    }
    //}
}
