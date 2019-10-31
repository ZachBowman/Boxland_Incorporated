using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Xml;

namespace Boxland
  {
  public partial class Boxland : Game
    {
    Key_Bindings key_bindings = new Key_Bindings ();
    
    ////////////////////////////////////////////////////////////////////////////////

    void load_bindings ()
      {
      using (XmlReader reader = XmlReader.Create ("settings.xml"))
        {
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void save_bindings ()
      {
      using (XmlWriter writer = XmlWriter.Create ("settings.xml"))
        {
        writer.WriteStartDocument ();
        writer.WriteStartElement ("control_bindings");
        writer.WriteString ("\n");

        for (int b = 0; b < key_bindings.bindings.Count; b += 1)
          {
          writer.WriteStartElement ("binding");
          writer.WriteAttributeString ("key", key_bindings.bindings[b].key_binding.ToString ());
          writer.WriteAttributeString ("action", key_bindings.bindings[b].action.ToString ());
          writer.WriteEndElement ();
          writer.WriteString ("\n");
          }
        writer.WriteEndElement ();
        writer.WriteEndDocument ();
        }
      }
    
    ////////////////////////////////////////////////////////////////////////////////

    void Get_Input()  // decide if player is using keyboard or controller/gamepad at title screen
      {
      // check for keyboard key
      keyboard = Keyboard.GetState();
      bool keydown = false;
      for (int k = 0; k < 160; k += 1) if (keyboard.IsKeyDown((Keys)k)) keydown = true;
      if (keydown == true) player_movement = Movement.keyboard;

      // check for controller button
      controller = GamePad.GetState(PlayerIndex.One);
      if (controller.Buttons.A == ButtonState.Pressed
          || controller.Buttons.B == ButtonState.Pressed
          || controller.Buttons.X == ButtonState.Pressed
          || controller.Buttons.Y == ButtonState.Pressed
          || controller.Buttons.Start == ButtonState.Pressed
          || controller.Buttons.Back == ButtonState.Pressed
          || controller.Buttons.BigButton == ButtonState.Pressed
          || controller.Buttons.LeftShoulder == ButtonState.Pressed
          || controller.Buttons.RightShoulder == ButtonState.Pressed
          || controller.Buttons.LeftStick == ButtonState.Pressed
          || controller.Buttons.RightStick == ButtonState.Pressed)
        {
        player_movement = Movement.controller;
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void Keyboard_Input()
      {
      //int b;
      double next_dir = character_control.character[PLAYER].dir;
      bool skid = false;

      keyboard = Keyboard.GetState();

      // IN GAME
      if (game_state == Game_State.game && character_control.active (PLAYER) && game_menu == false)
        {
        if (player_movement == Movement.keyboard)
          {
          if (debug)  // keep track of the last key pressed for tracking control issues
            {
            Keys[] pressed_keys = keyboard.GetPressedKeys ();
            if (pressed_keys.Length > 0) last_key_pressed = pressed_keys[0];
            }

          // change direction facing (if-else chain handles diagonal directions)
          if (keyboard.IsKeyDown (Keys.Down) && keyboard.IsKeyDown (Keys.Right)) next_dir = MathHelper.ToRadians (315);
          else if (keyboard.IsKeyDown (Keys.Down) && keyboard.IsKeyDown (Keys.Left)) next_dir = MathHelper.ToRadians (225);
          else if (keyboard.IsKeyDown (Keys.Up) && keyboard.IsKeyDown (Keys.Left)) next_dir = MathHelper.ToRadians (135);
          else if (keyboard.IsKeyDown (Keys.Up) && keyboard.IsKeyDown (Keys.Right)) next_dir = MathHelper.ToRadians (45);
          else if (keyboard.IsKeyDown (Keys.Right)) next_dir = MathHelper.ToRadians (0);
          else if (keyboard.IsKeyDown (Keys.Down)) next_dir = MathHelper.ToRadians (270);
          else if (keyboard.IsKeyDown (Keys.Left)) next_dir = MathHelper.ToRadians (180);
          else if (keyboard.IsKeyDown (Keys.Up)) next_dir = MathHelper.ToRadians (90);

          else if (keyboard.IsKeyDown (Keys.NumPad5) && keyboard.IsKeyDown (Keys.NumPad6)) next_dir = MathHelper.ToRadians (315);
          else if (keyboard.IsKeyDown (Keys.NumPad5) && keyboard.IsKeyDown (Keys.NumPad4)) next_dir = MathHelper.ToRadians (225);
          else if (keyboard.IsKeyDown (Keys.NumPad8) && keyboard.IsKeyDown (Keys.NumPad4)) next_dir = MathHelper.ToRadians (135);
          else if (keyboard.IsKeyDown (Keys.NumPad8) && keyboard.IsKeyDown (Keys.NumPad6)) next_dir = MathHelper.ToRadians (45);
          else if (keyboard.IsKeyDown (Keys.NumPad6)) next_dir = MathHelper.ToRadians (0);
          else if (keyboard.IsKeyDown (Keys.NumPad5)) next_dir = MathHelper.ToRadians (270);
          else if (keyboard.IsKeyDown (Keys.NumPad4)) next_dir = MathHelper.ToRadians (180);
          else if (keyboard.IsKeyDown (Keys.NumPad8)) next_dir = MathHelper.ToRadians (90);

          else if (keyboard.IsKeyDown (Keys.S) && keyboard.IsKeyDown (Keys.D)) next_dir = MathHelper.ToRadians (315);
          else if (keyboard.IsKeyDown (Keys.S) && keyboard.IsKeyDown (Keys.A)) next_dir = MathHelper.ToRadians (225);
          else if (keyboard.IsKeyDown (Keys.W) && keyboard.IsKeyDown (Keys.A)) next_dir = MathHelper.ToRadians (135);
          else if (keyboard.IsKeyDown (Keys.W) && keyboard.IsKeyDown (Keys.D)) next_dir = MathHelper.ToRadians (45);
          else if (keyboard.IsKeyDown (Keys.D)) next_dir = MathHelper.ToRadians (0);
          else if (keyboard.IsKeyDown (Keys.S)) next_dir = MathHelper.ToRadians (270);
          else if (keyboard.IsKeyDown (Keys.A)) next_dir = MathHelper.ToRadians (180);
          else if (keyboard.IsKeyDown (Keys.W)) next_dir = MathHelper.ToRadians (90);

          // if the player makes a sharp turn and is running, skid.
          // if the player makes a sharp turn and is running and the skid counter is less than max, skid.

          bool player_sharp_turn = sharp_turn (next_dir, character_control.character[PLAYER].dir);

          if (player_sharp_turn && character_control.character[PLAYER].action == Action.run) skid = true;// character_control.character[PLAYER].skid ();
          if (player_sharp_turn && character_control.character[PLAYER].action == Action.none && character_control.character[PLAYER].skid_counter < Character.skid_delay) skid = true;// character_control.character[PLAYER].skid ();

          if (next_dir != character_control.character[PLAYER].dir)
            {
            character_control.character[PLAYER].last_dir = character_control.character[PLAYER].dir;
            character_control.character[PLAYER].dir = next_dir;
            }
          if (skid) character_control.character[PLAYER].skid ();

          // if any movement key is held down
          if (keyboard.IsKeyDown (Keys.Up) || keyboard.IsKeyDown (Keys.Down)
              || keyboard.IsKeyDown (Keys.Left) || keyboard.IsKeyDown (Keys.Right)
              || keyboard.IsKeyDown (Keys.NumPad5) || keyboard.IsKeyDown (Keys.NumPad8)
              || keyboard.IsKeyDown (Keys.NumPad4) || keyboard.IsKeyDown (Keys.NumPad6)
              || keyboard.IsKeyDown (Keys.W) || keyboard.IsKeyDown (Keys.S)
              || keyboard.IsKeyDown (Keys.A) || keyboard.IsKeyDown (Keys.D))
            {
            if (key_anydirection == false) key_anydirection = true;
            player_move ();
            }
          else
            {
            // when the last arrow key is released
            if (key_anydirection == true)
              {
              key_anydirection = false;
              // if player is walking, running, or skidding and he lets go, he stops moving
              if (character_control.character[PLAYER].action == Action.skid)
                {
                character_control.character[PLAYER].ext_x_velocity = 0;
                character_control.character[PLAYER].ext_y_velocity = 0;
                }
              if (character_control.character[PLAYER].action == Action.walk || character_control.character[PLAYER].action == Action.run
                  || character_control.character[PLAYER].action == Action.skid) character_control.character[PLAYER].stand ();
              if (character_control.character[PLAYER].action == Action.push)
                {
                character_control.character[PLAYER].action = Action.grab;// ending_push;
                }
              }
            }
          }

        // up
        if (keyboard.IsKeyDown (Keys.Up))  // up held down
          {
          if (game_state == Game_State.game) push_north ();
          if (key_up == false) key_up = true;
          }
        if (!keyboard.IsKeyDown (Keys.Up) && key_up == true) key_up = false;

        // down
        if (keyboard.IsKeyDown (Keys.Down))  // down held down
          {
          if (game_state == Game_State.game) push_south ();

          if (key_down == false) key_down = true;
          }
        if (!keyboard.IsKeyDown (Keys.Down) && key_down == true) key_down = false;

        // left
        if (keyboard.IsKeyDown (Keys.Left))  // left held down
          {
          if (game_state == Game_State.game) push_west ();

          if (key_left == false) key_left = true;
          }
        if (!keyboard.IsKeyDown (Keys.Left) && key_left == true) key_left = false;  // left released

        // right
        if (keyboard.IsKeyDown (Keys.Right))  // right held down
          {
          if (game_state == Game_State.game)
            {
            if (character_control.character[PLAYER].action == Action.push
                || character_control.character[PLAYER].action == Action.grab) push_east ();
            }

          if (key_right == false) key_right = true;
          }
        if (!keyboard.IsKeyDown (Keys.Right) && key_right == true) key_right = false;

        // number pad 8 - up
        if (keyboard.IsKeyDown (Keys.NumPad8))
          {
          if (game_state == Game_State.game) push_north ();
          if (key_numpad8 == false) key_numpad8 = true;
          }
        if (!keyboard.IsKeyDown (Keys.NumPad8) && key_numpad8 == true) key_numpad8 = false;

        // number pad 5 - down
        if (keyboard.IsKeyDown (Keys.NumPad5))
          {
          if (game_state == Game_State.game) push_south ();

          if (key_numpad5 == false) key_numpad5 = true;
          }
        if (!keyboard.IsKeyDown (Keys.NumPad5) && key_numpad5 == true) key_numpad5 = false;

        // number pad 4 - left
        if (keyboard.IsKeyDown (Keys.NumPad4))
          {
          if (game_state == Game_State.game) push_west ();

          if (key_numpad4 == false) key_numpad4 = true;
          }
        if (!keyboard.IsKeyDown (Keys.NumPad4) && key_numpad4 == true) key_numpad4 = false;  // left released

        // number pad 6 - right
        if (keyboard.IsKeyDown (Keys.NumPad6))
          {
          if (game_state == Game_State.game) push_east ();

          if (key_numpad6 == false) key_numpad6 = true;
          }
        if (!keyboard.IsKeyDown (Keys.NumPad6) && key_right == true) key_numpad6 = false;

        // W - up      
        if (keyboard.IsKeyDown (Keys.W))
          {
          if (game_state == Game_State.game) push_north ();
          if (key_w == false) key_w = true;
          }
        if (!keyboard.IsKeyDown (Keys.W) && key_w == true) key_w = false;

        // S - down
        if (keyboard.IsKeyDown (Keys.S))
          {
          if (game_state == Game_State.game) push_south ();
          if (key_s == false) key_s = true;
          }
        if (!keyboard.IsKeyDown (Keys.S) && key_s == true) key_s = false;

        // A - left
        if (keyboard.IsKeyDown (Keys.A))
          {
          if (game_state == Game_State.game) push_west ();
          if (key_a == false) key_a = true;
          }
        if (!keyboard.IsKeyDown (Keys.A) && key_a == true) key_a = false;

        // D - right
        if (keyboard.IsKeyDown (Keys.D))
          {
          if (game_state == Game_State.game) push_east ();
          if (key_d == false) key_d = true;
          }
        if (!keyboard.IsKeyDown (Keys.D) && key_d == true) key_d = false;

        // left shift - run
        //if (keyboard.IsKeyDown (Keys.LeftShift) && key_leftshift == false)
        //  {
        //  key_leftshift = true;
        //  character_control.character[PLAYER].runboost = true;
        //  if (character_control.character[PLAYER].action == Action.walk) character_control.character[PLAYER].action = Action.run;
        //  }
        //else if (!keyboard.IsKeyDown (Keys.LeftShift) && key_leftshift == true)
        //  {
        //  key_leftshift = false;
        //  character_control.character[PLAYER].runboost = false;
        //  if (character_control.character[PLAYER].action == Action.run) character_control.character[PLAYER].action = Action.walk;
        //  }

        // right shift - run
        if (keyboard.IsKeyDown (Keys.RightShift) && key_rightshift == false)
          {
          key_rightshift = true;
          //player_runboost = true;
          }
        else if (!keyboard.IsKeyDown (Keys.RightShift) && key_rightshift == true)
          {
          key_rightshift = false;
          //player_runboost = false;
          }

        // left ctrl - punch / swing
        if (keyboard.IsKeyDown (Keys.LeftControl) && key_leftcontrol == false)
          {
          key_leftcontrol = true;
          if (character_control.active (PLAYER))
            {
            if (character_on_ground (PLAYER)) character_punch (PLAYER);
            else character_jump_kick (PLAYER);
            }
          }
        else if (!keyboard.IsKeyDown (Keys.LeftControl) && key_leftcontrol == true)
          {
          key_leftcontrol = false;
          if (character_control.character[PLAYER].action == Action.punch) character_control.character[PLAYER].anim_frame_sequence = punch_rest_delay;
          }

        // right ctrl - punch / swing
        if (keyboard.IsKeyDown (Keys.RightControl) && key_rightcontrol == false)
          {
          key_rightcontrol = true;
          if (character_control.active (PLAYER))
            {
            if (character_on_ground (PLAYER)) character_punch (PLAYER);
            else character_jump_kick (PLAYER);
            }
          }
        else if (!keyboard.IsKeyDown (Keys.RightControl) && key_rightcontrol == true)
          {
          key_rightcontrol = false;
          if (character_control.character[PLAYER].action == Action.punch) character_control.character[PLAYER].anim_frame_sequence = punch_rest_delay;
          }

        // left alt - special attack (shoot, throw, butterball, butt pound, etc.)
        if (keyboard.IsKeyDown (Keys.LeftAlt))
          {
          continue_special_attack ();
          //if (character_control.character[PLAYER].shirt == (int) Object_Type.shirt_yellow) character_control.character[PLAYER].action = Action.superpunch;
          //else if (character_control.character[PLAYER].shirt == "fire") character_control.character[PLAYER].action = Action.flamethrower;
          //else if (character_control.character[PLAYER].shirt == "ice") character_control.character[PLAYER].action = Action.freeze_ray;

          if (key_leftalt == false)
            {
            key_leftalt = true;

            begin_special_attack ();

            //if (character_control.character[PLAYER].shirt == (int) Object_Type.shirt_yellow) particle_superpunch (character_control.character[PLAYER].x, character_control.character[PLAYER].y, character_control.character[PLAYER].z + Convert.ToInt32 (character_control.character[PLAYER].box_height * .75), Convert.ToInt32 (MathHelper.ToDegrees (Convert.ToSingle (character_control.character[PLAYER].dir))));
            //else
            //if (character_control.character[PLAYER].shirt == "fire")
            //  particle_flamethrower (character_control.character[PLAYER].x, character_control.character[PLAYER].y, character_control.character[PLAYER].z + Convert.ToInt32 (character_control.character[PLAYER].box_height * .75), Convert.ToInt32 (MathHelper.ToDegrees (Convert.ToSingle (character_control.character[PLAYER].dir))), "character", PLAYER);
            //particle_incinerator (character_control.character[PLAYER].x, character_control.character[PLAYER].y, character_control.character[PLAYER].z + Convert.ToInt32 (character_control.character[PLAYER].box_height * .75), Convert.ToInt32 (MathHelper.ToDegrees (Convert.ToSingle (character_control.character[PLAYER].dir))));
            //else if (character_control.character[PLAYER].shirt == "ice") particle_freeze_ray (character_control.character[PLAYER].x, character_control.character[PLAYER].y, character_control.character[PLAYER].z + Convert.ToInt32 (character_control.character[PLAYER].box_height * .75), Convert.ToInt32 (MathHelper.ToDegrees (Convert.ToSingle (character_control.character[PLAYER].dir))), "character", PLAYER);
            }
          }
        else if (!keyboard.IsKeyDown (Keys.LeftAlt) && key_leftalt == true)
          {
          key_leftalt = false;
          end_special_attack ();
          //if (character_control.character[PLAYER].action == Action.superpunch || character_control.character[PLAYER].action == Action.freeze_ray
          //    || character_control.character[PLAYER].action == Action.flamethrower) character_control.character[PLAYER].action = Action.none;
          }

        // space - jump
        if (keyboard.IsKeyDown (Keys.Space) && key_space == false)
          {
          key_space = true;
          character_jump (PLAYER);
          }
        else if (!keyboard.IsKeyDown (Keys.Space) && key_space == true) key_space = false;

        // tab - cycle shirt
        if (keyboard.IsKeyDown (Keys.Tab) && key_tab == false)
          {
          key_tab = true;

          bool endloop = false;
          while (!endloop)
            {
            character_control.character[PLAYER].shirt += 1;
            if (character_control.character[PLAYER].shirt > 5) character_control.character[PLAYER].shirt = 0;
            if (character_control.character[PLAYER].shirt == 0) endloop = true;
            else if (character_control.character[PLAYER].shirt == (int) Object_Type.shirt_yellow && character_control.character[PLAYER].shirt_power) endloop = true;
            else if (character_control.character[PLAYER].shirt == (int) Object_Type.shirt_red && character_control.character[PLAYER].shirt_fire) endloop = true;
            else if (character_control.character[PLAYER].shirt == (int) Object_Type.shirt_white && character_control.character[PLAYER].shirt_ice) endloop = true;
            else if (character_control.character[PLAYER].shirt == (int) Object_Type.shirt_teal && character_control.character[PLAYER].shirt_electric) endloop = true;
            else if (character_control.character[PLAYER].shirt == (int) Object_Type.shirt_fushia && character_control.character[PLAYER].shirt_magnetic) endloop = true;
            }
          }
        else if (!keyboard.IsKeyDown (Keys.Tab) && key_tab == true) key_tab = false;

        // number pad 0 - grab box
        if (keyboard.IsKeyDown (Keys.NumPad0) && key_numpad0 == false)
          {
          key_numpad0 = true;
          if (character_control.character[PLAYER].action != Action.grab && character_control.character[PLAYER].action != Action.push) character_grab_brush (PLAYER);
          }

        // B - punch / jump kick
        if (keyboard.IsKeyDown (Keys.B) && key_b == false)
          {
          key_b = true;
          if (character_control.active (PLAYER))
            {
            if (character_on_ground (PLAYER)) character_punch (PLAYER);
            else character_jump_kick (PLAYER);
            }
          }
        if (!keyboard.IsKeyDown (Keys.B) && key_b == true)
          {
          key_b = false;
          if (character_control.character[PLAYER].action == Action.punch) character_control.character[PLAYER].anim_frame_sequence = punch_rest_delay;
          }

        // C - special attack
        if (keyboard.IsKeyDown (Keys.C))
          {
          continue_special_attack ();
          if (key_c == false)
            {
            key_c = true;
            begin_special_attack ();
            }
          }
        else if (!keyboard.IsKeyDown (Keys.C) && key_c == true)
          {
          key_c = false;
          end_special_attack ();
          }

        // E - grab box / activate / open door
        if (keyboard.IsKeyDown (Keys.E) && key_e == false)
          {
          key_e = true;
          if (character_control.character[PLAYER].action != Action.grab && character_control.character[PLAYER].action != Action.push) character_grab_brush (PLAYER);
          if (character_control.character[PLAYER].action != Action.grab && character_control.character[PLAYER].action != Action.push) character_use_door (PLAYER);
          }

        // F - special attack
        if (keyboard.IsKeyDown (Keys.F))
          {
          continue_special_attack ();
          if (key_f == false)
            {
            key_f = true;
            begin_special_attack ();
            }
          }
        else if (!keyboard.IsKeyDown (Keys.F) && key_f == true)
          {
          key_f = false;
          end_special_attack ();
          }

        // Q
        if (keyboard.IsKeyDown (Keys.Q) && key_q == false)
          {
          key_q = true;
          if (character_control.active (PLAYER))
            {
            if (character_on_ground (PLAYER)) character_punch (PLAYER);
            else character_jump_kick (PLAYER);
            }
          }
        if (!keyboard.IsKeyDown (Keys.Q) && key_q == true)
          {
          key_q = false;
          if (character_control.character[PLAYER].action == Action.punch) character_control.character[PLAYER].anim_frame_sequence = punch_rest_delay;
          }

        // V - grab box
        if (keyboard.IsKeyDown (Keys.V) && key_v == false)
          {
          key_v = true;
          if (character_control.character[PLAYER].action != Action.grab && character_control.character[PLAYER].action != Action.push) character_grab_brush (PLAYER);
          }

        // X - grab box / activate / open door
        if (keyboard.IsKeyDown (Keys.X) && key_x == false)
          {
          key_x = true;
          if (character_control.character[PLAYER].action != Action.grab && character_control.character[PLAYER].action != Action.push) character_grab_brush (PLAYER);
          if (character_control.character[PLAYER].action != Action.grab && character_control.character[PLAYER].action != Action.push) character_use_door (PLAYER);
          }

        // z - run
        if (keyboard.IsKeyDown (Keys.Z) && key_z == false)
          {
          key_z = true;
          character_control.character[PLAYER].run ();
          }
        else if (!keyboard.IsKeyDown (Keys.Z) && key_z == true)
          {
          key_z = false;
          character_control.character[PLAYER].walk ();
          }

        // release e, x, v, 0 (letting go of a box)
        if (!keyboard.IsKeyDown (Keys.E) && !keyboard.IsKeyDown (Keys.X) && !keyboard.IsKeyDown (Keys.NumPad0) && !keyboard.IsKeyDown (Keys.V))
          {
          if (game_state == Game_State.game)
            {
            if (character_control.character[PLAYER].action == Action.push
                || character_control.character[PLAYER].action == Action.grab)
              {
              //character_control.character[PLAYER].brush_grab = -1;
              //if (character_control.character[PLAYER].action == Action.grab)
              //|| character_control.character[PLAYER].action == Action.push
              //|| character_control.character[PLAYER].action == Action.ending_push)
              //{
              character_control.character[PLAYER].stand ();
              //character_control.character[PLAYER].action = Action.ending_push;
              //}
              }
            }
          if (key_e == true) key_e = false;
          if (key_v == true) key_v = false;
          if (key_x == true) key_x = false;
          if (key_numpad0 == true) key_numpad0 = false;
          }

        // escape - menu
        if (keyboard.IsKeyDown (Keys.Escape) && key_esc == false)  // escape single hit
          {
          key_esc = true;

          if (game_state == Game_State.game && game_menu == false)
            {
            game_menu = true;
            menu_screen = "main";
            }
          else game_menu = false;
          }
        else if (!keyboard.IsKeyDown (Keys.Escape) && key_esc == true) key_esc = false;

        // 1 - toggle debug mode (developer)
        if (keyboard.IsKeyDown (Keys.D1) && key_1 == false)
          {
          key_1 = true;
          if (debug == true) debug = false;
          else debug = true;
          }
        if (!keyboard.IsKeyDown (Keys.D1) && key_1 == true) key_1 = false;

        // 2 - toggle background layer (developer)
        if (keyboard.IsKeyDown (Keys.D2) && key_2 == false)
          {
          key_2 = true;
          if (draw_background.on) draw_background.turn_off ();
          else draw_background.turn_on ();
          }
        if (!keyboard.IsKeyDown (Keys.D2) && key_2 == true) key_2 = false;

        // 3 - toggle wall drawing (developer)
        if (keyboard.IsKeyDown (Keys.D3) && key_3 == false)
          {
          key_3 = true;
          if (draw_walls == true) draw_walls = false;
          else draw_walls = true;
          }
        if (!keyboard.IsKeyDown (Keys.D3) && key_3 == true) key_3 = false;

        // 4 - toggle box drawing (developer)
        if (keyboard.IsKeyDown (Keys.D4) && key_4 == false)
          {
          key_4 = true;
          if (draw_boxes == true) draw_boxes = false;
          else draw_boxes = true;
          }
        if (!keyboard.IsKeyDown (Keys.D4) && key_4 == true) key_4 = false;

        // 5 - toggle outline drawing (developer)
        if (keyboard.IsKeyDown (Keys.D5) && key_5 == false)
          {
          key_5 = true;
          if (draw_outlines.on) draw_outlines.turn_off ();
          else
            draw_outlines.turn_on ();
          }
        if (!keyboard.IsKeyDown (Keys.D5) && key_5 == true) key_5 = false;

        // 6 - toggle lighting engine on/off (developer)
        if (keyboard.IsKeyDown (Keys.D6) && key_6 == false)
          {
          key_6 = true;
          if (draw_lighting == true) draw_lighting = false;
          else draw_lighting = true;
          }
        if (!keyboard.IsKeyDown (Keys.D6) && key_6 == true) key_6 = false;

        // R - reset map
        if (keyboard.IsKeyDown (Keys.R) && key_r == false)
          {
          key_r = true;
          if (character_control.character[PLAYER].health <= 0) character_control.character[PLAYER].health = 100;
          construct_world ();
          }
        if (!keyboard.IsKeyDown (Keys.R) && key_r == true) key_r = false;

        // + - skip area (cheat - take out later)
        if (keyboard.IsKeyDown (Keys.OemPlus) && key_plus == false)
          {
          key_plus = true;
          skip_area ();
          }
        if (!keyboard.IsKeyDown (Keys.OemPlus) && key_plus == true) key_plus = false;

        // - - skip back area (cheat - take out later)
        if (keyboard.IsKeyDown (Keys.OemMinus) && key_minus == false)
          {
          key_minus = true;
          skip_back_area ();
          }
        if (!keyboard.IsKeyDown (Keys.OemMinus) && key_minus == true) key_minus = false;

        // backspace - refresh graphics (developer only)
        if (keyboard.IsKeyDown (Keys.Back) && key_backspace == false)
          {
          key_backspace = true;

          load_graphics ();
          }
        if (!keyboard.IsKeyDown (Keys.Back) && key_backspace == true) key_backspace = false;
        }  // game_state == Game_State.game

      // TITLE SCREEN
      else if (game_state == Game_State.title)
        {
        // enter - start game
        if (keyboard.IsKeyDown (Keys.Enter) && key_enter == false)
          {
          key_enter = true;
          game_state = Game_State.game;
          }
        if (!keyboard.IsKeyDown (Keys.Enter) && key_enter == true) key_enter = false;
        }

      // MAP CREATION
      else if (game_state == Game_State.creation)
        {
        if (keyboard.IsKeyDown (Keys.Space) && key_space == false)
          {
          key_space = true;
          if (observe_creation == true) load_map ();
          }
        else if (!keyboard.IsKeyDown (Keys.Space) && key_space == true) key_space = false;
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void Mouse_Input(GameTime game_time)
      {
      //double h_distance;
      //Rectangle r;
      //int b;

      mouse_last = mouse_current;
      mouse_current = Mouse.GetState();

      // if cursor moved, changed direction facing
      if (mouse_current.X != mouse_last.X || mouse_current.Y != mouse_last.Y)
        {
        //character_control.character[PLAYER].dir = get_direction (character_control.character[PLAYER].x, character_control.character[PLAYER].y, mouse_current.X - scroll_x, screen.height - (character_control.character[PLAYER].z / 2) + scroll_y - mouse_current.Y);
        }

      // left mouse button - move to point
      if (mouse_current.LeftButton == ButtonState.Pressed)
        {
        if (game_state == Game_State.title)
          {
          foreach (Text_Box box in text_boxes)
            {
            if (mouse_over(box.box))
              {
              box_click(box);
              break;
              }
            }
          }

    //    /*
    //    if (game_state == Game_State.game && character_control.active (PLAYER))
    //      {
    //      if (character_control.character[PLAYER].action != Action.grab && character_control.character[PLAYER].action != Action.push)
    //        {
    //        // distance between mouse and player
    //        h_distance = distance2d (character_control.character[PLAYER].x, character_control.character[PLAYER].y, mouse_current.X - scroll_x, screen.height - (character_control.character[PLAYER].z / 2) + scroll_y - mouse_current.Y);

    //        // round to zero to prevent jittering
    //        if (h_distance >= 20)
    //          {
    //          // get radians of direction
    //          character_control.character[PLAYER].dir = get_direction (character_control.character[PLAYER].x, character_control.character[PLAYER].y, mouse_current.X - scroll_x, screen.height - (character_control.character[PLAYER].z / 2) + scroll_y - mouse_current.Y);
    //          character_control.character[PLAYER].self_velocity = (h_distance * .015) * (character_control.character[PLAYER].speed * .3);
    //          if (character_on_ground (PLAYER)) character_control.character[PLAYER].walk ();
    //          }
    //        else if (character_control.character[PLAYER].action == Action.walk || character_control.character[PLAYER].action == Action.run) character_control.character[PLAYER].stand ();
    //        }

    //      // grabbing or pushing box
    //      else if (character_control.character[PLAYER].action == Action.grab && brush_control.brush[character_control.character[PLAYER].brush_grab].moveable == true)
    //        {
    //        if (character_control.character[PLAYER].grab_position == "below")  // up
    //          {
    //          character_control.character[PLAYER].action = Action.push;
    //          b = character_control.character[PLAYER].brush_grab;
    //          brush_control.brush[b].moving = true;
    //          character_control.character[PLAYER].dx = brush_control.brush[b].x + (brush_control.brush[b].width / 2);
    //          character_control.character[PLAYER].dy = brush_control.brush[b].y - (tilesize / 3);
    //          character_control.character[PLAYER].push_x = character_control.character[PLAYER].x;
    //          character_control.character[PLAYER].push_y = character_control.character[PLAYER].y + box_move;
    //          character_control.character[PLAYER].push_dir = "up";
    //          character_control.character[PLAYER].self_x_velocity = 0;
    //          }
    //        else if (character_control.character[PLAYER].grab_position == "above")  // down
    //          {
    //          character_control.character[PLAYER].action = Action.push;
    //          b = character_control.character[PLAYER].brush_grab;
    //          brush_control.brush[b].moving = true;
    //          character_control.character[PLAYER].dx = brush_control.brush[b].x + (brush_control.brush[b].width / 2);
    //          character_control.character[PLAYER].dy = brush_control.brush[b].y + brush_control.brush[b].length + (tilesize / 4);
    //          character_control.character[PLAYER].push_x = character_control.character[PLAYER].x;
    //          character_control.character[PLAYER].push_y = character_control.character[PLAYER].y - box_move;
    //          character_control.character[PLAYER].push_dir = "down";
    //          character_control.character[PLAYER].self_x_velocity = 0;
    //          }
    //        else if (character_control.character[PLAYER].grab_position == "right")  // left
    //          {
    //          character_control.character[PLAYER].action = Action.push;
    //          b = character_control.character[PLAYER].brush_grab;
    //          brush_control.brush[b].moving = true;
    //          character_control.character[PLAYER].dx = brush_control.brush[b].x + brush_control.brush[b].width + (tilesize / 3);
    //          character_control.character[PLAYER].dy = brush_control.brush[b].y + (brush_control.brush[b].length / 2);
    //          character_control.character[PLAYER].push_x = character_control.character[PLAYER].x - box_move;
    //          character_control.character[PLAYER].push_y = character_control.character[PLAYER].y;
    //          character_control.character[PLAYER].push_dir = "left";
    //          character_control.character[PLAYER].self_y_velocity = 0;
    //          }
    //        else if (character_control.character[PLAYER].grab_position == "left")  // right
    //          {
    //          character_control.character[PLAYER].action = Action.push;
    //          b = character_control.character[PLAYER].brush_grab;
    //          brush_control.brush[b].moving = true;
    //          character_control.character[PLAYER].dx = brush_control.brush[b].x - (tilesize / 3);
    //          character_control.character[PLAYER].dy = brush_control.brush[b].y + (brush_control.brush[b].length / 2);
    //          character_control.character[PLAYER].push_x = character_control.character[PLAYER].x + box_move;
    //          character_control.character[PLAYER].push_y = character_control.character[PLAYER].y;
    //          character_control.character[PLAYER].push_dir = "right";
    //          character_control.character[PLAYER].self_y_velocity = 0;
    //          }
    //        }
    //      }
    //    */
        
        if (game_state == Game_State.game && game_menu == true)
          {
          if (menu_screen == "main")
            {
            if (mouse_over(menu_exit_v, menu_exit_test)) Exit();
            }
          }

        if (mouse_left == false)
          {
          mouse_left = true;
          }
        }

      // left mouse button released
      if (mouse_current.LeftButton == ButtonState.Released & mouse_left == true)
        {
        mouse_left = false;
        if (game_state == Game_State.game && PLAYER > -1 && character_control.active(PLAYER) && character_control.character[PLAYER].action != Action.push) character_control.character[PLAYER].stand();
        }

      // right mouse button - shoot rock
      if (mouse_current.RightButton == ButtonState.Pressed)
        {
        if (mouse_right == false)
          {
          mouse_right = true;

          //if (character_control.active (PLAYER)) character_shoot_rock (PLAYER, character_control.character[PLAYER].dir);
          }
        }

      // right mouse button released
      if (mouse_current.RightButton == ButtonState.Released) mouse_right = false;

      // wheel up
      if (mouse_current.ScrollWheelValue > mouse_last.ScrollWheelValue)
        {
        }

      // wheel down
      if (mouse_current.ScrollWheelValue < mouse_last.ScrollWheelValue)
        {
        }

      // wheel click
      if (mouse_current.MiddleButton == ButtonState.Pressed)
        {
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    bool mouse_over(Vector2 location, Texture2D sprite)
      {
      // checks if mouse is over object's sprite

      MouseState current_mouse = Mouse.GetState();

      return (current_mouse.X >= location.X && current_mouse.X <= location.X + sprite.Width
          && current_mouse.Y >= location.Y && current_mouse.Y <= location.Y + sprite.Height);
      }

    ////////////////////////////////////////////////////////////////////////////////

    bool mouse_over(Rectangle r)
      {
      // checks if mouse is over rectangle-defined area

      mouse_current = Mouse.GetState();

      return (mouse_current.X >= r.X && mouse_current.X <= r.X + r.Width
          && mouse_current.Y >= r.Y && mouse_current.Y <= r.Y + r.Height);
      }

    ////////////////////////////////////////////////////////////////////////////////

    void box_click(Text_Box box)
      {
      switch (box.click)
        {
        case Click.new_game:
          game_state = Game_State.game;
          break;

        case Click.exit:
          Exit();
          break;
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void Controller_Input()
      {
      double h_distance;

      controller = GamePad.GetState(PlayerIndex.One);

      if (controller.IsConnected)
        {
        // left stick
        if (controller.ThumbSticks.Left.X < 0 || controller.ThumbSticks.Left.X > 0 ||
            controller.ThumbSticks.Left.Y < 0 || controller.ThumbSticks.Left.Y > 0)
          {
          controller_left_stick = true;

          if (character_control.active(PLAYER))
            {
            // stick range = -1 to 1.  max stick should be max running speed.
            // diagonally, h_distance is about -.9 to .9, so we add .1 to adjust.
            // character movement code will reduce to max_self_velocity if too high
            h_distance = distance2d(0, 0, controller.ThumbSticks.Left.X, controller.ThumbSticks.Left.Y);
            character_control.character[PLAYER].self_velocity = character_control.character[PLAYER].max_self_velocity * (h_distance + .1);
            character_control.character[PLAYER].dir = get_direction(0, 0, controller.ThumbSticks.Left.X, controller.ThumbSticks.Left.Y);
            if (character_on_ground(PLAYER)) character_control.character[PLAYER].walk();
            }
          }

        // left stick neutral
        else if (controller.ThumbSticks.Left.X == 0 && controller.ThumbSticks.Left.Y == 0)
          {
          if (controller_left_stick == true)
            {
            controller_left_stick = false;

            if (character_control.active(PLAYER) && (character_control.character[PLAYER].action == Action.walk || character_control.character[PLAYER].action == Action.run)) character_control.character[PLAYER].stand();  //character_control.character[PLAYER].self_velocity = 0;
            }
          }

        // X - punch, swing
        if (controller.Buttons.X == ButtonState.Pressed && controller_x == false)
          {
          controller_x = true;
          if (character_control.active(PLAYER))
            {
            if (character_on_ground(PLAYER)) character_punch(PLAYER);
            else character_jump_kick(PLAYER);
            }
          }
        else if (controller.Buttons.X == ButtonState.Released && controller_x == true)
          {
          controller_x = false;
          if (character_control.character[PLAYER].action == Action.punch) character_control.character[PLAYER].anim_frame_sequence = punch_rest_delay;
          }

        // A - jump
        if (controller.Buttons.A == ButtonState.Pressed && controller_a == false)
          {
          controller_a = true;

          if (character_control.active(PLAYER)) character_jump(PLAYER);
          }
        else if (controller.Buttons.A == ButtonState.Released && controller_a == true) controller_a = false;

        // B - push box / counter move
        if (controller.Buttons.B == ButtonState.Pressed && controller_b == false)
          {
          controller_b = true;
          if (character_control.character[PLAYER].action != Action.grab && character_control.character[PLAYER].action != Action.push) character_grab_brush(PLAYER);
          }
        else if (controller.Buttons.B == ButtonState.Released && controller_b == true)
          {
          controller_b = false;
          if (game_state == Game_State.game)
            {
            character_control.character[PLAYER].brush_grab = -1;
            if (character_control.character[PLAYER].action == Action.grab || character_control.character[PLAYER].action == Action.push) character_control.character[PLAYER].stand();
            }
          }

        // Y - special move
        if (controller.Buttons.Y == ButtonState.Pressed && controller_y == false)
          {
          controller_y = true;

          begin_special_attack();
          }
        else if (controller.Buttons.Y == ButtonState.Released && controller_y == true) controller_y = false;

        // right trigger - shoot, throw

        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void player_move ()
      {
      if (character_control.character[PLAYER].action != Action.grab && character_control.character[PLAYER].action != Action.push)  // just walking (off grid)
        {
        if (character_control.character[PLAYER].runboost == true)
          {
          character_control.character[PLAYER].self_velocity = character_control.character[PLAYER].max_self_velocity;
          if (character_on_ground (PLAYER) && character_control.character[PLAYER].action != Action.skid)
            {
            character_control.character[PLAYER].run ();
            if (character_control.character[PLAYER].action == Action.none) character_control.character[PLAYER].switch_animation (Action.run);
            }
          }
        else
          {
          character_control.character[PLAYER].self_velocity = character_control.character[PLAYER].speed * .3;
          if (character_on_ground (PLAYER) && character_control.character[PLAYER].action != Action.skid)
            {
            character_control.character[PLAYER].walk ();
            if (character_control.character[PLAYER].action == Action.none) character_control.character[PLAYER].switch_animation (Action.walk);
            }
          }
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    }
  }
