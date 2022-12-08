using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : MonoBehaviour
{
    [SerializeField] protected Animator anim;
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected float rotateSpeed = 15f;

    [SerializeField] protected GameObject back;
    protected Material material;
    [SerializeField] protected GameObject characterModel;
    [SerializeField] protected LayerMask ground;
    [SerializeField] protected bool isHitOther;

    protected Rigidbody characterRb;
    protected Collider characterColl;
    protected string currentAnimName;
    protected Stage currentStage;
    [SerializeField] protected Vector3 destination;
    [SerializeField] protected bool isDeath;
    [SerializeField] protected bool isDown;
    [SerializeField] protected bool isVitory;
    protected Stack<GameObject> bricks = new Stack<GameObject>();

    public Material Material
    {
        get { return material; }   
        set { material = value; }  
    }
    public Stack<GameObject> Bricks => bricks;
    public Stage CurrentStage => currentStage;

    void Start()
    {
        OnInit();
    }

    protected virtual void OnInit()
    {
        if( PlayerPrefs.GetInt("CharacterColor")==0 || this is Enemy)
        {
            int rd = Random.Range(0, GameManager.Ins.materials.Count);
            material = GameManager.Ins.materials[rd];
            GameManager.Ins.materials.RemoveAt(rd);
        }
        
        characterModel.GetComponent<SkinnedMeshRenderer>().material = material;
        characterRb = GetComponent<Rigidbody>();
        characterColl = GetComponent<Collider>();
    }


    public void RayCheck()
    {
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, 2f , ground))
        {
            if (hit.transform.CompareTag("BridgeBlock") && hit.transform.GetComponent<MeshRenderer>().material.color != material.color)
            {
                RemoveBrick(hit.collider.gameObject);
            }
            else if (hit.transform.CompareTag("Stage") && currentStage != hit.transform.GetComponent<Stage>())
            {
                ChangeStage(hit.collider.GetComponent<Stage>());
            }
            
        }
    }

    public void RadiusCheck()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position,1f);
        foreach (Collider other in colliders)
        {
            if (bricks.Contains(other.gameObject)) continue;
            if (other.transform.CompareTag("Brick"))
            {
                if (other.transform.GetComponentInChildren<MeshRenderer>().material.color == material.color)
                {
                    currentStage.StorePosition(other.transform.position);
                    AddBrick(other.gameObject);
                }
                else if (other.transform.GetComponentInChildren<MeshRenderer>().material.color == Color.white)
                {
                    other.GetComponentInChildren<MeshRenderer>().material.color = material.color;
                    currentStage.AddNonColorBrick(this,other.gameObject);
                    AddBrick(other.gameObject);
                }
            }
            else if (other.transform.CompareTag("Character") && other != GetComponent<Collider>() && isHitOther)
            {
                Character character = other.transform.GetComponent<Character>();
                if (bricks.Count > character.Bricks.Count)
                {
                    character.Down();
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,1f);
    }

    public void AddBrick(GameObject brick)
    {
        brick.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        brick.GetComponent<Collider>().isTrigger = false;
        bricks.Push(brick.gameObject);
        brick.transform.rotation = back.transform.rotation;
        brick.transform.position = new Vector3(back.transform.position.x, back.transform.position.y + bricks.Count * 0.5f, back.transform.position.z);
        brick.transform.parent = back.transform;
    }

    public void RemoveBrick(GameObject brige)
    {
        if (bricks.Count <= 0) return;
        GameObject brick = bricks.Pop();
        currentStage.StoreBrick(this,brick);
        brick.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        brick.GetComponent<Collider>().isTrigger = true;
        MeshRenderer mesh = brige.transform.GetComponent<MeshRenderer>();
        mesh.enabled = true;
        mesh.material = material;
        brige.transform.parent.GetComponent<Bridge>().CheckBridgeEnd();
    }

    public void ClearBrick()
    {
        while (bricks.Count>0)
        {
            GameObject brick = bricks.Pop();
            brick.transform.parent = Pool.instance.pool;
            brick.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            brick.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1f,1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * brick.GetComponent<Rigidbody>().mass * 5, ForceMode.Impulse);
            currentStage.NonColorBrick(this,brick);
        }
    }

    public void ChangeStage(Stage stage)
    {
        if (currentStage != null) currentStage.RemoveCharacter(this);
        currentStage = stage;
        stage.AddCharacter(this);
        destination = stage.GetDestination(this);

        if (stage.isWinStage)
        {
            Win(stage.transform);
        }
    }
    protected void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }

    public virtual void Down()
    {

    }

    public virtual void Death()
    {
        isDeath = true;
        ClearBrick();
        ChangeAnim("death");
    }

    public virtual IEnumerator StandUp()
    {
        yield return new WaitForSeconds(Random.Range(3f, 5f));
        isDown = false;
        characterRb.isKinematic = false;
        characterColl.enabled = true;
    }

    public virtual void Win(Transform winpos)
    {
        isVitory = true;
        characterRb.isKinematic = true;
        transform.position = new Vector3(winpos.position.x,transform.position.y,winpos.transform.position.z);
        transform.rotation = Quaternion.LookRotation(Vector3.back);
        CameraFollow.instance.player = transform;
        CameraFollow.instance.speed = 1;
        ClearBrick();
        ChangeAnim("dance4");
    }
}

