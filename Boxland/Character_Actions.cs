using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Boxland
  {
  public partial class Boxland : Game
    {
    public void Update_Characters ()
      {
      //int b;
      int f, c;
      int b_clip;   // character-brush collision
      int c_clip;   // character-character collision
      int b_clip2;  // box-brush collision
      int f_clip;   // character-fixture collision

      Random rnd = new Random ();

      // SKINS
      for (c = 0; c < character_control.character.Count; c += 1)
        {
        if (c == PLAYER) character_control.character[c].skin = character_control.character[c].shirt;
        }

      // PHYSICAL EFFECTS
      for (c = 0; c < character_control.character.Count; c += 1)
        {
        character_control.character[c].update_physical ();

        if (character_control.character[c].sound_knockout == true)
          {
          //if (character_control.character[c].type == Name.RICHARD) sound_richard_knockout_test = true;
          //else if (character_control.character[c].type == Name.RETARD) sound_retard_knockout_test = true;
          //else if (character_control.character[c].type == Name.THROWING_RETARD) sound_retard_knockout_test = true;
          character_control.character[c].sound_knockout = false;
          }

        if (character_control.character[c].on_fire > 0)
          {
          if (rnd.Next (0, 4) == 0)
            {
            particle_flames (character_control.character[c].x + rnd.Next (-1 * character_control.character[c].width / 2, character_control.character[c].width / 2), character_control.character[c].y + rnd.Next (-1 * character_control.character[c].length / 2, character_control.character[c].length / 2), character_control.character[c].z + rnd.Next (0, character_control.character[c].height) + 32, "character", c);
            if (character_control.character[c].shirt != (int) Object_Type.shirt_red && character_control.character[c].action != Action.knocked_out) character_damage (c, 3, 0, 0, character_control.character[c].x, character_control.character[c].y, "fire", c);
            character_control.character[c].on_fire -= 1;
            }
          }

        // stop richard from pushing if his box hits a wall
        //if (character_control.character[c].action == Action.push && character_control.character[c].brush_grab > -1 && brush_control.brush[character_control.character[c].brush_grab].moving == false)
        //  {
        //  character_control.character[c].stop_pushing (brush_control.brush[character_control.character[c].brush_grab]);

        //  foreach (Brush brush in brush_control.brush)
        //    {
        //    brush_control.calculate_top_shadows (brush);
        //    }
        //  }
        }

      // IDLE COLLISIONS
      // boxes pushed into characters and characters stuck in walls
      for (c = 0; c < character_control.character.Count; c += 1)
        {
        b_clip = character_in_brush (character_control.character[c]);
        if (b_clip > -1)// && brush_control.brush[b_clip].moving == true)
          {
          // box going west
          if (character_control.character[c].dx - (character_control.character[c].width / 2) < brush_control.brush[b_clip].x
              && character_control.character[c].dx + (character_control.character[c].width / 2) > brush_control.brush[b_clip].x) character_control.character[c].dx = brush_control.brush[b_clip].x - (character_control.character[c].width / 2);
          // box going east
          if (character_control.character[c].dx - (character_control.character[c].width / 2) < brush_control.brush[b_clip].x + brush_control.brush[b_clip].width
              && character_control.character[c].dx + (character_control.character[c].width / 2) > brush_control.brush[b_clip].x + brush_control.brush[b_clip].width) character_control.character[c].dx = brush_control.brush[b_clip].x + brush_control.brush[b_clip].width + (character_control.character[c].width / 2);
          // box going north
          if (character_control.character[c].dy - (character_control.character[c].length / 2) < brush_control.brush[b_clip].y + brush_control.brush[b_clip].length
              && character_control.character[c].dy + (character_control.character[c].length / 2) > brush_control.brush[b_clip].y + brush_control.brush[b_clip].length) character_control.character[c].dy = brush_control.brush[b_clip].y + brush_control.brush[b_clip].length + (character_control.character[c].length / 2);
          // box going south
          if (character_control.character[c].dy - (character_control.character[c].length / 2) < brush_control.brush[b_clip].y
              && character_control.character[c].dy + (character_control.character[c].length / 2) > brush_control.brush[b_clip].y) character_control.character[c].dy = brush_control.brush[b_clip].y - (character_control.character[c].length / 2);
          }
        }

      // MOVEMENT
      for (c = 0; c < character_control.character.Count; c += 1)
        {

        if (character_control.character[c].self_velocity > character_control.character[c].max_self_velocity) character_control.character[c].self_velocity = character_control.character[c].max_self_velocity;

        // X MOVEMENT
        if (character_control.character[c].action == Action.push
            || character_control.character[c].action == Action.grab)
          {
          // match box velocity if grabbing or pushing
          character_control.character[c].self_x_velocity = brush_control.brush[character_control.character[c].brush_grab].ext_x_velocity;

          if (brush_control.brush[character_control.character[c].brush_grab].ext_x_velocity > 0)
            {

            }
          }

        else  // normal movement
          {
          // find x speed based on speed and direction
          character_control.character[c].self_x_velocity = character_control.character[c].self_velocity * Math.Cos (character_control.character[c].dir);

          // slow external force speed to zero
          if (character_control.character[c].ext_x_velocity > 0 && character_control.character[c].ext_x_velocity < .1) character_control.character[c].ext_x_velocity = 0;
          else if (character_control.character[c].ext_x_velocity < 0 && character_control.character[c].ext_x_velocity > -.1) character_control.character[c].ext_x_velocity = 0;
          else if (character_control.character[c].ext_x_velocity > 0) character_control.character[c].ext_x_velocity -= .1;
          else if (character_control.character[c].ext_x_velocity < 0) character_control.character[c].ext_x_velocity += .1;
          }

        // factor in external forces on player unless anchored by holding onto something
        character_control.character[c].net_x_velocity = character_control.character[c].self_x_velocity + character_control.character[c].ext_x_velocity;            // combine self-locomotion and external forces

        // move character
        character_control.character[c].dx += character_control.character[c].net_x_velocity;
        character_control.character[c].x = Convert.ToInt32 (character_control.character[c].dx);

        // collision
        b_clip = character_in_brush (character_control.character[c]);
        c_clip = character_in_character (c);
        f_clip = character_in_fixture (character_control.character[c], true);
        if (character_control.character[c].brush_grab > -1
            && (character_control.character[c].action == Action.grab || character_control.character[c].action == Action.push))
          {
          b_clip2 = brush_control.brush_in_brush (character_control.character[c].brush_grab);
          }
        else b_clip2 = -1;

        // gateways
        if (c == PLAYER && b_clip > -1 && brush_control.brush[b_clip].gateway > -1)
          {
          player_last_level = player_level;
          player_level = brush_control.brush[b_clip].gateway;
          enter_level ();
          b_clip = -1;
          }

        // level exits
        //else if (c == PLAYER && b_clip > -1 && brush_control.brush[b_clip].top_texture_number == (int) Texture_Type.exit_red_v_top_closed_test && red_doors_open == true) skip_area ();
        //else if (c == PLAYER && b_clip > -1 && brush_control.brush[b_clip].top_texture_number == (int) Texture_Type.exit_red_h_top_closed_test && red_doors_open == true) skip_area ();
        //else if (c == PLAYER && b_clip > -1 && brush_control.brush[b_clip].top_texture_number == exit_test_yellow && yellow_doors_open == true) skip_area ();
        //else if (c == PLAYER && b_clip > -1 && brush_control.brush[b_clip].top_texture_number == exit_test_yellow_H && yellow_doors_open == true) skip_area ();

        // walking into jump kick
        if (c_clip > -1)
          {
          if (character_control.character[c].action == Action.jump_kick)
            {
            if (character_control.character[c].is_facing_character (character_control.character[c_clip])) character_damage (c_clip, 30, 7, 0, character_control.character[c].x, character_control.character[c].y, "impact", c);
            }
          if (character_control.character[c_clip].action == Action.jump_kick)
            {
            if (character_control.character[c_clip].is_facing_character (character_control.character[c])) character_damage (c, 30, 7, 0, character_control.character[c_clip].x, character_control.character[c_clip].y, "impact", c);
            }
          }

        //else
        if (b_clip > -1 || c_clip > -1 || b_clip2 > -1 || f_clip > -1)
          {
          //if (c == PLAYER)
          character_control.character[c].dx -= character_control.character[c].net_x_velocity;
          character_control.character[c].x = Convert.ToInt32 (character_control.character[c].dx);
          }

        // Y MOVEMENT
        if (character_control.character[c].action == Action.push
            || character_control.character[c].action == Action.grab)
          {
          // match box velocity if grabbing or pushing
          character_control.character[c].self_y_velocity = brush_control.brush[character_control.character[c].brush_grab].ext_y_velocity;
          }

        else  // normal movement
          {
          // find y speed based on speed and direction
          character_control.character[c].self_y_velocity = character_control.character[c].self_velocity * Math.Sin (character_control.character[c].dir);

          // slow external force speed to zero
          if (character_control.character[c].ext_y_velocity > 0 && character_control.character[c].ext_y_velocity < .1) character_control.character[c].ext_y_velocity = 0;  // slow external force speed to zero
          else if (character_control.character[c].ext_y_velocity < 0 && character_control.character[c].ext_y_velocity > -.1) character_control.character[c].ext_y_velocity = 0;
          else if (character_control.character[c].ext_y_velocity > 0) character_control.character[c].ext_y_velocity -= .1;
          else if (character_control.character[c].ext_y_velocity < 0) character_control.character[c].ext_y_velocity += .1;
          }

        // factor in external forces on player unless anchored by holding onto something
        character_control.character[c].net_y_velocity = character_control.character[c].self_y_velocity + character_control.character[c].ext_y_velocity;  // combine self-locomotion and external forces

        // move character
        character_control.character[c].dy += character_control.character[c].net_y_velocity;
        character_control.character[c].y = Convert.ToInt32 (character_control.character[c].dy);

        // collision
        b_clip = character_in_brush (character_control.character[c]);
        f_clip = character_in_fixture (character_control.character[c], true);
        c_clip = character_in_character (c);
        if (character_control.character[c].brush_grab > -1) b_clip2 = brush_control.brush_in_brush (character_control.character[c].brush_grab);
        else b_clip2 = -1;

        // gateways
        if (c == PLAYER && b_clip > -1 && brush_control.brush[b_clip].gateway > -1)
          {
          player_last_level = player_level;
          player_level = brush_control.brush[b_clip].gateway;
          enter_level ();
          b_clip = -1;
          }

        // level exits
        else if (c == PLAYER && b_clip > -1 && brush_control.brush[b_clip].top_texture_number == (int) Texture_Type.exit_red_v_top_closed_test && red_doors_open == true) skip_area ();
        else if (c == PLAYER && b_clip > -1 && brush_control.brush[b_clip].top_texture_number == (int) Texture_Type.exit_red_h_top_closed_test && red_doors_open == true) skip_area ();

        else if (b_clip > -1 || c_clip > -1 || b_clip2 > -1 || f_clip > -1)
          {
          character_control.character[c].dy -= character_control.character[c].net_y_velocity;
          character_control.character[c].y = Convert.ToInt32 (character_control.character[c].dy);
          }

        // z movement

        // gravity is applied to self_velocity to negate upward motion
        if (character_control.character[c].self_z_velocity > -1 * gravity) character_control.character[c].self_z_velocity -= gravity_acceleration;

        character_control.character[c].net_z_velocity = character_control.character[c].self_z_velocity + character_control.character[c].ext_z_velocity;  // combine self-locomotion and external forces

        // slow external force speed to zero
        if (character_control.character[c].ext_z_velocity > 0 && character_control.character[c].ext_z_velocity < .1) character_control.character[c].ext_z_velocity = 0;
        else if (character_control.character[c].ext_z_velocity < 0 && character_control.character[c].ext_z_velocity > -.1) character_control.character[c].ext_y_velocity = 0;

        // reduce external force speed  over time
        else if (character_control.character[c].ext_z_velocity > 0) character_control.character[c].ext_z_velocity -= .1;
        else if (character_control.character[c].ext_z_velocity < 0) character_control.character[c].ext_z_velocity += .1;

        character_control.character[c].dz += character_control.character[c].net_z_velocity;
        character_control.character[c].z = Convert.ToInt32 (character_control.character[c].dz);
        b_clip = character_in_brush (character_control.character[c]);
        if (b_clip > -1)
          {
          if (character_control.character[c].net_z_velocity <= 0)
            {
            character_control.character[c].dz = brush_control.brush[b_clip].z + brush_control.brush[b_clip].height + 1;
            character_control.character[c].z = Convert.ToInt32 (character_control.character[c].dz);
            }
          else
            {
            character_control.character[c].dz -= character_control.character[c].net_z_velocity;
            character_control.character[c].z = Convert.ToInt32 (character_control.character[c].dz);
            }
          }
        f_clip = character_in_fixture (character_control.character[c], true);
        if (f_clip > -1)
          {
          if (character_control.character[c].net_z_velocity <= 0)
            {
            character_control.character[c].dz = fixture_control.fixture[f_clip].z + fixture_control.fixture[f_clip].height + 1;
            character_control.character[c].z = Convert.ToInt32 (character_control.character[c].dz);
            }
          else
            {
            character_control.character[c].dz -= character_control.character[c].net_z_velocity;
            character_control.character[c].z = Convert.ToInt32 (character_control.character[c].dz);
            }
          }
        c_clip = character_in_character (c);
        if (c_clip > -1)
          {
          character_control.character[c].dz -= character_control.character[c].net_z_velocity;
          character_control.character[c].z = Convert.ToInt32 (character_control.character[c].dz);
          }
        // falling into pit
        if (character_control.character[c].z < 0 - tilesize)
          {
          character_control.character[c].z = 0 - tilesize;
          character_control.character[c].health = 0;
          }

        // conveyor belts
        if (character_on_ground (c) && character_control.character[c].action != Action.grab && character_control.character[c].action != Action.push)
          {
          f = fixture_control.character_on_fixture (character_control.character[c]);
          if (f > -1)
            {
            if (fixture_control.fixture[f].type == Fixture_Type.conveyor_north_test && fixture_control.fixture[f].on == true)
              character_control.character[c].ext_y_velocity = conveyor_speed;

            else if (fixture_control.fixture[f].type == Fixture_Type.conveyor_south_test && fixture_control.fixture[f].on == true)
              character_control.character[c].ext_y_velocity = -1 * conveyor_speed;

            else if (fixture_control.fixture[f].type == Fixture_Type.conveyor_west_test && fixture_control.fixture[f].on == true)
              character_control.character[c].ext_x_velocity = -1 * conveyor_speed;

            else if (fixture_control.fixture[f].type == Fixture_Type.conveyor_east_test && fixture_control.fixture[f].on == true)
              character_control.character[c].ext_x_velocity = conveyor_speed;
            }
          }
        }

      // AI
      if (cycle_counter == 0)
        {
        for (c = 0; c < character_control.character.Count; c += 1)
          {
          if (c != PLAYER)
            {
            Character character = character_control.character[c];

            if (character.target_type == "none" || character.target < 0)
              {
              character.target_type = "none";
              character.target = -1;
              character.subtarget_x = -1;
              character.subtarget_y = -1;
              }

            if (character_control.active (c))
              {
              // GOALS

              // if no there's no current goal
              if (character.goal == "none")
                {
                // if hostile, attack player if visible
                if (character.hostile == true && character.action != Action.knocked_out
                    && character_control.active (PLAYER)
                    && character_control.sees_character (c, PLAYER, brush_control.brush))
                  {
                  character.attack_character (character_control.character[PLAYER], PLAYER);
                  }
                }

              // if attacking character
              else if (character.goal == "attack" && character.target_type == "character")
                {

                // if target's unconscious, do nothing
                if (!character_control.active (character.target)) character.goal = "none";
                else if (character_control.sees_character (c, character.target, brush_control.brush))
                  {
                  if (character.action == Action.none || character.action == Action.walk)
                    {

                    // close enough to punch
                    if (character_control.reach_character (c, character.target, brush_control.brush))
                      {
                      // punch in intervals, random chance
                      character.attack_counter += 1;
                      if (character.attack_counter > character.attack_delay)
                        {
                        character.attack_counter = 0;
                        if (character.action != Action.punch && rnd.Next (0, 2) == 0)
                          {
                          character_punch (c);
                          }
                        }
                      }

                    // out of arm's reach
                    else
                      {

                      // if he throws stuff & nothing's in the way
                      if (character_control.character[c].projectiles == true && character_control.sees_character (c, character_control.character[c].target, brush_control.brush))
                        {
                        if (rnd.Next (0, 300) == 0) character_throw_food (c, get_direction (character_control.character[c].x, character_control.character[c].y, character_control.character[character_control.character[c].target].x, character_control.character[character_control.character[c].target].y));
                        else
                          {
                          character_control.character[c].subtarget_x = character_control.character[character_control.character[c].target].x;
                          character_control.character[c].subtarget_y = character_control.character[character_control.character[c].target].y;
                          character_control.character[c].subtarget_z = character_control.character[character_control.character[c].target].z;
                          character_control.character[c].walk ();
                          }
                        }  // projectiles = true
                      else character_control.character[c].walk ();
                      }  // !character_reach_character
                    }  // NONE || WALKING
                  }  // character_sees_character
                // if target leaves sight
                else if (character_control.character[c].subtarget_x == character_control.character[character_control.character[c].target].x
                         && character_control.character[c].subtarget_y == character_control.character[character_control.character[c].target].y)
                  {
                  // new subtarget is last known location
                  character_control.character[c].subtarget_x = character_control.character[character_control.character[c].target].x;
                  character_control.character[c].subtarget_y = character_control.character[character_control.character[c].target].y;
                  }
                }

              // actions
              if (character_control.character[c].action == Action.punch)
                {
                }
              else if (character_control.character[c].action == Action.walk)
                {
                if (character_control.character[c].target_type == "object")
                  {
                  if (character_near_object (c, character_control.character[c].target))
                    {
                    character_control.character[c].self_velocity = 0;
                    character_control.character[c].action = Action.none;
                    }
                  else  // not near object, keep walking
                    {
                    continue_to_target (c);
                    }
                  }
                else if (character_control.character[c].target_type == "character")
                  {
                  if (character_control.reach_character (c, character_control.character[c].target, brush_control.brush))
                    {
                    character_control.character[c].action = Action.none;
                    character_control.character[c].self_velocity = 0;
                    }
                  else
                    {
                    continue_to_target (c);
                    }
                  }
                }
              else if (character_control.character[c].action == Action.knocked_out)
                {
                character_control.character[c].goal = "none";
                character_control.character[c].self_x_velocity = 0;
                character_control.character[c].self_y_velocity = 0;
                character_control.character[c].self_z_velocity = 0;
                }
              }
            }
          }
        }

      // ANIMATION
      for (c = 0; c < character_control.character.Count; c += 1)
        {
        // from walking to running - THIS IS NOW IN PHYSICAL_UPDATE
        //if (character_control.character[c].action == Action.walk && character_control.character[c].self_velocity > 2.5) character_control.character[c].run ();

        // from jumping to standing
        if (character_control.character[c].action == Action.jump && character_on_ground (c) && character_control.character[c].self_z_velocity <= 0) character_control.character[c].stand ();

        // from jump kicking to standing
        if (character_control.character[c].action == Action.jump_kick && character_on_ground (c) && character_control.character[c].self_z_velocity <= 0) character_control.character[c].stand ();

        // vary walk animation speed based on movement speed
        if (character_control.character[c].action == Action.walk && character_control.character[c].self_velocity != 0)
          character_control.character[c].max_anim_frame_counter = Convert.ToInt32 (character_control.character[c].walk_pixels / character_control.character[c].self_velocity);
        else
          character_control.character[c].max_anim_frame_counter = 4;//6

        character_control.character[c].anim_frame_counter += 1;
        if (character_control.character[c].anim_frame_counter >= character_control.character[c].max_anim_frame_counter) character_control.character[c].anim_frame_counter = 0;

        if (character_control.character[c].anim_frame_counter == 0)
          {
          if (character_control.character[c].blinking == true)  // blinking works independently of some animations
            {
            character_control.character[c].anim_frame_sequence += 1;
            if (character_control.character[c].anim_frame_sequence > character_control.character[c].max_anim_frame_sequence)
              {
              character_control.character[c].anim_frame_sequence = 0;
              character_control.character[c].blinking = false;
              }
            }

          if (character_control.character[c].action == Action.none)
            {
            character_control.character[c].absolute_frame = 0;
            character_control.character[c].combo = -1;
            character_control.blink (c);
            }
          else if (character_control.character[c].action == Action.walk)
            {
            character_control.character[c].next_frame ();
            character_control.blink (c);
            }
          else if (character_control.character[c].action == Action.run) character_control.character[c].next_frame ();
          else if (character_control.character[c].action == Action.skid)
            {
            if (character_control.character[c].type == Name.RICHARD) character_control.character[c].absolute_frame = 18;
            else character_control.character[c].absolute_frame = 0;
            }
          else if (character_control.character[c].action == Action.jump_kick)
            {
            //if (character_control.character[c].type == Name.RICHARD) character_control.character[c].absolute_frame = 20;
            //else character_control.character[c].absolute_frame = 0;
            }
          else if (character_control.character[c].action == Action.grab)
            {
            if (character_control.character[c].type == Name.RICHARD) character_control.character[c].absolute_frame = 26;
            else character_control.character[c].absolute_frame = 0;
            }
          else if (character_control.character[c].action == Action.push)
            {
            if (character_control.character[c].type == Name.RICHARD)
              {
              if (character_control.character[c].absolute_frame >= 27 && character_control.character[c].absolute_frame < 34) character_control.character[c].absolute_frame += 1;
              else character_control.character[c].absolute_frame = 27;
              }
            else character_control.character[c].absolute_frame = 0;
            }
          else if (character_control.character[c].action == Action.punch)
            {
            if (character_control.character[c].self_velocity != 0) character_control.character[c].self_velocity = 0;  // stop walking after step forward

            if (character_control.character[c].type == Name.RICHARD)
              {
              if (character_control.character[c].anim_frame_sequence < punch_rest_delay)
                {
                if (character_control.character[c].combo == 1) character_control.character[c].absolute_frame = 37;
                else if (character_control.character[c].combo == 2) character_control.character[c].absolute_frame = 38;
                else if (character_control.character[c].combo == 3) character_control.character[c].absolute_frame = 39;
                else character_control.character[c].absolute_frame = 36;
                }
              else if (character_control.character[c].anim_frame_sequence >= punch_rest_delay) character_control.character[c].absolute_frame = 35;
              }
            else if (character_control.character[c].type == Name.RICHARDS_DAD) character_control.character[c].absolute_frame = 10;
            else if (character_control.character[c].type == Name.RETARD)
              {
              if (character_control.character[c].absolute_frame < 16 || character_control.character[c].absolute_frame > 19) character_control.character[c].absolute_frame = 16;
              else character_control.character[c].absolute_frame += 1;
              //if (character_control.character[c].anim_frame_sequence < punch_rest_delay)
              //  {
              //  if (character_control.character[c].combo == 1) character_control.character[c].absolute_frame = 17;
              //  else if (character_control.character[c].combo == 2) character_control.character[c].absolute_frame = 18;
              //  else character_control.character[c].absolute_frame = 16;
              //  }
              //else if (character_control.character[c].anim_frame_sequence >= punch_rest_delay) character_control.character[c].absolute_frame = 35;
              }
            else if (character_control.character[c].type == Name.THROWING_RETARD)
              {
              if (character_control.character[c].anim_frame_sequence < punch_rest_delay)
                {
                if (character_control.character[c].combo == 1) character_control.character[c].absolute_frame = 37;
                else if (character_control.character[c].combo == 2) character_control.character[c].absolute_frame = 38;
                else if (character_control.character[c].combo == 3) character_control.character[c].absolute_frame = 39;
                else character_control.character[c].absolute_frame = 36;
                }
              else if (character_control.character[c].anim_frame_sequence >= punch_rest_delay) character_control.character[c].absolute_frame = 35;
              }
            else character_control.character[c].absolute_frame = 0;

            // move to next frame in sequence (unless waiting for player to release attack button)
            if (c != PLAYER || character_control.character[c].anim_frame_sequence >= punch_rest_delay) character_control.character[c].anim_frame_sequence += 1;
            if (character_control.character[c].anim_frame_sequence >= character_control.character[c].max_anim_frame_sequence) character_control.character[c].action = Action.none;
            }
          else if (character_control.character[c].action == Action.hurt1 || character_control.character[c].action == Action.hurt2)
            {
            if (character_control.character[c].type == Name.RICHARD) character_control.character[c].absolute_frame = 21;
            else if (character_control.character[c].type == Name.RETARD) character_control.character[c].absolute_frame = 11;
            else if (character_control.character[c].type == Name.THROWING_RETARD) character_control.character[c].absolute_frame = 21;
            else character_control.character[c].absolute_frame = 0;
            character_control.character[c].anim_frame_sequence += 1;
            if (character_control.character[c].anim_frame_sequence >= character_control.character[c].max_anim_frame_sequence) character_control.character[c].action = Action.none;
            }
          else if (character_control.character[c].action == Action.knocked_out)
            {
            if (character_control.character[c].type == Name.RICHARD) character_control.character[c].absolute_frame = 25;
            else if (character_control.character[c].type == Name.RETARD)
              {
              if (character_on_ground (c)) character_control.character[c].absolute_frame = 13;
              else character_control.character[c].absolute_frame = 12;
              }
            else if (character_control.character[c].type == Name.THROWING_RETARD) character_control.character[c].absolute_frame = 25;
            else character_control.character[c].absolute_frame = 0;
            }
          else if (character_control.character[c].action == Action.superpunch)
            {
            if (character_control.character[c].type == Name.RICHARD) character_control.character[c].absolute_frame = 36;
            else character_control.character[c].absolute_frame = 0;
            }
          else if (character_control.character[c].action == Action.flamethrower)
            {
            if (character_control.character[c].type == Name.RICHARD) character_control.character[c].absolute_frame = 36;
            else character_control.character[c].absolute_frame = 0;
            particle_flamethrower (character_control.character[PLAYER].x, character_control.character[PLAYER].y, character_control.character[PLAYER].z + Convert.ToInt32 (character_control.character[PLAYER].height * .75), Convert.ToInt32 (MathHelper.ToDegrees (Convert.ToSingle (character_control.character[PLAYER].dir))), "character", c);
            }
          else if (character_control.character[c].action == Action.freeze_ray)
            {
            if (character_control.character[c].type == Name.RICHARD) character_control.character[c].absolute_frame = 36;
            else character_control.character[c].absolute_frame = 0;
            particle_freeze_ray (character_control.character[PLAYER].x, character_control.character[PLAYER].y, character_control.character[PLAYER].z + Convert.ToInt32 (character_control.character[PLAYER].height * .75), Convert.ToInt32 (MathHelper.ToDegrees (Convert.ToSingle (character_control.character[PLAYER].dir))), "character", c);
            }

          else character_control.character[c].absolute_frame = 0;
          }
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void begin_special_attack ()
      {
      if (character_control.character[PLAYER].shirt == (int)Object_Type.shirt_yellow) particle_superpunch(character_control.character[PLAYER].x, character_control.character[PLAYER].y, character_control.character[PLAYER].z + Convert.ToInt32(character_control.character[PLAYER].height * .75), Convert.ToInt32(MathHelper.ToDegrees(Convert.ToSingle(character_control.character[PLAYER].dir))));
      else if (character_control.character[PLAYER].shirt == (int)Object_Type.shirt_red) particle_flamethrower(character_control.character[PLAYER].x, character_control.character[PLAYER].y, character_control.character[PLAYER].z + Convert.ToInt32(character_control.character[PLAYER].height * .75), Convert.ToInt32(MathHelper.ToDegrees(Convert.ToSingle(character_control.character[PLAYER].dir))), "character", PLAYER);
      else if (character_control.character[PLAYER].shirt == (int)Object_Type.shirt_white) particle_freeze_ray(character_control.character[PLAYER].x, character_control.character[PLAYER].y, character_control.character[PLAYER].z + Convert.ToInt32(character_control.character[PLAYER].height * .75), Convert.ToInt32(MathHelper.ToDegrees(Convert.ToSingle(character_control.character[PLAYER].dir))), "character", PLAYER);
      }

    ////////////////////////////////////////////////////////////////////////////////

    void continue_special_attack()
      {
      if (character_control.character[PLAYER].shirt == (int)Object_Type.shirt_yellow) character_control.character[PLAYER].action = Action.superpunch;
      else if (character_control.character[PLAYER].shirt == (int)Object_Type.shirt_red) character_control.character[PLAYER].action = Action.flamethrower;
      else if (character_control.character[PLAYER].shirt == (int)Object_Type.shirt_white) character_control.character[PLAYER].action = Action.freeze_ray;
      }

    ////////////////////////////////////////////////////////////////////////////////

    void end_special_attack()
      {
      if (character_control.character[PLAYER].action == Action.superpunch || character_control.character[PLAYER].action == Action.freeze_ray
          || character_control.character[PLAYER].action == Action.flamethrower) character_control.character[PLAYER].action = Action.none;
      }

    ////////////////////////////////////////////////////////////////////////////////

    void character_throw_food(int c, double dir)
      {
      int r = 0;

      //if (rock_color == 0)
      r = add_object((int)Object_Type.rock, character_control.character[c].x, character_control.character[c].y, character_control.character[c].z + Convert.ToInt32(character_control.character[c].height / 2));
      //else if (rock_color == 1) r = add_object((int)Object_Type.ROCK_brown, character_control.character[c].x, character_control.character[c].y, character_control.character[c].z + Convert.ToInt32(character_control.character[c].height / 2));
      //else if (rock_color == 2) r = add_object((int)Object_Type.ROCK_red, character_control.character[c].x, character_control.character[c].y, character_control.character[c].z + Convert.ToInt32(character_control.character[c].height / 2));
      //else if (rock_color == 3) r = add_object((int)Object_Type.ROCK_white, character_control.character[c].x, character_control.character[c].y, character_control.character[c].z + Convert.ToInt32(character_control.character[c].height / 2));
      //rock_color += 1;
      //if (rock_color > 3) rock_color = 0;

      if (r > -1)
        {
        object_control.obj[r].dir = dir;
        object_control.obj[r].velocity = 7;// 8;
        object_control.obj[r].z_velocity = 1.6;// 1.5;
        object_control.obj[r].source = c;
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void character_grab_brush(int c)
      {
      int distance = 0;
      int max_reach = 50;
      Vector2 reach = Vector2.Zero;
      int b;
      int abs_x_distance, abs_y_distance;
      int f_clip = -1;

      if (character_control.active(c) && character_on_ground(c))
        {
        character_control.character[c].brush_grab = -1;
        while (character_control.character[c].brush_grab == -1 && f_clip == -1 && reach.X >= 0 && reach.X < map_max_width && reach.Y >= 0 && reach.Y < map_max_length && distance <= max_reach)
          {
          reach.X = Convert.ToSingle(character_control.character[c].x + (distance * Math.Cos(character_control.character[c].dir)));
          reach.Y = Convert.ToSingle(character_control.character[c].y + (distance * Math.Sin(character_control.character[c].dir)));

          // check to make sure no fixture is between player and brush
          f_clip = fixture_control.point_in_fixture (Convert.ToInt32 (reach.X), Convert.ToInt32 (reach.Y), character_control.character[c].z);
          
          // find box character is in front of
          character_control.character[c].brush_grab = brush_control.point_in_brush(Convert.ToInt32(reach.X), Convert.ToInt32(reach.Y), character_control.character[c].z, false, false);//true);
          distance += 1;
          }

        b = character_control.character[c].brush_grab;
        // only grab if moveable (done to prevent "push any wall with running start" bug)
        if (b < 0) { }  // not a brush_control.brush
        else if (brush_control.brush[b].top_texture_number == (int)Texture_Type.box_metal_test && character_control.character[PLAYER].shirt != (int)Object_Type.shirt_yellow && character_control.character[PLAYER].shirt != (int)Object_Type.shirt_red) { }  // too heavy
        else if (brush_control.brush[b].top_texture_number == (int)Texture_Type.box_ice_test && character_control.character[PLAYER].shirt != (int)Object_Type.shirt_white) { }  // too cold
        else if (brush_control.brush[b].moveable == true)
          {
          character_control.character[c].action = Action.grab;
          character_control.character[c].blinking = false;

          grab_x_distance = character_control.character[c].x - (brush_control.brush[b].x + (brush_control.brush[b].width / 2));
          grab_y_distance = character_control.character[c].y - (brush_control.brush[b].y + (brush_control.brush[b].length / 2));
          abs_x_distance = grab_x_distance;
          abs_y_distance = grab_y_distance;
          if (abs_x_distance < 0) abs_x_distance = abs_x_distance * -1;
          if (abs_y_distance < 0) abs_y_distance = abs_y_distance * -1;

          // assign direction for box pushing
          if (abs_x_distance >= abs_y_distance && grab_x_distance < 0) character_control.character[c].grab_position = "left";
          if (abs_x_distance >= abs_y_distance && grab_x_distance >= 0) character_control.character[c].grab_position = "right";
          if (abs_x_distance < abs_y_distance && grab_y_distance < 0) character_control.character[c].grab_position = "below";
          if (abs_x_distance < abs_y_distance && grab_y_distance >= 0) character_control.character[c].grab_position = "above";

          // move character into position
          if (character_control.character[c].grab_position == "below")
            {
            character_control.character[PLAYER].dx = brush_control.brush[b].x + (brush_control.brush[b].width / 2);
            character_control.character[PLAYER].dy = brush_control.brush[b].y - (tilesize / 3);
            }
          else if (character_control.character[c].grab_position == "above")
            {
            character_control.character[PLAYER].dx = brush_control.brush[b].x + (brush_control.brush[b].width / 2);
            character_control.character[PLAYER].dy = brush_control.brush[b].y + brush_control.brush[b].length + (tilesize / 4);
            }
          else if (character_control.character[c].grab_position == "right")
            {
            character_control.character[PLAYER].dx = brush_control.brush[b].x + brush_control.brush[b].width + (tilesize / 3);
            character_control.character[PLAYER].dy = brush_control.brush[b].y + (brush_control.brush[b].length / 2);
            }
          else if (character_control.character[c].grab_position == "left")
            {
            character_control.character[PLAYER].dx = brush_control.brush[b].x - (tilesize / 3);
            character_control.character[PLAYER].dy = brush_control.brush[b].y + (brush_control.brush[b].length / 2);
            }

          // not moving yet, so destination same as position
          //character_control.character[c].push_x = character_control.character[c].x;
          //character_control.character[c].push_y = character_control.character[c].y;
          }
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void character_jump(int c)
      {
      if (character_control.active(c) && character_on_ground(c) && character_control.character[c].action != Action.grab && character_control.character[c].action != Action.push)
        {
        // jumping counteracts effect of skidding, like mario
        if (character_control.character[c].action == Action.skid)
          {
          character_control.character[c].ext_x_velocity = 0;
          character_control.character[c].ext_y_velocity = 0;
          }
        character_control.character[c].action = Action.jump;
        character_control.character[c].self_z_velocity = 6;

        if (c == PLAYER) sound_richard_jump.Play();
        character_control.character[c].switch_animation (Action.jump);
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void character_punch(int c)
      {
      if (character_control.active(c) && character_on_ground(c) && character_control.character[c].action != Action.grab && character_control.character[c].action != Action.push)
        {
        character_control.character[c].action = Action.punch;
        character_control.character[c].blinking = false;
        character_control.character[c].anim_frame_sequence = 0;
        character_control.character[c].max_anim_frame_sequence = 15;
        character_control.character[c].combo += 1;
        if (c == PLAYER)
          {
          if (character_control.character[c].combo > 3) character_control.character[c].combo = 0;
          }
        else if (character_control.character[c].combo > 1) character_control.character[c].combo = 0;
        character_control.character[c].self_velocity = 0;

        // TAKEN OUT - STEP FORWARD EXAGGERATES CLIPPING PROBLEMS
        // step forward with each fight move
        //if (character_control.character[c].combo < 2) character_control.character[c].self_velocity = 2;
        //else if (character_control.character[c].combo == 2) character_control.character[c].self_velocity = 2;
        //else if (character_control.character[c].combo == 3) character_control.character[c].self_velocity = 4;

        //for (int o = 0; o < total_objects; o += 1)
        for (int o = 0; o < object_control.obj.Count; o += 1)
          {
          if (character_near_object(c, o) && character_control.character_facing_object(character_control.character[c], object_control.obj[o]))
            {
            object_control.obj[o].dir = character_control.character[c].dir;
            object_control.obj[o].velocity = 3;// 2.5;
            object_control.obj[o].z_velocity = 1;
            }
          }

        for (int c2 = 0; c2 < character_control.character.Count; c2 += 1)
          {
          if (c2 != c && character_control.reach_character(c, c2, brush_control.brush) && character_control.character[c].is_facing_character(character_control.character[c2]))
            {
            int damage = character_control.character[c].punch_damage;
            if (character_control.character[c].shirt == (int) Object_Type.shirt_yellow) damage *= 2;
            int force = 0;
            int vertical_force = 0;

            if (character_control.character[c].combo < 2)  // punch, punch
              {
              sound_punch.Play();
              }
            else if (character_control.character[c].combo == 2)  // kick
              {
              damage = Convert.ToInt32 (damage * 1.5);
              sound_kick.Play();
              }
            else if (character_control.character[c].combo == 3)  // butt smash
              {
              damage *= 2;
              force = 5;
              vertical_force = 8;
              sound_richard_buttsmash.Play();
              }

            character_damage (c2, damage, force, vertical_force, character_control.character[c].x, character_control.character[c].y, "impact", c);
            if (c == PLAYER) color_flasher(solid_white);
            }
          }
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void character_jump_kick(int c)
      {
      if (character_control.active(c) && character_control.character[c].action != Action.grab && character_control.character[c].action != Action.push)
        {
        character_control.character[c].action = Action.jump_kick;
        //character_control.character[c].self_velocity = 0;
        character_control.character[c].switch_animation (Action.jump_kick);

        for (int o = 0; o < object_control.obj.Count; o += 1)
          {
          if (character_near_object(c, o))
            {
            object_control.obj[o].dir = character_control.character[c].dir;
            object_control.obj[o].velocity = 2.5;
            }
          }
        for (int c2 = 0; c2 < character_control.character.Count; c2 += 1)
          {
          if (c2 != c && character_control.reach_character(c, c2, brush_control.brush) && character_control.character[c].is_facing_character(character_control.character[c2]))
            {
            character_damage(c2, 30, 7, 0, character_control.character[c].x, character_control.character[c].y, "impact", c);
            sound_kick.Play();
            if (c == PLAYER) color_flasher(solid_white);
            }
          }
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void character_damage(int c, int damage, double force, double vertical_force, int x_origin, int y_origin, string damage_type, int source)
      {
      if (character_control.character[c].injury_timer == 0)
        {
        character_control.character[c].health -= damage;
        character_control.character[c].action = Action.hurt1;
        character_control.character[c].blinking = false;
        character_control.character[c].anim_frame_sequence = 0;
        character_control.character[c].max_anim_frame_sequence = 30;     // number of frames to stun character for after getting hit
        character_control.character[c].self_velocity = 0;
        character_control.character[c].apply_force_from_point(force, x_origin, y_origin);
        character_control.character[c].ext_z_velocity = vertical_force;  // goes airborn
        character_control.character[c].injury_timer = 10;
        if (damage_type == "impact") character_control.pow(c, false, false);

        // attack source of damage (like doom)
        if (c != PLAYER && character_control.active(source)) character_control.character[c].attack_character(character_control.character[source], source);

        // red flasher for player damage
        if (c == PLAYER)
          {
          //if (damage_type == "impact")
            //{
            if (character_control.character[c].health > 0) sound_richard_hurt.Play ();
            else sound_richard_death.Play ();
            //}
          color_flasher(solid_red);
          }
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void character_pow (int c, bool low, bool behind)
      {
      character_control.character[c].pow1.opacity = .75f;// 1f;
      character_control.character[c].pow1.x = -1 * (Character_Control.pow_sprite_width / 2);
      character_control.character[c].pow1.y = 0 - character_control.character[c].sprite_height;
      character_control.character[c].pow1.behind = behind;
      character_control.character[c].pow1.color = rnd.Next (0, 3);
      character_control.character[c].pow1.shape = rnd.Next (0, 5);
      }

    ////////////////////////////////////////////////////////////////////////////////

    void character_use_door (int user)
      {
      int distance = 0;
      int max_reach = 50;
      Vector2 reach = Vector2.Zero;
      int door_index = -1;
      //int abs_x_distance, abs_y_distance;

      if (character_control.active (user) && character_on_ground (user))
        {
        while (door_index == -1 && reach.X >= 0 && reach.X < map_max_width && reach.Y >= 0 && reach.Y < map_max_length && distance <= max_reach)
          {
          reach.X = Convert.ToSingle (character_control.character[user].x + (distance * Math.Cos (character_control.character[user].dir)));
          reach.Y = Convert.ToSingle (character_control.character[user].y + (distance * Math.Sin (character_control.character[user].dir)));

          // find door character is in front of
          door_index = fixture_control.point_in_fixture (Convert.ToInt32 (reach.X), Convert.ToInt32 (reach.Y), character_control.character[user].z);
          if (door_index > -1 && fixture_control.fixture[door_index].door_type == Door.none) door_index = -1;
          distance += 1;
          }

        if (door_index > -1)
          {
          fixture_control.fixture[door_index].door_toggle (character_control.character[PLAYER].keys);
          Fixture f = fixture_control.fixture[door_index];

          // check for any characters hit by door and reposition them accordingly
          for (int index = 0; index < character_control.character.Count; index += 1)
            {
            Character hitby = character_control.character[index];

            if (hitby.x + (hitby.width / 2) >= f.x && hitby.x - (hitby.width / 2) <= f.x + f.width
              && hitby.y + (hitby.length / 2) >= f.y && hitby.y - (hitby.length / 2) <= f.y + f.length
              && hitby.z + hitby.height >= f.z && hitby.z <= f.z + f.height)
              {
              if (index == user)  // character who opened the door
                {
                if (hitby.dir >= MathHelper.ToRadians (135) && hitby.dir < MathHelper.ToRadians (225))  // facing left
                  {
                  hitby.dx = f.x + (tilesize * .5);
                  hitby.dy = f.y - (tilesize * .25);
                  hitby.dz = f.z;
                  }
                else if (hitby.dir <= MathHelper.ToRadians (45) || hitby.dir > MathHelper.ToRadians (315))  // facing right
                  {
                  hitby.dx = f.x - (tilesize * .4);
                  hitby.dy = f.y + (tilesize * .5);
                  hitby.dz = f.z;
                  }
                else if (hitby.dir >= MathHelper.ToRadians (225) && hitby.dir <= MathHelper.ToRadians (315))  // facing down
                  {
                  hitby.dx = f.x + (tilesize * .5);
                  hitby.dy = f.y + (tilesize * .5);
                  hitby.dz = f.z;
                  }
                else  // facing up
                  {
                  hitby.dx = f.x + (tilesize * .6);
                  hitby.dy = f.y + (tilesize * .5);
                  hitby.dz = f.z;
                  }
                }
              }
            }
          }
        }
      }
    }
  }
