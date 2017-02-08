using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace VRStandardAssets.Examples
{
    public class AudioScript : MonoBehaviour
    {

        [SerializeField]
        private AudioSource aSource;
        [SerializeField]
        private AudioClip laser;
        [SerializeField]
        private AudioClip cannon;
        [SerializeField]
        private AudioClip reload;

        void Awake()
        {
            this.enabled = true;
        }

        void Start()
        {
            this.enabled = true;
        }

        private void onEnable()
        {
            Debug.Log("enabled");
            BrickEvent.audioLaser += playLaser;
            BrickEvent.audioCannon += playCannon;
            BrickEvent.audioReload += playReload;
        }

        private void onDisable()
        {
            BrickEvent.audioLaser -= playLaser;
            BrickEvent.audioCannon -= playCannon;
            BrickEvent.audioReload -= playReload;
        }

        private void playLaser()
        {
            //aSource.clip = laser;
            aSource.PlayOneShot(laser);
        }

        private void playCannon()
        {
            //aSource.clip = cannon;
            aSource.PlayOneShot(cannon);
            Debug.Log("cannon sound");
        }

        private void playReload()
        {
            //aSource.clip = reload;
            aSource.PlayOneShot(reload);
        }
    }
}


