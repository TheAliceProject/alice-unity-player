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
		AntlrInputStream antlerStream = new AntlrInputStream(file);
		TweedleLexer lexer = new TweedleLexer(antlerStream);
		CommonTokenStream tokenStream = new CommonTokenStream(lexer);
		TweedleParser parser = new TweedleParser(tokenStream);

		Debug.Log(parser.SourceName);
	}
}