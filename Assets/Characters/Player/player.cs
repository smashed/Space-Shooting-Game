using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] public float speed = 600.0f;
	[Export] public float collisionDamage = 15.0f;
	
	float _fireRate = 2.0f;
	AnimatedSprite2D _shipSprite;
	AnimationTree _animationTree;
	AnimationPlayer _animationPlayer;
	ShipWeapons _shipWeapon;
	ShipEngine _shipEngine;
	HealthComponent _health;

	bool isDead;
	bool _isCollision;
	Timer _bounceTimer;

	ParallaxLayer _background;

    public override void _Ready()
    {
		_shipSprite = GetNode<AnimatedSprite2D>("ShipAnimatedSprite");
        _animationTree = GetNode<AnimationTree>("AnimationTree");
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_animationPlayer.AnimationFinished += OnAnimationFinished;
		_shipWeapon = GetNode<ShipWeapons>("Ship Weapons");
		_shipEngine = GetNode<ShipEngine>("Ship Engine");
		_health = GetNode<HealthComponent>("HealthComponent");
		_background = GetNode<ParallaxLayer>("/root/Game/ParallaxBackground/TransparentBG");
		_bounceTimer = GetNode<Timer>("BounceTimer");
    	_bounceTimer.Timeout += OnBounceTimerTimeout;

		_shipSprite.Animation = "Ship";
		_shipSprite.Frame = 0;
    }

    public override void _PhysicsProcess(double delta)
	{
		if (!isDead)
		{
			Move(delta);
			Fire();
			ShipEngine();
			ShipWeapon();
			return;
		}
		// Force animation // Sometimes it gets stuck
		OnHealthDepleted(); 
	}

	void Move(double delta)
	{
		// Get the input direction and handle the movement/deceleration.
		Vector2 direction = Input.GetVector("Left", "Right", "Up", "Down");
		if (!_isCollision)
			Velocity = direction * speed;

		if (Velocity > Vector2.Zero || Velocity < Vector2.Zero) //direction != Vector2.Zero)
		{
			_animationTree.Set("parameters/conditions/is_moving", true);
			_animationTree.Set("parameters/conditions/idle", false);
			if (!IsOnWall())
				_background.MotionOffset -= direction;
		}
		else
		{
			_animationTree.Set("parameters/conditions/idle", true);
			_animationTree.Set("parameters/conditions/is_moving", false);
		}

		MoveAndSlide();
		OnCollisionDetected();
	}

	void Fire()
	{
		// Handle Shoot.
		if (Input.IsActionPressed("Fire"))
			_animationPlayer.Play(_shipWeapon.FireProjectile, -1, _fireRate);
	}

	void ShipWeapon()
	{
		if (Input.IsActionJustPressed("Weapon 1"))
			_shipWeapon.SetWeapon(Weapon.AutoCannon);
		if (Input.IsActionJustPressed("Weapon 2"))
			_shipWeapon.SetWeapon(Weapon.BigSpaceGun);
		if (Input.IsActionJustPressed("Weapon 3"))
			_shipWeapon.SetWeapon(Weapon.Rockets);
		if (Input.IsActionJustPressed("Weapon 4"))
			_shipWeapon.SetWeapon(Weapon.Zapper);
	}

	void ShipEngine()
	{
		if (Input.IsActionJustPressed("Engine 1"))
			_shipEngine.SetEngine(ShipEngineType.Base);
		if (Input.IsActionJustPressed("Engine 2"))
			_shipEngine.SetEngine(ShipEngineType.BigPulse);
		if (Input.IsActionJustPressed("Engine 3"))
			_shipEngine.SetEngine(ShipEngineType.Burst);
		if (Input.IsActionJustPressed("Engine 4"))
			_shipEngine.SetEngine(ShipEngineType.Supercharged);
	}

	void OnCollisionDetected()
	{
		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			var collision = GetSlideCollision(i);
			if (((Node)collision.GetCollider()).IsInGroup("Enemies") )
			{
				_health.Damage(collisionDamage);
				Velocity = Velocity.Bounce(collision.GetNormal()) + collision.GetColliderVelocity();
				Velocity *= 0.6f;
				_isCollision = true;
				_bounceTimer.Start();
			}
		}
	}

	void OnBounceTimerTimeout()
	{
		_isCollision = false;
	}

	void OnHealthChanged(float oldHealth, float newHealth) 
	{
		if (oldHealth > newHealth) 
		{
			int frame = 0;
			if (newHealth / _health.MaxHealth < 0.3)
				frame = 3;
			else if (newHealth / _health.MaxHealth < 0.6)
				frame = 2;
			else if (newHealth / _health.MaxHealth < 0.9)
				frame = 1;
			_shipSprite.Animation = "Ship";
			_shipSprite.Frame = frame;
		}
		GD.Print("My health: " + newHealth);
		if (newHealth <= 0)
			OnHealthDepleted();
	}

	void OnHealthDepleted() 
	{
		isDead = true;
		SetCollisionLayerValue(1, false);
		if (_animationPlayer.CurrentAnimation != "Explode")
			_animationPlayer.Play("Explode", -1, 5);
	}

	void OnAnimationFinished(StringName animName)
    {
		if (animName == "Explode")
		{
			GetNode<Game>("/root/Game").OnPlayerDied();
			QueueFree();
		}
    }
}
