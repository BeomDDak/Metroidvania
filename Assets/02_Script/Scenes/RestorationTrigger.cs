using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RestorationTrigger : MonoBehaviour
{
    public GameObject boosRoomDoor;

    public GameObject caution;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.position = new Vector2(-1, 4);
            caution.SetActive(true);
            StartCoroutine(DisppearCaution());
        }
    }

    IEnumerator DisppearCaution()
    {
        yield return new WaitForSeconds(0.5f);
        caution.SetActive(false);
    }

    private void Update()
    {
        if(DataManager.instance.stage1Clear && DataManager.instance.stage2Clear && DataManager.instance.stage3Clear)
        {
            boosRoomDoor.SetActive(true);
            Destroy(gameObject);
        }
    }
}
