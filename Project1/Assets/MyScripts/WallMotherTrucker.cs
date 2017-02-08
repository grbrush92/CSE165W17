using System.Collections;
using System.Collections.Generic;
using VRStandardAssets.Utils;
using UnityEngine;
using System;

namespace VRStandardAssets.Examples
{
    public class WallMotherTrucker : MonoBehaviour
    {

        public Transform brick;
        [SerializeField]
        private VRInteractiveItem interactiveBrick;
        //public Transform target;
        public Vector3 vec, target;
        float r, x, y, z;
        bool offset;
        private Coroutine buildWall;

        [SerializeField]
        private AudioSource aSource;
        [SerializeField]
        private AudioClip laser;
        [SerializeField]
        private AudioClip cannon;
        [SerializeField]
        private AudioClip reload;

        private void OnEnable()
        {
            Reset.destroyHandler += destroyAll;
            Reset.restartHandler += Start;
            BrickEvent.audioLaser += playLaser;
            BrickEvent.audioCannon += playCannon;
            //BrickEvent.audioReload += playReload;
        }

        private void OnDisable()
        {
            Reset.destroyHandler -= destroyAll;
            Reset.restartHandler -= Start;
            BrickEvent.audioLaser -= playLaser;
            BrickEvent.audioCannon -= playCannon;
            //BrickEvent.audioReload -= playReload;
        }


        void Start()
        {
            buildWall = StartCoroutine(Build());
        }

        public IEnumerator Build()
        {
            r = 12.0f;
            z = 0.5f;
            offset = false;
            target = new Vector3(0, 0.5f, 0);
            for (int h = 0; h < 15; h++)
            {
                for (int a = 0; a < 36; a++)
                {
                    if (offset)
                    {
                        x = r * ((float)Math.Cos((a * 10.0f / 180.0f * (float)Math.PI) + 20.0f));
                        y = r * ((float)Math.Sin((a * 10.0f / 180.0f * (float)Math.PI) + 20.0f));
                    }
                    else
                    {
                        x = r * ((float)Math.Cos(a * 10.0f / 180.0f * (float)Math.PI));
                        y = r * ((float)Math.Sin(a * 10.0f / 180.0f * (float)Math.PI));
                    }
                    vec = new Vector3(x, z, y);
                    Vector3 relativePos = target - vec;
                    Instantiate(brick, vec, Quaternion.LookRotation(relativePos));
                }
                offset = !offset;
                z++;
                target.y = z;
                yield return new WaitForSeconds(0.05f);
            }
        }

        private void destroyAll()
        {
            if (buildWall != null)
                StopCoroutine(buildWall);
            GameObject o = GameObject.Find("Brick(Clone)");
            while (o)
            {
                o.name = "Brick(Clone) - deleted";
                Destroy(o);
                o = GameObject.Find("Brick(Clone)");
            }
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
    }
}

