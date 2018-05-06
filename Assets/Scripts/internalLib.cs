using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class internalLib {

    public static bool NOT_FOUND = false;
    public static objective OBJECTIVE_NOT_FOUND = new objective(NOT_FOUND);
    public Type[] allowedObjectiveAttrTypes = { typeof(string), typeof(float) };

    public int charToInt(char character)
    {
        return (int)char.GetNumericValue(character);
    }

    public bool intToBool(int integer)
    {
        if (integer == 0)
        {
            return false;
        }
        else {
            return true;
        }
    }
}
