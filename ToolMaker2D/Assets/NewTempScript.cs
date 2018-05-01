using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTempScript : MonoBehaviour
{
}
   /* void SolveConditional()
    {
        if (CurrentType != Type.T_Conditional)
            return;

        bool PropositionValue = false;
        string Proposition = Dialogue.Dialogue.DialogueList[CurrentIndex].Message;
        PropositionValue = EvaluateProposition(Proposition);


        //CurrentAddress = Dialogue.Dialogue.DialogueList[CurrentIndex].ArrowAddress;
        //CurrentIndex = Dialogue.Dialogue.DialogueList.FindIndex(x => x.Address == CurrentAddress);
        //Print();
    }
    bool EvaluateProposition(string Proposition)
    {
        bool BooleanValue = true;
        string[] AndSeparation = Proposition.Split('&');
        if (AndSeparation.Length > 1)
        {
            Debug.Log("Hit Mark 1");
            foreach (string s in AndSeparation)
            {
                BooleanValue = BooleanValue && EvaluateProposition(s);
            }
        }
        else
        {
            BooleanValue = false;
            string[] OrSeparation = Proposition.Split('|');
            if (OrSeparation.Length > 1)
            {
                Debug.Log("Hit Mark 2");
                foreach (string s in OrSeparation)
                {
                    BooleanValue = BooleanValue || EvaluateProposition(s);
                }
            }
            else
            {
                string[] SpaceSeparation = Proposition.Split(' ');
                switch (SpaceSeparation[0])
                {
                    case "=":
                        string[] Names = SpaceSeparation[1].Split(',');
                        object Obj1 = Dictionary.FindElement(Names[0]).Object; object Obj2 = Dictionary.FindElement(Names[1]).Object;
                        BooleanValue = Obj1.Equals(Obj2);
                        Debug.Log("Hit Mark 3 - " + Obj1 + " - " + Obj2);
                        break;

                }
            }
        }
        return BooleanValue;
    }
}


      Dictionary.AddElement("@Victorrr", VarType._INT, 21);
        Dictionary.AddElement("@Lucasss", VarType._INT, 21);
        Dictionary.AddElement("@Pedro", VarType._INT, 21);
        Dictionary.AddElement("@name", VarType._STRING, "Victor");

        Debug.Log(EvaluateProposition("= @Victorrr,@Lucasss&= @Victorrr,@Pedro")); */