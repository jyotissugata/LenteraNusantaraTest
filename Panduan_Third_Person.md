# Panduan Pembuatan Third Person Controller & Kamera (Tanpa Plugin)

Dokumen ini menjelaskan konsep dan arsitektur dari Third Person Controller ala Unity Starter Assets. Tujuannya adalah agar AI atau developer di project lain dapat membuat ulang (recreate) pergerakan karakter dan kamera yang terasa serupa dari awal tanpa bergantung pada package `Starter Assets`.

---

## 1. Konsep Dasar & Arsitektur

Sistem ini memisahkan logika pergerakan karakter dan pergerakan kamera, namun pergerakan karakter akan **selalu relatif terhadap arah hadap kamera**. 

Komponen utama yang dibutuhkan:
1. **Character Controller**: Komponen bawaan Unity (bukan Rigidbody) untuk menangani pergerakan, tabrakan (collision), dan menaiki tangga/lereng.
2. **Cinemachine Virtual Camera**: Sistem kamera Unity untuk mengurus interpolasi, collision kamera dengan dinding, dan posisi *orbit*.
3. **Player Input**: Sistem untuk membaca input pergerakan (WASD/Analog) dan pandangan (Mouse Delta/Analog Kanan).

---

## 2. Setup Kamera (Cinemachine)

Rahasia dari pergerakan kamera yang mulus di Starter Assets adalah penggunaan sebuah **Camera Target (Pivot)**.

### Langkah-langkah Setup:
1. Buat **Empty GameObject** dan jadikan *child* dari karakter (Player). Beri nama `PlayerCameraRoot`.
2. Posisikan `PlayerCameraRoot` di area leher/kepala karakter.
3. Buat **Cinemachine Virtual Camera** di scene.
4. Set nilai **Follow** pada Virtual Camera ke `PlayerCameraRoot`. (Jangan set *LookAt* jika menggunakan mode 3rd Person Follow).
5. Pada Virtual Camera, gunakan body type **3rd Person Follow** atau **Transposer**. Atur jarak (Camera Distance) dan posisi pundak (Shoulder Offset).
6. **Logika Rotasi**: Script karakter *tidak* memutar kamera secara langsung, melainkan memutar `PlayerCameraRoot` pada sumbu X (Pitch/Naik-Turun) dan sumbu Y (Yaw/Kiri-Kanan) berdasarkan input mouse. Cinemachine akan secara otomatis mengikuti rotasi root ini.

---

## 3. Logika Pergerakan (Scripting)

Berikut adalah algoritma utama untuk script Player Controller:

### A. Rotasi Kamera (CameraLook)
1. Tangkap input pergerakan mouse (X dan Y).
2. Tambahkan input X ke variabel `cinemachineTargetYaw`.
3. Tambahkan input Y ke variabel `cinemachineTargetPitch`.
4. **Sangat Penting:** Batasi (Clamp) nilai Pitch agar kamera tidak berputar terbalik (misal: -30 derajat sampai 70 derajat).
5. Terapkan rotasi ini ke `PlayerCameraRoot` menggunakan `Quaternion.Euler(Pitch, Yaw, 0)`.

### B. Pergerakan Karakter Relatif Terhadap Kamera
Jika karakter mendapat input maju (W), karakter tidak bergerak pada sumbu Z dunia, melainkan maju ke arah yang sedang **dilihat oleh kamera**.

1. Hitung arah input menggunakan *Atan2*. 
2. Tambahkan hasil tersebut dengan sudut rotasi Y (Yaw) dari **Main Camera**. Ini adalah `targetRotation`.
3. Putar badan karakter ke arah `targetRotation` secara perlahan menggunakan `Mathf.SmoothDampAngle`.
4. Ubah `targetRotation` menjadi vektor arah gerak.
5. Pindahkan karakter menggunakan `CharacterController.Move(arah * kecepatan * Time.deltaTime)`.

---

## 4. Referensi Script (Template)

Berikut adalah contoh script mandiri (standalone) yang merangkum semua logika di atas. Script ini bisa diberikan kepada AI di project lain sebagai pondasi dasar.

