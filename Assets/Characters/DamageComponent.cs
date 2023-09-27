public partial class DamageComponent : Node2D
{
	[Signal] public delegate void DamagedEventHandler(int points);
	[Signal] public delegate void DestroyedEventHandler();
	[Export] public int damagePoints = 5;

	

	public override void _Ready()
	{
		
	}

	void OnHealthChanged(float oldHealth, float newHealth) {
		if (oldHealth > newHealth)
		{
			EmitSignal(SignalName.Damaged, damagePoints);
		}
	}

	void OnHealthDepleted() { 
		EmitSignal(SignalName.Destroyed);
		GetParent<CharacterBody2D>().SetCollisionLayerValue(3, false);
	}
}
