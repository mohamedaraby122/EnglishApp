using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DragManagerScene : MonoBehaviour
{
    private static DragManagerScene _instance;

    public static DragManagerScene Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }


    public Image[] Snappedimages;
    public int snapCounter = 0;
    public void SetCurrentLayer(Image img)
    {
        print("Change");
        Snappedimages[snapCounter++] = img;
    }
    public CanvasGroup _MianBoard;

    public void ActivateMainBoardBlockRayCaster()
    {
        _MianBoard.GetComponent<CanvasGroup>().blocksRaycasts = true;

    }
}
