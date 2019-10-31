using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;

namespace Boxland
  {
  public partial class Boxland : Game
    {
      void particle_coinsparkle(int x, int y, int z)
      {
        //particle_effect[free_particle ()].create (light_sprite[(int) Light_Color.white], 1, x, y, z, 0, 0, 0, 0, 0, 0, 0, .5f, -.02f, 0, .06, .01, "light", "none", -1);
        particle_engine.create(light_sprite[(int)Light_Color.white], 1, x, y, z, 0, 0, 0, 0, 0, 0, 0, .5f, -.02f, 0, .06, .06, .01, "light", "none", -1);
      }

      ////////////////////////////////////////////////////////////////////////////////

      void particle_coingrab(int x, int y, int z)
      {
        //particle_effect[free_particle ()].create (effect_dollars, 1, x, y, z, 0, 0, 0, 0, 1.5, 0, 0, .6f, -.01f, 0, .6, .02, "light", "none", -1);
        //particle_effect[free_particle ()].create (light_sprite[(int) Light_Color.yellow], 1, x, y, z, 0, 0, 0, 0, 0, 0, 0, .5f, -.01f, 0, .2, .02, "light", "none", -1);

        particle_engine.create(effect_dollars, 1, x, y, z, 0, 0, 0, 0, 1.5, 0, 0, .6f, -.01f, 0, .6, .6, .02, "light", "none", -1);
        particle_engine.create(light_sprite[(int)Light_Color.yellow], 1, x, y, z, 0, 0, 0, 0, 0, 0, 0, .5f, -.01f, 0, .2, .2, .02, "light", "none", -1);
      }

      ////////////////////////////////////////////////////////////////////////////////

      void particle_itemgrab(int x, int y, int z, int direction)
      {
        //particle_effect[free_particle ()].create (light_sprite[(int) Light_Color.yellow], 1, x, y, z, direction, 0, 0, 0, 0, 0, 0, .5f, -.01f, 0, .2, .02, "light", "none", -1);
        particle_engine.create(light_sprite[(int)Light_Color.yellow], 1, x, y, z, direction, 0, 0, 0, 0, 0, 0, .5f, -.01f, 0, .2, .2, .02, "light", "none", -1);
      }

      ////////////////////////////////////////////////////////////////////////////////

      void particle_superpunch(int x, int y, int z, int direction)
      {
        //particle_effect[free_particle ()].create (light_sprite[(int) Light_Color.yellow], 1, x, y, z, direction, 0, 0, 0, 0, 0, 0, .5f, -.01f, 0, 1, 0, "light", "none", -1);
        particle_engine.create(light_sprite[(int)Light_Color.yellow], 1, x, y, z, direction, 0, 0, 0, 0, 0, 0, .5f, -.01f, 0, 1, 1, 0, "light", "none", -1);
      }

      ////////////////////////////////////////////////////////////////////////////////

      void particle_steam(int x, int y, int z, int direction)
      {
        //particle_effect[free_particle ()].create (effect_cold_energy, 1, x, y, z, 0, 3, 0, 0, .4, .1, 0, .3f, -.001f, 0, 1.75, .04, "air", "none", -1);
        particle_engine.create(effect_cold_energy, 1, x, y, z, 0, 3, 0, 0, .4, .1, 0, .3f, -.001f, 0, 1.75, 1.75, .04, "air", "none", -1);
      }

      ////////////////////////////////////////////////////////////////////////////////

      void particle_flames(int x, int y, int z, string source_type, int source)
      {
        //particle_effect[free_particle ()].create (effect_smoke, 1, x, y, z, 0, 0, 0, 0, .7, .25, 0, .25f, -.001f, 0, 1.5, .02, "smoke", source_type, source);
        particle_engine.create(effect_smoke, 1, x, y, z, 0, 0, 0, 0, .7, .25, 0, .25f, -.001f, 0, 1.5, 1.5, .02, "smoke", source_type, source);

        //if (rnd.Next (0, 3) == 0) particle_effect[free_particle ()].create (effect_flame_red, 1, x + rnd.Next (-8, 8), y + rnd.Next (-8, 8), z + rnd.Next (-8, 8), 0, 360, .4, .1, .7, .2, 0, .6f, -.015f, 0, 2.5, .01, "fire", source_type, source);// break;
        //if (rnd.Next (0, 2) == 0) particle_effect[free_particle ()].create (effect_flame_orange, 1, x + rnd.Next (-8, 8), y + rnd.Next (-8, 8), z + rnd.Next (-8, 8), 0, 360, .4, .1, .7, .2, 0, .6f, -.015f, 0, 2.25, .01, "fire", source_type, source);// break;
        //if (rnd.Next (0, 2) == 0) particle_effect[free_particle ()].create (effect_flame_yellow, 1, x + rnd.Next (-8, 8), y + rnd.Next (-8, 8), z + rnd.Next (-8, 8), 0, 360, .4, .1, .7, .2, 0, .6f, -.015f, 0, 2, .01, "fire", source_type, source);// break;
        //if (rnd.Next (0, 3) == 0) particle_effect[free_particle ()].create (effect_flame_white, 1, x + rnd.Next (-8, 8), y + rnd.Next (-8, 8), z + rnd.Next (-8, 8), 0, 360, .4, .1, .7, .2, 0, .6f, -.015f, 0, 1.75, .01, "fire", source_type, source);// break;
        if (rnd.Next(0, 3) == 0) particle_engine.create(effect_flame_red, 1, x + rnd.Next(-8, 8), y + rnd.Next(-8, 8), z + rnd.Next(-8, 8), 0, 360, .4, .1, .7, .2, 0, .6f, -.015f, 0, 2.5, 2.5, .01, "fire", source_type, source);// break;
        if (rnd.Next(0, 2) == 0) particle_engine.create(effect_flame_orange, 1, x + rnd.Next(-8, 8), y + rnd.Next(-8, 8), z + rnd.Next(-8, 8), 0, 360, .4, .1, .7, .2, 0, .6f, -.015f, 0, 2.25, 2.5, .01, "fire", source_type, source);// break;
        if (rnd.Next(0, 2) == 0) particle_engine.create(effect_flame_yellow, 1, x + rnd.Next(-8, 8), y + rnd.Next(-8, 8), z + rnd.Next(-8, 8), 0, 360, .4, .1, .7, .2, 0, .6f, -.015f, 0, 2, 2, .01, "fire", source_type, source);// break;
        if (rnd.Next(0, 3) == 0) particle_engine.create(effect_flame_white, 1, x + rnd.Next(-8, 8), y + rnd.Next(-8, 8), z + rnd.Next(-8, 8), 0, 360, .4, .1, .7, .2, 0, .6f, -.015f, 0, 1.75, 1.75, .01, "fire", source_type, source);// break;
      }

      ////////////////////////////////////////////////////////////////////////////////

      void particle_flamethrower(int x, int y, int z, int direction, string source_type, int source)
      {
        double h_scale = 1.5;
        double v_scale = 2;

        //if (direction >= MathHelper.ToRadians (135) && direction < MathHelper.ToRadians (225))
        if (direction >= 135 && direction < 225)
        {
          h_scale = 3;
          v_scale = 1;
        }
        //else if (direction <= MathHelper.ToRadians (45) || direction > MathHelper.ToRadians (315))
        else if (direction <= 45 || direction > 315)
        {
          h_scale = 3;
          v_scale = 1;
        }

        //if (rnd.Next (0, 2) == 0) particle_effect[free_particle ()].create (effect_smoke, 1, x, y, z, direction, 3, 1, .5, .7, .25, 0, .25f, -.001f, 0, 1, .02, "smoke", source_type, source);
        //if (rnd.Next (0, 2) == 0) particle_effect[free_particle ()].create (effect_flame_red, 2, x, y, z, direction, 3, 2, .5, 0, 0, 0, .75f, -.005f, 0, 1, .01, "fire", source_type, source);
        //if (rnd.Next (0, 2) == 0) particle_effect[free_particle ()].create (effect_flame_orange, 3, x, y, z, direction, 3, 2, .5, 0, 0, 0, .75f, -.005f, 0, 1, .01, "fire", source_type, source);
        //if (rnd.Next (0, 2) == 0) particle_effect[free_particle ()].create (effect_flame_yellow, 4, x, y, z, direction, 3, 2, .5, 0, 0, 0, .75f, -.005f, 0, 1, .01, "fire", source_type, source);
        //if (rnd.Next (0, 2) == 0) particle_effect[free_particle ()].create (effect_flame_white, 5, x, y, z, direction, 3, 2, .5, 0, 0, 0, .75f, -.005f, 0, 1, .01, "fire", source_type, source);

        //if (rnd.Next (0, 2) == 0)
        //{
        int color = rnd.Next(0, 4);
        //if (rnd.Next (0, 5) == 0) particle_engine.create (effect_smoke, 1, x, y, z, direction, 3, 1, .5, .7, .25, 0, .25f, -.001f, 0, 1, .02, "smoke", source_type, source);
        if (color == 0) particle_engine.create(effect_flame_red, 1, x, y, z, direction, 2, 2, .5, 0, 0, 0, 1f, -.007f, 0, h_scale, v_scale, .01, "fire", source_type, source);
        if (color == 1) particle_engine.create(effect_flame_orange, 1, x, y, z, direction, 2, 2, .5, 0, 0, 0, 1f, -.007f, 0, h_scale, v_scale, .01, "fire", source_type, source);
        if (color == 2) particle_engine.create(effect_flame_yellow, 1, x, y, z, direction, 2, 2, .5, 0, 0, 0, 1f, -.007f, 0, h_scale, v_scale, .01, "fire", source_type, source);
        else particle_engine.create(effect_flame_white, 2, x, y, z, direction, 2, 2, .5, 0, 0, 0, 1f, -.007f, 0, h_scale, v_scale, .01, "fire", source_type, source);
        //}
      }

      ////////////////////////////////////////////////////////////////////////////////

      void particle_incinerator(int x, int y, int z, int direction, string source_type, int source)
      {
        double h_scale = 1.5;
        double v_scale = 2;

        if (direction >= 135 && direction < 225)
        {
          h_scale = 2.5;
          v_scale = 1;
        }
        else if (direction <= 45 || direction > 315)
        {
          h_scale = 2.5;
          v_scale = 1;
        }

        //if (rnd.Next (0, 4) == 0) particle_effect[free_particle ()].create (effect_smoke, 1, x, y, z, direction, 3, 1, .5, .7, .25, 0, .25f, -.001f, 0, 1, .02, "smoke", source_type, source);
        //if (rnd.Next (0, 5) == 0) particle_effect[free_particle ()].create (effect_flame_red, 2, x, y, z, direction, 3, 2, .5, 0, 0, 0, .75f, -.005f, 0, 1.5, .01, "fire", source_type, source);
        //if (rnd.Next (0, 5) == 0) particle_effect[free_particle ()].create (effect_flame_orange, 3, x, y, z, direction, 3, 2, .5, 0, 0, 0, .75f, -.005f, 0, 1.5, .01, "fire", source_type, source);
        //if (rnd.Next (0, 6) == 0) particle_effect[free_particle ()].create (effect_flame_yellow, 4, x, y, z, direction, 3, 2, .5, 0, 0, 0, .75f, -.005f, 0, 1.5, .01, "fire", source_type, source);
        //if (rnd.Next (0, 6) == 0) particle_effect[free_particle ()].create (effect_flame_white, 5, x, y, z, direction, 3, 2, .5, 0, 0, 0, .75f, -.005f, 0, 1.5, .01, "fire", source_type, source);

        //if (rnd.Next (0, 6) == 0) particle_engine.create (effect_smoke, 1, x, y, z, direction, 3, 1, .5, .7, .25, 0, .25f, -.001f, 0, 1, .02, "smoke", source_type, source);
        //if (rnd.Next (0, 6) == 0) particle_engine.create (effect_flame_red, 1, x, y, z, direction, 2, 2, .5, 0, 0, 0, .75f, -.005f, 0, h_scale, v_scale, .01, "fire", source_type, source);
        //if (rnd.Next (0, 6) == 0) particle_engine.create (effect_flame_orange, 1, x, y, z, direction, 2, 2, .5, 0, 0, 0, .75f, -.005f, 0, h_scale, v_scale, .01, "fire", source_type, source);
        //if (rnd.Next (0, 5) == 0) particle_engine.create (effect_flame_yellow, 1, x, y, z, direction, 2, 2, .5, 0, 0, 0, .75f, -.005f, 0, h_scale, v_scale, .01, "fire", source_type, source);
        //if (rnd.Next (0, 5) == 0) particle_engine.create (effect_flame_white, 1, x, y, z, direction, 2, 2, .5, 0, 0, 0, .75f, -.005f, 0, h_scale, v_scale, .01, "fire", source_type, source);

        if (rnd.Next(0, 2) == 0)
        {
          int color = rnd.Next(0, 4);
          //if (rnd.Next (0, 5) == 0) particle_engine.create (effect_smoke, 1, x, y, z, direction, 3, 1, .5, .7, .25, 0, .25f, -.001f, 0, 1, 1, .02, "smoke", source_type, source);
          if (color == 0) particle_engine.create(effect_flame_red, 1, x, y, z, direction, 3, 2, .5, 0, 0, 0, 1f, -.007f, 0, h_scale, v_scale, .01, "fire", source_type, source);
          if (color == 1) particle_engine.create(effect_flame_orange, 1, x, y, z, direction, 3, 2, .5, 0, 0, 0, 1f, -.007f, 0, h_scale, v_scale, .01, "fire", source_type, source);
          if (color == 2) particle_engine.create(effect_flame_yellow, 1, x, y, z, direction, 3, 2, .5, 0, 0, 0, 1f, -.007f, 0, h_scale, v_scale, .01, "fire", source_type, source);
          else particle_engine.create(effect_flame_white, 2, x, y, z, direction, 3, 2, .5, 0, 0, 0, 1f, -.007f, 0, h_scale, v_scale, .01, "fire", source_type, source);
        }
      }

      ////////////////////////////////////////////////////////////////////////////////

      void particle_freeze_ray(int x, int y, int z, int direction, string source_type, int source)
      {
        int new_x, new_y, new_z;
        int variance = 12;  // radius of beam

        //cold energy light
        //particle_effect[free_particle ()].create (effect_cold_energy, 2, x, y, z, direction, 2, 6, 0, 0, 0, -.02, .5f, -.005f, 0, 1, 0, "freeze", source_type, source);
        particle_engine.create(effect_cold_energy, 2, x, y, z, direction, 2, 6, 0, 0, 0, -.02, .5f, -.005f, 0, 1, 1, 0, "freeze", source_type, source);

        //slightly randomize snow placement
        new_x = rnd.Next(x - variance, x + variance);
        new_y = rnd.Next(y - variance, y + variance);
        new_z = rnd.Next(z - variance, z + variance);
        //particle_effect[free_particle ()].create (effect_snowflake, 2, new_x, new_y, new_z, direction, 2, 6, 2, 0, 0, -.02, 1f, -.01f, 0, 1, 0, "freeze", source_type, source);
        particle_engine.create(effect_snowflake, 2, new_x, new_y, new_z, direction, 2, 6, 2, 0, 0, -.02, 1f, -.01f, 0, 1, 1, 0, "freeze", source_type, source);

        //multiple flakes
        new_x = rnd.Next(x - variance, x + variance);
        new_y = rnd.Next(y - variance, y + variance);
        new_z = rnd.Next(z - variance, z + variance);
        //particle_effect[free_particle ()].create (effect_snowflake, 2, new_x, new_y, new_z, direction, 2, 6, 2, 0, 0, -.02, 1f, -.01f, 0, 1, 0, "freeze", source_type, source);
        particle_engine.create(effect_snowflake, 2, new_x, new_y, new_z, direction, 2, 6, 2, 0, 0, -.02, 1f, -.01f, 0, 1, 1, 0, "freeze", source_type, source);
      }

      ////////////////////////////////////////////////////////////////////////////////

      void particle_green_tripwire(int x, int y, int z, int direction)
      {
        //particle_effect[free_particle ()].create (pixel_green, 1, x, y, z, direction, 0, .5, 0, 0, 0, .01, .7f, -.01f, 0, 2, 0, "light", "none", -1);
        particle_engine.create(pixel_green, 1, x, y, z, direction, 0, .5, 0, 0, 0, .01, .7f, -.01f, 0, 2, 2, 0, "light", "none", -1);
      }

      ////////////////////////////////////////////////////////////////////////////////

    }
  }
