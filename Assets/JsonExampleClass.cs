using System;

[Serializable]
public class JsonExampleClass
{
	public int level;
	public float timeElapsed;
	public string playerName;

	public string ToString ()
	{
		return "Level: " + level + ", Time Elapsed: " + timeElapsed + ", PlayerName: " + playerName;
	}
}
