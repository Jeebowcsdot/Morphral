using System.Collections.Generic;
using Godot;

namespace Morph.Scripts.UI;

// [Tool]
public partial class RadialUI : Control
{
	[Export] public Color BgColor { get; set; }
	[Export] public Color LineColor { get; set; }
	
	[Export] public int Radius { get; set; }
	[Export] public int Width { get; set; }	
	
	[Export] public int InnerRadius { get; set; }
	[Export] public int InnerWidth { get; set; }
	
	public List<Attacks> Options { get; private set; } = new List<Attacks>();

	public RadialUI(List<Attacks> A)
	{
		AttackUpdate(A);
	}
	
	private void AttackUpdate(List<Attacks> attacks)
	{
		Options.Clear();
		foreach (Attacks a in attacks)
		{
			Options.Add(a);
		}
	}
	
	public override void _Draw()
	{
		DrawCircle(Vector2.Zero, Radius, BgColor);
		DrawArc(Vector2.Zero, Radius, 0.0f, 2 * Mathf.Pi, 128, LineColor, Width, true);
		DrawCircle(Vector2.Zero, InnerRadius, BgColor);
		DrawArc(Vector2.Zero, InnerRadius, 0.0f, 2 * Mathf.Pi, 128, LineColor, InnerWidth, true);

		if (Options.Count >= 3)
		{
			for (int i = 0; i <= Options.Count -1; i++)
			{
				float rads = 2 * Mathf.Pi * i / (Options.Count - 1);
				Vector2 P = Vector2.FromAngle(rads);
				DrawLine(P*InnerRadius, P*Radius, LineColor, InnerWidth, true);
			}
		}
	}

	public override void _Process(double delta)
	{
		QueueRedraw();
	}
}
