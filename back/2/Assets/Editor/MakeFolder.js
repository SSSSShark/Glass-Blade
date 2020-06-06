import System.IO;

@MenuItem("Project Tools/MakeFolders")

static function MakeFolder() 
{
	GenerateFolders();
	Debug.Log("Test");	

}

static function GenerateFolders() {
	Directory.CreateDirectory(Application.dataPath + "/DefaulFoldeer");
	Directory.CreateDirectory(Application.dataPath + "/audio");
	Directory.CreateDirectory(Application.dataPath + "/materials");
	Directory.CreateDirectory(Application.dataPath + "/images");

}