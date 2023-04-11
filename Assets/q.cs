using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class q : MonoBehaviour
{
    public Transform root; //Canvas
    public Image pic;//一张50*100image
    Image[] pics;
    public int length = 10;//数组长度
    public float width = 35f;//图片间距
    public float angl = 10;//图片角度差
    void Start()
    {
        //init();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            init();
            
        }
    }
    void init()
    {
        if (pics !=null)
        {
            for (int i = 0; i < pics.Length; i++)
            {
                Destroy(pics[i].gameObject);
            }
        }
        pics = new Image[length];
        for (int i = 0; i < length; i++)
        {
            pics[i] = Instantiate(pic) as Image;
            pics[i].transform.parent = root;
            Debug.Log(pics[i].rectTransform.position);
            pics[i].rectTransform.localPosition = new Vector3(-(width * ((float)(length - 1) / 2 - i)), 0, 0);
            pics[i].color = Color.white - (i * new Color(0, 0.1f, 0.1f, 0));
        }
        sort();
    }
    void sort()
    {
        Debug.Log(Mathf.Tan(Mathf.Deg2Rad * angl));
        for (int i = 0; i < length; i++)
        {
            pics[i].rectTransform.localPosition = new Vector3(-(width * ((float)(length - 1) / 2 - i)), -(width * ((float)(length - 1) / 2 - i)) / Mathf.Sin(Mathf.Deg2Rad * angl * ((float)(length - 1) / 2 - i)) + (1f / (Mathf.Tan(Mathf.Deg2Rad * angl * ((float)(length - 1) / 2 - i))) * (width * ((float)(length - 1) / 2 - i))), 0);
            pics[i].rectTransform.eulerAngles = new Vector3(0, 0, angl * ((float)(length - 1) / 2 - i));
        }
    }

}
