using Godot;
using System;
using System.Diagnostics;

public partial class player : CharacterBody2D
{
	public Weapon weapon = Weapon.AutoCannon;

	float speed = 600.0f;
	AnimationTree animationTree;
	AnimationPlayer animationPlayer;
	ship_weapons shipWeapon;
	ship_engine shipEngine;

    public override void _Ready()
    {
        animationTree = GetNode("AnimationTree") as AnimationTree;
		animationPlayer = GetNode("AnimationPlayer") as AnimationPlayer;
		shipWeapon = GetNode("Ship Weapons") as ship_weapons;
		shipEngine = GetNode("Ship Engine") as ship_engine;
    }

    public override void _PhysicsProcess(double delta)
	{
		Move();
		Fire();
		ShipEngine();
		ShipWeapon();
	}

	void Move() 
	{
		Vector2 velocity = Velocity;

		// Get the input direction and handle the movement/deceleration.
		Vector2 direction = Input.GetVector("Left", "Right", "Up", "Down");
		if (direction != Vector2.Zero)
		{
			velocity = direction * speed;
			animationTree.Set("parameters/conditions/is_moving", true);
			animationTree.Set("parameters/conditions/idle", false);
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, speed);
			animationTree.Set("parameters/conditions/idle", true);
			animationTree.Set("parameters/conditions/is_moving", false);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	void Fire()
	{
		// Handle Shoot.
		if (Input.IsActionJustPressed("Fire"))
			animationPlayer.Play(shipWeapon.fireProjectile);
	}

	void ShipWeapon()
	{
		if (Input.IsActionJustPressed("Weapon 1"))
			shipWeapon.SetWeapon(Weapon.AutoCannon);
		if (Input.IsActionJustPressed("Weapon 2"))
			shipWeapon.SetWeapon(Weapon.BigSpaceGun);
		if (Input.IsActionJustPressed("Weapon 3"))
			shipWeapon.SetWeapon(Weapon.Rockets);
		if (Input.IsActionJustPressed("Weapon 4"))
			shipWeapon.SetWeapon(Weapon.Zapper);
	}

	void ShipEngine()
	{
		if (Input.IsActionJustPressed("Engine 1"))
			shipEngine.SetEngine(Engine.Base);
		if (Input.IsActionJustPressed("Engine 2"))
			shipEngine.SetEngine(Engine.BigPulse);
		if (Input.IsActionJustPressed("Engine 3"))
			shipEngine.SetEngine(Engine.Burst);
		if (Input.IsActionJustPressed("Engine 4"))
			shipEngine.SetEngine(Engine.Supercharged);
	}

	
}
