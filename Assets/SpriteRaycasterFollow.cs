using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRaycasterFollow : MonoBehaviour
{
    bool startFollow;
    Transform playerTrans;
    public Transform PlayerTrans
    {
        set
        {
            startFollow = true;
            playerTrans = value;
        }
    }
    void Update()
    {
        if (!startFollow)
            return;
        Vector3 targetPosition = Camera.main.WorldToScreenPoint(playerTrans.position);
        this.transform.position = targetPosition;
    }
}
