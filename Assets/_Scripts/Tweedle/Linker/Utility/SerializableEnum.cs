using System;

[Serializable]
public class SerializableEnum<T> where T : struct, IConvertible
{
	public T Value
	{
		get { return m_EnumValue; }
		set { m_EnumValue = value; }
	}

	private string m_EnumValueAsString;
	private T m_EnumValue;
}