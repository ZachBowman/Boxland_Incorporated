using System;
using Microsoft.Xna.Framework;

namespace Boxland
  {
  public class Fixture : Thing
    {
    public int type;
    public bool drawn;                 // has it been moved to draw list this step?
    public int draw_distance;          // used for version 3 (diagonal) draw order
    public string dir;                 // north, south, east, west.  used only for some fixtures, like machines
    public bool electric;              // uses electricity (can be connected with wires)
    public bool powered;               // plugged in or has charge
    public bool has_switch;            // can be switched on/off at will by player
    public bool on;                    // on/off, used only for machines
    public int total_frames;           // 1 = not animated
    public int current_frame;
    public int frame_delay;            // time between animation frames
    public int frame_counter;
    public int sprite_width;           // only used for animated fixtures
    public int sprite_height;
    public bool solid;                 // non-solid fixtures can be passed though, like laser beams
    }
  }
