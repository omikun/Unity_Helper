using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    Vector3 lastMousePos;
    Vector3 lastPos;
    Vector3 lastTargetPos;
    Vector3 last_offset;
    public GameObject cameraFocusTarget;
    float totalAngle = 0, totalAnglex = 0;
    // Start is called before the first frame update
    void Start()
    {
        lastTargetPos = cameraFocusTarget.transform.position;
        last_offset = transform.position - lastTargetPos;
    }

    void rotateWorldMouse()
    {
        float angle = .5f * (Input.mousePosition.x - lastMousePos.x);
        float xangle = -.5f * (Input.mousePosition.y - lastMousePos.y);
        lastMousePos = Input.mousePosition;
        totalAngle += angle;
        if (totalAnglex + xangle > 80 || totalAnglex + xangle < -80)
            return;
        totalAnglex += xangle;

        transform.Rotate(0, angle, 0, Space.World);
        transform.Rotate(xangle, 0, 0, Space.Self);
    }
    void rotateAroundTarget()
    {
        if (!cameraFocusTarget)
        {
            return;
        }
        float angle = .5f * (Input.mousePosition.x - lastMousePos.x);
        float xangle = -.5f * (Input.mousePosition.y - lastMousePos.y);
        lastMousePos = Input.mousePosition;
        Vector3 target_pos = cameraFocusTarget.transform.position;
        float distance = Vector3.Distance(transform.position, target_pos);
        float ymove = distance * Mathf.Tan(0.01f * xangle);

        Vector3 to_target = target_pos - transform.position;
        float xmove = distance * Mathf.Tan(angle);
        Debug.Log("xmove: " + xmove + "\tangle: " + xangle + "\tdistance: " + distance.ToString());
        Vector3 xmove_dir = Vector3.zero;  //Cross(to_target, Vector3.up).normalized * xmove;
        transform.Translate(new Vector3(xmove_dir.x, ymove, xmove_dir.z));
        transform.LookAt(target_pos);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;
        }
        bool buttonEvent = Input.GetMouseButton(0);

        var target_pos = cameraFocusTarget.transform.position;
        if (cameraFocusTarget)
        {
            transform.position = target_pos + last_offset;
            lastTargetPos = target_pos;
        }
        if (buttonEvent) rotateAroundTarget();
        last_offset = transform.position - lastTargetPos;
        RenderSettings.skybox.SetFloat("_Rotation", 180.0f);
    }
}
