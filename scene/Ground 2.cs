using Godot;
using System;

public partial class Ground : Sprite2D
{
	public bool dead=false;
	public Rect2 gr=>groundhitbox(this);
	public override void _Ready()
	{
		
	}
	
	//Reset mặt đất về vị trí ban đầu
	public void reset()
	{
		Position=new Vector2(GetViewportRect().Size.X/3,GetViewportRect().Size.Y*3/4);
		dead=false;
	}
	
	//Cho mặt đất di chuyển theo con chim
	public override void _Process(double delta)
	{
		if(dead)return;
		Vector2 v=Vector2.Zero;
		v.X=200;
		v.Y=0;
		Position+=v*(float)delta;
	}
	
	//Tính cái HitBox của cái mặt đất để xem có va chạm với Player
	private Rect2 groundhitbox(Sprite2D ground)
	{
		Texture2D tex = ground.Texture;
		if (tex == null) return new Rect2();
		Vector2 texsize = tex.GetSize();
		Vector2 size=texsize*ground.Scale;	
		Vector2 pos = ground.GlobalPosition - (size / 2);
		return new Rect2(pos, size);
	}
}
