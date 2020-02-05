[System.Serializable]
public class ExampleSubClass {

    public int[] values = new int[]{ 1, 2, 3 };
    public System.Collections.Generic.List<char> characters = new System.Collections.Generic.List<char>{ 'a', 'b', 'c' };

	public string ObjToString()
    {
        string str = "";
        str += "Array: ";
        for (int i = 0; i < values.Length; i++)
        {
            str += values[i];
            if (i < values.Length - 1)
            {
                str += " - ";
            }
        }
        str += "\n";
        str += "List: ";
        for (int i = 0; i < characters.Count; i++)
        {
            str += characters[i];
            if (i < characters.Count - 1)
            {
                str += "/";
            }
        }
        return str;
    }
}
