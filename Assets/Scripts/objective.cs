using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objective {

    public int serialNumber;
    public string title;
    public string description;
    public string hint;
    public float completionLevel;

    List<childObjective> childObjectives = new List<childObjective>();

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

    private void MakeChild(objective child)
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

    private objective GetChild(int serialNumber)
    {
        return childObjectives[serialNumber].thisChild; // returns type "objective" from every instance of childObjective in the dynamic list 
    }

    public bool IsComplete() {

        if (completionLevel == 100)
        {
            return true;
        }
        else {
            return false;
        }
    }
}
