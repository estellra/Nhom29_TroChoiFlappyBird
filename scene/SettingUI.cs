// SettingUI.cs
using Godot;

public partial class SettingUI : CanvasLayer
{
	[Export] public NodePath AudioPath = "/root/Audio";
	[Export] public NodePath SfxBarPath = "Frame/VBox/SfxContainer/BarWrap/SegBar";
	[Export] public NodePath BgmSliderPath = "Frame/VBox/BGMContainer/BGMBar";

	private SegBar _sfxBar;
	private HSlider _bgmSlider;
	private Audio _audio;
	private bool _syncing;

	public override void _Ready()
	{
		_sfxBar = GetNodeOrNull<SegBar>(SfxBarPath);
		_bgmSlider = GetNodeOrNull<HSlider>(BgmSliderPath);
		CallDeferred(MethodName.LateBind);
		var xbutton = GetNode<TextureButton>("xbtn"); 
		xbutton.Pressed += () =>
		{
			Hide();
		};
	}

	private void LateBind()
	{
		_audio = GetNodeOrNull<Audio>(AudioPath);
		if (_audio == null)
			_audio = GetTree().GetFirstNodeInGroup("audio_manager") as Audio;

		if (_audio == null || _sfxBar == null || _bgmSlider == null) return;

		_sfxBar.MinDb = _audio.MinDb;
		_sfxBar.MaxDb = _audio.MaxDb;

		SyncFromAudio();

		_sfxBar.DbChanged += (db) => _audio.SetSfxDb(db);
		_bgmSlider.ValueChanged += (v) =>
		{
			if (_syncing) return;
			_audio.SetBgmDb(SliderToDb((float)v));
		};

		VisibilityChanged += () =>
		{
			if (Visible) SyncFromAudio();
		};
	}

	private void SyncFromAudio()
	{
		_syncing = true;
		_sfxBar.SetDb(_audio.GetSfxDb());
		_bgmSlider.Value = DbToSlider(_audio.GetBgmDb());
		_syncing = false;
	}

	private float SliderToDb(float sliderValue)
	{
		float min = (float)_bgmSlider.MinValue;
		float max = (float)_bgmSlider.MaxValue;
		float t = Mathf.InverseLerp(min, max, sliderValue);
		t = Mathf.Clamp(t, 0f, 1f);
		return Mathf.Lerp(_audio.MinDb, _audio.MaxDb, t);
	}

	private double DbToSlider(float db)
	{
		float min = (float)_bgmSlider.MinValue;
		float max = (float)_bgmSlider.MaxValue;
		float t = Mathf.InverseLerp(_audio.MinDb, _audio.MaxDb, Mathf.Clamp(db, _audio.MinDb, _audio.MaxDb));
		t = Mathf.Clamp(t, 0f, 1f);
		return Mathf.Lerp(min, max, t);
	}
}
