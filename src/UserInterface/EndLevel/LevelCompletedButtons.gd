extends Control


# Declare member variables here. Examples:
# var a = 2
# var b = "text"
var singleton = null;

# Called when the node enters the scene tree for the first time.
func _ready():
	if Engine.has_singleton("GodotShare"):
		singleton = Engine.get_singleton("GodotShare")
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass

func share_image(savedImage):
	singleton.sharePic(savedImage, "Shared Image", "Share Image Godot Test", "This is a test for sharing images")
	pass
