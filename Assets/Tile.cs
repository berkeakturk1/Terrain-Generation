using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;

public class Tile : MonoBehaviour
{
    public Sprite sprite;
    public List<String> adj_rules;
    private float xpos, ypos;
    public int type;
    public string _name = "";
    public List<Tile> selectable_tiles = new List<Tile>();
    public Quaternion angle;
    private void Awake()
    {
        checkSprite();
    }

    void Start()
    {
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setPosition(float x, float y)
    {
        this.xpos = x;
        this.ypos = y;
    }

    public float getX()
    {
        return xpos;
    }
    
    public float getY()
    {
        return ypos;
    }
    
    void checkSprite()
    {
        switch (sprite.name)
        {
            case "shore":
                _name = "shore";
                adj_rules.Add("shore");
                adj_rules.Add("water");
                adj_rules.Add("grass");
                break;
            case "water":
                _name = "water";
                adj_rules.Add("shore");
                adj_rules.Add("water");
                break;
            case "flowers":
                _name = "flowers";
                adj_rules.Add("grass");
                adj_rules.Add("flowers");
                break;
            case "grass":
                _name = "grass";
                adj_rules.Add("grass");
                adj_rules.Add("flowers");
                adj_rules.Add("shore");
                break;
            default:
                adj_rules.Add("grass");
                adj_rules.Add("flowers");
                adj_rules.Add("water");
                break;
            
        }
    }
}
