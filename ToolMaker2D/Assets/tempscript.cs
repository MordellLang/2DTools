using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tempscript : MonoBehaviour {

    public Text MessageText;
    public List<Text> ButtonText;
    public GameObject ButtonSource1;
    public GameObject ButtonSource2;
    public GameObject ButtonSource3;
    public Scrollbar scrlbar;
    Button Button1;
    Button Button2;
    Button Button3;

    DialogueManager Manager;
    public GameObject messageSource;

    public int DecisionDelta;


    void Start()
    {
        DecisionDelta = 0;
        Button1 = ButtonSource1.GetComponent<Button>();
        Button2 = ButtonSource2.GetComponent<Button>();
        Button3 = ButtonSource3.GetComponent<Button>();
        Manager = messageSource.GetComponent<DialogueManager>();
        Button1.onClick.AddListener(delegate { SendButton(1); });
        Button2.onClick.AddListener(delegate { SendButton(2); });
        Button3.onClick.AddListener(delegate { SendButton(3); });

        PrintMessages();

    }

    void Update()
    {
        PrintMessages();
        if((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Mouse ScrollWheel") < 0 )&& Manager.CurrentType == Type.T_DecisionBox)
        {
            DecisionDelta += 1;
            DecisionDelta = Mathf.Min(DecisionDelta, Manager.DecisionList.Count - 3);
            DecisionDelta = Mathf.Max(DecisionDelta, 0);

        }
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Mouse ScrollWheel") > 0) && Manager.CurrentType == Type.T_DecisionBox)
        {
            DecisionDelta -= 1;
            DecisionDelta = Mathf.Max(DecisionDelta, 0);

        }

        float Ratio = 1.0f/Mathf.Max(Manager.DecisionList.Count - 2.0f, 1.0f);
        scrlbar.GetComponent<Scrollbar>().size = Ratio;
        scrlbar.GetComponent<Scrollbar>().value = ((float)DecisionDelta / (Manager.DecisionList.Count - 3));
    }
    void SendButton(int ButtonID)
    {
        switch(Manager.CurrentType)
        {
            case Type.T_Message: if (ButtonID == 1) { Manager.Continue(); } break;
            case Type.T_DecisionBox: if (Manager.DecisionList.Count > ButtonID + DecisionDelta - 1)
                {
                    //Debug.Log(ButtonID + DecisionDelta - 1);
                    Manager.Continue(ButtonID + DecisionDelta - 1);
                }
                break;
        }
        PrintMessages();
        DecisionDelta = 0;
    }

    void PrintMessages()
    {
        if (Manager.CurrentType == Type.T_Base)
            return;

        MessageText.text = "";
        foreach(Text t in ButtonText)
        {
            t.text = "";
        }

        switch (Manager.CurrentType)
        {
            case Type.T_Base: Manager.Continue(); PrintMessages(); break;

            case Type.T_Message:
                MessageText.text = Manager.Message;
                if(ButtonText.Count > 0)
                    ButtonText[0].text = "Continue";
                break;
            case Type.T_DecisionBox:
                MessageText.text = Manager.Message;
                EstablishButtons();
                break;
            case Type.T_End:
                MessageText.text = "END OF GAME";
                break;

        }
    }

    void EstablishButtons()
    {
        for(int i = 0;i < Mathf.Min(Manager.DecisionList.Count, ButtonText.Count); i++)
        {
            if(Manager.DecisionList.Count > i + DecisionDelta)
                ButtonText[i].text = Manager.DecisionList[i + DecisionDelta];
        }
    }


}
