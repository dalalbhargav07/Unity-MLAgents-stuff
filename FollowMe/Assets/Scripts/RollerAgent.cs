using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class RollerAgent : Agent
{
	private float previousDistance;
	private const float BoundaryofPlatform = 5.0f;
	private float speed = 8.0f;

	public Transform Target;
	Vector3 center;
	int DeathCount, GoalCount = 1;

	Rigidbody rb;
	// Use this for initialization
	void Start()
	{
		center = this.transform.position - new Vector3(0, 0.4f, 0);
		rb = GetComponent<Rigidbody>();
	}


	public override void AgentReset()
	{
		if (this.transform.position.y < -1.0)
		{
			//Agent Fall
			this.transform.position = center;
			this.rb.angularVelocity = Vector3.zero;
			this.rb.velocity = Vector3.zero;
		}
		else
		{
			//Move target to new spot 
			var range = 8f;

			Target.position = new Vector3(Random.value * range - range / 2, 0.4f, Random.value * range - range / 2) + center;

		}
	}

	public override void CollectObservations()
	{
		//Calculate Relative position
		Vector3 relativePosition = Target.position - this.transform.position;

		//Relative Position'
		AddVectorObs(relativePosition.x / BoundaryofPlatform);
		AddVectorObs(relativePosition.z / BoundaryofPlatform);

		//Distance to edges of pl;atform
		AddVectorObs((this.transform.position.x + BoundaryofPlatform) / BoundaryofPlatform);
		AddVectorObs((this.transform.position.x - BoundaryofPlatform) / BoundaryofPlatform);
		AddVectorObs((this.transform.position.z + BoundaryofPlatform) / BoundaryofPlatform);
		AddVectorObs((this.transform.position.z - BoundaryofPlatform) / BoundaryofPlatform);

		// Agent velocity
		AddVectorObs(rb.velocity.x / BoundaryofPlatform);
		AddVectorObs(rb.velocity.z / BoundaryofPlatform);
	}


	public override void AgentAction(float[] vectorAction, string textAction)
	{
		var controlSignal = new Vector3(vectorAction[0], 0f, vectorAction[1]);
		rb.AddForce(controlSignal * speed);
		//Rewards

		float distanceToTarget = Vector3.Distance(this.transform.position,
											  Target.position);

		// Reached target
		if (distanceToTarget < 1.42f)
		{
			AddReward(1.0f);
			GoalCount++;
			Done();
		}

		// Time penalty
		AddReward(-0.05f);

		// Getting closer to target
		if (distanceToTarget < previousDistance)
		{
			AddReward(0.04f);
		}
		previousDistance = distanceToTarget;


		// Fell off platform
		if (this.transform.position.y < -1.0)
		{
			AddReward(-1.0f);
			DeathCount++;
			Done();
		}

		Monitor.Log("Goal:", GoalCount.ToString());
		Monitor.Log("Death:", DeathCount.ToString());
		Monitor.Log("Reward:", GetCumulativeReward().ToString("F2"));
	}

}
