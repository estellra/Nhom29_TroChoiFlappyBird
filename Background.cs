using Godot;
using System;
using System.Collections.Generic;

public partial class Background : Node2D
{
	// Export để có thể chỉnh trong Inspector
	[Export] public NodePath PlayerPath;
	[Export] public float ScrollSpeed = 0.3f; // Tốc độ background so với player (0-1)
	[Export] public Texture2D BackgroundTexture;
	[Export] public int LayerCount = 3; // Số lớp background để tạo hiệu ứng depth
	
	private Node2D player;
	private List<BackgroundLayer> layers = new List<BackgroundLayer>();
	private float lastPlayerX = 0;
	
	// Class con để quản lý từng lớp background
	private class BackgroundLayer
	{
		public Sprite2D Sprite1;
		public Sprite2D Sprite2;
		public float Speed;
		public float Width;
		
		public BackgroundLayer(Sprite2D s1, Sprite2D s2, float speed, float width)
		{
			Sprite1 = s1;
			Sprite2 = s2;
			Speed = speed;
			Width = width;
		}
	}
	
	public override void _Ready()
	{
		// Lấy player reference
		if (PlayerPath != null)
			player = GetNode<Node2D>(PlayerPath);
		else
			player = GetParent().GetNodeOrNull<Node2D>("Player");
		
		if (player == null)
		{
			GD.PrintErr("Background: Không tìm thấy Player!");
			return;
		}
		
		// Nếu không có texture, tạo background mặc định
		if (BackgroundTexture == null)
		{
			CreateDefaultBackground();
		}
		else
		{
			SetupBackgroundLayers();
		}
		
		lastPlayerX = player.Position.X;
		
		GD.Print("=== BACKGROUND READY ===");
		GD.Print($"Player: {player.Name}");
		GD.Print($"Layers: {LayerCount}");
	}
	
	private void CreateDefaultBackground()
	{
		// Tạo background gradient đơn giản nếu không có texture
		float viewportWidth = GetViewportRect().Size.X;
		float viewportHeight = GetViewportRect().Size.Y;
		
		// Tạo 3 lớp với màu khác nhau
		Color[] colors = new Color[] 
		{
			new Color(0.53f, 0.81f, 0.92f), // Sky blue - xa nhất
			new Color(0.68f, 0.85f, 0.90f), // Light blue - giữa
			new Color(0.80f, 0.88f, 0.95f)  // Very light blue - gần nhất
		};
		
		for (int i = 0; i < 3; i++)
		{
			// Tạo 2 sprite cho mỗi layer (để loop)
			Sprite2D sprite1 = CreateColoredSprite(viewportWidth * 2, viewportHeight, colors[i]);
			Sprite2D sprite2 = CreateColoredSprite(viewportWidth * 2, viewportHeight, colors[i]);
			
			AddChild(sprite1);
			AddChild(sprite2);
			
			sprite1.ZIndex = -10 + i;
			sprite2.ZIndex = -10 + i;
			
			// Vị trí ban đầu
			sprite1.Position = new Vector2(viewportWidth, viewportHeight / 2);
			sprite2.Position = new Vector2(viewportWidth * 3, viewportHeight / 2);
			
			// Tốc độ khác nhau cho hiệu ứng parallax
			float speed = ScrollSpeed * (0.3f + (i * 0.3f));
			
			layers.Add(new BackgroundLayer(sprite1, sprite2, speed, viewportWidth * 2));
		}
	}
	
	private Sprite2D CreateColoredSprite(float width, float height, Color color)
	{
		// Tạo texture gradient đơn giản
		Image img = Image.Create((int)width, (int)height, false, Image.Format.Rgba8);
		
		for (int y = 0; y < height; y++)
		{
			float gradient = (float)y / height;
			Color pixelColor = color.Lerp(color.Darkened(0.3f), gradient);
			
			for (int x = 0; x < width; x++)
			{
				img.SetPixel(x, y, pixelColor);
			}
		}
		
		ImageTexture texture = ImageTexture.CreateFromImage(img);
		Sprite2D sprite = new Sprite2D();
		sprite.Texture = texture;
		sprite.Centered = true;
		
		return sprite;
	}
	
