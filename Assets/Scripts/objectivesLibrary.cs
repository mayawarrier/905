using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class objective : internalLib {

    private string sourceLibraryName = "objectivesLibrary";
    public int serialNumber { get; private set; }
    private int lastIndex;
    public string title;
    public string description;
    public string hint;
    public float completionLevel;

    public List<objective> childObjectives = new List<objective>();
    private ErrorHandler ErrorLogger;

    public objective()
    {
        SetValidObjective();
    }

    public objective(bool validObjective)
    {
        if (!validObjective)
        {
            serialNumber = 0;
            // this serial is invalid. This objective is returned as an error code: if an objective is searched for and not found, or when you attempt to do 
            // anything with an invalid objective. This is the functional replacement of a null object
        }
        else {
            SetValidObjective();
        }
    }

    public objective(string titleParam, string descriptionParam, float completionLevelParam, string hintParam, objective[] childObjectives)
    {
        SetUpParameters(titleParam, descriptionParam, completionLevelParam, hintParam, childObjectives);
        SetValidObjective();
        // SetValidObjective sets the serial number to 1. All main objectives are initialised with a serial number of 1 until pushed to the main tree, after which 
        // they are assigned a serial number based on the order in which they are pushed
    }

    // do not call this method outside this file
    // overloads the constructors to take serialNumber as an argument and create a complete serial number so objective.serialNumber can be used to index an objective
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

    // if MakeChild fails, it simply returns the original objective and logs an error
    public objective MakeChild(objective child)
    {
        if (serialNumber != 0 && child.serialNumber != 0)
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
        else {
            ErrorLogger.Log();
            return this;
        }
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

    public bool IsValid()
    {
        return intToBool(serialNumber);
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

    private void SetValidObjective()
    {
        serialNumber = 1; // only serial is initialised, other attributes can be intialised as required. This can be used as a root objective when none of the other
        // attributes are initialised 
    }

    private int getIndex(string serialNumber, int index)
    {
        return charToInt(serialNumber[index]) - 1; // returns 0-centered index
    }
}

public class mainObjectivesList : internalLib
{
    // contains a couple of functions to manipulate the objective list
    //todo: implement tree search based on objective attributes
    private List<objective> objectiveList = new List<objective>();
    private ErrorHandler ErrorLogger;

    public List<objective> GetList()
    {
        return objectiveList;
    }

    public objective AddTree(objective mainObjective)
    {
        // todo: create serial numbers upon adding
        objectiveList.Add(mainObjective);
        return objectiveList[objectiveList.Count - 1];
    }

    public objective InsertTree(int index, objective mainObjective)
    {
        // todo: change all the serial numbers of succeeding objectives
        objectiveList.Insert(index, mainObjective);
        return objectiveList[index];
    }

    public void RemoveTree(int index)
    {
        // todo: change the serial numbers of succeeding objectives
        objectiveList.RemoveAt(index);
    }

    public objective GetObjectiveWithAttribute<T>(attribute<T> objectiveAttribute)
    {
        //todo: include constraints for T
        return OBJECTIVE_NOT_FOUND;
    }

    public void Delete(int serialNumber)
    {
        // todo: adjust serial numbers
        string serialNumberString = serialNumber.ToString();
        int serialNumberOfParent = int.Parse(serialNumberString.Substring(0, serialNumberString.Length - 1));
        int indexOfChild = int.Parse(serialNumberString.Substring(serialNumberString.Length - 1, 1)) - 1;
        GetObjectiveWithSerial(serialNumberOfParent).childObjectives.RemoveAt(indexOfChild);
    }

    public objective GetObjectiveWithSerial(int serialNumber)
    {
        string serialNumberString = serialNumber.ToString();
        objective objectiveWithSerial = objectiveList[charToInt(serialNumberString[0]) - 1];

        if (serialNumberString.Length > 1) {
            objectiveWithSerial = objectiveWithSerial.GetChildWithSerial(serialNumber);
        }
        return objectiveWithSerial;
    }
}

public class attribute<T> : internalLib
{
    private T attr;
    public string name;
    private ErrorHandler ErrorLogger;

    public attribute(T value, string nameOfAttribute)
    {
        bool allowed = false;
        foreach (Type allowedType in allowedObjectiveAttrTypes) {
            if (value.GetType() == allowedType)
            {
                name = nameOfAttribute;
                attr = value;
                allowed = true;
                break;
            }
        }
        if (!allowed) {

        }
    }

    public Type GetTypeOfAttribute()
    {
        return attr.GetType();
    }
}