using Godot;
using System;

public partial class player : CharacterBody2D
{
	public Weapon weapon = Weapon.AutoCannon;

	public float speed = 600.0f;
	AnimationTree animationTree;
	AnimationPlayer animationPlayer;
	ship_weapons shipWeapon;
	ship_engine shipEngine;

	bool isCollision;
	Timer bounceTimer;

	ParallaxLayer background;

    public override void _Ready()
    {
        animationTree = GetNode("AnimationTree") as AnimationTree;
		animationPlayer = GetNode("AnimationPlayer") as AnimationPlayer;
		shipWeapon = GetNode("Ship Weapons") as ship_weapons;
		shipEngine = GetNode("Ship Engine") as ship_engine;
		background = GetNode("/root/Game/ParallaxBackground/TransparentBG") as ParallaxLayer;
		bounceTimer = GetNode<Timer>("BounceTimer");
    	bounceTimer.Timeout += OnBounceTimerTimeout;
    }

    public override void _PhysicsProcess(double delta)
	{
		Move(delta);
		Fire();
		ShipEngine();
		ShipWeapon();
	}

	void Move(double delta)
	{
		// Get the input direction and handle the movement/deceleration.
		Vector2 direction = Input.GetVector("Left", "Right", "Up", "Down");
		if (!isCollision)
			Velocity = direction * speed;

		if (Velocity > Vector2.Zero || Velocity < Vector2.Zero) //direction != Vector2.Zero)
		{
			animationTree.Set("parameters/conditions/is_moving", true);
			animationTree.Set("parameters/conditions/idle", false);
			if (!IsOnWall())
				background.MotionOffset -= direction;
		}
		else
		{
			animationTree.Set("parameters/conditions/idle", true);
			animationTree.Set("parameters/conditions/is_moving", false);
		}

		MoveAndSlide();
		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			var collision = GetSlideCollision(i);
			if (((Node)collision.GetCollider()).IsInGroup("Enemies") )
			{
				//GD.Print("I collided with ", ((Node)collision.GetCollider()).GetGroups());
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
		// 	Velocity = Velocity.Slide(collision.GetNormal());
		// 	//Velocity = Velocity.Bounce(collision.GetNormal());
		// }


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

	public void OnBounceTimerTimeout()
	{
		isCollision = false;
	}


}
