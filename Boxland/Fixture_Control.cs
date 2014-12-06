using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Boxland
  {
  public class Fixture_Control
    {
    public const int max_fixtures = 50;
    public const int max_fixture_sprites = 18;
    public List<Fixture> fixture = new List<Fixture> ();
    public Texture2D[] fixture_sprite = new Texture2D[max_fixture_sprites];
    int tilesize;

    // FIXTURE SPRITES (furniture & machines)
    public enum F
      {
      CHAIR1_SOUTH_TEST,
      CONVEYOR_NORTH_TEST,
      CONVEYOR_SOUTH_TEST,
      CONVEYOR_EAST_TEST,
      CONVEYOR_WEST_TEST,
      COUCH_TEST,
      FILING_TEST,
      LASER_HORIZONTAL_TEST,
      LASER_HORIZONTAL_GREEN_TEST,
      PLANT1_TEST,
      TABLE1_TEST,
      TABLE2_TEST,
      TV1_TEST,
      VENDING_TEST,
      WIRES,
      WIRES_HORIZONTAL_TEST,
      WIRES_VERTICAL_TEST,
      WIRES_SOUTHEAST_TEST
      }

    public Fixture_Control (int new_tilesize)
      {
      tilesize = new_tilesize;
      }

    ////////////////////////////////////////////////////////////////////////////////

    public int point_collide (int x, int y, int z)
      {
      int f = 0;
      int clip = -1;

      while (clip == -1 && f < fixture.Count)//total_fixtures)
        {
        if (x >= fixture[f].x && x <= fixture[f].x + fixture[f].width &&
            y >= fixture[f].y && y <= fixture[f].y + fixture[f].length &&
            z >= fixture[f].z && z <= fixture[f].z + fixture[f].height)
          clip = f;
        f += 1;
        }
      return clip;
      }

    ////////////////////////////////////////////////////////////////////////////////

    public void draw (int f, SpriteBatch spriteBatch, Screen screen)
      {
      Vector2 v_draw;
      Rectangle r_source;
      float opacity;
      Random rnd = new Random ();

      v_draw.X = fixture[f].x + screen.scroll_x;
      v_draw.Y = (screen.height - fixture[f].y - fixture[f].length) - (fixture[f].z / 2) - (fixture[f].height / 2) + screen.scroll_y;

      if (fixture[f].total_frames > 1)  // animated fixtures
        {
        r_source.X = 1 + ((fixture[f].width + 1) * fixture[f].current_frame);
        r_source.Y = 1;
        r_source.Width = fixture[f].sprite_width;
        r_source.Height = fixture[f].sprite_height;
        //v_draw.X = fixture[f].x + screen.scroll_x;
        //v_draw.Y = (screen.height - fixture[f].y - fixture[f].length) - (fixture[f].z / 2) - (fixture[f].height / 2) + screen.scroll_y;

        spriteBatch.Draw (fixture_sprite[fixture[f].type], v_draw, r_source, Color.White);
        }
      else  // non-animated
        {
        //v_draw.X = fixture[f].x + screen.scroll_x;
        //v_draw.Y = (screen.height - fixture[f].y - fixture[f].length) - (fixture[f].z / 2) - (fixture[f].height / 2) + screen.scroll_y;

        if (fixture[f].type == (int) F.PLANT1_TEST) v_draw.Y -= 24;
        if (fixture[f].type == (int) F.LASER_HORIZONTAL_GREEN_TEST) spriteBatch.Draw (fixture_sprite[(int) F.LASER_HORIZONTAL_TEST], v_draw, Color.White);
        if (fixture[f].type == (int) F.LASER_HORIZONTAL_GREEN_TEST) opacity = Convert.ToSingle (rnd.Next (3, 8)) / 10;
        else opacity = 1f;
        spriteBatch.Draw (fixture_sprite[fixture[f].type], v_draw, Color.White * opacity);
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    public void add (int sprite_number, int x, int y, int z)
      {
      if (fixture.Count < max_fixtures)
        {
        Fixture newfix = new Fixture ();

        // defaults
        newfix.width = tilesize;
        newfix.length = tilesize;
        newfix.height = tilesize;
        newfix.x = x;
        newfix.y = y;
        newfix.z = z;
        newfix.electric = false;
        newfix.powered = false;
        newfix.on = true;
        newfix.has_switch = false;
        newfix.type = sprite_number;
        newfix.total_frames = 0;
        newfix.frame_delay = 4;
        newfix.frame_counter = 0;
        newfix.sprite_width = tilesize;
        newfix.sprite_height = tilesize;
        newfix.solid = true;

        switch (sprite_number)
          {
          case (int) F.CHAIR1_SOUTH_TEST:
            newfix.width = fixture_sprite[sprite_number].Width;
            newfix.length = fixture_sprite[sprite_number].Width;
            newfix.x = x + (tilesize / 4);
            newfix.y = y - (tilesize / 4);
            break;

          case (int) F.COUCH_TEST:
            newfix.width = 288;
            newfix.length = 96;
            newfix.height = 48;
            newfix.y = y + (tilesize - newfix.length);
            break;

          case (int) F.CONVEYOR_NORTH_TEST:
            newfix.total_frames = 8;
            break;

          case (int) F.CONVEYOR_SOUTH_TEST:
            newfix.total_frames = 8;
            break;

          case (int) F.CONVEYOR_WEST_TEST:
            newfix.total_frames = 8;
            break;
          
          case (int) F.CONVEYOR_EAST_TEST:
            newfix.total_frames = 8;
            break;
          
          case (int) F.FILING_TEST:
            newfix.width = 96;
            newfix.length = 48;
            newfix.height = 96;
            newfix.y = y + (tilesize - newfix.length);
            break;
          
          case (int) F.LASER_HORIZONTAL_GREEN_TEST:
            newfix.length = 32;
            newfix.height = 16;
            newfix.y = y + (tilesize / 4);
            newfix.solid = false;
            newfix.electric = true;
            break;
          
          case (int) F.PLANT1_TEST:
            newfix.width = 72;
            newfix.length = 72;
            newfix.height = 96;
            newfix.y = y + (tilesize - newfix.length);
            break;
          
          case (int) F.TABLE1_TEST:
            newfix.width = tilesize;
            newfix.length = tilesize * 2;
            newfix.height = tilesize / 2;
            newfix.y = y + (tilesize - newfix.length);
            break;
          
          case (int) F.TABLE2_TEST:
            newfix.width = 192;
            newfix.length = 96;
            newfix.height = 48;
            newfix.y = y + (tilesize - newfix.length);
            break;
          
          case (int) F.TV1_TEST:
            newfix.width = 96;
            newfix.length = 48;
            newfix.height = 48;
            newfix.y = y + (tilesize - newfix.length);
            break;
          
          case (int) F.VENDING_TEST:
            newfix.width = 58;
            newfix.length = 32;
            newfix.height = 96;
            newfix.x = x + (tilesize / 2) - (newfix.width / 2);
            newfix.y = y + (tilesize - newfix.length);
            newfix.z = z;
            newfix.electric = true;
            newfix.powered = true;
            break;

          case (int) F.WIRES:
            newfix.z -= 8;
            newfix.height = 8;
            newfix.solid = false;
            newfix.electric = true;
            break;          
          }

        fixture.Add (newfix);
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    //  needs access to particle effects and brush collisions first

    //public void update ()
    //  {
    //  Random rnd = new Random();
    //  int f2, q, b_clip;

    //  for (int f = 0; f < fixture.Count; f += 1)
    //    {
    //    if (fixture[f].total_frames > 1)
    //      {
    //      fixture[f].frame_counter += 1;
    //      if (fixture[f].frame_counter >= fixture[f].frame_delay)
    //        {
    //        fixture[f].frame_counter = 0;
    //        fixture[f].current_frame += 1;
    //        if (fixture[f].current_frame >= fixture[f].total_frames) fixture[f].current_frame = 0;
    //        }
    //      }

    //    // laser tripwires
    //    if (fixture[f].type == (int) F.LASER_HORIZONTAL_GREEN_TEST)
    //      {
    //      b_clip = fixture_in_brush (fixture[f]);
    //      if (b_clip > -1) fixture[f].powered = true;
    //      }

    //    // power
    //    if (fixture[f].electric == true && fixture[f].powered == false)
    //      {
    //      //    //f2 = point_collide (fixture[f].x + fixture[f].width + (tilesize / 2), fixture[f].y + (fixture[f].length / 2), fixture[f].z);
    //      //    //if (f2 > -1)// && fixture[f2].electric == true)// && fixture[f2].powered == true)
    //      //    //fixture[f].powered = true;
    //      //    //else
    //      f2 = point_collide (fixture[f].x + fixture[f].width + (tilesize / 2), fixture[f].y + (fixture[f].length / 2), fixture[f].z + 8);
    //      if (f2 > -1 && fixture[f2].type == (int) F.LASER_HORIZONTAL_GREEN_TEST && fixture[f2].electric == true && fixture[f2].powered == true) fixture[f].powered = true;
    //      //    //else f2 = point_collide (fixture[f].x + fixture[f].width + (tilesize / 2), fixture[f].y + (fixture[f].length / 2), fixture[f].z + (tilesize / 2));
    //      //    //if (f2 > -1)// && fixture[f2].electric == true)// && fixture[f2].powered == true) 
    //      //    //  fixture[f].powered = true;
    //      //    //else f2 = point_collide (fixture[f].x + fixture[f].width + (tilesize / 2), fixture[f].y + (fixture[f].length / 2), fixture[f].z + tilesize);
    //      //    //if (f2 > -1)// && fixture[f2].electric == true)// && fixture[f2].powered == true) 
    //      //    //  fixture[f].powered = true;
    //      }

    //    // particles
    //    if (fixture[f].type == (int) F.LASER_HORIZONTAL_GREEN_TEST)
    //      {
    //      if (rnd.Next (0, 50) == 0) particle_green_tripwire (fixture[f].x + 6, fixture[f].y + 17, fixture[f].z + fixture[f].height, 0);
    //      if (rnd.Next (0, 50) == 0) particle_green_tripwire (fixture[f].x + fixture[f].width - 6, fixture[f].y + 17, fixture[f].z + fixture[f].height, 180);
    //      }
    //    }  // for (int f = 0
    //  }
    }
  }
