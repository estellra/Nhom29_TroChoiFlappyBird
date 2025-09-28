using Godot;
using System;

public partial class PlayerControls : CharacterBody2D
{
	public const int Speed = 200;
	public const int Jump = -400;
	public const int Fall= 1000;
	public override void _PhysicsProcess(double delta)
	{
			Vector2 velocity= Velocity;
			if(Input.IsActionPressed("Jump"))
			{
				velocity.Y=Jump;
			}
			velocity.Y+=Fall*(float)delta; //v=g*t
			velocity.X=Speed; // vận tốc bthuog
			Velocity=velocity;
			MoveAndSlide();
	}
}
