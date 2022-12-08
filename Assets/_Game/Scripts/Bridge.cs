using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] private GameObject endBridgeBlock;
    [SerializeField] private GameObject destinationPoint;
    [SerializeField] private GameObject gateFront;
    [SerializeField] private GameObject gateBack;

    private Animator gateFrontAnim;
    private Animator gateBackAnim;
    private bool isClosed;
    private Color color;

    public GameObject DestinationPoint => destinationPoint;
    public bool IsSelected { get; set; }
    public bool IsClosed => isClosed;

    private void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
        gateFrontAnim = gateFront.GetComponent<Animator>();
        gateBackAnim = gateBack.GetComponent<Animator>();

        ChangeAnim(gateFrontAnim,"open");
        ChangeAnim(gateBackAnim, "close");
    }

    protected void ChangeAnim(Animator anim,string animName)
    {
        anim.SetTrigger(animName);
    }

    public void CheckBridgeEnd()
    {
        if (isClosed) return;
        if(endBridgeBlock.GetComponent<MeshRenderer>().material.color != Color.white)
        {
            ChangeAnim(gateBackAnim, "open");
            color = endBridgeBlock.GetComponent<MeshRenderer>().material.color;
            StartCoroutine(AutoClose());
            isClosed = true;
        }
    }

    IEnumerator AutoClose()
    {
        yield return new WaitForSeconds(1f);
        ChangeAnim(gateBackAnim, "close");
        foreach (MeshRenderer mesh in GetComponentsInChildren<MeshRenderer>())
        {
            mesh.material.color = color;
        }
        isClosed = true;
    }


    public void OnTriggerExit(Collider other)
    {
/*        if (isClosed || other.GetComponent<Character>().Material.color != color) return;
        ChangeAnim(gateBackAnim, "close");
        foreach (MeshRenderer mesh in GetComponentsInChildren<MeshRenderer>())
        {
            mesh.material.color = color;
        }
        isClosed = true;*/
    }
}
