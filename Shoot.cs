using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rogue {

public class Shoot : MonoBehaviour {


        public float nextFire;
        public float fireRate = 15f;
        public int maxAmmo = 10;
        public float damage = 10f;
        public float range = 100f;
        public int currentAmmo = 1;
        public float reloadTime = 1f;
        public Camera fpsCamera;
        public ParticleSystem muzzleflash;
        public GameObject impactEffect;
        public float impactForce = 30f;


        // Use this for initialization
        void Start () {
         

        }
	
	// Update is called once per frame
	void Update () {
            if (Input.GetButtonUp("fire1") && Time.time > nextFire)
            {

                nextFire = Time.time + fireRate;
                ShootGun();
            }
        }
       public void ShootGun()
        {
            muzzleflash.Play();
            RaycastHit hit;
            if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range))
            {
                Debug.Log(hit.transform.name);
                Health health = hit.transform.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(damage);
                }
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                }
                GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 2f);
            }
        }

	
}
}
