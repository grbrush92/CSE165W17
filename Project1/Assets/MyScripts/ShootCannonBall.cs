using UnityEngine;
using VRStandardAssets.Utils;
using System;
using System.Collections;
using UnityEngine.UI;

namespace VRStandardAssets.Examples
{
    public class ShootCannonBall : MonoBehaviour
    {

        public Rigidbody projectile;
        public Transform shotPos;
        public float shotForce = 1000f;
        public float moveSpeed = 10f;
        private Vector3 direction;

        public void OnEnable()
        {
            BrickEvent.cannonHandle += shootBall;
        }

        public void OnDisable()
        {
            BrickEvent.cannonHandle -= shootBall;
        }

        void Update()
        {
            float h = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
            float v = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

            Ray ray = new Ray(shotPos.position, shotPos.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 500f))
            {
                direction = hit.point - shotPos.position;
                direction.Normalize();
            }
            //shotPos.Translate(new Vector3(h, v, 0));
        }

        public void shootBall()
        {
            Rigidbody shot = Instantiate(projectile, shotPos.position + direction, Quaternion.LookRotation(direction)) as Rigidbody;
            shot.AddForce(shotPos.forward * shotForce);
        }
    }
}


