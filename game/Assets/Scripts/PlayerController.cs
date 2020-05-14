using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace com.greghilston {
    [RequireComponent(typeof(MovementController))]
    [RequireComponent(typeof(GunController))]
    public class PlayerController : MonoBehaviour {
        [Tooltip("The speed of the character")]
        [SerializeField]
        private float movementSpeed = 5;
        private Camera viewCamera;
        private MovementController movementController;
        private GunController gunController;
        private Vector3 moveVelocity;
        private Vector3 lookPoint;
        private bool shouldShoot;

        void Start () {
            this.movementController = GetComponent<MovementController> ();
            this.gunController = GetComponent<GunController> ();
            this.viewCamera = Camera.main;
        }

        void captureMovementInput() {
            Vector3 moveInput = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));
            this.moveVelocity = moveInput.normalized * movementSpeed;
        }

        void captureLookInput() {
            Ray ray = viewCamera.ScreenPointToRay (Input.mousePosition);
            Plane groundPlane = new Plane (Vector3.up, Vector3.zero);
            float rayDistance;
            if (groundPlane.Raycast(ray, out rayDistance)) {
                this.lookPoint = ray.GetPoint(rayDistance);
                Debug.DrawLine(ray.origin, this.lookPoint, Color.red);
            }
        }

        void captureWeaponInput() {
            this.shouldShoot = Input.GetMouseButton(0);
        }

        void Update() {
            this.captureMovementInput();
            this.captureLookInput();
            this.captureWeaponInput();
        }

        void processMovementInput() {
            this.movementController.move(this.moveVelocity);
        }

        void processLookInput() {
            this.movementController.lookAt(this.lookPoint);
        }

        void processWeaponInput() {
            // if (this.shouldShoot) {
            //     gunController.shoot();
            // }
        }

        void FixedUpdate() {
            this.processMovementInput();
            this.processLookInput();
            this.processWeaponInput();
        }
    }
}