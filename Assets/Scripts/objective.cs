using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class objective {

    public int serialNumber { get; private set; }
    private int lastIndex;
    public string title;
    public string description;
    public string hint;
    public float completionLevel;

    public List<objective> childObjectives = new List<objective>();
    private additionalMethods internalLib = new additionalMethods();

    // overloads the constructors to take serialNumber as an argument and create a complete serial number so objective.serialNumber can be used to index an objective
    public objective(string titleParam, string descriptionParam, float completionLevelParam, string hintParam, objective[] childObjectives)
    {
        SetUpParameters(titleParam, descriptionParam, completionLevelParam, hintParam, childObjectives);
        serialNumber = 1;
        // all main objectives are initialised with a serial number of 1 until pushed to the main tree, after which they are assigned a serial number based on the
        // order in which they are pushed
    }

    public objective(string titleParam, string descriptionParam, float completionLevelParam, string hintParam, objective[] childObjectives, int serialNumberOfParent, int lastIndex)
    {
        SetUpParameters(titleParam, descriptionParam, completionLevelParam, hintParam, childObjectives);
        serialNumber = 10 * serialNumberOfParent + lastIndex;
    }

    private void SetUpParameters(string titleParam, string descriptionParam, float completionLevelParam, string hintParam, objective[] childObjectives)
    {
        title = titleParam;
        description = descriptionParam;
        completionLevel = completionLevelParam;
        lastIndex = 1;
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
    public objective MakeChild(objective child)
    {
        int numberOfGrandChildren = child.childObjectives.Count; // a child objective may have grandchildren
        if (numberOfGrandChildren == 0) // if no grandchildren pass null as childObjectives of child
        {
            childObjectives.Add(new objective(child.title, child.description, child.completionLevel, child.hint, null, serialNumber, lastIndex));
            lastIndex++;
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
            childObjectives.Add(new objective(child.title, child.description, child.completionLevel, child.hint, grandChildObjectives, serialNumber, lastIndex));
            lastIndex++;
        }
        return GetChild(lastIndex - 2); // returns the child created so functions can be chained
    }

    public objective GetChild(int index)
    {
        return childObjectives[index]; // returns type "objective" from every instance of childObjective in the dynamic list 
    }

    public objective GetChildWithSerial(int serialNumber)
    {
        string serialNumberString = serialNumber.ToString();
        objective currentObjectiveGotten = GetChild(getIndex(serialNumberString, 1));

        for (int i = 2; i < serialNumberString.Length; i++) {
            currentObjectiveGotten = currentObjectiveGotten.GetChild(getIndex(serialNumberString, i));
        }
        return currentObjectiveGotten;
    }

    /// <summary>
    /// Adds this objective to the main objective list specified.
    /// </summary>
    public void AddToMainList(mainObjectivesList mainList) {
        // if not a main objective do not push
        // creates the serial numbers for main objectives
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

    private int getIndex(string serialNumber, int index)
    {
        return internalLib.charToInt(serialNumber[index]) - 1; // returns 0-centered index
    }
}

public class mainObjectivesList
{
    // contains a couple of functions to manipulate the objective list
    //todo: implement tree search based on serial number
    private List<objective> objectiveList = new List<objective>();
    private additionalMethods internalLib = new additionalMethods();

    public List<objective> GetList()
    {
        return objectiveList;
    }

    public void AddTree(objective mainObjective)
    {
        objectiveList.Add(mainObjective);
    }

    public void InsertTree(int index, objective mainObjective)
    {
        objectiveList.Insert(index, mainObjective);
    }

    public void RemoveTree(int index)
    {
        objectiveList.RemoveAt(index);
    }

    public void Delete(int serialNumber)
    {
        string serialNumberString = serialNumber.ToString();
        int serialNumberOfParent = int.Parse(serialNumberString.Substring(0, serialNumberString.Length - 1));
        int indexOfChild = int.Parse(serialNumberString.Substring(serialNumberString.Length - 1, 1)) - 1;
        GetObjectiveWithSerial(serialNumberOfParent).childObjectives.RemoveAt(indexOfChild);
    }

    public objective GetObjectiveWithSerial(int serialNumber)
    {
        string serialNumberString = serialNumber.ToString();
        objective objectiveWithSerial = objectiveList[internalLib.charToInt(serialNumberString[0]) - 1];

        if (serialNumberString.Length > 1) {
            objectiveWithSerial = objectiveWithSerial.GetChildWithSerial(serialNumber);
        }
        return objectiveWithSerial;
    }
}