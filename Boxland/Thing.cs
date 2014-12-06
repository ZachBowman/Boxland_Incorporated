using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boxland
  {
  public class Thing
    {
    public int x, y, z;         // world location
    public double dx, dy, dz;   // fractional world location
    public int width;           // dimensions of bounding box
    public int length;
    public int height;

    public int collision ()
      {
      return -1;
      }
    }
  }
