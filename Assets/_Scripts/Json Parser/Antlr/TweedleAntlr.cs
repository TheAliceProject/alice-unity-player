using Antlr4.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweedleAntlr : MonoBehaviour
{
	public string path = "Assets/_Scripts/Json Parser/Antlr/SThing.twe";
	
	void Start () {

		string file = System.IO.File.ReadAllText(path);
		//Debug.Log(file);
		/*
		AntlrInputStream antlerStream = new AntlrInputStream(file);
		Alice.Tweedle.TweedleLexer lexer = new Alice.Tweedle.TweedleLexer(antlerStream);
		CommonTokenStream tokenStream = new CommonTokenStream(lexer);
		Alice.Tweedle.TweedleParser parser = new Alice.Tweedle.TweedleParser(tokenStream);
		Debug.Log(parser.typeType().classOrInterfaceType().GetText());
		*/

		Alice.Tweedle.Unlinked.TweedleUnlinkedParser parse = new Alice.Tweedle.Unlinked.TweedleUnlinkedParser();
		Alice.Tweedle.TweedleType type = parse.Parse(file);
		Debug.Log(type.Name);
	}
}