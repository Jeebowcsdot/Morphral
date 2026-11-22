using Godot;
using System;
using System.Collections.Generic;

public partial class CooldownBar : Node2D
{
	public List<Texture2D> TextureList { get; private set; } = new List<Texture2D>();
	private const string JsonPath = "res://Json/CooldownBar.json";
	public int i = 0;
	public Sprite2D Sprite;
	
	public CooldownBar()
	{
		LoadTextures();
	}

	public override void _Ready()
	{
		Sprite = new Sprite2D();
		this.AddChild(Sprite);
		Sprite.Texture = TextureList[16];
	}
	
	private void LoadTextures()
	{
		if (!FileAccess.FileExists(JsonPath))
		{
			GD.PushError("Json not found.");
			return;
		}

		FileAccess file = FileAccess.Open(JsonPath, FileAccess.ModeFlags.Read);
		Variant data = Json.ParseString(file.GetAsText());
		Godot.Collections.Dictionary
			dataDict = data.AsGodotDictionary();
		dataDict.TryGetValue("Default", out Variant dataDefault);
		Godot.Collections.Array
			dataArray = dataDefault.AsGodotArray();

		foreach (Variant d in dataArray)
		{
			Godot.Collections.Dictionary
				dict = d.AsGodotDictionary();
			string path = dict["path"].AsString();
			TextureList.Add(GD.Load<Texture2D>(path));
		}
	}
}
