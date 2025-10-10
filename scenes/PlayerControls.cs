using Godot;
using System;

public partial class PlayerControls : Node2D{
	private Vector2 velocity=Vector2.Zero;
	private float gravity=500;
	private float speed=200;
	private float jump=-400;
	private float radiushitbox=40;
	Rect2 test=new Rect2(300,200,80,40);
	private Font font;

	//
	public double Waitting_Time = 0;
	public bool isFalling = false; // Rơi khi chạm trần khác rơi bình thường
	public bool isDead = false;
	public const int BotY = 364; // Xác định qua Bot   
	public const int TopY = -300 ;  // Xác định qua Top
	public Vector2 Start_Position = new Vector2(192,226); // xác định qua PlayerControls
	
	/*public override void _PhysicsProcess(double delta) {
		if (isDead){
			Waitting_Time += delta;
			if (Waitting_Time >= 3)
				Restart();
			return;
		}
		if (Position.Y <= TopY) {  
			isFalling = true;
			Position = new Vector2(Position.X,TopY);
		}
		if (Position.Y >= BotY) 
			Dead();
	}*/
	
	public void Dead() {
		isDead = true;
		isFalling = false;
		velocity = Vector2.Zero;
	}
	
	public void Restart() {
		Waitting_Time = 0;
		Position = Start_Position; 
		isDead = false;
	}
	
	public override void _Draw()
	{
		DrawCircle(Vector2.Zero, radiushitbox, new Color(1, 0, 0, 0.3f));
		DrawCircle(Vector2.Zero, radiushitbox, new Color(1, 0, 0), false); 
		Rect2 local = new Rect2
		(
		test.Position - GlobalPosition,
		test.Size
		);
		DrawRect(local , Colors.Red, filled: false, width: 2);
		if (isDead && font != null)
		{
			string text = "you died:(";
			int fontSize = 48;
		DrawString(font,  Vector2.Zero, text, HorizontalAlignment.Center, -1, fontSize, Colors.Red);
		}
	}
	public override void _Ready()
	{
		Position = new Vector2(100, GetViewportRect().Size.Y / 2);
		font=ResourceLoader.Load<Font>("res://asset/font/RasterForgeRegular-JpBgm.ttf");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
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
	
		if(Collision(getCenter(),radiushitbox,test))
		{
			GD.Print("ban da die!");
			isDead=true;
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
		if(Input.IsActionJustPressed("Jump")) velocity.Y=jump;
		//if(Input.IsActionJustPressed("control_pause"))
	}
	private void Physic(float delta)
	{
		if(isDead) return;
		  velocity.X=speed;
		  velocity.Y+=gravity*delta;
		  Position +=velocity*delta;
	}
}
	
  
