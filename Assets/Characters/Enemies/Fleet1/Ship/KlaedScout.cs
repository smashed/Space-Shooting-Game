using Godot;
using System;

public partial class KlaedScout : CharacterBody2D
{
	[Export] public float speed = 600.0f;
	[Export] public float collisionDamage = 10.0f;
	
	
	bool _isCollision;
	Timer _bounceTimer;
	Timer _shootTimer;
	float _fireRate = 1.0f;
	
	HealthComponent _health;
	AnimationPlayer _animationPlayer;
	RandomNumberGenerator random;

	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_animationPlayer.AnimationFinished += OnAnimationFinished;
		_bounceTimer = GetNode<Timer>("BounceTimer");
    	_bounceTimer.Timeout += OnBounceTimerTimeout;
		_health = GetNode<HealthComponent>("HealthComponent");
		_shootTimer = GetNode<Timer>("ShootTimer");
		_shootTimer.WaitTime = GetRandomRange();
		_shootTimer.Timeout += OnShootTimerTimeOut;
		_shootTimer.Start();
	}

    public override void _PhysicsProcess(double delta)
	{
		MoveAndSlide();
		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			var collision = GetSlideCollision(i);
			if (((Node)collision.GetCollider()).Name == "Player")
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
		Velocity = Vector2.Zero;
	}

	void OnShipDestroyed()
	{
		_animationPlayer.Play("Explode", -1, 5);
	}

	void OnAnimationFinished(StringName animName)
    {
        if (animName == "Explode")
        	QueueFree();
    }

	void OnShootTimerTimeOut()
	{
		//GD.Print("Shooting" + this.Name);
		_animationPlayer.Play("KlaedWaveProjectile", -1, _fireRate);
		_shootTimer.WaitTime = GetRandomRange();
		_shootTimer.Start();
	}
	

	float GetRandomRange() {
		random = new RandomNumberGenerator();
		random.Randomize();
		return random.RandfRange(5.0f, 15.0f);
	}
}
