using GDLibrary;
using Microsoft.Xna.Framework;
using System;


public static class InsantiateFunctionExtensions
{

    //instantiates a new GameObject based on an existing GameObject at the specified position and 
    public static Object InstantiateGameObject(GameObject baseObj, Vector3 pos, Vector3 rot)
    {
        GameObject temp = (GameObject)baseObj.Clone();
        temp.Transform.SetTranslation(pos);
        temp.Transform.SetRotation(rot);
        Application.SceneManager.ActiveScene.Add(temp);
        return temp;
    }
}
