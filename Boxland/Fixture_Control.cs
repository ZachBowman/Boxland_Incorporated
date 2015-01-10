using System;
using System.Collections.Generic;
using System.IO;
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

    public void load_sprites (GraphicsDevice GraphicsDevice)
      {
      fixture_sprite[(int) F.CHAIR1_SOUTH_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "fixtures\\chair1_south_test.png", FileMode.Open, FileAccess.Read));
      fixture_sprite[(int) F.CONVEYOR_NORTH_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "fixtures\\conveyor_north_test.png", FileMode.Open, FileAccess.Read));
      fixture_sprite[(int) F.CONVEYOR_SOUTH_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "fixtures\\conveyor_south_test.png", FileMode.Open, FileAccess.Read));
      fixture_sprite[(int) F.CONVEYOR_EAST_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "fixtures\\conveyor_east_test.png", FileMode.Open, FileAccess.Read));
      fixture_sprite[(int) F.CONVEYOR_WEST_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "fixtures\\conveyor_west_test.png", FileMode.Open, FileAccess.Read));
      fixture_sprite[(int) F.COUCH_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "fixtures\\couch_test.png", FileMode.Open, FileAccess.Read));
      fixture_sprite[(int) F.FILING_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "fixtures\\filing_cabinet_test.png", FileMode.Open, FileAccess.Read));
      fixture_sprite[(int) F.LASER_HORIZONTAL_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "fixtures\\laser_horizontal_test.png", FileMode.Open, FileAccess.Read));
      fixture_sprite[(int) F.LASER_HORIZONTAL_GREEN_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "fixtures\\laser_horizontal_green_test.png", FileMode.Open, FileAccess.Read));
      fixture_sprite[(int) F.PLANT1_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "fixtures\\plant1_test.png", FileMode.Open, FileAccess.Read));
      fixture_sprite[(int) F.TABLE1_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "fixtures\\table1_test.png", FileMode.Open, FileAccess.Read));
      fixture_sprite[(int) F.TABLE2_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "fixtures\\table2_test.png", FileMode.Open, FileAccess.Read));
      fixture_sprite[(int) F.TV1_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "fixtures\\tv1_test.png", FileMode.Open, FileAccess.Read));
      fixture_sprite[(int) F.VENDING_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "fixtures\\vending_test.png", FileMode.Open, FileAccess.Read));
      fixture_sprite[(int) F.WIRES_HORIZONTAL_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "fixtures\\wires_horizontal_test.png", FileMode.Open, FileAccess.Read));
      fixture_sprite[(int) F.WIRES_VERTICAL_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "fixtures\\wires_vertical_test.png", FileMode.Open, FileAccess.Read));
      fixture_sprite[(int) F.WIRES_SOUTHEAST_TEST] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "fixtures\\wires_southeast_test.png", FileMode.Open, FileAccess.Read));

      ConvertToPremultipliedAlpha (fixture_sprite[(int) F.CHAIR1_SOUTH_TEST], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (fixture_sprite[(int) F.CONVEYOR_EAST_TEST], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (fixture_sprite[(int) F.CONVEYOR_WEST_TEST], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (fixture_sprite[(int) F.COUCH_TEST], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (fixture_sprite[(int) F.LASER_HORIZONTAL_TEST], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (fixture_sprite[(int) F.LASER_HORIZONTAL_GREEN_TEST], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (fixture_sprite[(int) F.PLANT1_TEST], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (fixture_sprite[(int) F.TABLE1_TEST], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (fixture_sprite[(int) F.TABLE2_TEST], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (fixture_sprite[(int) F.TV1_TEST], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (fixture_sprite[(int) F.WIRES_HORIZONTAL_TEST], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (fixture_sprite[(int) F.WIRES_VERTICAL_TEST], new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (fixture_sprite[(int) F.WIRES_SOUTHEAST_TEST], new Color (255, 0, 255, 255));
      }

    ////////////////////////////////////////////////////////////////////////////////

    void ConvertToPremultipliedAlpha (Texture2D texture, Color? colorKey)
      {
      Color[] data = new Color[texture.Width * texture.Height];
      texture.GetData<Color> (data, 0, data.Length);
      if (colorKey.HasValue)
        {
        for (int i = 0; i < data.Length; i += 1)
          {
          if (data[i] == colorKey)
            {
            data[i] = Color.Transparent;
            }
          else
            {
            data[i] = new Color (new Vector4 (data[i].ToVector3 () * (data[i].A / 255f), (data[i].A / 255f)));
            }
          }
        }
      else
        {
        for (int i = 0; i < data.Length; i += 1)
          {
          data[i] = new Color (new Vector4 (data[i].ToVector3 () * (data[i].A / 255f), (data[i].A / 255f)));
          }
        }
      texture.SetData<Color> (data, 0, data.Length);
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
            //newfix.z = z;
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

    public int point_collide (int x, int y, int z)
      {
      int f = 0;
      int clip = -1;

      while (clip == -1 && f < fixture.Count)
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

    public void draw (int f, SpriteBatch spriteBatch, ScreenInfo screen)
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

    ////////////////////////////////////////////////////////////////////////////////

    public int fixture_in_brush (Fixture f, List<Brush> brush)
      {
      int b = 0;
      int clip = -1;

      while (clip == -1 && b < brush.Count)
        {
        if (f.x + f.width >= brush[b].x && f.x <= brush[b].x + brush[b].width
            && f.y + f.length >= brush[b].y && f.y <= brush[b].y + brush[b].length
            && f.z + f.height >= brush[b].z && f.z <= brush[b].z + brush[b].height - 1)
          clip = b;
        b += 1;
        }

      return clip;
      }

    ////////////////////////////////////////////////////////////////////////////////

    public int point_in_fixture (int x, int y, int z)
      {
      int f = 0;
      int clip = -1;

      while (clip == -1 && f < fixture.Count)
        {
        if (x >= fixture[f].x && x <= fixture[f].x + fixture[f].width &&
            y >= fixture[f].y && y <= fixture[f].y + fixture[f].length &&
            z >= fixture[f].z && z <= fixture[f].z + fixture[f].height)
          {
          clip = f;
          break;  // stop looking
          }
        f += 1;
        }
      return clip;
      }

    //////////////////////////////////////////////////////////////////////////////////

    public int fixture_west_of_brush (Brush check_brush)
      {
      int clip;

      clip = point_in_fixture (check_brush.x + 4 - tilesize, check_brush.y + tilesize - 4, check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x - (tilesize / 2), check_brush.y + tilesize - 4, check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x - 4, check_brush.y + tilesize - 4, check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x + 4 - tilesize, check_brush.y + (tilesize / 2), check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x - (tilesize / 2), check_brush.y + (tilesize / 2), check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x - 4, check_brush.y + (tilesize / 2), check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x + 4 - tilesize, check_brush.y + 4, check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x - (tilesize / 2), check_brush.y + 4, check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x - 4, check_brush.y + 4, check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      return -1;
      }

    //////////////////////////////////////////////////////////////////////////////////

    public int fixture_east_of_brush (Brush check_brush)
      {
      int clip;

      clip = point_in_fixture (check_brush.x + 4 + (tilesize * 2), check_brush.y + tilesize - 4, check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x + (tilesize * 3 / 2), check_brush.y + tilesize - 4, check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x - 4, check_brush.y + tilesize - 4, check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x + 4 + (tilesize * 2), check_brush.y + (tilesize / 2), check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x - (tilesize * 3 / 2), check_brush.y + (tilesize / 2), check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x - 4, check_brush.y + (tilesize / 2), check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x + 4 + (tilesize * 2), check_brush.y + 4, check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x - (tilesize * 3 / 2), check_brush.y + 4, check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x - 4, check_brush.y + 4, check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      return -1;
      }

    //////////////////////////////////////////////////////////////////////////////////

    public int fixture_north_of_brush (Brush check_brush)
      {
      int clip;

      clip = point_in_fixture (check_brush.x + 4, check_brush.y + (tilesize * 2) - 4, check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x + (tilesize / 2), check_brush.y + (tilesize * 2) - 4, check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x + tilesize - 4, check_brush.y + (tilesize * 2) - 4, check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x + 4, check_brush.y + (tilesize * 3 / 2), check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x + (tilesize / 2), check_brush.y + (tilesize * 3 / 2), check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x + tilesize - 4, check_brush.y + (tilesize * 3 / 2), check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x + 4, check_brush.y + tilesize + 4, check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x + (tilesize / 2), check_brush.y + tilesize + 4, check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x + tilesize - 4, check_brush.y + tilesize + 4, check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      return -1;
      }

    //////////////////////////////////////////////////////////////////////////////////

    public int fixture_south_of_brush (Brush check_brush)
      {
      int clip;

      clip = point_in_fixture (check_brush.x + 4, check_brush.y - 4, check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;
      
      clip = point_in_fixture (check_brush.x + (tilesize / 2), check_brush.y - 4, check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x + tilesize - 4, check_brush.y - 4, check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x + 4, check_brush.y - (tilesize / 2), check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x + (tilesize / 2), check_brush.y - (tilesize / 2), check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x + tilesize - 4, check_brush.y - (tilesize / 2), check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x + 4, check_brush.y - tilesize + 4, check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x + (tilesize / 2), check_brush.y - tilesize + 4, check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x + tilesize - 4, check_brush.y - tilesize + 4, check_brush.z + (tilesize / 4));
      if (clip > -1) return clip;

      return -1;
      }

    //////////////////////////////////////////////////////////////////////////////////

    public int fixture_above_brush (Brush check_brush)
      {
      int clip;

      clip = point_in_fixture (check_brush.x + 4, check_brush.y + tilesize - 4, check_brush.z + tilesize + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x + (tilesize / 2), check_brush.y + tilesize - 4, check_brush.z + tilesize + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x + tilesize - 4, check_brush.y + tilesize - 4, check_brush.z + tilesize + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x + 4, check_brush.y + (tilesize / 2), check_brush.z + tilesize + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x + (tilesize / 2), check_brush.y + (tilesize / 2), check_brush.z + tilesize + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x + tilesize - 4, check_brush.y + (tilesize / 2), check_brush.z + tilesize + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x + 4, check_brush.y + 4, check_brush.z + tilesize + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x + (tilesize / 2), check_brush.y + 4, check_brush.z + tilesize + (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (check_brush.x + tilesize - 4, check_brush.y + 4, check_brush.z + tilesize + (tilesize / 4));
      if (clip > -1) return clip;

      return -1;
      }

    //////////////////////////////////////////////////////////////////////////////////

    // takes a center point of a grid cube and checks cube for fixtures
    
    public int fixture_in_gridspace (int x, int y, int z)
      {
      int clip;

      clip = point_in_fixture (x - (tilesize / 2) + 4, y + (tilesize / 2) - 4, z - (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (x, y + (tilesize / 2) - 4, z - (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (x + (tilesize / 2) - 4, y + (tilesize / 2) - 4, z - (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (x - (tilesize / 2) + 4, y, z - (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (x, y, z - (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (x + (tilesize / 2) - 4, y, z - (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (x - (tilesize / 2) + 4, y - (tilesize / 2) + 4, z - (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (x, y - (tilesize / 2) + 4, z - (tilesize / 4));
      if (clip > -1) return clip;

      clip = point_in_fixture (x + (tilesize / 2) - 4, y - (tilesize / 2) + 4, z - (tilesize / 4));
      if (clip > -1) return clip;

      return -1;
      }

    ////////////////////////////////////////////////////////////////////////////////

    public int character_on_fixture (Character c)
      {
      int fixture_below = point_collide (c.x + (c.width / 2), c.y + (c.length / 2), c.z - 1);
      if (fixture_below == -1) fixture_below = point_collide (c.x + 1, c.y + 1, c.z - 1);
      if (fixture_below == -1) fixture_below = point_collide (c.x + c.width - 1, c.y + 1, c.z - 1);
      if (fixture_below == -1) fixture_below = point_collide (c.x + 1, c.y + c.length - 1, c.z - 1);
      if (fixture_below == -1) fixture_below = point_collide (c.x + c.width - 1, c.y + c.length - 1, c.z - 1);

      return fixture_below;
      }
    }
  }
