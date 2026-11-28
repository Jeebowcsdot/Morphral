using Godot;
using System;
using Morph.Scripts;

public partial class Attacks : Node
{
	public Player.Characters Char;
	public string Name;
	public Texture2D Texture;

	public Attacks(Player.Characters c, string n, Texture2D t)
	{
		Char = c;
		Name = n;
		Texture = t;
	}
}
