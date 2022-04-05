using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private float camSpeed = 100;
    private Vector3 prevPos;
    private float angle;
    private float angleVelocity = 0;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            prevPos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 currentPos = Input.mousePosition;
            angleVelocity += (prevPos.x - currentPos.x) / camSpeed; 
        }
        else
        {
            angleVelocity /= 1.1f;

        }
        angle += angleVelocity;

        Vector3 camPos = new(6 * Mathf.Cos(angle), 8, 6 * Mathf.Sin(angle));
        transform.position = camPos;
        transform.eulerAngles = new Vector3(45, (-1 * Mathf.Rad2Deg * angle) - 90, 0);
    }



}
