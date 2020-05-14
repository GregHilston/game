using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.greghilston {
    public class Projectile : MonoBehaviour{
        private float speed = 10;

        public void setSpeed(float newSpeed) {
            this.speed = newSpeed;
        }
        
        void Update () {
            this.transform.Translate(Vector3.forward * Time.deltaTime * this.speed);
        }
    }
}