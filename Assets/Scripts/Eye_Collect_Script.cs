using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EyeCollectible : MonoBehaviour
{
    public float rotateSpeed = 90f;             // spins on Y
    public string playerTag = "MainEyeball";    // the eyeball's tag

    void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
        gameObject.tag = "EyeCollect";
    }

    void Update()
    {
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f, Space.Self);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        // Tell the manager we collected one
        if (EyeLevelManager.Instance) EyeLevelManager.Instance.OnCollectedOne();

        Destroy(gameObject);
    }
}
