using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.greghilston {
    public class GunController : MonoBehaviour {
    public Transform weaponHold;
        public Gun startingGun;
        private Gun equippedGun;

        void Start() {
            if (startingGun != null) {
                equipGun(startingGun);
            }
        }

        public void equipGun(Gun gunToEquip) {
            if (equippedGun != null) {
                Destroy(equippedGun.gameObject);
            }
            equippedGun = Instantiate (gunToEquip, weaponHold.position,weaponHold.rotation) as Gun;
            equippedGun.transform.parent = weaponHold;
        }

        public void shoot() {
            if (equippedGun != null) {
                equippedGun.shoot();
            }
        }
    }
}