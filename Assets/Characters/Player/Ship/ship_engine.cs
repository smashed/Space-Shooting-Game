using Godot;
using System;

public partial class ship_engine : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetEngine(Engine.Base);
	}

	public void SetEngine(Engine shipEngine)
	{
		switch (shipEngine)
		{
			case Engine.BigPulse:
				HideEngines();
				GetNode<Node2D>("Big Pulse").Show();
				break;

			case Engine.Burst:
				HideEngines();
				GetNode<Node2D>("Burst").Show();
				break;

			case Engine.Supercharged:
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
		Array values = Enum.GetValues(typeof(Engine));
		var random = new Random();
		SetEngine((Engine)values.GetValue(random.Next(values.Length)));
	}
}