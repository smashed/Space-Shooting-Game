using Godot;
using System;

public partial class klaed_scout : CharacterBody2D
{
	public float speed = 600.0f;
	bool isCollision;
	Timer bounceTimer;

	public override void _Ready()
	{
		bounceTimer = GetNode<Timer>("BounceTimer");
    	bounceTimer.Timeout += OnBounceTimerTimeout;
	}

	public override void _PhysicsProcess(double delta)
	{
		MoveAndSlide();
		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			var collision = GetSlideCollision(i);
			if (((Node)collision.GetCollider()).Name == "Player")
			{
				//GD.Print("I collided with ", ((Node)collision.GetCollider()).Name);
				//GD.Print("Velocity " + Velocity.Bounce(collision.GetNormal()));
				Velocity = Velocity.Bounce(collision.GetNormal()) + collision.GetColliderVelocity();
				Velocity *= 0.6f;
				isCollision = true;
				bounceTimer.Start();
			}
		}
		

		// var collision = MoveAndCollide(Velocity * (float)delta);
		// if (collision != null) 
		// {
		// 	GD.Print("I collided with ", ((Node)collision.GetCollider()).Name);
		// 	Velocity = Velocity.Bounce(collision.GetNormal()) + collision.GetColliderVelocity();
		// }
            
	}

	public void OnBounceTimerTimeout()
	{
		isCollision = false;
		Velocity = Vector2.Zero;
	}

	
}
