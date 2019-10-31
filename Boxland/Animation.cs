using System;
using System.Collections.Generic;

namespace Boxland
  {
  public class Animation
    {
    public Action action;
    public List<int> sequence = new List<int> ();
    public bool looping = false;

    ////////////////////////////////////////////////////////////

    public Animation (Action new_action, List<int> new_sequence)
      {
      action = new_action;
      sequence = new_sequence;
      }

    ////////////////////////////////////////////////////////////

    public Animation (Action new_action, List<int> new_sequence, bool is_looping)
      {
      action = new_action;
      sequence = new_sequence;
      looping = is_looping;
      }

    ////////////////////////////////////////////////////////////

    }
  }
