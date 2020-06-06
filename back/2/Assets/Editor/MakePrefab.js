
@MenuItem("Project Tools/Make Prefab")

static function CreatePrefab()
{
	var sel = Selection.gameObjects;
	
	for(var go : GameObject in sel) {
		print(go.name);
	}
}