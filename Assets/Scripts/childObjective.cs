using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class childObjective : IComparable<childObjective> { // defines a collection/list

    public objective thisChild;

    public childObjective(string title, string description, float completionLevel, string hint, objective[] grandChildObjectives)
    {
        // creates a child objective of type "objective" inside every instance of the dynamic list "childObjective"
        thisChild = new objective(title, description, completionLevel, hint, grandChildObjectives); 
    }

    public int CompareTo(childObjective other) {

        if (other == null) {
            return 0;
        }

        return 0;
    }
}
