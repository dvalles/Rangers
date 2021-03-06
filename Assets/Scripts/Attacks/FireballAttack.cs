﻿using UnityEngine;
using Assets.Scripts.Player;
using Assets.Scripts.Level;

namespace Assets.Scripts.Attacks
{
	/// <summary>
	/// Attack that simulates an explosion and damages and sends affected objects flying
	/// </summary>
    public class FireballAttack : SpawnAttack
    {
		private float maxSize = 10f;

		/// <summary>
		/// Layers that are not affected by the explosion
		/// </summary>
		[SerializeField]
		private LayerMask doNotActivate;

        void Start()
        {
            damage = 5;
			transform.localScale = new Vector3 (maxSize, maxSize, maxSize);
			Destroy(GetComponent<Collider>(),0.1f);
			Destroy(gameObject,1f);
        }

        void OnTriggerEnter(Collider col)
        {
			if ((doNotActivate.value & (1 << col.gameObject.layer)) != 0) return;
			// Hit the player and damage
			if (col.transform.root.tag.Equals ("Player"))
			{
				Controller controller = col.transform.root.GetComponent<Controller>();
				controller.LifeComponent.ModifyHealth(-damage);
				hitPlayer = controller.ID;
                // Apply an explosion force to the object hit
                col.transform.root.GetComponent<Rigidbody>().AddExplosionForce(200f, transform.position, 10);
            }
            else if (col.transform.tag.Equals("Target"))
            {
                Debug.Log(col.transform.name);
                col.gameObject.GetComponent<Target>().TargetHit(fromPlayer);
            }
        }
    } 
}
