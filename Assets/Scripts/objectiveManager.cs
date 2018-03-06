using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectiveManager : MonoBehaviour {

	void Start () {
        objective parent = new objective("test", "This is objective 1", 0, null, null);
        objective child = new objective("test2", "This is nested objective 1", 0, null, null);
        objective child2 = new objective("test3", "This is nested objective 2", 0.5f, "fuckoff", null);

        parent.MakeChild(child);
        parent.MakeChild(child2);

        parent.PrintContents();
        parent.GetChild(0).PrintContents();
        parent.GetChild(1).PrintContents();
	}
	
}
