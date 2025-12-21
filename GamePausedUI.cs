using Godot;
using System;

public partial class GamePausedUI : Control
{
	private float dy = 40f;
	private float time = 1.5f;

	private TextureRect title;
	private float startTop;

	public override void _Ready()
	{
		// QUAN TRỌNG: để tween chạy khi pause
		ProcessMode = ProcessModeEnum.Always;

		title = GetNode<TextureRect>("TitleGameOver");

		// Lấy vị trí offset ban đầu
		startTop = title.OffsetTop;

		MoveTitle();
	}

	private void MoveTitle()
	{
		var tween = CreateTween();
		tween.SetLoops();

		tween.TweenProperty(
			title,
			"offset_top",
			startTop + dy,
			time
		).SetTrans(Tween.TransitionType.Sine)
		 .SetEase(Tween.EaseType.InOut);

		tween.TweenProperty(
			title,
			"offset_top",
			startTop,
			time
		).SetTrans(Tween.TransitionType.Sine)
		 .SetEase(Tween.EaseType.InOut);
	}
}
