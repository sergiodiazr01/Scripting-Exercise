using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{

public float runSpeed;
public float gotHayDestroyDelay;
public float dropDestroyDelay; // 1
private bool hitByHay;
private Collider myCollider; // 2
private Rigidbody myRigidbody;
private SheepSpawner sheepSpawner;
public float heartOffset; // 1
public GameObject heartPrefab; // 2
private bool hasDropped = false;
    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<Collider>();
        myRigidbody = GetComponent<Rigidbody>();
    }
    private void Drop()
   {
        if (hasDropped) return; // Avoiding multiple drops
        hasDropped = true;
       GameStateManager.Instance.DroppedSheep();
       sheepSpawner.RemoveSheepFromList(gameObject);
       myRigidbody.isKinematic = false; // 1
       myCollider.isTrigger = false; // 2
       SoundManager.Instance.PlaySheepDroppedClip();
       Destroy(gameObject, dropDestroyDelay); // 3

   }

   
    // Update is called once per frame
    void Update()
    {

        transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);
        
    }
    private void HitByHay()
    {
        sheepSpawner.RemoveSheepFromList(gameObject);
        hitByHay = true; // 1
        runSpeed = 0; // 2
        Destroy(gameObject, gotHayDestroyDelay); // 3
        Instantiate(heartPrefab, transform.position + new Vector3(0, heartOffset, 0), Quaternion.identity);
        TweenScale tweenScale = gameObject.AddComponent<TweenScale>();; // 1
        tweenScale.targetScale = 0; // 2
        tweenScale.timeToReachTarget = gotHayDestroyDelay; // 3
        SoundManager.Instance.PlaySheepHitClip();
        GameStateManager.Instance.SavedSheep();

    }
    private void OnTriggerEnter(Collider other) // 1
    {
        if (other.CompareTag("Hay") && !hitByHay) // 2
        {
            Destroy(other.gameObject); // 3
            HitByHay(); // 4
        }
        else if (other.CompareTag("DropSheep"))
        {
            Drop();
        }
    }

    public void SetSpawner(SheepSpawner spawner)
   {
       sheepSpawner = spawner;
   }


}