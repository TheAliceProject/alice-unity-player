[System.Serializable]
public class ExampleClass
{
	public int level;
	public float timeElapsed = 0.123f;
	public string playerName = "default";
    public ExampleSubClass obj;

	public string ObjToString ()
	{
		return "Level: " + level + ", Time Elapsed: " + timeElapsed + ", PlayerName: " + playerName + "\n"
            + obj.ObjToString();
	}
}
