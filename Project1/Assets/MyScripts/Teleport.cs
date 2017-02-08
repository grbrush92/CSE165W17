using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;
using System;
using UnityEngine.UI;

namespace VRStandardAssets.Examples
{
    public class Teleport : MonoBehaviour
    {
        public delegate void teleportEvent();
        public static event teleportEvent teleportHandler;
        public Camera viewCamera;
        public Transform player;
        [SerializeField]
        private VRInteractiveItem InteractiveBrick;
        bool m_GazeOver;
        public float m_Timer;
        private Coroutine CountdownRoutine;
        private Vector3 offset = new Vector3 (0, 2f, 0);
        private bool isRunning = false;
        private bool yesTeleport = true;

        private void OnEnable()
        {
            InteractiveBrick.OnOver += HandleOver;
            InteractiveBrick.OnOut += HandleOut;
        }


        private void OnDisable()
        {
            InteractiveBrick.OnOver -= HandleOver;
            InteractiveBrick.OnOut -= HandleOut;
        }

        void Update()
        {
            
        }

        private void HandleOver()
        {
            Debug.Log("Begin Teleport");
            m_GazeOver = true;
            if (m_GazeOver && (isRunning == false))
            {
                Debug.Log("starting teleport routine");
                CountdownRoutine = StartCoroutine(Countdown());
            }
                

        }

        private void HandleOut()
        {
            Debug.Log("Stop teleport");
            m_GazeOver = false;
            if (CountdownRoutine != null)
                StopCoroutine(CountdownRoutine);
            m_Timer = 0f;
            isRunning = false;
        }

        private IEnumerator Countdown()
        {
            isRunning = true;
            m_Timer = 0f;
            while (m_Timer < 2f)
            {
                m_Timer += Time.deltaTime;
                yield return null;
                if (m_GazeOver)
                    continue;
                m_Timer = 0f;
                yield break;
            }

            Ray ray = new Ray(viewCamera.transform.position, viewCamera.transform.rotation * Vector3.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.name == "Ground")
                {
                    //viewCamera.transform.position = hit.point + new Vector3(0, 4, 0);
                    player.transform.position = hit.point + new Vector3(0, 4, 0);
                    Debug.Log("teleport");
                }
                    
            }
            isRunning = false;
            HandleOver();
        }
    }
}

