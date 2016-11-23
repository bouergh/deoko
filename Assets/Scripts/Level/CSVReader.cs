using UnityEngine;
using System.Collections;
using System.Linq; 

public class CSVReader {
	// splits a CSV file into an array of array of int
	static public string[][] SplitCsvGrid(string csvText)
	{
		string[] lines = csvText.Split('\n');

		string[][] outputGrid = new string[lines.Length][]; 
		for (int i = 0; i < lines.Length; i++)
		{
			string[] row = SplitCsvLine( lines[i] );
			outputGrid [i] = row;
		}

		return outputGrid; 
	}

	// splits a CSV row 
	static public string[] SplitCsvLine(string line)
	{
		return (line.Trim().Split (';'));
	}
}