using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class objective {

    public int serialNumber { get; private set; }
    public int lastSerialNumberOfChild;
    public string title;
    public string description;
    public string hint;
    public float completionLevel;

    public List<childObjective> childObjectives = new List<childObjective>();

    // overloads the constructors to take serialNumber as an argument and create a complete serial number so objective.serialNumber can be used to index an objective
    public objective(string titleParam, string descriptionParam, float completionLevelParam, string hintParam, objective[] childObjectives)
    {
        SetUpParameters(titleParam, descriptionParam, completionLevelParam, hintParam, childObjectives);
    }

    public objective(string titleParam, string descriptionParam, float completionLevelParam, string hintParam, objective[] childObjectives, int serialNumberOfParent, int lastSerialNumberOfChild)
    {
        SetUpParameters(titleParam, descriptionParam, completionLevelParam, hintParam, childObjectives);
        serialNumber = 10 * serialNumberOfParent + lastSerialNumberOfChild;
    }

    private void SetUpParameters(string titleParam, string descriptionParam, float completionLevelParam, string hintParam, objective[] childObjectives)
    {
        title = titleParam;
        description = descriptionParam;
        completionLevel = completionLevelParam;
        lastSerialNumberOfChild = 1;
        if (hintParam != null)
        {
            hint = hintParam; // not necessary to define a hint
        }

        if (childObjectives != null)
        { // not necessary to have any child objectives
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
            lastSerialNumberOfChild++;
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
            lastSerialNumberOfChild++;
        }
    }

    public objective GetChild(int serialNumber)
    {
        return childObjectives[serialNumber].thisChild; // returns type "objective" from every instance of childObjective in the dynamic list 
    }

    /// <summary>
    /// Pushes this objective to the main objective list.
    /// </summary>
    public void PushToMainTree() {
        // if not a main objective do not push
        // creates the serial numbers for main objectives that don't have parents
    }

    public void PrintContents()
    {
        // outputs parameters of this objective only
        Debug.Log("Serial number: " + serialNumber.ToString());
        Debug.Log("Title: " + title);
        Debug.Log("Description: " + description);
        Debug.Log("Hint: " + hint);
        Debug.Log("Completion level: " + completionLevel);
    }

    public bool IsComplete()
    {
        if (completionLevel == 1)
        {
            return true;
        }
        else
        {
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

public class mainObjectiveTree
{ 
    // contains a couple of functions to manipulate the objective tree
    //todo: implement tree search based on serial number  
}