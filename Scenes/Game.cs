public partial class Game : Node2D
{
	PackedScene _player;
	PackedScene _enemy;
	PackedScene _topBarUI;
	PackedScene _gameOver;

	TopbarUI _gameUI;
	bool _isGameOver;
	CanvasLayer _canvasLayer;
	Node2D _enemies;
	Node2D _gameScene;

	public HealthComponent PlayerHealth {get; private set;}

	public override void _Ready()
	{
		InitGame();
	}

	public override void _Process(double delta)
	{
		if (GetTree().GetNodesInGroup("Enemies").Count == 0 && !_isGameOver)
			SpawnEnemies();
	}

	void InitGame() 
	{
		_player = ResourceLoader.Load<PackedScene>("res://Assets/Characters/Player/Player.tscn");
		_enemy = ResourceLoader.Load<PackedScene>("res://Assets/Characters/Enemies/Fleet1/Ship/KlaedScout.tscn");
		_topBarUI = ResourceLoader.Load<PackedScene>("res://Assets/UI/TopbarUI.tscn");
		_gameOver = ResourceLoader.Load<PackedScene>("res://Scenes/GameOver.tscn");
		_gameScene = GetNode<Node2D>(GetPath());

		SpawnPlayer();
		LoadUI();
		SpawnEnemies();
		EnemySwingMotion();
	}

	void SpawnPlayer() 
	{
		var player = (Node2D)_player.Instantiate();
		_gameScene.AddChild(player);
		player.GlobalPosition = new Vector2(360,1080);
		PlayerHealth = player.GetNode<HealthComponent>("HealthComponent");
		PlayerHealth.HealthChanged += OnPlayerDamaged;
	}

	void LoadUI() 
	{
		_canvasLayer = new CanvasLayer();
		_gameScene.AddChild(_canvasLayer);
		var topbarUI = (MarginContainer)_topBarUI.Instantiate();
		_canvasLayer.AddChild(topbarUI);
		_gameUI = GetNode<TopbarUI>(topbarUI.GetPath());
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
		_canvasLayer.AddChild(gameOver);
	}

	void SpawnEnemies()
	{
		if (_enemies == null) {
			_enemies = new CharacterBody2D();
			_gameScene.AddChild(_enemies);
		}
		for (int x = 0; x < 8; x++) 
		{
			for (int y = 0; y < 3; y++)
			{
				var enemy = (CharacterBody2D)_enemy.Instantiate();
				var pos = new Vector2(x * (64 + 20) + 64, (64 * 3) + y * 128);
				_enemies.AddChild(enemy);
				enemy.Position = pos;
				enemy.Visible = false;
				enemy.SetCollisionLayerValue(3, false);
				var damage = enemy.GetNode<DamageComponent>("DamageComponent");
				damage.Damaged += OnEnemyDamaged;
			}
		}
	}

	void EnemySwingMotion()
	{
		var tween = CreateTween().SetLoops().SetParallel(false).SetTrans(Tween.TransitionType.Sine);
		tween.TweenProperty(_enemies, "position:x", _enemies.Position.X + 6, 1.0);
		tween.TweenProperty(_enemies, "position:x", _enemies.Position.X - 6, 1.0);
		var tween2 = CreateTween().SetLoops().SetParallel(false).SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.InOut);
		tween2.TweenProperty(_enemies, "position:y", _enemies.Position.Y + 4, 1.0);
		tween2.TweenProperty(_enemies, "position:y", _enemies.Position.Y - 4, 1.0);
	}

	void OnEnemyDamaged(int points)
	{
		_gameUI.IncreaseScore(points);
	}
}
