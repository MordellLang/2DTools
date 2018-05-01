using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour {


    DialogueScript Dialogue;
    public GameObject Translator;
    TranslatorScript Dictionary;
    public string File;
    public string Message;
    public List<string> DecisionList;
    public Type CurrentType;

    int CurrentAddress;
    int CurrentIndex; //Here we are assuming we have not yet optimized the code such that CurrentID = CurrentIndex. when this is done, there will be no need for searching anything


    void Start()
    {
         Dialogue = new DialogueScript();
         Dialogue.Load(File);


         Dictionary = Translator.GetComponent<TranslatorScript>();

         Message = "";

         DecisionList = new List<string>();

         CurrentAddress = 0;
         CurrentIndex = Dialogue.Dialogue.DialogueList.FindIndex(x => x.Address == CurrentAddress);
         Continue();

        Dictionary.AddElement("@name", VarType._INT, "9");
        Dictionary.AddElement("@gender", VarType._STRING, "Male");

        Debug.Log(EvaluateProposition("@gender == #Male"));
    } 
    void Print()
    {
        switch(Dialogue.Dialogue.DialogueList[CurrentIndex].DialogueType)
        {
            case Type.T_Base:
                    Message = "";
                    DecisionList = new List<string>();
                    CurrentType = Type.T_Base;
                    break;

            case Type.T_Message:
                    MessageBox mBox = Dialogue.Dialogue.DialogueList[CurrentIndex] as MessageBox;
                    DecisionList = new List<string>();
                    Message = Translate(mBox.Message);
                    CurrentType = mBox.DialogueType;
                    break;

            case Type.T_DecisionBox:
                    DecisionBox dBox = Dialogue.Dialogue.DialogueList[CurrentIndex] as DecisionBox;
                    DecisionList = new List<string>();
                    foreach (Decision d in dBox.DecisionSet) {
                        DecisionList.Add(Translate(d.Message)); 
                    }
                    Message = Translate(dBox.Message);
                    CurrentType = dBox.DialogueType;
                    break;

            case Type.T_Conditional:
                    ConditionalBox cBox = Dialogue.Dialogue.DialogueList[CurrentIndex] as ConditionalBox;
                    DecisionList = new List<string>();
                    Message = "";
                    CurrentType = cBox.DialogueType;
                    break;

            case Type.T_End:
                Message = "";
                DecisionList = new List<string>();
                CurrentType = Type.T_End;
                break;
        }
    } 
    public void Continue() //Goes to the Next Message
    {
        CurrentAddress = Dialogue.Dialogue.DialogueList[CurrentIndex].ArrowAddress;
        CurrentIndex = Dialogue.Dialogue.DialogueList.FindIndex(x => x.Address == CurrentAddress);
        Print();
    }
    public void Continue(int Decision)
    {

        DecisionBox dBox = Dialogue.Dialogue.DialogueList[CurrentIndex] as DecisionBox;
        if(dBox != null)
        {
            CurrentAddress = dBox.DecisionSet[Decision].ArrowAddress;
            CurrentIndex = Dialogue.Dialogue.DialogueList.FindIndex(x => x.Address == CurrentAddress);
            Print();
        }
    } //Continue to the Next Message considering the Decision 
    public List<Decision> RequestDecisions()
    {
        if (CurrentType != Type.T_DecisionBox)
            return null;

        DecisionBox dBox = Dialogue.Dialogue.DialogueList[CurrentIndex] as DecisionBox;
        return dBox.DecisionSet;

    } //Returns the list of Decisions
    void SolveConditional()
    {
        if (CurrentType != Type.T_Conditional)
            return;

        int PropositionValue = 0;
        string Proposition = Dialogue.Dialogue.DialogueList[CurrentIndex].Message;
        PropositionValue = EvaluateProposition(Proposition);

        ConditionalBox CBox = Dialogue.Dialogue.DialogueList[CurrentIndex] as ConditionalBox;
        if(PropositionValue == 1)
        {
            CurrentAddress = CBox.YArrowAddress;
        }
        else if(PropositionValue == 0)
        {
            CurrentAddress = CBox.NArrowAddress;
        }
        else
        {
            CurrentAddress = CBox.ArrowAddress;
        }
        CurrentIndex = Dialogue.Dialogue.DialogueList.FindIndex(x => x.Address == CurrentAddress);
        Print();

    }
    int EvaluateProposition(string Proposition)
    {
        int BooleanValue = 1;


        Proposition = Proposition.Replace(" ", string.Empty); //Removes all space characters

        string[] AndSeparation = Proposition.Split('&');
        if(AndSeparation.Length > 1) //If there ARE and operators, then we split them and use their conjugation
        {
            Debug.Log("Hit Mark 1");
            foreach(string s in AndSeparation)
            {
                int inductiveValue = EvaluateProposition(s);
                if(inductiveValue < 0)
                {
                    return inductiveValue;
                }

                BooleanValue = BooleanValue * inductiveValue; //Boolean Value is 1 if and only if each evaluateProposition is 1
            }
        }
        else
        {
            BooleanValue = 0;
            string[] OrSeparation = Proposition.Split('|');
            if(OrSeparation.Length > 1) //if there ARE or operators, then we split them and use their disjunction
            {
                Debug.Log("Hit Mark 2");
                foreach (string s in OrSeparation)
                {
                    int inductiveValue = EvaluateProposition(s);
                    if (inductiveValue < 0)
                        return inductiveValue;

                    BooleanValue = Mathf.Max(BooleanValue, inductiveValue); //Boolean value is 1 if any of the evaluataProposition is 1
                }
            }
            else //If there are NO and|or operators, we are left with atomic relations. These being ==, <=, >=, <, >, !=
            {
                BooleanValue = EvaluateAtomicProposition(Proposition);
            }
        }
        return BooleanValue;
    }
    int EvaluateAtomicProposition(string Proposition)
    {
        System.StringSplitOptions options = System.StringSplitOptions.None;
        string[] Separator = {"=="};
        string[] EqualitySeparator = Proposition.Split(Separator, options);
        if(EqualitySeparator.Length > 1) //if There are == separators.
        {
            if(EqualitySeparator.Length > 2)
            {
                Debug.Log("ERROR -1 EVALUATING ATOMIC PROPOSITION. MORE THAN ONE EQUALITY SEPARATOR IN: " + Proposition);
                return -1;
            }
            object S1 = EvaluateTerm(EqualitySeparator[0]);
            object S2 = EvaluateTerm(EqualitySeparator[1]);

            if(S1 == null || S2 == null)
            {
                Debug.Log("ERROR -2 EVALUATING TERMS " + EqualitySeparator[0] + " OR " + EqualitySeparator[1]);
                return -2;
            }

            if (S1.Equals(S2))
                return 1;

            return 0;
        }

        Separator = new string[]{ "!=" };
        EqualitySeparator = Proposition.Split(Separator, options);
        if (EqualitySeparator.Length > 1) //if There are == separators.
        {
            if (EqualitySeparator.Length > 2)
            {
                Debug.Log("ERROR -3 EVALUATING ATOMIC PROPOSITION. MORE THAN ONE INEQUALITY SEPARATOR IN: " + Proposition);
                return -3;
            }
            object S1 = EvaluateTerm(EqualitySeparator[0]);
            object S2 = EvaluateTerm(EqualitySeparator[1]);

            if (S1 == null || S2 == null)
            {
                Debug.Log("ERROR -4 EVALUATING TERMS " + EqualitySeparator[0] + " OR " + EqualitySeparator[1]);
                return -4;
            }

            if (S1.Equals(S2))
                return 0;

            return 1;
        }

        Separator = new string[] { "<=" };
        EqualitySeparator = Proposition.Split(Separator, options);
        if (EqualitySeparator.Length > 1) //if There are == separators.
        {
            if (EqualitySeparator.Length > 2)
            {
                Debug.Log("ERROR -5 EVALUATING ATOMIC PROPOSITION. MORE THAN ONE INEQUALITY SEPARATOR IN: " + Proposition);
                return -5;
            }
            object S1 = EvaluateTerm(EqualitySeparator[0]);
            object S2 = EvaluateTerm(EqualitySeparator[1]);

            if (S1 == null || S2 == null)
            {
                Debug.Log("ERROR -6 EVALUATING TERMS " + EqualitySeparator[0] + " OR " + EqualitySeparator[1]);
                return -6;
            }

            double D1 = 0;
            double D2 = 0;

            if(!double.TryParse(S1.ToString(), out D1) || !double.TryParse(S2.ToString(), out D2))
            {
                Debug.Log("ERROR -7 EVALUATING ATOMIC PROPOSITION. TERMS ARE NOT NUMBERS IN: " + EqualitySeparator[0] + " OR " + EqualitySeparator[1]);
                return -7;
            }

            if (D1 <= D2)
                return 1;
            return 0;
        }
        Separator = new string[] { ">=" };
        EqualitySeparator = Proposition.Split(Separator, options);
        if (EqualitySeparator.Length > 1) //if There are == separators.
        {
            if (EqualitySeparator.Length > 2)
            {
                Debug.Log("ERROR -8 EVALUATING ATOMIC PROPOSITION. MORE THAN ONE INEQUALITY SEPARATOR IN: " + Proposition);
                return -8;
            }
            object S1 = EvaluateTerm(EqualitySeparator[0]);
            object S2 = EvaluateTerm(EqualitySeparator[1]);

            if (S1 == null || S2 == null)
            {
                Debug.Log("ERROR -9 EVALUATING TERMS " + EqualitySeparator[0] + " OR " + EqualitySeparator[1]);
                return -9;
            }

            double D1 = 0;
            double D2 = 0;

            if (!double.TryParse(S1.ToString(), out D1) || !double.TryParse(S2.ToString(), out D2))
            {
                Debug.Log("ERROR -10 EVALUATING ATOMIC PROPOSITION. TERMS ARE NOT NUMBERS IN: " + EqualitySeparator[0] + " OR " + EqualitySeparator[1]);
                return -10;
            }

            if (D1 >= D2)
                return 1;
            return 0;
        }
        Separator = new string[] { "<<" };
        EqualitySeparator = Proposition.Split(Separator, options);
        if (EqualitySeparator.Length > 1) //if There are == separators.
        {
            if (EqualitySeparator.Length > 2)
            {
                Debug.Log("ERROR -11 EVALUATING ATOMIC PROPOSITION. MORE THAN ONE INEQUALITY SEPARATOR IN: " + Proposition);
                return -11;
            }
            object S1 = EvaluateTerm(EqualitySeparator[0]);
            object S2 = EvaluateTerm(EqualitySeparator[1]);

            if (S1 == null || S2 == null)
            {
                Debug.Log("ERROR -12 EVALUATING TERMS " + EqualitySeparator[0] + " OR " + EqualitySeparator[1]);
                return -12;
            }

            double D1 = 0;
            double D2 = 0;

            if (!double.TryParse(S1.ToString(), out D1) || !double.TryParse(S2.ToString(), out D2))
            {
                Debug.Log("ERROR -13 EVALUATING ATOMIC PROPOSITION. TERMS ARE NOT NUMBERS IN: " + EqualitySeparator[0] + " OR " + EqualitySeparator[1]);
                return -13 ;
            }

            if (D1 < D2)
                return 1;
            return 0;
        }
        Separator = new string[] { ">>" };
        EqualitySeparator = Proposition.Split(Separator, options);
        if (EqualitySeparator.Length > 1) //if There are == separators.
        {
            if (EqualitySeparator.Length > 2)
            {
                Debug.Log("ERROR -14 EVALUATING ATOMIC PROPOSITION. MORE THAN ONE INEQUALITY SEPARATOR IN: " + Proposition);
                return -14;
            }
            object S1 = EvaluateTerm(EqualitySeparator[0]);
            object S2 = EvaluateTerm(EqualitySeparator[1]);

            if (S1 == null || S2 == null)
            {
                Debug.Log("ERROR -15 EVALUATING TERMS " + EqualitySeparator[0] + " OR " + EqualitySeparator[1]);
                return -15;
            }

            double D1 = 0;
            double D2 = 0;

            if (!double.TryParse(S1.ToString(), out D1) || !double.TryParse(S2.ToString(), out D2))
            {
                Debug.Log("ERROR -16 EVALUATING ATOMIC PROPOSITION. TERMS ARE NOT NUMBERS IN: " + EqualitySeparator[0] + " OR " + EqualitySeparator[1]);
                return -16;
            }

            if (D1 > D2)
                return 1;
            return 0;
        }

        Debug.Log("ERROR -17 EVALUATING ATOMIC PROPOSITION. SEPARATOR NOT IDENTIFIED IN: " + Proposition);
        return -17;
    }
    object EvaluateTerm(string Term)
    {
        string[] SumSeparation = Term.Split('+');
        if(SumSeparation.Length > 1)
        {
            double Sum = 0;
            foreach(string s in SumSeparation)
            {
                object result = EvaluateTerm(s);
                double D = 0;
                if (!double.TryParse(result.ToString(), out D))
                {
                    Debug.Log("ERROR -18 PARSING SUM ELEMENT OF " + result.ToString());
                    return null;
                }
                Sum += D;
            }

            return Sum;
        }
        else
        {
            string[] SubSeparation = Term.Split('-');
            if (SubSeparation.Length > 1)
            {
                double Sub = 0;
                int i = 0;
                foreach (string s in SubSeparation)
                {
                    object result = EvaluateTerm(s);
                    double D = 0;
                    if (!double.TryParse(result.ToString(), out D))
                    {
                        Debug.Log("ERROR -19 PARSING SUBTRACTION ELEMENT OF " + result.ToString());
                        return null;
                    }
                    if (i == 0)
                        Sub = D;
                    else
                        Sub -= D;
                    i++;
                }

                return Sub;
            }
            else
            {
                string[] divSeparation = Term.Split('/');
                if (divSeparation.Length > 1)
                {
                    double div = 1;
                    int i = 0;
                    foreach (string s in divSeparation)
                    {
                        object result = EvaluateTerm(s);
                        double D = 0;
                        if (!double.TryParse(result.ToString(), out D))
                        {
                            Debug.Log("ERROR -19 PARSING DIVISION ELEMENT OF " + result.ToString());
                            return null;
                        }
                        if(D == 0)
                        {
                            Debug.Log("ERROR -20 DIVISION BY 0 ");
                            return null;
                        }
                        if (i == 0)
                            div = D;
                        else
                            div = div / D;
                        i++;
                    }

                    return div;
                }
                else
                {
                    string[]dotSeparation = Term.Split('*');
                    if (dotSeparation.Length > 1)
                    {
                        double Dot = 1;
                        foreach (string s in dotSeparation)
                        {
                            object result = EvaluateTerm(s);
                            double D = 0;
                            if (!double.TryParse(result.ToString(), out D))
                            {
                                Debug.Log("ERROR -21 PARSING PRODUCT ELEMENT OF " + result.ToString());
                                return null;
                            }
                            Dot = Dot * D;
                        }

                        return Dot;
                    }
                    else
                    {
                        return EvaluateAtomicTerm(Term);
                    }
                }
            }
        }

        return null;
    }
    object EvaluateAtomicTerm(string AtomicTerm)
    {
        if (AtomicTerm.StartsWith("@"))
        {
            Element e = Dictionary.FindElement(AtomicTerm);
            if (e == null)
                return null;
            else
            {
                if (e._var == VarType._INT || e._var == VarType._FLOAT)
                {
                    double R = 0;
                    if (!double.TryParse(e.Object.ToString(), out R))
                    {
                        Debug.Log("ERROR -23 OBJECT OF TYPE NUMBER IS NOT NUMBER");
                        return null;
                    }
                    return R;
                }
                else if(e._var == VarType._STRING)
                {

                    return e.Object;
                }
            }
            return e.Object.ToString();
        }
        else if (AtomicTerm.StartsWith("#"))
        {
            string[] Separate = AtomicTerm.Split('#');
            object obj = Separate[1];
            return obj.ToString();
        }

        double D = 0;
        if(!double.TryParse(AtomicTerm, out D))
        {
            Debug.Log("ERROR -22 ATOMICTERM IS NOT A NUMBER");
            return null;
        }
        return D;
    }
    string Translate(string OriginalMessage)
    {
        string TranslatedString = "";
        string[] words = OriginalMessage.Split(' ');
        foreach(string s in words)
        {
            if(s.StartsWith("@"))
            {
                Element e = Dictionary.FindElement(s);
                if (e == null)
                    break;

                string TranslatedWord = e.Object.ToString();
                TranslatedString = TranslatedString + " " + TranslatedWord;
            }
            else
            {
                TranslatedString = TranslatedString + " " + s;
            }
        }
        return TranslatedString;
    } 
    void Update()
    {
        if (CurrentType == Type.T_Conditional)
        {
            SolveConditional();
        }
    }

}


//After creating an Event Manager. i need to add this kind of AtomicTerm to the atomicProposition list. Maybe with _NAMEOFEVENT
//This would mean i could either check in Conditional Boxes: Has This Happened( Id of what i wonder happened)
//Or, ideally, later, one could do: Does this happen(Boolean Function(Parameters)) and if the function returns true, do what i want to do.