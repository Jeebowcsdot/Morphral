using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public partial class Enemy : RigidBody2D
{
	private Label _dmgLabel;
	private List<Label> _labelArray = new List<Label>();
	private List<Timer> _timerArray = new List<Timer>();
	// private Timer _dmgTimer;
	private RandomNumberGenerator _rng;

	// In-Game Stats
	public float Hp { get; private set; } = 100.0f;
	public float Defense { get; private set; } = 10.0f;
	public float Attack { get; private set; } = 10.0f;
	
	private Vector2 _labelPosition;
	
	public override void _Ready()
	{
		// Setting Variables
		_dmgLabel = GetNode<Label>("DamageLabel");
		_rng = new RandomNumberGenerator();
		
		// RigidBody2D Modifier
		GravityScale = 0.0f;
		
		// Get Label Info
		_labelPosition = _dmgLabel.Position;
		_dmgLabel.CallDeferred("queue_free");
	}

	public void takeDamage(int dmg, Vector2 labelPos, bool isCrit = false)
	{
		Hp -= dmg; 
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
		_labelArray.Add(label);
		Timer timer = new Timer();
		this.AddChild(timer);
		timer.WaitTime = 1.0f;
		timer.Timeout += OnDmgTimeout;
		_timerArray.Add(timer);
		timer.Start();
	}
	
	private void OnDmgTimeout()
	{
		Label currentLabel = _labelArray[0];
		Timer currentTimer = _timerArray[0];
		currentLabel.QueueFree();
		currentTimer.QueueFree();
		_labelArray.RemoveAt(0);
		_timerArray.RemoveAt(0);
	}
}
