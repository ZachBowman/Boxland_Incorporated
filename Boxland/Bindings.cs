using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Text;

namespace Boxland
  {
  public partial class Boxland : Game
    {  
    class Binding
      {
      public Key_Action action;
      public Keys key_binding;
      public Mouse_Button mouse_binding;
      public Controller_Button controller_binding;

      public Binding (Key_Action new_action, Keys new_key)
        {
        action = new_action;
        key_binding = new_key;
        mouse_binding = Mouse_Button.none;
        controller_binding = Controller_Button.none;
        }

      public Binding (Key_Action new_action, Mouse_Button new_button)
        {
        action = new_action;
        key_binding = Keys.Kanji;
        mouse_binding = new_button;
        controller_binding = Controller_Button.none;
        }

      public Binding (Key_Action new_action, Controller_Button new_button)
        {
        action = new_action;
        key_binding = Keys.Kanji;
        mouse_binding = Mouse_Button.none;
        controller_binding = new_button;
        }
      }

    class Key_Bindings
      {
      Movement move;
      public List<Binding> bindings = new List<Binding> ();

      public Key_Bindings ()
        {
        assign_defaults (Preset.keyboard1);
        }

      void assign_defaults (Preset preset)
        {
        bindings.Clear ();

        if (preset == Preset.keyboard1)
          {
          move = Movement.keyboard;

          bindings.Add (new Binding (Key_Action.up, Keys.Up));
          bindings.Add (new Binding (Key_Action.down, Keys.Down));
          bindings.Add (new Binding (Key_Action.left, Keys.Left));
          bindings.Add (new Binding (Key_Action.right, Keys.Right));
          bindings.Add (new Binding (Key_Action.jump, Keys.Space));
          bindings.Add (new Binding (Key_Action.attack, Keys.LeftControl));
          bindings.Add (new Binding (Key_Action.run, Keys.LeftShift));
          bindings.Add (new Binding (Key_Action.run, Keys.Z));
          bindings.Add (new Binding (Key_Action.grab, Keys.LeftAlt));
          bindings.Add (new Binding (Key_Action.grab, Keys.X));
          bindings.Add (new Binding (Key_Action.menu, Keys.Escape));
          }
        else if (preset == Preset.keyboard1)
          {
          move = Movement.keyboard;

          bindings.Add (new Binding (Key_Action.up, Keys.W));
          bindings.Add (new Binding (Key_Action.down, Keys.S));
          bindings.Add (new Binding (Key_Action.left, Keys.A));
          bindings.Add (new Binding (Key_Action.right, Keys.D));
          bindings.Add (new Binding (Key_Action.jump, Keys.Space));
          bindings.Add (new Binding (Key_Action.attack, Keys.RightControl));
          bindings.Add (new Binding (Key_Action.run, Keys.LeftShift));
          bindings.Add (new Binding (Key_Action.run, Keys.RightShift));
          bindings.Add (new Binding (Key_Action.grab, Keys.RightAlt));
          bindings.Add (new Binding (Key_Action.menu, Keys.Escape));
          }
        }
      }
    }
  }
