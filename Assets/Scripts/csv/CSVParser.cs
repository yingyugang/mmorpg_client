using UnityEngine;
using System.Collections;
using CSV;
using System.Collections.Generic;
//using System.Linq;

public class CSVParser : MonoBehaviour
{
	protected CsvContext csvContext;

	private void Awake ()
	{
		csvContext = new CsvContext ();

	}

	virtual public void Parse ()
	{
		
	}
}
