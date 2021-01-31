using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraControl : MonoBehaviour
{
    public float inertia = 0.9f;

    bool down = false;

    Vector2 prevPos;
    Vector2 velocity;
    Vector3 initPos;
    Vector3 camInitPos;

    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Vector2 mouseWP = cam.ScreenToWorldPoint(Input.mousePosition);

#if UNITY_ANDROID
        
#else
        // TODO if there's time: Zoom by pinching

        //if (isOverUI())
        //{
        //    if (Input.touchCount == 1)
        //    {
        //        if (!down)
        //        {
        //            prevPos = mouseWP;
        //            initPos = Input.mousePosition;
        //            camInitPos = cam.transform.position;
        //        }
        //        Vector2 prevWorldPos = cam.transform.position;
        //        prevPos = mouseWP;
        //        cam.transform.position = camInitPos + (initPos - Input.mousePosition) * ((cam.orthographicSize * 2f) / Screen.height);

        //        velocity = (((Vector2)cam.transform.position - prevWorldPos) / Time.deltaTime) * (1f - inertia) + velocity * inertia;

        //        down = true;
        //    }
        //    else if (Input.touchCount == 2)
        //    {

        //    }
        //}


        if (Input.GetMouseButton(0) && !isOverUI() && Game.Instance.currentAction == Game.ActionType.None)
        {

            if (!down)
            {
                prevPos = mouseWP;
                initPos = Input.mousePosition;
                camInitPos = cam.transform.position;
            }
            Vector2 prevWorldPos = cam.transform.position;
            prevPos = mouseWP;
            cam.transform.position = camInitPos + (initPos - Input.mousePosition) * ((cam.orthographicSize * 2f) / Screen.height);

            velocity = (((Vector2)cam.transform.position - prevWorldPos) / Time.deltaTime) * (1f - inertia) + velocity * inertia;

            down = true;
        }
        else
        {
            down = false;
            cam.transform.position += (Vector3)velocity * Time.deltaTime;
        }

        Vector3 pos = cam.transform.position;
        pos.z = -10;
        cam.transform.position = pos;

        if (!isOverUI())
        {
            if (Input.mouseScrollDelta.y > 0f && cam.orthographicSize > 2)
            {
                cam.orthographicSize *= 0.75f;
            }
            if (Input.mouseScrollDelta.y < 0f)
            {
                cam.orthographicSize /= 0.75f;
            }
        }
#endif
    }

    private void FixedUpdate()
    {
        if (!down)
        {
            velocity *= inertia;
        }
    }

    bool isOverUI()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
            {
                return true;
            }
        }
        if (EventSystem.current.IsPointerOverGameObject())
            return true;
        return false;
    }
}
