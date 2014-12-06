using System;
using Microsoft.Xna.Framework;

namespace Boxland
  {
  public class Object : Thing
    {
    public int type;
    public double dir;          // radians
    public double velocity;
    public double x_velocity;
    public double y_velocity;
    public double z_velocity;
    public bool drawn;          // has object been moved to draw list this step?
    public double distance;     // distance from camera (for draw order)
    public int source;          // source character (no clipping or damage from object to source)
    public bool essential;      // non-essential objects can be destroyed for memory/performance reasons
    public bool destroyed;      // used to prevent last-item-in-list error
    //public int draw_distance;          // used for version 3 (diagonal) draw order
    public int skin;
    }
  }
