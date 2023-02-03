using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    float maxHp;
    float currentHp;
    float power = 1f;
    float defenseStat;
    // Start is called before the first frame update
    void Start()
    {
        maxHp = currentHp = 100f;
        defenseStat = 10f;
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Die();
        Debug.Log(currentHp);
      /*  EnchanterChangeStat(id);
        ItemChangeStat(id);*/
    }
    

    void Move()
    {
        if (Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.S) == false && Input.GetKey(KeyCode.D) == false) return;
        else if (Input.GetKey(KeyCode.W)) rb.AddForce(Vector3.up * power);
        else if (Input.GetKey(KeyCode.A)) rb.AddForce(Vector3.left * power);
        else if (Input.GetKey(KeyCode.S)) rb.AddForce(Vector3.down * power);
        else if (Input.GetKey(KeyCode.D)) rb.AddForce(Vector3.right * power);
    }

    void EnchanterChangeStat(int id)
    {
        Debug.Log("스텟 변화");
        switch (id)
        {
            case 1:
                maxHp = currentHp += 100;
                Debug.Log("현재 체력은:" + currentHp);
                Debug.Log("최대 체력은:" + maxHp);
                break;
            case 2:
                power *= 2;
                break;
            case 3:
                defenseStat *= 2;
                break;
        }
                
    }

    void ItemChangeStat(int id)
    {
        if (id == 1)
        {
            float saveHp = currentHp + 50;
            if (saveHp >= maxHp) currentHp = maxHp;
            else currentHp = saveHp;
            Debug.Log("회복된 현재 체력은:" + currentHp);
        }

    }

    void OnCollisionEnter2D(Collision2D other)
    { if (other.gameObject.tag == "Monster") currentHp -= other.gameObject.GetComponent<Monster>().damage; }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag ("Enchanter") == true) EnchanterChangeStat(other.gameObject.GetComponent<Enchanter>().id);
        else if (other.CompareTag ("Item")== true) ItemChangeStat(other.gameObject.GetComponent<Item>().id);  
    }

    void Die()
    { if (currentHp <= 0) Destroy(this.gameObject);}
}
