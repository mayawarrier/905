using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class objective {

    public int serialNumber;
    public string title;
    public string description;
    public string hint;
    public float completionLevel;

    public List<childObjective> childObjectives = new List<childObjective>();

    public objective(string titleParam, string descriptionParam, float completionLevelParam, string hintParam, objective[] childObjectives) {

        title = titleParam;
        description = descriptionParam;
        completionLevel = completionLevelParam;
        if (hintParam != null) {
            hint = hintParam; // not necessary to define a hint
        }

        if (childObjectives != null) { // not necessary to have any child objectives
            foreach (objective child in childObjectives)
            {
                MakeChild(child);
            }
        }
    }

    public void MakeChild(objective child)
    {
        int numberOfGrandChildren = child.childObjectives.Count; // a child objective may have grandchildren
        if (numberOfGrandChildren == 0) // if no grandchildren pass null as childObjectives of child
        {
            childObjectives.Add(new childObjective(child.title, child.description, child.completionLevel, child.hint, null));
        }
        else // else get the list of child's child objectives and pass it as grandChildObjectives
        {
            objective[] grandChildObjectives = new objective[numberOfGrandChildren];
            for (int i = 0; i < numberOfGrandChildren; i++)
            {
                grandChildObjectives[i] = child.GetChild(i);
            }
            // grandChildObjectives will now be re-passed as childObjectives of the child in the constructor of "objective", which calls MakeChild again
            // and gets the children of the grandchildren recursively till it hits null
            childObjectives.Add(new childObjective(child.title, child.description, child.completionLevel, child.hint, grandChildObjectives));
        }
    }

    public objective GetChild(int serialNumber)
    {
        return childObjectives[serialNumber].thisChild; // returns type "objective" from every instance of childObjective in the dynamic list 
    }

    public void PrintContents()
    {
        Debug.Log("Serial number: " + serialNumber.ToString());
        Debug.Log("Title: " + title);
        Debug.Log("Description: " + description);
        Debug.Log("Hint: " + hint);
        Debug.Log("Completion level: " + completionLevel);
    }

    public bool IsComplete()
    {
        if (completionLevel == 100)
        {
            return true;
        }
        else {
            return false;
        }
    }
}

public class childObjective : IComparable<childObjective>
{ // defines a collection/list
    public objective thisChild;

    public childObjective(string title, string description, float completionLevel, string hint, objective[] grandChildObjectives)
    {
        // creates a child objective of type "objective" inside every instance of the dynamic list "childObjective"
        thisChild = new objective(title, description, completionLevel, hint, grandChildObjectives);
    }

    public int CompareTo(childObjective other)
    {
        if (other == null)
        {
            return 0;
        }

        return 0;
    }
}

public class serialNumberManager {

}