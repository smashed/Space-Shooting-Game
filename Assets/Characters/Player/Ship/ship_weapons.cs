using Godot;
using System;

public partial class ship_weapons : AnimatedSprite2D
{
	public string fireProjectile;

	Weapon weapon = Weapon.AutoCannon;
	PackedScene projectile;
	PackedScene autoCannonProjectile;
	PackedScene bigSpaceGunProjectile;
	PackedScene rocketProjectile;
	PackedScene zapperProjectile;

	bool autoCannonShot = false;
	int rockets = 6;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetWeapon(weapon);
		autoCannonProjectile = ResourceLoader.Load<PackedScene>("res://Assets/Characters/Player/Projectiles/auto_cannon_projectile.tscn");
		bigSpaceGunProjectile = ResourceLoader.Load<PackedScene>("res://Assets/Characters/Player/Projectiles/big_space_gun_projectile.tscn");
		rocketProjectile = ResourceLoader.Load<PackedScene>("res://Assets/Characters/Player/Projectiles/rocket_projectile.tscn");
		zapperProjectile = ResourceLoader.Load<PackedScene>("res://Assets/Characters/Player/Projectiles/zapper_projectile.tscn");
	}

	public void FireAutoCannonProjectile()
	{
		var bullet = (Node2D)autoCannonProjectile.Instantiate();
		var shotPos = autoCannonShot ? 27 : -27;
		autoCannonShot = autoCannonShot ? false : true;
		GetNode("/root/Game").AddChild(bullet);
		bullet.GlobalPosition = GlobalPosition + new Vector2(shotPos, 0);
	}

	public void FireBigSpaceGunProjectile() 
	{
		var bullet = (Node2D)bigSpaceGunProjectile.Instantiate();
		GetNode("/root/Game").AddChild(bullet);
		bullet.GlobalPosition = GlobalPosition;
	}

	public void FireRocketProjectile() 
	{
		var bullet = (Node2D)rocketProjectile.Instantiate();
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
				break;
			default:
				break;
		}
		GetNode("/root/Game").AddChild(bullet);
		bullet.GlobalPosition = GlobalPosition + new Vector2(shotPosX, shotPosY);
		rockets--;
		if (rockets <= 0 )
			rockets = 6;
	}

	public void FireZapperProjectile() 
	{
		var bullet1 = (Node2D)zapperProjectile.Instantiate();
		var bullet2 = (Node2D)zapperProjectile.Instantiate();
		var shotPosX1 = -22.5f;
		var shotPosX2 = 22.5f;
		var shotPosY = -46f;
		GetNode("/root/Game").AddChild(bullet1);
		GetNode("/root/Game").AddChild(bullet2);
		bullet1.GlobalPosition = GlobalPosition + new Vector2(shotPosX1, shotPosY);
		bullet2.GlobalPosition = GlobalPosition + new Vector2(shotPosX2, shotPosY);
	}

	public void SetWeapon(Weapon weapon) 
	{
		switch (weapon) 
		{
			case Weapon.BigSpaceGun:
				Animation = "Big Space Gun";
				fireProjectile = "BigSpaceGunProjectile";
				return;
			case Weapon.Rockets:
				Animation = "Rockets";
				fireProjectile = "RocketProjectile";
				return;
			case Weapon.Zapper:
				Animation = "Zapper";
				fireProjectile = "ZapperProjectile";
				return;
			default:
				Animation = "Auto Cannon";
				fireProjectile = "AutoCannonProjectile";
				return;
		}
	}
}
