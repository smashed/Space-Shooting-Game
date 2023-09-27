public partial class BG : ParallaxBackground
{
	float _scrollSpeed = 100.0f;

	public override void _Process(double delta)
    {
        ScrollBaseOffset += new Vector2(0, _scrollSpeed * (float)delta);
	}
}
