using Godot;
using System;

public partial class auto_cannon_projectile : Area2D
{
	float _projectile_speed = 2000.0f;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var timer = GetNode<Timer>("Timer");
		timer.Timeout += OnTimerTimeout;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position -= new Vector2(0, _projectile_speed * (float)delta);
	}

	void OnTimerTimeout()
	{
		QueueFree();
	}
}
