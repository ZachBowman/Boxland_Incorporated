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
    public int char_width;
    public int tile_width;
    public int tile_length;
    public int tile_height;
    public int pixel_width;
    public int pixel_length;
    public int pixel_height;
    public float ambient_dark;
    public Color ambient_light;
    public Texture2D background;
    public bool bg_scroll;
    public int random_retard1 = 10;  // default values to be overwritten later
    public int random_retard2 = 5;

    public List<List<string>> grid = new List<List<string>> ();
    }
  }
