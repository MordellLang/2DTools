using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageButtons : MonoBehaviour {


    public GameObject DialogueSource;
    public Text Message;
    public Text Button1;
    public Text Button2;
    public Text Button3;
    Button BT1;
    Button BT2;
    Button BT3;
    public  GameObject B1;
    public GameObject B2;
    public GameObject B3;
    DialogueManager Manager;


    void Start () {
        Manager = DialogueSource.GetComponent<DialogueManager>();
        BT1 = B1.GetComponent<Button>();
        BT2 = B2.GetComponent<Button>();
        BT3 = B3.GetComponent<Button>();
        BT1.onClick.AddListener(Send1);
        BT2.onClick.AddListener(Send2);
        BT3.onClick.AddListener(Send3);
    }
	
    void Update()
    {

        if(Manager.CurrentType == Type.T_Message || Manager.CurrentType == Type.T_Base)
        {
            Message.text = Manager.Message;
            Button1.text = "Continue";
            Button2.text = "";
            Button3.text = "";
        }
        if(Manager.CurrentType == Type.T_DecisionBox)
        {
            Message.text = Manager.Message;
            List<Decision> DecisionList = Manager.RequestDecisions();
            if(DecisionList.Count > 0)
                Button1.text = DecisionList[0].Message;
            if (DecisionList.Count > 1)
                Button2.text = DecisionList[1].Message;
            if (DecisionList.Count > 2)
                Button3.text = DecisionList[2].Message;
        }
        if(Manager.CurrentType == Type.T_End)
        {
            Message.text = "END of GAME";
        }
    }
	// Update is called once per frame
    void Send1()
    {
        if(Manager.CurrentType == Type.T_Message || Manager.CurrentType == Type.T_Base)
        {
            Manager.Continue();
        }
        else
        {
            Manager.Continue(0);
        }
    }
    void Send2()
    {
        if (Manager.CurrentType == Type.T_Message)
        {
        }
        else
        {
            Manager.Continue(1);
        }
    }
    void Send3()
    {
        if (Manager.CurrentType == Type.T_Message)
        {
        }
        else
        {
            Manager.Continue(2);
        }
    }
}
