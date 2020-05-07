using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.MyCompany.MyGame {
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(GunController))]
public class ArdPlayer : MonoBehaviour {
    public float moveSpeed = 5;

        private Camera viewCamera;
        private PlayerController controller;
        private GunController gunController;
        private Vector3 moveVelocity;
        private bool shouldShoot;

        void Start () {
            this.controller = GetComponent<PlayerController> ();
            this.gunController = GetComponent<GunController> ();
            this.viewCamera = Camera.main;
        }

        void captureMovementInput() {
            Vector3 moveInput = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));
            this.moveVelocity = moveInput.normalized * moveSpeed;
        }

        void captureWeaponInput() {
            this.shouldShoot = Input.GetMouseButton(0);
        }

        void Update() {
            this.captureMovementInput();
            this.captureWeaponInput();
        }

        void processMovementInput() {
            this.controller.Move (this.moveVelocity);
        }

        void processLookInput() {
            Ray ray = viewCamera.ScreenPointToRay (Input.mousePosition);
            Plane groundPlane = new Plane (Vector3.up, Vector3.zero);
            float rayDistance;
            if (groundPlane.Raycast(ray,out rayDistance)) {
                Vector3 point = ray.GetPoint(rayDistance);
                //Debug.DrawLine(ray.origin,point,Color.red);
                controller.LookAt(point);
            }
        }

        void processWeaponInput() {
            if (this.shouldShoot) {
                gunController.shoot();
            }
        }

        void FixedUpdate() {
            processMovementInput();
            processLookInput();
            processWeaponInput();
        }
    }
}