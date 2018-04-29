using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class internalLib {

    public int charToInt(char character)
    {
        return (int)char.GetNumericValue(character);
    }
}
