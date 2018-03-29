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
        objective currentObjectiveGotten = GetChild(serialNumberString[1]);

        for (int i = 2; i < serialNumberString.Length; i++) {
            currentObjectiveGotten.GetChild(serialNumberString[i]);
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
}

public class mainObjectivesList
{
    // contains a couple of functions to manipulate the objective list
    //todo: implement tree search based on serial number
    private List<objective> objectiveList = new List<objective>();

    public List<objective> GetList() {
        return objectiveList;
    }

    public void AddTree(objective thisObjective) {
        objectiveList.Add(thisObjective);
    }

    public mainObjectivesList Delete(int serialNumber) {

        // not possible to achieve this in Unity's version of C# --> pointers would fix this issue but can't be made to point to classes or any other reference
        // type. The only possible "clean" implementation is GetChild().GetChild().GetChild().GetChild()..... ad infinitum. But of course this is terrible and
        // non-general. However, we can go with a really really messy implementation by first finding the parent of the objective that needs to be deleted, 
        // then deleting the objective using the parent's removeAt() function. Then the grandparent is found, the old parent deleted and the new parent inserted in 
        // place. This continues with the grandparent's parent and so on until we hit a main objective and the entire tree has been re-built. The re-built tree is 
        // returned.

        string serialNumberString = serialNumber.ToString();

        for (int i = 0; i < serialNumberString.Length; i++) {
            objective parentObjective, grandParentObjective;
            int serialNumberOfParent = int.Parse(serialNumberString.Substring(0, serialNumberString.Length - 1));
            int serialNumberOfGrandparent = int.Parse(serialNumberString.Substring(0, serialNumberString.Length - 2));
            int indexOfParent = int.Parse(serialNumberString.Substring(serialNumberString.Length - 2, 1));
            int indexOfChild = int.Parse(serialNumberString.Substring(serialNumberString.Length - 1, 1));
            parentObjective = GetObjectiveWithSerial(serialNumberOfParent);
            parentObjective.childObjectives.RemoveAt(indexOfChild);
            grandParentObjective = GetObjectiveWithSerial(serialNumberOfGrandparent);
            grandParentObjective.childObjectives.RemoveAt(indexOfParent);
            grandParentObjective.childObjectives.Insert(indexOfParent, parentObjective);
        }

        return new mainObjectivesList(); // returns an empty list for now so that we have no errors in console
        // ^^^ terrible implementation looks something like this, still needs to be completed and error-checked
    }

    public objective GetObjectiveWithSerial(int serialNumber)
    {
        string serialNumberString = serialNumber.ToString();
        objective objectiveWithSerial = objectiveList[serialNumberString[0]];

        if (serialNumberString.Length > 1) {
            objectiveWithSerial.GetChildWithSerial(serialNumber);
        }
        return objectiveWithSerial;
    }
}