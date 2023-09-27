public partial class KlaedScout : CharacterBody2D
{
	[Export] public float speed = 600.0f;
	[Export] public float collisionDamage = 10.0f;
	
	
	bool _isCollision;
	Timer _bounceTimer;
	Timer _shootTimer;
	float _fireRate = 1.0f;
	
	HealthComponent _health;
	AnimationPlayer _animationFire;
	AnimationPlayer _animationExplosion;
	RandomNumberGenerator random;

	public override void _Ready()
	{
		_animationFire = GetNode<AnimationPlayer>("AnimationFire");
		_animationExplosion = GetNode<AnimationPlayer>("AnimationExplosion");
		_animationExplosion.AnimationFinished += OnAnimationFinished;
		_bounceTimer = GetNode<Timer>("BounceTimer");
    	_bounceTimer.Timeout += OnBounceTimerTimeout;
		_health = GetNode<HealthComponent>("HealthComponent");
		_shootTimer = GetNode<Timer>("ShootTimer");
		_shootTimer.WaitTime = GetRandomRange(5.0f, 15.0f);
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
		_animationExplosion.Play("Explode", -1, 5);
	}

	void OnAnimationFinished(StringName animName)
    {
        if (animName == "Explode")
        	QueueFree();
    }

	void OnShootTimerTimeOut()
	{
		//GD.Print("Shooting" + this.Name);
		_animationFire.Play("KlaedWaveProjectile", -1, _fireRate);
		_shootTimer.WaitTime = GetRandomRange(5.0f, 15.0f);
		_shootTimer.Start();
	}
	

	float GetRandomRange(float x, float y) {
		random = new RandomNumberGenerator();
		random.Randomize();
		return random.RandfRange(x, y);
	}
}
