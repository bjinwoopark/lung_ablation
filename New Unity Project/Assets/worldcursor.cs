using UnityEngine;
using Academy.HoloToolkit.Unity;

public class worldcursor : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = this.gameObject.GetComponentInChildren<MeshRenderer>();
    }

    void Update()
    {

        if (GazeManager.Instance == null)
        {
            return;
        }

        if (GazeManager.Instance.Hit)
        {
            // myedits
            meshRenderer.enabled = false;
        }
        else
        {
            //myedits
            meshRenderer.enabled = true;
        }

        // Assign gameObject's transform position equals GazeManager's instance Position.
        this.transform.position = GazeManager.Instance.Position;

        // Assign gameObject's transform up vector equals GazeManager's instance Normal.
        this.transform.forward  = GazeManager.Instance.Normal;
    }
}