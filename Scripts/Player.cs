using Godot;
using System;

/// <summary>
/// the general player class with everything included that is needed for every player.
/// </summary>

public partial class Player : CharacterBody2D
{
	// Movement and Velocity
	public Vector2 Direction { get; protected set; } = Vector2.Zero;
	public  float Speed { get; protected set; } = 650.0f;

	// Statistics and Combat
	public float BaseAttack { get; protected set; } = 20.0f;
	public  float KickDamage { get; protected set; } = 20.0f;
	public float Defense { get; protected set; } = 25.0f;
	
	// (Child) Nodes
	public Area2D DamageArea { get; protected set; }
	public CollisionShape2D DamageCollision { get; protected set; }
	public Node2D AttackPoint { get; protected set; }
	
	// Others
	private RandomNumberGenerator _rng;

	public override void _Ready()
	{
		DamageArea = GetNode<Area2D>("DmgArea");
		DamageCollision = GetNode<CollisionShape2D>("DmgArea/DmgCollision");
		AttackPoint = GetNode<Node2D>("DmgArea/AttackPoint");
		DamageArea.BodyEntered += OnDmgBodyEntered;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Direction = Input.GetVector("MoveLeft", "MoveRight", "MoveUp", "MoveDown");
		Velocity = Direction * Speed;
		MoveAndSlide();
	}

	protected void OnDmgBodyEntered(Node2D body)
	{
		if (body.IsInGroup("Enemy"))
		{
			Enemy enemy = GetNode<Enemy>(body.GetPath());
			var (damage, isCrit) = DamageCalc(enemy.Defense, false);
			enemy.takeDamage(damage, AttackPoint.GlobalPosition, isCrit);
		}
	}

	protected (int Damage, bool isCrit) DamageCalc(float defense, bool invincible = false)
	{
		if (invincible)
		{
			return (0, false);
		}
		bool isCrit = false;
		_rng = new RandomNumberGenerator();
		float totalDamage = KickDamage * BaseAttack / (KickDamage + Defense);
		float offset = _rng.RandfRange((-1) * totalDamage * 0.1f, totalDamage * 0.1f);
		totalDamage += offset;

		int criticalHit = _rng.RandiRange(0, 19); // 5% Chance
		if (criticalHit == 1)
		{
			totalDamage *= 1.5f;
			isCrit = true;
		}

		return (Mathf.RoundToInt(totalDamage), isCrit);
	}
}
