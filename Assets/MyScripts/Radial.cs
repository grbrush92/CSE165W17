using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;


namespace VRStandardAssets.Examples
{ 
    public class Radial : MonoBehaviour
    {
        Image radial;
        float current;
        float fillTime;
        public Camera viewCamera;
        
        // Use this for initialization
        void Start()
        {
            radial = this.GetComponent<Image>();
            fillTime = 1f;
        }

        // Update is called once per frame
        void Update()
        {
            //BrickEvent hitBrick;
            //Ray ray = new Ray(viewCamera.transform.position, viewCamera.transform.rotation * Vector3.forward);
            //RaycastHit hit;

            //if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            //{
            //    Debug.Log(hit.point);
            //    hitBrick = hit.collider.gameObject.GetComponent<BrickEvent>();
            //    current = hitBrick.m_Timer;
            //    //Debug.Log(current);
            //}

            //radial.fillAmount = current / fillTime;
            UpdateRay();
        }

        private void UpdateRay()
        {
            // Create a gaze ray pointing forward from the camera
            BrickEvent hitBrick;
            Color color = new Color();
            WeaponEvent weapon;
            Disarm disarm;
            Reset reset;
            Teleport teleport;
            Ray ray = new Ray(viewCamera.transform.position, viewCamera.transform.rotation * Vector3.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.name == "Brick(Clone)")
                {
                    hitBrick = hit.collider.gameObject.GetComponent<BrickEvent>();
                    radial.color = new Color32(0x09, 0xFF, 0x00, 0x50);
                    current = hitBrick.m_Timer;
                    
                }
                else if (hit.collider.name == "WeaponToggleText")
                {
                    weapon = hit.collider.gameObject.GetComponent<WeaponEvent>();
                    radial.color = new Color32(0xF1, 0xFF, 0x00, 0x50);
                    current = weapon.m_Timer;
                }
                else if (hit.collider.name == "DisarmText")
                {
                    disarm = hit.collider.gameObject.GetComponent<Disarm>();
                    radial.color = new Color32(0x28, 0x00, 0xFF, 0x50);
                    current = disarm.m_Timer;
                }
                else if (hit.collider.name == "ResetScene")
                {
                    reset = hit.collider.gameObject.GetComponent<Reset>();
                    ColorUtility.TryParseHtmlString("FF0000FF", out color);
                    radial.color = new Color32(0xFF, 0x00, 0x00, 0x50);
                    current = reset.m_Timer;
                }
                else if (hit.collider.name == "Ground")
                {
                    teleport = hit.collider.gameObject.GetComponent<Teleport>();
                    radial.color = new Color32(0x55, 0x55, 0x55, 0x50);
                    current = teleport.m_Timer / 2f;
                }
                else
                {
                    current = 0f;
                }
                
                
            }
            else
            {
                current = 0f;
            }

            radial.fillAmount = current / fillTime;

        }
    }
}


