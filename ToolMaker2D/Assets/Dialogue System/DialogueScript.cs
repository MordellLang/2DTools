using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueScript : MonoBehaviour {

    public DialogueClass Dialogue;


    public void Load(string File)
    {
        Dialogue = Serializer.Load<DialogueClass>(File);

       /* Dialogue = new DialogueClass();

        MessageBox m1 = new MessageBox(1, "Ah... You're Finally awake!", 2);
        MessageBox m2 = new MessageBox(2, "You've been unconscious for a while now!", 3);

        Decision d1 = new Decision(20, "Where Am i?", 5);
        Decision d2 = new Decision(21, "Who Am i?", 6);
        Decision d3 = new Decision(22, "Who are you?", 7);
        Decision d4 = new Decision(23, "What gender Am I", 8);
        List<Decision> list1 = new List<Decision>();
        list1.Add(d1);
        list1.Add(d2);
        list1.Add(d3);
        list1.Add(d4);
        DecisionBox db1 = new DecisionBox(3, list1, "Any questions, honey?");

        MessageBox m3 = new MessageBox(5, "You are in a Hospital!", 3);
        MessageBox m4 = new MessageBox(6, "You are @name", 3);
        MessageBox m5 = new MessageBox(7, "I'm a nurse here. My name is jennifer.", 3);
        MessageBox m6 = new MessageBox(11, "UhOh. Something doofy happened!", 3);

        ConditionalBox cB = new ConditionalBox(8, "@gender == #Male", 9, 10, 11);

        MessageBox m7 = new MessageBox(9, "You are a Man", 3);
        MessageBox m8 = new MessageBox(10, "You are a Woman", -1);

        Dialogue.AddDialogBox(m1);
        Dialogue.AddDialogBox(m2);
        Dialogue.AddDialogBox(m3);
        Dialogue.AddDialogBox(m4);
        Dialogue.AddDialogBox(m5);
        Dialogue.AddDialogBox(m6);
        Dialogue.AddDialogBox(m7);
        Dialogue.AddDialogBox(m8);
        Dialogue.AddDialogBox(cB);
        Dialogue.AddDialogBox(db1);
        Dialogue.SetFirstBox(1);

        Serializer.Save<DialogueClass>(File, Dialogue);*/
    }

}
