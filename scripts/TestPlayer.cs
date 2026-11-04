using Godot;
using System;

public partial class TestPlayer : CharacterBody2D
{

	private Vector2 _direction = Vector2.Zero;
	private float _speed = 650.0f;
	
	public override void _Ready()
	{
		
	}

	public override void _PhysicsProcess(double delta)
	{
		_direction = Input.GetVector("MoveLeft", "MoveRight", "MoveUp", "MoveDown");
		Velocity = _direction * _speed;
		MoveAndSlide();
	}
}
