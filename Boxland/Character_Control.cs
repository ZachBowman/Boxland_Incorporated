using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boxland
  {
  public class Character_Control
    {
    public List<Character> character = new List<Character> ();

    ////////////////////////////////////////////////////////////////////////////////

    public void add (string name, int sprite, int x, int y, int z)
      {
      Character c = new Character ();

      c.name = name;
      c.sprite = sprite;
      c.dx = x;
      c.dy = y;
      c.dz = z;
      c.defaults ();

      character.Add (c);
      }

    ////////////////////////////////////////////////////////////////////////////////

    public void cycle_shirts ()
      {
      }    
    }
  }
