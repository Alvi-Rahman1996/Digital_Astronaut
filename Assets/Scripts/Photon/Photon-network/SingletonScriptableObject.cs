using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Singletons/MasterManager")]
public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    private static T _instance = null;

    public static T Instance
    {
        get
        {
            if(_instance == null)
            {
                T[] results = Resources.FindObjectsOfTypeAll<T>();
                if (results.Length ==0)
                {
                    //Debug.LogError("SingletonScriptableObject -> Instance -- Results length is 0 for type" + typeof(T).ToString() +".");
                    return null;
                }
                if(results.Length > 1)
                {
                  //Debug.LogError("SingletonScriptableObject -> Instance -- Results length is grater than 1 for type" + typeof(T).ToString() +".");
                  return null;  
                }

                _instance = results[0];
            }
            return _instance;
        }
    }
    
}
