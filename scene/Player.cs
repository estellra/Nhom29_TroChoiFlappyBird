using Godot;
using System;

public partial class Player : Node2D
{
	private Vector2 velocity=Vector2.Zero;
	private float gravity=700;
	private float speed=200;
	private float jump=-350;
	private float radiushitbox=44 ;
	public bool dead;
	private bool start;
	private float Waiting_Time;
	private Vector2 Start_Position;
	private PipeSpawner Pipes;
	private AnimatedSprite2D sprite;
	private Ground ground;
	private RichTextLabel diem;
	public int point;
	// Called when the node enters the scene tree for the first time.
	public override void _Draw()
	{
		DrawCircle(Vector2.Zero, radiushitbox, new Color(1, 0, 0), false); 
	}
	public void Start()
	{
		Start_Position = new Vector2(150 , (GetViewportRect().Size.Y / 2)-(GetViewportRect().Size.Y/4));
		Position = Start_Position;
		velocity=Vector2.Zero;
		dead=false;
		start=true;
		Pipes.stop=false;
		ground.reset();
		point=0;
		diem.Text=point.ToString();
	}
	public override void _Ready()
	{
		Pipes=GetNode<PipeSpawner>("../PipeSpawner");
		sprite=GetNode<AnimatedSprite2D>("Sprite2D");
		ground=GetNode<Ground>("../Ground");
		diem=GetNode<RichTextLabel>("../ig_ui/Points");
		this.ZIndex=2;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(!start) return;
		if(dead) return;
		sprite.Play("flap");
		Control();
		Physic((float)delta);
		CheckForCollision();
		tinhdiem();
		QueueRedraw();
	}
	private void tinhdiem()
	{
		foreach(Pipe pipe in Pipes.PipeList)
		{
			if(pipe.pointed==true) continue;
			if(getCenter().X>pipe.TopPipe.Position.X+pipe.TopPipe.Size.X/2&&
			getCenter().X>pipe.BotPipe.Position.X+pipe.BotPipe.Size.X/2)
				{
					point++;
					pipe.pointed=true;
					diem.Text=point.ToString();
				}
		}
	}
	private Vector2 getCenter()
	{
		return GlobalPosition;
	}
	private void CheckForCollision()
	{
		if(Pipes==null) return;
		if(Collision(getCenter(),radiushitbox,ground.gr))
		{
					dead=true;
					ground.dead=true;
					velocity=Vector2.Zero;
					Pipes.stop=true;
					return;
		}
		foreach(Pipe pipe in Pipes.PipeList)
		{
			if(Collision(getCenter(),radiushitbox,pipe.TopPipe)||
			Collision(getCenter(),radiushitbox,pipe.BotPipe))
				{
					dead=true;
					ground.dead=true;
					velocity=Vector2.Zero;
					Pipes.stop=true;
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
	


}
