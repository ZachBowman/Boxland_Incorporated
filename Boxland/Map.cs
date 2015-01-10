using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Boxland
  {
  public class Map
    {
    public int width;
    public int length;
    public int height;
    public float ambient_dark;
    public Color ambient_light;
    public Texture2D background;
    public bool bg_scroll;
    public int random_retard1 = 10;  // default values to be overwritten later
    public int random_retard2 = 5;

    public List<List<string>> grid = new List<List<string>> ();
    }
  }
