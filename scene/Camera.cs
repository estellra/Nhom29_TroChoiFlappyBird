using Godot;
using System;
using System.Collections.Generic;

public partial class Camera : Node2D
{
	[Export] public NodePath PlayerPath;
	private Node2D player;
	[Export] public float CameraSmooth = 8f;
	private Vector2 desiredPlayerScreenPos; 
	private Vector2 currentCameraPos = Vector2.Zero; 
	public override void _Ready()
	{
		if (PlayerPath != null)
			player = GetNode<Node2D>(PlayerPath);
		else
			player = GetParent().GetNodeOrNull<Node2D>("Player");
		
		if (player == null) return;
		Vector2 viewport = GetViewportRect().Size;
		desiredPlayerScreenPos = new Vector2(viewport.X / 3, viewport.Y / 2);
		currentCameraPos = player.Position - desiredPlayerScreenPos;
	}
	
	public override void _Process(double delta)
	{
		if (player == null) return;
		Vector2 targetCameraPos = player.Position - desiredPlayerScreenPos;
		currentCameraPos = currentCameraPos.Lerp(targetCameraPos, CameraSmooth * (float)delta);
		Vector2 playerRenderPos = player.Position - currentCameraPos;
		GetViewport().CanvasTransform = Transform2D.Identity.Translated(-currentCameraPos);
		this.GlobalPosition = currentCameraPos;
	}
	
	public void ResetCamera()
	{
		if (player != null)
			currentCameraPos = player.Position - desiredPlayerScreenPos;
	}
}
