using Godot;

public partial class ProjectileComponent : Area2D
{
	[Export] float projectileSpeed = 2000.0f;
	[Export] float damageAmount = 10.0f;
	[Export] bool isPiercing = false;
	[Export] bool isPlayer = false;
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
		if (isPlayer)
			_parentProjectile.Position -= new Vector2(0, projectileSpeed * (float)delta);
		else
			_parentProjectile.Position += new Vector2(0, projectileSpeed * (float)delta);
	}

	void OnBodyEntered(PhysicsBody2D body)
	{
		if (body.IsInGroup("Enemies") && isPlayer)
		{
			HitTarget(body);
			if (!isPiercing)
				_parentProjectile.QueueFree();
		}
		if (body.Name == "Player" && !isPlayer)
		{
			HitTarget(body);
			_parentProjectile.QueueFree();
		}
	}

	void HitTarget(PhysicsBody2D body) {
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
