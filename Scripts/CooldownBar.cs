using Godot;
using System;
using System.Collections.Generic;

public partial class CooldownBar : Node2D
{
	public List<Texture2D> TextureList { get; private set; } = new List<Texture2D>();
	private const string JsonPath = "res://Json/CooldownBar.json";
	public int i = 0;
	public Sprite2D Sprite;
	public Label Label;
	private bool _flipH = false;
	private Vector2 _initialScale = Vector2.One;
	
	public CooldownBar()
	{
		LoadTextures();
	}

	public override void _Ready()
	{
		this.Scale = new Vector2(1.2f, 1.2f);
		
		Sprite = new Sprite2D();
		this.AddChild(Sprite);
		Sprite.Texture = TextureList[16];

		Label = new Label();
		this.AddChild(Label);
		Label.Position = new Vector2(-200.0f, -41.0f);
		Label.Size = new Vector2(400.0f, 76.0f);
		Label.AddThemeColorOverride("font_color", Colors.WhiteSmoke);
		Label.AddThemeColorOverride("font_outline_color", Colors.Black);
		Label.AddThemeConstantOverride("outline_size", 4);
		Label.AddThemeFontOverride("font", GD.Load<Font>("res://Fonts/sigmarcyrillic.otf"));
		Label.AddThemeFontSizeOverride("font_size", 32);
		Label.HorizontalAlignment = HorizontalAlignment.Center;
		Label.VerticalAlignment = VerticalAlignment.Center;
	}

	public void FlipBarH()
	{
		if (_flipH)
		{
			Label.Scale = _initialScale;
			Label.Rotation = 0.0f;
			Label.Position = new Vector2(-200.0f, -41.0f);
			_flipH = false;
		}
		else
		{
			GD.Print("runs");
			Label.Scale = new Vector2(_initialScale.X, -Math.Abs(_initialScale.Y));
			Label.Rotation = 180.0f;
			Label.Position = new Vector2(200.0f, -41.0f);
			_flipH = true;
		}

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
