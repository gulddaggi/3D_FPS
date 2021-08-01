using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTrap : MonoBehaviour
{
    private Rigidbody[] rigid;
    [SerializeField]
    private GameObject go_Meat;

    [SerializeField]
    private int damage;

    private bool isActivated =  false;

    private AudioSource theAudio;
    [SerializeField]
    private AudioClip soung_Activate;

    void Start()
    {
        rigid = GetComponentsInChildren<Rigidbody>();
        theAudio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActivated)
        {
            if (other.transform.tag != "Untagged")
            {
                isActivated = true;
                theAudio.clip = soung_Activate;
                theAudio.Play();
                Destroy(go_Meat); // 고기 제거

                for (int i = 0; i < rigid.Length; i++)
                {
                    rigid[i].isKinematic = false;
                    rigid[i].useGravity = true;
                }

                if (other.transform.name == "Player")
                {
                    FindObjectOfType<StatusController>().DecreaseHP(damage);
                }
            }
        }
    }

}
