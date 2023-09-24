using Godot;
using System;
using System.Reflection;

public partial class ProjectileComponent : Area2D
{
	[Export] float projectileSpeed = 2000.0f;
	Timer timer;
	Node2D parentProjectile;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		parentProjectile = GetParent<Node2D>();
		timer = GetNode<Timer>("Timer");
		timer.Timeout += OnTimerTimeout;
		//BodyEntered += OnBodyEntered;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		parentProjectile.Position -= new Vector2(0, projectileSpeed * (float)delta);
	}

	void OnBodyEntered(PhysicsBody2D body)
	{
		// if (((Node)collision.GetCollider()).IsInGroup("Enemies") )
		// 	{
		if (body.IsInGroup("Enemies"))
		{
			HitEnemy(body); 
			parentProjectile.QueueFree();
			GD.Print("Hit something " + body.Name);
		}
	}

	void HitEnemy(PhysicsBody2D body) {
		if (body is KlaedScout klaedScout)
		{
			klaedScout.Hit();
		}
	}

	void OnTimerTimeout()
	{
		parentProjectile.QueueFree();
	}
}
