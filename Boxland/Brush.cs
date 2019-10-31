using System;
//using Microsoft.Xna.Framework.Graphics;

namespace Boxland
  {
  public class Brush : Thing
    {
    public int top_texture_number;       // top of cube - references an array of TextureStruct
    public int top_texture_offset_x;     // location within texture sheet of this brush's texture
    public int top_texture_offset_y;
    public int front_texture_number;     // front of cube
    public int front_texture_offset_x;
    public int front_texture_offset_y;
    public int background_texture = -1;  // some surfaces like switches need an extra texture drawn behind them
    public bool moveable;                // can it be pushed around?
    public bool drawn = false;           // has it been moved to draw list this step?
    public int weight;                   // in pounds
    public double ext_x_velocity;        // xyz velocity of external forces acting on brush
    public double ext_y_velocity;        // (box pushing, wind, conveyor)
    public double ext_z_velocity;
    public int destination_x;            // intended stopping position when moving
    public int destination_y;
    public bool moving_north;            // used to manage external force velocity
    public bool moving_south;
    public bool moving_west;
    public bool moving_east;
    //public bool moving_on_conveyor;

    public int top_sticker;              // 0 = none
    public string top_sticker_type;      // standard, art, sign
    public int top_sticker_offset_x;
    public int top_sticker_offset_y;
    public float top_sticker_alpha;
    public int front_sticker;
    public string front_sticker_type;
    public int front_sticker_offset_x;
    public int front_sticker_offset_y;
    public float front_sticker_alpha;

    public bool moving;                  // true if box being moved (used for shadow correction)
    public int gateway;                  // level number the game loads if richards passes through (for gateway doors, -1 = none)
    public double temperature;           // used for hot metal boxes
    public bool transparent;             // does sprite have a transparent portion? (used for drawing)
    public bool electric;                // uses electricity (can be connected with wires)
    public bool solid = true;            // collision detection.  set to false for open doors, etc.
    public Door door;

    public bool top_shadow_north;  // tells whether the top of the brush has a shadow casted from a wall in that direction
    public bool top_shadow_south;
    public bool top_shadow_east;
    public bool top_shadow_west;
    public bool top_shadow_northeast;
    public bool top_shadow_northwest;

    public bool top_left_outline;    // draws a black line around the outside of a wall section
    public bool top_right_outline;   // outlines should wrap each unique group of textures
    public bool top_top_outline;
    public bool top_bottom_outline;
    public bool top_top_left_outline;
    public bool top_top_right_outline;
    public bool top_bottom_left_outline;
    public bool top_bottom_right_outline;
    public bool front_left_outline;
    public bool front_right_outline;
    public bool front_top_outline;
    public bool front_bottom_outline;
    public bool front_top_left_outline;
    public bool front_top_right_outline;
    public bool front_bottom_left_outline;
    public bool front_bottom_right_outline;

    ////////////////////////////////////////////////////////////////////////////////

  
    ////////////////////////////////////////////////////////////////////////////////

    }  // class brush
  }
