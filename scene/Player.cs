using Godot;
using System;

public partial class Player : Node2D
{
	//Khởi tạo giá trị vật lý của người chơi và chứng ngại vật
	private Vector2 velocity=Vector2.Zero;
	private float gravity=500;
	private float speed=200;
	private float jump=-350;
<<<<<<< Updated upstream
	private float radiushitbox=44 ;
	private bool dead=false;
	private float Waiting_Time;
=======
	private float radiushitbox=32 ;
	public bool dead;
	private bool start;
>>>>>>> Stashed changes
	private Vector2 Start_Position;
	private PipeSpawner Pipes;
	// Called when the node enters the scene tree for the first time.
<<<<<<< Updated upstream
	public override void _Draw()
	{
		DrawCircle(Vector2.Zero, radiushitbox, new Color(1, 0, 0), false); 
	}
=======
	
	//Khởi tạo giá trị mặc định để bắt đầu game mới
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
	
	//Lấy dữ liệu từ các Sprite khác
>>>>>>> Stashed changes
	public override void _Ready()
	{
		Start_Position = new Vector2(150 , (GetViewportRect().Size.Y / 2)-(GetViewportRect().Size.Y/4));
		Position = Start_Position;
		Pipes=GetNode<PipeSpawner>("../PipeSpawner");
		this.ZIndex=2;
	}
	
	//Cập nhật game theo từng frame
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
<<<<<<< Updated upstream
=======
	
	//Tính điểm trong game
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
	
	//Lấy vị trí người chơi
>>>>>>> Stashed changes
	private Vector2 getCenter()
	{
		return GlobalPosition;
	}
	
	//Kiểm tra người chơi với chướng ngại vật có va chạm không?
	private void CheckForCollision()
	{
		if(Pipes==null) return;
<<<<<<< Updated upstream
=======
		if(Collision(getCenter(),radiushitbox,ground.gr))
		{
					ground.dead=true;
					velocity=new Vector2(0, 0);
					Pipes.stop=true;
					Cats.stop=true;
					Die();
		}
>>>>>>> Stashed changes
		foreach(Pipe pipe in Pipes.PipeList)
		{
			if(Collision(getCenter(),radiushitbox,pipe.TopPipe)||
			Collision(getCenter(),radiushitbox,pipe.BotPipe))
				{
<<<<<<< Updated upstream
					dead=true;
					velocity=Vector2.Zero;
					break;
=======
					ground.dead=true;
					velocity= new Vector2(0,velocity.Y);
					Pipes.stop=true;
					Cats.stop=true;
					Die();
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
>>>>>>> Stashed changes
				}
		}
	}
	
	//Hàm kiểm tra va chạm
	private bool Collision(Vector2 HitboxCenter,float radius, Rect2 obj)
	{
		float X = Mathf.Clamp(HitboxCenter.X, obj.Position.X, obj.Position.X + obj.Size.X);
		float Y = Mathf.Clamp(HitboxCenter.Y, obj.Position.Y, obj.Position.Y + obj.Size.Y);
		float dx = HitboxCenter.X - X;
		float dy = HitboxCenter.Y - Y;
		return (dx * dx + dy * dy) < (radius * radius); //so sánh độ dài đường đi ngắn nhấn từ tâm của hitbox 
														//đến obj so với bán kính của hitbox
	}
	
	//Nhận input từ bàn phím
	private void Control()
	{
		if(Input.IsActionJustPressed("control_jump")) velocity.Y=jump;
		//if(Input.IsActionJustPressed("control_pause"))
	}
	
	//Hàm cho chim có trọng lực
	private void Physic(float delta)
	{
		  velocity.X=speed;
		  velocity.Y+=gravity*delta;
		  Position +=velocity*delta;
	}
	
<<<<<<< Updated upstream
	public void Restart() {
		Waiting_Time = 0;
		Position = Start_Position; 
		dead = false;
	}
	

=======
	//Khởi tạo DataBase khi chim chết
	private void Die()
	{
		if (dead) return;
		var db = new DatabaseManager();
		db.SaveScore("Player", point);
		GetNode<UserData>("../UserData").CheckAndSave(point);
		dead = true;
		sfx.PlayHit(); 
		if (main != null) 
		{
			main.GameOver();
		}
	}
>>>>>>> Stashed changes
}
