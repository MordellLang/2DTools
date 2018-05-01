using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum VarType {_INT, _CHAR, _STRING, _FLOAT, _BOOL, _OTHER }

[System.Serializable]
public class Element
{
    public Element(string N, VarType V, object O)
    {
        Name = N;
        _var = V;
        Object = O;
        ObjectString = Object.ToString();
    }
    public string Name;
    public VarType _var;
    public object Object;
    public string ObjectString;
}

[System.Serializable]
public class TranslatorScript : MonoBehaviour
{
    public List<Element> theDictionary;

    void Start()
    {
        theDictionary = new List<Element>();

        Element e = new Element("Lost", VarType._STRING, "Victor");
        theDictionary.Add(e);


    }
    public int AddElement(string Name, VarType _vtype, object Obj)
    {
        if(theDictionary.FindIndex(x => x.Name == Name) != -1)
        {
            Debug.Log("ERROR ADDING " + Name + " TO DICTIONARY. NAME IS ALREADY INCLUDED");
            return -1;
        }

        Element newElement = new Element(Name, _vtype, Obj);
        theDictionary.Add(newElement);
        return 0;
    }
    public Element FindElement(string Name)
    {
        return theDictionary.Find(x => x.Name == Name);
    }
    public void RemoveElement(string Name)
    {
        int Index = theDictionary.FindIndex(x => x.Name == Name);
        if(Index >= 0)
        {
            theDictionary.RemoveAt(Index);
        }
    }
}