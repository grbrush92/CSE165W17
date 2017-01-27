using UnityEngine;
using VRStandardAssets.Utils;
using System;
using System.Collections;
using UnityEngine.UI;

namespace VRStandardAssets.Examples
{
    public class Disarm : MonoBehaviour
    {
        [SerializeField]
        private VRInteractiveItem InteractiveBrick;
        bool m_GazeOver;
        public float m_Timer;
        private Coroutine CountdownRoutine;
        public delegate void disarmEvent();
        public static event disarmEvent disarmHandler;
        public static event disarmEvent teleportHandler;


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
            Debug.Log("Begin Disarming");
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
            if (disarmHandler != null)
                disarmHandler();


            Debug.Log("Disarmed");
        }
    }
}
