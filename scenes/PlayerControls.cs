using Godot;
using System;

public partial class PlayerControls : CharacterBody2D {
	public const int Speed = 200;
	public const int Jump = -400;
	public const int Fall= 1000;
	public double Waitting_Time = 0;
	public bool isFalling = false; // Rơi khi chạm trần khác rơi bình thường
	public bool isDead = false;
	public const int BotY = 364; // Xác định qua Bot   
	public const int TopY = -300 ;  // Xác định qua Top
	public Vector2 Start_Position = new Vector2(192,226); // xác định qua PlayerControls
	
	public override void _PhysicsProcess(double delta) {
		if (isDead){
			Waitting_Time += delta;
			if (Waitting_Time >= 3)
				Restart();
			return;
		}
		Vector2 velocity= Velocity;
		if((!isFalling)&&(Input.IsActionPressed("Jump")))
			{
				velocity.Y=Jump;
			}
		velocity.Y+=Fall*(float)delta; //v=g*t
		if (isFalling) velocity.X = 0;
		else velocity.X=Speed; // vận tốc bthuog
		Velocity=velocity;
		MoveAndSlide();
		if (Position.Y <= TopY) {  
			isFalling = true;
			Position = new Vector2(Position.X,TopY);
		}
		if (Position.Y >= BotY) 
			Dead();
	}
	
	public void Dead() {
		isDead = true;
		isFalling = false;
		Velocity = Vector2.Zero;
	}
	
	public void Restart() {
		Waitting_Time = 0;
		Position = Start_Position; 
		isDead = false;
	}
}
	
