public partial class HealthComponent : Node2D
{
	[Signal] public delegate void HealthChangedEventHandler(float oldHealth, float newHealth);
	[Signal] public delegate void HealthDepletedEventHandler();

	float _currentHealth = 10.0f;
	float _maxHealth = 10.0f;
	bool _hasDied;

	[Export] public float MaxHealth { get => _maxHealth; private set => _maxHealth = value; }

	public float CurrentHealth => _currentHealth;

	public bool HasDied => _hasDied;

    public override void _Ready()
    {
       InitializeHealth();
    }

	public void Damage(float damage)
	{
		float previousHealth = _currentHealth;
		_currentHealth -= damage;
		EmitSignal(SignalName.HealthChanged, previousHealth, _currentHealth);
		//GD.Print("Health " + _currentHealth);

		if (_currentHealth <= 0 && !_hasDied)
		{
			_hasDied = true;
			EmitSignal(SignalName.HealthDepleted);
			//GD.Print("Has Died = " + _hasDied);
		}
	}

	public void InitializeHealth()
	{
		_currentHealth = MaxHealth;
		//EmitSignal(SignalName.HealthChanged, _currentHealth, _currentHealth);
	}

    internal void Connect(string v, Action<float, float> onPlayerHealthChanged, Godot.Collections.Array array)
    {
        throw new NotImplementedException();
    }

}
