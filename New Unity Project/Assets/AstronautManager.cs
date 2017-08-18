using Academy.HoloToolkit.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class AstronautManager : Singleton<AstronautManager>
{
    // KeywordRecognizer object.
    KeywordRecognizer keywordRecognizer;

    // Defines which function to call when a keyword is recognized.
    delegate void KeywordAction(PhraseRecognizedEventArgs args);
    Dictionary<string, KeywordAction> keywordCollection;

    private Renderer[] meshes;

    void Start()
    {
        keywordCollection = new Dictionary<string, KeywordAction>();

        // Add keyword to start manipulation.
        keywordCollection.Add("Move Hologram", MoveAstronautCommand);
        keywordCollection.Add("Move Mike Sheng", MoveAstronautCommand);
        keywordCollection.Add("On Hologram", OnAstronautCommand);
        keywordCollection.Add("On Mike Sheng", OnAstronautCommand);
        keywordCollection.Add("Off Hologram", OffAstronautCommand);
        keywordCollection.Add("Off Mike Sheng", OffAstronautCommand);

        // Initialize KeywordRecognizer with the previously added keywords.
        keywordRecognizer = new KeywordRecognizer(keywordCollection.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();

        meshes = FindObjectsOfType<Renderer>();
        //meshes = allObjects.GetComponentInChildren<Renderer>()
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        KeywordAction keywordAction;

        if (keywordCollection.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke(args);
        }
    }

    private void MoveAstronautCommand(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Moving object...");
        GestureManager.Instance.Transition(GestureManager.Instance.ManipulationRecognizer);
    }

    private void OnAstronautCommand(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Show all objects");
        foreach (Renderer go in meshes)
        {
            go.enabled = true;
        }
    }

    private void OffAstronautCommand(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Hide all objects");
        //gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
        foreach (Renderer go in meshes)
        {
            go.enabled = false;
            //go.GetComponentInChildren<Renderer>().enabled = false;
            /*go.SetActive(false);
            if (go.activeInHierarchy)
            {
                go.GetComponentInChildren<Renderer>().enabled = false;
            }*/
        }
    }
}