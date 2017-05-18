﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    [SerializeField]
    private bool hits;

    [SerializeField]
    private bool multipleRoundhouseHit;
    public bool MultipleRoundhouseHit
    {
        get { return multipleRoundhouseHit; }
    }

    [SerializeField]
    private bool hitAOE;
    public bool HitAOE
    {
        get { return hitAOE; }
    }

    [SerializeField]
    private int damage;
    public int Damage
    {
        get { return damage; }
    }

    [SerializeField]
    private float attackCooldown;

    [SerializeField]
    private float projectileSpeed;
    public float ProjectileSpeed
    {
        get { return projectileSpeed; }
    }

    [SerializeField]
    private string effect;
    public string Effect
    {
        get { return effect; }
    }

    [SerializeField]
    private int effectValue;

    [SerializeField]
    private Projectile projectileObject;

	[SerializeField]
	private int id;
	public int Id{
		get{ return id; }
		set{ id = value; }
	}

    private SpriteRenderer sr;

	private Enemy enem2;
    public Enemy Enem2
    {
        get { return enem2; }
    }

    private Queue<Enemy> enemies = new Queue<Enemy>();

    private float attackTimer;
    private bool canAttack;

    private Vector2[] vlist = new Vector2[8];
    


    // Use this for initialization
    void Start ()
    {
        sr = GetComponent<SpriteRenderer>();
        if (multipleRoundhouseHit)
        {
            vlist[0] = new Vector2(transform.position.x + 100, transform.position.y);
            vlist[1] = new Vector2(transform.position.x - 100, transform.position.y);
            vlist[2] = new Vector2(transform.position.x, transform.position.y + 100);
            vlist[3] = new Vector2(transform.position.x, transform.position.y - 100);
            vlist[4] = new Vector2(transform.position.x + 100, transform.position.y + 100);
            vlist[5] = new Vector2(transform.position.x + 100, transform.position.y - 100);
            vlist[6] = new Vector2(transform.position.x - 100, transform.position.y + 100);
            vlist[7] = new Vector2(transform.position.x - 100, transform.position.y - 100);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		UpdateRange();

        if(hits == true) Attack();
    }
      
	public void UpdateRange()
    {
		GetComponent<CircleCollider2D>().radius = Mathf.Max(transform.localScale.x,transform.localScale.y);
	}

    private void Attack()
    {
        if (!canAttack)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackCooldown)
            {
                canAttack = true;
                attackTimer = 0;
            }
        }

        if (enem2 == null && enemies.Count > 0)
        {
            enem2 = enemies.Dequeue();
        }

        if(enem2 != null && enem2.isActiveAndEnabled && canAttack)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
		if(projectileObject == null)
        {
			projectileObject = (Projectile)Resources.Load("Prefabs/Projectiles/Projectile", typeof(Projectile));
		}

        if (multipleRoundhouseHit)
        {
            Projectile[] proj = new Projectile[8];
            
            for (int i = 0; i < 8; i++)
            {
                proj[i] = Instantiate(projectileObject, this.transform.position, Quaternion.identity) as Projectile;
                proj[i].Initialize(this,vlist[i]);
            }

            canAttack = false;
        }
        else {
            Projectile proj = Instantiate(projectileObject, this.transform.position, Quaternion.identity) as Projectile;
            proj.Initialize(this, new Vector2(0,0));

            canAttack = false;
        }
    }

    public void Select()
    {
        sr.enabled = !sr.enabled;
    }


    public void DamageBuff(int value)
    {
        damage += value;
    }


    public void OnTriggerEnter2D(Collider2D o)
    {
        if (o.tag == "Enemy")
        {           
            if (hits == true)
            {
                enemies.Enqueue(o.GetComponent<Enemy>());
            }

            if (effect == "Slow")
            {
                o.GetComponent<Enemy>().EffectHit("Slow", effectValue);
            }
        }

        if (o.tag == "Tower")
        {
            if (effect == "DamageBuff")
            {
                o.GetComponentInChildren<Tower>().DamageBuff(effectValue);
            }
        }
    }


    public void OnTriggerExit2D(Collider2D o)
    {
        if(o.tag == "Enemy")
        {
            if (hits == true)
            {
                enem2 = null;
            }

            if (effect == "Slow")
            {
                o.GetComponent<Enemy>().EffectHit("Restore Movement", effectValue);
            }
        }

        if (o.tag == "Tower")
        {
            if (effect == "DamageBuff")
            {
                o.GetComponentInChildren<Tower>().DamageBuff(effectValue);
            }
        }

        if (multipleRoundhouseHit)
        {
            if(o.tag == "Projectile")
            {
                Destroy(o.gameObject);
            }
        }
    }
}
