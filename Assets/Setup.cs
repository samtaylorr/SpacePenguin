using UnityEngine;

public class Setup : MonoBehaviour
{
    public GameObject persistantObjects;
    void Awake()
    {
        if(GameObject.Find(persistantObjects.name) == null)
        {
            GameObject po = Instantiate(persistantObjects);
            po.name = persistantObjects.name;
        }
    }
}
