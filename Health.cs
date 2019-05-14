using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rogue {

public class Health : MonoBehaviour {

        public float health = 100;
        public GameObject explosionParticles;

        public void TakeDamage(float amount)
        {
            health -= amount;
            if(health <= 0f)
            {
                Die();
            }
        }
        void Die()
        {
            if(explosionParticles != null)
            {
                Instantiate(explosionParticles, transform.position, transform.rotation);
            }
           
            Destroy(gameObject);
        }
	 
}
}
