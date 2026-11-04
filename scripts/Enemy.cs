using Godot;
using System;

public partial class Enemy : RigidBody2D
{
	private Label _dmgLabel;
	private Timer _dmgTimer;
	private RandomNumberGenerator _rng;
	
	private float _hp = 100.0f;
	private Vector2 _labelPosition;
	
	public override void _Ready()
	{
		// Setting Variables
		_dmgLabel = GetNode<Label>("DamageLabel");
		_dmgTimer = GetNode<Timer>("DamageLabelTimer");
		_rng = new RandomNumberGenerator();
		
		// Signals
		_dmgTimer.Timeout += OnDmgTimeout;
		
		// Timer set
		_dmgTimer.WaitTime = 1.5f;
		_dmgTimer.Start();
		
		// RigidBody2D Modifier
		GravityScale = 0.0f;
		
		// Get Label Info
		_labelPosition = _dmgLabel.Position;
		_dmgLabel.CallDeferred("queue_free");
	}

	public override void _Process(double delta)
	{
		
	}

	public override void _Input(InputEvent @event) // this is only here so I can test if this works.
	{
		if (Input.IsActionJustPressed("MoveLeft"))
		{
			takeDamage(67.0f);
		}
	}

	private void takeDamage(float dmg)
	{
		Label label = new Label();
		label.Position = _labelPosition;
		label.Text = $"{dmg}";
		AddChild(label);
		
		// _dmgLabel.Text = Mathf.RoundToInt(dmg).ToString();
		// Vector2 offset = new Vector2(
		// 	_rng.RandfRange(-10.0f, 10.0f), _rng.RandfRange(-10.0f, 10.0f));
		// _dmgLabel.Position += offset;
	}

	private void OnDmgTimeout()
	{
		
	}
}
