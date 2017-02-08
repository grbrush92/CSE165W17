using UnityEngine;
using VRStandardAssets.Utils;
using System;
using System.Collections;
using UnityEngine.UI;

namespace VRStandardAssets.Examples
{
    public class BrickEvent : MonoBehaviour
    {
        [SerializeField]
        private VRInteractiveItem InteractiveBrick;
        [SerializeField]
        private Transform playerCam;
        [SerializeField]
        private SelectionRadial m_SelectionRadial;         // This controls when the selection is complete.

        bool m_GazeOver;
        public float m_Timer;
        private Coroutine CountdownRoutine;
        public bool isLaser = true;
        public bool LockedAndLoaded = false;

        public delegate void cannonEvent();
        public static event cannonEvent cannonHandle;
        public delegate void audioEvent();
        public static event audioEvent audioLaser;
        public static event audioEvent audioCannon;
        public static event audioEvent audioReload;
        //public Image radial;

        private void OnEnable()
        {
            InteractiveBrick.OnOver += HandleOver;
            InteractiveBrick.OnOut += HandleOut;
            WeaponEvent.weaponHandler += toggleWeapon;
            Disarm.disarmHandler += noWeapon;
        }


        private void OnDisable()
        {
            InteractiveBrick.OnOver -= HandleOver;
            InteractiveBrick.OnOut -= HandleOut;
            WeaponEvent.weaponHandler -= toggleWeapon;
            Disarm.disarmHandler -= noWeapon;
        }

        private void HandleOver()
        {
            m_SelectionRadial.Show();
            m_GazeOver = true;
            if (m_GazeOver)
                CountdownRoutine = StartCoroutine(Countdown());
        }

        private void HandleOut()
        {
            m_SelectionRadial.Hide();
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
                //radial.fillAmount = m_Timer / 1f;
                //Debug.Log("m_Timer: " + m_Timer);
                yield return null;
                if (m_GazeOver)
                    continue;
                m_Timer = 0f;
                yield break;
            }
            destroyBrick();    
        }

        public void destroyBrick()
        {
            if (LockedAndLoaded)
            {
                if (isLaser)
                {
                    Debug.Log("pew pew");
                    if (audioLaser != null)
                        audioLaser();
                    Destroy(InteractiveBrick.gameObject);
                }
                    
                else
                {
                    Debug.Log("bang bang");
                    if (audioCannon != null)
                        audioCannon();
                    if (cannonHandle != null)
                        cannonHandle();
                }        
            }          
        }

        public void toggleWeapon()
        {
            if (LockedAndLoaded)
                isLaser = !isLaser;
            LockedAndLoaded = true;
            if (audioReload != null)
                audioReload();
        }

        public void noWeapon()
        {
            LockedAndLoaded = false;
        }
    }
}
