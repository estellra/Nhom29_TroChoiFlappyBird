using Godot;
using System;

public partial class TenNhom : RichTextLabel
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Font font=GD.Load<Font>("res://asset/prstartk.ttf");
		AddThemeFontSizeOverride("normal_font_size", 20);
		AddThemeFontOverride("normal_font",font);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
