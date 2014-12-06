using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Boxland
  {
  public struct Pow
    {
    public int x, y;       // relative to character's sprite
    public int color;      // random 0-2
    public int shape;      // random 0-4
    public float opacity;  // fades amount
    public bool behind;    // true = pow drawn behind character
    }

  struct Draw_Slot
    {
    public int id;       // id # of thing being drawn
    public string type;  // brush / character / object
    }

  struct Light
    {
    public int x, y, z;
    public int light_number;
    public float alpha;
    public float scale;
    public int type;          // 0 = normal, 1 = pulsing, 2 = flickering
    public bool on;           // on/off for flickering lights
    public bool waxing;       // wax/wane used for pulsing
    public float dimness;     // amount of darkness to apply over light, used for pulsing
    public float pulse_speed;
    public Color c;           // used only for lighting engine 2
    }

  struct Location  // used for random enemy placement
    {
    public int x, y, z;
    public bool used;
    }

  public struct Screen  // info about screen size, scrolling, etc. (used for drawing)
    {
    public int width;
    public int height;
    public int scroll_x;             // screen scroll offset
    public int scroll_y;
    public double bg1_scroll_speed;  // % of scroll offset used for background scrolling
    public double bg2_scroll_speed;
    public int bg1_scroll_x;
    public int bg1_scroll_y;
    public int bg2_scroll_x;
    public int bg2_scroll_y;
    }
  }
