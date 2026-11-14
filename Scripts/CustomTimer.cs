using System.Collections.Generic;
using Godot;

namespace Morph.Scripts;

public partial class CustomTimer : Godot.Timer
{
	// public void OnDmgTimeout()
	// {
	// 	Label currentLabel = labelArray[0];
	// 	Timer currentTimer = timerArray[0];
	// 	currentLabel.QueueFree();
	// 	currentTimer.QueueFree();
	// 	labelArray.RemoveAt(0);
	// 	timerArray.RemoveAt(0);
	// }

	private Label _label;
	
	public CustomTimer(Label label)
	{
		_label = label;
	}
	
	public override void _Ready()
	{
		this.Timeout += OnDmgTimeout;
	}
	
	public void OnDmgTimeout()
	{
		_label.QueueFree();
		this.QueueFree();
	}
}
