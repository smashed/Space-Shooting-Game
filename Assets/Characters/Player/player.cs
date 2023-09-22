using Godot;
using System;

public partial class player : CharacterBody2D
{
	const float Speed = 600.0f;
	//@onready var anim = get_node("AnimationPlayer")

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Handle Shoot.
		//if (Input.IsActionJustPressed("ui_accept"))
			

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("Left", "Right", "Up", "Down");
		if (direction != Vector2.Zero)
		{
			velocity = direction * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
