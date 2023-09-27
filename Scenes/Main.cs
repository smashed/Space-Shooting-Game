public partial class Main : Node2D
{
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Start"))
			OnPlayButtonPressed();
	}

	private void OnPlayButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Game.tscn");
	}
}
