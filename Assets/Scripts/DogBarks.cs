using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class DogBarks : MonoBehaviour
{
    [SerializeField] private EventReference barks;
    private EventInstance eventInstance;
    private const string BARKS_PARAMETER_NAME = "Bark Frequency";
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        eventInstance = RuntimeManager.CreateInstance(barks);
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        eventInstance.start();
    }

    public void SetBarks(int frequency)
    {
        if (eventInstance.isValid())
        {
            eventInstance.setParameterByName(BARKS_PARAMETER_NAME, frequency);
        }
    }
}
