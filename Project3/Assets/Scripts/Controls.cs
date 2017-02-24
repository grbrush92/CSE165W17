using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap;

public class Controls : MonoBehaviour {

    public LeapProvider provider;
    public Camera cam;
    private float pitch, yaw, roll;
    private Vector3 dir;
    private float speed;
    private float time;
    private bool start;

    public TextMesh countDown;
    public Audio moveAudio;
    public Audio playAudio;

    public LoadCheckPoints lcp;

    // Use this for initialization
    void Start () {
        provider = FindObjectOfType<LeapProvider>() as LeapProvider;
        speed = 1.0f;
        time = 0.0f;
        start = true;
        playAudio.setStart();
    }
	
	// Update is called once per frame
	void Update () {
        Frame frame = provider.CurrentFrame;
        time += Time.deltaTime; // inc time for countdown 

        if (time < 5.0f) 
        {
            
            float newTime = 5.0f - time;
            int timeInt;
            timeInt = (int)newTime + 1;
            countDown.text = timeInt.ToString();
            /* countdown beep noise */
            
        }
        else
        {
            if (time < 6.0f)
            {
                countDown.text = "GO!";
            }
            else
            {
                countDown.text = "";
            }
            if (start == true)
            {
                start = false;
            }
        }
        if (start == false)
        {
            //start timer
            lcp.displayTime();
        }
        foreach (Hand hand in frame.Hands)
        {
            if(time > 5.0f)
            {

                if (hand.IsRight)
                {
                    dir = (hand.PalmPosition.ToVector3() - hand.WristPosition.ToVector3());
                }

                // gas 
                if (hand.IsLeft)
                {

                    if (hand.GrabStrength > 0.2f)
                    {
                        float dist = Vector3.Distance(hand.PalmPosition.ToVector3(), cam.transform.position);
                        speed = Mathf.Pow(dist * 10, 2);
                        /* sound for movement w/ pitch for faster speed */
                        moveAudio.setPitch(dist * 2);
                        //Debug.Log(speed);
                        transform.position = transform.position + (dir * speed);
                    }
                    else
                    {
                        moveAudio.setPitch(0);
                    }
                }
            }
        }
    }
    public void collisionEV()
    {
        time = 0.0f;
        moveAudio.setPitch(0);
        //playAudio.setStart();
    }
}
