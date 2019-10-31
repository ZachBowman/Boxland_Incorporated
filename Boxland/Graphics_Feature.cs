using System;
//using Microsoft.Xna.Framework.Graphics;

namespace Boxland
  {
  public class Graphics_Feature
    {
    public bool on = true;
    public float opacity = 1f;
    Fade_State fade = Fade_State.none;

    public void turn_on ()
      {
      on = true;
      fade = Fade_State.fading_in;
      }

    public void turn_off ()
      {
      fade = Fade_State.fading_out;
      }
    
    public void update ()
      {
      if (!on) return;

      if (fade == Fade_State.fading_in)
        {
        if (opacity < 1f) opacity += .01f;
        else
          {
          opacity = 1f;
          fade = Fade_State.none;
          }
        }
      else if (fade == Fade_State.fading_out)
        {
        if (opacity > 0f) opacity -= .01f;
        else
          {
          opacity = 0f;
          fade = Fade_State.none;
          on = false;
          }
        }
      }
    }
  }
 