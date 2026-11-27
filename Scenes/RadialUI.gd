@tool
extends Control

@export var bgColor: Color
@export var lineColor: Color
@export var r: int = 256

func _draw() -> void:
	draw_circle(Vector2.ZERO, r, bgColor)
	draw_arc(Vector2.ZERO, r, 0, 2 * PI, 120, lineColor, true)

func _process(delta: float) -> void:
	queue_redraw()
