using Godot;
using System;

public partial class HealthBarRect : ColorRect
{
	HealthComponent _playerHealth;
	ProgressBar _progressBarBack;
	ProgressBar _progressBarFront;
	Tween _tweenBack;
	Tween _tweenFront;

	public override void _Ready()
	{
		_progressBarBack = GetNode<ProgressBar>("ProgressBarBack");
		_progressBarFront = GetNode<ProgressBar>("ProgressBarFront");
		_playerHealth = GetNode<HealthComponent>("/root/Game/Player/HealthComponent");
    	_playerHealth.HealthChanged += OnPlayerHealthChanged;

		InitPlayerHealth();
	}

	public void InitPlayerHealth() {
		var health = (_playerHealth.CurrentHealth / _playerHealth.MaxHealth) * 100;
		_progressBarBack.Value = health;
		_progressBarFront.Value = health;
		GD.Print("current health " + health);
	}

	public void OnPlayerHealthChanged(float oldHealth, float newHealth) 
	{
		var health = (newHealth / _playerHealth.MaxHealth) * 100;
		if (_tweenBack != null)
			if (_tweenBack.IsValid())
				_tweenBack.Kill();
		if (_tweenBack != null)
			if (_tweenFront.IsValid())
		   		_tweenFront.Kill();
		_tweenBack = GetTree().CreateTween();
		_tweenFront = GetTree().CreateTween();
		_tweenBack.TweenProperty(_progressBarBack, "value", health, 0.4).SetDelay(0.75);
		_tweenFront.TweenProperty(_progressBarFront, "value", health, 0.2);
	}
}
