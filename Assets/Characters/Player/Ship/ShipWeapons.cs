public partial class ShipWeapons : AnimatedSprite2D
{
	Weapon weapon = Weapon.AutoCannon;
	PackedScene _projectile;
	PackedScene _autoCannonProjectile;
	PackedScene _bigSpaceGunProjectile;
	PackedScene _rocketProjectile;
	PackedScene _zapperProjectile;

	bool autoCannonShot = false;
	int rockets = 6;
	
	public string FireProjectile { get; private set; }
	
	public override void _Ready()
	{
		SetWeapon(weapon);
		_autoCannonProjectile = ResourceLoader.Load<PackedScene>("res://Assets/Characters/Player/Projectiles/AutoCannonProjectile.tscn");
		_bigSpaceGunProjectile = ResourceLoader.Load<PackedScene>("res://Assets/Characters/Player/Projectiles/BigSpaceGunProjectile.tscn");
		_rocketProjectile = ResourceLoader.Load<PackedScene>("res://Assets/Characters/Player/Projectiles/RocketProjectile.tscn");
		_zapperProjectile = ResourceLoader.Load<PackedScene>("res://Assets/Characters/Player/Projectiles/ZapperProjectile.tscn");
	}

	public void FireAutoCannonProjectile()
	{
		var bullet = (Node2D)_autoCannonProjectile.Instantiate();
		var shotPos = autoCannonShot ? 27 : -27;
		autoCannonShot = autoCannonShot ? false : true;
		GetNode("/root/Game").AddChild(bullet);
		bullet.GlobalPosition = GlobalPosition + new Vector2(shotPos, 0);
	}

	public void FireBigSpaceGunProjectile() 
	{
		var bullet = (Node2D)_bigSpaceGunProjectile.Instantiate();
		GetNode("/root/Game").AddChild(bullet);
		bullet.GlobalPosition = GlobalPosition;
	}

	public void FireRocketProjectile() 
	{
		var bullet = (Node2D)_rocketProjectile.Instantiate();
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
		var bullet1 = (Node2D)_zapperProjectile.Instantiate();
		var bullet2 = (Node2D)_zapperProjectile.Instantiate();
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
				FireProjectile = "BigSpaceGunProjectile";
				return;
			case Weapon.Rockets:
				Animation = "Rockets";
				FireProjectile = "RocketProjectile";
				return;
			case Weapon.Zapper:
				Animation = "Zapper";
				FireProjectile = "ZapperProjectile";
				return;
			default:
				Animation = "Auto Cannon";
				FireProjectile = "AutoCannonProjectile";
				return;
		}
	}
}
