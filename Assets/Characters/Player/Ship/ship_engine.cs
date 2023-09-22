using Godot;
using System;

public partial class ship_engine : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetRandomShipEngine();
	}

	public void SetEngine(ShipEngine shipEngine)
	{
		switch (shipEngine)
		{
			case ShipEngine.BigPulse:
				HideEngines();
				GetNode<Node2D>("Big Pulse").Show();
				break;

			case ShipEngine.Burst:
				HideEngines();
				GetNode<Node2D>("Burst").Show();
				break;

			case ShipEngine.Supercharged:
				HideEngines();
				GetNode<Node2D>("Supercharged").Show();
				break;

			default:
				HideEngines();
				GetNode<Node2D>("Base").Show();
				break;
		}
	}

	private void HideEngines() 
	{
		foreach (Node2D eng in GetChildren()) 
			eng.Hide();
	}

	private void SetRandomShipEngine() {
		Array values = Enum.GetValues(typeof(ShipEngine));
		var random = new Random();
		SetEngine((ShipEngine)values.GetValue(random.Next(values.Length)));
	}
}

public enum ShipEngine 
{
	Base,
	BigPulse,
	Burst,
	Supercharged
}