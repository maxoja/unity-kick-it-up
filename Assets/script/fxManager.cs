using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class fxManager : MonoBehaviour 
{
    static public List<int> fCall = new List<int>();
    static public List<Vector3> fParam1 = new List<Vector3>();
    static public List<string> fParam2 = new List<string>();

    public List<floatText> poolFloatText = new List<floatText>();
    List<floatText> ActiveFloatText = new List<floatText>();

    public List<forceUpEffect> poolForceUp = new List<forceUpEffect>();
    List<forceUpEffect> ActiveForceUp = new List<forceUpEffect>();
	
	void Update () 
    {
        while (fCall.Count > 0)
        {
            switch (fCall[0])
            {
                case 1: 
                    if (poolFloatText.Count > 0)
                    {
                        poolFloatText[0].FLoat(fParam1[0], fParam2[0]);
                        poolFloatText[0].gameObject.SetActive(true);
                        ActiveFloatText.Add(poolFloatText[0]);
                        poolFloatText.RemoveAt(0);
                    } 
                    break;

                case 2:
                    if(poolForceUp.Count > 0)
                    {
                        poolForceUp[0].Force(fParam1[0]);
                        poolForceUp[0].gameObject.SetActive(true);
                        ActiveForceUp.Add(poolForceUp[0]);
                        poolForceUp.RemoveAt(0);
                    }
                    break;
            }

            fCall.RemoveAt(0);
            fParam1.RemoveAt(0);
            fParam2.RemoveAt(0);
        }

        inActiveCheck();
	}

    void inActiveCheck()
    {
        for (int ct = 0; ct < ActiveFloatText.Count; ct++)
        {
            if (ActiveFloatText[ct].isActive == false)
            {
                poolFloatText.Add(ActiveFloatText[ct]);
                ActiveFloatText[ct].gameObject.SetActive(false);
                ActiveFloatText.RemoveAt(ct);
            }
        }

        for (int ct = 0; ct < ActiveForceUp.Count; ct++)
        {
            if (ActiveForceUp[ct].isActive == false)
            {
                poolForceUp.Add(ActiveForceUp[ct]);
                ActiveForceUp[ct].gameObject.SetActive(false);
                ActiveForceUp.RemoveAt(ct);
            }
        }
    }

    static public void CallFunction(int id, Vector3 p1,string p2)
    {
        fCall.Add(id);
        fParam1.Add(p1);
        fParam2.Add(p2);
    }
}
