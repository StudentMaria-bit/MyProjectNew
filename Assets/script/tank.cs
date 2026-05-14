using UnityEngine;

public class tank : MonoBehaviour
{
    private float vertical;
    private float horizontal;
    public float speed;
    public float turnSpeed;
    public Rigidbody rb;
    public Transform turret;
    public float turretSpeed;
    private float mouseX;
    public float rotateLimit;
    private float turretAngle;
    public Transform firePoint;
    public GameObject ammo;
    public float firePower;
    public Transform pivot;
    public float cameraSense;
    private float camX,camY;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState=CursorLockMode.Locked;
        Cursor.visible=false;
    }

    // Update is called once per frame
    void Update()
    {
        // 1. УПРАВЛЕНИЕ КАМЕРОЙ (Свободный обзор)
        camX += Input.GetAxis("Mouse X") * cameraSense;
        camY -= Input.GetAxis("Mouse Y") * cameraSense;
        camY = Mathf.Clamp(camY, -20f, 40f); // Ограничение наклона вверх/вниз

        pivot.rotation = Quaternion.Euler(camY, camX, 0);

        // 2. ДВИЖЕНИЕ ТАНКА 
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        Vector3 move = transform.forward * speed * vertical;
        rb.MovePosition(transform.position + move * Time.deltaTime);
        transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontal);

        // 3. ПЛАВНОЕ НАВЕДЕНИЕ БАШНИ ЗА КАМЕРОЙ
        // Башня будет медленно поворачиваться туда, куда смотрит камера
        Quaternion targetRotation = Quaternion.LookRotation(pivot.forward);
        
        // Вращаем только по Y (горизонтально)
        Vector3 eulerRotation = Quaternion.RotateTowards(
            turret.rotation, 
            targetRotation, 
            turretSpeed * Time.deltaTime
        ).eulerAngles;
        
        turret.rotation = Quaternion.Euler(0, eulerRotation.y, 0);

        if (Input.GetButtonDown("Fire1"))
        {
            shoot();
        }
    }
    void shoot()
    {
        GameObject pulya=Instantiate(ammo,firePoint.position,firePoint.rotation);
        Rigidbody ammoRB=pulya.GetComponent<Rigidbody>();
        ammoRB.AddForce(firePoint.forward*firePower,ForceMode.Impulse);
    }
}
