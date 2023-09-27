public partial class ShipEngine : Node2D
{
	public override void _Ready()
	{
		SetEngine(ShipEngineType.Base);
	}

	public void SetEngine(ShipEngineType shipEngine)
	{
		switch (shipEngine)
		{
			case ShipEngineType.BigPulse:
				HideEngines();
				GetNode<Node2D>("Big Pulse").Show();
				break;

			case ShipEngineType.Burst:
				HideEngines();
				GetNode<Node2D>("Burst").Show();
				break;

			case ShipEngineType.Supercharged:
				HideEngines();
				GetNode<Node2D>("Supercharged").Show();
				break;

			default:
				HideEngines();
				GetNode<Node2D>("Base").Show();
				break;
		}
	}

	void HideEngines() 
	{
		foreach (Node2D eng in GetChildren()) 
			eng.Hide();
	}

	void SetRandomShipEngine() {
		Array values = Enum.GetValues(typeof(ShipEngineType));
		var random = new Random();
		SetEngine((ShipEngineType)values.GetValue(random.Next(values.Length)));
	}
}