public partial class EnemyShipWeapons : AnimatedSprite2D
{
	PackedScene _klaedWaveProjectile;

	public override void _Ready()
	{
		_klaedWaveProjectile = ResourceLoader.Load<PackedScene>("res://Assets/Characters/Enemies/Fleet1/Projectiles/KlaedWaveProjectile.tscn");
	}

	public void KlaedWaveProjectile()
	{
		var bullet = (Node2D)_klaedWaveProjectile.Instantiate();
		GetNode("/root/Game").AddChild(bullet);
		bullet.GlobalPosition = GlobalPosition + new Vector2(0, 25);
	}
}
