using Godot;
using System;

public partial class PlayerControls : CharacterBody2D
{
	public const int Speed = 200;
	public const int Jump = -400;
	public const int Fall= 1000;
	public bool isDead = false;
	public const int BotY = 365; // Xác định qua Bot
	public const int TopY = -312; // Xác định qua Top
	public override void _PhysicsProcess(double delta)
	{
			if (isDead)
				return;
			Vector2 velocity= Velocity;
			if(Input.IsActionPressed("Jump"))
			{
				velocity.Y=Jump;
			}
			velocity.Y+=Fall*(float)delta; //v=g*t
			velocity.X=Speed; // vận tốc bthuog
			Velocity=velocity;
			MoveAndSlide();
			if ((Position.Y >= BotY)||(Position.Y <= -312)){     
				Position = new Vector2(Position.X, BotY); 
				Dead();
			}
	}

	public void Dead() {
		if (isDead) return;
		isDead = true;
		Velocity = Vector2.Zero;
	}
}
