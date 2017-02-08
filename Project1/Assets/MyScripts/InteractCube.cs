using UnityEngine;
using VRStandardAssets.Utils;
using System;
using System.Collections;
using UnityEngine.UI;

namespace VRStandardAssets.Examples
{
    // This script is a simple example of how an interactive item can
    // be used to change things on gameobjects by handling events.
    public class InteractCube : MonoBehaviour
    {         
        [SerializeField] private VRInteractiveItem m_InteractiveItem;
        bool m_GazeOver;
        float m_Timer;
        private Coroutine CountdownRoutine;

        private void Awake ()
        {
        }


        private void OnEnable()
        {
            m_InteractiveItem.OnOver += HandleOver;
            m_InteractiveItem.OnOut += HandleOut;
        }


        private void OnDisable()
        {
            m_InteractiveItem.OnOver -= HandleOver;
            m_InteractiveItem.OnOut -= HandleOut;
        }


        //Handle the Over event
        private void HandleOver()
        {
            Debug.Log("Show over state");
            //m_Renderer.material = m_OverMaterial;
            m_GazeOver = true;
            if (m_GazeOver)
                CountdownRoutine = StartCoroutine(Countdown());

        }


        //Handle the Out event
        private void HandleOut()
        {
            m_GazeOver = false;

            // If the coroutine has been started (and thus we have a reference to it) stop it.
            if (CountdownRoutine != null)
                StopCoroutine(CountdownRoutine);

            // Reset the timer and bar values.
            m_Timer = 0f;
        }

        private IEnumerator Countdown()
        {
            // When the bar starts to fill, reset the timer.
            m_Timer = 0f;

            // The amount of time it takes to fill is either the duration set in the inspector, or the duration of the radial.
            // float fillTime = m_SelectionRadial != null ? m_SelectionRadial.SelectionDuration : m_Duration;

            // Until the timer is greater than the fill time...
            while (m_Timer < 1f)
            {
                // ... add to the timer the difference between frames.
                m_Timer += Time.deltaTime;

                // Set the value of the slider or the UV based on the normalised time.
                //SetSliderValue(m_Timer / fillTime);

                // Wait until next frame.
                yield return null;

                // If the user is still looking at the bar, go on to the next iteration of the loop.
                if (m_GazeOver)
                    continue;

                // If the user is no longer looking at the bar, reset the timer and bar and leave the function.
                m_Timer = 0f;
                //SetSliderValue(0f);
                yield break;
            }

            // If the loop has finished the bar is now full.
            //m_BarFilled = true;

            // If anything has subscribed to OnBarFilled call it now.
            //if (OnBarFilled != null)
            //{
            //    OnBarFilled();
            //}


            // Play the clip for when the bar is filled.
            //m_Audio.clip = m_OnFilledClip;
            //m_Audio.Play();
            //Debug.Log("Audio Destroy");
            Destroy(m_InteractiveItem.gameObject);

            // If the bar should be disabled once it is filled, do so now.
            //if (m_DisableOnBarFill)
            //    enabled = false;
        }

    }

}