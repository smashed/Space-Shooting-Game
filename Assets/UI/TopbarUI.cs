using Godot;
using System;

public partial class TopbarUI : MarginContainer
{
	[Export] Label score;

	ProgressBar _progressBarBack;
	ProgressBar _progressBarFront;
	Tween _tweenBack;
	Tween _tweenFront;

	int pointsEarned = 0;

	public override void _Ready()
	{
		_progressBarBack = GetNode<ProgressBar>("HBoxContainer/HealthBarRect/ProgressBarBack");
		_progressBarFront = GetNode<ProgressBar>("HBoxContainer/HealthBarRect/ProgressBarFront");
	}

	public void InitPlayerHealth(float currentHealth, float maxHealth) {
		var health = (currentHealth / maxHealth) * 100;
		_progressBarBack.Value = health;
		_progressBarFront.Value = health;
		GD.Print("current health " + health);
	}

	public void OnPlayerHealthChanged(float newHealth, float maxHealth) 
	{
		var health = (newHealth / maxHealth) * 100;
		if (_tweenBack != null)
			if (_tweenBack.IsValid())
				_tweenBack.Kill();
		if (_tweenBack != null)
			if (_tweenFront.IsValid())
		   		_tweenFront.Kill();
		_tweenBack = GetTree().CreateTween();
		_tweenFront = GetTree().CreateTween();
		_tweenFront.TweenProperty(_progressBarFront, "value", health, 0.2);
		if (newHealth <= 0)
			_tweenBack.TweenProperty(_progressBarBack, "value", health, 0.2);
		else
			_tweenBack.TweenProperty(_progressBarBack, "value", health, 0.4).SetDelay(0.75);
	}

	public void IncreaseScore(int points)
	{
		pointsEarned += points;
		score.Text = (pointsEarned.ToString()).PadZeros(9);
	}
}
