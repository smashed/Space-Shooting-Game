using Godot;
using System;
using System.Diagnostics;

public partial class player : CharacterBody2D
{
	public Weapon weapon = Weapon.AutoCannon;

	float speed = 600.0f;
	AnimationTree animationTree;
	AnimationPlayer animationPlayer;
	AnimatedSprite2D shipWeapon;

	PackedScene projectile;
	bool autoCannonShot = false;
	int rockets = 6;
	string autoCannonProjectile = "res://Assets/Characters/Player/Projectiles/auto_cannon_projectile.tscn";
	string bigSpaceGunProjectile = "res://Assets/Characters/Player/Projectiles/big_space_gun_projectile.tscn";
	string rocketProjectile = "res://Assets/Characters/Player/Projectiles/rocket_projectile.tscn";
	string zapperProjectile = "res://Assets/Characters/Player/Projectiles/zapper_projectile.tscn";
	string loadProjectile;
	string fireProjectile;

    public override void _Ready()
    {
        animationTree = GetNode("AnimationTree") as AnimationTree;
		animationPlayer = GetNode("AnimationPlayer") as AnimationPlayer;
		shipWeapon = GetNode("Ship Weapons") as AnimatedSprite2D;
		SetWeapon(weapon);
		projectile = ResourceLoader.Load<PackedScene>(loadProjectile);
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Handle Shoot.
		if (Input.IsActionJustPressed("Fire"))
			animationPlayer.Play(fireProjectile);
			

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
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

	public void FireAutoCannonProjectile()
	{
		var bullet = (Node2D)projectile.Instantiate();
		var shotPos = autoCannonShot ? 27 : -27;
		autoCannonShot = autoCannonShot ? false : true;
		GetNode("/root/Game").AddChild(bullet);
		bullet.GlobalPosition = GlobalPosition + new Vector2(shotPos, 0);
	}

	public void FireBigSpaceGunProjectile() 
	{
		var bullet = (Node2D)projectile.Instantiate();
		GetNode("/root/Game").AddChild(bullet);
		bullet.GlobalPosition = GlobalPosition;
	}

	public void FireRocketProjectile() 
	{
		var bullet = (Node2D)projectile.Instantiate();
		var shotPosX = -19.5f;
		var shotPosY = 12f;

		switch(rockets) 
		{
			case 6:
				shotPosX = -19.5f;
				shotPosY = 0.0f;
				break; 
			case 5:
				shotPosX = 19.5f;
				shotPosY = 0.0f;
				break;
			case 4:
				shotPosX = -31.5f;
				shotPosY = 12.0f;
				break; 
			case 3:
				shotPosX = 31.5f;
				shotPosY = 12.0f;
				break;
			case 2:
				shotPosX = -43.5f;
				shotPosY = 24.0f;
				break; 
			case 1:
				shotPosX = 43.5f;
				shotPosY = 24.0f;
				rockets = 6; // refill
				break;
			default:
				break;
		}
		GetNode("/root/Game").AddChild(bullet);
		bullet.GlobalPosition = GlobalPosition + new Vector2(shotPosX, shotPosY);
	}

	public void FireZapperProjectile() 
	{
		var bullet1 = (Node2D)projectile.Instantiate();
		var bullet2 = (Node2D)projectile.Instantiate();
		var shotPosX1 = -22.5f;
		var shotPosX2 = 22.5f;
		var shotPosY = -46f;
		GetNode("/root/Game").AddChild(bullet1);
		GetNode("/root/Game").AddChild(bullet2);
		bullet1.GlobalPosition = GlobalPosition + new Vector2(shotPosX1, shotPosY);
		bullet2.GlobalPosition = GlobalPosition + new Vector2(shotPosX2, shotPosY);
	}

	void SetWeapon(Weapon weapon) 
	{
		switch (weapon) 
		{
			case Weapon.BigSpaceGun:
				shipWeapon.Animation = "Big Space Gun";
				loadProjectile = bigSpaceGunProjectile;
				fireProjectile = "BigSpaceGun";
				return;
			case Weapon.Rockets:
				shipWeapon.Animation = "Rockets";
				loadProjectile = rocketProjectile;
				fireProjectile = "Rockets";
				return;
			case Weapon.Zapper:
				shipWeapon.Animation = "Zapper";
				loadProjectile = zapperProjectile;
				fireProjectile = "Zapper";
				return;
			default:
				shipWeapon.Animation = "Auto Cannon";
				loadProjectile = autoCannonProjectile;
				fireProjectile = "AutoCannonProjectile";
				return;
		}
	}
}