```csharp
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimpleThirdPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5.0f;
    public float rotationSmoothTime = 0.12f;
    public float gravity = -15.0f;
    public float jumpHeight = 1.2f;

    [Header("Camera Settings")]
    public Transform cameraTargetRoot; // Assign Empty GameObject (Child dari Player)
    public float topClamp = 70.0f;
    public float bottomClamp = -30.0f;

    // References
    private CharacterController controller;
    private Transform mainCamera;

    // State Variables
    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;
    private float rotationVelocity;
    private float verticalVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main.transform;
        
        cinemachineTargetYaw = cameraTargetRoot.rotation.eulerAngles.y;
        
        // Kunci kursor mouse di tengah layar
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleCameraLook();
        HandleMovement();
        HandleGravityAndJump();
    }

    void LateUpdate()
    {
        // Update posisi rotasi camera root setelah pergerakan terjadi
        cameraTargetRoot.rotation = Quaternion.Euler(cinemachineTargetPitch, cinemachineTargetYaw, 0.0f);
    }

    private void HandleCameraLook()
    {
        // Ganti dengan Input System baru jika diperlukan
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        if (mouseX != 0 || mouseY != 0)
        {
            cinemachineTargetYaw += mouseX;
            cinemachineTargetPitch -= mouseY; 
        }

        // Clamp rotasi vertikal (Pitch) agar tidak berputar melewati kepala/kaki
        cinemachineTargetPitch = Mathf.Clamp(cinemachineTargetPitch, bottomClamp, topClamp);
        // Clamp Yaw untuk menjaga angka tetap dalam 0-360
        if (cinemachineTargetYaw < -360f) cinemachineTargetYaw += 360f;
        if (cinemachineTargetYaw > 360f) cinemachineTargetYaw -= 360f;
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 inputDirection = new Vector3(horizontal, 0, vertical).normalized;

        if (inputDirection.magnitude >= 0.1f)
        {
            // 1. Hitung sudut target berdasarkan input dan arah kamera saat ini
            float targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;

            // 2. Putar model karakter (visual) agar menghadap arah pergerakan dengan halus
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

            // 3. Konversi target rotasi menjadi vektor pergerakan
            Vector3 moveDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

            // 4. Pindahkan karakter
            controller.Move(moveDirection.normalized * (moveSpeed * Time.deltaTime));
        }
    }

    private void HandleGravityAndJump()
    {
        if (controller.isGrounded)
        {
            // Cegah akumulasi gravitasi tak berhingga saat di tanah
            if (verticalVelocity < 0.0f)
            {
                verticalVelocity = -2f;
            }

            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        // Aplikasikan gravitasi dari waktu ke waktu
        verticalVelocity += gravity * Time.deltaTime;
        
        // Pindahkan karakter secara vertikal
        controller.Move(new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
    }
}
```

## 5. Instruksi Penggunaan untuk AI / Agen di Project Baru

Jika Anda (sebagai AI) diminta untuk mengimplementasikan sistem yang serupa pada project lain, ikuti instruksi berikut:
1. Pastikan project memiliki package **Cinemachine** terinstall.
2. Buat objek (misal: Capsule 3D) sebagai Player, lalu tambahkan komponen `CharacterController`. (Jika ada `CapsuleCollider` bawaan, hapus saja karena `CharacterController` sudah memilikinya).
3. Buat script baru bernama `SimpleThirdPersonController` dengan referensi kode di atas, pasang ke objek Player.
4. Buat **Empty GameObject** di dalam objek Player, beri nama `CameraRoot`, dan atur posisinya setinggi kepala/leher karakter (misal Y = 1.5). Pasangkan `CameraRoot` ini ke field `Camera Target Root` di script `SimpleThirdPersonController`.
5. Tambahkan **Cinemachine Virtual Camera** ke dalam scene. Pada inspector-nya, atur:
   - **Follow**: tarik objek `CameraRoot` ke parameter ini.
   - **Body**: Pilih `3rd Person Follow`.
   - Atur **Camera Distance** ke jarak yang diinginkan (misal 3 atau 4).
6. Selesai. Saat dimainkan, script akan merotasi `CameraRoot` berdasarkan pergerakan Mouse, dan Cinemachine akan secara otomatis mengikuti rotasi orbit di sekitarnya. Pergerakan W,A,S,D akan selalu dikalkulasi relatif terhadap arah pandang kamera saat itu.
