using Godot;
using System;

public partial class BackgroundManager : Node2D
{
	[Export] public Texture2D BgTexture;
	[Export] public NodePath CameraPath; 
	
	private Node2D camera;
	private Sprite2D[] sprites = new Sprite2D[4];
	private float textureWidth;
	private float scaleFactor;
	private string bg1;
	private string bg2;
	private enum timeofday{
		morning,night
	}
	private timeofday currenttime=timeofday.morning;

	public override void _Ready()
	{
		if (BgTexture == null) return;
		if (CameraPath != null)
			camera = GetNode<Node2D>(CameraPath);
		else
			camera = GetParent().GetNodeOrNull<Node2D>("Camera");

		if (camera == null) return;
		float viewportHeight = GetViewportRect().Size.Y;
		scaleFactor = viewportHeight / BgTexture.GetHeight();
		textureWidth = BgTexture.GetWidth() * scaleFactor;

		for (int i = 0; i < 4; i++)
		{
			sprites[i] = new Sprite2D();
			sprites[i].Texture = BgTexture;
			sprites[i].Scale = new Vector2(scaleFactor, scaleFactor);
			sprites[i].Centered = false; 
			sprites[i].ZIndex = -100;    
			AddChild(sprites[i]);
		}
		bg1="res://asset/bg.png";
		bg2="res://asset/nightbackground.png";
	}

	public override void _Process(double delta)
	{
		if (camera == null) return;
		float camX = camera.GlobalPosition.X;
		float camY = camera.GlobalPosition.Y;
		int currentSegment = (int)Math.Floor(camX / textureWidth);
		for (int i = 0; i < 4; i++)
		{
			sprites[i].Position = new Vector2((currentSegment + i - 1) * textureWidth, camY);
		}
		if(GlobalData.time>=7)
		{
			if(currenttime==timeofday.morning)
				{
					SetBackground(GD.Load<Texture2D>(bg2));
					currenttime=timeofday.night;
				}
			else
				{
					SetBackground(GD.Load<Texture2D>(bg1));
					currenttime=timeofday.morning;
				}
				GlobalData.time-=7;
		}
			
	}
	public void SetBackground(Texture2D newTexture)
	{
		if (newTexture == null) return;
		BgTexture = newTexture;
		float viewportHeight = GetViewportRect().Size.Y;
		scaleFactor = viewportHeight / BgTexture.GetHeight();
		textureWidth = BgTexture.GetWidth() * scaleFactor;
		for (int i = 0; i < sprites.Length; i++)
		{
			sprites[i].Texture = BgTexture;
			sprites[i].Scale = new Vector2(scaleFactor, scaleFactor);
		}
	}
}
