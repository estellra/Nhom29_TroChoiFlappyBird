using Godot;

public partial class Sfx : Node
{
	private AudioStreamPlayer bgm;
	private AudioStreamPlayer flap;
	private AudioStreamPlayer hit;
	private AudioStreamPlayer point;

	public override void _Ready()
	{
		bgm   = GetNode<AudioStreamPlayer>("BGMSfx");
		flap  = GetNode<AudioStreamPlayer>("FlapSfx");
		hit   = GetNode<AudioStreamPlayer>("HitSfx");
		point = GetNode<AudioStreamPlayer>("PointSfx");
	}

	public void PlayBgm()
	{
		if (!bgm.Playing)
			bgm.Play();
	}

	public void StopBgm()
	{
		bgm.Stop();
	}

	public void PlayFlap()
	{
		flap.Stop();
		flap.Play();
	}

	public void PlayHit()
	{
		hit.Stop();
		hit.Play();
	}

	public void PlayPoint()
	{
		point.Stop();
		point.Play();
	}
}
