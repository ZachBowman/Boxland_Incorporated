using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Boxland
  {
  class Map
    {
    //int tile_width;
    //int tile_length;
    //int tile_height;
    float ambient_dark;
    Color ambient_light;
    Texture2D test_background;
    int random_retard1 = 10;  // default values to be overwritten later
    int random_retard2 = 5;

    List<List<string>> grid = new List<List<string>> ();

    public Map (List<List<string>> new_grid)
      {
      grid = new_grid;
      }
    }
  }
