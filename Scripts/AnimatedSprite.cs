using Godot;

namespace Morph.Scripts;

public partial class AnimatedSprite : AnimatedSprite2D
{
	// private Player _player;
	//
	// public override void _Ready()
	// {
	// 	_player = GetParent<Player>();
	// }
	//
	// private bool lookedLeft = false;
	// public override void _PhysicsProcess(double delta)
	// {
	// 	if (_player.Direction.X > 0)
	// 	{
	// 		if (lookedLeft)
	// 		{ 
	// 			this.FlipH = false;
	// 			lookedLeft = false;
	// 		}
	// 	}
	//
	// 	if (_player.Direction.X < 0)
	// 	{
	// 		if (!lookedLeft)
	// 		{
	// 			this.FlipH = true;
	// 			lookedLeft = true;
	// 		}
	// 	}
	// }
}
