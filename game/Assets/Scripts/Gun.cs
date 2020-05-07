﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.MyCompany.MyGame {
    public class Gun : MonoBehaviour {
        public Transform muzzle;
        public Projectile projectile;
        public float msBetweenShots = 100;
        public float muzzleVelocity = 35;
        private float nextShotTime;

        public void shoot() {
            if (Time.time > nextShotTime) {
                nextShotTime = Time.time + msBetweenShots / 1000;
                Projectile newProjectile = Instantiate (projectile, muzzle.position, muzzle.rotation) as Projectile;
                newProjectile.setSpeed (muzzleVelocity);
            }
        }
    }
}