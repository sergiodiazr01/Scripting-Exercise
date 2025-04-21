using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HayMachine : MonoBehaviour
{
    public float movementSpeed;
    public float horizontalBoundary = 22; // Corregido: falta el punto y coma

    
    public GameObject hayBalePrefab; //Reference to the Hay Bale prefab.
    public Transform haySpawnpoint; //The point from which the hay will to be shot. 
    public float shootInterval; //The smallest amount of time between shots 
    private float shootTimer; //A timer that to keep track whether the machine can shoot
    public Transform modelParent; // 1

    // 2
    public GameObject blueModelPrefab;
    public GameObject yellowModelPrefab;
    public GameObject redModelPrefab;


    // Start is called before the first frame update
    void Start()
    {
        LoadModel();
    }
    private void LoadModel()
    {
        //Destroy(modelParent.GetChild(0).gameObject); // 1

        switch (GameSettings.hayMachineColor) // 2
        {
            case HayMachineColor.Blue:
                Instantiate(blueModelPrefab, modelParent);
            break;

            case HayMachineColor.Yellow:
                Instantiate(yellowModelPrefab, modelParent);
            break;

            case HayMachineColor.Red:
                Instantiate(redModelPrefab, modelParent);
            break;
        }
    }


    private void UpdateMovement()
    {
        // Declarar y obtener el valor de la entrada horizontal
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // Movimiento hacia la izquierda
        if (horizontalInput < 0 && transform.position.x > -horizontalBoundary)
        {
            transform.Translate(transform.right * -movementSpeed * Time.deltaTime);
        }
        // Movimiento hacia la derecha
        else if (horizontalInput > 0 && transform.position.x < horizontalBoundary)
        {
            transform.Translate(transform.right * movementSpeed * Time.deltaTime);
        }
    } // <-- Este es el cierre que faltaba

    private void ShootHay()
    {
        Vector3 hayPosition = haySpawnpoint.position;
        Instantiate(hayBalePrefab, haySpawnpoint.position, Quaternion.identity);
        SoundManager.Instance.PlayShootClip();
    }
    private void UpdateShooting()
    {
        shootTimer -= Time.deltaTime; // 1

        if (shootTimer <= 0 && Input.GetKey(KeyCode.Space)) // 2
        {
            shootTimer = shootInterval; // 3
            ShootHay(); // 4
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement(); 
        UpdateShooting();
    }
}



