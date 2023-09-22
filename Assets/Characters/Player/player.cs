using Godot;
using System;
using System.Reflection.Metadata.Ecma335;

public partial class player : CharacterBody2D
{
	const float Speed = 600.0f;
	AnimationTree animationTree;

    public override void _Ready()
    {
        animationTree = GetNode("AnimationTree") as AnimationTree;
		
    }

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
			animationTree.Set("parameters/conditions/is_moving", true);
			animationTree.Set("parameters/conditions/idle", false);
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
			animationTree.Set("parameters/conditions/idle", true);
			animationTree.Set("parameters/conditions/is_moving", false);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
