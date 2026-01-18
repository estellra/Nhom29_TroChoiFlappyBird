using Godot;
using System;

public partial class BackgroundManager : Node2D
{
	//Khởi tạo 4 khung hình
	[Export] public Texture2D BgTexture;
	[Export] public NodePath CameraPath; 
	private Node2D camera;
	private Sprite2D[] sprites = new Sprite2D[4];
	private float textureWidth;
	private float scaleFactor;


	//Link cái camera với BackGround và khởi tạo 3 khung hình chồng lên nhau
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
	}

	//Tính lại cái cameara cho nó bằng với tốc độ di chuyển của khung hình
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
	}
}
