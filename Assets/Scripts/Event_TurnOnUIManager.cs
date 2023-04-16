using UnityEngine;

public class Event_TurnOnUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void TurnOnUIManager(){
        GameManager.Get().UIManager().gameObject.SetActive(true);
    }

    public void TurnOffUIManager(){
        GameManager.Get().UIManager().gameObject.SetActive(false);
    }
}
