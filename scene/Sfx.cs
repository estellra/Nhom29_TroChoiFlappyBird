using Godot;

public partial class Sfx : Node
{
	//Âm thanh khởi tạo
	private AudioStreamPlayer bgm;
	private AudioStreamPlayer flap;
	private AudioStreamPlayer hit;
	private AudioStreamPlayer point;

	//Link âm thanh
	public override void _Ready()
	{
		bgm   = GetNode<AudioStreamPlayer>("BGMSfx");
		flap  = GetNode<AudioStreamPlayer>("FlapSfx");
		hit   = GetNode<AudioStreamPlayer>("HitSfx");
		point = GetNode<AudioStreamPlayer>("PointSfx");
	}

	//Nhạc background
	public void PlayBgm()
	{
		if (!bgm.Playing)
			bgm.Play();
	}

	//Dừng nhạc background lúc chết
	public void StopBgm()
	{
		bgm.Stop();
	}

	//Tiếng vỗ cánh một lần
	public void PlayFlap()
	{
		flap.Stop();
		flap.Play();
	}
	
	//Chết một lần
	public void PlayHit()
	{
		hit.Stop();
		hit.Play();
	}
	
	//Qua ống một lần
	public void PlayPoint()
	{
		point.Stop();
		point.Play();
	}
}
