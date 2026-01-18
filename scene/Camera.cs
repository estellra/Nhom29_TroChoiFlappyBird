using Godot;
using System;
using System.Collections.Generic;

public partial class Camera : Node2D
{
	//Khởi tạo vị trí camera so với con chim 
	[Export] public NodePath PlayerPath;
	private Node2D player;
	[Export] public float CameraSmooth = 8f;
	private Vector2 desiredPlayerScreenPos; 
<<<<<<< Updated upstream
	private Vector2 currentCameraPos = Vector2.Zero;
	
	// List các node cần di chuyển theo camera (KHÔNG bao gồm Background)
	private List<Node2D> nodesToMove = new List<Node2D>();
	
=======
	private Vector2 currentCameraPos = Vector2.Zero; 
	
	//Link node Player
>>>>>>> Stashed changes
	public override void _Ready()
	{
		// Lấy player
		if (PlayerPath != null)
			player = GetNode<Node2D>(PlayerPath);
		else
			player = GetParent().GetNodeOrNull<Node2D>("Player");
		
		if (player == null)
		{
			GD.PrintErr("KHÔNG TÌM THẤY PLAYER! Vào Inspector của Camera, kéo Player vào ô PlayerPath");
			return;
		}
		
		// Vị trí mong muốn của player trên màn hình
		Vector2 viewport = GetViewportRect().Size;
		desiredPlayerScreenPos = new Vector2(viewport.X / 3, viewport.Y / 2);
		
		// Khởi tạo camera tại vị trí player
		currentCameraPos = player.Position - desiredPlayerScreenPos;
		
		// Lấy tất cả các node cần di chuyển theo camera (trừ Bg và Player)
		CollectNodesToMove();
		
		GD.Print("=== CAMERA READY ===");
		GD.Print($"Player: {player.Name}");
		GD.Print($"Desired screen pos: {desiredPlayerScreenPos}");
		GD.Print($"Nodes to move: {nodesToMove.Count}");
	}
	
	private void CollectNodesToMove()
	{
		// Lấy tất cả các node con của main (trừ Bg, Camera, và Player)
		Node parent = GetParent();
		if (parent == null) return;
		
		foreach (Node child in parent.GetChildren())
		{
			if (child is Node2D node2D)
			{
				string nodeName = node2D.Name;
				
				// KHÔNG di chuyển: Background, Camera, Player
				if (nodeName == "Bg" || nodeName == "Camera" || nodeName == "Player")
					continue;
				
				// DI CHUYỂN: Ground, PipeSpawner, và các node khác
				nodesToMove.Add(node2D);
				GD.Print($"Camera will move: {nodeName}");
			}
		}
	}
	
	//Tính vị trí camera đi theo con chim
	public override void _Process(double delta)
	{
		if (player == null) return;
		
		// Camera target = vị trí player - offset mong muốn
		Vector2 targetCameraPos = player.Position - desiredPlayerScreenPos;
		
		// Smooth camera movement
		currentCameraPos = currentCameraPos.Lerp(targetCameraPos, CameraSmooth * (float)delta);
		
		// Dùng CanvasTransform để di chuyển TOÀN BỘ viewport
		// Background sẽ tự bù trừ chuyển động này trong code Bg.cs
		GetViewport().CanvasTransform = Transform2D.Identity.Translated(-currentCameraPos);
	}
	
	//Cho cái camera lại trước chỗ con chim spam
	public void ResetCamera()
	{
		if (player != null)
		{
			currentCameraPos = player.Position - desiredPlayerScreenPos;
			
			// Reset vị trí các node về 0
			foreach (Node2D node in nodesToMove)
			{
				if (node != null && IsInstanceValid(node))
					node.Position = Vector2.Zero;
			}
		}
	}
}
