﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Data;

namespace Assets.Scripts.Player.AI
{
	/// <summary> Interface for AI to control a character. </summary>
	public class AIController : Controller
    {
		/// <summary> The policies used to decide the AI's actions. </summary>
		private List<IPolicy> policies;

		/// <summary> The speed that the character is moving at. </summary>
		[SerializeField]
		internal float runSpeed = 0;

		/// <summary> Whether the character is jumping. </summary>
		[SerializeField]
		internal bool jump = false;
		/// <summary> Whether the character is sliding. </summary>
		[SerializeField]
		internal bool slide = false;

		/// <summary> Whether the character is aiming a shot. </summary>
		[SerializeField]
		internal bool aiming = false;
		/// <summary> Whether the character was aiming its shot on the last tick. </summary>
		private bool wasAiming = false;
		/// <summary> Vector in the direction that the character is aiming. </summary>
		[SerializeField]
		internal Vector3 aim = Vector3.zero;

		/// <summary> The default movement speed of the character. </summmary>
		[SerializeField]
		private float defaultMoveSpeed = 1;

		/// <summary> The opponent of this character. </summary>
		public Controller opponent;

		/// <summary> Initializes the AI policy to use. </summary>
		void Start()
		{
			policies = new List<IPolicy>();
			policies.Add(new StandShoot(1, 0.6f));
			policies.Add(new RushEnemy(5));
			foreach (Controller controller in GameManager.instance.AllPlayers)
			{
				if (controller != this)
				{
					opponent = controller;
					break;
				}
			}
			base.Start();
		}
		
		/// <summary> Moves the agent every tick. </summary>
		void Update()
		{
			if (opponent == null || life.Health <= 0) {
				aiming = false;
				return;
			} else {
				foreach (IPolicy policy in policies) {
					policy.ChooseAction(this);
				}
			}

			if (jump)
			{
				parkour.Jump();
			}

			if (slide)
			{
				parkour.SlideOn();
			}
			else
			{
				parkour.SlideOff();
			}

			if (aiming)
			{
				archery.UpdateFirePoint(Vector3.Normalize(aim));
				wasAiming = true;
			}
			else if (wasAiming)
			{
				wasAiming = false;
				archery.Fire();
			}
			else
			{
				archery.AimUpperBodyWithLegs();
			}
		}

		/// <summary> Updates the player's running movement. </summary>
		void FixedUpdate()
		{
			if (life.Health > 0)
			{
				parkour.Locomote(runSpeed);
			}
		}

		/// <summary>
		/// Sets the character's run speed in the direction of the given number.
		/// </summary>
		/// <param name="direction">The direction to set the run speed to.</param>
		internal void SetRunInDirection(float direction) {
			if (direction > 0) {
				runSpeed = defaultMoveSpeed;
			} else if (direction < 0) {
				runSpeed = -defaultMoveSpeed;
			} else {
				runSpeed = 0;
			}
		}

		/// <summary>
		/// Gets a vector of the difference between the opponent position and the AI position.
		/// </summary>
		/// <returns>A vector of the difference between the opponent position and the AI position.</returns>
		internal Vector3 GetOpponentDistance()
		{
			return opponent.transform.position - transform.position;
		}
	}
}