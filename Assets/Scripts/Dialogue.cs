using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Creating a class to reference a Dialogue object
[System.Serializable]
public class Dialogue
{
    //The speaker
    public string talkingName;

    //What the speaker says
    [TextArea(3, 10)]
    public string[] sentences;
}
