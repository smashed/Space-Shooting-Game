using Godot;

public partial class ProjectileComponent : Area2D
{
	[Export] float projectileSpeed = 2000.0f;
	[Export] float damageAmount = 10.0f;
	[Export] bool isPiercing = false;
	Timer _timer;
	Node2D _parentProjectile;

	public override void _Ready()
	{
		_parentProjectile = GetParent<Node2D>();
		_timer = GetNode<Timer>("Timer");
		_timer.Timeout += OnTimerTimeout;
		//BodyEntered += OnBodyEntered;
	}

	public override void _Process(double delta)
	{
		_parentProjectile.Position -= new Vector2(0, projectileSpeed * (float)delta);
	}

	void OnBodyEntered(PhysicsBody2D body)
	{
		if (body.IsInGroup("Enemies"))
		{
			HitEnemy(body);
			if (!isPiercing)
				_parentProjectile.QueueFree();
		}
	}

	void HitEnemy(PhysicsBody2D body) {
		if (IsInstanceValid(body.GetNode<HealthComponent>("HealthComponent"))) {
			var healthComponent = body.GetNode<HealthComponent>("HealthComponent");
			healthComponent.Damage(damageAmount);
		}
	}

	void OnTimerTimeout()
	{
		_parentProjectile.QueueFree();
	}
}
