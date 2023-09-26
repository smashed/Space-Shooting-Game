using Godot;

public partial class EnemyMovement : Node2D
{
	float _speed = 0.0f;
	Vector2 _startPos = Vector2.Zero;
	Vector2 _screenSize;
	Timer _moveTimer;
	Timer _curveTimer;
	CharacterBody2D _parent;
	RandomNumberGenerator _random;
	bool _startProcess;
	int _curvedMovement;
	bool _curvedRight;
	float xPos = 6.0f;
	
	public override void _Ready()
	{
		_screenSize = GetViewportRect().Size;
		_parent = GetParent<CharacterBody2D>();
		
		_moveTimer = GetNode<Timer>("MoveTimer");
		_moveTimer.WaitTime = GetRandomRange(5.0f, 20.0f);
		_moveTimer.Timeout += OnMoveTimerTimeout;
		_curveTimer = GetNode<Timer>("CurveTimer");
		_curveTimer.WaitTime = GetRandomRange(0.1f, 1.0f);
		_curveTimer.Timeout += OnCurveTimerTimeout;
		_curvedMovement = _random.RandiRange(1, 2);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_speed > 0)
			_parent.Position = _parent.Position + new Vector2(xPos * (float)delta, _speed * (float)delta);

		if (_parent.Position.Y > _screenSize.Y + 64)
			Start(_startPos);
		
		if (!_startProcess)
		{
			_startProcess = true;
			Start(_parent.Position);
		}
	}

	async void Start(Vector2 pos) 
	{
		_speed = 0.0f;
		_parent.Position = new Vector2(pos.X, -pos.Y);
		_startPos = pos;
		
		await ToSignal(GetTree().CreateTimer(GetRandomRange(0.25f, 0.55f)), SceneTreeTimer.SignalName.Timeout);
		MoveToPosition();
		_moveTimer.Start();
	}

	async void MoveToPosition()
	{
		var tween = CreateTween().SetTrans(Tween.TransitionType.Elastic);
		tween.TweenProperty(_parent, "position:y", _startPos.Y, 1.8);
		_parent.Visible = true;
		_parent.SetCollisionLayerValue(3, true);
		await ToSignal(tween, Tween.SignalName.Finished);
	}

	void OnMoveTimerTimeout()
	{
		_speed = GetRandomRange(250, 350);
	}

	void OnCurveTimerTimeout()
	{
		if (_curvedMovement == 1)
		{
			_curvedMovement = 3;
			xPos = 120.0f;
			_curveTimer.WaitTime = 1.0f;
		}
		else if (_curvedMovement == 2)
		{
			_curvedMovement = 3;
			xPos = -120.0f;
			_curveTimer.WaitTime = 1.0f;
		}
		else
		{
			if (_curvedRight)
			{
				_curvedRight = false;
				_curvedMovement = 2;
			}
			else 
			{
				_curvedRight = true;
				_curvedMovement = 1;
			}
			xPos = 0.0f;
			_curveTimer.WaitTime = 1.0f;
		}
	}

	float GetRandomRange(float x, float y) {
		_random = new RandomNumberGenerator();
		_random.Randomize();
		return _random.RandfRange(x, y);
	}
}
