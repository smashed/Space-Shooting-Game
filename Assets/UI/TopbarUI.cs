using Godot;
using System;

public partial class TopbarUI : MarginContainer
{
	[Export] Label score;

	int pointsEarned = 0;

	public void IncreaseScore(int points)
	{
		pointsEarned += points;
		score.Text = (pointsEarned.ToString()).PadZeros(9);
	}
}
