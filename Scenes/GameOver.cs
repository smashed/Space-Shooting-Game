public partial class GameOver : Control
{
	public override void _Ready()
	{
		var tween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.InOut);
		tween.TweenProperty(this, "position", Vector2.Zero, 0.75f);
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Start"))
			OnPlayButtonPressed();
		if (Input.IsActionJustPressed("Menu"))
			OnMenuButtonPressed();
	}

	void OnPlayButtonPressed()
	{
		GetTree().ReloadCurrentScene();
	}

	void OnMenuButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Main.tscn");
	}
}
