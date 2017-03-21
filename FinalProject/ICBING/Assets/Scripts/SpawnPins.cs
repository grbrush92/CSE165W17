using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPins : MonoBehaviour {

    private void Start()
    {
        spawnPins();
    }

    public GameObject pin;

	public void removePins()
    {
        foreach (GameObject tempObj in GameObject.FindGameObjectsWithTag("Grabbable"))
        {
            Destroy(tempObj);
        }
        foreach (GameObject tempObj in GameObject.FindGameObjectsWithTag("Reset"))
        {
            //if (tempObj.name.Contains("Bowling_Pin 1"))
            //{
            //    Destroy(tempObj);
            //}
            Destroy(tempObj);
        }
        //GameObject temp = FindObjectOfType<GameObject>.name();
    }

    public void spawnPins()
    {
        Instantiate(pin, new Vector3(0, 0.35f, 6.0f), new Quaternion(0, 0, 0, 0));
        Instantiate(pin, new Vector3(.1f, 0.35f, 6.1f), new Quaternion(0, 0, 0, 0));
        Instantiate(pin, new Vector3(-.1f, 0.35f, 6.1f), new Quaternion(0, 0, 0, 0));
        Instantiate(pin, new Vector3(0, 0.35f, 6.2f), new Quaternion(0, 0, 0, 0));
        Instantiate(pin, new Vector3(0.2f, 0.35f, 6.2f), new Quaternion(0, 0, 0, 0));
        Instantiate(pin, new Vector3(-0.2f, 0.35f, 6.2f), new Quaternion(0, 0, 0, 0));
        Instantiate(pin, new Vector3(-0.3f, 0.35f, 6.3f), new Quaternion(0, 0, 0, 0));
        Instantiate(pin, new Vector3(-0.15f, 0.35f, 6.3f), new Quaternion(0, 0, 0, 0));
        Instantiate(pin, new Vector3(0, 0.35f, 6.3f), new Quaternion(0, 0, 0, 0));
        Instantiate(pin, new Vector3(0.15f, 0.35f, 6.3f), new Quaternion(0, 0, 0, 0));
        Instantiate(pin, new Vector3(0.3f, 0.35f, 6.3f), new Quaternion(0, 0, 0, 0));
    }
}
