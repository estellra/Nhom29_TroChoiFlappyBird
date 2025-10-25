using Godot;
using System;

public partial class Background : Node2D
{
	[Export] public NodePath PlayerPath;
	[Export] public float ScrollSpeed = 0.2f;
	[Export] public Texture2D BackgroundTexture;
	
	private Node2D player;
	private Sprite2D sprite1;
	private Sprite2D sprite2;
	private float bgWidth;
	private float lastPlayerX = 0;
	
	public override void _Ready()
	{
		// Lấy player
		if (PlayerPath != null)
			player = GetNode<Node2D>(PlayerPath);
		else
			player = GetParent().GetNodeOrNull<Node2D>("Player");
		
		if (player == null)
		{
			GD.PrintErr("Background: Không tìm thấy Player!");
			return;
		}
		
		float viewportHeight = GetViewportRect().Size.Y;
		
		// Tạo 2 sprite đơn giản
		sprite1 = new Sprite2D();
		sprite2 = new Sprite2D();
		
		if (BackgroundTexture != null)
		{
			sprite1.Texture = BackgroundTexture;
			sprite2.Texture = BackgroundTexture;
			
			// Scale để fit height
			float scale = viewportHeight / BackgroundTexture.GetHeight();
			sprite1.Scale = new Vector2(scale, scale);
			sprite2.Scale = new Vector2(scale, scale);
			
			bgWidth = BackgroundTexture.GetWidth() * scale;
		}
		else
		{
			// Tạo background màu xanh nếu không có texture
			bgWidth = 1920;
			sprite1.Texture = CreateSimpleTexture(viewportHeight);
			sprite2.Texture = CreateSimpleTexture(viewportHeight);
		}
		
		AddChild(sprite1);
		AddChild(sprite2);
		
		// Set top level để không bị ảnh hưởng bởi camera transform
		sprite1.TopLevel = true;
		sprite2.TopLevel = true;
		sprite1.ZIndex = -20;
		sprite2.ZIndex = -20;
		
		// Vị trí ban đầu
		sprite1.GlobalPosition = new Vector2(0, viewportHeight / 2);
		sprite2.GlobalPosition = new Vector2(bgWidth, viewportHeight / 2);
		
		lastPlayerX = player.GlobalPosition.X;
		
		GD.Print("=== BACKGROUND READY ===");
		GD.Print($"BG Width: {bgWidth}");
	}
	
	private Texture2D CreateSimpleTexture(float height)
	{
		int width = 1920;
		int h = (int)height;
		
		Image img = Image.Create(width, h, false, Image.Format.Rgba8);
		Color skyColor = new Color(0.53f, 0.81f, 0.92f);
		
		for (int y = 0; y < h; y++)
		{
			float t = (float)y / h;
			Color color = skyColor.Lerp(skyColor.Darkened(0.3f), t);
			
			for (int x = 0; x < width; x++)
			{
				img.SetPixel(x, y, color);
			}
		}
		
		return ImageTexture.CreateFromImage(img);
	}
	
	public override void _Process(double delta)
	{
		if (player == null || sprite1 == null) return;
		
		// Lấy vị trí camera
		Transform2D canvasTransform = GetViewport().CanvasTransform;
		float cameraX = -canvasTransform.Origin.X;
		
		float viewportWidth = GetViewportRect().Size.X;
		float viewportHeight = GetViewportRect().Size.Y;
		
		// Tính parallax offset dựa trên camera
		float parallaxOffset = cameraX * ScrollSpeed;
		
		// Đặt vị trí 2 sprite để luôn che kín màn hình
		float pos1X = cameraX - parallaxOffset;
		float pos2X = cameraX - parallaxOffset + bgWidth;
		
		// Wrap around để loop vô hạn
		while (pos1X > cameraX + viewportWidth)
			pos1X -= bgWidth * 2;
		while (pos1X < cameraX - bgWidth)
			pos1X += bgWidth * 2;
			
		while (pos2X > cameraX + viewportWidth)
			pos2X -= bgWidth * 2;
		while (pos2X < cameraX - bgWidth)
			pos2X += bgWidth * 2;
		
		sprite1.GlobalPosition = new Vector2(pos1X, viewportHeight / 2);
		sprite2.GlobalPosition = new Vector2(pos2X, viewportHeight / 2);
	}
}
