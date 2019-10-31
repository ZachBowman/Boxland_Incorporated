using System;
//using Microsoft.Xna.Framework;

namespace Boxland
  {
  public class Fixture : Thing
    {
    public Fixture_Type type;
    public bool drawn;                  // has it been moved to draw list this step?
    public int draw_distance;           // used for version 3 (diagonal) draw order
    public string dir;                  // north, south, east, west.  used only for some fixtures, like machines
    public bool electric;               // uses electricity (can be connected with wires)
    public bool powered;                // plugged in or has charge
    public bool has_switch;             // can be switched on/off at will by player
    public bool on;                     // on/off, used only for machines
    public int total_frames;            // 1 = not animated
    public int current_frame;
    public int frame_delay;             // time between animation frames
    public int frame_counter;
    public int sprite_width;            // only used for animated fixtures
    public int sprite_height;
    public bool solid;                  // non-solid fixtures can be passed though, like laser beams
    public Door door_type;              // none, pressure, swing, slide
    public bool door_open;              // used for doors
    public bool door_horizontal;        // used for door orientation
    public int door_keys;               // number of keys needed to open door
    public Fixture_Animation animation;
    public int destination_x;           // location at end of animation sequence, currently only for sliding doors

    ////////////////////////////////////////////////////////////////////////////////

    // for animated fixtures, like conveyor belts

    public void update_frames ()
      {
      if (total_frames > 1)
        {
        frame_counter += 1;
        if (frame_counter >= frame_delay)
          {
          frame_counter = 0;
          current_frame += 1;
          if (current_frame >= total_frames) current_frame = 0;
          }
        }

      if (animation != Fixture_Animation.idle)
        {
        if (x > destination_x) x -= 1;
        else if (x < destination_x) x += 1;
        else if (x == destination_x) animation = Fixture_Animation.idle;
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    public void door_toggle (int keys)
      {
      if (door_keys > keys) return;
      if (animation != Fixture_Animation.idle) return;

      if (door_type == Door.swing)
        {
        if (door_horizontal)
          {
          if (!door_open)
            {
            x -= 9;
            y -= 65;
            }
          else  // closed
            {
            x += 9;
            y += 65;
            }
          }

        else  // vertical
          {
          if (!door_open)
            {
            x += 9;
            y += 65;
            }
          else  // closed
            {
            x -= 9;
            y -= 65;
            }
          }

        //x = Convert.ToInt16 (dx);
        //y = Convert.ToInt16 (dy);
        
        // trade width and length
        int temp_size = width;
        width = length;
        length = temp_size;
        }

      else if (door_type == Door.slide) door_slide_toggle ();

      door_open = door_open ? false : true;
      }

    //////////////////////////////////////////////////////////////////////

    public void door_slide_toggle ()
      {
      if (door_horizontal)
        {
        // TO DO: change to animated slide
        if (!door_open)
          {
          destination_x = x - 80;
          animation = Fixture_Animation.door_opening;
          }
        else
          {
          destination_x = x + 80;
          animation = Fixture_Animation.door_closing;
          }
        }
      }

    //////////////////////////////////////////////////////////////////////

    }
  }
