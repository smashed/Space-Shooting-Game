using Godot;
using System;

public partial class Game : Node2D
{
	PackedScene _player;
	PackedScene _enemy;
	PackedScene _topBarUI;
	PackedScene _gameOver;

	TopbarUI _gameUI;
	bool _isGameOver;

	public HealthComponent PlayerHealth {get; private set;}

	public override void _Ready()
	{
		InitGame();
	}

	void InitGame() 
	{
		_player = ResourceLoader.Load<PackedScene>("res://Assets/Characters/Player/Player.tscn");
		_enemy = ResourceLoader.Load<PackedScene>("res://Assets/Characters/Enemies/Fleet1/Ship/KlaedScout.tscn");
		_topBarUI = ResourceLoader.Load<PackedScene>("res://Assets/UI/TopbarUI.tscn");
		_gameOver = ResourceLoader.Load<PackedScene>("res://Scenes/GameOver.tscn");

		SpawnPlayer();
		LoadUI();
		SpawnEnemies();
	}

	void SpawnPlayer() 
	{
		var player = (Node2D)_player.Instantiate();
		GetNode("/root/Game").AddChild(player);
		player.GlobalPosition = new Vector2(360,1080);
		PlayerHealth = player.GetNode<HealthComponent>("HealthComponent");
		PlayerHealth.HealthChanged += OnPlayerDamaged;
	}

	void LoadUI() 
	{
		var topbarUI = (MarginContainer)_topBarUI.Instantiate();
		GetNode("/root/Game/CanvasLayer").AddChild(topbarUI);
		_gameUI = GetNode<TopbarUI>("CanvasLayer/TopbarUI");
		_gameUI.InitPlayerHealth(PlayerHealth.CurrentHealth, PlayerHealth.MaxHealth);
	}

	void OnPlayerDamaged(float oldHealth, float newHealth)
	{
		_gameUI.OnPlayerHealthChanged(newHealth, PlayerHealth.MaxHealth);
	}

	public void OnPlayerDied()
	{
		_isGameOver = true;
		OnGameOver();
	}

	void OnGameOver()
	{
		var gameOver = (Control)_gameOver.Instantiate();
		GetNode("/root/Game/CanvasLayer").AddChild(gameOver);
	}

	void SpawnEnemies()
	{
		for (int x = 0; x < 9; x++) 
		{
			for (int y = 0; y < 3; y++)
			{
				var enemy = (Node2D)_enemy.Instantiate();
				var pos = new Vector2(x * (64 + 20) + 60, 64 * 4 + y * 128);
				GetNode("/root/Game").AddChild(enemy);
				enemy.GlobalPosition = pos;
				var damage = enemy.GetNode<DamageComponent>("DamageComponent");
				damage.Damaged += OnEnemyDamaged;
			}
		}
	}

	void OnEnemyDamaged(int points)
	{
		GetNode<TopbarUI>("CanvasLayer/TopbarUI").IncreaseScore(points);
	}
}
