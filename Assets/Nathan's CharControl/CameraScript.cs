using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    GameObject m_player;
    Camera m_POV;
    float xRot;

    Vector2 m_mousePrior;
    Vector2 m_mouseDelta;


    void Start()
    {
        m_mousePrior = Input.mousePosition;

        m_player = GameObject.FindGameObjectWithTag("Player");
        m_POV = Camera.main;
        xRot = 0f;

        m_POV.transform.Translate(Vector3.up);
        m_POV.transform.SetParent(m_player.transform);

        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        Vector2 cache = Input.mousePosition;
        m_mouseDelta = cache - m_mousePrior;
        m_mousePrior = cache;

        if (Input.GetAxis("Fire1") != 0f)
        {
            m_POV.fieldOfView = 60f;
        }

        HandleCameraRotation();
        HandleCameraPosition();
        HandleScrollWheel();
    }

    void HandleCameraRotation()
    {
        xRot += 50f * m_mouseDelta.y * Time.deltaTime;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        float yRot = 50f * m_mouseDelta.x * Time.deltaTime;

        m_player.transform.Rotate(yRot * Vector3.up);
        m_POV.transform.localEulerAngles = xRot * Vector3.left;
    }

    void HandleCameraPosition()
    {
        m_POV.transform.position = m_player.transform.position;
    }

    void HandleScrollWheel()
    {
        m_POV.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * 20f;
    }
}
