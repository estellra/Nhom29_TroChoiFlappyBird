using Godot;
using System;

public partial class Player : Node2D
{
	private Vector2 velocity=Vector2.Zero;
	private float gravity=700;
	private float speed=200;
	private float jump=-350;
	private float radiushitbox=32 ;
	public bool dead;
	private bool start;
	private float Waiting_Time;
	private Vector2 Start_Position;
	private PipeSpawner Pipes;
	private CatSpawner Cats;
	private AnimatedSprite2D sprite;
	private Ground ground;
	private Main main;
	private RichTextLabel diem;
	public int point;
	private Audio audio;
	private CanvasLayer GameOverUI;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Draw()
	{
		DrawCircle(Vector2.Zero, radiushitbox, new Color(1, 0, 0), false); 
	}
	public void Start()
	{
		Pipes.ClearPipe();
		Cats.ClearCat();
		Start_Position = new Vector2(150 , (GetViewportRect().Size.Y / 2)-(GetViewportRect().Size.Y/4));
		Position = Start_Position;
		velocity=Vector2.Zero;
		Rotation = 0;
		dead=false;
		start=true;
		Pipes.stop=false;
		Cats.stop=false;
		ground.reset();
		point=0;
		diem.Text=point.ToString();
	}
	public override void _Ready()
	{
		Pipes=GetNode<PipeSpawner>("../PipeSpawner");
		Cats=GetNode<CatSpawner>("../CatSpawner");
		sprite=GetNode<AnimatedSprite2D>("Sprite2D");
		int SkinDaLuu=GlobalData.SkinDangChon;
		sprite.Play("skin_"+SkinDaLuu);
		ground=GetNode<Ground>("../Ground");
		diem=GetNode<RichTextLabel>("../ig_ui/Points");
		this.ZIndex=2;
		audio = GetNode<Audio>("/root/Audio");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(!start) return;
		if(dead) return;
		Control();
		Physic((float)delta);
		CheckForCollision();
		tinhdiem();
		QueueRedraw();
	}
	public string tinhdiem()
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
					audio.PlayPoint();
				}	
		}
		return point.ToString();
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
					ground.dead=true;
					velocity=new Vector2(0, 0);
					Pipes.stop=true;
					Cats.stop=true;
					Die();
					//return;
		}
		foreach(Pipe pipe in Pipes.PipeList)
		{
			if(Collision(getCenter(),radiushitbox,pipe.TopPipe)||
			Collision(getCenter(),radiushitbox,pipe.BotPipe))
				{
					ground.dead=true;
					velocity= new Vector2(0,velocity.Y);
					Pipes.stop=true;
					Cats.stop=true;
					Die();
					//break;
				}
		}
		if(Cats==null) return;
		foreach(Cat cat in Cats.CatList)
		{
			if(Collision(getCenter(),radiushitbox,cat.meomeo))
				{
					ground.dead=true;
					velocity= new Vector2(0,velocity.Y);
					Pipes.stop=true;
					Cats.stop=true;
					Die();
					return;
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
		if(Input.IsActionJustPressed("control_jump")){
			velocity.Y=jump;
			audio.PlayFlap();
		}
		//if(Input.IsActionJustPressed("control_pause"))
	}
	private void Physic(float delta)
	{
		  velocity.X=speed;
		  velocity.Y+=gravity*delta;
		  Position +=velocity*delta;
	}
	
	private void Die()
{
	if (dead) return;
	var db = new DatabaseManager();
	db.SaveScore("Player", point);
	GetNode<UserData>("../UserData").CheckAndSave(point);
	dead = true;
	audio.PlayHit(); 
	if (main != null) 
	{
		main.GameOver();
	}
}
}
