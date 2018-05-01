using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type { T_Base, T_Message, T_Decision, T_DecisionBox, T_Conditional, T_End};

[System.Serializable]
public class DialogueBox
{
    public DialogueBox(Type DType, int _Address, int _Arrow, string _Message)
    {
        DialogueType = DType;
        Address = _Address;
        ArrowAddress = _Arrow;
        Message = _Message;
    }

    public int Address; //ID of the DialogueBox
    public Type DialogueType; //Type of Dialogue
    public int ArrowAddress; //Addresses of the Dialogue Boxes this Box points to
    public string Message;
}
[System.Serializable]
public class MessageBox : DialogueBox
{
    public MessageBox(int _Address, string T_Message, int _Arrow) : base(Type.T_Message, _Address, _Arrow, T_Message)
    {
    }
}
[System.Serializable]
public class Decision : DialogueBox
{
    public Decision(int _Address, string T_Decision, int _Arrow) : base(Type.T_Decision, _Address, _Arrow, T_Decision)
    {
    }
}
[System.Serializable]
public class DecisionBox : DialogueBox
{
    public DecisionBox(int _Address, List<Decision> _DecisionSet, string T_Message) : base(Type.T_DecisionBox, _Address, -1, T_Message)
    {
        DecisionSet = _DecisionSet;
    }
    public List<Decision> DecisionSet;
}
[System.Serializable]
public class ConditionalBox : DialogueBox
{
    public int YArrowAddress;
    public int NArrowAddress;
    public ConditionalBox(int _Address, string ConditionalMessage, int YesArrow, int NoArrow, int ArrowAddress) : base(Type.T_Conditional, _Address, ArrowAddress, ConditionalMessage)
    {
        YArrowAddress = YesArrow;
        NArrowAddress = NoArrow;
    }
}

[System.Serializable]
public class DialogueClass
{
    public DialogueClass()
    {
        DialogueList = new List<DialogueBox>();
        DialogueBox Start = new DialogueBox(Type.T_Base, 0, 1, "");
        DialogueBox End = new DialogueBox(Type.T_End, -1, -1, "");
        DialogueList.Add(Start);
        DialogueList.Add(End);
    }
    public void AddDialogBox(DialogueBox DB)
    {
        DialogueList.Add(DB);
    }
    public void SetFirstBox(int ID)
    {
        foreach(DialogueBox DB in DialogueList)
        {
            if(DB.Address == 0)
            {
                DB.ArrowAddress = ID;
            }
        }
    }
    public List<DialogueBox> DialogueList;
}
