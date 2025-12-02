using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void Restart()
    {
        FMOD.Studio.Bus masterBus = RuntimeManager.GetBus("bus:/");
    
        // Detienes todos los eventos en ese bus inmediatamente
        masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
        SceneManager.LoadScene("End");
    }
}
