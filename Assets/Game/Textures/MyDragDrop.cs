using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyDragDrop : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    public Transform parentToReturnTo = null;
    public Transform placeHolderParent = null;
    public GameObject placeHolder=null;
    public Vector3 rectPos;
    public void OnBeginDrag(PointerEventData eventData)
    {
        print("BeginDrag");
        //GameManager.Instance.HideMotorCategoryCanvas();
        //GameManager.Instance.ActivateMainBoard();
        DragManagerScene.Instance.ActivateMainBoardBlockRayCaster();

        rectPos = this.transform.position;
        placeHolder = new GameObject();
        placeHolder.transform.SetParent(this.transform.parent);

        placeHolder.AddComponent<DragDrop>();
        LayoutElement le = placeHolder.AddComponent<LayoutElement>();
        le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        le.flexibleHeight = 0;
        le.flexibleWidth = 0;
        placeHolder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
        placeHolder.AddComponent<CanvasGroup>();

        parentToReturnTo = this.transform.parent;
        placeHolderParent = parentToReturnTo;
        this.transform.SetParent(this.transform.root);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        print("Drag");
        this.transform.position = eventData.position;
        if(placeHolder.transform.parent != placeHolderParent)
        {
            placeHolder.transform.SetParent(placeHolderParent);
        }

        int newSibling = placeHolderParent.childCount;
        for (int i = 0; i < placeHolderParent.childCount; i++) {
            if (this.transform.position.y > placeHolderParent.GetChild(i).position.y)
            {
                newSibling = i;
                if (placeHolderParent.transform.GetSiblingIndex() < newSibling)
                {
                    newSibling--;
                }
                break;
            }
        }
        placeHolder.transform.SetSiblingIndex(newSibling);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        print("EndDrag");
        this.transform.SetParent(parentToReturnTo);
        this.transform.SetSiblingIndex(placeHolder.transform.GetSiblingIndex());
        this.transform.position = rectPos;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
       // GamManager.Instance.SetCurrentLevel(levl);
       // gameObject.SetActive(false);
        Destroy(placeHolder);

    }

    // Start is called before the first frame update
    void Start()
    {
        parentToReturnTo = null;
        placeHolder = null;
        placeHolderParent = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
