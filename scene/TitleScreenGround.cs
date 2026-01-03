using Godot;
using System;

public partial class TitleScreenGround : TextureRect
{
	public float speed=-200;
	public float width;
	private AnimatedSprite2D sprite;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
			width=Texture.GetSize().X;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		float x=speed*(float)delta;
		Position=new Vector2(Position.X+x,Position.Y);
		if(Position.X<=-width/3)
		{
			Position=new Vector2(Position.X+width/3,Position.Y);
		}
	}
}