	private void SetupBackgroundLayers()
	{
		float viewportWidth = GetViewportRect().Size.X;
		float viewportHeight = GetViewportRect().Size.Y;
		
		// Chỉ tạo 1 layer cho bg.png
		Sprite2D sprite1 = new Sprite2D();
		Sprite2D sprite2 = new Sprite2D();
		
		sprite1.Texture = BackgroundTexture;
		sprite2.Texture = BackgroundTexture;
		
		AddChild(sprite1);
		AddChild(sprite2);
		
		sprite1.ZIndex = -20;
		sprite2.ZIndex = -20;
		
		// Scale để fit viewport height
		float scaleY = viewportHeight / BackgroundTexture.GetHeight();
		// Scale X để texture không bị méo
		float scaleX = scaleY;
		
		sprite1.Scale = new Vector2(scaleX, scaleY);
		sprite2.Scale = new Vector2(scaleX, scaleY);
		
		float bgWidth = BackgroundTexture.GetWidth() * scaleX;
		
		// Đặt vị trí 2 sprite nối tiếp nhau
		sprite1.Position = new Vector2(bgWidth / 2, viewportHeight / 2);
		sprite2.Position = new Vector2(bgWidth / 2 + bgWidth, viewportHeight / 2);
		
		layers.Add(new BackgroundLayer(sprite1, sprite2, ScrollSpeed, bgWidth));
	}
	
	public override void _Process(double delta)
	{
		if (player == null || layers.Count == 0) return;
		
		// Tính khoảng cách player di chuyển
		float playerMovement = player.Position.X - lastPlayerX;
		lastPlayerX = player.Position.X;
		
		// Di chuyển từng layer
		foreach (var layer in layers)
		{
			// Di chuyển sprite theo player với tốc độ parallax
			float moveAmount = playerMovement * layer.Speed;
			
			layer.Sprite1.Position = new Vector2(
				layer.Sprite1.Position.X - moveAmount,
				layer.Sprite1.Position.Y
			);
			
			layer.Sprite2.Position = new Vector2(
				layer.Sprite2.Position.X - moveAmount,
				layer.Sprite2.Position.Y
			);
			
			// Loop logic - khi sprite đi ra ngoài màn hình, đưa nó về phía sau
			float cameraX = player.Position.X;
			float viewportWidth = GetViewportRect().Size.X;
			
			// Nếu sprite1 đi ra khỏi màn hình bên trái
			if (layer.Sprite1.Position.X + layer.Width / 2 < cameraX - viewportWidth)
			{
				layer.Sprite1.Position = new Vector2(
					layer.Sprite2.Position.X + layer.Width,
					layer.Sprite1.Position.Y
				);
			}
			
			// Nếu sprite2 đi ra khỏi màn hình bên trái
			if (layer.Sprite2.Position.X + layer.Width / 2 < cameraX - viewportWidth)
			{
				layer.Sprite2.Position = new Vector2(
					layer.Sprite1.Position.X + layer.Width,
					layer.Sprite2.Position.Y
				);
			}
			
			// Tương tự cho phía bên phải (nếu player đi lùi)
			if (layer.Sprite1.Position.X - layer.Width / 2 > cameraX + viewportWidth * 2)
			{
				layer.Sprite1.Position = new Vector2(
					layer.Sprite2.Position.X - layer.Width,
					layer.Sprite1.Position.Y
				);
			}
			
			if (layer.Sprite2.Position.X - layer.Width / 2 > cameraX + viewportWidth * 2)
			{
				layer.Sprite2.Position = new Vector2(
					layer.Sprite1.Position.X - layer.Width,
					layer.Sprite2.Position.Y
				);
			}
		}
	}
	
	// Reset background về vị trí ban đầu
	public void ResetBackground()
	{
		if (player == null || layers.Count == 0) return;
		
		float viewportWidth = GetViewportRect().Size.X;
		float viewportHeight = GetViewportRect().Size.Y;
		
		foreach (var layer in layers)
		{
			layer.Sprite1.Position = new Vector2(layer.Width / 2, viewportHeight / 2);
			layer.Sprite2.Position = new Vector2(layer.Width / 2 + layer.Width, viewportHeight / 2);
		}
		
		lastPlayerX = player.Position.X;
	}
}
