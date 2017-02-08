using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VRStandardAssets.Examples
{
    public class ChangeMesh : MonoBehaviour
    {
        [SerializeField]
        private MeshFilter curFilter;
        [SerializeField]
        private Mesh Cannon;
        [SerializeField]
        private Mesh Laser;
        [SerializeField]
        private Material cannonMaterial;
        [SerializeField]
        private Material laserMaterial;
        [SerializeField] private Renderer toggleRender;
        private bool isLaser = false;
        private bool lastItemLaser = false;
        private bool disarmed = true;
        private Vector3 lastTransform;
        private Vector3 shrink = new Vector3(0, 0, -0.2f);
        
        // Use this for initialization
        private void OnEnable()
        {
            WeaponEvent.weaponHandler += toggleMesh;
            Disarm.disarmHandler += disarm;
            Reset.restartHandler += reset;
        }

        private void OnDisable()
        {
            WeaponEvent.weaponHandler -= toggleMesh;
            Disarm.disarmHandler -= disarm;
            Reset.restartHandler -= reset;
        }

        public void toggleMesh()
        {
            if (disarmed == false)
                isLaser = !isLaser;
            disarmed = false;
            transform.localScale = new Vector3 (0.25f,0.25f,0.25f);
            if (isLaser)
            {
                //transform.localScale -= new Vector3(0, 0, 1);
                toggleRender.material = laserMaterial;
                curFilter.mesh = Laser;
                lastTransform = shrink;
                transform.localScale += lastTransform;
                transform.Translate(new Vector3(0, 0, -.1f));
                lastItemLaser = true;

            }
            else
            {
                //transform.localScale -= new Vector3(0, 0, 0.2f);
                toggleRender.material = cannonMaterial;
                curFilter.mesh = Cannon;
                if (lastItemLaser)
                {
                    transform.Translate(new Vector3(0, 0, 0.1f));
                    lastItemLaser = false;
                }
                //lastTransform = new Vector3(0, 0, 1);
                //transform.localScale += lastTransform;
            }
            
            
        }

        private void disarm()
        {
            disarmed = true;
            curFilter.mesh = null;
        }

        private void reset()
        {
            disarmed = true;
            curFilter.mesh = null;
            if (isLaser)
            {
                isLaser = !isLaser;
            }
        }

    }
}
