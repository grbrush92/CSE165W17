using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour {

    public AudioClip music;
    public AudioClip rollClip;
    public AudioClip crashClip;
    public AudioClip finishClip;
    public AudioClip finishMusic;
    public AudioSource source;

    public void setStart()
    {
        source.PlayOneShot(music);
    }
    public void setCrash()
    {
        source.PlayOneShot(crashClip);
    }
    public void setRoll()
    {
        source.PlayOneShot(rollClip);
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

    public void setStop()
    {
        source.Stop();
    }
}
