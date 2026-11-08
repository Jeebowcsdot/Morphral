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
		// _dmgTimer = GetNode<Timer>("DamageLabelTimer");
		_rng = new RandomNumberGenerator();
		
		// Signals
		// _dmgTimer.Timeout += OnDmgTimeout;
		
		// Timer set
		// _dmgTimer.WaitTime = 1.0f;
		
		// RigidBody2D Modifier
		GravityScale = 0.0f;
		
		// Get Label Info
		_labelPosition = _dmgLabel.Position;
		_dmgLabel.CallDeferred("queue_free");
	}

	public void takeDamage(int dmg, bool isCrit = false)
	{
		Hp -= dmg; 
		Label label = new Label();
		label.AddThemeFontSizeOverride("font_size", 64);
		label.AddThemeFontOverride("font", GD.Load<Font>("res://Fonts/sigmarcyrillic.otf"));
		label.Position = new Vector2(_labelPosition.X + _rng.RandfRange(-25.0f, 25.0f), _labelPosition.Y + _rng.RandfRange(-25.0f, 25.0f));
		label.Text = $"{dmg}";
		label.AddThemeConstantOverride("outline_size", 15);
		label.AddThemeColorOverride("font_outline_color", Colors.Black);
		if (isCrit)
		{
			label.AddThemeColorOverride("font_color", Colors.DarkOrange);
			label.AddThemeColorOverride("font_outline_color", new Color(225, 0, 0));
		}
		AddChild(label);
		_labelArray.Add(label);
		// _dmgTimer.Start();
		Timer timer = new Timer();
		this.AddChild(timer);
		timer.WaitTime = 1.0f;
		timer.Timeout += OnDmgTimeout;
		_timerArray.Add(timer);
		timer.Start();
	}
	
	private void OnDmgTimeout()
	{
		GD.Print("Labels: " + _labelArray);
		GD.Print("Timers: " + _timerArray);
		Label currentLabel = _labelArray[0];
		Timer currentTimer = _timerArray[0];
		currentLabel.QueueFree();
		currentTimer.QueueFree();
		_labelArray.RemoveAt(0);
		_timerArray.RemoveAt(0);
	}
}
