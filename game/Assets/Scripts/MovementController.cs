using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.greghilston {
    [RequireComponent(typeof(Rigidbody))]
    public class MovementController : MonoBehaviour {
        Rigidbody myRigidbody;

        void Start () {
            myRigidbody = GetComponent<Rigidbody> ();
        }

        public void move(Vector3 velocity) {
            myRigidbody.MovePosition(myRigidbody.position + velocity * Time.fixedDeltaTime);
        }

        public void lookAt(Vector3 lookPoint) {
            Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
            transform.LookAt(heightCorrectedPoint);
        }
    }
}