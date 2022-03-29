using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraRotation : MonoBehaviour
{

    [SerializeField]
    private Transform target; //невидимая цель для камеры

    public float mouse_sens = 1f;
    public float zoomSpeed = 0.5f; //скорость приближения камеры
    public Camera cam_holder;
    float x_axis, y_axis, z_axis, _rotY, _rotX; //мышь по x, y, зум, координаты для обзора
    float distance = 200;

    // Use this for initialization

    void Start()
    {
        _rotY = transform.eulerAngles.y;
        _rotX = transform.eulerAngles.x;
    }

    void LookAtTarget()
    {
        Quaternion rotation = Quaternion.Euler(_rotY, 0, 0);
        rotation *= Quaternion.Euler(0, _rotX, 0);
        transform.rotation = rotation;
        Vector3 position = rotation * (new Vector3(0.0f, 0.0f, -distance)) + target.position;
        transform.position = position;
    }

    void LateUpdate()
    {

#if ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR)
        if (Input.touchSupported)
        {
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);
 
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition; //позиция предыдущего кадра
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
 
                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude; //длина пути между каждым тачем
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
 
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag; //длина щипка
 
                distance = Mathf.Clamp(distance + deltaMagnitudeDiff * zoomSpeed, 5, 800);
                Vector3 position = transform.rotation * (new Vector3(0.0f, 0.0f, -distance)) + target.position;
                transform.position = position;
            }
            if (Input.touchCount == 1)
            {
                Touch touchZero = Input.GetTouch(0);
 
                _rotX -= touchZero.deltaPosition.x; //поворот камеры вокруг объекта и сохранение координат
                _rotY -= touchZero.deltaPosition.y;
 
                LookAtTarget();
            }
 
        }
 
#else

        float input = Input.GetAxis("Mouse ScrollWheel");
        if (input != 0) //если крутится колесико мыши
        {
            distance = Mathf.Clamp(distance * (1 - input), 5, 800);
            Vector3 position = transform.rotation * (new Vector3(0.0f, 0.0f, -distance)) + target.position;
            transform.position = position;
        }

        if (Input.GetMouseButton(0)) //левая кнопка мыши
        {
            //вращение вокруг объекта
            _rotX += Input.GetAxis("Mouse X") * mouse_sens; //поворот камеры вокруг объекта и сохранение координат
            _rotY -= Input.GetAxis("Mouse Y") * mouse_sens;

            LookAtTarget();
        }

        if (Input.GetMouseButton(1)) //правая кнопка
        {
            x_axis = Input.GetAxis("Mouse X") * mouse_sens;
            y_axis = Input.GetAxis("Mouse Y") * mouse_sens;//смещение камеры по осям X и Y

            target.position = new Vector3(target.position.x + x_axis, target.position.y + y_axis, target.position.z);

            LookAtTarget();
        }

        if (Input.GetMouseButton(2)) //колесико
        {
            x_axis = Input.GetAxis("Mouse X") * mouse_sens;
            y_axis = Input.GetAxis("Mouse Y") * mouse_sens;
            //z_axis = Input.GetAxis("Mouse ScrollWheel") * wheel_sens;

            cam_holder.transform.Rotate(Vector3.up, x_axis, Space.World);
            cam_holder.transform.Rotate(Vector3.right, y_axis, Space.Self);
            //cam_holder.transform.localPosition = cam_holder.transform.localPosition * (1 - z_axis);
            //обзор вокруг камеры
        }

#endif
    }
}
