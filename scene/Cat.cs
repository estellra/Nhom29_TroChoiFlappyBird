using Godot;
using System;

public partial class Cat : Node2D
{
	public float Speed = 500;
	public float MinY = -120;
	public float MaxY = 120;
	private Sprite2D conmeo;
	public Rect2 meomeo => CatHitbox(conmeo);
	private Node2D player;
	private Sprite2D Ground;
	private CatSpawner Cats;
	
	public override void _Ready()
	{
		conmeo = GetNode<Sprite2D>("ConMeo");
		player=GetNode<Node2D>("../Player");
		Ground=GetNode<Sprite2D>("../Ground");
		Cats=GetNode<CatSpawner>("../CatSpawner");
		conmeo.ZIndex = 2;
		conmeo.Scale = new Vector2(0.05f,0.05f);
		conmeo.Position = new Vector2(-500, -500);

	}
	
	public void Setup(float PosY)
	{
		conmeo.Position = new Vector2(0, PosY);
	}

	public override void _Process(double delta)
	{
		if(Cats.stop)
		{
			return;	
		}
		Position = new Vector2(Position.X - Speed * (float)delta, Position.Y);
		if (GlobalPosition.X < player.GlobalPosition.X-1300) 
		{
			Cats.RemoveCat(this);
			QueueFree();
		}
		QueueRedraw();
	}	
	
	private Rect2 CatHitbox(Sprite2D meo)
	{
		Texture2D tex = meo.Texture;
		if (tex == null) return new Rect2();
		Vector2 texsize = tex.GetSize();
		Vector2 size=texsize*meo.Scale*0.8f;	
		Vector2 pos = meo.GlobalPosition - (size / 2);
		return new Rect2(pos, size);
	}
}
