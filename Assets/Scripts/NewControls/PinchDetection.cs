using System.Collections;
using UnityEngine;

public class PinchDetection : MonoBehaviour
{
    private TouchControls m_touchControls;
    private Coroutine m_zoomCoroutine;
    private Camera m_camera;

    private const float m_orthoZoomStep = 0.5f;
    private const float m_minCamOrthoSize = 4f;
    private const float m_maxCamOrthoSize = 15f;

    private void Awake()
    {
        m_touchControls = new TouchControls();
        m_camera = Camera.main;
    }

    private void OnEnable()
    {
        m_touchControls.Enable();
    }
    private void OnDisable()
    {
        m_touchControls.Disable();
    }

    private void Start()
    {
        m_touchControls.Touch.SecondaryTouchContact.started += _ => ZoomStart();
        m_touchControls.Touch.SecondaryTouchContact.canceled += _ => ZoomEnd();
    }


    private void ZoomStart()
    {
        m_zoomCoroutine = StartCoroutine(ZoomDetection());
    }

    private void ZoomEnd()
    {
        StopCoroutine(m_zoomCoroutine);
    }

    IEnumerator ZoomDetection()
    {
        float distance, previousDistance = Vector2.Distance(
            m_touchControls.Touch.PrimaryFingerPos.ReadValue<Vector2>(),
            m_touchControls.Touch.SecondaryFingerPos.ReadValue<Vector2>());
        while (true)
        {
            distance = Vector2.Distance(
                m_touchControls.Touch.PrimaryFingerPos.ReadValue<Vector2>(),
                m_touchControls.Touch.SecondaryFingerPos.ReadValue<Vector2>());
            // Detection
            if (distance > previousDistance) // Zoom out
            {
                if (m_camera.orthographicSize <= m_maxCamOrthoSize)
                {
                    m_camera.orthographicSize += m_orthoZoomStep;
                }
            }
            else if (distance < previousDistance) // Zoom in
            {
                if (m_camera.orthographicSize >= m_minCamOrthoSize)
                {
                    m_camera.orthographicSize -= m_orthoZoomStep;
                }
            }
            previousDistance = distance;
            yield return null;
        }
    }
}
