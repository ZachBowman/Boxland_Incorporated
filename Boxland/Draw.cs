using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Boxland
  {
  public partial class Boxland : Game
    {
    protected override void Draw (GameTime game_time)
      {
      //Vector2 brush_screen_draw;
      Vector2 v_draw, v_draw2, v_origin, floor_draw;
      Rectangle r_source = Rectangle.Empty;
      Rectangle r_draw;
      int tx, ty, tz;
      int b;//, b1, b2, b3, b4, b5, b6;
      int c, f;//, l
      int o;
      bool endloop, endloop2;
      int distance = 0;
      //float opacity = 1f;
      float temp_fade;
      double shadow_scale;
      Draw_Slot draw_slot;
      int min_y, max_y, min_z, max_z;
      bool all_items_drawn;
      //int wall_to_north, wall_to_south, wall_to_west, wall_to_east, wall_above;
      //int floor_to_north, floor_to_south, floor_to_west, floor_to_east;
      //bool shadow_north, shadow_south, shadow_west, shadow_east;
      //bool floor_visible;
      int bg_start_x, bg_start_y;
      //bool draw_brush;
      //int min_distance;
      string debug_string = string.Empty;

      draw_list = new List<Draw_Slot> ();
      
      if (draw_lighting)
        {
        GraphicsDevice.SetRenderTarget (light_buffer);
        GraphicsDevice.Clear (ambient_light);
        spriteBatch.Begin (SpriteSortMode.Deferred, BlendState.Additive);

        foreach (Light l in light)
          {
          if (l.on == true)
            {
            v_origin.X = light_sprite[(int) Light_Color.white].Width / 2;
            v_origin.Y = light_sprite[(int) Light_Color.white].Height / 2;
            v_draw.X = l.x + screen.scroll_x;
            v_draw.Y = y_draw_coordinate (l.y, l.z - tilesize);
            spriteBatch.Draw (light_sprite[(int) Light_Color.white], v_draw, null, l.c * l.alpha, 0f, v_origin, l.scale, SpriteEffects.None, 0);
            }
          }

        spriteBatch.End ();

        GraphicsDevice.SetRenderTarget (null);
        }

      spriteBatch.Begin (SpriteSortMode.Immediate, BlendState.AlphaBlend);

      if (game_state == Game_State.title)
        {
        GraphicsDevice.Clear (new Color (0, 255, 255, 255));

        v_draw.X = (screen.width / 2) - (title_logo_test.Width / 2);
        v_draw.Y = 100;
        spriteBatch.Draw (title_logo_test, v_draw, Color.White);

        draw_text_boxes ();

        //foreach (Text_Box box in text_control.boxes)
        //  {
        //  shape.rectangle (spriteBatch, box.box.X, box.box.Y, box.box.X + box.box.Width, box.box.Y + box.box.Height, pixel_yellow, 1f);
        //  }
        }

      else if (game_state == Game_State.game || game_state == Game_State.creation)
        {
        GraphicsDevice.Clear (Color.Black);

        if (draw_background.on)
          {
          // background layer 1
          for (bg_start_x = screen.bg1_scroll_x; bg_start_x + map.background.Width > 0; bg_start_x -= map.background.Width) { }
          for (bg_start_y = screen.bg1_scroll_y; bg_start_y + map.background.Height > 0; bg_start_y -= map.background.Height) { }
          for (v_draw.Y = bg_start_y; v_draw.Y < screen.height; v_draw.Y += map.background.Height)
            for (v_draw.X = bg_start_x; v_draw.X < screen.width; v_draw.X += map.background.Width)
              spriteBatch.Draw (map.background, v_draw, Color.White * draw_background.opacity);

          // background layer 2
          //for (bg_start_x = bg2_scroll_x; bg_start_x + test_background3.Width > 0; bg_start_x -= test_background3.Width) { }
          //for (bg_start_y = bg2_scroll_y; bg_start_y + test_background3.Height > 0; bg_start_y -= test_background3.Height) { }
          //for (v_draw.Y = bg_start_y; v_draw.Y < screen.height; v_draw.Y += test_background3.Height)
          //  for (v_draw.X = bg_start_x; v_draw.X < screen.width; v_draw.X += test_background3.Width)
          //    spriteBatch.Draw (test_background3, v_draw, Color.White);

          //for (v_draw.Y = bg1_scroll_y - map.background.Height; v_draw.Y < screen.height; v_draw.Y += map.background.Height)
          //  for (v_draw.X = bg1_scroll_x - map.background.Width; v_draw.X < screen.width; v_draw.X += map.background.Width)
          //    spriteBatch.Draw (map.background, v_draw, Color.White);

          //for (v_draw.Y = bg2_scroll_y - test_background3.Height; v_draw.Y < screen.height; v_draw.Y += test_background3.Height)
          //  for (v_draw.X = bg2_scroll_x - test_background3.Width; v_draw.X < screen.width; v_draw.X += test_background3.Width)
          //    spriteBatch.Draw (test_background3, v_draw, Color.White);
          }

        // reset draw flags
        for (b = 0; b < brush_control.brush.Count; b += 1) brush_control.brush[b].drawn = false;
        foreach (Fixture fix in fixture_control.fixture) fix.drawn = false;
        for (o = 0; o < object_control.obj.Count; o += 1) object_control.obj[o].drawn = false;
        foreach (Character ch in character_control.character) ch.drawn = false;

        // draw the floor
        if (use_floor_buffer)
          {
          if (redraw_floor_buffer)
            {
            spriteBatch.End ();
            floor_buffer = new RenderTarget2D (GraphicsDevice, map.pixel_width, map.pixel_length);
            GraphicsDevice.SetRenderTarget (floor_buffer);
            //GraphicsDevice.Clear (Color.Purple);//Black);
            spriteBatch.Begin (SpriteSortMode.Deferred, BlendState.AlphaBlend);

            redraw_floor_buffer = false;

            draw_slot.id = -1;
            draw_slot.type = "brush";

            // lowest grid elevation from back to front, then move up each layer
            // layers are half tilesize

            //l = 0;
            min_z = 0;
            max_z = (tilesize / 2) - 1;
            endloop = false;

            //while (!endloop && l < total_draw_slots)
            while (!endloop && draw_list.Count < max_draw_slots)//l < max_draw_slots)
              {
              all_items_drawn = true;
              draw_slot.id = -1;
              max_y = map_max_length;
              min_y = 0;

              for (b = 0; b < brush_control.brush.Count; b += 1)
                {
                if (brush_control.brush[b].drawn == false && brush_control.brush[b].y >= min_y && brush_control.brush[b].y <= max_y
                    && brush_control.brush[b].z >= min_z && brush_control.brush[b].z <= max_z)
                  {
                  all_items_drawn = false;
                  draw_slot.id = b;
                  draw_slot.type = "brush";
                  min_y = brush_control.brush[b].y;
                  }
                }
              if (draw_slot.id > -1)  // found item to draw
                {
                draw_list.Add (draw_slot);
                if (draw_slot.type == "brush") brush_control.brush[draw_slot.id].drawn = true;
                }
              else endloop = true;  // nothing left in this layer
              }

            foreach (Draw_Slot slot in draw_list)
              {
              if (slot.type == "brush") draw_brush (slot.id, false);
              }

            //spriteBatch.Draw (solid_red, new Rectangle (0, 0, 96, 96), new Rectangle (0, 0, 96, 96), Color.White);

            //ConvertToPremultipliedAlpha (floor_buffer, Color.Purple);

            spriteBatch.End ();
            GraphicsDevice.SetRenderTarget (null);
            spriteBatch.Begin (SpriteSortMode.Immediate, BlendState.AlphaBlend);

            draw_list = new List<Draw_Slot> ();
            }

          floor_draw.X = screen.scroll_x;
          floor_draw.Y = Convert.ToInt32 ((screen.height - map.pixel_length) - (tilesize * parallax) - tilesize + screen.scroll_y);

          spriteBatch.Draw (floor_buffer, floor_draw, Color.White);
          }

        // CREATE ORDERED LIST FOR DRAWING BRUSHES, OBJECTS & CHARACTERS

        draw_slot.id = -1;
        draw_slot.type = "brush";

        // lowest grid elevation from back to front, then move up each layer
        // layers are half tilesize

        all_items_drawn = false;

        if (use_floor_buffer)
          {
          min_z = tilesize;
          max_z = (tilesize * 3 / 2) - 1;  // with floor buffer
          }
        else
          {
          min_z = 0;
          max_z = (tilesize / 2) - 1;      // without floor buffer
          }

        while (all_items_drawn == false && max_z < map_max_height && draw_list.Count < max_draw_slots)//l < total_draw_slots)
          {
          all_items_drawn = true;
          draw_slot.id = -1;
          max_y = map_max_length;
          min_y = 0;

          for (b = 0; b < brush_control.brush.Count; b += 1)
            {
            if (brush_control.brush[b].drawn == false && brush_control.brush[b].y >= min_y && brush_control.brush[b].y <= max_y
                && brush_control.brush[b].z >= min_z && brush_control.brush[b].z <= max_z)
              {
              all_items_drawn = false;
              draw_slot.id = b;
              draw_slot.type = "brush";
              min_y = brush_control.brush[b].y;
              }
            }
          for (f = 0; f < fixture_control.fixture.Count; f += 1)
            {
            if (fixture_control.fixture[f].drawn == false && fixture_control.fixture[f].y >= min_y && fixture_control.fixture[f].y <= max_y
                && fixture_control.fixture[f].z >= min_z && fixture_control.fixture[f].z <= max_z)
              {
              all_items_drawn = false;
              draw_slot.id = f;
              draw_slot.type = "fixture";
              min_y = fixture_control.fixture[f].y;
              }
            }
          for (o = 0; o < object_control.obj.Count; o += 1)
            {
            if (object_control.obj[o].drawn == false && object_control.obj[o].y >= min_y && object_control.obj[o].y <= max_y
                && object_control.obj[o].z >= min_z && object_control.obj[o].z <= max_z)
              {
              all_items_drawn = false;
              draw_slot.id = o;
              draw_slot.type = "object";
              min_y = object_control.obj[o].y;
              }
            }
          for (c = 0; c < character_control.character.Count; c += 1)
            {
            if (character_control.character[c].drawn == false && character_control.character[c].y >= min_y && character_control.character[c].y <= max_y
              && character_control.character[c].z >= min_z && character_control.character[c].z <= max_z)
            //&& character_control.character[c].z + character_control.character[c].box_height >= min_z && character_control.character[c].z + character_control.character[c].box_height <= max_z)
              {
              all_items_drawn = false;
              draw_slot.id = c;
              draw_slot.type = "character";
              min_y = character_control.character[c].y;
              }
            }
          if (draw_slot.id > -1)  // found item to draw
            {
            draw_list.Add (draw_slot);
            if (draw_slot.type == "brush") brush_control.brush[draw_slot.id].drawn = true;
            else if (draw_slot.type == "fixture") fixture_control.fixture[draw_slot.id].drawn = true;
            else if (draw_slot.type == "object") object_control.obj[draw_slot.id].drawn = true;
            else if (draw_slot.type == "character") character_control.character[draw_slot.id].drawn = true;
            }
          else  // nothing left in this layer
            {
            min_z += tilesize / 2;
            max_z += tilesize / 2;
            all_items_drawn = false;
            }
          }

        foreach (Draw_Slot slot in draw_list)
          {
          if (slot.type == "brush" && brush_on_screen (slot.id)) draw_brush (slot.id, true);

          else
          if (slot.type == "fixture")
            {
            fixture_control.draw (slot.id, spriteBatch, screen, parallax);

            //brush_screen_draw.Y = Convert.ToInt32 ((screen.height - brush_control.brush[b].y - brush_control.brush[b].length) - (brush_control.brush[b].z * parallax) - (brush_control.brush[b].height * parallax) + screen.scroll_y);

            f = slot.id;
            v_draw.X = fixture_control.fixture[f].x + screen.scroll_x;
            v_draw.Y = Convert.ToInt32 (screen.height - fixture_control.fixture[f].y - fixture_control.fixture[f].length - (fixture_control.fixture[f].z * parallax) - (fixture_control.fixture[f].height * parallax) + screen.scroll_y);
            //v_draw.Y = y_draw_coordinate (fixture_control.fixture[f].y, fixture_control.fixture[f].z);

            // fixture effects
            if (fixture_control.fixture[f].type == Fixture_Type.wires_southeast_test && fixture_control.fixture[f].powered == true)
              {
              //spriteBatch.Draw (wires_southeast_powered_test, v_draw, Color.White);
              }
            else if (fixture_control.fixture[f].type == Fixture_Type.vending_yellow_test)
              {
              // glow from lit machine
              //r_draw.Width = light_sprite[(int) Light_Color.yellow].Width / 2;
              //r_draw.Height = light_sprite[(int) Light_Color.yellow].Height / 2;
              //r_draw.X = Convert.ToInt32 (v_draw.X + (fixture_control.fixture[f].width / 2) - (r_draw.Width / 2));
              //r_draw.Y = Convert.ToInt32 (v_draw.Y + (fixture_control.fixture[f].height / 2) - (r_draw.Height / 2));
              //spriteBatch.Draw (light_sprite[(int) Light_Color.yellow], r_draw, Color.White * .25f);
              }
            }
          else if (slot.type == "object" && object_control.obj[slot.id].destroyed == false)
            {
            o = slot.id;

            // draw floor shadow
            tx = object_control.obj[o].x;
            ty = object_control.obj[o].y;
            tz = object_control.obj[o].z;

            endloop = false;
            while (endloop == false && tz >= 0)
              {
              endloop2 = false;
              b = 0;
              f = 0;
              while (endloop2 == false && b < brush_control.brush.Count)
                {
                if (tx >= brush_control.brush[b].x && tx <= brush_control.brush[b].x + brush_control.brush[b].width &&
                    ty >= brush_control.brush[b].y && ty <= brush_control.brush[b].y + brush_control.brush[b].length &&
                    tz == brush_control.brush[b].z + brush_control.brush[b].height) { endloop2 = true; endloop = true; }
                b += 1;
                }
              while (endloop2 == false && f < fixture_control.fixture.Count)//total_fixtures)
                {
                if (tx >= fixture_control.fixture[f].x && tx <= fixture_control.fixture[f].x + fixture_control.fixture[f].width &&
                    ty >= fixture_control.fixture[f].y && ty <= fixture_control.fixture[f].y + fixture_control.fixture[f].length &&
                    tz == fixture_control.fixture[f].z + fixture_control.fixture[f].height) { endloop2 = true; endloop = true; }
                f += 1;
                }
              tz -= 1;
              }

            distance = object_control.obj[o].z - tz;
            temp_fade = Convert.ToSingle (.3f - (distance / 300f));
            if (temp_fade < .1f) temp_fade = .1f;
            shadow_scale = 1.0 + (distance / 128.0);

            r_source.X = 0;
            r_source.Y = 0;
            r_source.Width = object_control.object_sprite[object_control.obj[o].type, 0].Width;
            r_source.Height = object_control.object_sprite[object_control.obj[o].type, 0].Height;
            r_draw.X = tx + screen.scroll_x;
            r_draw.Y = Convert.ToInt32 ((screen.height - ty) - (tz * parallax) + screen.scroll_y - 3);
            r_draw.Width = Convert.ToInt32 (object_control.object_sprite[object_control.obj[o].type, 0].Width * shadow_scale);
            r_draw.Height = Convert.ToInt32 (object_control.object_sprite[object_control.obj[o].type, 0].Height / 4 * shadow_scale);
            v_origin.X = object_control.object_sprite[object_control.obj[o].type, 0].Width / 2;
            v_origin.Y = 0;
            spriteBatch.Draw (object_control.object_sprite[object_control.obj[o].type, object_control.obj[o].skin], r_draw, r_source, Color.Black * temp_fade, MathHelper.ToRadians (0), v_origin, SpriteEffects.FlipVertically, 0);

            // draw object
            v_draw.X = object_control.obj[o].x - (object_control.obj[o].width / 2) + screen.scroll_x;
            v_draw.Y = Convert.ToInt32 ((screen.height - object_control.obj[o].y) - object_control.obj[o].height - (object_control.obj[o].z * parallax) + screen.scroll_y);
            spriteBatch.Draw (object_control.object_sprite[object_control.obj[o].type, object_control.obj[o].skin], v_draw, Color.White);

            if (debug)
              {
              v_draw2.X = v_draw.X + (object_control.object_sprite[object_control.obj[o].type, 0].Width / 2) - 5;
              v_draw2.Y = v_draw.Y - 15;
              spriteBatch.DrawString (debug_font, Convert.ToString (o), v_draw2, Color.White);
              }
            }
          else if (slot.type == "character")
            {
            //if (character_control.character[slot.id].type == Name.SECRETARY)
              //{
              draw_character (character_control.character[slot.id]);
              //}
            }
          }

        // particles
        //for (p = 0; p < max_effects; p += 1)
        //  if (particle_effect[p].active == true) particle_effect[p].draw (spriteBatch, screen.width, screen.height, screen.scroll_x, screen.scroll_y);
        particle_engine.draw (spriteBatch, screen.width, screen.height, screen.scroll_x, screen.scroll_y);

        draw_text_boxes ();

        // richard's pain and sorrow
        if (character_control.character[PLAYER].health < 75)
          {
          r_draw.X = 0;
          r_draw.Y = 0;
          r_draw.Width = screen.width;
          r_draw.Height = screen.height;
          spriteBatch.Draw (effect_pain, r_draw, Color.White * ((75f - character_control.character[PLAYER].health) / 75f));
          }
        }  // if game_state == Game_State.game

      // color flash / fade
      if (color_flash.activated == true) // fade has been started
        {
        color_flash.fade (spriteBatch, color_flash_sprite, Vector2.Zero);
        }

      Fade_Control (game_time);

      // menu
      if (game_menu == true)
        {
        r_source.X = 0;
        r_source.Y = 0;
        r_source.Width = menu_width;
        r_source.Height = menu_height;

        r_draw.X = menu_x;// (screen.width - menu_width) / 2;
        r_draw.Y = menu_y;// (screen.height - menu_height) / 2;
        r_draw.Width = menu_width;
        r_draw.Height = menu_height;

        spriteBatch.Draw (solid_black, r_draw, r_source, Color.White * .7f);

        if (menu_screen == "main")
          {
          spriteBatch.Draw (menu_exit_test, menu_exit_v, Color.White);
          }
        }

      spriteBatch.End ();

      if (game_state == Game_State.game && draw_lighting)
        {
        BlendState blendState = new BlendState ();
        blendState.ColorSourceBlend = Blend.DestinationColor;
        blendState.ColorDestinationBlend = Blend.SourceColor;
        spriteBatch.Begin (SpriteSortMode.Immediate, blendState);
        spriteBatch.Draw (light_buffer, Vector2.Zero, Color.White);
        spriteBatch.End ();
        }

      // debugging stuff
      if (debug)
        {
        spriteBatch.Begin (SpriteSortMode.Immediate, BlendState.AlphaBlend);

        debug_string += "game state: ";
        if (game_state == Game_State.game) debug_string += "Game";//spriteBatch.DrawString (debug_font, "Game", debug_pos, Color.Yellow);
        else if (game_state == Game_State.creation) debug_string += "Creation";// spriteBatch.DrawString (debug_font, "Creation", debug_pos, Color.Yellow);
        debug_string += "\n";

        if (game_state == Game_State.game)
          {
          debug_string += "fps: " + Convert.ToString (fps) + "\n";
          //debug_string += "lights: " + Convert.ToString (light.Count) + "\n";

          // controller
          /*
          "left stick x: " + Convert.ToString (controller.ThumbSticks.Left.X)
          "left stick y: " + Convert.ToString (controller.ThumbSticks.Left.Y)
          */

          // map
          //debug_string += "map.pixel_length: " + map.pixel_length + "\n";

          //player
          debug_string += "world x: " + character_control.character[PLAYER].x + "\n";
          debug_string += "world y: " + character_control.character[PLAYER].y + "\n";
          debug_string += "world z: " + character_control.character[PLAYER].z + "\n";
          //debug_string += "left alt: " + key_leftalt + "\n";
          debug_string += "brush grab: " + Convert.ToString (character_control.character[PLAYER].brush_grab) + "\n";
          debug_string += "grab position: " + character_control.character[PLAYER].grab_position + "\n";
          debug_string += "direction: " + Convert.ToString (MathHelper.ToDegrees (Convert.ToSingle (character_control.character[PLAYER].dir))) + "\n";
          //debug_string += "last_dir: " + Convert.ToString (MathHelper.ToDegrees (Convert.ToSingle (character_control.character[PLAYER].last_dir))) + "\n";
          debug_string += "action: " + character_control.character[PLAYER].action + "\n";
          debug_string += "screen x: " + Convert.ToString (character_control.character[PLAYER].x + screen.scroll_x) + "\n";
          debug_string += "screen y: " + Convert.ToString (screen.height - character_control.character[PLAYER].y - (character_control.character[PLAYER].z / 2) + screen.scroll_y) + "\n";
          debug_string += "runboost: " + character_control.character[PLAYER].runboost + "\n";
          //debug_string += "skid_counter: " + character_control.character[PLAYER].skid_counter + "\n";
          debug_string += "key code: " + last_key_pressed + "\n";

          //debug_string += "scroll_x: " + screen.scroll_x + "\n";
          //debug_string += "Scroll_y: " + screen.scroll_y + "\n";

          //debug_string += "mouse x: " + Convert.ToString (mouse_current.X) + "\n";
          //debug_string += "mouse y: " + Convert.ToString (mouse_current.Y) + "\n";

          // more player stuff
          //"player id: " + Convert.ToString (PLAYER)
          //"player active: " + Convert.ToString (character_control.active (PLAYER))

          //"total characters: " + Convert.ToString (total_characters)

          //if (total_characters > 0)
          //  {
          //  "char 0 active: " + Convert.ToString (character_control.active (0))
          //  "char 0 name: " + Convert.ToString (character_control.character[0].name)
          //  }

          //if (total_characters > 1)
          //  {
          //  spriteBatch.DrawString (debug_font, "char 1 active: ", Convert.ToString (character_control.active (1)), debug_pos, Color.Yellow);
          //  spriteBatch.DrawString (debug_font, "char 1 name: ", Convert.ToString (character_control.character[1].name), debug_pos, Color.Yellow);
          //  }

          //if (total_characters > 2)
          //  {
          //  spriteBatch.DrawString (debug_font, "char 2 active: ", Convert.ToString (character_control.active (2)), debug_pos, Color.Yellow);
          //  spriteBatch.DrawString (debug_font, "char 2 name: ", Convert.ToString (character_control.character[2].name), debug_pos, Color.Yellow);
          //  }
          //*/

          /*
          // objects
          spriteBatch.DrawString (debug_font, "max obj: ", Convert.ToString (max_objects), debug_pos, Color.Yellow);
          spriteBatch.DrawString (debug_font, "total obj: ", Convert.ToString (total_objects), debug_pos, Color.Yellow);

          // object 0
          spriteBatch.DrawString (debug_font, "obj[0].x: ", Convert.ToString (obj[0].x), debug_pos, Color.Yellow);
          spriteBatch.DrawString (debug_font, "obj[0].y: ", Convert.ToString (obj[0].y), debug_pos, Color.Yellow);
          spriteBatch.DrawString (debug_font, "obj[0].z: ", Convert.ToString (obj[0].z), debug_pos, Color.Yellow);
          spriteBatch.DrawString (debug_font, "obj[0].destroyed: ", Convert.ToString (obj[0].destroyed), debug_pos, Color.Yellow);

          // object 1
          spriteBatch.DrawString (debug_font, "obj[1].x: ", Convert.ToString (obj[1].x), debug_pos, Color.Yellow);
          spriteBatch.DrawString (debug_font, "obj[1].y: ", Convert.ToString (obj[1].y), debug_pos, Color.Yellow);
          spriteBatch.DrawString (debug_font, "obj[1].z: ", Convert.ToString (obj[1].z), debug_pos, Color.Yellow);
          spriteBatch.DrawString (debug_font, "obj[1].destroyed: ", Convert.ToString (obj[1].destroyed), debug_pos, Color.Yellow);

          // object 2
          spriteBatch.DrawString (debug_font, "obj[2].x: ", Convert.ToString (obj[2].x), debug_pos, Color.Yellow);
          spriteBatch.DrawString (debug_font, "obj[2].y: ", Convert.ToString (obj[2].y), debug_pos, Color.Yellow);
          spriteBatch.DrawString (debug_font, "obj[2].z: ", Convert.ToString (obj[2].z), debug_pos, Color.Yellow);
          spriteBatch.DrawString (debug_font, "obj[2].destroyed: ", Convert.ToString (obj[2].destroyed), debug_pos, Color.Yellow);
          */

          //    // retard
          //    /*
          //    spriteBatch.DrawString (debug_font, "direction: ", Convert.ToString (MathHelper.ToDegrees (Convert.ToSingle (character_control.character[0].dir))), debug_pos, Color.Yellow);
          //    spriteBatch.DrawString (debug_font, "action: ", debug_pos, Color.Yellow);
          //    if (character_control.character[0].action == Action.none) spriteBatch.DrawString (debug_font, "none", debug_pos, Color.Yellow);
          //    else if (character_control.character[0].action == Action.stand) spriteBatch.DrawString (debug_font, Action.stand, debug_pos, Color.Yellow);
          //    else if (character_control.character[0].action == Action.walk) spriteBatch.DrawString (debug_font, Action.walk, debug_pos, Color.Yellow);
          //    else if (character_control.character[0].action == Action.run) spriteBatch.DrawString (debug_font, Action.run, debug_pos, Color.Yellow);
          //    else if (character_control.character[0].action == Action.jump) spriteBatch.DrawString (debug_font, Action.jump, debug_pos, Color.Yellow);
          //    else if (character_control.character[0].action == Action.grab) spriteBatch.DrawString (debug_font, Action.grab, debug_pos, Color.Yellow);
          //    else if (character_control.character[0].action == Action.push) spriteBatch.DrawString (debug_font, Action.push, debug_pos, Color.Yellow);
          //    spriteBatch.DrawString (debug_font, "screen x: ", Convert.ToString (character_control.character[0].x + scroll_x), debug_pos, Color.Yellow);
          //    spriteBatch.DrawString (debug_font, "screen y: ", Convert.ToString (screen.height - character_control.character[0].y - (character_control.character[0].z / 2) + scroll_y), debug_pos, Color.Yellow);
          //    spriteBatch.DrawString (debug_font, "mouse x: ", Convert.ToString (mouse_current.X), debug_pos, Color.Yellow);
          //    spriteBatch.DrawString (debug_font, "mouse y: ", Convert.ToString (mouse_current.Y), debug_pos, Color.Yellow);
          //    */

          //    // brushes
          //    spriteBatch.DrawString (debug_font, "brush_grab: ", debug_pos, Color.Yellow);
          //    spriteBatch.DrawString (debug_font, Convert.ToString (character_control.character[PLAYER].brush_grab), debug_pos, Color.Yellow);

              if (character_control.character[PLAYER].brush_grab > -1)
                {
                debug_string += "brush[grab].destination_y: " + brush_control.brush[character_control.character[PLAYER].brush_grab].destination_y + "\n";
                //debug_string += "brush[grab].x: " + Convert.ToString (brush_control.brush[character_control.character[PLAYER].brush_grab].x) + "\n";
                debug_string += "brush[grab].y: " + Convert.ToString (brush_control.brush[character_control.character[PLAYER].brush_grab].y) + "\n";
                debug_string += "brush[grab].moving: " + Convert.ToString (brush_control.brush[character_control.character[PLAYER].brush_grab].moving) + "\n";
                debug_string += "brush[grab].moving_north: " + Convert.ToString (brush_control.brush[character_control.character[PLAYER].brush_grab].moving_north) + "\n";

          //      spriteBatch.DrawString (debug_font, "brush_control.brush[grab].moving_south: ", debug_pos, Color.Yellow);
          //      spriteBatch.DrawString (debug_font, Convert.ToString (brush_control.brush[character_control.character[PLAYER].brush_grab].moving_south), debug_pos, Color.Yellow);

          //      spriteBatch.DrawString (debug_font, "brush_control.brush[grab].moving_west: ", debug_pos, Color.Yellow);
          //      spriteBatch.DrawString (debug_font, Convert.ToString (brush_control.brush[character_control.character[PLAYER].brush_grab].moving_west), debug_pos, Color.Yellow);

          //      spriteBatch.DrawString (debug_font, "brush_control.brush[grab].moving_east: ", debug_pos, Color.Yellow);
          //      spriteBatch.DrawString (debug_font, Convert.ToString (brush_control.brush[character_control.character[PLAYER].brush_grab].moving_east), debug_pos, Color.Yellow);
                }

          //    // scroll box
          //    //shape.rectangle (spriteBatch, scroll_border.X, scroll_border.Y, scroll_border.X + scroll_border.Width, scroll_border.Y + scroll_border.Height, pixel_yellow, .5f);

          // inventory
          debug_string += "keys: " + Convert.ToString (character_control.character[PLAYER].keys) + "\n";

          // level
          debug_string += "level: " + Convert.ToString (player_level) + "\n";
          }

        else if (game_state == Game_State.creation)
          {
          //    spriteBatch.DrawString (debug_font, "moves: ", debug_pos, Color.Yellow);
          //    spriteBatch.DrawString (debug_font, Convert.ToString (creation_moves), debug_pos, Color.Yellow);

          //    spriteBatch.DrawString (debug_font, "max moves: ", debug_pos, Color.Yellow);
          //    spriteBatch.DrawString (debug_font, Convert.ToString (creation_max_moves), debug_pos, Color.Yellow);

          //    spriteBatch.DrawString (debug_font, "direction: ", debug_pos, Color.Yellow);
          //    spriteBatch.DrawString (debug_font, Convert.ToString (creation_direction), debug_pos, Color.Yellow);

          //    spriteBatch.DrawString (debug_font, "last direction: ", debug_pos, Color.Yellow);
          //    spriteBatch.DrawString (debug_font, Convert.ToString (creation_last_direction), debug_pos, Color.Yellow);

          //    spriteBatch.DrawString (debug_font, "distance: ", debug_pos, Color.Yellow);
          //    spriteBatch.DrawString (debug_font, Convert.ToString (creation_distance), debug_pos, Color.Yellow);
          }

        shape.rectangle_filled (spriteBatch, 0, 0, 300, 350, pixel_black, .6f);
        spriteBatch.DrawString (debug_font, debug_string, new Vector2 (30, 10), Color.White);
        spriteBatch.End ();
        }

      fps_counter += 1;
      }

    //////////////////////////////////////////////////////////////////////////////////

    void draw_brush (int b, bool with_scrolling)
      {
      Vector2 brush_screen_draw, v_draw;
      Rectangle r_source, r_draw;
      bool draw_brush;
      int b1, b2, b3, b4, b5, b6;

      if (with_scrolling)
        {
        brush_screen_draw.X = brush_control.brush[b].x + screen.scroll_x;
        brush_screen_draw.Y = Convert.ToInt32 ((screen.height - brush_control.brush[b].y - brush_control.brush[b].length) - (brush_control.brush[b].z * parallax) - (brush_control.brush[b].height * parallax) + screen.scroll_y);
        }
      else
        {
        brush_screen_draw.X = brush_control.brush[b].x;
        brush_screen_draw.Y = map.pixel_length - brush_control.brush[b].y;
        }

      // background brush_control.texture
      if (brush_control.brush[b].background_texture > -1) spriteBatch.Draw (brush_control.tile[brush_control.brush[b].background_texture].texture, brush_screen_draw, new Rectangle (0, 0, tilesize, tilesize), Color.White);

      if (draw_walls == false
          && (brush_control.brush[b].top_texture_number == (int) Texture_Type.brick_grey_test
          || brush_control.brush[b].top_texture_number == (int) Texture_Type.brick_red
          || brush_control.brush[b].top_texture_number == (int) Texture_Type.brick_white_test
          || brush_control.brush[b].top_texture_number == (int) Texture_Type.drywall_mint_top_test
          || brush_control.brush[b].top_texture_number == (int) Texture_Type.drywall_purple_top_test
          || brush_control.brush[b].top_texture_number == (int) Texture_Type.drywall_tan_top_test
          || brush_control.brush[b].top_texture_number == (int) Texture_Type.drywall_yellow_top_test
          || brush_control.brush[b].top_texture_number == (int) Texture_Type.metal_black_test
          || brush_control.brush[b].top_texture_number == (int) Texture_Type.metal_blue_top_test
          || brush_control.brush[b].top_texture_number == (int) Texture_Type.metal_mint_top_test))
      { }  // draw nothing
      else if (draw_boxes == false
        && (brush_control.brush[b].top_texture_number == (int) Texture_Type.box_wood
        || brush_control.brush[b].top_texture_number == (int) Texture_Type.box_metal_test)) { }  // draw nothing

      // animated surfaces (doors, buttons, etc)
      //else if (brush_control.brush[b].top_texture_number == door_test_h_green && green_doors_open == true) spriteBatch.Draw (brush_control.texture[door_test_h_green_open], brush_screen_draw, Color.White);
      //else if (brush_control.brush[b].top_texture_number == door_test_h_yellow && yellow_doors_open == true) spriteBatch.Draw (brush_control.texture[door_test_h_yellow_open], brush_screen_draw, Color.White);
      else if (brush_control.brush[b].top_texture_number == (int) Texture_Type.switch_green_test && green_doors_open == true) spriteBatch.Draw (brush_control.tile[(int) Texture_Type.switch_green_down_test].texture, brush_screen_draw, Color.White);
      else if (brush_control.brush[b].top_texture_number == (int) Texture_Type.switch_red_test && red_doors_open == true) spriteBatch.Draw (brush_control.tile[(int) Texture_Type.switch_red_down_test].texture, brush_screen_draw, Color.White);
      else if (brush_control.brush[b].top_texture_number == (int) Texture_Type.switch_yellow_test && yellow_doors_open == true) spriteBatch.Draw (brush_control.tile[(int) Texture_Type.switch_yellow_down_test].texture, brush_screen_draw, Color.White);
      else if (brush_control.brush[b].top_texture_number == (int) Texture_Type.switch_blue_test && blue_doors_open == true) spriteBatch.Draw (brush_control.tile[(int) Texture_Type.switch_blue_down_test].texture, brush_screen_draw, Color.White);

      // large, one-piece units do not get tiled
      //else if (brush_control.brush[b].top_texture_number == big_machine_test || brush_control.brush[b].top_texture_number == WARNING_SIGN_test1)
      else if (brush_control.brush[b].top_texture_number > (int) Texture_Type.single_piece && brush_control.brush[b].top_texture_number != (int) Texture_Type.invisible)
        {
        spriteBatch.Draw (brush_control.tile[brush_control.brush[b].top_texture_number].texture, brush_screen_draw, Color.White);
        }

      // normal brushes use texture sheets to create seamless, tiled walls and floors
      else
        {
        // bottom of brush (certain transparent brushes only)
        if (brush_control.brush[b].top_texture_number == (int) Texture_Type.box_ice_test)
          {
          r_source.X = brush_control.brush[b].top_texture_offset_x;
          r_source.Y = brush_control.brush[b].top_texture_offset_y;
          r_source.Width = tilesize;
          r_source.Height = tilesize;

          r_draw.X = Convert.ToInt32 (brush_screen_draw.X);
          r_draw.Y = Convert.ToInt32 ((brush_screen_draw.Y) + (tilesize * parallax));
          r_draw.Width = tilesize;
          r_draw.Height = tilesize;
          spriteBatch.Draw (brush_control.tile[brush_control.brush[b].top_texture_number].texture, r_draw, r_source, Color.White * .9f);
          }

        // top of brush

        // only draw if visible (not completely covered by a brush above, unless that brush is transparent)
        draw_brush = false;

        if (brush_control.brush_above_brush (brush_control.brush[b], false, false) == -1) draw_brush = true;

        if (draw_brush == true)
          {
          r_source.X = brush_control.brush[b].top_texture_offset_x;
          r_source.Y = brush_control.brush[b].top_texture_offset_y;
          r_source.Width = tilesize;
          r_source.Height = tilesize;

          r_draw.X = Convert.ToInt32 (brush_screen_draw.X);
          r_draw.Y = Convert.ToInt32 (brush_screen_draw.Y);
          r_draw.Width = tilesize;
          r_draw.Height = tilesize;

          // background brush_control.texture
          if (brush_control.brush[b].background_texture > -1) spriteBatch.Draw (brush_control.tile[brush_control.brush[b].background_texture].texture, r_draw, new Rectangle (0, 0, tilesize, tilesize), Color.White);

          if (brush_control.brush[b].top_texture_number == (int) Texture_Type.invisible) { }
          else if (brush_control.brush[b].top_texture_number == (int) Texture_Type.door_red_v_top_closed_test && red_doors_open == true) spriteBatch.Draw (brush_control.tile[(int) Texture_Type.door_red_v_top_open_test].texture, r_draw, r_source, Color.White);
          else if (brush_control.brush[b].top_texture_number == (int) Texture_Type.door_red_h_top_closed_test && red_doors_open == true) spriteBatch.Draw (brush_control.tile[(int) Texture_Type.door_red_h_top_open_test].texture, r_draw, r_source, Color.White);
          else if (brush_control.brush[b].top_texture_number == (int) Texture_Type.door_yellow_v_top_closed_test && yellow_doors_open == true) spriteBatch.Draw (brush_control.tile[(int) Texture_Type.door_yellow_v_top_open_test].texture, r_draw, r_source, Color.White);
          else if (brush_control.brush[b].top_texture_number == (int) Texture_Type.door_yellow_h_top_closed_test && yellow_doors_open == true) spriteBatch.Draw (brush_control.tile[(int) Texture_Type.door_yellow_h_top_open_test].texture, r_draw, r_source, Color.White);
          else if (brush_control.brush[b].top_texture_number == (int) Texture_Type.door_green_v_top_closed_test && green_doors_open == true) spriteBatch.Draw (brush_control.tile[(int) Texture_Type.door_green_v_top_open_test].texture, r_draw, r_source, Color.White);
          else if (brush_control.brush[b].top_texture_number == (int) Texture_Type.door_green_h_top_closed_test && green_doors_open == true) spriteBatch.Draw (brush_control.tile[(int) Texture_Type.door_green_h_top_open_test].texture, r_draw, r_source, Color.White);
          else if (brush_control.brush[b].top_texture_number == (int) Texture_Type.door_blue_v_top_closed_test && blue_doors_open == true) spriteBatch.Draw (brush_control.tile[(int) Texture_Type.door_blue_v_top_open_test].texture, r_draw, r_source, Color.White);
          else if (brush_control.brush[b].top_texture_number == (int) Texture_Type.door_blue_h_top_closed_test && blue_doors_open == true) spriteBatch.Draw (brush_control.tile[(int) Texture_Type.door_blue_h_top_open_test].texture, r_draw, r_source, Color.White);
          else if (brush_control.brush[b].top_texture_number == (int) Texture_Type.exit_red_v_top_closed_test && red_doors_open == true) spriteBatch.Draw (brush_control.tile[(int) Texture_Type.exit_red_v_top_open_test].texture, r_draw, r_source, Color.White);
          else if (brush_control.brush[b].top_texture_number == (int) Texture_Type.exit_red_h_top_closed_test && red_doors_open == true) spriteBatch.Draw (brush_control.tile[(int) Texture_Type.exit_red_h_top_open_test].texture, r_draw, r_source, Color.White);
          else if (brush_control.brush[b].top_texture_number == (int) Texture_Type.box_ice_test) spriteBatch.Draw (brush_control.tile[brush_control.brush[b].top_texture_number].texture, r_draw, r_source, Color.White * .9f);
          else spriteBatch.Draw (brush_control.tile[brush_control.brush[b].top_texture_number].texture, r_draw, r_source, Color.White);

          // top stickers
          if (brush_control.brush[b].top_sticker > -1)
            {
            r_source.X = brush_control.brush[b].top_sticker_offset_x;
            r_source.Y = brush_control.brush[b].top_sticker_offset_y;
            r_source.Width = tilesize;
            r_source.Height = tilesize;
            r_draw.Width = tilesize;
            r_draw.Height = tilesize;

            if (brush_control.brush[b].top_sticker_type == "office") spriteBatch.Draw (sticker_office[brush_control.brush[b].top_sticker], r_draw, r_source, Color.White);
            else if (brush_control.brush[b].top_sticker_type == "office floor") spriteBatch.Draw (sticker_office_floor[brush_control.brush[b].top_sticker], r_draw, r_source, Color.White);
            else if (brush_control.brush[b].top_sticker_type == "factory") spriteBatch.Draw (sticker_factory[brush_control.brush[b].top_sticker], r_draw, r_source, Color.White);
            else if (brush_control.brush[b].top_sticker_type == "factory floor") spriteBatch.Draw (sticker_factory_floor[brush_control.brush[b].top_sticker], r_draw, r_source, Color.White * brush_control.brush[b].top_sticker_alpha);
            }

          // TOP EDGE OUTLINES
          if (draw_outlines.on)
            {
            if (brush_control.brush[b].top_top_outline)
              {
              v_draw.X = brush_screen_draw.X;
              v_draw.Y = brush_screen_draw.Y;
              spriteBatch.Draw (brush_outline_top, v_draw, Color.White * draw_outlines.opacity);
              }
            if (brush_control.brush[b].top_left_outline)
              {
              v_draw.X = brush_screen_draw.X;
              v_draw.Y = brush_screen_draw.Y;
              spriteBatch.Draw (brush_outline_left, v_draw, Color.White * draw_outlines.opacity);
              }
            if (brush_control.brush[b].top_right_outline)
              {
              v_draw.X = brush_screen_draw.X + tilesize - brush_outline_right.Width;
              v_draw.Y = brush_screen_draw.Y;
              spriteBatch.Draw (brush_outline_right, v_draw, Color.White * draw_outlines.opacity);
              }
            if (brush_control.brush[b].top_bottom_outline)
              {
              v_draw.X = brush_screen_draw.X;
              v_draw.Y = brush_screen_draw.Y + tilesize - brush_outline_bottom.Height;
              spriteBatch.Draw (brush_outline_bottom, v_draw, Color.White * draw_outlines.opacity);
              }
            if (brush_control.brush[b].top_top_left_outline)
              {
              v_draw.X = brush_screen_draw.X;
              v_draw.Y = brush_screen_draw.Y;
              spriteBatch.Draw (brush_outline_top_left, v_draw, Color.White * draw_outlines.opacity);
              }
            if (brush_control.brush[b].top_top_right_outline)
              {
              v_draw.X = brush_screen_draw.X + tilesize - brush_outline_right.Width;
              v_draw.Y = brush_screen_draw.Y;
              spriteBatch.Draw (brush_outline_top_right, v_draw, Color.White * draw_outlines.opacity);
              }
            if (brush_control.brush[b].top_bottom_left_outline)
              {
              v_draw.X = brush_screen_draw.X;
              v_draw.Y = brush_screen_draw.Y + tilesize - brush_outline_bottom.Height;
              spriteBatch.Draw (brush_outline_bottom_left, v_draw, Color.White * draw_outlines.opacity);
              }
            if (brush_control.brush[b].top_bottom_right_outline)
              {
              v_draw.X = brush_screen_draw.X + tilesize - brush_outline_right.Width;
              v_draw.Y = brush_screen_draw.Y + tilesize - brush_outline_bottom.Height;
              spriteBatch.Draw (brush_outline_bottom_right, v_draw, Color.White * draw_outlines.opacity);
              }
            }
          }

        // front of brush

        // only draw if visible (not completely covered by a brush_control.brush in front)
        draw_brush = false;
        b1 = brush_control.point_in_brush (brush_control.brush[b].x + (tilesize / 2), brush_control.brush[b].y - 1, brush_control.brush[b].z + Convert.ToInt32 (brush_control.brush[b].height * .5), true, false);
        b2 = brush_control.point_in_brush (brush_control.brush[b].x, brush_control.brush[b].y - 1, brush_control.brush[b].z + Convert.ToInt32 (brush_control.brush[b].height * .5), true, false);
        b3 = brush_control.point_in_brush (brush_control.brush[b].x + tilesize - 1, brush_control.brush[b].y - 1, brush_control.brush[b].z + Convert.ToInt32 (brush_control.brush[b].height * .5), true, false);
        b4 = brush_control.point_in_brush (brush_control.brush[b].x + (tilesize / 2), brush_control.brush[b].y - 1, brush_control.brush[b].z + Convert.ToInt32 (brush_control.brush[b].height * .75), true, false);
        b5 = brush_control.point_in_brush (brush_control.brush[b].x, brush_control.brush[b].y - 1, brush_control.brush[b].z + Convert.ToInt32 (brush_control.brush[b].height * .75), true, false);
        b6 = brush_control.point_in_brush (brush_control.brush[b].x + tilesize - 1, brush_control.brush[b].y - 1, brush_control.brush[b].z + Convert.ToInt32 (brush_control.brush[b].height * .75), true, false);
        if (b1 == -1) draw_brush = true;
        else if (brush_control.brush[b1].transparent == true) draw_brush = true;
        else if (b2 == -1) draw_brush = true;
        else if (brush_control.brush[b2].transparent == true) draw_brush = true;
        else if (b3 == -1) draw_brush = true;
        else if (brush_control.brush[b3].transparent == true) draw_brush = true;
        else if (b4 == -1) draw_brush = true;
        else if (brush_control.brush[b4].transparent == true) draw_brush = true;
        else if (b5 == -1) draw_brush = true;
        else if (brush_control.brush[b5].transparent == true) draw_brush = true;
        else if (b6 == -1) draw_brush = true;
        else if (brush_control.brush[b6].transparent == true) draw_brush = true;
        
        if (draw_brush == true)
          {
          r_draw.X = Convert.ToInt32 (brush_screen_draw.X);
          r_draw.Y = Convert.ToInt32 (brush_screen_draw.Y + tilesize);
          r_draw.Width = tilesize;
          r_draw.Height = Convert.ToInt32 (brush_control.brush[b].height * parallax);// 2;

          r_source.X = brush_control.brush[b].front_texture_offset_x;
          r_source.Y = brush_control.brush[b].front_texture_offset_y;
          r_source.Width = tilesize;
          r_source.Height = brush_control.brush[b].height;

          if (brush_control.brush[b].front_texture_number == (int) Texture_Type.floor_grate_test)
            {
            r_source.Height = 6;
            r_draw.Height = 6;
            }

          if (brush_control.brush[b].top_texture_number == (int) Texture_Type.invisible) { }
          else if (brush_control.brush[b].front_texture_number == (int) Texture_Type.door_green_v_front_closed_test && green_doors_open == true) spriteBatch.Draw (brush_control.tile[(int) Texture_Type.door_green_v_front_open_test].texture, r_draw, r_source, Color.White);
          else if (brush_control.brush[b].front_texture_number == (int) Texture_Type.door_red_v_front_closed_test && red_doors_open == true) spriteBatch.Draw (brush_control.tile[(int) Texture_Type.door_red_v_front_open_test].texture, r_draw, r_source, Color.White);
          else if (brush_control.brush[b].front_texture_number == (int) Texture_Type.door_blue_v_front_closed_test && blue_doors_open == true) spriteBatch.Draw (brush_control.tile[(int) Texture_Type.door_blue_v_front_open_test].texture, r_draw, r_source, Color.White);
          else if (brush_control.brush[b].front_texture_number == (int) Texture_Type.door_yellow_v_front_closed_test && yellow_doors_open == true) spriteBatch.Draw (brush_control.tile[(int) Texture_Type.door_yellow_v_front_open_test].texture, r_draw, r_source, Color.White);
          else if (brush_control.brush[b].front_texture_number == (int) Texture_Type.exit_red_v_front_closed_test && red_doors_open == true) spriteBatch.Draw (brush_control.tile[(int) Texture_Type.exit_red_v_front_open_test].texture, r_draw, r_source, Color.White);
          else if (brush_control.brush[b].front_texture_number == (int) Texture_Type.box_ice_test) spriteBatch.Draw (brush_control.tile[brush_control.brush[b].front_texture_number].texture, r_draw, r_source, Color.White * .9f);
          else
            {
            spriteBatch.Draw (brush_control.tile[brush_control.brush[b].front_texture_number].texture, r_draw, r_source, Color.White);
            spriteBatch.Draw (brush_control.tile[brush_control.brush[b].front_texture_number].texture, r_draw, r_source, Color.Black * .3f);
            }

          // front stickers
          if (brush_control.brush[b].front_sticker > -1)
            {
            r_source.X = brush_control.brush[b].front_sticker_offset_x;
            r_source.Y = brush_control.brush[b].front_sticker_offset_y;
            r_source.Width = tilesize;
            r_source.Height = tilesize;
            r_draw.Width = tilesize;
            r_draw.Height = Convert.ToInt32 (tilesize * parallax);

            if (brush_control.brush[b].front_sticker_type == "office") spriteBatch.Draw (sticker_office[brush_control.brush[b].front_sticker], r_draw, r_source, Color.White);
            else if (brush_control.brush[b].front_sticker_type == "factory") spriteBatch.Draw (sticker_factory[brush_control.brush[b].front_sticker], r_draw, r_source, Color.White);
            }

          // FRONT EDGE OUTLINES
          if (draw_outlines.on)
            {
            if (brush_control.brush[b].front_top_outline)
              {
              v_draw.X = brush_screen_draw.X;
              v_draw.Y = brush_screen_draw.Y + tilesize;
              spriteBatch.Draw (brush_outline_top, v_draw, Color.White * draw_outlines.opacity);
              }
            if (brush_control.brush[b].front_left_outline)
              {
              r_draw.X = Convert.ToInt32 (brush_screen_draw.X);
              r_draw.Y = Convert.ToInt32 (brush_screen_draw.Y + tilesize);
              r_draw.Width = brush_outline_left.Width;
              r_draw.Height = Convert.ToInt32 (tilesize * parallax);
              spriteBatch.Draw (brush_outline_left, r_draw, Color.White * draw_outlines.opacity);
              }
            if (brush_control.brush[b].front_right_outline)
              {
              r_draw.X = Convert.ToInt32 (brush_screen_draw.X + tilesize - brush_outline_right.Width);
              r_draw.Y = Convert.ToInt32 (brush_screen_draw.Y + tilesize);
              r_draw.Width = brush_outline_right.Width;
              r_draw.Height = Convert.ToInt32 (tilesize * parallax);
              spriteBatch.Draw (brush_outline_right, r_draw, Color.White * draw_outlines.opacity);
              }
            if (brush_control.brush[b].front_bottom_outline)
              {
              v_draw.X = brush_screen_draw.X;
              v_draw.Y = Convert.ToInt32 (brush_screen_draw.Y + tilesize + (tilesize * parallax) - brush_outline_bottom.Height);
              spriteBatch.Draw (brush_outline_bottom, v_draw, Color.White * draw_outlines.opacity);
              }
            }
          }  // if (draw_brush == true)
        }

      // red tinting for hot metal
      if ((brush_control.brush[b].top_texture_number == (int) Texture_Type.box_metal_test || brush_control.brush[b].top_texture_number == (int) Texture_Type.metal_mint_top_test
            || brush_control.brush[b].top_texture_number == (int) Texture_Type.metal_blue_top_test || brush_control.brush[b].top_texture_number == (int) Texture_Type.metal_black_test)
          && brush_control.brush[b].temperature > 70)
        {
        spriteBatch.Draw (brush_control.tile[(int) Texture_Type.texture_highlight_red].texture, brush_screen_draw, Color.White * ((Convert.ToSingle (brush_control.brush[b].temperature) - 70f) / 300f));
        }

      // WALL SHADOWS

      // if box is being moved
      if (brush_control.brush[b].moving == true)
        {
        // use older cast-from-wall method
        //shadow_west = false;
        //shadow_south = false;
        //shadow_east = false;

        //shadow_south = true;
        v_draw.X = brush_control.brush[b].x + screen.scroll_x;
        v_draw.Y = Convert.ToInt32 ((screen.height - brush_control.brush[b].y) - (brush_control.brush[b].z * parallax) + screen.scroll_y);
        spriteBatch.Draw (wall_shadow_south, v_draw, Color.White * .75f);

        //shadow_west = true;
        v_draw.X = brush_control.brush[b].x + screen.scroll_x - wall_shadow_west.Width;
        v_draw.Y = Convert.ToInt32 ((screen.height - brush_control.brush[b].y - brush_control.brush[b].length) - (brush_control.brush[b].z * parallax) + screen.scroll_y);
        spriteBatch.Draw (wall_shadow_west, v_draw, Color.White * .75f);

        //shadow_east = true;
        v_draw.X = brush_control.brush[b].x + brush_control.brush[b].width + screen.scroll_x;
        v_draw.Y = Convert.ToInt32 ((screen.height - brush_control.brush[b].y - brush_control.brush[b].length) - (brush_control.brush[b].z * parallax) + screen.scroll_y);
        spriteBatch.Draw (wall_shadow_east, v_draw, Color.White * .75f);

        v_draw.X = brush_control.brush[b].x - wall_shadow_south_west.Width + screen.scroll_x;
        v_draw.Y = Convert.ToInt32 ((screen.height - brush_control.brush[b].y) - (brush_control.brush[b].z * parallax) + screen.scroll_y);
        spriteBatch.Draw (wall_shadow_south_west, v_draw, Color.White * .75f);

        v_draw.X = brush_control.brush[b].x + brush_control.brush[b].width + screen.scroll_x;
        v_draw.Y = Convert.ToInt32 ((screen.height - brush_control.brush[b].y) - (brush_control.brush[b].z * parallax) + screen.scroll_y);
        spriteBatch.Draw (wall_shadow_south_east, v_draw, Color.White * .75f);
        }

      //shadow_north = false;
      //shadow_east = false;
      //shadow_west = false;
      //floor_visible = true;

      //if (floor_visible == true)
        //{
        // shadow from north
        if (brush_control.brush[b].top_shadow_north)
          {
          //shadow_north = true;
          spriteBatch.Draw (wall_shadow_south, brush_screen_draw, Color.White * .75f);
          }
        // shadow from west
        if (brush_control.brush[b].top_shadow_west)
          {
          //shadow_west = true;
          spriteBatch.Draw (wall_shadow_east, brush_screen_draw, Color.White * .75f);
          }
        // shadow from east
        if (brush_control.brush[b].top_shadow_east)
          {
          //shadow_east = true;
          v_draw.X = brush_screen_draw.X + tilesize - wall_shadow_west.Width;
          v_draw.Y = brush_screen_draw.Y;
          spriteBatch.Draw (wall_shadow_west, v_draw, Color.White * .75f);
          }
        // shadow from north west
        if (brush_control.brush[b].top_shadow_northwest)
          {
          spriteBatch.Draw (wall_shadow_south_east, brush_screen_draw, Color.White * .75f);
          }
        // shadow from north east
        if (brush_control.brush[b].top_shadow_northeast)
          {
          v_draw.X = brush_screen_draw.X + tilesize - wall_shadow_south_west.Width;
          v_draw.Y = brush_screen_draw.Y;
          spriteBatch.Draw (wall_shadow_south_west, v_draw, Color.White * .75f);
          }
        //}

      if (character_control.character[PLAYER].brush_grab == b && character_control.character[PLAYER].grab_position != "above")
        {
        draw_character (character_control.character[PLAYER]);
        }
      }
    
    ////////////////////////////////////////////////////////////////////////////////

    void draw_character (Character c)
      {
      int tx, ty, tz;
      bool endloop, endloop2;
      int b, f;
      int distance, rotation;
      float temp_fade;
      double shadow_scale;
      Rectangle r_source, r_draw;//, r_shadow;
      Vector2 v_draw, v_draw2, v_origin, v_subtarget;

      //spriteBatch.Begin (SpriteSortMode.Immediate, BlendState.AlphaBlend);

      // draw floor shadow
      tx = c.x;
      ty = c.y;
      tz = c.z;

      endloop = false;
      while (endloop == false && tz >= 0)
        {
        endloop2 = false;
        b = 0;
        f = 0;
        while (endloop2 == false && (b < brush_control.brush.Count || f < fixture_control.fixture.Count))//total_fixtures))
          {
          if (b < brush_control.brush.Count)
            {
            if (tx >= brush_control.brush[b].x && tx <= brush_control.brush[b].x + brush_control.brush[b].width &&
                ty >= brush_control.brush[b].y && ty <= brush_control.brush[b].y + brush_control.brush[b].length &&
                tz == brush_control.brush[b].z + brush_control.brush[b].height) { endloop2 = true; endloop = true; }
            b += 1;
            }
          if (f < fixture_control.fixture.Count)//total_fixtures)
            {
            if (tx >= fixture_control.fixture[f].x && tx <= fixture_control.fixture[f].x + fixture_control.fixture[f].width &&
                ty >= fixture_control.fixture[f].y && ty <= fixture_control.fixture[f].y + fixture_control.fixture[f].length &&
                tz == fixture_control.fixture[f].z + fixture_control.fixture[f].height) { endloop2 = true; endloop = true; }
            f += 1;
            }
          }
        tz -= 1;
        }

      distance = c.z - tz;
      temp_fade = Convert.ToSingle (.4f - (distance / 300f));
      if (temp_fade < .2f) temp_fade = .2f;
      shadow_scale = 1.0 + (distance / 192.0);

      // find animation frame
      r_source.X = c.get_frame_x ();

      // find frames for direction facing
      if (c.dir >= MathHelper.ToRadians (225) && c.dir <= MathHelper.ToRadians (315)) r_source.Y = 1;
      else if (c.dir > MathHelper.ToRadians (45) && c.dir < MathHelper.ToRadians (135)) r_source.Y = 1 + c.sprite_height + 1;
      else if (c.dir >= MathHelper.ToRadians (135) && c.dir < MathHelper.ToRadians (225)) r_source.Y = 1 + (c.sprite_height + 1) * 2;
      else if (c.dir <= MathHelper.ToRadians (45) || c.dir > MathHelper.ToRadians (315)) r_source.Y = 1 + (c.sprite_height + 1) * 3;
      else r_source.Y = 1;

      r_source.Width = c.sprite_width - 1;
      r_source.Height = c.sprite_height;

      r_draw.Width = Convert.ToInt32 (c.sprite_width * shadow_scale);
      r_draw.Height = Convert.ToInt32 (c.sprite_height / 4 * shadow_scale);
      //r_draw.Width = r_source.Width;
      //r_draw.Height = r_source.Height;
      r_draw.X = tx + screen.scroll_x + 1;
      r_draw.Y = y_draw_coordinate (ty, tz) - (c.walk_pixels + 1);// / 2);

      if (dynamic_shadows)
        {
        v_origin.X = c.sprite_width / 2;
        v_origin.Y = 0;

        spriteBatch.Draw (character_control.character_sprite[c.sprite, 0], r_draw, r_source, Color.Black * temp_fade, MathHelper.ToRadians (0), v_origin, SpriteEffects.FlipVertically, 0);
        }
      else
        {
        r_source.X = 0;
        r_source.Y = 0;
        r_source.Width = shadow_character_generic.Width;
        r_source.Height = shadow_character_generic.Height;

        v_origin.X = shadow_character_generic.Width / 2;
        v_origin.Y = 0;

        spriteBatch.Draw (shadow_character_generic, r_draw, r_source, Color.White * temp_fade, 0, v_origin, SpriteEffects.None, 0);
        }

      // draw guy

      // find animation frame
      r_source.X = c.get_frame_x ();
      r_source.Y = c.get_frame_y ();
      r_source.Width = c.sprite_width - 1;
      r_source.Height = c.sprite_height;

      v_draw.X = c.x + screen.scroll_x;
      v_draw.Y = y_draw_coordinate (c.y, c.z) + c.walk_pixels;
      v_origin.X = c.sprite_width / 2;
      v_origin.Y = c.sprite_height;
      rotation = 0;

      spriteBatch.Draw (character_control.character_sprite[c.sprite, c.skin], v_draw, r_source, Color.White, MathHelper.ToRadians (rotation), v_origin, 1, SpriteEffects.None, 0);

      if (c.blinking == true)
        {
        r_source.X = 1 + (c.sprite_width + 1);
        spriteBatch.Draw (character_control.character_sprite[c.sprite, c.skin], v_draw, r_source, Color.White, MathHelper.ToRadians (rotation), v_origin, 1, SpriteEffects.None, 0);
        }

      if (debug && game_state == Game_State.game)
        {
        // draw arrow (remove later)
        v_draw2.X = v_draw.X;
        v_draw2.Y = v_draw.Y - (c.sprite_height / 3);
        v_origin.X = 16;
        v_origin.Y = 16;
        spriteBatch.Draw (arrow_sprite, v_draw2, null, Color.White * .3f, Convert.ToSingle (c.dir) * -1, v_origin, 1, SpriteEffects.None, 0);
        }

      // draw POW
      if (c.pow1.opacity > 0f)
        {
        v_draw2.X = v_draw.X + c.pow1.x;
        v_draw2.Y = v_draw.Y + c.pow1.y;
        r_source.X = 1 + (c.pow1.shape * (Character_Control.pow_sprite_width + 1));
        r_source.Y = 1 + (c.pow1.color * (Character_Control.pow_sprite_height + 1));
        r_source.Width = Character_Control.pow_sprite_width;
        r_source.Height = Character_Control.pow_sprite_height;
        spriteBatch.Draw (pow_sprite, v_draw2, r_source, Color.White * c.pow1.opacity);
        c.pow1.opacity -= 0.03f;
        if (c.pow1.opacity < 0f) c.pow1.opacity = 0f;
        }

      // draw health over head
      if (game_state == Game_State.game && debug == true)
        {
        v_draw2.X = v_draw.X - 15;
        v_draw2.Y = v_draw.Y - c.sprite_height - 20;
        spriteBatch.DrawString (debug_font, Convert.ToString (c.health), v_draw2, Color.White);
        }

      // draw subtarget location for ai navigation (remove later)
      if (debug && game_state == Game_State.game && c.player && character_control.active (c.target))
        {
        //v_subtarget.X = character_control.character[c.target].x + scroll_x;
        v_subtarget.X = c.subtarget_x + screen.scroll_x;
        //v_subtarget.Y = y_draw_coordinate (character_control.character[c.target].y, character_control.character[c.target].z) + character_control.character[c.target].walk_pixels;
        v_subtarget.Y = y_draw_coordinate (c.subtarget_y, c.subtarget_z) + character_control.character[c.target].walk_pixels;
        v_draw2.X = v_subtarget.X - (target_sprite.Width / 2);
        v_draw2.Y = v_subtarget.Y - (target_sprite.Height / 2);

        spriteBatch.Draw (target_sprite, v_draw2, Color.White * 0.5f);
        shape.line (spriteBatch, Convert.ToInt32 (v_draw.X), Convert.ToInt32 (v_draw.Y), Convert.ToInt32 (v_subtarget.X), Convert.ToInt32 (v_subtarget.Y), pixel_yellow, 1f);
        }
      }
    
    //////////////////////////////////////////////////////////////////////////////

    int y_draw_coordinate (int world_y, int world_z)
      {
      return Convert.ToInt32 ((screen.height - world_y) - (world_z * parallax) + screen.scroll_y);
      }

    ////////////////////////////////////////////////////////////////////////////////

    }
  }
