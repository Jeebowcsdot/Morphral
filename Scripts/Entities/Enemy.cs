using System.Collections.Generic;
using Godot;

namespace Morph.Scripts;

public partial class Enemy : RigidBody2D
{
	// (Child-)Nodes
	protected Label DmgLabel;
	protected ProgressBar HealthBar;
	protected Label HpLabel;
	
	public List<Label> LabelArray = new List<Label>();
	public List<Timer> TimerArray = new List<Timer>();

	// In-Game Stats
	public float MaxHp { get; protected set; } = 100.0f;
	public float Hp { get; protected set; } = 100.0f;
	public float Defense { get; protected set; } = 10.0f;
	public float Attack { get; protected set; } = 10.0f;
	
	private Vector2 _labelPosition;
	private RandomNumberGenerator _rng;
	
	public override void _Ready()
	{
		// Setting Variables
		HpLabel = GetNode<Label>("HpLabel");
		DmgLabel = GetNode<Label>("DamageLabel");
		HealthBar = GetNode<ProgressBar>("HealthBar");
		_rng = new RandomNumberGenerator();
		
		// RigidBody2D Modifier
		GravityScale = 0.0f;
		
		// Get Label Info
		_labelPosition = DmgLabel.Position;
		DmgLabel.CallDeferred("queue_free");
		
		// Health Bar setup
		HealthBar.MaxValue = MaxHp;
		HealthBar.Value = MaxHp;
		HpLabel.Text = Hp + " / " + MaxHp;

	}

	public void takeDamage(int dmg, Vector2 labelPos, bool isCrit = false)
	{
		Hp -= dmg;
		HealthBar.Value -= dmg;
		HpLabel.Text = Hp + " / " + MaxHp;
		Label label = new Label();
		label.AddThemeFontSizeOverride("font_size", 64);
		label.AddThemeFontOverride("font", GD.Load<Font>("res://Fonts/sigmarcyrillic.otf"));
		label.GlobalPosition = labelPos;
		label.Text = $"{dmg}";
		label.AddThemeConstantOverride("outline_size", 15);
		label.AddThemeColorOverride("font_outline_color", Colors.Black);
		if (isCrit)
		{
			label.AddThemeColorOverride("font_color", Colors.DarkOrange);
			label.AddThemeColorOverride("font_outline_color", new Color(225, 0, 0));
		}
		GetParent<Node2D>().AddChild(label);
		LabelArray.Add(label);
		CustomTimer timer = new CustomTimer(label);
		GetParent<Node2D>().AddChild(timer);
		timer.WaitTime = 1.0f;
		TimerArray.Add(timer);
		timer.Start();

		if (Hp <= 0)
		{
			this.CallDeferred("queue_free");
		} 
	}
	

}
