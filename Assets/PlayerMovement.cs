using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float baseJumpForce = 5f;          // แรงกระโดดเริ่มต้น
    public float jumpForceIncrement = 2f;     // แรงที่เพิ่มขึ้นต่อคอมโบ
    public int maxCombo = 5;                  // จำนวนคอมโบสูงสุด
    public float comboResetTime = 0.5f;       // เวลาที่คอมโบจะรีเซ็ตถ้าไม่กดต่อ

    private Rigidbody rb;
    private int currentCombo = 0;
    private float lastJumpTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            float currentTime = Time.time;

            // ถ้ากดต่อเนื่องในช่วงเวลา comboResetTime → เพิ่มคอมโบ
            if (currentTime - lastJumpTime <= comboResetTime)
            {
                currentCombo++;
            }
            else
            {
                currentCombo = 1; // เริ่มคอมโบใหม่
            }

            // จำกัดคอมโบไม่ให้เกิน max
            currentCombo = Mathf.Clamp(currentCombo, 1, maxCombo);

            // คำนวณแรงกระโดดตามคอมโบ
            float jumpForce = baseJumpForce + (jumpForceIncrement * (currentCombo - 1));

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            lastJumpTime = currentTime; // บันทึกเวลาล่าสุด
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}
