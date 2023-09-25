using Godot;
using System;

public partial class KlaedScout : CharacterBody2D
{
	[Export] public float speed = 600.0f;
	[Export] public float collisionDamage = 10.0f;
	bool _isCollision;
	Timer _bounceTimer;
	AnimationPlayer _animationPlayer;
	HealthComponent _health;

	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_animationPlayer.AnimationFinished += OnAnimationFinished;
		_bounceTimer = GetNode<Timer>("BounceTimer");
    	_bounceTimer.Timeout += OnBounceTimerTimeout;
		_health = GetNode<HealthComponent>("HealthComponent");
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

	void OnHealthDepleted() {
		SetCollisionLayerValue(1, false);
		_animationPlayer.Play("Explode", -1, 5);
	}

	void OnAnimationFinished(StringName animName)
    {
        QueueFree();
    }
}
