using Godot;
using System;

public partial class Player : Node2D
{
	private Vector2 velocity=Vector2.Zero;
	private float gravity=500;
	private float speed=200;
	private float jump=-350;
	private float radiushitbox=44 ;
	private bool dead=false;
	private float Waiting_Time;
	private Vector2 Start_Position;
	private PipeSpawner Pipes;
	// Called when the node enters the scene tree for the first time.
	public override void _Draw()
	{
		DrawCircle(Vector2.Zero, radiushitbox, new Color(1, 0, 0), false); 
	}
	public override void _Ready()
	{
		Start_Position = new Vector2(150 , (GetViewportRect().Size.Y / 2)-(GetViewportRect().Size.Y/4));
		Position = Start_Position;
		Pipes=GetNode<PipeSpawner>("../PipeSpawner");
		this.ZIndex=2;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
			if(dead)
			{
			Waiting_Time += (float)delta;
			if (Waiting_Time >= 3)
				Restart();
			return;
			}
		Control();
		Physic((float)delta);
		CheckForCollision();
		QueueRedraw();
	}
	private Vector2 getCenter()
	{
		return GlobalPosition;
	}
	private void CheckForCollision()
	{
		if(Pipes==null) return;
		foreach(Pipe pipe in Pipes.PipeList)
		{
			if(Collision(getCenter(),radiushitbox,pipe.TopPipe)||
			Collision(getCenter(),radiushitbox,pipe.BotPipe))
				{
					dead=true;
					velocity=Vector2.Zero;
					break;
				}
		}
	}
	private bool Collision(Vector2 HitboxCenter,float radius, Rect2 obj)
	{
		float X = Mathf.Clamp(HitboxCenter.X, obj.Position.X, obj.Position.X + obj.Size.X);
		float Y = Mathf.Clamp(HitboxCenter.Y, obj.Position.Y, obj.Position.Y + obj.Size.Y);
		float dx = HitboxCenter.X - X;
		float dy = HitboxCenter.Y - Y;
		return (dx * dx + dy * dy) < (radius * radius); //so sánh độ dài đường đi ngắn nhấn từ tâm của hitbox 
														//đến obj so với bán kính của hitbox
	}
	private void Control()
	{
		if(Input.IsActionJustPressed("control_jump")) velocity.Y=jump;
		//if(Input.IsActionJustPressed("control_pause"))
	}
	private void Physic(float delta)
	{
		  velocity.X=speed;
		  velocity.Y+=gravity*delta;
		  Position +=velocity*delta;
	}
	
	public void Restart() {
		Waiting_Time = 0;
		Position = Start_Position; 
		dead = false;
	}
	

}
