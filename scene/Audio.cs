// Audio.cs
using Godot;

public partial class Audio : Node
{
	[Export] public NodePath BgmPlayerPath = "Bgm";
	[Export] public NodePath FlapPlayerPath = "FlapSfx";
	[Export] public NodePath HitPlayerPath = "HitSfx";
	[Export] public NodePath PointPlayerPath = "PointSfx";

	[Export] public string SfxBusName = "SFX";
	[Export] public string BgmBusName = "BGM";

	[Export] public float MinDb = -80f;
	[Export] public float MaxDb = 0f;

	private const string SavePath = "user://audio.save";
	private const string Section = "audio";
	private const float MuteEps = 0.05f;

	private AudioStreamPlayer _bgm;
	private AudioStreamPlayer _flap;
	private AudioStreamPlayer _hit;
	private AudioStreamPlayer _point;

	private int _sfxBus = -1;
	private int _bgmBus = -1;

	private float _sfxDb = -40f;
	private float _bgmDb = -40f;

	public override void _Ready()
	{
		AddToGroup("audio_manager");

		_bgm = GetNodeOrNull<AudioStreamPlayer>(BgmPlayerPath);
		_flap = GetNodeOrNull<AudioStreamPlayer>(FlapPlayerPath);
		_hit = GetNodeOrNull<AudioStreamPlayer>(HitPlayerPath);
		_point = GetNodeOrNull<AudioStreamPlayer>(PointPlayerPath);

		_sfxBus = AudioServer.GetBusIndex(SfxBusName);
		_bgmBus = AudioServer.GetBusIndex(BgmBusName);

		LoadVolumes();
		ApplySfxBus(_sfxDb);
		ApplyBgmBus(_bgmDb);
		
		ApplySfxBus(0f);
		PlayFlap();
		
		TestSfx();
	}

	private void LoadVolumes()
	{
		var cfg = new ConfigFile();
		var err = cfg.Load(SavePath);
		if (err != Error.Ok) return;

		_sfxDb = Mathf.Clamp((float)cfg.GetValue(Section, "sfxDb", _sfxDb), MinDb, MaxDb);
		_bgmDb = Mathf.Clamp((float)cfg.GetValue(Section, "bgmDb", _bgmDb), MinDb, MaxDb);
	}

	private void SaveVolumes()
	{
		var cfg = new ConfigFile();
		cfg.Load(SavePath);
		cfg.SetValue(Section, "sfxDb", _sfxDb);
		cfg.SetValue(Section, "bgmDb", _bgmDb);
		cfg.Save(SavePath);
	}

	private void ApplySfxBus(float db)
	{
		if (_sfxBus < 0) return;
		db = Mathf.Clamp(db, MinDb, MaxDb);
		bool mute = db <= (MinDb + MuteEps);
		AudioServer.SetBusVolumeDb(_sfxBus, db);
		AudioServer.SetBusMute(_sfxBus, mute);
	}

	private void ApplyBgmBus(float db)
	{
		if (_bgmBus < 0) return;
		db = Mathf.Clamp(db, MinDb, MaxDb);
		bool mute = db <= (MinDb + MuteEps);
		AudioServer.SetBusVolumeDb(_bgmBus, db);
		AudioServer.SetBusMute(_bgmBus, mute);
	}

	public void SetSfxDb(float db)
	{
		_sfxDb = Mathf.Clamp(db, MinDb, MaxDb);
		ApplySfxBus(_sfxDb);
		SaveVolumes();
	}

	public float GetSfxDb() => _sfxDb;

	public void SetBgmDb(float db)
	{
		_bgmDb = Mathf.Clamp(db, MinDb, MaxDb);
		ApplyBgmBus(_bgmDb);
		SaveVolumes();
	}

	public float GetBgmDb() => _bgmDb;

	public void PlayBgm()
	{
		if (_bgm == null) return;
		if (_bgm.Playing) return;   // đang phát thì thôi
		_bgm.Play();
	}

	public void PlayFlap()
	{
		if (_flap == null || _flap.Stream == null) return;
		_flap.Stop();
		_flap.Play();
	}

	public void PlayHit()
	{
		if (_hit == null || _hit.Stream == null) return;
		_hit.Stop();
		_hit.Play();
	}

	public void PlayPoint()
	{
		if (_point == null || _point.Stream == null) return;
		_point.Stop();
		_point.Play();
	}
	
	public async void TestSfx()
{
	if (_flap == null)
	{
		GD.PushError("FlapSfx is NULL (NodePath sai hoặc node không tồn tại).");
		return;
	}

	GD.Print($"Flap stream null={_flap.Stream == null} bus={_flap.Bus} volDb={_flap.VolumeDb}");
	GD.Print($"SFX bus idx={_sfxBus} mute={( _sfxBus >= 0 ? AudioServer.IsBusMute(_sfxBus) : true)} vol={( _sfxBus >= 0 ? AudioServer.GetBusVolumeDb(_sfxBus) : -999f)}");

	int master = AudioServer.GetBusIndex("Master");
	AudioServer.SetBusMute(master, false);
	AudioServer.SetBusVolumeDb(master, 0f);

	if (_sfxBus >= 0)
	{
		AudioServer.SetBusMute(_sfxBus, false);
		AudioServer.SetBusVolumeDb(_sfxBus, 0f);
	}

	_flap.VolumeDb = 0f;
	_flap.Bus = "SFX";

	await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

	_flap.Stop();
	_flap.Play();
	GD.Print($"Flap playing={_flap.Playing}");
}
}
