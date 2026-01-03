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
	private Main main;
	private RichTextLabel diem;
	public int point;
	private Sfx sfx;
	private CanvasLayer GameOverUI;
	
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
		Rotation = 0;
		dead=false;
		start=true;
		Pipes.stop=false;
		ground.reset();
		point=0;
		diem.Text=point.ToString();
		GameOverUI = GetNode<CanvasLayer>("../GameOverUI");
		GameOverUI.ProcessMode = ProcessModeEnum.Always;
		GameOverUI.Hide();
		//sfx.PlayBgm(); 
	}
	public override void _Ready()
	{
		Pipes=GetNode<PipeSpawner>("../PipeSpawner");
		sprite=GetNode<AnimatedSprite2D>("Sprite2D");
		int SkinDaLuu=GlobalData.SkinDangChon;
		sprite.Play("skin_"+SkinDaLuu);
		ground=GetNode<Ground>("../Ground");
		diem=GetNode<RichTextLabel>("../ig_ui/Points");
		this.ZIndex=2;
		sfx = GetNode<Sfx>("../Sfx");
		sfx.PlayBgm();
		GameOverUI = GetNode<CanvasLayer>("../GameOverUI");
		GameOverUI.ProcessMode = ProcessModeEnum.Always;
		GameOverUI.Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(!start) return;
		if(dead) return;
		Control();
		Physic((float)delta);
		RotateBird((float)delta);
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
					sfx.PlayPoint();
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
					Die();
					//break;
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
			sfx.PlayFlap();
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
	dead = true;
	sfx.PlayHit(); 
	GameOverUI.Show();
	GameOverUI.GetNode<TextureRect>("ScorePanel").Show();
	GameOverUI.GetNode<TextureRect>("Medal").Show();
	GetNode<TextureRect>("../GamePausedUI//PauseBtn").Hide();
	if (main != null) 
	{
		main.GameOver();
	}
}

private void RotateBird(float delta)
{
	if (velocity.Y < 0)
	{
		Rotation = Mathf.DegToRad(-25);
	}
	else if (velocity.Y > 0)
	{
		Rotation += 3f * delta;
		if (Rotation > Mathf.DegToRad(90))
		{
			Rotation = Mathf.DegToRad(90);
		}
	}
}

}
