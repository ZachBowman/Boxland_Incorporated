using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Boxland
  {
  public class Fixture_Control : Game
    {
    public const int max_fixtures = 100;
    public const int max_fixture_sprites = 33;
    public List<Fixture> fixture = new List<Fixture> ();
    public Texture2D[] fixture_sprite = new Texture2D[max_fixture_sprites];
    int tilesize;

    public Fixture_Control (int new_tilesize)
      {
      tilesize = new_tilesize;
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

    public void add (Fixture_Type type, int x, int y, int z)
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
        newfix.type = type;
        newfix.total_frames = 0;
        newfix.frame_delay = 4;
        newfix.frame_counter = 0;
        newfix.sprite_width = tilesize;
        newfix.sprite_height = tilesize;
        newfix.solid = true;
        newfix.door_type = Door.none;
        newfix.door_open = false;
        newfix.door_horizontal = true;
        newfix.door_keys = 0;

        switch (type)
          {
          case Fixture_Type.bench1_west_test:
            newfix.width = 48;
            newfix.length = 182;
            newfix.height = 48;
            newfix.x = x + tilesize - newfix.width;
            break;

          case Fixture_Type.chair1_south_test:
            newfix.width = fixture_sprite[(int) type].Width;
            newfix.length = fixture_sprite[(int) type].Width;
            newfix.x = x + (tilesize / 4);
            newfix.y = y - (tilesize / 4);
            break;

          case Fixture_Type.chair3_east_test:
            newfix.height = 48;
            break;

          case Fixture_Type.chair3_north_test:
            newfix.height = 48;
            break;

          case Fixture_Type.couch_south_test:
            newfix.width = 288;
            newfix.length = 96;
            newfix.height = 48;
            newfix.y = y + (tilesize - newfix.length);
            break;

          case Fixture_Type.conveyor_north_test:
            newfix.total_frames = 8;
            break;

          case Fixture_Type.conveyor_south_test:
            newfix.total_frames = 8;
            break;

          case Fixture_Type.conveyor_west_test:
            newfix.total_frames = 8;
            break;

          case Fixture_Type.conveyor_east_test:
            newfix.total_frames = 8;
            break;

          case Fixture_Type.desk1_test:
            newfix.width = 288;
            newfix.length = 96;
            newfix.height = 48;
            break;

          case Fixture_Type.door_mint_horizontal_test:
            newfix.width = 96;
            newfix.length = 32;
            newfix.height = 96;
            newfix.door_type = Door.swing;
            newfix.door_horizontal = true;
            break;
          
          case Fixture_Type.door_mint_vertical_test:
            newfix.width = 32;
            newfix.length = 96;
            newfix.height = 96;
            newfix.x -= 9;
            newfix.door_type = Door.swing;
            newfix.door_horizontal = false;
            break;
          
          case Fixture_Type.door_white_horizontal_test:
            newfix.length = 16;
            newfix.y = y + (tilesize / 2) - (newfix.length / 2);
            newfix.door_type = Door.swing;
            newfix.door_horizontal = true;
            break;
          
          case Fixture_Type.filing_test:
            newfix.width = 96;
            newfix.length = 48;
            newfix.height = 96;
            newfix.y = y + (tilesize - newfix.length);
            break;

          case Fixture_Type.keydoor_1_test:
            newfix.length = 21;
            newfix.y = y + (tilesize / 2) - (newfix.length / 2);
            newfix.door_type = Door.slide;
            newfix.door_horizontal = true;
            newfix.door_keys = 1;
            break;

          case Fixture_Type.keydoor_10_test:
            newfix.length = 21;
            newfix.y = y + (tilesize / 2) - (newfix.length / 2);
            newfix.door_type = Door.slide;
            newfix.door_horizontal = true;
            newfix.door_keys = 10;
            break;

          case Fixture_Type.laser_horizontal_green_test:
            newfix.length = 32;
            newfix.height = 16;
            newfix.y = y + (tilesize / 4);
            newfix.solid = false;
            newfix.electric = true;
            break;

          case Fixture_Type.phone1_test:
            newfix.width = 48;
            newfix.length = 48;
            break;
          
          case Fixture_Type.plant1_green_test:
            newfix.width = 72;
            newfix.length = 72;
            newfix.height = 96;
            newfix.y = y + (tilesize - newfix.length);
            break;

          case Fixture_Type.table1_test:
            newfix.width = tilesize;
            newfix.length = tilesize * 2;
            newfix.height = tilesize / 2;
            newfix.y = y + (tilesize - newfix.length);
            break;

          case Fixture_Type.table2_test:
            newfix.width = 192;
            newfix.length = 96;
            newfix.height = 48;
            newfix.y = y + (tilesize - newfix.length);
            break;

          case Fixture_Type.table3_mint_test:
            newfix.width = 96;
            newfix.length = 96;
            newfix.height = 48;
            break;

          case Fixture_Type.tv1_test:
            newfix.width = 96;
            newfix.length = 48;
            newfix.height = 48;
            newfix.y = y + (tilesize - newfix.length);
            break;

          case Fixture_Type.tv2_test:
            newfix.length = 48;
            newfix.height = 48;
            break;

          case Fixture_Type.vending_red_test:
            newfix.width = 58;
            newfix.length = 32;
            newfix.height = 96;
            newfix.x = x + (tilesize / 2) - (newfix.width / 2);
            newfix.y = y + (tilesize - newfix.length);
            newfix.electric = true;
            newfix.powered = true;
            break;

          case Fixture_Type.vending_yellow_test:
            newfix.width = 58;
            newfix.length = 32;
            newfix.height = 96;
            newfix.x = x + (tilesize / 2) - (newfix.width / 2);
            newfix.y = y + (tilesize - newfix.length);
            newfix.electric = true;
            newfix.powered = true;
            break;

          case Fixture_Type.wires:
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

    public void draw (int f, SpriteBatch spriteBatch, ScreenInfo screen, double parallax)
      {
      Vector2 v_draw;
      Rectangle r_source;
      float opacity;
      Random rnd = new Random ();
      Texture2D sprite = fixture_sprite[(int) fixture[f].type];

      v_draw.X = fixture[f].x + screen.scroll_x;
      v_draw.Y = Convert.ToInt32(screen.height - fixture[f].y - fixture[f].length - (fixture[f].z * parallax) - (fixture[f].height * parallax) + screen.scroll_y);

      if (fixture[f].total_frames > 1)  // animated fixtures
        {
        r_source.X = 1 + ((fixture[f].width + 1) * fixture[f].current_frame);
        r_source.Y = 1;
        r_source.Width = fixture[f].sprite_width;
        r_source.Height = fixture[f].sprite_height;
        //v_draw.X = fixture[f].x + screen.scroll_x;
        //v_draw.Y = (screen.height - fixture[f].y - fixture[f].length) - (fixture[f].z / 2) - (fixture[f].height / 2) + screen.scroll_y;

        spriteBatch.Draw (sprite, v_draw, r_source, Color.White);
        }
      else  // non-animated
        {
        if (fixture[f].type == Fixture_Type.door_mint_horizontal_test && fixture[f].door_open) sprite = fixture_sprite[(int) Fixture_Type.door_mint_vertical_test];
        if (fixture[f].type == Fixture_Type.door_mint_vertical_test && fixture[f].door_open) sprite = fixture_sprite[(int) Fixture_Type.door_mint_horizontal_test];
        if (fixture[f].type == Fixture_Type.plant1_green_test) v_draw.Y -= 24;

        // WHY IS GREEN LASER HERE TWICE?
        if (fixture[f].type == Fixture_Type.laser_horizontal_green_test) spriteBatch.Draw (fixture_sprite[(int) Fixture_Type.laser_fixture_horizontal_test], v_draw, Color.White);
        if (fixture[f].type == Fixture_Type.laser_horizontal_green_test) opacity = Convert.ToSingle (rnd.Next (3, 8)) / 10;

        else opacity = 1f;
        spriteBatch.Draw (sprite, v_draw, Color.White * opacity);
        }
      }

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

      // FIX THESE POINTS
      
      // lower left
      //clip = point_in_fixture (check_brush.x + 4 + (tilesize * 2), check_brush.y + 4, check_brush.z + (tilesize / 4));
      clip = point_in_fixture (check_brush.x + tilesize + 4, check_brush.y + 4, check_brush.z + 4);
      if (clip > -1) return clip;

      // lower middle
      //clip = point_in_fixture (check_brush.x - (tilesize * 3 / 2), check_brush.y + 4, check_brush.z + (tilesize / 4));
      clip = point_in_fixture (check_brush.x + (tilesize / 2 * 3), check_brush.y + 4, check_brush.z + 4);
      if (clip > -1) return clip;

      // lower right
      //clip = point_in_fixture (check_brush.x - 4, check_brush.y + 4, check_brush.z + (tilesize / 4));
      clip = point_in_fixture (check_brush.x + tilesize + tilesize - 4, check_brush.y + 4, check_brush.z + 4);
      if (clip > -1) return clip;

      // middle left
      //clip = point_in_fixture (check_brush.x + 4 + (tilesize * 2), check_brush.y + (tilesize / 2), check_brush.z + (tilesize / 4));
      clip = point_in_fixture (check_brush.x + tilesize + 4, check_brush.y + (tilesize / 2), check_brush.z + 4);
      if (clip > -1) return clip;

      // middle middle
      //clip = point_in_fixture (check_brush.x - (tilesize * 3 / 2), check_brush.y + (tilesize / 2), check_brush.z + (tilesize / 4));
      clip = point_in_fixture (check_brush.x + (tilesize * 3 / 2), check_brush.y + (tilesize / 2), check_brush.z + 4);
      if (clip > -1) return clip;

      // middle right
      //clip = point_in_fixture (check_brush.x - 4, check_brush.y + (tilesize / 2), check_brush.z + (tilesize / 4));
      clip = point_in_fixture (check_brush.x + tilesize + tilesize - 4, check_brush.y + (tilesize / 2), check_brush.z + 4);
      if (clip > -1) return clip;

      // upper left
      //clip = point_in_fixture (check_brush.x + (tilesize * 2) + 4, check_brush.y + tilesize - 4, check_brush.z + (tilesize / 4));
      clip = point_in_fixture (check_brush.x + tilesize + 4, check_brush.y + tilesize - 4, check_brush.z + 4);
      if (clip > -1) return clip;

      // upper middle
      //clip = point_in_fixture (check_brush.x + (tilesize * 3 / 2), check_brush.y + tilesize - 4, check_brush.z + (tilesize / 4));
      clip = point_in_fixture (check_brush.x + (tilesize / 2 * 3), check_brush.y + tilesize - 4, check_brush.z + 4);
      if (clip > -1) return clip;

      // upper right
      //clip = point_in_fixture (check_brush.x - 4, check_brush.y + tilesize - 4, check_brush.z + (tilesize / 4));
      clip = point_in_fixture (check_brush.x + tilesize + tilesize - 4, check_brush.y + tilesize - 4, check_brush.z + 4);
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

    //////////////////////////////////////////////////////////////////////////////////

    }
  }
