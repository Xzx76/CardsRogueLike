using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragNoTarget : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool dragging = false;
    private bool selectMode = true;
    private PreView preView;
    private void Awake()
    {
        if (GetComponent<PreView>() != null)
        {
            preView = GetComponent<PreView>();
        }
        else
        {
            Debug.Log("没找到Preview组件");
        }
    }

    //拖拽模式
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (!dragging)
            {
                Debug.Log("按住鼠标左键");
                dragging = true;
                selectMode = false;
                //开始拖拽状态的预览
                preView.DragPreview();
            }
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        foreach (var rayResult in raycastResults)
        {
            if (rayResult.gameObject.tag == "Enemy")
            {
                preView.TurnArrowColor(1);
                return;
            }
            else if (rayResult.gameObject.tag == "Player")
            {
                preView.TurnArrowColor(2);
                return;
            }
        }
        preView.TurnArrowColor(0);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("松开鼠标左键");
        selectMode = true;
        EndThisDrag();
    }

    //选取模式
    public void OnPointerClick(PointerEventData eventData)
    {
        //点击鼠标左键
        if (selectMode && eventData.button == PointerEventData.InputButton.Left)
        {
            if (!dragging)
            {
                Debug.Log("选取");
                dragging = true;
                //开始拖拽状态的预览
                preView.DragPreview();
            }
        }
    }

/*    //拖拽中每帧更新位置
    private void Update()
    {
        if (dragging)
        {
            Vector3 mousePos = Input.mousePosition;
            transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
        }
    }*/

    //取消拖拽，返回原来状态
    private void EndThisDrag()
    {
        Debug.Log("取消拖拽");
        dragging = false;
        preView.EndDrag();
    }
}