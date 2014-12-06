using System;
using Microsoft.Xna.Framework;

namespace Boxland
  {
  public class Character : Thing
    {
    //public Texture2D sprite;

    // physical
    public string name;
    public bool player;
    public int sprite;              // = RICHARD, RETARD, etc.
    public int total_skins;         // possible skins (colors, shirts) for character
    public int skin;                // current sprite / appearance
    //public int x, y, z;             // world location
    //public double dx, dy, dz;       // fractional world location
    //public int box_width;           // dimensions of bounding box
    //public int box_length;
    //public int box_height;
    public double dir;              // direction facing (radians)
    public double last_dir;         // last direction facing (used to calculate floor skidding)
    public double self_velocity;    // speed characters wants to move
    public double max_self_velocity;
    public double self_x_velocity;  // xyz velocity character wants to move by
    public double self_y_velocity;
    public double self_z_velocity;
    public double ext_x_velocity;   // xyz velocity of external forces acting on character
    public double ext_y_velocity;   // (wind, collision, etc.)
    public double ext_z_velocity;
    public double net_x_velocity;   // actual world speed of character
    public double net_y_velocity;
    public double net_z_velocity;
    public int sprite_width;
    public int sprite_height;
    public Pow pow1, pow2;          // impact stars
    public int brush_grab;          // brush number of grabbed box, -1 = none
    public string grab_position;    // relative to grabbed box: ABOVE, BELOW, LEFT, RIGHT
    public int push_x, push_y;      // destination coordinates when moving boxes
    public string push_dir;         // direction pushing
    //public string shirt;           // shirt wearing
    public int shirt;           // shirt wearing
    public int draw_distance;       // used for version 3 (diagonal) draw order
    public bool runboost;
    //public int run_distance_for_skidding = 10;

    // inventory
    public int coins = 0;
    public int scrap_metal = 0;
    public int energy = 0;
    public bool shirt_power = false;     // has power glove?
    public bool shirt_fire = false;
    public bool shirt_ice = false;
    public bool shirt_electric = false;
    public bool shirt_magnetic = false;
    public bool projectiles = false;      // shoots/throws something

    // animations
    public string action;                // current physical action (changed from int to string to accomodate class usage)
    public string last_action;
    public int total_frames;
    public int anim_frame;               // current frame # (based on bitmap)
    public string anim_direction;        // direction facing
    public int anim_frame_counter;
    public int max_anim_frame_counter;
    public int walk_pixels;
    public int anim_frame_sequence;      // current frame # in the current animation
    public int max_anim_frame_sequence;  // total frames in the current animation
    public bool blinking;                // is he blinking?
    public const int skid_delay = 5;   // the max delay between button presses for skidding to occur
    public int skid_counter = skid_delay;// allows a delay for skidding caused by a radical direction change while running
    //public int run_counter = 0;          // keeps track of time spent running - a minimum must be met for skidding to take place
    
    // ai (non-player only)
    public bool hostile;       // if false, does not attack
    public string goal;        // motivation
    public string target_type; // object, character or location
    public int target;         // target of goal
    public int target_x;       // last known location of target
    public int target_y;
    public int target_z;
    public int subtarget_x;    // used for going around walls when projected path clips
    public int subtarget_y;
    public int subtarget_z;

    // variable stats
    public int health;
    public int injury_timer;  // minimum numbers of frames until next damage (prevents cheap deaths by fire / jump kick)
    public int on_fire;       // number of frames until fire burns out.  0 = not on fire
    //public bool burnt;
    public int combo;         // 0 = punch right
                              // 1 = punch left
                              // 2 = kick
                              // 3 = special finisher

    // constant traits (0-10, 5 avg)
    //public int combat;
    //public int intelligence;
    public double speed;
    //public int toughness;
    public int strength;

    public bool drawn;       // has character been moved to draw list this step?
    public double distance;  // distance from camera (for draw order)

    // sound cues
    public bool sound_knockout = false;

    ////////////////////////////////////////////////////////////////////////////////

    //public Character (string new_name, int new_sprite, int new_x, int new_y, int new_z)
      //{
      //int c;

      //if (total_characters < 0) total_characters = 0;

//      if (total_characters < max_characters)
//        {
//        c = total_characters;

        //name = new_name;
        //sprite = new_sprite;
        //dx = new_x;
        //dy = new_y;
        //dz = new_z;
//        character[c].defaults ();

//        //character[c].dir = MathHelper.ToRadians (270);

//        total_characters += 1;
//        total_draw_slots += 1;
//        }
      //}

    ////////////////////////////////////////////////////////////////////////////////

    public void defaults ()
      {
      Random rnd = new Random ();

      total_skins = 1;
      skin = 0;

      dir = MathHelper.ToRadians (270);
      last_dir = dir;
      action = "none";
      push_dir = "none";
      shirt_power = false;
      shirt_fire = false;
      shirt_ice = false;
      shirt_electric = false;
      shirt_magnetic = false;
      coins = 0;
      total_frames = 1;
      anim_frame = 0;

      health = 100;
      on_fire = 0;
      combo = 0;
      speed = 4;
      projectiles = false;

      hostile = true;
      goal = "none";
      target_type = "none";
      target = -1;
      subtarget_x = -1;
      subtarget_y = -1;
      action = "none";
      brush_grab = -1;
      shirt = 0;// "none";

      player = false;
        
      if (name == "Richard")
        {
        total_skins = 6;
        skin = 0;

        speed = 6;
        sprite_width = 80;
        sprite_height = 93;// 87;
        walk_pixels = 6;
        player = true;

        hostile = false;
        }
      else if (name == "Richard's Dad")
        {
        total_skins = 1;
        speed = 6;
        sprite_width = 72;
        sprite_height = 113;
        walk_pixels = 6;

        hostile = false;
        }
      else if (name == "retard")
        {
        total_skins = 1;
        //skin = rnd.Next (0, total_skins);

        speed = 4;

        sprite_width = 102;// 80;
        sprite_height = 139;
        walk_pixels = 6;
        }
      else if (name == "throwing retard")
        {
        total_skins = 1;
        //skin = rnd.Next (0, total_skins);

        speed = 3;

        projectiles = true;

        sprite_width = 80;// 96;
        sprite_height = 93;// 103;
        walk_pixels = 6;// 3;
        }

      width = Convert.ToInt16 (sprite_width * .5);
      length = width;
      height = sprite_height;
      max_self_velocity = speed * .6;
      }

    ////////////////////////////////////////////////////////////////////////////////

    public void stand ()
      {
      self_velocity = 0;
      last_action = action;
      if (action == "running") skid_counter = 0;
      //run_counter = 0;
      action = "standing";
      }

    ////////////////////////////////////////////////////////////////////////////////

    public void walk ()
      {
      last_action = action;
      if (player == false) self_velocity = speed * .3;
      runboost = false;
      action = "walking";
      }

    ////////////////////////////////////////////////////////////////////////////////

    //public void run ()
      //{
      //action = "running";
      //if (player == false) self_velocity = max_self_velocity;
      //runboost = true;
      //}

    ////////////////////////////////////////////////////////////////////////////////

    public void skid ()//_if_able ()
      {
      //double dir_degrees = MathHelper.ToDegrees (Convert.ToSingle (dir));
      //double last_dir_degrees = MathHelper.ToDegrees (Convert.ToSingle (last_dir));
      //double angle_difference = Math.Abs (dir_degrees - last_dir_degrees);
      //if (angle_difference > 180) angle_difference = Math.Abs (angle_difference - 360);  // compensate for angles around the 0 degree mark
      //if (angle_difference > 90)
      //if (sharp_turn ())
      //  {
        // convert last self-propelled x & y speed into external inertia
      //double skid_x_speed = self_velocity * Math.Cos (last_dir);
      //double skid_y_speed = self_velocity * Math.Sin (last_dir);
      double skid_x_speed = max_self_velocity * Math.Cos (last_dir);
      double skid_y_speed = max_self_velocity * Math.Sin (last_dir);

        // change external force speed to slightly less than double previous running speed
        ext_x_velocity = skid_x_speed * 1.7;
        ext_y_velocity = skid_y_speed * 1.7;

        action = "skidding";
        //}

      // SETTING THIS IS UNDONE ONCE WE RETURN TO KEYBOARD_INPUT AND SET THE NEW DIRECTION.
      // MAYBE SKID SHOULD BE SAVED AND CALLED AFTER THE DIRECTION CHANGE?
      last_dir = dir;
      }

    ////////////////////////////////////////////////////////////////////////////////

    public void apply_force_from_point (double force, int source_x, int source_y)
      {
      double force_direction = get_direction (source_x, source_y, x, y);

      ext_x_velocity = force * Math.Cos (force_direction);
      ext_y_velocity = force * Math.Sin (force_direction);
      }

    ////////////////////////////////////////////////////////////////////////////////

    double get_direction (double x1, double y1, double x2, double y2)
      {
      double dir_radians;
      double x_distance, y_distance;

      x_distance = x2 - x1;
      y_distance = y2 - y1;

      // get radians of direction
      if (x_distance > 0 && y_distance >= 0) dir_radians = Math.Atan(y_distance / x_distance);
      else if (x_distance > 0 && y_distance < 0) dir_radians = Math.Atan(y_distance / x_distance) + (2 * Math.PI);
      else if (x_distance < 0) dir_radians = Math.Atan(y_distance / x_distance) + Math.PI;
      else if (x_distance == 0 && y_distance > 0) dir_radians = MathHelper.ToRadians(90);//Math.PI / 2;
      else if (x_distance == 0 && y_distance < 0) dir_radians = MathHelper.ToRadians(270);//-1 * Math.PI / 2;
      else dir_radians = 0;  // x_distance = 0, y_distance = 0

      return dir_radians;
      }

    ////////////////////////////////////////////////////////////////////////////////

    public void attack_character (Character target_character, int target_id)
      {
      goal = "attack";
      target_type = "character";
      target = target_id;
      subtarget_x = target_character.x;
      subtarget_y = target_character.y;
      subtarget_z = target_character.z;
      face_character (target_character);
      //self_velocity = speed;
      }

    ////////////////////////////////////////////////////////////////////////////////

    public void face_character (Character target)
      {
      dir = get_direction (x, y, target.x, target.y);
      }
    
    ////////////////////////////////////////////////////////////////////////////////

    public bool is_facing_character (Character target)
      {
      double radians_needed = get_direction (dx, dy, target.dx, target.dy);

      if (radians_needed + MathHelper.ToRadians (45) > MathHelper.ToRadians (360)
          && dir + MathHelper.ToRadians (360) <= radians_needed + MathHelper.ToRadians (45)
          && dir + MathHelper.ToRadians (360) >= radians_needed - MathHelper.ToRadians (45))
        return true;

      else if (radians_needed - MathHelper.ToRadians (45) < MathHelper.ToRadians (0)
          && dir <= radians_needed + MathHelper.ToRadians (45)
          && dir - MathHelper.ToRadians (360) >= radians_needed - MathHelper.ToRadians (45))
        return true;

      else if (dir <= radians_needed + MathHelper.ToRadians(45)
          && dir >= radians_needed - MathHelper.ToRadians(45))
        return true;

      else return false;
      }

    ////////////////////////////////////////////////////////////////////////////////

    double distance2d (double x1, double y1, double x2, double y2)
      {
      double h, x, y;

      x = x2 - x1;
      y = y2 - y1;
      h = Math.Sqrt ((x * x) + (y * y));

      if (h < 0) h = h * -1;  // absolute value

      return h;
      }

    ////////////////////////////////////////////////////////////////////////////////

    public void update_physical ()
      {
      //double angle_difference = 0;
      //double dir_degrees, last_dir_degrees;

      // health gone, knocked out
      if (health <= 0 && action != "knocked out")
        {
        health = 0;
        action = "knocked out";
        self_velocity = 0;
        sound_knockout = true;
        }
      else sound_knockout = false;

      if (injury_timer > 0) injury_timer -= 1;

      // max running speed
      if (self_velocity > max_self_velocity) self_velocity = max_self_velocity;

      // change from walking to running
      //if (action == "walking" && self_velocity > 2.5) run ();

      // run counter (minimum distance for skidding)
      //if (action == "running" && run_counter < run_distance_for_skidding) run_counter += 1;
      //else if (action != "running") run_counter = 0;

      // stop skidding if direction changes while skidding
      if (action == "skidding" && dir != last_dir)
        {
        last_dir = dir;
        last_action = action;
        ext_x_velocity = 0;
        ext_y_velocity = 0;
        skid_counter = skid_delay;
        action = "running";
        }
      
      // skid (from quick reverse while running)
      //if (dir != last_dir && skid_counter == 0) // changing running direction
      //if (dir != last_dir && skid_counter < skid_delay) // changing running direction
        //{
        //if (run_counter >= 20)
        //  skid_counter += 1;  // minimum distance met, start the skid counter
        //else  // hasn't run far enough to skid - reset everything
        //  {
        //  last_dir = dir;
        //  run_counter = 0;
        //  }
        //}

      if (skid_counter < skid_delay) skid_counter += 1;

      //if (last_dir != dir && action == "running" && skid_counter < skid_delay)
        //{
        //dir_degrees = MathHelper.ToDegrees (Convert.ToSingle (dir));
        //last_dir_degrees = MathHelper.ToDegrees (Convert.ToSingle (last_dir));
        //angle_difference = Math.Abs (dir_degrees - last_dir_degrees);
        //if (angle_difference > 180) angle_difference = Math.Abs (angle_difference - 360);  // compensate for angles around the 0 degree mark
        //if (angle_difference > 90)
        //  {
        //  //skid ();
        //  }

        //last_dir = dir;

        //skid ();// _if_able ();
        //}

      // skid_counter allows for a slight delay between button presses
      //if (skid_counter > 0)
      //if (skid_counter < skid_delay)
      //  {
      //  skid_counter += 1;
      //  if (skid_counter >= skid_delay)//1)
      //    {
      //    //skid_counter = 0;

      //    // if guy turns more than 90 degrees while running, he skids on floor a little
      //    if (action == "running")// && run_counter >= run_distance_for_skidding)
      //      {
      //      dir_degrees = MathHelper.ToDegrees (Convert.ToSingle (dir));
      //      last_dir_degrees = MathHelper.ToDegrees (Convert.ToSingle (last_dir));
      //      angle_difference = Math.Abs (dir_degrees - last_dir_degrees);
      //      if (angle_difference > 180) angle_difference = Math.Abs (angle_difference - 360);  // compensate for angles around the 0 degree mark
      //      if (angle_difference > 90)
      //        {
      //        // convert last self-propelled x & y speed into external inertia
      //        double skid_x_speed = self_velocity * Math.Cos (last_dir);
      //        double skid_y_speed = self_velocity * Math.Sin (last_dir);

      //        // change external force speed to slightly less than double previous running speed
      //        ext_x_velocity = skid_x_speed * 1.7;
      //        ext_y_velocity = skid_y_speed * 1.7;

      //        action = "skidding";
      //        }
      //      }

      //    last_dir = dir;
      //    }
      //  }

      if (action == "skidding")
        {
        if (Math.Abs (ext_x_velocity) <= Math.Abs (self_x_velocity)
            && Math.Abs (ext_y_velocity) <= Math.Abs (self_y_velocity))
        //if (Math.Abs (ext_x_velocity) <= 0
          //  && Math.Abs (ext_y_velocity) <= 0)
          {
          last_action = action;
          ext_x_velocity = 0;
          ext_y_velocity = 0;
          skid_counter = skid_delay;
          action = "running";
          }
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    public int y_draw_coordinate (int screen_height, int scroll_y)
      {
      return (screen_height - y) - (z / 2) + scroll_y + walk_pixels;
      }

    ////////////////////////////////////////////////////////////////////////////////

    public void stop_pushing ()
      {
      self_x_velocity = 0;
      self_y_velocity = 0;
      x = push_x;
      y = push_y;
      dx = push_x;
      dy = push_y;
      action = "grabbing";
      push_dir = "none";
      }

    ////////////////////////////////////////////////////////////////////////////////

    }  // class character
  }  // namespace Boxland
