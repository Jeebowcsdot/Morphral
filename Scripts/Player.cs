using System;
using Godot;

namespace Morph.Scripts;

/// <summary>
/// the general player class with everything included that is needed for every player.
/// </summary>
public partial class Player : CharacterBody2D
{
	// Movement and Velocity
	public Vector2 Direction { get; protected set; } = Vector2.Zero;
	public float Speed { get; protected set; } = 800.0f;
	public float SprintSpeed { get; protected set; } = 1200.0f;
	public float Acceleration { get; protected set; } = 100.0f;
	public StaminaWheel Wheel { get; protected set; }

	// Statistics and Combat
	public float BaseAttack { get; protected init; } = 20.0f;
	public  float KickDamage { get; protected set; } = 20.0f;
	public float Defense { get; protected set; } = 25.0f;
	public float Stamina { get; protected set; } = 80.0f;
	public float StaminaMax { get; protected set; }
	
	// (Child) Nodes
	public Area2D DamageArea { get; protected set; }
	public CollisionShape2D DamageCollision { get; protected set; }
	public Node2D AttackPoint { get; protected set; }
	
	// Others
	public Vector2 initialScale { get; protected set; }
	private RandomNumberGenerator _rng;
	protected Timer exhaustionTimer;

	public override void _Ready()
	{
		// Setting Variables
		DamageArea = GetNode<Area2D>("DmgArea");
		DamageCollision = GetNode<CollisionShape2D>("DmgArea/DmgCollision");
		AttackPoint = GetNode<Node2D>("DmgArea/AttackPoint");
		initialScale = GetScale();
		StaminaMax = Stamina;
		
		// Signals
		DamageArea.BodyEntered += OnDmgBodyEntered;
		
		// Basic Setup on Ready
		DamageArea.Monitoring = false;
		
		// Spawn Stamina Wheel
		Wheel = new StaminaWheel(StaminaMax);
		Wheel.Position = new Vector2(-137.5f, -512.5f);
		this.AddChild(Wheel);
	}

	public override void _PhysicsProcess(double delta)
	{
		Direction = Input.GetVector("MoveLeft", "MoveRight", "MoveUp", "MoveDown");
		if (Input.IsActionPressed("Sprint") && Stamina >= 0.0f) // Sprinting
		{
			Velocity = Velocity.MoveToward(Direction * SprintSpeed, Acceleration);
			Stamina -= 20.0f * (float)delta;
			Wheel.LowerStamina(20.0f * (float)delta);
		}
		else if (Stamina >= 0.0f)
		{
			Velocity = Velocity.MoveToward(Direction * Speed, Acceleration);
		}
		else if (Stamina <= 0.0f)
		{
			Velocity = Velocity.MoveToward(Direction * Speed / 3.0f, Acceleration);
			if (exhaustionTimer == null)
			{
				exhaustionTimer = new Timer();
				exhaustionTimer.Timeout += OnStaminaExhaustionTimeout;
				this.AddChild(exhaustionTimer);
				exhaustionTimer.WaitTime = 5.0f;
				exhaustionTimer.Start();
			}
		}
		MoveAndSlide();

		if (Direction.X < 0)
		{
			Scale = new Vector2(initialScale.X, -Math.Abs(initialScale.Y));
			RotationDegrees = 180.0f;
		}
		else if (Direction.X > 0)
		{
			Scale = initialScale;
			RotationDegrees = 0.0f;
		}
		
		if (Input.IsActionJustReleased("Sprint"))
		{
			
		}
	}

	protected void OnStaminaExhaustionTimeout()
	{
		Stamina = StaminaMax;
		exhaustionTimer.QueueFree();
		exhaustionTimer = null;
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
