// SegBar.cs
using Godot;

public partial class SegBar : Control
{
	[Export] public int Segments = 20;
	[Export] public float MinDb = -80f;
	[Export] public float MaxDb = 0f;
	[Export] public float GapPx = 4f;

	[Export] public Color EmptyColor = new(0.15f, 0.15f, 0.15f);
	[Export] public Color FillColor = new(0.9f, 0.2f, 0.2f);

	private float _db = -40f;

	[Signal]
	public delegate void DbChangedEventHandler(float db);

	public override void _Ready()
	{
		MouseFilter = MouseFilterEnum.Stop;
	}

	public void SetDb(float db)
	{
		_db = Mathf.Clamp(db, MinDb, MaxDb);
		QueueRedraw();
	}

	public float GetDb() => _db;

	public override void _Draw()
	{
		var r = GetRect();
		float totalGap = (Segments - 1) * GapPx;
		float segW = (r.Size.X - totalGap) / Mathf.Max(1, Segments);
		float segH = r.Size.Y;

		float stepDb = (MaxDb - MinDb) / Mathf.Max(1, Segments);
		int filled = Mathf.RoundToInt((_db - MinDb) / stepDb);
		filled = Mathf.Clamp(filled, 0, Segments);

		for (int i = 0; i < Segments; i++)
		{
			float x = i * (segW + GapPx);
			var rect = new Rect2(new Vector2(x, 0), new Vector2(segW, segH));
			DrawRect(rect, i < filled ? FillColor : EmptyColor);
		}
	}

	public override void _GuiInput(InputEvent e)
	{
		if (e is InputEventMouseMotion mm)
		{
			UpdateFromX(mm.Position.X);
			AcceptEvent();
			return;
		}

		if (e is InputEventMouseButton mb && mb.ButtonIndex == MouseButton.Left)
		{
			UpdateFromX(mb.Position.X);
			AcceptEvent();
		}
	}

	private void UpdateFromX(float x)
	{
		float w = Mathf.Max(1f, Size.X);
		float ratio = Mathf.Clamp(x / w, 0f, 1f);
		float rawDb = Mathf.Lerp(MinDb, MaxDb, ratio);
		float snapped = SnapDb(rawDb);

		if (!Mathf.IsEqualApprox(snapped, _db))
		{
			SetDb(snapped);
			EmitSignal(SignalName.DbChanged, snapped);
		}
	}

	private float SnapDb(float db)
	{
		float step = (MaxDb - MinDb) / Mathf.Max(1, Segments);
		int idx = Mathf.RoundToInt((db - MinDb) / step);
		idx = Mathf.Clamp(idx, 0, Segments);
		return MinDb + idx * step;
	}
}
