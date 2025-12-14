using Godot;
using System;

public partial class Pipe : Node2D
{
	public float Speed = 220;
	public float Gap = 180;
	public float MinY = -120;
	public float MaxY = 120;
	private Sprite2D Top;
	private Sprite2D Bot;
	public Rect2 TopPipe => PipeHitbox(Top);
	public Rect2 BotPipe => PipeHitbox(Bot);
	private Node2D player;
	private Sprite2D Ground;
	private PipeSpawner Pipes;
	public bool pointed=false;
	
	public override void _Ready()
	{
		Top = GetNode<Sprite2D>("Top");
		Bot = GetNode<Sprite2D>("Bottom");
		player=GetNode<Node2D>("../Player");
		Ground=GetNode<Sprite2D>("../Ground");
		Pipes=GetNode<PipeSpawner>("../PipeSpawner");
		Top.ZIndex = 0;
		Bot.ZIndex = 0;
	}
	
	public void Setup(float CenterY, float gap)
	{
		Gap = gap;
		float WindowHeight = GetViewportRect().Size.Y;
		//648
		float TopPipeHeight = (WindowHeight/2) + Gap;
		float TopPipeSpriteHeight = Top.Texture.GetHeight();
		Top.Scale = new Vector2(1, TopPipeHeight / TopPipeSpriteHeight);
		Top.Position = new Vector2(0, CenterY - Gap * (float)0.5 - (TopPipeHeight/2));
		float BottomPipeHeight = (WindowHeight / 2) + Gap;
		float BottomPipeSpriteHeight = Bot.Texture.GetHeight();
		Bot.Scale = new Vector2(1, BottomPipeHeight / BottomPipeSpriteHeight);
		Bot.Position = new Vector2(0, CenterY + Gap * (float)0.5 + (BottomPipeHeight/2));
	}

	public override void _Process(double delta)
	{
		if(Pipes.stop)
		{
			return;	
		}
		Position = new Vector2(Position.X - Speed * (float)delta, Position.Y);
		if (GlobalPosition.X < player.GlobalPosition.X-1300) 
		{
			Pipes.RemovePipe(this);
			QueueFree();
		}
		QueueRedraw();
	}	
	
	private Rect2 PipeHitbox(Sprite2D pipe)
	{
		Texture2D tex = pipe.Texture;
		if (tex == null) return new Rect2();
		Vector2 texsize = tex.GetSize();
		Vector2 size=texsize*pipe.Scale;	
		Vector2 pos = pipe.GlobalPosition - (size / 2);
		return new Rect2(pos, size);
	}
	
	
	public override void _Draw()
	{
		DrawRect
		(
			new Rect2(ToLocal(TopPipe.Position), TopPipe.Size),new Color(1, 0, 0),false
		);
		DrawRect
		(
			new Rect2(ToLocal(BotPipe.Position), BotPipe.Size),new Color(1, 0, 0),false
		);
	}
}
