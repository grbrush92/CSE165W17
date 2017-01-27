using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;
using System;
using UnityEngine.UI;

namespace VRStandardAssets.Examples
{
    public class Reset : MonoBehaviour
    {
        public delegate void destroyEvent();
        public static event destroyEvent destroyHandler;
        public Transform viewCamera;
        [SerializeField]
        private VRInteractiveItem InteractiveBrick;
        bool m_GazeOver;
        public float m_Timer;
        private Coroutine CountdownRoutine;
        public static event destroyEvent restartHandler;
        public Transform player;

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

        private void HandleOver()
        {
            Debug.Log("Begin Weapon Change");
            m_GazeOver = true;
            if (m_GazeOver)
                CountdownRoutine = StartCoroutine(Countdown());

        }

        private void HandleOut()
        {
            m_GazeOver = false;
            if (CountdownRoutine != null)
                StopCoroutine(CountdownRoutine);
            m_Timer = 0f;
        }

        private IEnumerator Countdown()
        {
            m_Timer = 0f;
            while (m_Timer < 1f)
            {
                m_Timer += Time.deltaTime;
                yield return null;
                if (m_GazeOver)
                    continue;
                m_Timer = 0f;
                yield break;
            }
            if (destroyHandler != null)
                destroyHandler();

            if (restartHandler != null)
                restartHandler();

            viewCamera.transform.position = new Vector3(0, 4, 0);
            player.transform.position = new Vector3(0, 4, 0);

            Debug.Log("Destroy");
        }
    }
}

