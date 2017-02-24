using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour {

    public AudioClip startClip;
    public AudioClip cpClip;
    public AudioClip crashClip;
    public AudioClip finishClip;
    public AudioClip finishMusic;
    public AudioSource source;

    public void setStart()
    {
        source.PlayOneShot(startClip);
    }
    public void setCrash()
    {
        source.PlayOneShot(crashClip);
    }
    public void setCP()
    {
        source.PlayOneShot(cpClip);
    }
    public void setFinish()
    {
        //source.PlayOneShot(finishClip);
        source.PlayOneShot(finishMusic);
    }
    public void setPitch(float pitch)
    {
        source.pitch = pitch;
    }
}
