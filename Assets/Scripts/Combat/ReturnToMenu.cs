using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{

    public GameObject[] objectsToActivate;
    public GameObject[] objectsToDeActivate;

    public void ReturnToMainMenu()
    {
        foreach (GameObject obj in objectsToActivate)
        {
            obj.SetActive(true);
        }
        foreach (GameObject obj in objectsToDeActivate)
        {
            obj.SetActive(false);
        }

    }
}
