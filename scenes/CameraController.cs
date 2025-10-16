using Godot;
using System;
using System.Collections.Generic;

public partial class CameraController : Node2D
{
	[Export] public NodePath PlayerPath;
	private Node2D player;
	
	[Export] public float CameraSmooth = 8f;
	
	private Vector2 desiredPlayerScreenPos; 
	private Vector2 currentCameraPos = Vector2.Zero; // Vị trí camera hiện tại (world space)
	
	public override void _Ready()
	{
		// Lấy player
		if (PlayerPath != null)
			player = GetNode<Node2D>(PlayerPath);
		else
			player = GetParent().GetNodeOrNull<Node2D>("PlayerControls");
		
		if (player == null)
		{
			GD.PrintErr("KHÔNG TÌM THẤY PLAYER! Vào Inspector của CameraController, kéo PlayerControls vào ô PlayerPath");
			return;
		}
		
		// Vị trí mong muốn của player trên màn hình
		Vector2 viewport = GetViewportRect().Size;
		desiredPlayerScreenPos = new Vector2(viewport.X / 3, viewport.Y / 2);
		
		// Khởi tạo camera tại vị trí player
		currentCameraPos = player.Position - desiredPlayerScreenPos;
		
		GD.Print("=== CAMERA READY ===");
		GD.Print($"Player: {player.Name}");
		GD.Print($"Desired screen pos: {desiredPlayerScreenPos}");
	}
	
	public override void _Process(double delta)
	{
		if (player == null) return;
		
		// Camera target = vị trí player - offset mong muốn
		Vector2 targetCameraPos = player.Position - desiredPlayerScreenPos;
		
		// Smooth camera movement
		currentCameraPos = currentCameraPos.Lerp(targetCameraPos, CameraSmooth * (float)delta);
		
		// Di chuyển player về vị trí hiển thị mong muốn
		// Player logic vẫn di chuyển bình thường, nhưng render ở vị trí khác
		Vector2 playerRenderPos = player.Position - currentCameraPos;
		
		// Dùng Transform để không ảnh hưởng logic
		// Ta sẽ offset toàn bộ viewport
		GetViewport().CanvasTransform = Transform2D.Identity.Translated(-currentCameraPos);
	}
	
	public void ResetCamera()
	{
		if (player != null)
			currentCameraPos = player.Position - desiredPlayerScreenPos;
	}
}
