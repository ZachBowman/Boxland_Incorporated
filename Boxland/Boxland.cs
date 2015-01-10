//Boxland, inc.

//2011-2014
//Nightmare Games
//Cricketheads

#region Using Statements

using System;
using System.Collections.Generic;
//using System.Diagnostics;
using drawing = System.Drawing;
using System.IO;
//using System.Runtime.InteropServices;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;

using Forms = System.Windows.Forms;

#endregion

namespace Boxland
  {
  public class Boxland : Game
    {
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    RenderTarget2D light_buffer;

    public Random rnd = new Random ();

    // game settings
    bool debug = false;
    bool randomized_map = false;
    bool draw_walls = true;
    bool draw_boxes = true;
    bool observe_creation = false;
    bool toggle_enemies = true;
    bool toggle_texture_blending = false;
    int lighting_engine = 2;  // 0 = off, 1 = black lights, 2 = canopy darkness, 3 = net style
    int draw_order = 3;       // 1 = bottom to top first, 2 = back to front first, 3 = bottom to top in half layers
    bool dynamic_shadows = true;

    // game states
    const int TITLE = 0;
    const int GAME = 1;
    const int CREATION = 2;

    // general game variables
    int game_state = TITLE;   // game, creation
    bool game_menu = false;
    bool green_doors_open = false;  // are exits & doors open?
    bool green_switch_down = false;  // is the green switch being pushed down?
    bool red_doors_open = false;
    bool red_switch_down = false;
    bool yellow_doors_open = false;
    bool yellow_switch_down = false;
    bool blue_doors_open = false;
    bool blue_switch_down = false;

    // levels / areas
    int player_level = 6;
    int player_last_level = -1;   // map area the player was in just before this (-1 = new game)
    const int max_levels = 9;

    // creation mode
    bool creation_box = false;
    int creation_direction = -1;
    int creation_last_direction = -1;
    int creation_distance = 0;
    int creation_px, creation_py, creation_pz;
    int creation_moves = 0;
    const int creation_max_moves = 40;
    string creation_wall = "..";
    string creation_floor = "..";
    int creation_mode = 0;  // 1 = tunnelling, 2 = shuffling

    // screen
    ScreenInfo screen = new ScreenInfo ()
      {
      //width = 1950,             // xbox high (does not show correctly on pc)
      //height = 1080,
      //width = 1280,             // xbox normal
      //height = 720,
      //width = 1024,             // pc normal
      //height = 768,
      width = 1280,             // pc wide
      height = 768,
      //width = 1440,             // pc wide high res
      //height = 900,
      //width = 1024,             // pc small window
      //height = 600,
      //width = 800,              // pc low res
      //height = 600,
      scroll_x = 0,             // screen scroll offset
      scroll_y = 0,
      bg1_scroll_speed = .5,    // % of scroll offset used for background scrolling
      bg2_scroll_speed = .75,
      bg1_scroll_x = 0,
      bg1_scroll_y = 0,
      bg2_scroll_x = 0,
      bg2_scroll_y = 0,
      fullscreen = true
      };

    // map
    Map map = new Map ();
    public const int tilesize = 96;
    public const double parallax = .75;
    int map_tile_width;                       // total tile size of current map area
    int map_tile_length;
    int map_tile_height;
    int map_char_width;                       // width of map in string characters
    int map_width = 0;                        // total pixel size of current map area
    int map_length = 0;
    int map_height = 0;
    const int box_move = tilesize;
    Rectangle scroll_border;

    const int map_tile_max_width = 55;
    const int map_char_max_width = map_tile_max_width * 3;
    const int map_tile_max_length = 25;
    const int map_tile_max_height = 4;//3;

    const int map_max_width  = map_tile_max_width * tilesize;//2000;//1500;  // maximum allowable pixel size of map
    const int map_max_length = map_tile_max_length * tilesize;//2000;//1500;
    const int map_max_height = map_tile_max_height * tilesize;//400;

    //string[,] textmap = new string[map_tile_max_length, map_tile_max_height];
    List<List<string>> textmap = new List<List<string>>();
    string[,,]matrixmap = new string[map_tile_max_width, map_tile_max_length, map_tile_max_height];

    // locations
    const int max_character_spots = 50;
    int total_character_spots = 0;
    Location[] random_character_spot = new Location[max_character_spots];

    //physics
    const double gravity = 8;//2.5;
    const double gravity_acceleration = .2;
    const double conveyor_speed = .5;

    // time
    const double time_ratio = .002;  // time speed as compared to real time
    //double game_frame_counter = 0;   // assumes 30 fps
    int game_second_last;            // for comparison with game_time.TotalGameTime.Seconds
    int game_second = 0;
    int game_minute = 0;
    int game_hour = 10;
    //bool minute_change = false;
    int fps_counter;                 // adds one each time game is drawn between seconds
    int fps;                         // shows how many times fps_counter was added to the last second
    int cycle_counter;               // increases by one each frame
    int cycle_delay = 30;             // the number of frames in between certain scripts running, like ai

    // controls
    string player_control = "none";  // waits for input from keyboard or gamepad on title screen.

    MouseState mouse_current;
    MouseState mouse_last;
    bool mouse_left    = false;
    bool mouse_right   = false;

    KeyboardState keyboard;
    bool key_anydirection = false;  // true if any arrow key is being pressed
    bool key_up           = false;
    bool key_down         = false;
    bool key_left         = false;
    bool key_right        = false;
    bool key_space        = false;
    bool key_leftshift    = false;
    bool key_rightshift   = false;
    bool key_leftcontrol  = false;
    bool key_rightcontrol = false;
    bool key_leftalt      = false;
    //bool key_rightalt     = false;
    bool key_numpad0      = false;
    bool key_esc          = false;
    bool key_tab          = false;
    bool key_1            = false;
    bool key_2            = false;
    bool key_3            = false;
    bool key_a            = false;
    bool key_b            = false;
    bool key_c            = false;
    bool key_d            = false;
    bool key_e            = false;
    bool key_f            = false;
    bool key_q            = false;
    bool key_r            = false;
    bool key_s            = false;
    bool key_v            = false;
    bool key_w            = false;
    bool key_x            = false;
    bool key_z            = false;
    bool key_plus         = false;
    bool key_minus        = false;
    bool key_enter        = false;
    bool key_backspace    = false;

    GamePadState controller;
    bool controller_a = false;
    bool controller_b = false;
    bool controller_x = false;
    bool controller_y = false;
    bool controller_left_stick = false;

    // menu
    int menu_x, menu_y;
    int menu_width, menu_height;
    string menu_screen = "none";
    Texture2D title_screen_test;
    Texture2D menu_exit_test;
    Vector2 menu_exit_v;

    // graphics
    Shapes shape = new Shapes ();
    Texture2D pixel_yellow;

    // particles
    public const int max_effects = 400;//200;
    public Particle_Effect[] particle_effect = new Particle_Effect[max_effects];

    //public enum thing_type
    //  {
    //  character,
    //  brush_control.brush,
    //  obj,
    //  fixture
    //  }

    // LIGHT SPRITES
    public enum L
      {
      blue,
      blue_pale,
      dark,
      fushia,
      green,
      red,
      white,
      yellow,
      yellow_pale
      }

    const int max_light_sprites = 9;
    Texture2D[] light_sprite = new Texture2D[max_light_sprites];
    float ambient_dark = 0f;  // 0 = normal, 1 = pitch black
    Color ambient_light = new Color (96, 96, 96);  // used only for light engine 2
    const int max_lights = 150;
    int total_lights = 0;
    Light[] light = new Light[max_lights];

    // LIGHT TYPES
    const int SOLID      = 0;
    const int PULSING    = 1;
    const int FLICKERING = 2;

    // effects
    Texture2D solid_black;
    Texture2D wall_shadow_center, wall_shadow_west, wall_shadow_south_west, wall_shadow_south, wall_shadow_south_east, wall_shadow_east;
    Texture2D shading_wall, shading_door_test_closed, shading_door_test_open;//, shading_exit_test_closed, shading_exit_test_open;
    Texture2D shading_gateway_test, shading_box_test_ice;
    Texture2D pow_sprite;
    Texture2D test_background1, test_background2, test_background3, test_background4, test_background5;
    Texture2D effect_snowflake, effect_cold_energy, effect_dollars;
    Texture2D effect_flame_white, effect_flame_yellow, effect_flame_orange, effect_flame_red, effect_smoke, effect_sparkle_white;
    Texture2D color_flash_sprite, solid_white, solid_red, effect_pain;
    Texture2D pixel_green;
    Texture2D wires_southeast_powered_test;
    Texture2D shadow_character_generic;
    public Fade fader       = new Fade ();
    public Fade color_flash = new Fade ();

    // stickers
    const int total_office_stickers = 33;
    const int total_office_floor_stickers = 1;
    const int total_factory_stickers = 11;
    const int total_factory_floor_stickers = 2;
    Texture2D[] sticker_office = new Texture2D[total_office_stickers];
    Texture2D[] sticker_office_floor = new Texture2D[total_office_floor_stickers];
    Texture2D[] sticker_factory = new Texture2D[total_factory_stickers];
    Texture2D[] sticker_factory_floor = new Texture2D[total_factory_floor_stickers];

    // BRUSHES
    Brush_Control brush_control = new Brush_Control (tilesize);

    // FIXTURES (furniture & machines)
    Fixture_Control fixture_control = new Fixture_Control (tilesize);

    // OBJECTS
    Object_Control object_control = new Object_Control ();
    int rock_color = 0;

    // CHARACTERS
    Character_Control character_control = new Character_Control (tilesize);
    Texture2D[] character_shadow = new Texture2D[Character_Control.max_character_list];
    Texture2D arrow_sprite, target_sprite;
    const int max_characters = 30;
    //public List<Character> character = new List<Character> ();
    public const int PLAYER = 0;
    bool player_created = false;       // becomes true after first time player is added to the map (used to retain stats between levels)
    bool player_added_to_map = false;  // becomes true each time player is added to map (in case of missing player position)
    int punch_rest_delay = 5;          // number of animation frames until character's fist retracts (for all combat moves)
    int random_retard1 = 0;            // number of randomly-placed standard retards to genereate, set on map load
    int random_retard2 = 0;            // number of randomly-placed rock-throwing retards to genereate, set on map load

    // draw list
    const int max_draw_slots = 700;
    int total_draw_slots = 0;
    Draw_Slot[] draw_list = new Draw_Slot[max_draw_slots];

    // sound
    //bool sound_richard_knockout_test = false;
    //bool sound_retard_knockout_test = false;

    SoundEffect sound_door;
    SoundEffect sound_fire;
    SoundEffect sound_kick;
    SoundEffect sound_pickup_coin;
    SoundEffect sound_pickup_glove;
    SoundEffect sound_pickup_key;
    SoundEffect sound_punch;
    SoundEffect sound_retard_death;
    SoundEffect sound_richard_buttsmash;
    SoundEffect sound_richard_death;
    SoundEffect sound_richard_hurt;
    SoundEffect sound_richard_jump;

    // debug
    SpriteFont debug_font;
    Vector2    debug_pos;
    //bool       test_sprite = false;
    //int        debug_int;
    int grab_x_distance = 0;
    int grab_y_distance = 0;
    //int Q = 0;  // test variable

    public Boxland () : base ()
      {
      graphics = new GraphicsDeviceManager (this);
      Content.RootDirectory = "Content";
      }

    protected override void Initialize ()
      {
      base.Initialize ();
      this.IsMouseVisible = true;

      if (screen.fullscreen)
        {
        screen.width = Forms.Screen.PrimaryScreen.Bounds.Width;
        screen.height = Forms.Screen.PrimaryScreen.Bounds.Height;
        graphics.IsFullScreen = true;
        Window.IsBorderless = true;
        }
      graphics.PreferredBackBufferWidth = screen.width;
      graphics.PreferredBackBufferHeight = screen.height;
      graphics.ApplyChanges ();
      Window.Position = new Point (0, 0);

      light_buffer = new RenderTarget2D (GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

      // initialize class arrays
      for (int i = 0; i < max_effects; i += 1) particle_effect[i] = new Particle_Effect ();

      //player_level = 6;  // starting level, default 0

      load_map ();

      if (player_added_to_map == false)
        {
        add_character ("Richard", (int) Character_Control.C.RICHARD, 64, 64, 256);
        }

      // scroll border is inside box of screen (non-scrolling part)
      scroll_border.X = Convert.ToInt32 (screen.width * 2 / 5);
      scroll_border.Y = Convert.ToInt32 (screen.height * 2 / 5);
      scroll_border.Width = Convert.ToInt32 (screen.width * 1 / 5);
      scroll_border.Height = Convert.ToInt32 (screen.height * 1 / 5);

      // menu
      menu_width = Convert.ToInt32 (screen.width * .75);
      menu_height = Convert.ToInt32 (screen.height * .75);
      menu_x = (screen.width - menu_width) / 2;
      menu_y = (screen.height - menu_height) / 2;
      menu_exit_v.X = menu_x + 20;
      menu_exit_v.Y = menu_y + 20;
      }

    ////////////////////////////////////////////////////////////////////////////////

    private Texture2D LoadTextureStream (string loc)
      {
      Texture2D file = null;
      RenderTarget2D result = null;

      using (Stream titleStream = TitleContainer.OpenStream ("Content\\" + loc + ".png"))
        {
        file = Texture2D.FromStream (GraphicsDevice, titleStream);
        }

      //Setup a render target to hold our final brush_control.texture which will have premulitplied alpha values
      result = new RenderTarget2D (GraphicsDevice, file.Width, file.Height);

      GraphicsDevice.SetRenderTarget (result);

      //Multiply each color by the source alpha, and write in just the color values into the final brush_control.texture
      BlendState blendColor = new BlendState ();
      blendColor.ColorWriteChannels = ColorWriteChannels.Red | ColorWriteChannels.Green | ColorWriteChannels.Blue;

      blendColor.AlphaDestinationBlend = Blend.Zero;
      blendColor.ColorDestinationBlend = Blend.Zero;

      blendColor.AlphaSourceBlend = Blend.SourceAlpha;
      blendColor.ColorSourceBlend = Blend.SourceAlpha;

      SpriteBatch spriteBatch = new SpriteBatch (GraphicsDevice);
      spriteBatch.Begin (SpriteSortMode.Immediate, blendColor);
      spriteBatch.Draw (file, file.Bounds, Color.White);
      spriteBatch.End ();

      //Now copy over the alpha values from the PNG source brush_control.texture to the final one, without multiplying them
      BlendState blendAlpha = new BlendState ();
      blendAlpha.ColorWriteChannels = ColorWriteChannels.Alpha;

      blendAlpha.AlphaDestinationBlend = Blend.Zero;
      blendAlpha.ColorDestinationBlend = Blend.Zero;

      blendAlpha.AlphaSourceBlend = Blend.One;
      blendAlpha.ColorSourceBlend = Blend.One;

      spriteBatch.Begin (SpriteSortMode.Immediate, blendAlpha);
      spriteBatch.Draw (file, file.Bounds, Color.White);
      spriteBatch.End ();

      //Release the GPU back to drawing to the screen
      GraphicsDevice.SetRenderTarget (null);

      return result as Texture2D;
      }

    ////////////////////////////////////////////////////////////////////////////////

    // LoadContent will be called once per game and is the place to load all of your content.
    protected override void LoadContent ()
      {
      // Create a new SpriteBatch, which can be used to draw textures.
      spriteBatch = new SpriteBatch (GraphicsDevice);

      load_graphics ();

      debug_font = Content.Load<SpriteFont> ("Fonts\\Kooten");

      //audioEngine = new AudioEngine ("Content/boxland_sound.xgs");
      //waveBank = new WaveBank (audioEngine, "Content/Wave Bank.xwb");
      //soundBank = new SoundBank (audioEngine, "Content/Sound Bank.xsb");

      sound_door              = Content.Load<SoundEffect> ("Sounds\\door_test");
      sound_fire              = Content.Load<SoundEffect> ("Sounds\\fire_test");
      sound_kick              = Content.Load<SoundEffect> ("Sounds\\kick_test");
      sound_pickup_coin       = Content.Load<SoundEffect> ("Sounds\\pickup_coin_test");
      sound_pickup_glove      = Content.Load<SoundEffect> ("Sounds\\pickup_glove_test");
      sound_pickup_key        = Content.Load<SoundEffect> ("Sounds\\pickup_key_test");
      sound_punch             = Content.Load<SoundEffect> ("Sounds\\punch_test");
      sound_retard_death      = Content.Load<SoundEffect> ("Sounds\\retard_death_test");
      sound_richard_buttsmash = Content.Load<SoundEffect> ("Sounds\\richard_buttsmash_test");
      sound_richard_hurt      = Content.Load<SoundEffect> ("Sounds\\richard_hurt_test");
      sound_richard_jump      = Content.Load<SoundEffect> ("Sounds\\richard_jump_test");

      MediaPlayer.Volume = 1.0f;
      }

    ////////////////////////////////////////////////////////////////////////////////

    void load_graphics ()
      {
      solid_black = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\solid_black.png", FileMode.Open, FileAccess.Read));
      pixel_yellow = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\pixel_yellow.png", FileMode.Open, FileAccess.Read));
      //light_effect_darkness = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\light_effect_darkness.png", FileMode.Open, FileAccess.Read));

      // background
      title_screen_test = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\test_background1a.png", FileMode.Open, FileAccess.Read));

      test_background1 = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\test_background1a.png", FileMode.Open, FileAccess.Read));
      test_background2 = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\test_background2a.png", FileMode.Open, FileAccess.Read));
      test_background3 = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\test_background3.png", FileMode.Open, FileAccess.Read));
      test_background4 = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\test_background4a.png", FileMode.Open, FileAccess.Read));
      test_background5 = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\test_background5.png", FileMode.Open, FileAccess.Read));
      map.background = test_background4;

      // effects
      wall_shadow_center = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\shadow_center.png", FileMode.Open, FileAccess.Read));
      wall_shadow_west = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\shadow_left3.png", FileMode.Open, FileAccess.Read));
      wall_shadow_south_west = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\shadow_lower_left3.png", FileMode.Open, FileAccess.Read));
      wall_shadow_south = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\shadow_lower3.png", FileMode.Open, FileAccess.Read));
      wall_shadow_south_east = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\shadow_lower_right3.png", FileMode.Open, FileAccess.Read));
      wall_shadow_east = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\shadow_right3.png", FileMode.Open, FileAccess.Read));

      shading_wall = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\shading_wall.png", FileMode.Open, FileAccess.Read));
      ConvertToPremultipliedAlpha (shading_wall, new Color (255, 0, 255, 255));

      shading_door_test_closed = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\shading_door_test_closed.png", FileMode.Open, FileAccess.Read));
      ConvertToPremultipliedAlpha (shading_door_test_closed, new Color (255, 0, 255, 255));

      shading_door_test_open = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\shading_door_test_open.png", FileMode.Open, FileAccess.Read));
      ConvertToPremultipliedAlpha (shading_door_test_open, new Color (255, 0, 255, 255));

      shading_gateway_test = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\shading_gateway_test.png", FileMode.Open, FileAccess.Read));
      ConvertToPremultipliedAlpha (shading_gateway_test, new Color (255, 0, 255, 255));

      shading_box_test_ice = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\shading_box_test_ice3.png", FileMode.Open, FileAccess.Read));
      ConvertToPremultipliedAlpha (shading_box_test_ice, new Color (255, 0, 255, 255));

      light_sprite[(int) L.blue] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\light_blue.png", FileMode.Open, FileAccess.Read));
      light_sprite[(int) L.blue_pale] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\light_blue_pale.png", FileMode.Open, FileAccess.Read));
      light_sprite[(int) L.dark] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\light_dark.png", FileMode.Open, FileAccess.Read));
      light_sprite[(int) L.fushia] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\light_fushia.png", FileMode.Open, FileAccess.Read));
      light_sprite[(int) L.green] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\light_green.png", FileMode.Open, FileAccess.Read));
      light_sprite[(int) L.red] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\light_red.png", FileMode.Open, FileAccess.Read));
      light_sprite[(int) L.white] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\light_white.png", FileMode.Open, FileAccess.Read));
      light_sprite[(int) L.yellow] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\light_yellow.png", FileMode.Open, FileAccess.Read));
      light_sprite[(int) L.yellow_pale] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\light_yellow_pale.png", FileMode.Open, FileAccess.Read));
      effect_snowflake = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\snowflake.png", FileMode.Open, FileAccess.Read));
      effect_cold_energy = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\cold_energy.png", FileMode.Open, FileAccess.Read));
      effect_flame_white = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\flame_white.png", FileMode.Open, FileAccess.Read));
      effect_flame_yellow = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\flame_yellow.png", FileMode.Open, FileAccess.Read));
      effect_flame_orange = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\flame_orange.png", FileMode.Open, FileAccess.Read));
      effect_flame_red = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\flame_red.png", FileMode.Open, FileAccess.Read));
      effect_smoke = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\smoke.png", FileMode.Open, FileAccess.Read));
      effect_dollars = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\dollars.png", FileMode.Open, FileAccess.Read));
      effect_sparkle_white = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\sparkle_white.png", FileMode.Open, FileAccess.Read));
      solid_white = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\solid_white.png", FileMode.Open, FileAccess.Read));
      solid_red = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\solid_red.png", FileMode.Open, FileAccess.Read));
      effect_pain = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\pain.png", FileMode.Open, FileAccess.Read));
      pixel_green = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\pixel_green.png", FileMode.Open, FileAccess.Read));

      wires_southeast_powered_test = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\wires_southeast_powered_test2.png", FileMode.Open, FileAccess.Read));
      ConvertToPremultipliedAlpha (wires_southeast_powered_test, new Color (255, 0, 255, 255));

      shadow_character_generic = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "effects\\shadow_character_generic.png", FileMode.Open, FileAccess.Read));

      color_flash_sprite = solid_white;

      // textures
      brush_control.load_textures (GraphicsDevice);

      // stickers
      sticker_office[0] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office0_test2.png", FileMode.Open, FileAccess.Read));
      sticker_office[1] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office1_test.png", FileMode.Open, FileAccess.Read));
      sticker_office[2] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office2_test.png", FileMode.Open, FileAccess.Read));
      sticker_office[3] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office3_test.png", FileMode.Open, FileAccess.Read));
      sticker_office[4] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office4_test.png", FileMode.Open, FileAccess.Read));
      sticker_office[5] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office5_test.png", FileMode.Open, FileAccess.Read));
      sticker_office[6] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office6_test.png", FileMode.Open, FileAccess.Read));
      sticker_office[7] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office7_test.png", FileMode.Open, FileAccess.Read));
      sticker_office[8] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office8_test.png", FileMode.Open, FileAccess.Read));
      sticker_office[9] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office9_test.png", FileMode.Open, FileAccess.Read));
      sticker_office[10] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office10_test.png", FileMode.Open, FileAccess.Read));
      sticker_office[11] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office11_test.png", FileMode.Open, FileAccess.Read));
      sticker_office[12] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office12_test.png", FileMode.Open, FileAccess.Read));
      sticker_office[13] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office13_test.png", FileMode.Open, FileAccess.Read));
      sticker_office[14] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office14_test.png", FileMode.Open, FileAccess.Read));
      sticker_office[15] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office15_test.png", FileMode.Open, FileAccess.Read));
      sticker_office[16] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office16_test.png", FileMode.Open, FileAccess.Read));
      sticker_office[17] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office17_test.png", FileMode.Open, FileAccess.Read));
      sticker_office[18] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office18_test.png", FileMode.Open, FileAccess.Read));
      sticker_office[19] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office19_test.png", FileMode.Open, FileAccess.Read));
      sticker_office[20] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office20_2_test.png", FileMode.Open, FileAccess.Read));
      sticker_office[21] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office21_test.png", FileMode.Open, FileAccess.Read));
      sticker_office[22] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office22_double_test.png", FileMode.Open, FileAccess.Read));
      sticker_office[23] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office23_test.png", FileMode.Open, FileAccess.Read));
      sticker_office[24] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office24_test.png", FileMode.Open, FileAccess.Read));
      sticker_office[25] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office25_double_test.png", FileMode.Open, FileAccess.Read));
      sticker_office[26] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office26_test.png", FileMode.Open, FileAccess.Read));
      sticker_office[27] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office27_test.png", FileMode.Open, FileAccess.Read));
      sticker_office[28] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office28_test.png", FileMode.Open, FileAccess.Read));
      sticker_office[29] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office29_test.png", FileMode.Open, FileAccess.Read));
      sticker_office[30] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office30_test.png", FileMode.Open, FileAccess.Read));
      sticker_office[31] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office31_test.png", FileMode.Open, FileAccess.Read));
      sticker_office[32] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office32_test.png", FileMode.Open, FileAccess.Read));
      for (int q = 0; q < total_office_stickers; q += 1) ConvertToPremultipliedAlpha (sticker_office[q], new Color (255, 0, 255, 255));

      sticker_office_floor[0] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_office_floor0_test.png", FileMode.Open, FileAccess.Read));
      for (int q = 0; q < total_office_floor_stickers; q += 1) ConvertToPremultipliedAlpha (sticker_office_floor[q], new Color (255, 0, 255, 255));

      sticker_factory[0] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_factory0_test.png", FileMode.Open, FileAccess.Read));
      sticker_factory[1] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_factory1_test.png", FileMode.Open, FileAccess.Read));
      sticker_factory[2] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_factory2_test.png", FileMode.Open, FileAccess.Read));
      sticker_factory[3] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_factory3_test.png", FileMode.Open, FileAccess.Read));
      sticker_factory[4] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_factory4_test.png", FileMode.Open, FileAccess.Read));
      sticker_factory[5] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_factory5_test.png", FileMode.Open, FileAccess.Read));
      sticker_factory[6] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_factory6_test.png", FileMode.Open, FileAccess.Read));
      sticker_factory[7] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_factory7_test.png", FileMode.Open, FileAccess.Read));
      sticker_factory[8] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_factory8_test.png", FileMode.Open, FileAccess.Read));
      sticker_factory[9] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_factory9_test.png", FileMode.Open, FileAccess.Read));
      sticker_factory[10] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_factory10_test.png", FileMode.Open, FileAccess.Read));
      for (int q = 0; q < 11; q += 1) ConvertToPremultipliedAlpha (sticker_factory[q], new Color (255, 0, 255, 255));

      sticker_factory_floor[0] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_factory_floor0a_test.png", FileMode.Open, FileAccess.Read));
      sticker_factory_floor[1] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "textures\\sticker_factory_floor1_test.png", FileMode.Open, FileAccess.Read));
      //for (int q = 0; q < 1; q += 1) ConvertToPremultipliedAlpha (sticker_factory[q], new Color (255, 0, 255, 255));

      // fixtures
      fixture_control.load_sprites (GraphicsDevice);

      // objects
      object_control.object_sprite[(int) Object_Control.O.SHIRT_YELLOW, 0] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\shirt_power0_ink.png", FileMode.Open, FileAccess.Read));
      ConvertToPremultipliedAlpha (object_control.object_sprite[(int) Object_Control.O.SHIRT_YELLOW, 0], new Color (255, 0, 255, 255));

      object_control.object_sprite[(int) Object_Control.O.SHIRT_YELLOW, 1] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\shirt_power1_ink.png", FileMode.Open, FileAccess.Read));
      ConvertToPremultipliedAlpha (object_control.object_sprite[(int) Object_Control.O.SHIRT_YELLOW, 1], new Color (255, 0, 255, 255));

      object_control.object_sprite[(int) Object_Control.O.SHIRT_RED, 0] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\shirt_fire0_ink.png", FileMode.Open, FileAccess.Read));
      ConvertToPremultipliedAlpha (object_control.object_sprite[(int) Object_Control.O.SHIRT_RED, 0], new Color (255, 0, 255, 255));

      object_control.object_sprite[(int) Object_Control.O.SHIRT_WHITE, 0] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\shirt_ice0_ink.png", FileMode.Open, FileAccess.Read));
      ConvertToPremultipliedAlpha (object_control.object_sprite[(int) Object_Control.O.SHIRT_WHITE, 0], new Color (255, 0, 255, 255));

      object_control.object_sprite[(int) Object_Control.O.SHIRT_WHITE, 1] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\shirt_ice1_ink.png", FileMode.Open, FileAccess.Read));
      ConvertToPremultipliedAlpha (object_control.object_sprite[(int) Object_Control.O.SHIRT_WHITE, 1], new Color (255, 0, 255, 255));

      object_control.object_sprite[(int) Object_Control.O.SHIRT_PURPLE, 0] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\shirt_magnet0_ink.png", FileMode.Open, FileAccess.Read));
      ConvertToPremultipliedAlpha (object_control.object_sprite[(int) Object_Control.O.SHIRT_PURPLE, 0], new Color (0, 255, 0, 255));

      object_control.object_sprite[(int) Object_Control.O.SHIRT_PURPLE, 1] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\shirt_magnet1_ink.png", FileMode.Open, FileAccess.Read));
      ConvertToPremultipliedAlpha (object_control.object_sprite[(int) Object_Control.O.SHIRT_PURPLE, 1], new Color (0, 255, 0, 255));

      object_control.object_sprite[(int) Object_Control.O.SHIRT_BLUE, 0] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\shirt_electric.png", FileMode.Open, FileAccess.Read));
      ConvertToPremultipliedAlpha (object_control.object_sprite[(int) Object_Control.O.SHIRT_BLUE, 0], new Color (255, 0, 255, 255));

      object_control.object_sprite[(int) Object_Control.O.ROCK, 0] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\food_hotdog_ink.png", FileMode.Open, FileAccess.Read));
      ConvertToPremultipliedAlpha (object_control.object_sprite[(int) Object_Control.O.ROCK, 0], new Color (255, 0, 255, 255));

      object_control.object_sprite[(int) Object_Control.O.ROCK_BROWN, 0] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\food_hamburger_ink.png", FileMode.Open, FileAccess.Read));
      ConvertToPremultipliedAlpha (object_control.object_sprite[(int) Object_Control.O.ROCK_BROWN, 0], new Color (255, 0, 255, 255));

      object_control.object_sprite[(int) Object_Control.O.ROCK_RED, 0] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\food_pizza_ink.png", FileMode.Open, FileAccess.Read));
      ConvertToPremultipliedAlpha (object_control.object_sprite[(int) Object_Control.O.ROCK_RED, 0], new Color (255, 0, 255, 255));

      object_control.object_sprite[(int) Object_Control.O.ROCK_WHITE, 0] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\rock_white_test.png", FileMode.Open, FileAccess.Read));
      ConvertToPremultipliedAlpha (object_control.object_sprite[(int) Object_Control.O.ROCK_WHITE, 0], new Color (255, 0, 255, 255));

      object_control.object_sprite[(int) Object_Control.O.KEYCARD, 0] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\key1_ink.png", FileMode.Open, FileAccess.Read));
      ConvertToPremultipliedAlpha (object_control.object_sprite[(int) Object_Control.O.KEYCARD, 0], new Color (255, 0, 255, 255));

      object_control.object_sprite[(int) Object_Control.O.HEALTH, 0] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\health0_ink.png", FileMode.Open, FileAccess.Read));
      ConvertToPremultipliedAlpha (object_control.object_sprite[(int) Object_Control.O.HEALTH, 0], new Color (255, 0, 255, 255));

      object_control.object_sprite[(int) Object_Control.O.HEALTH, 1] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\health1_ink.png", FileMode.Open, FileAccess.Read));
      ConvertToPremultipliedAlpha (object_control.object_sprite[(int) Object_Control.O.HEALTH, 1], new Color (255, 0, 255, 255));

      object_control.object_sprite[(int) Object_Control.O.HEALTH, 2] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\health2_ink.png", FileMode.Open, FileAccess.Read));
      ConvertToPremultipliedAlpha (object_control.object_sprite[(int) Object_Control.O.HEALTH, 2], new Color (255, 0, 255, 255));

      object_control.object_sprite[(int) Object_Control.O.HEALTH, 3] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\health3_ink.png", FileMode.Open, FileAccess.Read));
      ConvertToPremultipliedAlpha (object_control.object_sprite[(int) Object_Control.O.HEALTH, 3], new Color (255, 0, 255, 255));

      object_control.object_sprite[(int) Object_Control.O.COIN, 0] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\coin0_ink.png", FileMode.Open, FileAccess.Read));
      ConvertToPremultipliedAlpha (object_control.object_sprite[(int) Object_Control.O.COIN, 0], new Color (255, 0, 255, 255));

      object_control.object_sprite[(int) Object_Control.O.SCRAP_METAL, 0] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\scrap_metal_ink.png", FileMode.Open, FileAccess.Read));
      ConvertToPremultipliedAlpha (object_control.object_sprite[(int) Object_Control.O.SCRAP_METAL, 0], new Color (255, 0, 255, 255));

      object_control.object_sprite[(int) Object_Control.O.ENERGY, 0] = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "objects\\energy0_ink.png", FileMode.Open, FileAccess.Read));
      ConvertToPremultipliedAlpha (object_control.object_sprite[(int) Object_Control.O.ENERGY, 0], new Color (255, 0, 255, 255));

      // characters
      character_control.load_sprites (GraphicsDevice);

      pow_sprite = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "characters\\pow.png", FileMode.Open, FileAccess.Read));
      arrow_sprite = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "characters\\arrow.png", FileMode.Open, FileAccess.Read));
      target_sprite = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "characters\\target.png", FileMode.Open, FileAccess.Read));

      ConvertToPremultipliedAlpha (pow_sprite, new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (arrow_sprite, new Color (255, 0, 255, 255));
      ConvertToPremultipliedAlpha (target_sprite, new Color (255, 0, 255, 255));

      // menu
      title_screen_test = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "menu\\title_screen_test2.png", FileMode.Open, FileAccess.Read));
      menu_exit_test = Texture2D.FromStream (GraphicsDevice, new FileStream (Brush_Control.Texture_Path + "menu\\menu_exit.png", FileMode.Open, FileAccess.Read));
      ConvertToPremultipliedAlpha (menu_exit_test, new Color (0, 0, 0, 255));
      }

    ////////////////////////////////////////////////////////////////////////////////

    protected override void UnloadContent ()
      {
      }

    ////////////////////////////////////////////////////////////////////////////////

    protected override void Update (GameTime game_time)
      {
      cycle_counter += 1;
      if (cycle_counter >= cycle_delay) cycle_counter = 0;

      if (player_control == "none") Get_Input ();
      Keyboard_Input ();
//      if (player_control == "controller") Controller_Input ();
      Mouse_Input (game_time);  // used only for menus, etc

      Update_Time (game_time);
      if (game_state == GAME && game_menu == false) Update_Brushes ();
      if (game_state == GAME && game_menu == false) Update_Fixtures ();
      if (game_state == GAME && game_menu == false) Update_Objects ();
      if (game_state == GAME && game_menu == false) Update_Characters ();
      if (game_state == GAME && game_menu == false) Update_Lights ();
      if (game_state == GAME || game_state == CREATION) Update_Scrolling ();
      if (game_state == GAME && game_menu == false) Update_Particles ();
      if (game_state == GAME) Check_Doors ();
      if (game_state == CREATION && observe_creation == false) load_map ();
//      Sound_Manager ();

      base.Update (game_time);
//      //audioEngine.Update ();
      }

    ////////////////////////////////////////////////////////////////////////////////

    public void Update_Characters ()
      {
      int b, f, c;
      int b_clip;   // character clips world brush_control.brush
      int c_clip;   // character clips another character
      int b_clip2;  // box clips brush_control.brush
      int f_clip;   // character clips fixture

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
          //if (character_control.character[c].name == "Richard") sound_richard_knockout_test = true;
          //else if (character_control.character[c].name == "retard") sound_retard_knockout_test = true;
          //else if (character_control.character[c].name == "throwing retard") sound_retard_knockout_test = true;
          character_control.character[c].sound_knockout = false;
          }

        if (character_control.character[c].on_fire > 0)
          {
          if (rnd.Next (0, 4) == 0)
            {
            particle_flames (character_control.character[c].x + rnd.Next (-1 * character_control.character[c].width / 2, character_control.character[c].width / 2), character_control.character[c].y + rnd.Next (-1 * character_control.character[c].length / 2, character_control.character[c].length / 2), character_control.character[c].z + rnd.Next (0, character_control.character[c].height) + 32, "character", c);
            if (character_control.character[c].shirt != (int) Object_Control.O.SHIRT_RED && character_control.character[c].action != "knocked out") character_damage (c, 3, 0, 0, character_control.character[c].x, character_control.character[c].y, "fire", c);
            character_control.character[c].on_fire -= 1;
            }
          }

        // stop richard from pushing if his box hits a wall
        if (character_control.character[c].action == "pushing" && character_control.character[c].brush_grab > -1 && brush_control.brush[character_control.character[c].brush_grab].moving == false) character_control.character[c].stop_pushing ();
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

        // x movement

        // if pushing box
        if (character_control.character[c].action == "pushing")
          {
          // move toward box destination
          if (character_control.character[c].push_dir == "right")
            {
            character_control.character[c].self_x_velocity = character_control.character[c].speed * .45;

            // stop moving horizontally if destination hit
            if (character_control.character[c].x >= character_control.character[c].push_x)
              {
              character_control.character[c].stop_pushing ();
              }
            }
          else if (character_control.character[c].push_dir == "left")
            {
            character_control.character[c].self_x_velocity = character_control.character[c].speed * -.45;

            // stop moving horizontally if destination hit
            if (character_control.character[c].x <= character_control.character[c].push_x)
              {
              character_control.character[c].stop_pushing ();
              }
            }
          }

        else  // not holding box, normal movement
          {
          // find x speed based on speed and direction
          character_control.character[c].self_x_velocity = character_control.character[c].self_velocity * Math.Cos (character_control.character[c].dir);
          }
        // factor in external forces on player unless anchored by holding onto something
        //if (character_control.character[c].brush_grab == -1)
        character_control.character[c].net_x_velocity = character_control.character[c].self_x_velocity + character_control.character[c].ext_x_velocity;            // combine self-locomotion and external forces
        //else character_control.character[c].net_x_velocity = character_control.character[c].self_x_velocity;
        //}

        // slow external force speed to zero
        if (character_control.character[c].ext_x_velocity > 0 && character_control.character[c].ext_x_velocity < .1) character_control.character[c].ext_x_velocity = 0;
        else if (character_control.character[c].ext_x_velocity < 0 && character_control.character[c].ext_x_velocity > -.1) character_control.character[c].ext_x_velocity = 0;
        else if (character_control.character[c].ext_x_velocity > 0) character_control.character[c].ext_x_velocity -= .1;
        else if (character_control.character[c].ext_x_velocity < 0) character_control.character[c].ext_x_velocity += .1;

        // move character
        character_control.character[c].dx += character_control.character[c].net_x_velocity;
        character_control.character[c].x = Convert.ToInt32 (character_control.character[c].dx);

        // collision
        b_clip = character_in_brush (character_control.character[c]);
        c_clip = character_in_character (c);
        f_clip = character_in_fixture (character_control.character[c], true);
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
        //else if (c == PLAYER && b_clip > -1 && brush_control.brush[b_clip].top_texture_number == (int) Brush_Control.T.EXIT_RED_V_TOP_CLOSED_TEST && red_doors_open == true) skip_area ();
        //else if (c == PLAYER && b_clip > -1 && brush_control.brush[b_clip].top_texture_number == (int) Brush_Control.T.EXIT_RED_H_TOP_CLOSED_TEST && red_doors_open == true) skip_area ();
        //else if (c == PLAYER && b_clip > -1 && brush_control.brush[b_clip].top_texture_number == EXIT_TEST_YELLOW && yellow_doors_open == true) skip_area ();
        //else if (c == PLAYER && b_clip > -1 && brush_control.brush[b_clip].top_texture_number == EXIT_TEST_YELLOW_H && yellow_doors_open == true) skip_area ();

        // walking into jump kick
        if (c_clip > -1)
          {
          if (character_control.character[c].action == "jump kicking")
            {
            if (character_control.character[c].is_facing_character (character_control.character[c_clip])) character_damage (c_clip, 30, 7, 0, character_control.character[c].x, character_control.character[c].y, "impact", c);
            }
          if (character_control.character[c_clip].action == "jump kicking")
            {
            if (character_control.character[c].is_facing_character (character_control.character[c])) character_damage (c, 30, 7, 0, character_control.character[c_clip].x, character_control.character[c_clip].y, "impact", c);
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

        // if pushing box
        if (character_control.character[c].action == "pushing")
          {
          // move toward box destination
          if (character_control.character[PLAYER].push_dir == "up")//character_control.character[c].y < character_control.character[c].push_y)  // going up
            {
            character_control.character[c].self_y_velocity = character_control.character[c].speed * .45;

            // stop moving vertically if destination hit
            if (character_control.character[c].y >= character_control.character[c].push_y)
              {
              character_control.character[c].stop_pushing ();
              }
            }
          else if (character_control.character[PLAYER].push_dir == "down")//character_control.character[c].y > character_control.character[c].push_y)  // going down
            {
            character_control.character[c].self_y_velocity = character_control.character[c].speed * -.45;

            // stop moving vertically if destination hit
            if (character_control.character[c].y <= character_control.character[c].push_y)
              {
              character_control.character[c].stop_pushing ();
              }
            }
          }

        else // not holding box, normal movement
          {
          // find y speed based on speed and direction
          character_control.character[c].self_y_velocity = character_control.character[c].self_velocity * Math.Sin (character_control.character[c].dir);
          }
        // factor in external forces on player unless anchored by holding onto something
        character_control.character[c].net_y_velocity = character_control.character[c].self_y_velocity + character_control.character[c].ext_y_velocity;  // combine self-locomotion and external forces

        // slow external force speed to zero
        if (character_control.character[c].ext_y_velocity > 0 && character_control.character[c].ext_y_velocity < .1) character_control.character[c].ext_y_velocity = 0;  // slow external force speed to zero
        else if (character_control.character[c].ext_y_velocity < 0 && character_control.character[c].ext_y_velocity > -.1) character_control.character[c].ext_y_velocity = 0;
        else if (character_control.character[c].ext_y_velocity > 0) character_control.character[c].ext_y_velocity -= .1;
        else if (character_control.character[c].ext_y_velocity < 0) character_control.character[c].ext_y_velocity += .1;

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
        else if (c == PLAYER && b_clip > -1 && brush_control.brush[b_clip].top_texture_number == (int) Brush_Control.T.EXIT_RED_V_TOP_CLOSED_TEST && red_doors_open == true) skip_area ();
        else if (c == PLAYER && b_clip > -1 && brush_control.brush[b_clip].top_texture_number == (int) Brush_Control.T.EXIT_RED_H_TOP_CLOSED_TEST && red_doors_open == true) skip_area ();

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
        if (character_on_ground (c) && character_control.character[c].action != "grabbing" && character_control.character[c].action != "pushing")
          {
          f = fixture_control.character_on_fixture (character_control.character[c]);
          if (f > -1)
            {
            if (fixture_control.fixture[f].type == (int) Fixture_Control.F.CONVEYOR_NORTH_TEST && fixture_control.fixture[f].on == true)
              character_control.character[c].ext_y_velocity = conveyor_speed;
            else if (fixture_control.fixture[f].type == (int) Fixture_Control.F.CONVEYOR_SOUTH_TEST && fixture_control.fixture[f].on == true)
              character_control.character[c].ext_y_velocity = -conveyor_speed;
            else if (fixture_control.fixture[f].type == (int) Fixture_Control.F.CONVEYOR_WEST_TEST && fixture_control.fixture[f].on == true)
              character_control.character[c].ext_x_velocity = -conveyor_speed;
            else if (fixture_control.fixture[f].type == (int) Fixture_Control.F.CONVEYOR_EAST_TEST && fixture_control.fixture[f].on == true)
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
            if (character_control.character[c].target_type == "none" || character_control.character[c].target < 0)
              {
              character_control.character[c].target_type = "none";
              character_control.character[c].target = -1;
              character_control.character[c].subtarget_x = -1;
              character_control.character[c].subtarget_y = -1;
              }

            if (character_control.active (c))
              {
              // GOALS

              // if no there's no current goal
              if (character_control.character[c].goal == "none")
                {
                // if hostile, attack player if visible
                if (character_control.character[c].hostile == true && character_control.character[c].action != "knocked out" && character_control.active (PLAYER)
                    && character_control.sees_character (c, PLAYER, brush_control.brush))
                  character_control.character[c].attack_character (character_control.character[PLAYER], PLAYER);
                }

              // if attacking character
              else if (character_control.character[c].goal == "attack" && character_control.character[c].target_type == "character")
                {

                // if target's unconscious, do nothing
                if (!character_control.active (character_control.character[c].target)) character_control.character[c].goal = "none";
                else if (character_control.sees_character (c, character_control.character[c].target, brush_control.brush))
                  {
                  if (character_control.character[c].action == "none" || character_control.character[c].action == "walking")
                    {

                    // close enough to punch
                    if (character_control.reach_character (c, character_control.character[c].target, brush_control.brush))
                      {
                      if (character_control.character[c].action != "punching" && rnd.Next (0, 50) == 0) character_punch (c);
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
              if (character_control.character[c].action == "punching")
                {
                }
              else if (character_control.character[c].action == "walking")
                {
                if (character_control.character[c].target_type == "object")
                  {
                  if (character_near_object (c, character_control.character[c].target))
                    {
                    character_control.character[c].self_velocity = 0;
                    character_control.character[c].action = "none";
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
                    character_control.character[c].action = "none";
                    character_control.character[c].self_velocity = 0;
                    }
                  else
                    {
                    continue_to_target (c);
                    }
                  }
                }
              else if (character_control.character[c].action == "knocked out")
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
        //if (character_control.character[c].action == "walking" && character_control.character[c].self_velocity > 2.5) character_control.character[c].run ();

        // from jumping to standing
        if (character_control.character[c].action == "jumping" && character_on_ground (c) && character_control.character[c].self_z_velocity <= 0) character_control.character[c].stand ();

        // from jump kicking to standing
        if (character_control.character[c].action == "jump kicking" && character_on_ground (c) && character_control.character[c].self_z_velocity <= 0) character_control.character[c].stand ();

        // vary walk animation speed based on movement speed
        if (character_control.character[c].action == "walking" && character_control.character[c].self_velocity != 0)
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

          if (character_control.character[c].action == "none" || character_control.character[c].action == "standing")
            {
            character_control.character[c].anim_frame = 0;
            character_control.character[c].combo = -1;
            character_control.blink (c);
            }
          else if (character_control.character[c].action == "walking")
            {
            if (character_control.character[c].anim_frame >= 2 && character_control.character[c].anim_frame < 9) character_control.character[c].anim_frame += 1;
            else character_control.character[c].anim_frame = 2;
            character_control.blink (c);
            }
          else if (character_control.character[c].action == "running")
            {
            if (character_control.character[c].name == "Richard")
              {
              if (character_control.character[c].anim_frame >= 10 && character_control.character[c].anim_frame < 17) character_control.character[c].anim_frame += 1;
              else character_control.character[c].anim_frame = 10;
              }
            else character_control.character[c].anim_frame = 0;
            }
          else if (character_control.character[c].action == "skidding")
            {
            if (character_control.character[c].name == "Richard") character_control.character[c].anim_frame = 18;
            else character_control.character[c].anim_frame = 0;
            }
          else if (character_control.character[c].action == "jumping")
            {
            if (character_control.character[c].name == "Richard") character_control.character[c].anim_frame = 19;
            else if (character_control.character[c].name == "retard") character_control.character[c].anim_frame = 19;
            else if (character_control.character[c].name == "throwing retard") character_control.character[c].anim_frame = 19;
            else character_control.character[c].anim_frame = 0;
            }
          else if (character_control.character[c].action == "jump kicking")
            {
            if (character_control.character[c].name == "Richard") character_control.character[c].anim_frame = 20;
            else character_control.character[c].anim_frame = 0;
            }
          else if (character_control.character[c].action == "grabbing")
            {
            if (character_control.character[c].name == "Richard") character_control.character[c].anim_frame = 26;
            else character_control.character[c].anim_frame = 0;
            }
          else if (character_control.character[c].action == "pushing")
            {
            if (character_control.character[c].name == "Richard")
              {
              if (character_control.character[c].anim_frame >= 27 && character_control.character[c].anim_frame < 34) character_control.character[c].anim_frame += 1;
              else character_control.character[c].anim_frame = 27;
              }
            else character_control.character[c].anim_frame = 0;
            }
          else if (character_control.character[c].action == "punching")
            {
            if (character_control.character[c].self_velocity != 0) character_control.character[c].self_velocity = 0;  // stop walking after step forward

            if (character_control.character[c].name == "Richard")
              {
              if (character_control.character[c].anim_frame_sequence < punch_rest_delay)
                {
                if (character_control.character[c].combo == 1) character_control.character[c].anim_frame = 37;
                else if (character_control.character[c].combo == 2) character_control.character[c].anim_frame = 38;
                else if (character_control.character[c].combo == 3) character_control.character[c].anim_frame = 39;
                else character_control.character[c].anim_frame = 36;
                }
              else if (character_control.character[c].anim_frame_sequence >= punch_rest_delay) character_control.character[c].anim_frame = 35;
              }
            else if (character_control.character[c].name == "Richard's Dad") character_control.character[c].anim_frame = 10;
            else if (character_control.character[c].name == "retard")
              {
              if (character_control.character[c].anim_frame < 16 || character_control.character[c].anim_frame > 19) character_control.character[c].anim_frame = 16;
              else character_control.character[c].anim_frame += 1;
              //if (character_control.character[c].anim_frame_sequence < punch_rest_delay)
              //  {
              //  if (character_control.character[c].combo == 1) character_control.character[c].anim_frame = 17;
              //  else if (character_control.character[c].combo == 2) character_control.character[c].anim_frame = 18;
              //  else character_control.character[c].anim_frame = 16;
              //  }
              //else if (character_control.character[c].anim_frame_sequence >= punch_rest_delay) character_control.character[c].anim_frame = 35;
              }
            else if (character_control.character[c].name == "throwing retard")
              {
              if (character_control.character[c].anim_frame_sequence < punch_rest_delay)
                {
                if (character_control.character[c].combo == 1) character_control.character[c].anim_frame = 37;
                else if (character_control.character[c].combo == 2) character_control.character[c].anim_frame = 38;
                else if (character_control.character[c].combo == 3) character_control.character[c].anim_frame = 39;
                else character_control.character[c].anim_frame = 36;
                }
              else if (character_control.character[c].anim_frame_sequence >= punch_rest_delay) character_control.character[c].anim_frame = 35;
              }
            else character_control.character[c].anim_frame = 0;

            // move to next frame in sequence (unless waiting for player to release attack button)
            if (c != PLAYER || character_control.character[c].anim_frame_sequence >= punch_rest_delay) character_control.character[c].anim_frame_sequence += 1;
            if (character_control.character[c].anim_frame_sequence >= character_control.character[c].max_anim_frame_sequence) character_control.character[c].action = "none";
            }
          else if (character_control.character[c].action == "stunned")
            {
            if (character_control.character[c].name == "Richard") character_control.character[c].anim_frame = 21;
            else if (character_control.character[c].name == "retard") character_control.character[c].anim_frame = 11;
            else if (character_control.character[c].name == "throwing retard") character_control.character[c].anim_frame = 21;
            else character_control.character[c].anim_frame = 0;
            character_control.character[c].anim_frame_sequence += 1;
            if (character_control.character[c].anim_frame_sequence >= character_control.character[c].max_anim_frame_sequence) character_control.character[c].action = "none";
            }
          else if (character_control.character[c].action == "knocked out")
            {
            if (character_control.character[c].name == "Richard") character_control.character[c].anim_frame = 25;
            else if (character_control.character[c].name == "retard") character_control.character[c].anim_frame = 12;
            else if (character_control.character[c].name == "throwing retard") character_control.character[c].anim_frame = 25;
            else character_control.character[c].anim_frame = 0;
            }
          else if (character_control.character[c].action == "superpunch")
            {
            if (character_control.character[c].name == "Richard") character_control.character[c].anim_frame = 36;
            else character_control.character[c].anim_frame = 0;
            }
          else if (character_control.character[c].action == "flamethrower")
            {
            if (character_control.character[c].name == "Richard") character_control.character[c].anim_frame = 36;
            else character_control.character[c].anim_frame = 0;
            particle_flamethrower (character_control.character[PLAYER].x, character_control.character[PLAYER].y, character_control.character[PLAYER].z + Convert.ToInt32 (character_control.character[PLAYER].height * .75), Convert.ToInt32 (MathHelper.ToDegrees (Convert.ToSingle (character_control.character[PLAYER].dir))), "character", c);
            }
          else if (character_control.character[c].action == "freeze ray")
            {
            if (character_control.character[c].name == "Richard") character_control.character[c].anim_frame = 36;
            else character_control.character[c].anim_frame = 0;
            particle_freeze_ray (character_control.character[PLAYER].x, character_control.character[PLAYER].y, character_control.character[PLAYER].z + Convert.ToInt32 (character_control.character[PLAYER].height * .75), Convert.ToInt32 (MathHelper.ToDegrees (Convert.ToSingle (character_control.character[PLAYER].dir))), "character", c);
            }

          else character_control.character[c].anim_frame = 0;
          }
        }
      }

    //////////////////////////////////////////////////////////////////////////////////

    int add_object (int object_type, int x, int y, int z)
      {
    //  //int o = -1;
    //  //int o2;

    //  //if (total_objects < 0) total_objects = 0;

    //  //if (total_objects >= max_objects)
    //    //{
    //    //o2 = 0;
    //    //while (o2 < max_objects && obj[o2].essential == true) o2 += 1;
    //    //if (o2 < max_objects && obj[o2].essential == false) destroy_object (o2);
    //    //destroy_object (0);//total_objects - 1);
    //    //destroy_object (total_objects);
    //    //}

    //  //else
    //  //if (total_objects >= 0 && total_objects < max_objects)
    //  if (object_control.obj.Count < max_objects)
    //    {
    //    Object o = new Object ();
    //    //o = total_objects;
    //    o.type = object_type;
    //    o.dx = x;
    //    o.dy = y;
    //    o.dz = z;
    //    o.x = x;
    //    o.y = y;
    //    o.z = z;

    //    // set defaults
    //    o.width = object_sprite[o.type, 0].Width;
    //    o.length = object_sprite[o.type, 0].Width;
    //    o.height = object_sprite[o.type, 0].Height;
    //    o.dir = MathHelper.ToRadians (270);
    //    o.velocity = 0;
    //    o.source = -1;
    //    o.essential = false;
    //    o.destroyed = false;
    //    o.skin = 0;

    //    if (object_type == (int) Object_Control.O.HEALTH)
    //      {
    //      o.skin = rnd.Next (0, 4);
    //      }

    //    //total_objects += 1;
    //    object_control.obj.Add (o);
    //    total_draw_slots += 1;
    //    }

    //  //return o;
    //  return object_control.obj.Count - 1;

      if (object_control.obj.Count < Object_Control.max_objects)
        {
        object_control.add (object_type, x, y, z);
        total_draw_slots += 1;
        return object_control.obj.Count - 1;
        }
      else return -1;
      }

    ////////////////////////////////////////////////////////////////////////////////

    void Update_Objects ()
      {
      int o;
      int b_clip, c_clip;
      float degrees;
      float ricochet_angle;

      //for (o = 0; o < total_objects; o += 1)
      for (o = 0; o < object_control.obj.Count; o += 1)
        {
        if (object_control.obj[o].destroyed == false)
          {
          // movement
          object_control.obj[o].x_velocity = object_control.obj[o].velocity * Math.Cos (object_control.obj[o].dir);
          object_control.obj[o].y_velocity = object_control.obj[o].velocity * Math.Sin (object_control.obj[o].dir);

          // x movement
          object_control.obj[o].dx += object_control.obj[o].x_velocity;
          object_control.obj[o].x = Convert.ToInt32 (object_control.obj[o].dx);

          if (object_control.obj[o].destroyed == false) c_clip = object_in_character (o);
          else c_clip = -1;
          if (object_control.obj[o].destroyed == false) b_clip = object_in_brush (o);
          else b_clip = -1;

          if ((b_clip > -1 || c_clip > -1) && object_control.obj[o].velocity > 0)  // hit wall or character while in motion
            {
            object_control.obj[o].dx -= object_control.obj[o].x_velocity;
            object_control.obj[o].x = Convert.ToInt32 (object_control.obj[o].dx);
            if (object_control.obj[o].velocity > .1) object_control.obj[o].velocity = object_control.obj[o].velocity / 2;
            if (object_control.obj[o].velocity < 0) object_control.obj[o].velocity = 0;

            // ricochet
            degrees = MathHelper.ToDegrees (Convert.ToSingle (object_control.obj[o].dir));
            if (object_control.obj[o].x_velocity < 0)  // going west
              {
              ricochet_angle = 180 - degrees;
              degrees = 0 + ricochet_angle;
              if (degrees < 0) degrees += 360;
              if (degrees >= 360) degrees -= 360;
              }
            else if (object_control.obj[o].x_velocity >= 0)  // going east
              {
              ricochet_angle = 0 - MathHelper.ToDegrees (Convert.ToSingle (degrees));
              degrees = 180 + ricochet_angle;
              if (degrees < 0) degrees += 360;
              if (degrees >= 360) degrees -= 360;
              }
            object_control.obj[o].dir = MathHelper.ToRadians (degrees);
            }

          // y movement
          object_control.obj[o].dy += object_control.obj[o].y_velocity;
          object_control.obj[o].y = Convert.ToInt32 (object_control.obj[o].dy);

          if (object_control.obj[o].destroyed == false) c_clip = object_in_character (o);
          else c_clip = -1;
          if (object_control.obj[o].destroyed == false) b_clip = object_in_brush (o);
          else b_clip = -1;

          if ((b_clip > -1 || c_clip > -1) && object_control.obj[o].velocity > 0)  // hit wall or character while in motion
            {
            object_control.obj[o].dy -= object_control.obj[o].y_velocity;
            object_control.obj[o].y = Convert.ToInt32 (object_control.obj[o].dy);
            if (object_control.obj[o].velocity > .1) object_control.obj[o].velocity = object_control.obj[o].velocity / 2;// 1.5;// .5;
            if (object_control.obj[o].velocity < 0) object_control.obj[o].velocity = 0;

            // ricochet
            degrees = MathHelper.ToDegrees (Convert.ToSingle (object_control.obj[o].dir));
            if (object_control.obj[o].y_velocity > 0)  // going north
              {
              ricochet_angle = 90 - degrees;
              degrees = 270 + ricochet_angle;
              if (degrees < 0) degrees += 360;
              if (degrees >= 360) degrees -= 360;
              }
            else if (object_control.obj[o].y_velocity < 0)  // going south
              {
              ricochet_angle = 270 - degrees;
              degrees = 90 + ricochet_angle;
              if (degrees < 0) degrees += 360;
              if (degrees >= 360) degrees -= 360;
              }
            object_control.obj[o].dir = MathHelper.ToRadians (degrees);
            }

          // z movement
          if (object_control.obj[o].z_velocity > -1 * gravity) object_control.obj[o].z_velocity -= gravity_acceleration;
          object_control.obj[o].dz += object_control.obj[o].z_velocity;
          object_control.obj[o].z = Convert.ToInt32 (object_control.obj[o].dz);
          b_clip = object_in_brush (o);
          if (b_clip > -1 || object_control.obj[o].z < 0)  // on the ground
            {
            object_control.obj[o].dz -= object_control.obj[o].z_velocity;
            object_control.obj[o].z = Convert.ToInt32 (object_control.obj[o].dz);

            // slowdown from friction against floor
            if (object_control.obj[o].velocity > 0)
              {
              object_control.obj[o].velocity -= .4;
              if (object_control.obj[o].velocity < 0) object_control.obj[o].velocity = 0;
              }
            }

          // particles
          if (object_control.obj[o].type == (int) Object_Control.O.COIN)
            {
            if (rnd.Next (0, 60) == 0)
              {
              int x = rnd.Next (object_control.obj[o].x - (object_control.object_sprite[object_control.obj[o].type, 0].Width / 2), object_control.obj[o].x + (object_control.object_sprite[object_control.obj[o].type, 0].Width / 2));
              int z = rnd.Next (object_control.obj[o].z, object_control.obj[o].z + (object_control.object_sprite[object_control.obj[o].type, 0].Height * 2));
              particle_coinsparkle (x, object_control.obj[o].y, z);
              }
            }
          }
        }

      //remove_one_destroyed_object ();
      }

    ////////////////////////////////////////////////////////////////////////////////

    int character_in_fixture (Character c, bool solid_only)
      {
      int f = 0;
      int clip = -1;

      while (clip == -1 && f < fixture_control.fixture.Count)//total_fixtures)
              {
              if (c.x + (c.width / 2) >= fixture_control.fixture[f].x && c.x - (c.width / 2) <= fixture_control.fixture[f].x + fixture_control.fixture[f].width
                  && c.y + (c.length / 2) >= fixture_control.fixture[f].y && c.y - (c.length / 2) <= fixture_control.fixture[f].y + fixture_control.fixture[f].length
                  && c.z + c.height >= fixture_control.fixture[f].z && c.z <= fixture_control.fixture[f].z + fixture_control.fixture[f].height)
                {
                if (solid_only == true && fixture_control.fixture[f].solid == false) clip = -1;
                else clip = f;
                }
              f += 1;
              }

      return clip;
      }

    ////////////////////////////////////////////////////////////////////////////////

    int character_in_brush (Character c)
      {
      int b = 0;
      int clip = -1;

      while (clip == -1 && b < brush_control.brush.Count)
        {
        if (c.x + (c.width / 2) >= brush_control.brush[b].x
            && c.x - (c.width / 2) <= brush_control.brush[b].x + brush_control.brush[b].width
            && c.y + (c.length / 2) >= brush_control.brush[b].y
            && c.y - (c.length / 2) <= brush_control.brush[b].y + brush_control.brush[b].length
            && c.z + c.height >= brush_control.brush[b].z
            && c.z <= brush_control.brush[b].z + brush_control.brush[b].height)
          {
          if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.DOOR_RED_V_TOP_CLOSED_TEST && red_doors_open == true) clip = -1;
          else if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.DOOR_RED_H_TOP_CLOSED_TEST && red_doors_open == true) clip = -1;
          else if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.DOOR_YELLOW_V_TOP_CLOSED_TEST && yellow_doors_open == true) clip = -1;
          else if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.DOOR_YELLOW_H_TOP_CLOSED_TEST && yellow_doors_open == true) clip = -1;
          else if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.DOOR_GREEN_V_TOP_CLOSED_TEST && green_doors_open == true) clip = -1;
          else if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.DOOR_GREEN_H_TOP_CLOSED_TEST && green_doors_open == true) clip = -1;
          else if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.DOOR_BLUE_V_TOP_CLOSED_TEST && blue_doors_open == true) clip = -1;
          else if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.DOOR_BLUE_H_TOP_CLOSED_TEST && blue_doors_open == true) clip = -1;
          else clip = b;
          }
        b += 1;
        }

      return clip;
      }

    ////////////////////////////////////////////////////////////////////////////////

    int character_in_character (int c)
      {
      int c2 = 0;
      int clip = -1;

      while (clip == -1 && c2 < character_control.character.Count)
        {
        if (c2 != c && character_control.active (c2)
            && character_control.character[c].x + (character_control.character[c].width / 2) >= character_control.character[c2].x && character_control.character[c].x - (character_control.character[c].width / 2) <= character_control.character[c2].x + (character_control.character[c2].width / 2)
            && character_control.character[c].y + (character_control.character[c].length / 2) >= character_control.character[c2].y && character_control.character[c].y - (character_control.character[c].length / 2) <= character_control.character[c2].y + (character_control.character[c2].length / 2)
            && character_control.character[c].z + character_control.character[c].height >= character_control.character[c2].z && character_control.character[c].z <= character_control.character[c2].z + character_control.character[c2].height)
          clip = c2;
        c2 += 1;
        }
      return clip;
      }

    ///////////////////////////////////////////////////////////////////////////////

    void Update_Brushes ()
      {
            int x, y;
            double distance;
            int f;
            int b_clip, f_clip;

            for (int b = 0; b < brush_control.brush.Count; b += 1)
              {
              if (brush_control.brush[b].moving_north == true || brush_control.brush[b].moving_south == true
                  || brush_control.brush[b].moving_east == true || brush_control.brush[b].moving_west == true)
                brush_control.brush[b].moving = true;
              else brush_control.brush[b].moving = false;

              // move north
              if (brush_control.brush[b].moving_north == true)
                {
                brush_control.brush[b].dy += brush_control.brush[b].ext_y_velocity;
                brush_control.brush[b].y = Convert.ToInt32 (brush_control.brush[b].dy);
                b_clip = brush_control.brush_in_brush (b);
                f_clip = brush_control.brush_in_fixture (brush_control.brush[b], fixture_control.fixture, true);
                //if (b_clip > -1 || f_clip > -1)  // hit another wall or fixture
                  //{
                  //brush_control.brush[b].dy -= brush_control.brush[b].ext_y_velocity;
                  //brush_control.brush[b].y = Convert.ToInt32 (brush_control.brush[b].dy);
                  //}
                if (b_clip > -1)  // hit another wall
                  {
                  brush_control.brush[b].dy = brush_control.brush[b_clip].y - brush_control.brush[b_clip].length;
                  brush_control.brush[b].y = Convert.ToInt32 (brush_control.brush[b].dy);
                  }
                if (f_clip > -1)  // hit a fixture
                  {
                  brush_control.brush[b].dy = fixture_control.fixture[f_clip].y - fixture_control.fixture[f_clip].length;
                  brush_control.brush[b].y = Convert.ToInt32 (brush_control.brush[b].dy);
                  }
                if (brush_control.brush[b].dy >= brush_control.brush[b].destination_y && character_control.character[PLAYER].brush_grab != b)  // hit or went past destination
                  {
                  brush_control.brush[b].moving_north = false;
                  brush_control.brush[b].ext_y_velocity = 0;
                  brush_control.brush[b].y = brush_control.brush[b].destination_y;
                  brush_control.brush[b].dy = brush_control.brush[b].destination_y;
                  }
                }

              // move south
              else if (brush_control.brush[b].moving_south == true)
                {
                brush_control.brush[b].dy -= brush_control.brush[b].ext_y_velocity;
                brush_control.brush[b].y = Convert.ToInt32 (brush_control.brush[b].dy);
                b_clip = brush_control.brush_in_brush (b);
                f_clip = brush_control.brush_in_fixture (brush_control.brush[b], fixture_control.fixture, true);
                //if (b_clip > -1 || f_clip > -1)  // hit another wall
                //{
                //brush_control.brush[b].dy += brush_control.brush[b].ext_y_velocity;
                //brush_control.brush[b].y = Convert.ToInt32 (brush_control.brush[b].dy);
                //}
                if (b_clip > -1)  // hit another wall
                  {
                  brush_control.brush[b].dy = brush_control.brush[b_clip].y + brush_control.brush[b_clip].length;
                  brush_control.brush[b].y = Convert.ToInt32 (brush_control.brush[b].dy);
                  }
                if (f_clip > -1)  // hit another wall
                  {
                  brush_control.brush[b].dy = fixture_control.fixture[f_clip].y + fixture_control.fixture[f_clip].length;
                  brush_control.brush[b].y = Convert.ToInt32 (brush_control.brush[b].dy);
                  }
                if (brush_control.brush[b].dy <= brush_control.brush[b].destination_y && character_control.character[PLAYER].brush_grab != b)  // hit or went past destination
                  {
                  brush_control.brush[b].moving_south = false;
                  brush_control.brush[b].ext_y_velocity = 0;
                  brush_control.brush[b].y = brush_control.brush[b].destination_y;
                  brush_control.brush[b].dy = brush_control.brush[b].destination_y;
                  }
                }

              // move east
              if (brush_control.brush[b].moving_east == true)
                {
                brush_control.brush[b].dx += brush_control.brush[b].ext_x_velocity;
                brush_control.brush[b].x = Convert.ToInt32 (brush_control.brush[b].dx);
                b_clip = brush_control.brush_in_brush (b);
                f_clip = brush_control.brush_in_fixture (brush_control.brush[b], fixture_control.fixture, true);
                //if (b_clip > -1 || f_clip > -1)  // hit another wall
                //{
                //brush_control.brush[b].dx -= brush_control.brush[b].ext_x_velocity;
                //brush_control.brush[b].x = Convert.ToInt32 (brush_control.brush[b].dx);
                //}
                if (b_clip > -1)  // hit another wall
                  {
                  brush_control.brush[b].dx = brush_control.brush[b_clip].x - brush_control.brush[b_clip].width;
                  brush_control.brush[b].x = Convert.ToInt32 (brush_control.brush[b].dx);
                  }
                if (f_clip > -1)  // hit a fixture
                  {
                  brush_control.brush[b].dx = fixture_control.fixture[f_clip].x - fixture_control.fixture[f_clip].width;
                  brush_control.brush[b].x = Convert.ToInt32 (brush_control.brush[b].dx);
                  }
                if (brush_control.brush[b].dx >= brush_control.brush[b].destination_x && character_control.character[PLAYER].brush_grab != b)  // hit or went past destination
                  {
                  brush_control.brush[b].moving_east = false;
                  brush_control.brush[b].ext_x_velocity = 0;
                  brush_control.brush[b].x = brush_control.brush[b].destination_x;
                  brush_control.brush[b].dx = brush_control.brush[b].destination_x;
                  }
                }

              // move west
              else if (brush_control.brush[b].moving_west == true)
                {
                brush_control.brush[b].dx -= brush_control.brush[b].ext_x_velocity;
                brush_control.brush[b].x = Convert.ToInt32 (brush_control.brush[b].dx);
                b_clip = brush_control.brush_in_brush (b);
                f_clip = brush_control.brush_in_fixture (brush_control.brush[b], fixture_control.fixture, true);
                //if (b_clip > -1 || f_clip > -1)  // hit another wall
                //{
                //brush_control.brush[b].dx += brush_control.brush[b].ext_x_velocity;
                //brush_control.brush[b].x = Convert.ToInt32 (brush_control.brush[b].dx);
                //}
                if (b_clip > -1)  // hit another wall
                  {
                  brush_control.brush[b].dx = brush_control.brush[b_clip].x + brush_control.brush[b_clip].width;
                  brush_control.brush[b].x = Convert.ToInt32 (brush_control.brush[b].dx);
                  }
                if (f_clip > -1)  // hit a fixture
                  {
                  brush_control.brush[b].dx = fixture_control.fixture[f_clip].x + fixture_control.fixture[f_clip].width;
                  brush_control.brush[b].x = Convert.ToInt32 (brush_control.brush[b].dx);
                  }
                if (brush_control.brush[b].dx <= brush_control.brush[b].destination_x && character_control.character[PLAYER].brush_grab != b)  // hit or went past destination
                  {
                  brush_control.brush[b].moving_west = false;
                  brush_control.brush[b].ext_x_velocity = 0;
                  brush_control.brush[b].x = brush_control.brush[b].destination_x;
                  brush_control.brush[b].dx = brush_control.brush[b].destination_x;
                  }
                }

              // snap brush_control.brush x and y to grid if not being moved
              // NEEDS TO BE ENHANCED WITH SMOOTH SNAPPING
              if (character_control.character[PLAYER].action != "grabbing" && character_control.character[PLAYER].action != "pushing" && brush_control.brush[b].moving == false)
                {
                // snap x
                x = 0;
                distance = box_move;

                while (x < brush_control.brush[b].dx)
                  {
                  distance = Convert.ToInt32 (brush_control.brush[b].dx - x);
                  if (distance < 0) distance = distance * -1;

                  x += box_move;
                  }

                if (x - brush_control.brush[b].dx < (box_move / 2)) brush_control.brush[b].dx = x;
                else
                  brush_control.brush[b].dx = x - box_move;
                brush_control.brush[b].x = Convert.ToInt32 (brush_control.brush[b].dx);

                // snap y
                y = 0;
                distance = box_move;

                while (y < brush_control.brush[b].dy)
                  {
                  distance = Convert.ToInt32 (brush_control.brush[b].dy - y);
                  if (distance < 0) distance = distance * -1;

                  y += box_move;
                  }

                if (y - brush_control.brush[b].dy < (box_move / 2)) brush_control.brush[b].dy = y;
                else
                  brush_control.brush[b].dy = y - box_move;
                brush_control.brush[b].y = Convert.ToInt32 (brush_control.brush[b].dy);
                }

              // laser tripwires
              f_clip = brush_control.brush_in_fixture (brush_control.brush[b], fixture_control.fixture, false);

              // conveyor belts
              if (brush_control.brush[b].moveable == true && brush_control.brush[b].moving == false && character_control.character[PLAYER].brush_grab != b)
                {
                f = brush_on_fixture (brush_control.brush[b]);
                if (f > -1)
                  {
                  if (fixture_control.fixture[f].type == (int) Fixture_Control.F.CONVEYOR_NORTH_TEST && fixture_control.fixture[f].on == true)
                    {
                    brush_control.brush[b].destination_y = brush_control.brush[b].y + tilesize;
                    brush_control.brush[b].moving = true;
                    brush_control.brush[b].moving_north = true;
                    brush_control.brush[b].ext_y_velocity = conveyor_speed;
                    }
                  else if (fixture_control.fixture[f].type == (int) Fixture_Control.F.CONVEYOR_SOUTH_TEST && fixture_control.fixture[f].on == true)
                    {
                    brush_control.brush[b].destination_y = brush_control.brush[b].y - tilesize;
                    brush_control.brush[b].moving = true;
                    brush_control.brush[b].moving_south = true;
                    brush_control.brush[b].ext_y_velocity = conveyor_speed;
                    }
                  else if (fixture_control.fixture[f].type == (int) Fixture_Control.F.CONVEYOR_WEST_TEST && fixture_control.fixture[f].on == true)
                    {
                    brush_control.brush[b].destination_x = brush_control.brush[b].x - tilesize;
                    brush_control.brush[b].moving = true;
                    brush_control.brush[b].moving_west = true;
                    brush_control.brush[b].ext_x_velocity = conveyor_speed;
                    }
                  else if (fixture_control.fixture[f].type == (int) Fixture_Control.F.CONVEYOR_EAST_TEST && fixture_control.fixture[f].on == true)
                    {
                    brush_control.brush[b].destination_x = brush_control.brush[b].x + tilesize;
                    brush_control.brush[b].moving = true;
                    brush_control.brush[b].moving_east = true;
                    brush_control.brush[b].ext_x_velocity = conveyor_speed;
                    }
                  }
                }

        // hot metal cooldown
        if (brush_control.brush[b].temperature > 70 && rnd.Next (0, 8) == 0) brush_control.brush[b].temperature -= 1;

        // particles
        if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.BOX_ICE_TEST)  // cold steam
          {
          if (rnd.Next (0, 100) == 0) particle_steam (brush_control.brush[b].x + rnd.Next (0, tilesize), brush_control.brush[b].y + rnd.Next (0, tilesize), brush_control.brush[b].z + brush_control.brush[b].height + 1, 270);
          }
        if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.INCINERATOR_TEST_DOWN)  // incinerator flames
          {
          particle_incinerator (brush_control.brush[b].x + (brush_control.brush[b].width / 2), brush_control.brush[b].y - 1, brush_control.brush[b].z + brush_control.brush[b].height - (tilesize / 4), 270, "brush_control.brush", b);
          }

        // door collisions
        //if (brush_control.brush[b].top_texture_number == Brush_Control.T.DOOR_
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void create_map ()
      {
      int random = 0;
      int random2 = 0;
      int mx, my, mz;
      bool endloop = true;
      int q = 0;
      int max_loop;
      bool tunnel_exception = false;

      if (creation_moves == 0)
        {
        game_state = CREATION;
        creation_mode = 1;

        map_tile_width = rnd.Next (8, 12);
        map_tile_length = rnd.Next (8, 12);
        map_tile_height = 3;
        ambient_dark = 0;

        random = rnd.Next (0, 6);
        if (random == 0) creation_wall = "WE";       // grey
        else if (random == 1) creation_wall = "WR";  // red
        else if (random == 2) creation_wall = "WG";  // minty green
        else if (random == 3) creation_wall = "WY";  // yellow
        else if (random == 4) creation_wall = "WP";  // purple
        else creation_wall = "MB";                   // blue

        random2 = rnd.Next (0, 4);
        if (random2 == 0) creation_floor = "ce";       // grey
        else if (random2 == 1) creation_floor = "fb";  // blue
        else if (random2 == 2) creation_floor = "tn";  // brown
        else creation_floor = "tk";                    // black

        // initialize map to blank spaces
        for (mz = 0; mz < map_tile_height; mz += 1)
          for (my = 0; my < map_tile_length; my += 1)
            for (mx = 0; mx < map_tile_width; mx += 1)
              matrixmap[mx, my, mz] = "..";

        // floor & walls
        for (my = 0; my < map_tile_length; my += 1)
          {
          for (mx = 0; mx < map_tile_width; mx += 1)
            {
            matrixmap[mx, my, 0] = creation_floor;
            matrixmap[mx, my, 1] = creation_wall;
            }
          }

        // exit & player
        random = rnd.Next (0, 4);
        if (random == 0)  // north
          {
          my = 0;
          mx = rnd.Next (1, map_tile_width - 2);
          creation_px = mx;
          creation_py = my + 1;
          creation_pz = 1;
          matrixmap[mx, my, 1] = "XR";
          }
        else if (random == 1)  // south
          {
          my = map_tile_length - 1;
          mx = rnd.Next (1, map_tile_width - 2);
          creation_px = mx;
          creation_py = my - 1;
          creation_pz = 1;
          matrixmap[mx, my, 1] = "XR";
          }
        else if (random == 2)  // west
          {
          mx = 0;
          my = rnd.Next (1, map_tile_length - 2);
          creation_px = mx + 1;
          creation_py = my;
          creation_pz = 1;
          matrixmap[mx, my, 1] = "Xr";
          }
        else  // east
          {
          mx = map_tile_width - 1;
          my = rnd.Next (1, map_tile_length - 2);
          creation_px = mx - 1;
          creation_py = my;
          creation_pz = 1;
          matrixmap[mx, my, 1] = "Xr";
          }

        matrixmap[creation_px, creation_py, creation_pz] = "p ";

        creation_moves += 1;
        }

      // player moves backward randomly and carves out walking paths from the walls
      // player creates boxes and pulls them backward to unsolve puzzle
      //for (creation_moves = 0; creation_moves < 100; creation_moves += 1)
      //  {
      else if (creation_moves > 0)
        {
        creation_box = false;

        // check for adjacent box to pull
        if (creation_mode == 2)
          {
          if (matrixmap[creation_px, creation_py + 1, creation_pz] == "b " && creation_py > 1)  // below
            {
            random = rnd.Next (0, 1);
            if (random == 0)
              {
              creation_box = true;
              creation_direction = 0;
              }
            }
          if (matrixmap[creation_px, creation_py - 1, creation_pz] == "b " && creation_py < map_tile_length - 2)  // above
            {
            random = rnd.Next (0, 1);
            if (random == 0)
              {
              creation_box = true;
              creation_direction = 1;
              }
            }
          if (matrixmap[creation_px + 1, creation_py, creation_pz] == "b " && creation_px > 1)  // to the right
            {
            random = rnd.Next (0, 1);
            if (random == 0)
              {
              creation_box = true;
              creation_direction = 2;
              }
            }
          if (matrixmap[creation_px - 1, creation_py, creation_pz] == "b " && creation_px < map_tile_width - 2)  // to the left
            {
            random = rnd.Next (0, 1);
            if (random == 0)
              {
              creation_box = true;
              creation_direction = 3;
              }
            }
          }

        if (creation_box == false)
          {
          // choose random direction
          creation_last_direction = creation_direction;
          tunnel_exception = false;
          endloop = false;
          max_loop = 25;
          q = 0;
          while (endloop == false && q < max_loop)
            {
            creation_direction = rnd.Next (0, 4);

            if (creation_direction == 0 && creation_py > 1) endloop = true;
            else if (creation_direction == 1 && creation_py < map_tile_length - 2) endloop = true;
            else if (creation_direction == 2 && creation_px > 1) endloop = true;
            else if (creation_direction == 3 && creation_px < map_tile_width - 2) endloop = true;

            // not back the way he came
            if (creation_direction == 0 && creation_last_direction == 1) endloop = false;
            else if (creation_direction == 1 && creation_last_direction == 0) endloop = false;
            else if (creation_direction == 2 && creation_last_direction == 3) endloop = false;
            else if (creation_direction == 3 && creation_last_direction == 2) endloop = false;

            // not the same as last direction
            if (creation_direction == creation_last_direction) endloop = false;

            // not through a box
            if (creation_direction == 0 && matrixmap[creation_px, creation_py - 1, creation_pz] == "b ") endloop = false;
            else if (creation_direction == 1 && matrixmap[creation_px, creation_py + 1, creation_pz] == "b ") endloop = false;
            else if (creation_direction == 2 && matrixmap[creation_px - 1, creation_py, creation_pz] == "b ") endloop = false;
            else if (creation_direction == 0 && matrixmap[creation_px + 1, creation_py, creation_pz] == "b ") endloop = false;

            // not through a wall in shuffle mode
            if (creation_mode == 2 && creation_direction == 0 && matrixmap[creation_px, creation_py - 1, creation_pz] == creation_wall) endloop = false;
            else if (creation_mode == 2 && creation_direction == 1 && matrixmap[creation_px, creation_py + 1, creation_pz] == creation_wall) endloop = false;
            else if (creation_mode == 2 && creation_direction == 2 && matrixmap[creation_px - 1, creation_py, creation_pz] == creation_wall) endloop = false;
            else if (creation_mode == 2 && creation_direction == 0 && matrixmap[creation_px + 1, creation_py, creation_pz] == creation_wall) endloop = false;

            q += 1;
            }

          // if no random direction found, force movement
          if (q >= max_loop)
            {
            creation_direction = -1;

            if (creation_mode == 1)  // wall tunnel mode
              {
              // any direction (staying inside the room, not clipping a box)
              if (creation_py > 1 && matrixmap[creation_px, creation_py - 1, creation_pz] != "b ") creation_direction = 0;
              else if (creation_py < map_tile_length - 2 && matrixmap[creation_px, creation_py + 1, creation_pz] != "b ") creation_direction = 1;
              else if (creation_px > 1 && matrixmap[creation_px, creation_py + 1, creation_pz] != "b ") creation_direction = 2;
              else if (creation_px < map_tile_width - 2 && matrixmap[creation_px, creation_py + 1, creation_pz] != "b ") creation_direction = 3;
              }
            else  // box shuffle mode
              {
              // any direction (staying inside the room, not clipping a box or wall)
              if (creation_py > 1 && matrixmap[creation_px, creation_py - 1, creation_pz] != "b " && matrixmap[creation_px, creation_py - 1, creation_pz] != creation_wall) creation_direction = 0;
              else if (creation_py < map_tile_length - 2 && matrixmap[creation_px, creation_py + 1, creation_pz] != "b " && matrixmap[creation_px, creation_py + 1, creation_pz] != creation_wall) creation_direction = 1;
              else if (creation_px > 1 && matrixmap[creation_px, creation_py + 1, creation_pz] != "b " && matrixmap[creation_px, creation_py + 1, creation_pz] != creation_wall) creation_direction = 2;
              else if (creation_px < map_tile_width - 2 && matrixmap[creation_px, creation_py + 1, creation_pz] != "b " && matrixmap[creation_px, creation_py + 1, creation_pz] != creation_wall) creation_direction = 3;

              // still stuck, tunnelling allowed
              else if (creation_py > 1 && matrixmap[creation_px, creation_py - 1, creation_pz] != "b ") { creation_direction = 0; tunnel_exception = true; }
              else if (creation_py < map_tile_length - 2 && matrixmap[creation_px, creation_py + 1, creation_pz] != "b ") { creation_direction = 1; tunnel_exception = true; }
              else if (creation_px > 1 && matrixmap[creation_px, creation_py + 1, creation_pz] != "b ") { creation_direction = 2; tunnel_exception = true; }
              else if (creation_px < map_tile_width - 2 && matrixmap[creation_px, creation_py + 1, creation_pz] != "b ") { creation_direction = 3; tunnel_exception = true; }
              }
            }

          // creating a box (at random)
          // make sure there's space for a box opposite the direction player is moving
          // make sure new boxes are only created in walls to prevent solution errors from backtracking
          random = 1;
          if (creation_direction == 0 && matrixmap[creation_px, creation_py + 1, creation_pz] == creation_wall && creation_py < map_tile_length - 2) random = rnd.Next (0, 1);
          if (creation_direction == 1 && matrixmap[creation_px, creation_py - 1, creation_pz] == creation_wall && creation_py > 1) random = rnd.Next (0, 1);
          if (creation_direction == 2 && matrixmap[creation_px + 1, creation_py, creation_pz] == creation_wall && creation_px < map_tile_width - 2) random = rnd.Next (0, 1);
          if (creation_direction == 3 && matrixmap[creation_px - 1, creation_py, creation_pz] == creation_wall && creation_px > 1) random = rnd.Next (0, 1);
          if (random == 0)
            {
            creation_box = true;
            if (creation_direction == 0)  // moving up
              {
              matrixmap[creation_px, creation_py + 1, creation_pz] = "b ";
              matrixmap[creation_px, creation_py + 1, creation_pz - 1] = "zr";
              }
            else if (creation_direction == 1)  // moving down
              {
              matrixmap[creation_px, creation_py - 1, creation_pz] = "b ";
              matrixmap[creation_px, creation_py - 1, creation_pz - 1] = "zr";
              }
            else if (creation_direction == 2)  // moving left
              {
              matrixmap[creation_px + 1, creation_py, creation_pz] = "b ";
              matrixmap[creation_px + 1, creation_py, creation_pz - 1] = "zr";
              }
            else if (creation_direction == 3)  // moving right
              {
              matrixmap[creation_px - 1, creation_py, creation_pz] = "b ";
              matrixmap[creation_px - 1, creation_py, creation_pz - 1] = "zr";
              }
            }
          }

        // random distance
        if (creation_direction == 0) creation_distance = rnd.Next (1, creation_py - 1);
        else if (creation_direction == 1) creation_distance = rnd.Next (1, map_tile_length - 2 - creation_py);
        else if (creation_direction == 2) creation_distance = rnd.Next (1, creation_px - 1);
        else if (creation_direction == 3) creation_distance = rnd.Next (1, map_tile_width - 2 - creation_px);

        // stop if box hit (do not overwrite boxes)
        // stop if wall hit in shuffle mode (non-tunnelling), unless exception granted
        q = 0;
        endloop = false;
        while (q < creation_distance && endloop == false)
          {
          matrixmap[creation_px, creation_py, creation_pz] = "..";
          if (creation_direction == 0)  // moving up
            {
            if (matrixmap[creation_px, creation_py - 1, creation_pz] == "b ") endloop = true;  // hit box
            else if (creation_mode == 2 && matrixmap[creation_px, creation_py - 1, creation_pz] == creation_wall && tunnel_exception == false) endloop = true;  // hit wall in shuffle mode
            else
              {
              if (creation_box == true) matrixmap[creation_px, creation_py + 1, creation_pz] = "..";
              creation_py -= 1;
              if (creation_box == true) matrixmap[creation_px, creation_py + 1, creation_pz] = "b ";
              }
            }
          else if (creation_direction == 1)  // moving down
            {
            if (matrixmap[creation_px, creation_py + 1, creation_pz] == "b ") endloop = true;  // hit box
            else if (creation_mode == 2 && matrixmap[creation_px, creation_py + 1, creation_pz] == creation_wall && tunnel_exception == false) endloop = true;  // hit wall in shuffle mode
            else
              {
              if (creation_box == true) matrixmap[creation_px, creation_py - 1, creation_pz] = "..";
              creation_py += 1;
              if (creation_box == true) matrixmap[creation_px, creation_py - 1, creation_pz] = "b ";
              }
            }
          else if (creation_direction == 2)  // moving left
            {
            if (matrixmap[creation_px - 1, creation_py, creation_pz] == "b ") endloop = true;
            else if (creation_mode == 2 && matrixmap[creation_px - 1, creation_py, creation_pz] == creation_wall && tunnel_exception == false) endloop = true;  // hit wall in shuffle mode
            else
              {
              if (creation_box == true) matrixmap[creation_px + 1, creation_py, creation_pz] = "..";
              creation_px -= 1;
              if (creation_box == true) matrixmap[creation_px + 1, creation_py, creation_pz] = "b ";
              }
            }
          else if (creation_direction == 3)  // moving right
            {
            if (matrixmap[creation_px + 1, creation_py, creation_pz] == "b ") endloop = true;
            else if (creation_mode == 2 && matrixmap[creation_px + 1, creation_py, creation_pz] == creation_wall && tunnel_exception == false) endloop = true;  // hit wall in shuffle mode
            else
              {
              if (creation_box == true) matrixmap[creation_px - 1, creation_py, creation_pz] = "..";
              creation_px += 1;
              if (creation_box == true) matrixmap[creation_px - 1, creation_py, creation_pz] = "b ";
              }
            }
          matrixmap[creation_px, creation_py, creation_pz] = "p ";

          q += 1;
          }

        creation_moves += 1;

        if (creation_moves > creation_max_moves / 2) creation_mode = 2;
        }

      if (creation_moves > creation_max_moves)
        {
        // eliminate goal zones starting beneath boxes
        for (mz = 1; mz < map_tile_height; mz += 1)
          for (my = 0; my < map_tile_length; my += 1)
            for (mx = 0; mx < map_tile_width; mx += 1)
              if (matrixmap[mx, my, mz] == "b " && matrixmap[mx, my, mz - 1] == "zr") matrixmap[mx, my, mz - 1] = creation_floor;

        // add enemies
        if (toggle_enemies == true)
          {
          for (my = 0; my < map_tile_length; my += 1)
            {
            for (mx = 0; mx < map_tile_width; mx += 1)
              {
              if (matrixmap[mx, my, 1] == "..")
                {
                random = rnd.Next (0, 7);
                if (random == 0)
                  {
                  random = rnd.Next (0, 3);
                  if (random == 0) matrixmap[mx, my, 1] = "h ";
                  else matrixmap[mx, my, 1] = "r ";
                  }
                }
              }
            }
          }

        // add lights
        if (lighting_engine == 1)
          {
          // ambient light
          random = rnd.Next (0, 4);
          if (random == 0) ambient_dark = .1f;
          else if (random == 1) ambient_dark = .2f;
          else if (random == 2) ambient_dark = .3f;
          else ambient_dark = .4f;

          for (my = 0; my < map_tile_length - 1; my += 1)
            {
            for (mx = 0; mx < map_tile_width - 1; mx += 1)
              {
              if (matrixmap[mx, my, 2] == ".."
                  && matrixmap[mx - 1, my, 2] == ".." && matrixmap[mx + 1, my, 2] == ".."
                  && matrixmap[mx, my - 1, 2] == ".." && matrixmap[mx, my + 1, 2] == "..")
                {
                random = rnd.Next (0, 5);
                if (random == 0)
                  {
                  random = rnd.Next (0, 7);
                  if (random == 0) matrixmap[mx, my, 2] = "ly";
                  if (random == 1) matrixmap[mx, my, 2] = "lb";
                  if (random == 2) matrixmap[mx, my, 2] = "lY";
                  if (random == 3) matrixmap[mx, my, 2] = "lB";
                  if (random == 4) matrixmap[mx, my, 2] = "lR";
                  else matrixmap[mx, my, 2] = "lw";
                  }
                }
              }
            }
          }

        creation_moves = 0;
        game_state = GAME;
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void load_map ()
      {
      int mx, my, mz;
      //int light_number = 0;
      //float map_alpha = 0f;
      //bool light_found;
      //int radius;
      //Stream filestream;
      //string levelPath = string.Format ("maps/{0}-{1}.txt", player_world, player_level);
      //string levelPath = "0.txt";
      //string gridspace;
      //int random = 0;

      if (randomized_map == true)
        {
        game_state = CREATION;
        create_map ();
        }

      else if (randomized_map == false)
        {
        textmap = new List<List<string>> ();
        for (int layer = 0; layer < 3; layer += 1) textmap.Add (new List<string> ());

        map.background = test_background4;

        // RICHARD'S HOUSE
        if (player_level == 0)
          {
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[0].Add(".   cp  cp  cp  cp  tn  tn  tn  tn  tn  tn  tn  tn  tn  tn  tn  tn  tn  .   ");
          textmap[0].Add(".   cp  cp  cp  cp  tn  tn  tn  tn  tn  tn  tn  tn  tn  tn  tn  tn  tn  .   ");
          textmap[0].Add(".   cp  cp  cp  cp  tn  tn  tn  tn  tn  tn  tn  tn  tn  tn  tn  tn  tn  .   ");
          textmap[0].Add(".   cp  cp  cp  cp  tn  tn  tn  tn  tn  tn  tn  tn  tn  tn  tn  tn  tn  .   ");
          textmap[0].Add(".   cp  cp  cp  cp  tn  tn  tn  tn  tn  tn  tn  tn  tn  tn  tn  tn  tn  .   ");
          textmap[0].Add(".   cp  cp  cp  cp  cp  cp  cp  cp  cp  cp  cp  cp  cp  cp  cp  cp  cp  .   ");
          textmap[0].Add(".   cp  cp  cp  cp  cp  cp  cp  cp  cp  cp  cp  cp  cp  cp  cp  cp  cp  .   ");
          textmap[0].Add(".   cp  cp  cp  cp  cp  cp  cp  cp  cp  cp  cp  cp  cp  cp  cp  cp  cp  .   ");
          textmap[0].Add(".   cp  cp  cp  cp  cp  cp  cp  cp  cp  cp  cp  cp  cp  cp  cp  cp  cp  .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");

          textmap[1].Add("DT  DT  DT  DT  DT  DT  DT  DT  DT  DT  DT  DT  DT  DT  DT  DT  DT  DT  DT  ");
          textmap[1].Add("DT  .   .   .   .   DT  .   .   .   .   .   .   .   .   .   DT  .   .   DT  ");
          textmap[1].Add("DT  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   DT  ");
          textmap[1].Add("DT  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   DT  ");
          textmap[1].Add("DT  .   .   .   .   DT  .   .   .   .   .   .   .   .   .   DT  .   c   DT  ");
          textmap[1].Add("DT  .   .   .   .   DT  DT  DT  DT  DT  DT  DT  .   .   DT  DT  DT  DT  DT  ");
          textmap[1].Add("DT  .   .   .   .   DT  pl1 c   co  .   .   rd  .   .   .   DT  .   .   DT  ");
          textmap[1].Add("DT  .   .   .   .   .   .   .   pn  .   .   .   .   .   .   DT  .   .   DT  ");
          textmap[1].Add("DT  .   .   .   .   .   .   .   .   tv  .   .   .   .   .   .   .   .   DT  ");
          textmap[1].Add("DT  .   .   .   .   DT  .   p1  .   .   .   .   .   .   .   .   .   .   DT  ");
          textmap[1].Add("BW  BW  BW  BW  BW  BW  BW  G1  BW  BW  BW  BW  BW  BW  BW  BW  BW  BW  BW  ");

          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   lw  .   lw  .   lw  .   lw  .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   ly  .   ly  .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   lw  .   lw  .   lw  .   lw  .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");

          //map_tile_width = textmap[0][0].Length / 3;//19;
          //map_tile_length = 11;
          //map_tile_height = 4;
          ambient_dark = .2f;
          ambient_light = new Color (96, 96, 64);
          map.background = test_background5;
          map.bg_scroll = true;
          random_retard1 = 0;
          random_retard2 = 0;
          }

        // RICHARD'S STREET
        if (player_level == 1)
          {
          //map_tile_width = 19;
          //map_tile_length = 11;
          //map_tile_height = 3;
          ambient_dark = .2f;
          ambient_light = new Color (112, 112, 112);
          map.background = test_background5;
          map.bg_scroll = true;
          random_retard1 = 0;
          random_retard2 = 0;

          textmap[0].Add("as  sw  gr  gr  gr  gr  gr  gr  gr  gr  gr  gr  gr  gr  gr  gr  gr  gr  gr  ");
          textmap[0].Add("as  sw  gr  gr  gr  gr  gr  gr  gr  gr  gr  gr  gr  gr  gr  gr  gr  gr  gr  ");
          textmap[0].Add("as  sw  gr  gr  gr  gr  gr  gr  gr  gr  gr  gr  gr  gr  gr  gr  gr  gr  gr  ");
          textmap[0].Add("as  sw  gr  gr  gr  gr  gr  gr  gr  gr  gr  gr  gr  gr  gr  gr  gr  gr  gr  ");
          textmap[0].Add("as  sw  gr  gr  gr  gr  gr  gr  gr  gr  gr  as  as  as  as  gr  gr  gr  gr  ");
          textmap[0].Add("as  sw  gr  gr  gr  gr  gr  gr  gr  gr  gr  as  as  as  as  gr  gr  gr  gr  ");
          textmap[0].Add("as  sw  gr  gr  gr  gr  gr  gr  gr  gr  gr  as  as  as  as  gr  gr  gr  gr  ");
          textmap[0].Add("as  sw  gr  gr  gr  gr  gr  gr  gr  gr  gr  as  as  as  as  gr  gr  gr  gr  ");
          textmap[0].Add("as  sw  gr  gr  gr  gr  gr  gr  gr  gr  gr  as  as  as  as  gr  gr  gr  gr  ");
          textmap[0].Add("as  sw  gr  gr  gr  gr  gr  gr  gr  gr  gr  as  as  as  as  gr  gr  gr  gr  ");
          textmap[0].Add("as  sw  sw  sw  sw  sw  sw  sw  sw  sw  sw  sw  sw  sw  sw  sw  sw  sw  sw  ");

          textmap[1].Add(".   .   .   .   BW  BW  BW  BW  BW  BW  BW  BW  BW  BW  BW  .   .   .   .   ");
          textmap[1].Add(".   .   .   .   BW  BW  BW  BW  BW  BW  BW  BW  BW  BW  BW  .   .   .   .   ");
          textmap[1].Add(".   .   .   .   BW  BW  BW  BW  BW  BW  BW  BW  BW  BW  BW  .   .   .   .   ");
          textmap[1].Add(".   .   .   .   BW  BW  BW  G0  BW  BW  BW  BW  BW  BW  BW  .   .   .   .   ");
          textmap[1].Add(".   .   .   .   .   .   .   p0  .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[1].Add(".   .   .   .   .   .   .   pn  .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[1].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[1].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[1].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[1].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[1].Add(".   .   .   .   .   .   .   G2  .   .   .   .   .   .   .   .   .   .   .   ");

          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   lw  .   lw  .   lw  .   lw  .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   ly  .   ly  .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   lw  .   lw  .   lw  .   lw  .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          }

        // LOBBY
        if (player_level == 2)
          {
          textmap[0].Add(".   .   .   .   ce  .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[0].Add(".   .   .   .   ce  .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[0].Add(".   .   .   psy ce  .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[0].Add(".   .   .   ce  ce  .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[0].Add(".   .   .   ce  ce  ce  ce  ce  ce  ce  ce  ce  ce  ce  ce  ce  ce  ce  ");
          textmap[0].Add(".   .   .   ce  ce  ce  ce  ce  ce  ce  ce  ce  ce  ce  ce  ce  ce  .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   tn  tn  .   .   .   .   .   .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   tn  tn  .   .   .   .   .   .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   tn  tn  .   .   .   .   .   .   .   ");
          textmap[0].Add("tk  tk  tk  tn  tn  tn  .   .   .   tn  tn  .   .   .   .   .   .   .   ");
          textmap[0].Add("tk  tk  tk  tn  tn  tn  .   .   .   tn  tn  .   .   .   .   .   .   .   ");
          textmap[0].Add("tk  tk  tk  tn  tn  tn  tn  tn  tn  tn  tn  .   .   .   .   .   .   .   ");
          textmap[0].Add("tk  tk  tk  tn  tn  tn  tn  tn  tn  tn  tn  tn  tn  .   .   .   .   .   ");
          textmap[0].Add("tk  tk  tk  tn  tn  tn  tn  tn  tn  tn  tn  tn  tn  .   .   .   .   .   ");
          textmap[0].Add("tn  tn  tn  tn  tn  tn  tn  tn  tn  tn  tn  tn  .   .   .   .   .   .   ");
          textmap[0].Add("tn  tn  tn  tn  tn  tn  tn  tn  tn  tk  tk  tk  .   .   .   .   .   .   ");
          textmap[0].Add("tn  tn  tn  tn  tn  tn  tn  tn  tn  tk  tk  tk  .   .   .   .   .   .   ");
          textmap[0].Add("tn  tn  tn  tn  tn  tn  tn  tn  tn  tk  tk  tk  .   .   .   .   .   .   ");
          textmap[0].Add(".   .   .   .   tn  tn  tn  tn  tn  tk  tk  tk  .   .   .   .   .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");

          textmap[1].Add(".   .   .   DM  G3  DM  .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[1].Add(".   .   DM  DM  DYV DM  .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[1].Add(".   .   DM  .   p3  DM  .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[1].Add(".   .   DM  .   .   DM  DM  DM  DM  DM  DM  DM  DM  DM  DM  DM  DM  DM  ");
          textmap[1].Add(".   .   DM  .   .   .   b   .   pl1 .   vm  .   .   .   .   .   p4  g4  ");
          textmap[1].Add(".   .   DM  .   .   .   .   .   .   .   .   .   .   .   .   .   .   DM  ");
          textmap[1].Add(".   .   DM  DM  DM  DM  DM  DM  DM  DM  .   DM  DM  DM  DM  DM  DM  DM  ");
          textmap[1].Add(".   .   .   .   .   .   .   .   DT  .   .   DT  .   .   .   .   .   .   ");
          textmap[1].Add("DT  DT  DT  DT  DT  DT  DT  .   DT  .   .   DT  .   .   .   .   .   .   ");
          textmap[1].Add("DT  .   .   DT  .   .   DT  .   DT  .   DT  DT  .   .   .   .   .   .   ");
          textmap[1].Add("DT  .   .   .   .   .   DT  DT  DT  b   .   DT  .   .   .   .   .   .   ");
          textmap[1].Add("DT  DT  DT  DT  .   .   .   c1s t1  .   .   DT  DT  DT  .   .   .   .   ");
          textmap[1].Add("DT  .   .   DT  .   .   t2  .   .   .   .   .   .   DT  .   .   .   .   ");
          textmap[1].Add("DT  .   .   .   .   .   .   .   .   .   .   .   .   DT  .   .   .   .   ");
          textmap[1].Add("DT  DT  DT  DT  .   .   .   .   .   .   .   .   BR  DT  .   .   .   .   ");
          textmap[1].Add("DT  .   .  .    .   .   .   .   BR  .   .   .   BR  .   .   .   .   .   ");
          textmap[1].Add("DT  .   .   .   .   .   .   .   BR  .   .   .   BR  .   .   .   .   .   ");
          textmap[1].Add("DT  .   .   .   .   .   .   .   BR  .   .   b   BR  .   .   .   .   .   ");
          textmap[1].Add("DT  DT  DT  DT  pl1 .   pn  .   BR  .   b   b   BR  .   .   .   .   .   ");
          textmap[1].Add(".   .   .   BR  BR  BR  EN  BR  BR  BR  BR  BR  BR  .   .   .   .   .   ");

          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   lw  .   .   .   lw  .   .   .   lw  .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   lw  .   .   .   .   .   00  .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   00  00  .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   ly  .   ly  .   ly  .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   lw  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   ly  .   ly  .   ly  .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   ly  .   ly  .   ly  .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   ly  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   ly  .   ly  .   ly  .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");

          map_tile_width = textmap[0][0].Length / 4;// 18;
          map_tile_length = textmap.Count;// 20;
          map_tile_height = 3;
          ambient_dark = .35f;
          ambient_light = new Color (80, 80, 72);
          map.background = brush_control.texture[(int) Brush_Control.T.GRASS];
          map.bg_scroll = false;
          random_retard1 = 0;
          random_retard2 = 0;
          }

        // MACHINE ROOM
        if (player_level == 3)
          {
          //map_tile_width = 30;
          //map_tile_length = 21;
          //map_tile_height = 3;
          ambient_dark = .3f;
          ambient_light = new Color (72, 72, 72);
          map.background = test_background1;
          map.bg_scroll = true;
          random_retard1 = 1;
          random_retard2 = 0;

          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   cp  cp  cp  cp  cp  .   .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   cp  cp  cp  cp  cp  .   .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   cp  cp  cp  cp  cp  .   .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   cp  cp  cp  cp  cp  .   .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   cp  cp  cp  cp  cp  .   .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   tk  .   .   .   .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   tk  tk  zr  zr  zr  tk  tk  tk  tk  .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   tk  tk  tk  tk  tk  .   .   .   .   .   tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  .   ");
          textmap[0].Add(".   .   fg  fg  fg  fg  fg  tk  tk  tk  tk  tk  tk  .   .   .   .   .   .   tk  tk  tk  tk  tk  tk  tk  tk  tk  .   .   ");
          textmap[0].Add(".   .   fg  fg  fg  fg  fg  tk  tk  tk  tk  tk  tk  .   .   .   .   .   .   tk  tk  tk  tk  tk  tk  tk  tk  tk  .   .   ");
          textmap[0].Add(".   .   fg  fg  fg  fg  fg  tk  tk  tk  tk  tk  tk  .   .   .   .   .   .   tk  tk  tk  tk  tk  tk  tk  tk  tk  .   .   ");
          textmap[0].Add(".   .   fg  fg  fg  fg  fg  tk  tk  tk  tk  tk  .   .   .   .   .   .   .   tk  tk  tk  tk  tk  tk  tk  tk  tk  psg .   ");
          textmap[0].Add(".   fg  fg  fg  fg  fg  fg  fg  fg  fg  fg  fg  .   fg  fg  .   .   fg  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  .   ");
          textmap[0].Add(".   fg  .   .   fg  .   .   fg  .   .   .   .   .   .   .   .   .   .   .   tk  tk  tk  tk  tk  tk  tk  tk  tk  .   .   ");
          textmap[0].Add(".   fg  .   .   fg  .   .   fg  .   .   .   .   .   .   .   .   .   .   .   .   tk  tk  .   .   .   tk  tk  .   .   .   ");
          textmap[0].Add(".   fg  .   .   fg  .   .   fg  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[0].Add(".   fg  fg  fg  fg  fg  fg  fg  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[0].Add(".   .   .   .   fg  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[0].Add(".   .   .   .   fg  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");

          textmap[1].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   DY  DY  DY  DY  DY  DY  DY  .   .   ");
          textmap[1].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   DY  fc  fc  .   .   .   DY  .   .   ");
          textmap[1].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   DY  .   .   k   .   .   DY  .   .   ");
          textmap[1].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   DY  .   t2  .   .   .   DY  .   .   ");
          textmap[1].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   DY  .   .   .   .   .   DY  .   .   ");
          textmap[1].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   DY  c   .   .   .   pl1 DY  .   .   ");
          textmap[1].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   MB  MB  MB  MB  MB  MB  DRV MB  MB  MB  MB  .   ");
          textmap[1].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   MB  MB  b   .   .   .   .   .   .   b   b   MB  MB  ");
          textmap[1].Add(".   .   .   .   .   .   .   MM  MM  MM  MM  MM  MM  MM  .   .   .   MB  .   .   .   .   .   .   .   .   .   b   b   MB  ");
          textmap[1].Add(".   MM  MM  MM  MM  MM  MM  MM  c   vm  .   .   .   MM  .   .   .   MB  b   .   .   WE  .   .   .   WE  .   .   .   MB  ");
          textmap[1].Add(".   MM  c   .   .   .   .   MM  .   .   .   .   .   MM  .   .   .   MB  MB  .   .   .   b   .   .   .   .   .   MB  MB  ");
          textmap[1].Add(".   MM  .   BM  00  00  .   MM  .   .   .   .   .   MM  .   .   .   .   MB  b   .   .   .   ,,  .   .   b   .   MB  .   ");
          textmap[1].Add(".   MM  .   00  00  00  .   MM  .   .   .   .   .   MM  .   .   .   .   MB  .   .   .   .   .   m   .   .   b   MB  MB  ");
          textmap[1].Add("MM  MM  .   00  00  00  .   MM  MM  MM  DGV MM  MM  .   .   .   MM  MM  MB  .   .   WE  .   .   .   WE  .   m   .   MB  ");
          textmap[1].Add("MM  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   b   .   .   .   .   .   .   .   MB  ");
          textmap[1].Add("MM  .   .   .   .   .   .   .   MM  MM  MM  .   .   MM  .   .   .   MM  MB  .   .   .   .   .   m   .   b   .   MB  MB  ");
          textmap[1].Add("MM  .   .   .   .   .   .   .   MM  .   .   .   .   .   .   .   .   .   MB  MB  .   .   MB  MB  MB  c   b   MB  MB  .   ");
          textmap[1].Add("MM  .   .   .   .   .   .   .   MM  .   .   .   .   .   .   .   .   .   .   MB  MB  MB  MB  .   MB  MB  MB  MB  .   .   ");
          textmap[1].Add("MM  .   .   .   pn  .   .   .   MM  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[1].Add("MM  MM  MM  MM  p2  MM  MM  MM  MM  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[1].Add(".   .   .   MM  G2  MM  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");

          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ly  .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   lw  .   lw  .   lw  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   lw  .   lw  .   lw  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   lw  .   lR  .   lw  .   lb  .   lb  .   lb  .   .   .   lb  .   lb  .   lw  .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   lw  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   lw  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          }

        // METAL ZONE MAIN
        if (player_level == 4)
          {
          //map_tile_width = 22;
          //map_tile_length = 18;
          //map_tile_height = 3;
          ambient_dark = .3f;
          ambient_light = new Color (72, 72, 72);
          map.background = test_background1;
          map.bg_scroll = true;
          random_retard1 = 0;
          random_retard2 = 0;

          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   tk  .   .   .   .   .   .   .   .   .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   tk  tk  tk  tk  tk  .   .   .   .   .   .   .   .   ");
          textmap[0].Add(".   .   .   .   tk  .   .   .   .   tk  ce  ce  ce  tk  .   .   .   .   .   .   .   .   ");
          textmap[0].Add(".   .   .   tk  tk  tk  .   .   .   tk  ce  ce  ce  tk  .   .   .   .   .   .   .   .   ");
          textmap[0].Add(".   .   .   tk  tk  tk  .   .   .   tk  ce  ce  ce  tk  .   .   .   .   .   .   .   .   ");
          textmap[0].Add(".   .   .   tk  tk  tk  tk  tk  fg  tk  ce  ce  ce  tk  fg  tk  tk  tk  tk  tk  tk  .   ");
          textmap[0].Add(".   .   .   tk  tk  tk  tk  tk  fg  tk  ce  ce  ce  tk  fg  tk  tk  tk  tk  tk  tk  tk  ");
          textmap[0].Add(".   .   .   tk  tk  tk  tk  tk  fg  tk  ce  ce  ce  tk  fg  tk  tk  tk  tk  tk  tk  .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   tk  ce  ce  ce  tk  tk  tk  tk  tk  tk  tk  tk  .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   tk  ce  ce  ce  tk  tk  tn  tn  tn  tn  tn  tn  .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   tk  ce  ce  ce  tk  tk  tn  tn  tn  tn  tn  tn  .   ");
          textmap[0].Add(".   .   .   .   .   .   .   tk  fg  tk  ce  ce  ce  tk  tk  tn  tn  tn  tn  tn  tn  .   ");
          textmap[0].Add(".   .   .   .   .   .   tk  tk  fg  tk  ce  ce  ce  tk  tk  tn  tn  tn  tn  tn  tn  .   ");
          textmap[0].Add(".   .   .   .   .   .   .   tk  fg  tk  ce  ce  ce  tk  tk  tn  tn  tn  tn  tn  tn  .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   tk  ce  ce  ce  tk  tk  tn  tn  tn  tn  tn  tn  .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   tk  tk  tk  tk  tk  tk  tn  tn  tn  tn  tn  tn  .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   tk  tk  tk  tk  tk  .   .   .   .   .   .   .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   tk  .   .   .   .   .   .   .   .   .   .   ");
          
          textmap[1].Add(".   .   .   .   .   .   .   .   MB  MB  MB  XR  MB  MB  MB  .   .   .   .   .   .   .   ");
          textmap[1].Add(".   .   .   .   .   .   .   .   MB  pl1 .   .   .   pl1 MB  .   .   .   .   .   .   .   ");
          textmap[1].Add(".   .   MB  MB  G7  MB  MB  .   MB  .   .   .   .   .   MB  .   .   .   .   .   .   .   ");
          textmap[1].Add(".   .   MB  .   p7  .   MB  .   MB  .   .   .   .   .   MB  .   .   .   .   .   .   .   ");
          textmap[1].Add(".   .   MB  .   .   .   MB  MB  MB  .   .   .   .   .   MB  MB  MB  MB  MB  MB  MB  MB  ");
          textmap[1].Add(".   .   MB  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   MB  ");
          textmap[1].Add(".   .   MB  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   p6  g6  ");
          textmap[1].Add(".   .   MB  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   MB  ");
          textmap[1].Add(".   .   MB  MB  MB  MB  MB  MB  MB  .   .   .   .   .   DY  DY  DY  .   .   DY  DY  DY  ");
          textmap[1].Add(".   .   .   .   .   .   .   .   DY  .   .   .   .   .   .   vm  .   .   .   .   .   DY  ");
          textmap[1].Add(".   .   .   .   .   .   DY  DY  DY  .   .   .   .   .   .   .   .   .   .   .   .   DY  ");
          textmap[1].Add(".   .   .   .   .   .   DY  .   .   .   .   .   .   .   DY  .   .   t1  t1  .   .   DY  ");
          textmap[1].Add(".   .   .   .   .   .   g2  p2  pn  .   .   .   .   .   DY  .   .   .   .   .   .   DY  ");
          textmap[1].Add(".   .   .   .   .   .   DY  .   .   .   .   .   .   .   DY  .   .   t2  .   .   .   DY  ");
          textmap[1].Add(".   .   .   .   .   .   DY  DY  DY  .   .   .   .   .   .   .   .   .   .   .   .   DY  ");
          textmap[1].Add(".   .   .   .   .   .   .   .   DY  .   .   .   .   .   .   .   .   .   .   .   .   DY  ");
          textmap[1].Add(".   .   .   .   .   .   .   .   DY  .   .   p5  .   .   DY  DY  DY  DY  DY  DY  DY  DY  ");
          textmap[1].Add(".   .   .   .   .   .   .   .   DY  DY  DY  G5  DY  DY  DY  .   .   .   .   .   .   .   ");

          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   lw  .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   lw  .   lw  .   lw  .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   lw  .   lw  .   lw  .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   lw  .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   lw  .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          }

        // metal zone 1
        if (player_level == 5)
          {
          //map_tile_width = 5;
          //map_tile_length = 5;
          //map_tile_height = 3;
          ambient_dark = .3f;
          ambient_light = new Color (72, 72, 72);
          map.background = test_background1;
          map.bg_scroll = true;
          random_retard1 = 0;
          random_retard2 = 0;

          textmap[0].Add(".   .   tk  .   .   ");
          textmap[0].Add(".   tk  tk  tk  .   ");
          textmap[0].Add(".   tk  tk  tk  .   ");
          textmap[0].Add(".   tk  tk  tk  .   ");
          textmap[0].Add(".   .   .   .   .   ");

          textmap[1].Add("BE  BE  G4  BE  BE  ");
          textmap[1].Add("BE  .   p4  .   BE  ");
          textmap[1].Add("BE  .   pn  .   BE  ");
          textmap[1].Add("BE  .   .   .   BE  ");
          textmap[1].Add("BE  BE  BE  BE  BE  ");

          textmap[2].Add(".   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   ");
          }

        /*
        // metal zone 1
        if (player_level == 5)
          {
          //map_tile_width = 46;
          //map_tile_length = 16;
          //map_tile_height = 3;
          ambient_dark = .3f;
          ambient_light = new Color (72, 72, 72);
          map.background = test_background1;
          map.bg_scroll = true;
          random_retard1 = 4;
          random_retard2 = 0;

          textmap[0].Add(".  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .     tk .  .  .  .  .  .  .  .  ");
          textmap[0].Add(".  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  tk tk tk tk tk .  .  .  .  .  ");
          textmap[0].Add(".  .  .  .  .  tk tn tn tn tn tn tn tn tn tn tn tn tn tn tn tn tn tn tn tn tn .  .  .. .. .. .. .. .. .. .. tk tk tk tk tk .. .. .. .. .. ");
          textmap[0].Add(".  .  .  .  .  tk tn tn tn tn tn tn tn tn tn tn tn tn tn tn tn tn tn tn tn tn .  .  .. .. .. .. .. .. .. .. tk tk tk tk tk tk tk tk .. .. ");
          textmap[0].Add(".  .  .  .  .  tk fg fg fg fg fg fg fg tn tn tn tn tn tn tn tn tn tn tn tn tn .  .  .. .. .. .. .. .. .. .. tk tk tk tk tk tk tk tk .. .. ");
          textmap[0].Add(".  .  .  .  .  tk fg fg fg fg fg fg fg tn tn tn tn tn tn tn tn tn tn tn tn tn .  .  .. .. .. .. .. .. .. .. tk tk tk tk tk tk tk tk sg .. ");
          textmap[0].Add(".  .  .  .  .  tk fg fg fg fg fg fg fg tn tn tn tn tn tn tn tn tn tn tn tn tn .  .  .. .. .. .. .. .. .. .. tk tk tk tk tk tk tk tk tk .. ");
          textmap[0].Add(".  .  .  .  .  tk fg fg fg fg fg fg fg tn tn tn tn tn tn tn tn tn tn tn tn tn .  .  .. .. .. .. .. .. .. .. tk tk tk tk tk tk tk tk tk .. ");
          textmap[0].Add(".  .  fg fg fg fg fg fg fg fg fg fg fg fg fg fg fg fg fg fg fg fg fg fg fg fg .  .  .. .. .. .. .. .. .. .. tk tk tk tk tk tk tk tk tk .. ");
          textmap[0].Add(".  .  fg fg fg fg fg fg fg fg fg fg fg fg fg fg fg fg fg fg fg fg fg fg fg fg fg fg fg .. fg fg fg .. fg .. tk tk tk tk tk tk tk tk tk .. ");
          textmap[0].Add(".  .  fg fg fg fg fg fg fg fg fg fg .  .  .  .  .  .  .  .  .  fg fg fg fg fg .  .  .. .. .. .. .. .. .. .. tk tk tk tk tk tk tk tk tk .. ");
          textmap[0].Add(".  .  .  .  .  .  .  fg .  .  .  fg .  .  .  .  .  .  .  .  .  ce ce ce ce ce .  .  .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[0].Add(".  .  .  .  .  .  .  fg .  .  .  fg .  .  .  .  .  .  .  .  .  ce ce ce ce ce .  .  .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[0].Add(".  .  .  .  .  .  .  fg .  .  .  fg .  .  .  .  .  .  .  .  .  ce ce ce ce ce .  .  .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[0].Add(".  .  .  .  .  .  .  fg fg fg fg fg .  .  .  .  .  .  .  .  .  ce ce ce ce ce .  .  .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[0].Add(".  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");

          textmap[1].Add(".  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .. .. .. .. .. .. WR WR G3 WR WR WR WP .. .. .. .. ");
          textmap[1].Add(".  .  .  .  .  WY WY WY WY WY WY WY WY WY WY WY WY WY WY WY WY WY WY WY WY WY WY .  .  .. .. .. .. .. .. WR ,, p3 .. .. .  WP .. .. .. .. ");
          textmap[1].Add(".  .  .  .  .  WY .  .  .  .  .  .  .  m  .  .  .  .  fc WY WY .  .  .  .  .  WY .  .  .. .. .. .. .. .. WR .. .. WP WP WP WP WP WP WP .. ");
          textmap[1].Add(".  .  .  .  .  WY k  .  .  c  .  .  sm WG t1 .  .  .  .  b  .  .  .  .  .  .  WY .  .  .. .. .. .. .. .. WR .. .. WP .. .. .. .. .. WP .. ");
          textmap[1].Add(".  .  .  .  .  WG WG WG WG WG WG WG WG WG .  .  .  .  .  .  .  .  .  r1 .  .  WY .  .  .. .. .. .. .. .. WR .. .. .. .. .. WP WP b  WP WP ");
          textmap[1].Add(".  .  .  .  .  WG WG c  .  .  .  .  WG WG .  .  .  .  .  WY WY .  .  m  .  .  WY .  .  .. .. .. .. .. .. WR h  ,, WP .. b  WP b  .. .. WP ");
          textmap[1].Add(".  .  .  .  .  WG WG .  BM 00 00 .  WG WG .  .  .  .  .  WY WY h  .  b  .  .  WY .  .  .. .. .. .. .. .. WP WP WP WP b  .. WP .  ,, m  WP ");
          textmap[1].Add(".  .  .  .  .  WG WG .  00 00 00 .  WG WG WG WG b  WY WY WY WY WY WY .  WY WY WY WG WG .. .. .. .. .. .. WP .. .. r2 b  b  WP .  .. .. WP ");
          textmap[1].Add(".  MM MM MM WG WG WG .  00 00 00 .  WG WG WG WG .  WG WG WG WG WG WG .  WG WG WG WG WG WG WG .. WG .. WG WP .. .. .. .. b  .  .  .. .. WP ");
          textmap[1].Add(".  MM c  .  .  pn p1 .  .  .  .  .  .  b  .  .  .  .  b  c  .  .  .  .  .  ,, .  ,, .  .. .. .. .. .. .. .. .. .. .. .. .. WP .  .. .. WP ");
          textmap[1].Add(".  MM .  .  .  .  .  .  .  .  .  .  WG WG WG WG WG WG WG WG WG WG WG DG WG WG WG WG .  .. .. WG .. .. .. WP WP .. .. .. .. WP c  ,, h  WP ");
          textmap[1].Add(".  MM MM EN MM WG WG .  .  .  .  .  WG .  .  .  .  .  .  .  WG .  .  .  .  .  WG .  .  .. .. .. .. .. .. WP WP WP WP WP WP WP WP WP WP WP ");
          textmap[1].Add(".  .  .  .  .  .  WG .  .  .  .  .  WG .  .  .  .  .  .  .  WG .  .  .  .  .  WG .  .  .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[1].Add(".  .  .  .  .  .  WG .  .  .  .  .  WG .  .  .  .  .  .  .  WG .  .  .  .  .  WG .  .  .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[1].Add(".  .  .  .  .  .  WG .  .  c  .  .  WG .  .  .  .  .  .  .  WG .  .  k  .  .  WG .  .  .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[1].Add(".  .  .  .  .  .  WG WG WG WG WG WG WG .  .  .  .  .  .  .  WG WG WG WG WG WG WG .  .  .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");

          textmap[2].Add(".. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. WR WR WR WR WR WR .. .. .. .. .. ");
          textmap[2].Add(".. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. lY .. .. .. .. .. .. .. .. .. .. ");
          textmap[2].Add(".. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[2].Add(".. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[2].Add(".. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[2].Add(".. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. lw .. lw .. .. .. .. .. .. ");
          textmap[2].Add(".. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[2].Add(".. .. .. .. .. .. .. lw .. lw .. lw .. .. .. lw .. lw .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. lw .. lw .. lw .. .. .. .. .. .. ");
          textmap[2].Add(".. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[2].Add(".. .. .. lw .. lw .. lw .. lR .. lw .. lB .. lB .. lB .. lB .. lB .. lB .. lB .. lB .. .. .. .. .. .. .. lw .. lw .. lw .. .. .. .. .. .. ");
          textmap[2].Add(".. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[2].Add(".. .. .. .. .. .. .. lw .. lw .. lw .. .. .. .. .. .. .. .. .. .. .. lw .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[2].Add(".. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[2].Add(".. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. lw .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[2].Add(".. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          textmap[2].Add(".. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ..  .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. .. ");
          }
        */

        // metal zone 2
        else if (player_level == 6)
          {
          //map_tile_width = 29;
          //map_tile_length = 19;
          //map_tile_height = 3;
          ambient_dark = .4f;
          ambient_light = new Color (64, 64, 64);
          map.background = test_background5;
          map.bg_scroll = true;
          random_retard1 = 6;
          random_retard2 = 0;// 2;

          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   psy ce  ce  ce  ce  ce  .   .   .   .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ce  ce  ce  ce  ce  ce  .   .   .   .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ce  ce  ce  ce  ce  ce  .   .   .   .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ce  ce  ce  ce  ce  ce  .   .   .   .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ce  ce  ce  ce  ce  ce  .   .   .   .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ce  ce  ce  ce  ce  ce  psg .   .   .   .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   tk  tk  tk  tk  tk  tk  tk  tk  ce  ce  ce  ce  ce  ce  ce  .   .   .   .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   tk  tk  tk  tk  tk  tk  tk  tk  ce  ce  ce  ce  ce  ce  ce  tk  tk  tk  tk  .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   tk  tk  tk  tk  tk  tk  tk  tk  ce  ce  ce  ce  ce  ce  ce  tk  tk  tk  tk  .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   tk  WR  WR  tk  tk  tk  tk  tk  ce  ce  ce  ce  ce  ce  ce  tk  tk  tk  tk  .   ");
          textmap[0].Add(".   .   .   .   .   .   tk  tk  tk  tk  tk  tk  tk  WR  PSB tk  tk  ce  ce  ce  ce  ce  ce  ce  .   .   .   .   .   ");
          textmap[0].Add(".   .   .   .   .   .   tk  fm  fm  fm  fm  fm  tk  WR  tk  .   .   ce  ce  ce  ce  ce  ce  ce  .   .   .   .   .   ");
          textmap[0].Add(".   tk  tk  tk  tk  tk  tk  fm  fm  fm  fm  fm  tk  tk  tk  .   .   .   .   ce  ce  ce  .   .   .   .   .   .   .   ");
          textmap[0].Add("tk  tk  tk  tk  tk  tk  tk  fm  fm  fm  fm  fm  tk  tk  tk  .   .   .   .   tk  tk  tk  .   .   .   .   .   .   .   ");
          textmap[0].Add(".   tk  tk  tk  tk  tk  tk  fm  fm  fm  fm  fm  tk  tk  tk  .   .   .   .   tk  tk  tk  .   .   .   .   .   .   .   ");
          textmap[0].Add(".   .   .   .   .   .   tk  fm  fm  fm  fm  fm  tk  tk  tk  .   .   .   .   PSR tk  tk  .   .   .   .   .   .   .   ");
          textmap[0].Add(".   .   .   .   .   .   tk  tk  tk  tk  tk  tk  tk  tk  tk  .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");

          textmap[1].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   DY  DY  DY  DY  DY  DY  DY  DY  .   .   .   .   ");
          textmap[1].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   DY  .   c   .   .   .   fc  DY  .   .   .   .   ");
          textmap[1].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   DY  ,,  t2  .   .   b   .   DY  .   .   .   .   ");
          textmap[1].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   DY  .   ,,  ,,  .   .   .   DY  .   .   .   .   ");
          textmap[1].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   DY  .   .   .   .   .   .   DY  .   .   .   .   ");
          textmap[1].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   DP  DP  DP  DGV DP  DP  DP  DP  DP  .   .   .   .   ");
          textmap[1].Add(".   .   .   .   .   .   .   .   DP  DP  DP  DP  DP  DP  DP  DP  DP  .   .   .   t2  .   b   .   DP  .   .   .   .   ");
          textmap[1].Add(".   .   .   .   .   .   .   .   DP  .   k   .   DP  .   .   .   .   .   ,,  .   ,,  .   .   .   DP  BR  BR  BR  BR  ");
          textmap[1].Add(".   .   .   .   .   .   .   .   DP  .   .   .   .   .   .   b   .   .   .   .   .   .   .   .   DP  .   .   .   BR  ");
          textmap[1].Add(".   .   .   .   .   .   .   .   DP  ,,  .   h   DP  DP  DP  DP  DP  .   .   .   m   .   .   .   DRH ,,  k   ,,  BR  ");
          textmap[1].Add(".   .   .   .   .   BR  BR  BR  BR  DBV BR  BR  BR  BR  BR  BR  DP  t1  ,,  .   .   .   .   .   DP  .   .   .   BR  ");
          textmap[1].Add(".   .   .   .   .   BR  b   .   .   .   ,,  .   .   BR  .   BR  DP  .   m   .   .   .   .   .   DP  BR  BR  BR  BR  ");
          textmap[1].Add("BR  BR  BR  BR  BR  BR  ,,  .   .   .   .   .   .   BR  .   BR  DP  t2  .   .   .   .   .   pl1 DP  .   .   .   .   ");
          textmap[1].Add("BR  .   .   b   b   BR  .   .   .   .   .   .   .   .   .   BR  DP  DP  DP  DP  DYV DP  DP  DP  DP  .   .   .   .   ");
          textmap[1].Add("g4  p4  pn  .   b   .   .   .   .   .   ,,  .   .   .   ,,  BR  .   .   DY  b   ,,  .   DY  .   .   .   .   .   .   ");
          textmap[1].Add("BR  .   .   .   .   BR  .   .   .   .   .   .   .   .   .   BR  .   .   DY  .   sy  .   DY  .   .   .   .   .   .   ");
          textmap[1].Add("BR  BR  BR  BR  BR  BR  .   .   .   .   .   .   .   ,,  b   BR  .   .   DY  .   ,,  .   DY  .   .   .   .   .   .   ");
          textmap[1].Add(".   .   .   .   .   BR  c   .   .   .   ,,  .   .   b   b   BR  .   .   DY  DY  DY  DY  DY  .   .   .   .   .   .   ");
          textmap[1].Add(".   .   .   .   .   BR  BR  BR  BR  BR  BR  BR  BR  BR  BR  BR  .   .   .   .   .   .   .   .   .   .   .   .   .   ");

          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   lg  .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   lw  .   lw  .   lw  .   lw  .   lw  .   lw  .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   lF  .   lw  .   .   .   .   .   .   .   lw  .   lw  .   lR  .   lR  .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   lw  .   lw  .   lw  .   lF  .   lF  .   .   .   lw  .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   lw  .   lw  .   .   .   lw  .   lw  .   lw  .   lw  .   .   .   .   .   lY  .   lY  .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   lw  .   lw  .   .   .   lw  .   lw  .   lw  .   .   .   .   .   .   .   lY  .   lY  .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          }

        // metal zone 3
        else if (player_level == 7)
          {
          //map_tile_width = 15;
          //map_tile_length = 10;
          //map_tile_height = 3;
          ambient_dark = .3f;
          ambient_light = new Color (64, 64, 64);
          map.background = test_background4;
          map.bg_scroll = true;
          random_retard1 = 2;
          random_retard2 = 0;

          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[0].Add(".   zr  zr  tb  tb  tb  tb  tb  tb  tb  tb  tb  tb  tb  .   ");
          textmap[0].Add(".   zr  zr  tb  tb  tb  tb  tb  tb  tb  tb  tb  tb  tb  .   ");
          textmap[0].Add(".   zr  zr  tb  tb  tb  tb  tb  tb  tb  tb  tb  tb  tb  .   ");
          textmap[0].Add(".   tb  tb  tb  tb  tb  tb  tb  tb  tb  tb  tb  tb  tb  .   ");
          textmap[0].Add(".   tb  tb  tb  tb  tb  tb  tb  tb  tb  tb  tb  tb  tb  .   ");
          textmap[0].Add(".   tb  tb  tb  tb  tb  tb  tb  tb  tb  tb  tb  tb  tb  .   ");
          textmap[0].Add(".   .   tb  tb  tb  tb  .   .   .   .   tb  tb  tb  tb  .   ");
          textmap[0].Add(".   .   tb  tb  tb  tb  .   .   .   .   .   .   tb  .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");

          textmap[1].Add("BR  BR  BR  BR  BR  BR  BR  BR  BR  BR  .   .   .   .   .   ");
          textmap[1].Add("BR  .   .   .   ,,  .   .   .   .   BE  BE  BE  BE  BE  BE  ");
          textmap[1].Add("BR  .   .   .   .   .   .   .   .   .   .   .   b   .   BE  ");
          textmap[1].Add("BR  .   .   BR  .   BE  .   b   .   BE  .   BE  .   .   BE  ");
          textmap[1].Add("BR  BR  BR  BR  .   ,,  .   BE  BE  .   .   .   .   ,,  BE  ");
          textmap[1].Add("BE  ,,  .   .   BE  .   BE  .   b   .   .   BE  BE  BE  BE  ");
          textmap[1].Add("BE  .   .   .   .   .   .   .   .   .   b   .   b   .   BE  ");
          textmap[1].Add("BE  BE  .   b   BE  DRV BE  BE  BE  BE  .   pn  p4  .   BE  ");
          textmap[1].Add(".   BE  .   .   BE  k   BE  .   .   BE  BE  BE  G4  BE  BE  ");
          textmap[1].Add(".   BE  BE  BE  BE  BE  BE  .   .   .   .   .   .   .   .   ");

          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   lR  .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   lR  .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   lR  .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          }

        // ICE ZONE MAIN

        // HOT ZONE MAIN

        // hot zone 1
        else if (player_level == 8)
          {
          //map_tile_width = 24;
          //map_tile_length = 19;
          //map_tile_height = 3;
          ambient_dark = .2f;
          ambient_light = new Color (64, 64, 64);
          map.background = test_background2;
          map.bg_scroll = true;
          random_retard1 = 0;
          random_retard2 = 2;

          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   fm  fm  .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   fm  fm  .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   fm  fm  .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   fm  fm  .   .   .   .   .   .   .   .   PSR .   .   ");
          textmap[0].Add(".   fm  fm  fm  .   .   .   .   .   fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  zy  .   ");
          textmap[0].Add(".   fm  fm  fm  fg  fg  fg  fg  fg  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  zy  .   ");
          textmap[0].Add(".   fm  fm  fm  .   .   .   .   .   fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  zy  .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   fg  .   fg  .   .   .   .   .   .   .   .   fg  .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   fg  fg  fg  fg  fg  fg  fg  fg  fg  fg  fg  fg  .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   fg  fg  .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   fg  .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   fm  fm  fm  .   .   .   .   .   .   .   fg  .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   fm  fm  fm  .   .   .   .   .   .   .   fg  .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   fm  fm  fm  .   .   .   .   .   .   .   fg  .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   .   fg  .   .   .   .   .   .   .   .   fg  .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   .   fg  .   .   .   .   .   .   .   fg  fg  .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   .   fg  fg  fg  fg  fg  fg  fg  fg  fg  fg  .   .   ");
          textmap[0].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");

          textmap[1].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   MK  MK  MK  MK  .   ");
          textmap[1].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   MK  .   .   XY  .   ");
          textmap[1].Add(".   .   .   .   MK  MK  MK  MK  MK  .   .   .   .   .   .   .   .   .   .   MK  .   .   MK  .   ");
          textmap[1].Add(".   .   .   .   MK  .   .   .   MK  .   MK  MK  MK  MK  .   .   .   .   .   MK  .   .   MK  .   ");
          textmap[1].Add("MK  MK  MK  MK  MK  .   .   .   MK  MK  MK  .   .   MK  MK  MK  MK  MK  MK  MK  MK  .   WY  WY  ");
          textmap[1].Add("MK  .   .   .   MK  .   .   .   MK  .   .   .   m   .   .   MK  .   ,,  .   .   .   .   .   WY  ");
          textmap[1].Add("MK  .   pn  .   .   .   .   .   .   .   .   .   m   m   .   DRH .   .   .   .   .   ,,  .   WY  ");
          textmap[1].Add("MK  .   .   .   MK  .   .   .   MK  .   .   .   .   .   .   MK  .   ,,  .   .   .   .   .   WY  ");
          textmap[1].Add("MK  MK  MK  MK  MK  .   .   .   MK  MK  .   MK  .   MK  MK  MK  MK  MK  MK  MK  MK  .   WY  WY  ");
          textmap[1].Add(".   .   MK  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   MK  .   ");
          textmap[1].Add(".   .   MK  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   MK  .   ");
          textmap[1].Add(".   .   MK  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   MK  .   ");
          textmap[1].Add(".   .   MK  .   .   .   .   .   .   .   .   .   c   ,,  .   .   .   .   .   .   .   .   MK  .   ");
          textmap[1].Add(".   .   MK  MK  .   .   .   .   .   .   .   c   h   c   .   .   .   .   .   .   .   .   MK  .   ");
          textmap[1].Add(".   .   .   MK  MK  .   .   .   .   .   .   .   c   .   .   .   .   .   .   .   .   .   MK  .   ");
          textmap[1].Add(".   .   .   .   MK  MK  .   .   .   .   .   MK  DY  MK  .   .   .   .   .   .   .   .   MK  .   ");
          textmap[1].Add(".   .   .   .   .   MK  MK  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   MK  .   ");
          textmap[1].Add(".   .   .   .   .   .   MK  MK  .   .   .   .   .   .   .   .   .   .   .   .   .   .   MK  .   ");
          textmap[1].Add(".   .   .   .   .   .   .   MK  MK  MK  MK  MK  MK  MK  MK  MK  MK  MK  MK  MK  MK  MK  MK  .   ");

          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   lY  .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   lR  .   lR  .   .   .   .   .   lR  .   lR  .   .   .   .   .   lY  .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   lR  .   lR  .   .   .   .   .   lR  .   lR  .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   lR  .   lR  .   lR  .   lR  .   lR  .   lR  .   lR  .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   lR  .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   lR  .   lR  .   .   .   .   .   .   .   lR  .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   lR  .   lR  .   .   .   .   .   .   .   lR  .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   lR  .   lR  .   lR  .   lR  .   lR  .   lR  .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          }

        else if (player_level == 9)
          {
          //map_tile_width = 11;
          //map_tile_length = 9;
          //map_tile_height = 3;
          ambient_dark = .3f;
          ambient_light = new Color (64, 64, 64);
          map.background = test_background4;
          map.bg_scroll = true;
          random_retard1 = 0;
          random_retard2 = 0;

          textmap[0].Add("ce  ce  ce  ce  ce  ce  ce  ce  ce  ce  ce  ");
          textmap[0].Add("ce  ce  ce  ce  ce  ce  ce  ce  ce  ce  ce  ");
          textmap[0].Add("ce  ce  ce  tk  tk  tk  tk  tk  ce  ce  ce  ");
          textmap[0].Add("ce  ce  tk  tk  tk  tk  tk  zy  tk  ce  ce  ");
          textmap[0].Add("ce  ce  tk  tk  tk  tk  tk  zy  tk  ce  ce  ");
          textmap[0].Add("ce  ce  tk  tk  tk  tk  tk  zy  tk  ce  ce  ");
          textmap[0].Add("ce  ce  ce  tk  tk  tk  tk  tk  ce  ce  ce  ");
          textmap[0].Add("ce  ce  ce  ce  ce  ce  ce  ce  ce  ce  ce  ");
          textmap[0].Add("ce  ce  ce  ce  ce  ce  ce  ce  ce  ce  ce  ");

          textmap[1].Add("MB  MB  MB  MB  MB  XY  MB  MB  MB  MB  MB  ");
          textmap[1].Add("MB  .   .   .   .   .   .   .   .   .   MB  ");
          textmap[1].Add("MB  pn  .   .   .   .   .   DY  .   .   MB  ");
          textmap[1].Add("MB  .    .  b   .   MB  .   .   DY  .   MB  ");
          textmap[1].Add("MB  .   MB  b   MB  .   .   .   DY  .   MB  ");
          textmap[1].Add("MB  .   .   .   .   .   .   .   DY  .   MB  ");
          textmap[1].Add("MB  .   .   MB  b   MB  .   DY  .   .   MB  ");
          textmap[1].Add("MB  .   .   .   .   .   .   .   .   .   MB  ");
          textmap[1].Add("MB  MB  MB  MB  MB  MB  MB  MB  MB  MB  MB  ");

          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   lw  .   lw  .   lw  .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   lw  .   lw  .   lw  .   lY  .   lw  .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   lw  .   lw  .   lw  .   lY  .   lw  .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   lw  .   lw  .   lw  .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   ");
          }

        // test map - environment
        else if (player_level == 10)
          {
          //map_tile_width = 13;
          //map_tile_length = 12;
          //map_tile_height = 3;
          ambient_dark = .2f;
          ambient_light = new Color (96, 96, 96);
          random_retard1 = 0;
          random_retard2 = 0;

          textmap[0].Add("fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  ");
          textmap[0].Add("fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  ");
          textmap[0].Add("fm  fm  fm  psy fm  fm  fm  fm  fm  fm  fm  fm  fm  ");
          textmap[0].Add("fm  fm  fm  tk  tk  tk  tk  tk  tk  tk  fm  fm  fm  ");
          textmap[0].Add("fm  fm  fm  psg tk  tk  tk  tk  tk  tk  fm  fm  fm  ");
          textmap[0].Add("fm  fm  fm  tk  tk  tk  tk  tk  tk  tk  fm  fm  fm  ");
          textmap[0].Add("fm  fm  fm  PSR tk  tk  tk  tk  tk  tk  Cs  Cw  fm  ");
          textmap[0].Add("fm  fm  fm  tk  tk  tk  tk  tk  tk  tk  Ce  Cn  fm  ");
          textmap[0].Add("fm  fm  fm  tk  tk  tk  tk  tk  tk  tk  fm  fm  fm  ");
          textmap[0].Add("fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  ");
          textmap[0].Add("fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  ");
          textmap[0].Add("fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  ");

          textmap[1].Add("MK  MK  MK  MK  MK  MK  G0  MK  DY  DY  DY  DY  MK  ");
          textmap[1].Add("MK  r1  DYH .   vm  .   .   ID  .   .   fc  t1  MK  ");
          textmap[1].Add("MK  DYV MK  .   .   .   .   .   .   m   .   .   MK  ");
          textmap[1].Add("MK  .   DGH .   .   .   .   .   .   .   i   .   MK  ");
          textmap[1].Add("MK  DGV MK  .   .   pn  .   MK  .   i   i   .   MK  ");
          textmap[1].Add("MK  .   DRH .   .   .   .   .   .   .   .   .   MK  ");
          textmap[1].Add("MK  DRV MK  .   .   .   .   .   .   b   b   .   MK  ");
          textmap[1].Add("MK  .   .   .   .   pl1 .   .   .   .   .   .   MK  ");
          textmap[1].Add("MK  .   .   .   .   .   .   .   .   .   .   .   MK  ");
          textmap[1].Add("MK  k   h   c   .   .   .   sp  sf  si  se  sm  MK  ");
          textmap[1].Add("MK  e   sc  .   .   .   .   .   .   .   .   .   MK  ");
          textmap[1].Add("MK  MK  MK  MK  MK  MK  MK  MK  MK  MK  MK  MK  MK  ");

          textmap[2].Add(".   .   .   .   .   WK  WK  WK  .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   lw  .   lw  .   lw  .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   lw  .   lw  .   lw  .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   lw  .   lw  .   lw  .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   ");
          }

        // test map - AI
        else if (player_level == 11)
          {
          //map_tile_width = 9;
          //map_tile_length = 9;
          //map_tile_height = 3;
          ambient_dark = .4f;
          ambient_light = new Color (72, 72, 72);
          random_retard1 = 0;
          random_retard2 = 0;

                    textmap[0].Add("tk  tk  tk  tk  tk  tk  tk  tk  tk  ");
                    textmap[0].Add("tk  tk  tk  tk  tk  tk  tk  tk  tk  ");
                    textmap[0].Add("tk  tk  tk  tk  tk  tk  tk  tk  tk  ");
                    textmap[0].Add("tk  tk  tk  tk  tk  tk  tk  tk  tk  ");
                    textmap[0].Add("tk  tk  tk  tk  tk  tk  tk  tk  tk  ");
                    textmap[0].Add("tk  tk  zg  tk  tk  tk  tk  tk  tk  ");
                    textmap[0].Add("tk  tk  tk  tk  tk  tk  tk  tk  tk  ");
                    textmap[0].Add("tk  tk  tk  tk  tk  tk  tk  tk  tk  ");
                    textmap[0].Add("tk  tk  tk  tk  tk  tk  tk  tk  tk  ");

                    textmap[1].Add("MM  MM  MM  MM  MM  MM  MM  MM  MM  ");
                    textmap[1].Add("MM  .   .   .   MM  r2  .   .   MM  ");
                    textmap[1].Add("MM  .   MM  .   MM  MM  MM  .   MM  ");
                    textmap[1].Add("MM  .   MM  .   .   .   MM  .   MM  ");
                    textmap[1].Add("MM  .   MM  .   MM  .   MM  r1  MM  ");
                    textmap[1].Add("MM  .   .   .   MM  .   MM  .   MM  ");
                    textmap[1].Add("MM  pn  b   .   MM  .   .   .   MM  ");
                    textmap[1].Add("MM  p0  .   .   MM  .   MM  .   MM  ");
                    textmap[1].Add("MM  MM  MM  MM  MM  MM  MM  MM  MM  ");

                    textmap[2].Add(".   .   .   .   .   .   .   .   .   ");
                    textmap[2].Add(".   .   .   .   .   .   .   .   .   ");
                    textmap[2].Add(".   .   lw  .   lw  .   lw  .   .   ");
                    textmap[2].Add(".   .   .   .   .   .   .   .   .   ");
                    textmap[2].Add(".   .   lw  .   lw  .   lw  .   .   ");
                    textmap[2].Add(".   .   .   .   .   .   .   .   .   ");
                    textmap[2].Add(".   .   lw  .   lw  .   lw  .   .   ");
                    textmap[2].Add(".   .   .   .   .   .   .   .   .   ");
                    textmap[2].Add(".   .   .   .   .   .   .   .   .   ");
          }

        // test map - rube goldberg
        else if (player_level == 12)
          {
          //map_tile_width = 14;
          //map_tile_length = 14;
          //map_tile_height = 3;
          ambient_dark = .4f;
          ambient_light = new Color (80, 80, 80);
          random_retard1 = 0;
          random_retard2 = 0;

          textmap[0].Add("tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  ");
          textmap[0].Add("tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  ");
          textmap[0].Add("tk  tk  tk  tk  tk  tk  tk  tk  Cn  tk  tk  tk  tk  tk  ");
          textmap[0].Add("tk  tk  tk  tk  tk  Cs  tk  Ce  Cn  tk  tk  tk  tk  tk  ");
          textmap[0].Add("tk  tk  tk  tk  tk  Cs  tk  Cn  tk  tk  tk  tk  tk  tk  ");
          textmap[0].Add("tk  tk  tk  tk  tk  tk  tk  Cn  Cw  Cw  tk  tk  tk  tk  ");
          textmap[0].Add("tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  ");
          textmap[0].Add("tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  ");
          textmap[0].Add("tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  ");
          textmap[0].Add("tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  ");
          textmap[0].Add("tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  ");
          textmap[0].Add("tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  ");
          textmap[0].Add("tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  ");
          textmap[0].Add("tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  tk  ");

          textmap[1].Add("BR  BR  BR  BR  BR  BR  XR  BR  BR  BR  BR  BR  BR  BR  ");
          textmap[1].Add("BR  .   .   .   .   pn  p0  .   .   .   .   .   .   BR  ");
          textmap[1].Add("BR  .   WE  WE  WE  .   WE  WE  .   WE  WE  WE  .   BR  ");
          textmap[1].Add("BR  .   WE  .   .   bb  .   .   .   .   .   WE  .   BR  ");
          textmap[1].Add("BR  .   WE  .   wi  lhg .   .   WE  .   .   WE  .   BR  ");
          textmap[1].Add("BR  .   WE  .   wi  .   .   .   .   .   .   WE  .   BR  ");
          textmap[1].Add("BR  .   WE  WE  wi  WE  .   WE  .   .   .   WE  .   BR  ");
          textmap[1].Add("BR  .   WE  .   DGH .   .   .   .   .   .   WE  .   BR  ");
          textmap[1].Add("BR  .   WE  WE  WE  .   .   WE  i   .   .   WE  .   BR  ");
          textmap[1].Add("BR  .   WE  .   .   .   .   .   .   .   .   WE  .   BR  ");
          textmap[1].Add("BR  .   WE  .   .   .   .   .   .   .   .   WE  .   BR  ");
          textmap[1].Add("BR  .   WE  WE  WE  WE  WE  WE  WE  WE  WE  WE  .   BR  ");
          textmap[1].Add("BR  .   .   .   .   .   .   .   .   .   .   .   .   BR  ");
          textmap[1].Add("BR  BR  BR  BR  BR  BR  BR  BR  BR  BR  BR  BR  BR  BR  ");

          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   lW  .   lW  .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   lW  .   lW  .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   lW  .   lW  .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          textmap[2].Add(".   .   .   .   .   .   .   .   .   .   .   .   .   .   ");
          }

        // test map - web page
        else if (player_level == 13)
          {
          //map_tile_width = 13;
          //map_tile_length = 15;
          //map_tile_height = 3;
          ambient_dark = .4f;
          ambient_light = new Color (96, 96, 96);
          random_retard1 = 0;
          random_retard2 = 0;

                    textmap[0].Add("..  ..  ..  ..  ..  ..  ..  fm  fm  fm  fm  fm  fm  ");
                    textmap[0].Add("fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  ");
                    textmap[0].Add("fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  ");
                    textmap[0].Add("fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  fm  ");
                    textmap[0].Add("ce  ce  ce  ce  log00  00  00  ce  ce  ce  ce  fm  ");
                    textmap[0].Add("ce  ce  ce  ce  00  00  00  00  ce  ce  ce  ce  fm  ");
                    textmap[0].Add("ce  ce  ce  ce  tk  tk  tk  tk  ce  ce  ce  ce  fm  ");
                    textmap[0].Add("ce  ce  ce  ce  tk  tk  tk  tk  ce  ce  ce  ce  fm  ");
                    textmap[0].Add("ce  ce  ce  ce  tk  tk  tk  tk  ce  ce  ce  ce  fm  ");
                    textmap[0].Add("ce  ce  ce  ce  tk  tk  tk  tk  ce  ce  ce  ce  fm  ");
                    textmap[0].Add("ce  ce  ce  ce  tk  tk  tk  tk  ce  ce  ce  ce  fm  ");
                    textmap[0].Add("ce  ce  ce  ce  tk  tk  tk  tk  ce  ce  ce  ce  fm  ");
                    textmap[0].Add("ce  ce  ce  ce  tk  tk  tk  tk  ce  ce  ce  ce  fm  ");
                    textmap[0].Add("ce  ce  ce  ce  tk  tk  tk  tk  ce  ce  ce  ce  fm  ");
                    textmap[0].Add("..  ..  ..  ..  ..  ..  ..  ..  fm  fm  fm  fm  fm  ");

                    textmap[1].Add("..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ");
                    textmap[1].Add("DP  DP  DP  DP  DP  DP  DP  ..  DP  DP  DP  DP  ..  ");
                    textmap[1].Add("DP  tp  ..  b   p0  pn  ..  b   ..  m   vm  DP  ..  ");
                    textmap[1].Add("DP  DP  DP  DP  DP  DP  DP  DP  DP  DP  DP  DP  ..  ");
                    textmap[1].Add("DP  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  DP  ..  ");
                    textmap[1].Add("DP  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  DP  ..  ");
                    textmap[1].Add("DP  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  DP  ..  ");
                    textmap[1].Add("DP  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  DP  ..  ");
                    textmap[1].Add("DP  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  DP  ..  ");
                    textmap[1].Add("DP  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  DP  ..  ");
                    textmap[1].Add("DP  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  DP  ..  ");
                    textmap[1].Add("DP  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  DP  ..  ");
                    textmap[1].Add("DP  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  DP  ..  ");
                    textmap[1].Add("DP  DP  DP  DP  DP  DP  DP  DP  DP  DP  b   DP  ..  ");
                    textmap[1].Add("..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ");

                    textmap[2].Add("..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ");
                    textmap[2].Add("..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ");
                    textmap[2].Add("..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ");
                    textmap[2].Add("..  ..  ..  ..  ..  lw  ..  lw  ..  ..  ..  ..  ..  ");
                    textmap[2].Add("..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ");
                    textmap[2].Add("..  ..  ..  ..  ..  lw  ..  lw  ..  ..  ..  ..  ..  ");
                    textmap[2].Add("..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ");
                    textmap[2].Add("..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ");
                    textmap[2].Add("..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ");
                    textmap[2].Add("..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ");
                    textmap[2].Add("..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ");
                    textmap[2].Add("..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ");
                    textmap[2].Add("..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ");
                    textmap[2].Add("..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ");
                    textmap[2].Add("..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ..  ");
          }

        // test map - rube goldberg in feature order
        // 1 - pressure switch opens door
        // 2 - box behind door travels down conveyor belt
        // 3 - box is torched by incinerator
        // 4 - box sets second box on fire
        else if (player_level == 14)
          {
          ambient_dark = .4f;
          ambient_light = new Color (96, 96, 96);

          textmap[0].Add (".   .   .   ");
          textmap[0].Add (".   tk  .   ");
          textmap[0].Add (".   .   .   ");

          textmap[1].Add ("BY  BY  BY  ");
          textmap[1].Add ("BY  pn  BY  ");
          textmap[1].Add ("BY  BY  BY  ");

          textmap[2].Add (".   .   .   ");
          textmap[2].Add (".   .   .   ");
          textmap[2].Add (".   .   .   ");
          }

        map_tile_width = textmap[0][0].Length / 4;
        map_tile_length = textmap[0].Count;
        map_tile_height = textmap.Count;

        map_char_width = map_tile_width * 4;

        for (mz = 0; mz < map_tile_height; mz += 1)
          for (my = 0; my < map_tile_length; my += 1)
            for (mx = 0; mx < map_tile_width; mx += 1)
              matrixmap[mx, my, mz] = textmap[mz][my].Substring (mx * 4, 3);
        }

      construct_world ();

      fader.reset ();
      fadein ();
      }

    ////////////////////////////////////////////////////////////////////////////////

    void construct_world ()  // build world from matrix or reset current map
      {
      string gridspace;
      int mx, my, mz;
      int x, y, z;
      int light_number = 0;
      float light_alpha = 0f;
      Color light_color;
      //int light_type = 0;
      //string space;
      string gate;
      //int offset_x, offset_y;
      int b, f, f2, f3, r;
      bool electric_north, electric_south, electric_east, electric_west;
      int spot;

      // clear level data
      //total_brushes = 0;
      brush_control.brush = new List<Brush> ();
      fixture_control.fixture = new List<Fixture> ();
      object_control.obj = new List<Object> ();
      total_lights = 0;
      total_draw_slots = 1;
      total_character_spots = 0;

      while (character_control.character.Count > 1)
        {
        character_control.character.RemoveAt (1);
        }

      /*
      // map reader (text file)
      filestream = TitleContainer.OpenStream (levelPath);
      using (StreamReader reader = new StreamReader (filestream))
        {
        String[] line = new String[map_tile_max_length];
        l = 0;

        // read the file into a string array
        while (l < map_tile_max_length && (line[l] = reader.ReadLine()) != null) l ++;
        bool success = true;

        //if (success == true && string_is_int (line[0])) map_tile_width = Convert.ToInt16 (line[0]);
        //else success = false;

        //if (success == true && string_is_int (line[1])) map_tile_length = Convert.ToInt16 (line[1]);
        //else success = false;

        //if (success == true && string_is_int (line[2])) map_tile_height = Convert.ToInt16 (line[2]);
        //else success = false;

        //if (success == true && string_is_float (line[3])) ambient_light = Convert.ToInt16 (line[3]);
        //else success = false;

        map_tile_width = 37;
        map_tile_length = 1;// 14;
        map_tile_height = 3;
        ambient_light = -.4f;
        
        int mx = 0, my = 0, mz = 0;
        for (l = 5; l < 5 + map_tile_length; l += 1)
          {
          if (line[l].Length >= map_tile_width) textmap[my, mz] = line[l];
          my += 1;
          }
        }
      */

      player_added_to_map = false;

      //matrixmap[4, 4, 1] = "pl1";

      // map reader
      x = 0; y = map_tile_length * tilesize; z = 0;

      for (mz = 0; mz < map_tile_height; mz += 1)
        {
        for (my = 0; my < map_tile_length; my += 1)
          {
          for (mx = 0; mx < map_tile_width; mx += 1)
            {
            gridspace = matrixmap[mx, my, mz];

            if (gridspace == ",, ")// && total_character_spots < max_character_spots)
              {
              random_character_spot[total_character_spots].x = x;
              random_character_spot[total_character_spots].y = y;
              random_character_spot[total_character_spots].z = z;
              random_character_spot[total_character_spots].used = false;
              total_character_spots += 1;
              }
            else if (gridspace == "00 ") add_brush ((int) Brush_Control.T.INVISIBLE_WALL, (int) Brush_Control.T.INVISIBLE_WALL, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "BE ") add_brush ((int) Brush_Control.T.BRICK_GREY_TEST, (int) Brush_Control.T.BRICK_GREY_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "BR ") add_brush ((int) Brush_Control.T.BRICK_RED_TEST, (int) Brush_Control.T.BRICK_RED_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "BW ") add_brush ((int) Brush_Control.T.BRICK_WHITE_TEST, (int) Brush_Control.T.BRICK_WHITE_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "DRV") add_brush ((int) Brush_Control.T.DOOR_RED_V_TOP_CLOSED_TEST, (int) Brush_Control.T.DOOR_RED_V_FRONT_CLOSED_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "DRH") add_brush ((int) Brush_Control.T.DOOR_RED_H_TOP_CLOSED_TEST, (int) Brush_Control.T.DOOR_RED_H_FRONT_CLOSED_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "DYV") add_brush ((int) Brush_Control.T.DOOR_YELLOW_V_TOP_CLOSED_TEST, (int) Brush_Control.T.DOOR_YELLOW_V_FRONT_CLOSED_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "DYH") add_brush ((int) Brush_Control.T.DOOR_YELLOW_H_TOP_CLOSED_TEST, (int) Brush_Control.T.DOOR_YELLOW_H_FRONT_CLOSED_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "DGV") add_brush ((int) Brush_Control.T.DOOR_GREEN_V_TOP_CLOSED_TEST, (int) Brush_Control.T.DOOR_GREEN_V_FRONT_CLOSED_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "DGH") add_brush ((int) Brush_Control.T.DOOR_GREEN_H_TOP_CLOSED_TEST, (int) Brush_Control.T.DOOR_GREEN_H_FRONT_CLOSED_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "DBV") add_brush ((int) Brush_Control.T.DOOR_BLUE_V_TOP_CLOSED_TEST, (int) Brush_Control.T.DOOR_BLUE_V_FRONT_CLOSED_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "DBH") add_brush ((int) Brush_Control.T.DOOR_BLUE_H_TOP_CLOSED_TEST, (int) Brush_Control.T.DOOR_BLUE_H_FRONT_CLOSED_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "DM ") add_brush ((int) Brush_Control.T.DRYWALL_MINT_TOP_TEST, (int) Brush_Control.T.DRYWALL_MINT_FRONT_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "DP ") add_brush ((int) Brush_Control.T.DRYWALL_PURPLE_TOP_TEST, (int) Brush_Control.T.DRYWALL_PURPLE_FRONT_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "DT ") add_brush ((int) Brush_Control.T.DRYWALL_TAN_TOP_TEST, (int) Brush_Control.T.DRYWALL_TAN_FRONT_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "DY ") add_brush ((int) Brush_Control.T.DRYWALL_YELLOW_TOP_TEST, (int) Brush_Control.T.DRYWALL_YELLOW_FRONT_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if ((gridspace[0] == 'G' || gridspace[0] == 'g')  // gateway to new map
                     && (gridspace[1] == '0' || gridspace[1] == '1' || gridspace[1] == '2' || gridspace[1] == '3' || gridspace[1] == '4'
                     || gridspace[1] == '5' || gridspace[1] == '6' || gridspace[1] == '7' || gridspace[1] == '8' || gridspace[1] == '9'))
              {
              if (gridspace[0] == 'G') add_brush ((int) Brush_Control.T.GATEWAY_V_TOP_TEST, (int) Brush_Control.T.GATEWAY_V_FRONT_OPEN_TEST, x, y, z, tilesize, tilesize, tilesize);
              else if (gridspace[0] == 'g') add_brush ((int) Brush_Control.T.GATEWAY_H_TOP_TEST, (int) Brush_Control.T.GATEWAY_H_FRONT_TEST, x, y, z, tilesize, tilesize, tilesize);

              //gate = gridspace.Substring (1, 2);
              brush_control.brush[brush_control.brush.Count - 1].gateway = Convert.ToInt16 (gridspace.Substring (1, 2));
              }
            else if (gridspace == "IU ") add_brush ((int) Brush_Control.T.INCINERATOR_TEST_UP, (int) Brush_Control.T.METAL_BLACK_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "ID ") add_brush ((int) Brush_Control.T.INCINERATOR_TEST_DOWN, (int) Brush_Control.T.INCINERATOR_TEST_DOWN_FRONT, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "IL ") add_brush ((int) Brush_Control.T.INCINERATOR_TEST_LEFT, (int) Brush_Control.T.METAL_BLACK_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "IR ") add_brush ((int) Brush_Control.T.INCINERATOR_TEST_RIGHT, (int) Brush_Control.T.METAL_BLACK_TEST, x, y, z, tilesize, tilesize, tilesize);
            //else if (gridspace == "W1 ") add_brush ((int) Brush_Control.T.WARNING_SIGN_TEST1, (int) Brush_Control.T.WARNING_SIGN_TEST1, x, y, z, tilesize, tilesize, tilesize);
            //else if (gridspace == "W2 ") add_brush ((int) Brush_Control.T.WARNING_SIGN_TEST2, (int) Brush_Control.T.WARNING_SIGN_TEST2, x, y, z, tilesize, tilesize, tilesize);
            //else if (gridspace == "W3 ") add_brush ((int) Brush_Control.T.WARNING_SIGN_TEST3, (int) Brush_Control.T.WARNING_SIGN_TEST3, x, y, z, tilesize, tilesize, tilesize);
            //else if (gridspace == "W4 ") add_brush ((int) Brush_Control.T.WARNING_SIGN_TEST4, (int) Brush_Control.T.WARNING_SIGN_TEST4, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "MB ") add_brush ((int) Brush_Control.T.METAL_BLUE_TOP_TEST, (int) Brush_Control.T.METAL_BLUE_FRONT_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "MK ") add_brush ((int) Brush_Control.T.METAL_BLACK_TEST, (int) Brush_Control.T.METAL_BLACK_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "MM ") add_brush ((int) Brush_Control.T.METAL_MINT_TOP_TEST, (int) Brush_Control.T.METAL_MINT_FRONT_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "BM ") add_brush ((int) Brush_Control.T.BIG_MACHINE_TEST, (int) Brush_Control.T.BIG_MACHINE_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "EN ") add_brush ((int) Brush_Control.T.GATEWAY_V_TOP_TEST, (int) Brush_Control.T.GATEWAY_V_FRONT_CLOSED_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "En ") add_brush ((int) Brush_Control.T.GATEWAY_H_TOP_TEST, (int) Brush_Control.T.GATEWAY_H_FRONT_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "PSB") add_brush ((int) Brush_Control.T.SWITCH_BLUE_TEST, (int) Brush_Control.T.SWITCH_BLUE_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "psg") add_brush ((int) Brush_Control.T.SWITCH_GREEN_TEST, (int) Brush_Control.T.SWITCH_GREEN_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "PSR") add_brush ((int) Brush_Control.T.SWITCH_RED_TEST, (int) Brush_Control.T.SWITCH_RED_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "psy") add_brush ((int) Brush_Control.T.SWITCH_YELLOW_TEST, (int) Brush_Control.T.SWITCH_YELLOW_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "XR ") add_brush ((int) Brush_Control.T.EXIT_RED_V_TOP_CLOSED_TEST, (int) Brush_Control.T.EXIT_RED_V_FRONT_CLOSED_TEST, x, y, z, tilesize, tilesize, tilesize);  // red vertical exit
            else if (gridspace == "Xr ") add_brush ((int) Brush_Control.T.EXIT_RED_H_TOP_CLOSED_TEST, (int) Brush_Control.T.EXIT_RED_H_FRONT_CLOSED_TEST, x, y, z, tilesize, tilesize, tilesize);  // red horizontal exit
            else if (gridspace == "Cn ") add_fixture ((int) Fixture_Control.F.CONVEYOR_NORTH_TEST, x, y, z);
            else if (gridspace == "Cs ") add_fixture ((int) Fixture_Control.F.CONVEYOR_SOUTH_TEST, x, y, z);
            else if (gridspace == "Ce ") add_fixture ((int) Fixture_Control.F.CONVEYOR_EAST_TEST, x, y, z);
            else if (gridspace == "Cw ") add_fixture ((int) Fixture_Control.F.CONVEYOR_WEST_TEST, x, y, z);
            else if (gridspace == "as ") add_brush ((int) Brush_Control.T.ASPHALT_TEST, (int) Brush_Control.T.ASPHALT_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "b  ") add_brush ((int) Brush_Control.T.BOX_WOOD_TEST, (int) Brush_Control.T.BOX_WOOD_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "bb ") add_brush ((int) Brush_Control.T.BOX_BANDED_TEST, (int) Brush_Control.T.BOX_BANDED_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "c  ") add_object ((int) Object_Control.O.COIN, x + (tilesize / 2), y + (tilesize / 2), z + (tilesize / 2));
            else if (gridspace == "c1s") add_fixture ((int) Fixture_Control.F.CHAIR1_SOUTH_TEST, x, y, z);
            else if (gridspace == "ce ") add_brush ((int) Brush_Control.T.CARPET_GREY_TEST, (int) Brush_Control.T.CARPET_GREY_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "co ") add_fixture ((int) Fixture_Control.F.COUCH_TEST, x, y, z);
            else if (gridspace == "cp ") add_brush ((int) Brush_Control.T.CARPET_PURPLE_TEST, (int) Brush_Control.T.CARPET_PURPLE_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "e  ") add_object ((int) Object_Control.O.ENERGY, x + (tilesize / 2), y + (tilesize / 2), z + (tilesize / 2));
            else if (gridspace == "i  ") add_brush ((int) Brush_Control.T.BOX_ICE_TEST, (int) Brush_Control.T.BOX_ICE_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "fc ") add_fixture ((int) Fixture_Control.F.FILING_TEST, x, y, z);
            else if (gridspace == "fg ") add_brush ((int) Brush_Control.T.FLOOR_GRATE_TEST, (int) Brush_Control.T.FLOOR_GRATE_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "fm ") add_brush ((int) Brush_Control.T.FLOOR_METAL_TEST, (int) Brush_Control.T.FLOOR_METAL_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "gr ") add_brush ((int) Brush_Control.T.GRASS, (int) Brush_Control.T.GRASS, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "h  ") add_object ((int) Object_Control.O.HEALTH, x + (tilesize / 2), y + (tilesize / 2), z + (tilesize / 2));
            else if (gridspace == "k  ") add_object ((int) Object_Control.O.KEYCARD, x + (tilesize / 2), y + (tilesize / 2), z + (tilesize / 2));
            else if (gridspace == "lhg") add_fixture ((int) Fixture_Control.F.LASER_HORIZONTAL_GREEN_TEST, x, y, z);
            else if (gridspace == "log") add_brush ((int) Brush_Control.T.FLOOR_LOGO_TEST, (int) Brush_Control.T.FLOOR_LOGO_TEST, x, y, z, tilesize, tilesize, tilesize);
            if (lighting_engine == 2)
              {
              if (gridspace == "lB ") add_light (Color.Blue, x + (tilesize / 2), y + (tilesize / 2), z, 2f, .8f, SOLID);
              else if (gridspace == "lF ") add_light (Color.Purple, x + (tilesize / 2), y + (tilesize / 2), z, 2f, .8f, SOLID);
              else if (gridspace == "lG ") add_light (Color.Green, x + (tilesize / 2), y + (tilesize / 2), z, 2f, .8f, SOLID);
              else if (gridspace == "lR ") add_light (Color.Red, x + (tilesize / 2), y + (tilesize / 2), z, 2f, .8f, SOLID);
              else if (gridspace == "lY ") add_light (Color.Yellow, x + (tilesize / 2), y + (tilesize / 2), z, 2f, .8f, SOLID);
              else if (gridspace == "lW ") add_light (Color.White, x + (tilesize / 2), y + (tilesize / 2), z, 2f, .5f, SOLID);
              else if (gridspace == "lb ") add_light (new Color (128, 128, 255), x + (tilesize / 2), y + (tilesize / 2), z, 2f, .8f, SOLID);
              else if (gridspace == "lw ") add_light (Color.White, x + (tilesize / 2), y + (tilesize / 2), z, 2f, .5f, SOLID);
              else if (gridspace == "ly ") add_light (new Color (255, 255, 128), x + (tilesize / 2), y + (tilesize / 2), z, 2f, .5f, SOLID);
              }
            else
              {
              if (gridspace == "lB ") add_light ((int) L.blue, x + (tilesize / 2), y + (tilesize / 2), z, 2f, .2f, SOLID);
              else if (gridspace == "lb ") add_light ((int) L.blue_pale, x + (tilesize / 2), y + (tilesize / 2), z, 1.5f, .2f, SOLID);
              else if (gridspace == "lF ") add_light ((int) L.fushia, x + (tilesize / 2), y + (tilesize / 2), z, 2f, .15f, SOLID);
              else if (gridspace == "lG ") add_light ((int) L.green, x + (tilesize / 2), y + (tilesize / 2), z, 2f, .15f, SOLID);
              else if (gridspace == "lR ") add_light ((int) L.red, x + (tilesize / 2), y + (tilesize / 2), z, 2f, .15f, SOLID);
              else if (gridspace == "lW ") add_light ((int) L.white, x + (tilesize / 2), y + (tilesize / 2), z, 1f, .1f, SOLID);
              else if (gridspace == "lw ") add_light ((int) L.white, x + (tilesize / 2), y + (tilesize / 2), z, 1.5f, 0f, SOLID);
              else if (gridspace == "lw ") add_light ((int) L.white, x + (tilesize / 2), y + (tilesize / 2), z, 1.5f, .2f, SOLID);
              else if (gridspace == "lY ") add_light ((int) L.yellow, x + (tilesize / 2), y + (tilesize / 2), z, 2f, .15f, SOLID);
              else if (gridspace == "ly ") add_light ((int) L.yellow_pale, x + (tilesize / 2), y + (tilesize / 2), z, 1.5f, .1f, SOLID);
              }
            if (gridspace == "m  ") add_brush ((int) Brush_Control.T.BOX_METAL_TEST, (int) Brush_Control.T.BOX_METAL_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "pl1") add_fixture ((int) Fixture_Control.F.PLANT1_TEST, x, y, z);
            //else
            if (gridspace != null && gridspace[0] == 'p' && gridspace.Length > 1)
              {
              int level_tag;
              if (gridspace[1] != 'n'
                //&& player_last_level == Convert.ToInt16 (gridspace.Substring (1, 2)))
                && int.TryParse(gridspace.Substring (1, 2), out level_tag) && player_last_level == level_tag)
                add_player (x, y, z);
              else if (gridspace[1] == 'n' && player_last_level == -1) add_player (x, y, z);
              }
            else if (gridspace == "r1 " && toggle_enemies == true) add_character ("retard", (int) Character_Control.C.RETARD, x + (tilesize / 2), y + (tilesize / 2), z);
            else if (gridspace == "r2 " && toggle_enemies == true) add_character ("throwing retard", (int) Character_Control.C.THROWING_RETARD, x + (tilesize / 2), y + (tilesize / 2), z);
            else if (gridspace == "rd " && toggle_enemies == true) add_character ("Richard's Dad", (int) Character_Control.C.RICHARDS_DAD, x + (tilesize / 2), y + (tilesize / 2), z);
            else if (gridspace == "sc ") add_object ((int) Object_Control.O.SCRAP_METAL, x + (tilesize / 2), y + (tilesize / 2), z + (tilesize / 2));
            else if (gridspace == "sw ") add_brush ((int) Brush_Control.T.SIDEWALK_TEST, (int) Brush_Control.T.SIDEWALK_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "se ") add_object ((int) Object_Control.O.SHIRT_BLUE, x + (tilesize / 2), y + (tilesize / 2), z + (tilesize / 2));
            else if (gridspace == "sf ") add_object ((int) Object_Control.O.SHIRT_RED, x + (tilesize / 2), y + (tilesize / 2), z + (tilesize / 2));
            else if (gridspace == "si ") add_object ((int) Object_Control.O.SHIRT_WHITE, x + (tilesize / 2), y + (tilesize / 2), z + (tilesize / 2));
            else if (gridspace == "sm ") add_object ((int) Object_Control.O.SHIRT_PURPLE, x + (tilesize / 2), y + (tilesize / 2), z + (tilesize / 2));
            else if (gridspace == "sy ") add_object ((int) Object_Control.O.SHIRT_YELLOW, x + (tilesize / 2), y + (tilesize / 2), z + (tilesize / 2));
            else if (gridspace == "t1 ") add_fixture ((int) Fixture_Control.F.TABLE1_TEST, x, y, z);
            else if (gridspace == "t2 ") add_fixture ((int) Fixture_Control.F.TABLE2_TEST, x, y, z);
            else if (gridspace == "tb ") add_brush ((int) Brush_Control.T.TILE_BLUE_TEST, (int) Brush_Control.T.TILE_BLUE_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "tk ") add_brush ((int) Brush_Control.T.TILE_BLACK_TEST, (int) Brush_Control.T.TILE_BLACK_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "tn ") add_brush ((int) Brush_Control.T.TILE_BROWN_TEST, (int) Brush_Control.T.TILE_BROWN_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "tv ") add_fixture ((int) Fixture_Control.F.TV1_TEST, x, y, z);
            else if (gridspace == "vm ") add_fixture ((int) Fixture_Control.F.VENDING_TEST, x, y, z);
            else if (gridspace == "wi ") add_fixture ((int) Fixture_Control.F.WIRES, x, y, z);
            else if (gridspace == "zg ") add_brush ((int) Brush_Control.T.FLOOR_ZONE_GREEN_TEST, (int) Brush_Control.T.FLOOR_ZONE_GREEN_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "zr ") add_brush ((int) Brush_Control.T.FLOOR_ZONE_RED_TEST, (int) Brush_Control.T.FLOOR_ZONE_RED_TEST, x, y, z, tilesize, tilesize, tilesize);
            else if (gridspace == "zy ") add_brush ((int) Brush_Control.T.FLOOR_ZONE_YELLOW_TEST, (int) Brush_Control.T.FLOOR_ZONE_YELLOW_TEST, x, y, z, tilesize, tilesize, tilesize);
            x += tilesize;
            }
          x = 0;
          y -= tilesize;
          }
        x = 0;
        y = map_tile_length * tilesize;
        z += tilesize;
        }

      map_width = tilesize * map_tile_width;
      map_length = tilesize * map_tile_length;
      map_height = tilesize * map_tile_height;

      // add background textures to brushes that need them
      for (b = 0; b < brush_control.brush.Count; b += 1)
        {
        if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.SWITCH_YELLOW_TEST ||
            brush_control.brush[b].top_texture_number == (int) Brush_Control.T.SWITCH_RED_TEST ||
            brush_control.brush[b].top_texture_number == (int) Brush_Control.T.SWITCH_GREEN_TEST ||
            brush_control.brush[b].top_texture_number == (int) Brush_Control.T.SWITCH_BLUE_TEST)
          {
          int clip = brush_control.brush_north_of_brush (brush_control.brush[b]);
          if (clip == -1) clip = brush_control.brush_west_of_brush (brush_control.brush[b]);
          if (clip == -1) clip = brush_control.brush_east_of_brush (brush_control.brush[b]);
          if (clip == -1) clip = brush_control.brush_south_of_brush (brush_control.brush[b]);
          if (clip > -1) brush_control.brush[b].background_texture = brush_control.brush[clip].top_texture_number;
          else brush_control.brush[b].background_texture = (int) Brush_Control.T.TILE_BLACK_TEST;
          }
        }

      // reassign generic wires and pipes with facing directions based on surroundings
      for (f = 0; f < fixture_control.fixture.Count; f += 1)
        {
        if (fixture_control.fixture[f].type == (int) Fixture_Control.F.WIRES)
          {
          b = brush_control.point_in_brush (fixture_control.fixture[f].x + (tilesize / 2), fixture_control.fixture[f].y + Convert.ToInt32 (tilesize * 1.5), fixture_control.fixture[f].z + 9, false, false);
          f2 = fixture_control.point_collide (fixture_control.fixture[f].x + (tilesize / 2), fixture_control.fixture[f].y + Convert.ToInt32 (tilesize * 1.5), fixture_control.fixture[f].z + 1);
          f3 = fixture_control.point_collide (fixture_control.fixture[f].x + (tilesize / 2), fixture_control.fixture[f].y + Convert.ToInt32 (tilesize * 1.5), fixture_control.fixture[f].z + 9);
          if (b > -1 && brush_control.brush[b].electric == true) electric_north = true;
          else if (f2 > -1 && fixture_control.fixture[f2].electric == true) electric_north = true;
          else if (f3 > -1 && fixture_control.fixture[f3].electric == true) electric_north = true;
          else electric_north = false;

          b = brush_control.point_in_brush (fixture_control.fixture[f].x + (tilesize / 2), fixture_control.fixture[f].y - (tilesize / 2), fixture_control.fixture[f].z + 9, false, false);
          f2 = fixture_control.point_collide (fixture_control.fixture[f].x + (tilesize / 2), fixture_control.fixture[f].y - (tilesize / 2), fixture_control.fixture[f].z + 1);
          f3 = fixture_control.point_collide (fixture_control.fixture[f].x + (tilesize / 2), fixture_control.fixture[f].y - (tilesize / 2), fixture_control.fixture[f].z + 9);
          if (b > -1 && brush_control.brush[b].electric == true) electric_south = true;
          else if (f2 > -1 && fixture_control.fixture[f2].electric == true) electric_south = true;
          else if (f3 > -1 && fixture_control.fixture[f3].electric == true) electric_south = true;
          else electric_south = false;

          b = brush_control.point_in_brush (fixture_control.fixture[f].x - (tilesize / 2), fixture_control.fixture[f].y + (tilesize / 2), fixture_control.fixture[f].z + 9, false, false);
          f2 = fixture_control.point_collide (fixture_control.fixture[f].x - (tilesize / 2), fixture_control.fixture[f].y + (tilesize / 2), fixture_control.fixture[f].z + 1);
          f3 = fixture_control.point_collide (fixture_control.fixture[f].x - (tilesize / 2), fixture_control.fixture[f].y + (tilesize / 2), fixture_control.fixture[f].z + 9);
          if (b > -1 && brush_control.brush[b].electric == true) electric_west = true;
          else if (f2 > -1 && fixture_control.fixture[f2].electric == true) electric_west = true;
          else if (f3 > -1 && fixture_control.fixture[f3].electric == true) electric_west = true;
          else electric_west = false;

          b = brush_control.point_in_brush (fixture_control.fixture[f].x + Convert.ToInt32 (tilesize * 1.5), fixture_control.fixture[f].y + (tilesize / 2), fixture_control.fixture[f].z + 9, false, false);
          f2 = fixture_control.point_collide (fixture_control.fixture[f].x + Convert.ToInt32 (tilesize * 1.5), fixture_control.fixture[f].y + (tilesize / 2), fixture_control.fixture[f].z + 1);
          f3 = fixture_control.point_collide (fixture_control.fixture[f].x + Convert.ToInt32 (tilesize * 1.5), fixture_control.fixture[f].y + (tilesize / 2), fixture_control.fixture[f].z + 9);
          if (b > -1 && brush_control.brush[b].electric == true) electric_east = true;
          else if (f2 > -1 && fixture_control.fixture[f2].electric == true) electric_east = true;
          else if (f3 > -1 && fixture_control.fixture[f3].electric == true) electric_east = true;
          else electric_east = false;

          if (electric_north == true && electric_south == true) fixture_control.fixture[f].type = (int) Fixture_Control.F.WIRES_VERTICAL_TEST;
          else if (electric_south == true && electric_east == true) fixture_control.fixture[f].type = (int) Fixture_Control.F.WIRES_SOUTHEAST_TEST;
          else if (electric_north == true) fixture_control.fixture[f].type = (int) Fixture_Control.F.WIRES_VERTICAL_TEST;
          else if (electric_south == true) fixture_control.fixture[f].type = (int) Fixture_Control.F.WIRES_VERTICAL_TEST;
          else fixture_control.fixture[f].type = (int) Fixture_Control.F.WIRES_HORIZONTAL_TEST;
          }
        }

      Add_Stickers ();

      if (lighting_engine == 1)
        {
        // ambient lighting
        if (ambient_dark != 0f)
          {
          if (ambient_dark < 0f) ambient_dark = 0f;
          if (ambient_dark > 1f) ambient_dark = 1f;

          //total_lights = 0;
          for (y = tilesize / 2; y < Convert.ToInt32 (map_tile_length * tilesize); y += tilesize * 2)
            {
            for (x = tilesize / 2; x < Convert.ToInt32 (map_tile_width * tilesize); x += tilesize * 2)
              {
              r = rnd.Next (0, 100);
              // 30% dark
              // 60% white
              // 10% colored
              //r = 0;
              if (r < 30 && ambient_dark > 0f)
                {
                light_number = (int) L.dark;
                light_alpha = ambient_dark;
                }
              else if (r >= 30 && r < 90)
                {
                light_number = (int) L.white;
                light_alpha = 0f;
                }
              else
                {
                r = rnd.Next (0, 5);
                if (r == 0) light_number = (int) L.red;
                else if (r == 1) light_number = (int) L.green;
                else if (r == 2) light_number = (int) L.blue;
                else if (r == 3) light_number = (int) L.fushia;
                else light_number = (int) L.yellow;
                light_alpha = .15f;
                }
              add_light (light_number, x, y, z, 2f, light_alpha, SOLID);
              }
            }
          }
        }

      // additive blending
      if (lighting_engine == 2)
        {
        // ambient lighting
        if (ambient_dark != 0f)
          {
          if (ambient_dark < 0f) ambient_dark = 0f;
          if (ambient_dark > 1f) ambient_dark = 1f;

          //total_lights = 0;
          for (y = tilesize / 2; y < Convert.ToInt32 (map_tile_length * tilesize); y += tilesize * 2)
            {
            for (x = tilesize / 2; x < Convert.ToInt32 (map_tile_width * tilesize); x += tilesize * 2)
              {
              r = rnd.Next (0, 100);
              // 20% dark
              // 70% white
              // 10% colored
              //r = 0;
              if (r >= 20 && r < 90)
                {
                light_color = Color.White;
                light_alpha = 0f;
                }
              else
                {
                r = rnd.Next (0, 5);
                if (r == 0) light_color = Color.Red;
                else if (r == 1) light_color = Color.Green;
                else if (r == 2) light_color = Color.Blue;
                else if (r == 3) light_color = Color.Purple;
                else light_color = Color.Yellow;
                light_alpha = .5f;
                }
              add_light (light_color, x, y, z, 2f, light_alpha, SOLID);
              }
            }
          }
        }

      // experimental net-style lights
      else if (lighting_engine == 3)
        {
        // ambient lighting
        if (ambient_dark != 0f)
          {
          if (ambient_dark < 0f) ambient_dark = 0f;
          if (ambient_dark > 1f) ambient_dark = 1f;

          //total_lights = 0;
          for (y = tilesize * 3 / 2; y < Convert.ToInt32 (map_tile_length * tilesize); y += tilesize * 2)
            {
            for (x = tilesize * 3 / 2; x < Convert.ToInt32 (map_tile_width * tilesize); x += tilesize * 2)
              {
              r = rnd.Next (0, 100);
              // 20% dark
              // 70% white
              // 10% colored
              if (r < 20 && ambient_dark > 0f)
                {
                light_number = (int) L.dark;
                light_alpha = ambient_dark;
                }
              else if (r >= 20 && r < 90)
                {
                light_number = (int) L.white;
                light_alpha = 0f;
                }
              else
                {
                r = rnd.Next (0, 5);
                if (r == 0) light_number = (int) L.red;
                else if (r == 1) light_number = (int) L.green;
                else if (r == 2) light_number = (int) L.blue;
                else if (r == 3) light_number = (int) L.fushia;
                else light_number = (int) L.yellow;
                light_alpha = .15f;
                }
              add_light (light_number, x, y, z, 2f, light_alpha, SOLID);
              }
            }
          }
        }

      screen.scroll_x = 0;
      screen.scroll_y = 0;

      // reset player stats
      character_control.character[PLAYER].health = 100;
      character_control.character[PLAYER].action = "none";
      character_control.character[PLAYER].on_fire = 0;

      // random enemy placement
      if (total_character_spots >= random_retard1 + random_retard2)
        {
        spot = -1;
        for (int guy = 0; guy < random_retard1; guy += 1)
          {
          while (spot == -1 || random_character_spot[spot].used == true) spot = rnd.Next (0, total_character_spots);
          add_character ("retard", (int) Character_Control.C.RETARD, random_character_spot[spot].x + (tilesize / 2), random_character_spot[spot].y + (tilesize / 2), random_character_spot[spot].z);
          random_character_spot[spot].used = true;
          }
        spot = -1;
        for (int guy = 0; guy < random_retard2; guy += 1)
          {
          while (spot == -1 || random_character_spot[spot].used == true) spot = rnd.Next (0, total_character_spots);
          add_character ("throwing retard", (int) Character_Control.C.THROWING_RETARD, random_character_spot[spot].x + (tilesize / 2), random_character_spot[spot].y + (tilesize / 2), random_character_spot[spot].z);
          random_character_spot[spot].used = true;
          }
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void add_brush (int top_texture_number, int front_texture_number, int x, int y, int z, int width, int length, int height)
      {
      if (brush_control.brush.Count < Brush_Control.max_brushes)
        {
        brush_control.add (top_texture_number, front_texture_number, x, y, z, width, length, height);
        total_draw_slots += 1;
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    bool string_is_int (string s)
      {
      bool is_number = true;

      for (int c = 0; c < s.Length; c++)
        {
        if (s[c] != '0'
            && s[c] != '1'
            && s[c] != '2'
            && s[c] != '3'
            && s[c] != '4'
            && s[c] != '5'
            && s[c] != '6'
            && s[c] != '7'
            && s[c] != '8'
            && s[c] != '9') is_number = false;
        }

      return is_number;
      }

    ////////////////////////////////////////////////////////////////////////////////

    bool string_is_float (string s)
      {
      bool is_float = true;

      for (int c = 0; c < s.Length; c += 1)
        {
        if (s[c] != '0'
            && s[c] != '1'
            && s[c] != '2'
            && s[c] != '3'
            && s[c] != '4'
            && s[c] != '5'
            && s[c] != '6'
            && s[c] != '7'
            && s[c] != '8'
            && s[c] != '9'
            && s[c] != '.'
            && s[c] != '-') is_float = false;
        }

      return is_float;
      }

    ////////////////////////////////////////////////////////////////////////////////

    void save_game ()
      {
      //StreamWriter sw = new StreamWriter("content/map.dsm");
      //sw.Close();
      }

    ////////////////////////////////////////////////////////////////////////////////

    void add_light (int light_number, int x, int y, int z, float scale, float alpha, int type)
      {
      int radius;
      bool light_in_space = false;
      int r;
      float ambient_brightness;

      if (total_lights < max_lights)
        {
        if (light_number == (int) L.dark && lighting_engine != 3)
          {
          radius = Convert.ToInt32 (light_sprite[light_number].Width * .2 * scale);
          for (int l = 0; l < total_lights; l += 1)
            {
            if (x >= light[l].x - radius && x <= light[l].x + radius
                && y >= light[l].y - radius && y <= light[l].y + radius)
              light_in_space = true;
            }
          }

        if (light_in_space == false)
          {
          if (light_number != (int) L.dark)
            {
            r = rnd.Next (0, 100);
            if (r < 45) type = PULSING;
            else if (r == 45) type = FLICKERING;
            else type = SOLID;
            }

          light[total_lights].light_number = light_number;
          light[total_lights].x = x;
          light[total_lights].y = y;
          light[total_lights].z = z;
          light[total_lights].scale = scale;
          light[total_lights].alpha = alpha;
          light[total_lights].type = type;
          light[total_lights].on = true;
          light[total_lights].waxing = false;
          light[total_lights].dimness = 0f;

          if (type == PULSING)
            {
            r = rnd.Next (10, 30);
            light[total_lights].pulse_speed = Convert.ToSingle (r) / 10000f;
            light[total_lights].waxing = true;
            }
          else light[total_lights].pulse_speed = 0f;

          total_lights += 1;
          }
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    // lighting engine 2
    void add_light (Color color, int x, int y, int z, float scale, float alpha, int type)
      {
      int radius;
      bool light_in_space = false;
      int r;
      //float ambient_brightness;

      if (total_lights < max_lights)
        {
        radius = tilesize;
        //radius = Convert.ToInt32 (light_sprite[(int) L.white].Width * .2 * scale);
        for (int l = 0; l < total_lights; l += 1)
          {
          if (x >= light[l].x - radius && x <= light[l].x + radius
              && y >= light[l].y - radius && y <= light[l].y + radius)
            light_in_space = true;
          }

        if (light_in_space == false)
          {
          r = rnd.Next (0, 100);
          if (r < 45) type = PULSING;
          else if (r > 46) type = SOLID;
          else type = FLICKERING;

          light[total_lights].x = x;
          light[total_lights].y = y;
          light[total_lights].z = z;
          light[total_lights].scale = scale;

          //if (color == Color.White)
          //{
          // average rbg values of ambient light to get overall brightness value (0-255)
          //ambient_brightness = Convert.ToSingle ((ambient_light.R + ambient_light.G + ambient_light.B) / 3);
          //ambient_brightness = ambient_brightness / 255;  // convert to a (0.0-1.0) scale
          //alpha = 1f - ambient_brightness;
          //}
          light[total_lights].alpha = alpha;
          light[total_lights].type = type;
          light[total_lights].on = true;
          light[total_lights].waxing = false;
          light[total_lights].dimness = 0f;
          light[total_lights].c = color;

          if (type == PULSING)
            {
            r = rnd.Next (10, 30);
            light[total_lights].pulse_speed = Convert.ToSingle (r) / 10000f;
            light[total_lights].waxing = true;
            }
          else light[total_lights].pulse_speed = 0f;

          total_lights += 1;
          }
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void remove_light (int l1)
      {
      if (total_lights > 0 && l1 >= 0 && l1 < total_lights)
        {
        for (int l2 = l1; l2 < total_lights - 1; l2++) light[l2] = light[l2 + 1];
        total_lights -= 1;
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void destroy_object (int o)
      {
      // update_objects calls collision detection multiple times for each object every cycle.  therefor, a collision with a character
      // is detected multiple times.  object_control.obj[o].destroyed prevents multiple objects from being removed from the list for a single event.
      //if (object_control.obj[o].destroyed == false)
      //{
      object_control.obj[o].destroyed = true;

      //if (o < total_objects - 1)
      //  {
      //for (int q = o; q < total_objects - 1; q += 1)
      //{
      //obj[q] = obj[q + 1];
      //}
      //total_objects -= 1;
      //total_draw_slots -= 1;
      //  }

      //else  // last object in list
      //{
      //total_objects -= 1;
      //total_draw_slots -= 1;
      //}
      //}
      }

    ////////////////////////////////////////////////////////////////////////////////

    void remove_one_destroyed_object ()
      {
      int o, q;
      //int removed_objects = 0;

      //o = 0;
      //while (o < total_objects && object_control.obj[o].destroyed == false) o += 1;
      //if (object_control.obj[o].destroyed == true)
      //  {
      //  for (q = 0; q < total_objects - 1; q += 1)
      //    {
      //    obj[q] = obj[q + 1];
      //    }
      //  total_objects -= 1;
      //  }

      o = 0;
      while (o < object_control.obj.Count && object_control.obj[o].destroyed == false)
        {
        if (object_control.obj[o].destroyed) object_control.obj.RemoveAt (o);
        o += 1;
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void add_character (string name, int sprite, int x, int y, int z)
      {
      if (character_control.character.Count < max_characters)
        {
        character_control.add (name, sprite, x, y, z);
        total_draw_slots += 1;
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void add_player (int x, int y, int z)
      {
      player_added_to_map = true;

      if (character_control.character.Count == 0) character_control.character.Add (new Character ());

      character_control.character[PLAYER].x = x + (tilesize / 2);
      character_control.character[PLAYER].y = y + (tilesize / 2);
      character_control.character[PLAYER].z = z;
      character_control.character[PLAYER].dx = character_control.character[PLAYER].x;
      character_control.character[PLAYER].dy = character_control.character[PLAYER].y;
      character_control.character[PLAYER].dz = character_control.character[PLAYER].z;

      if (player_created == false)
        {
        player_created = true;
        character_control.character[PLAYER].name = "Richard";
        character_control.character[PLAYER].sprite = (int) Character_Control.C.RICHARD;
        character_control.character[PLAYER].defaults ();
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void Mouse_Input (GameTime game_time)
      {
      //double h_distance;
      //Rectangle r;
      //int b;

      mouse_last = mouse_current;
      mouse_current = Mouse.GetState ();

      // if cursor moved, changed direction facing
      if (mouse_current.X != mouse_last.X || mouse_current.Y != mouse_last.Y)
        {
        //character_control.character[PLAYER].dir = get_direction (character_control.character[PLAYER].x, character_control.character[PLAYER].y, mouse_current.X - scroll_x, screen.height - (character_control.character[PLAYER].z / 2) + scroll_y - mouse_current.Y);
        }

      // left mouse button - move to point
      if (mouse_current.LeftButton == ButtonState.Pressed)
        {
        /*
        if (game_state == GAME && character_control.active (PLAYER))
          {
          if (character_control.character[PLAYER].action != "grabbing" && character_control.character[PLAYER].action != "pushing")
            {
            // distance between mouse and player
            h_distance = distance2d (character_control.character[PLAYER].x, character_control.character[PLAYER].y, mouse_current.X - scroll_x, screen.height - (character_control.character[PLAYER].z / 2) + scroll_y - mouse_current.Y);

            // round to zero to prevent jittering
            if (h_distance >= 20)
              {
              // get radians of direction
              character_control.character[PLAYER].dir = get_direction (character_control.character[PLAYER].x, character_control.character[PLAYER].y, mouse_current.X - scroll_x, screen.height - (character_control.character[PLAYER].z / 2) + scroll_y - mouse_current.Y);
              character_control.character[PLAYER].self_velocity = (h_distance * .015) * (character_control.character[PLAYER].speed * .3);
              if (character_on_ground (PLAYER)) character_control.character[PLAYER].walk ();
              }
            else if (character_control.character[PLAYER].action == "walking" || character_control.character[PLAYER].action == "running") character_control.character[PLAYER].stand ();
            }

          // grabbing or pushing box
          else if (character_control.character[PLAYER].action == "grabbing" && brush_control.brush[character_control.character[PLAYER].brush_grab].moveable == true)
            {
            if (character_control.character[PLAYER].grab_position == "below")  // up
              {
              character_control.character[PLAYER].action = "pushing";
              b = character_control.character[PLAYER].brush_grab;
              brush_control.brush[b].moving = true;
              character_control.character[PLAYER].dx = brush_control.brush[b].x + (brush_control.brush[b].width / 2);
              character_control.character[PLAYER].dy = brush_control.brush[b].y - (tilesize / 3);
              character_control.character[PLAYER].push_x = character_control.character[PLAYER].x;
              character_control.character[PLAYER].push_y = character_control.character[PLAYER].y + box_move;
              character_control.character[PLAYER].push_dir = "up";
              character_control.character[PLAYER].self_x_velocity = 0;
              }
            else if (character_control.character[PLAYER].grab_position == "above")  // down
              {
              character_control.character[PLAYER].action = "pushing";
              b = character_control.character[PLAYER].brush_grab;
              brush_control.brush[b].moving = true;
              character_control.character[PLAYER].dx = brush_control.brush[b].x + (brush_control.brush[b].width / 2);
              character_control.character[PLAYER].dy = brush_control.brush[b].y + brush_control.brush[b].length + (tilesize / 4);
              character_control.character[PLAYER].push_x = character_control.character[PLAYER].x;
              character_control.character[PLAYER].push_y = character_control.character[PLAYER].y - box_move;
              character_control.character[PLAYER].push_dir = "down";
              character_control.character[PLAYER].self_x_velocity = 0;
              }
            else if (character_control.character[PLAYER].grab_position == "right")  // left
              {
              character_control.character[PLAYER].action = "pushing";
              b = character_control.character[PLAYER].brush_grab;
              brush_control.brush[b].moving = true;
              character_control.character[PLAYER].dx = brush_control.brush[b].x + brush_control.brush[b].width + (tilesize / 3);
              character_control.character[PLAYER].dy = brush_control.brush[b].y + (brush_control.brush[b].length / 2);
              character_control.character[PLAYER].push_x = character_control.character[PLAYER].x - box_move;
              character_control.character[PLAYER].push_y = character_control.character[PLAYER].y;
              character_control.character[PLAYER].push_dir = "left";
              character_control.character[PLAYER].self_y_velocity = 0;
              }
            else if (character_control.character[PLAYER].grab_position == "left")  // right
              {
              character_control.character[PLAYER].action = "pushing";
              b = character_control.character[PLAYER].brush_grab;
              brush_control.brush[b].moving = true;
              character_control.character[PLAYER].dx = brush_control.brush[b].x - (tilesize / 3);
              character_control.character[PLAYER].dy = brush_control.brush[b].y + (brush_control.brush[b].length / 2);
              character_control.character[PLAYER].push_x = character_control.character[PLAYER].x + box_move;
              character_control.character[PLAYER].push_y = character_control.character[PLAYER].y;
              character_control.character[PLAYER].push_dir = "right";
              character_control.character[PLAYER].self_y_velocity = 0;
              }
            }
          }
        */

        if (game_state == GAME && game_menu == true)
          {
          if (menu_screen == "main")
            {
            if (mouse_over (menu_exit_v, menu_exit_test)) Exit ();
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
        if (game_state == GAME && PLAYER > -1 && character_control.active (PLAYER) && character_control.character[PLAYER].action != "pushing") character_control.character[PLAYER].stand ();
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

    bool mouse_over (Vector2 v, Texture2D t)
      {
      // checks if mouse is over object's sprite

      MouseState current_mouse = Mouse.GetState ();

      if (current_mouse.X >= v.X && current_mouse.X <= v.X + t.Width
          && current_mouse.Y >= v.Y && current_mouse.Y <= v.Y + t.Height)
        return true;

      else
      return false;
      }

    ////////////////////////////////////////////////////////////////////////////////

    bool mouse_over_rect (Rectangle r)
      {
      // checks if mouse is over rectangle-defined area

      mouse_current = Mouse.GetState ();

      if (mouse_current.X >= r.X && mouse_current.X <= r.X + r.Width
          && mouse_current.Y >= r.Y && mouse_current.Y <= r.Y + r.Height)
        return true;

      else
      return false;
      }

    ////////////////////////////////////////////////////////////////////////////////

    void Get_Input ()  // decide if player is using keyboard or controller/gamepad at title screen
      {
      // check for keyboard key
      keyboard = Keyboard.GetState ();
      bool keydown = false;
      for (int k = 0; k < 160; k += 1) if (keyboard.IsKeyDown ((Keys) k)) keydown = true;
      if (keydown == true) player_control = "keyboard";

      // check for controller button
      controller = GamePad.GetState (PlayerIndex.One);
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
        player_control = "controller";

      if (player_control != "none") game_state = GAME;
      }

    ////////////////////////////////////////////////////////////////////////////////

    void Keyboard_Input ()
      {
      //int b;'
      double next_dir = character_control.character[PLAYER].dir;
      bool skid = false;

      keyboard = Keyboard.GetState ();

      if (player_control == "keyboard")
        {
        if (game_state == GAME && character_control.active (PLAYER) && game_menu == false)
          {
          // change direction facing (if-else chain handles diagonal directions)
          if (keyboard.IsKeyDown (Keys.Down) && keyboard.IsKeyDown (Keys.Right)) next_dir = MathHelper.ToRadians (315);
          else if (keyboard.IsKeyDown (Keys.Down) && keyboard.IsKeyDown (Keys.Left)) next_dir = MathHelper.ToRadians (225);
          else if (keyboard.IsKeyDown (Keys.Up) && keyboard.IsKeyDown (Keys.Left)) next_dir = MathHelper.ToRadians (135);
          else if (keyboard.IsKeyDown (Keys.Up) && keyboard.IsKeyDown (Keys.Right)) next_dir = MathHelper.ToRadians (45);
          else if (keyboard.IsKeyDown (Keys.Right)) next_dir = MathHelper.ToRadians (0);
          else if (keyboard.IsKeyDown (Keys.Down)) next_dir = MathHelper.ToRadians (270);
          else if (keyboard.IsKeyDown (Keys.Left)) next_dir = MathHelper.ToRadians (180);
          else if (keyboard.IsKeyDown (Keys.Up)) next_dir = MathHelper.ToRadians (90);

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

          if (player_sharp_turn && character_control.character[PLAYER].action == "running") skid = true;// character_control.character[PLAYER].skid ();
          if (player_sharp_turn && character_control.character[PLAYER].action == "standing" && character_control.character[PLAYER].skid_counter < Character.skid_delay) skid = true;// character_control.character[PLAYER].skid ();

          //if (player_sharp_turn)
          //  {
          //  character_control.character[PLAYER].last_action = character_control.character[PLAYER].action;
          //  if (character_control.character[PLAYER].action == "running" && character_control.character[PLAYER].last_action == "running") character_control.character[PLAYER].skid_if_able ();
          //  else if (character_control.character[PLAYER].action == "running"
          //  }
          if (next_dir != character_control.character[PLAYER].dir)
            {
            character_control.character[PLAYER].last_dir = character_control.character[PLAYER].dir;
            character_control.character[PLAYER].dir = next_dir;
            }
          if (skid) character_control.character[PLAYER].skid ();

          //if (character_control.character[PLAYER].last_dir != character_control.character[PLAYER].dir && character_control.character[PLAYER].sharp_turn ()
          //    && character_control.character[PLAYER].action == "running" && character_control.character[PLAYER].last_action == "running") character_control.character[PLAYER].skid_if_able ();

          // if any arrow key is held down (or w, a, s, d)
          if (keyboard.IsKeyDown (Keys.Up) || keyboard.IsKeyDown (Keys.Down) ||
              keyboard.IsKeyDown (Keys.Left) || keyboard.IsKeyDown (Keys.Right) ||
              keyboard.IsKeyDown (Keys.W) || keyboard.IsKeyDown (Keys.S) ||
              keyboard.IsKeyDown (Keys.A) || keyboard.IsKeyDown (Keys.D))
            {
            if (character_control.character[PLAYER].action != "grabbing" && character_control.character[PLAYER].action != "pushing")  // just walking (off grid)
              {
              if (character_control.character[PLAYER].runboost == true) character_control.character[PLAYER].self_velocity = character_control.character[PLAYER].max_self_velocity;
              else character_control.character[PLAYER].self_velocity = character_control.character[PLAYER].speed * .3;
              if (character_on_ground (PLAYER) && character_control.character[PLAYER].action != "skidding")
                {
                //if (character_control.character[PLAYER].runboost) character_control.character[PLAYER].run ();
                //else character_control.character[PLAYER].walk ();
                if (character_control.character[PLAYER].runboost)
                  {
                  character_control.character[PLAYER].last_action = character_control.character[PLAYER].action;
                  character_control.character[PLAYER].action = "running";
                  }
                else
                  {
                  character_control.character[PLAYER].walk ();
                  //character_control.character[PLAYER].last_action = character_control.character[PLAYER].action;
                  //character_control.character[PLAYER].action = "walking";
                  }
                }
              }
            if (key_anydirection == false) key_anydirection = true;
            }
          else
            {
            // when the last arrow key is released
            if (key_anydirection == true)
              {
              key_anydirection = false;
              // if player is walking, running, or skidding and he lets go, he stops moving
              if (character_control.character[PLAYER].action == "skidding")
                {
                character_control.character[PLAYER].ext_x_velocity = 0;
                character_control.character[PLAYER].ext_y_velocity = 0;
                }
              if (character_control.character[PLAYER].action == "walking" || character_control.character[PLAYER].action == "running"
                  || character_control.character[PLAYER].action == "skidding") character_control.character[PLAYER].stand ();
              }
            }
          }

        // up
        if (keyboard.IsKeyDown (Keys.Up))  // up held down
          {
          if (game_state == GAME) push_north ();
          if (key_up == false) key_up = true;
          }
        if (!keyboard.IsKeyDown (Keys.Up) && key_up == true) key_up = false;

        // down
        if (keyboard.IsKeyDown (Keys.Down))  // down held down
          {
          if (game_state == GAME) push_south ();

          if (key_down == false) key_down = true;
          }
        if (!keyboard.IsKeyDown (Keys.Down) && key_down == true) key_down = false;

        // left
        if (keyboard.IsKeyDown (Keys.Left))  // left held down
          {
          if (game_state == GAME) push_west ();

          if (key_left == false) key_left = true;
          }
        if (!keyboard.IsKeyDown (Keys.Left) && key_left == true) key_left = false;  // left released

        // right
        if (keyboard.IsKeyDown (Keys.Right))  // right held down
          {
          if (game_state == GAME) push_east ();

          if (key_right == false) key_right = true;
          }
        if (!keyboard.IsKeyDown (Keys.Right) && key_right == true) key_right = false;

        // W - up      
        if (keyboard.IsKeyDown (Keys.W))
          {
          if (game_state == GAME) push_north ();
          if (key_w == false) key_w = true;
          }
        if (!keyboard.IsKeyDown (Keys.W) && key_w == true) key_w = false;

        // S - down
        if (keyboard.IsKeyDown (Keys.S))
          {
          if (game_state == GAME) push_south ();
          if (key_s == false) key_s = true;
          }
        if (!keyboard.IsKeyDown (Keys.S) && key_s == true) key_s = false;

        // A - left
        if (keyboard.IsKeyDown (Keys.A))
          {
          if (game_state == GAME) push_west ();
          if (key_a == false) key_a = true;
          }
        if (!keyboard.IsKeyDown (Keys.A) && key_a == true) key_a = false;

        // D - right
        if (keyboard.IsKeyDown (Keys.D))
          {
          if (game_state == GAME) push_east ();
          if (key_d == false) key_d = true;
          }
        if (!keyboard.IsKeyDown (Keys.D) && key_d == true) key_d = false;

        // left shift - run
        //if (keyboard.IsKeyDown (Keys.LeftShift) && key_leftshift == false)
        //  {
        //  key_leftshift = true;
        //  character_control.character[PLAYER].runboost = true;
        //  if (character_control.character[PLAYER].action == "walking") character_control.character[PLAYER].action = "running";
        //  }
        //else if (!keyboard.IsKeyDown (Keys.LeftShift) && key_leftshift == true)
        //  {
        //  key_leftshift = false;
        //  character_control.character[PLAYER].runboost = false;
        //  if (character_control.character[PLAYER].action == "running") character_control.character[PLAYER].action = "walking";
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
          if (character_control.character[PLAYER].action == "punching") character_control.character[PLAYER].anim_frame_sequence = punch_rest_delay;
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
          if (character_control.character[PLAYER].action == "punching") character_control.character[PLAYER].anim_frame_sequence = punch_rest_delay;
          }

        // left alt - special attack (shoot, throw, butterball, butt pound, etc.)
        if (keyboard.IsKeyDown (Keys.LeftAlt))
          {
          continue_special_attack ();
          //if (character_control.character[PLAYER].shirt == (int) Object_Control.O.SHIRT_YELLOW) character_control.character[PLAYER].action = "superpunch";
          //else if (character_control.character[PLAYER].shirt == "fire") character_control.character[PLAYER].action = "flamethrower";
          //else if (character_control.character[PLAYER].shirt == "ice") character_control.character[PLAYER].action = "freeze ray";

          if (key_leftalt == false)
            {
            key_leftalt = true;

            begin_special_attack ();

            //if (character_control.character[PLAYER].shirt == (int) Object_Control.O.SHIRT_YELLOW) particle_superpunch (character_control.character[PLAYER].x, character_control.character[PLAYER].y, character_control.character[PLAYER].z + Convert.ToInt32 (character_control.character[PLAYER].box_height * .75), Convert.ToInt32 (MathHelper.ToDegrees (Convert.ToSingle (character_control.character[PLAYER].dir))));
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
          //if (character_control.character[PLAYER].action == "superpunch" || character_control.character[PLAYER].action == "freeze ray"
          //    || character_control.character[PLAYER].action == "flamethrower") character_control.character[PLAYER].action = "none";
          }

        // space - jump / enter level
        if (keyboard.IsKeyDown (Keys.Space) && key_space == false)
          {
          key_space = true;
          //if (game_state == WORLD) enter_level ();

          //else
          if (game_state == GAME && character_control.active (PLAYER)) character_jump (PLAYER);
          else if (game_state == CREATION && observe_creation == true) load_map ();
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
            else if (character_control.character[PLAYER].shirt == (int) Object_Control.O.SHIRT_YELLOW && character_control.character[PLAYER].shirt_power) endloop = true;
            else if (character_control.character[PLAYER].shirt == (int) Object_Control.O.SHIRT_RED    && character_control.character[PLAYER].shirt_fire) endloop = true;
            else if (character_control.character[PLAYER].shirt == (int) Object_Control.O.SHIRT_WHITE  && character_control.character[PLAYER].shirt_ice) endloop = true;
            else if (character_control.character[PLAYER].shirt == (int) Object_Control.O.SHIRT_BLUE   && character_control.character[PLAYER].shirt_electric) endloop = true;
            else if (character_control.character[PLAYER].shirt == (int) Object_Control.O.SHIRT_PURPLE && character_control.character[PLAYER].shirt_magnetic) endloop = true;
            }

          //if (character_control.character[PLAYER].shirt == 0)//"none")
          //  {
          //  if (character_control.character[PLAYER].shirt_power == true) character_control.character[PLAYER].shirt = (int) Object_Control.O.SHIRT_YELLOW;//(int) Object_Control.O.SHIRT_YELLOW;
          //  else if (character_control.character[PLAYER].shirt_fire == true) character_control.character[PLAYER].shirt = (int) Object_Control.O.SHIRT_RED;//"fire";
          //  else if (character_control.character[PLAYER].shirt_ice == true) character_control.character[PLAYER].shirt = (int) Object_Control.O.SHIRT_WHITE;//"ice";
          //  else if (character_control.character[PLAYER].shirt_magnetic == true) character_control.character[PLAYER].shirt = (int) Object_Control.O.SHIRT_PURPLE;//"magnet";
          //  else if (character_control.character[PLAYER].shirt_electric == true) character_control.character[PLAYER].shirt = (int) Object_Control.O.SHIRT_BLUE;//(int) Object_Control.O.SHIRT_BLUE;
          //  else character_control.character[PLAYER].shirt = 0;//"none";
          //  }
          //else if (character_control.character[PLAYER].shirt == (int) Object_Control.O.SHIRT_YELLOW)//(int) Object_Control.O.SHIRT_YELLOW)
          //  {
          //  if (character_control.character[PLAYER].shirt_fire == true) character_control.character[PLAYER].shirt = "fire";
          //  else if (character_control.character[PLAYER].shirt_ice == true) character_control.character[PLAYER].shirt = "ice";
          //  else if (character_control.character[PLAYER].shirt_magnetic == true) character_control.character[PLAYER].shirt = "magnet";
          //  else if (character_control.character[PLAYER].shirt_electric == true) character_control.character[PLAYER].shirt = (int) Object_Control.O.SHIRT_BLUE;
          //  else character_control.character[PLAYER].shirt = 0;// "none";
          //  }
          //else if (character_control.character[PLAYER].shirt == (int) Object_Control.O.SHIRT_RED)//"fire")
          //  {
          //  if (character_control.character[PLAYER].shirt_ice == true) character_control.character[PLAYER].shirt = "ice";
          //  else if (character_control.character[PLAYER].shirt_magnetic == true) character_control.character[PLAYER].shirt = "magnet";
          //  else if (character_control.character[PLAYER].shirt_electric == true) character_control.character[PLAYER].shirt = "electric";
          //  else character_control.character[PLAYER].shirt = 0;// "none";
          //  }
          //else if (character_control.character[PLAYER].shirt == (int) Object_Control.O.SHIRT_WHITE)//"ice")
          //  {
          //  if (character_control.character[PLAYER].shirt_magnetic == true) character_control.character[PLAYER].shirt = "magnet";
          //  else if (character_control.character[PLAYER].shirt_electric == true) character_control.character[PLAYER].shirt = "electric";
          //  else character_control.character[PLAYER].shirt = "none";
          //  }
          //else if (character_control.character[PLAYER].shirt == "magnet")
          //  {
          //  if (character_control.character[PLAYER].shirt_electric == true) character_control.character[PLAYER].shirt = "electric";
          //  else character_control.character[PLAYER].shirt = "none";
          //  }
          //else character_control.character[PLAYER].shirt = "none";
          }
        else if (!keyboard.IsKeyDown (Keys.Tab) && key_tab == true) key_tab = false;

        // number pad 0 - grab box
        if (keyboard.IsKeyDown (Keys.NumPad0) && key_numpad0 == false)
          {
          key_numpad0 = true;
          if (character_control.character[PLAYER].action != "grabbing" && character_control.character[PLAYER].action != "pushing") character_grab_brush (PLAYER);
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
          if (character_control.character[PLAYER].action == "punching") character_control.character[PLAYER].anim_frame_sequence = punch_rest_delay;
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

        // E - grab box / activate
        if (keyboard.IsKeyDown (Keys.E) && key_e == false)
          {
          key_e = true;
          if (character_control.character[PLAYER].action != "grabbing" && character_control.character[PLAYER].action != "pushing") character_grab_brush (PLAYER);
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
          if (character_control.character[PLAYER].action == "punching") character_control.character[PLAYER].anim_frame_sequence = punch_rest_delay;
          }

        // V - grab box
        if (keyboard.IsKeyDown (Keys.V) && key_v == false)
          {
          key_v = true;
          if (character_control.character[PLAYER].action != "grabbing" && character_control.character[PLAYER].action != "pushing") character_grab_brush (PLAYER);
          }

        // X - grab box
        if (keyboard.IsKeyDown (Keys.X) && key_x == false)
          {
          key_x = true;
          if (character_control.character[PLAYER].action != "grabbing" && character_control.character[PLAYER].action != "pushing") character_grab_brush (PLAYER);
          }

        // z - run
        if (keyboard.IsKeyDown (Keys.Z) && key_z == false)
          {
          key_z = true;
          character_control.character[PLAYER].runboost = true;
          if (character_control.character[PLAYER].action == "walking") character_control.character[PLAYER].action = "running";
          }
        else if (!keyboard.IsKeyDown (Keys.Z) && key_z == true)
          {
          key_z = false;
          character_control.character[PLAYER].runboost = false;
          if (character_control.character[PLAYER].action == "running") character_control.character[PLAYER].action = "walking";
          }

        // release e, x, v, 0 (letting go of a box)
        if (!keyboard.IsKeyDown (Keys.E) && !keyboard.IsKeyDown (Keys.X) && !keyboard.IsKeyDown (Keys.NumPad0) && !keyboard.IsKeyDown (Keys.V))
          {
          if (game_state == GAME)// && character_control.character[PLAYER].action != "pushing")
            {
            character_control.character[PLAYER].brush_grab = -1;
            if (character_control.character[PLAYER].action == "grabbing" || character_control.character[PLAYER].action == "pushing") character_control.character[PLAYER].stand ();
            }
          if (key_e == true) key_e = false;
          if (key_v == true) key_v = false;
          if (key_x == true) key_x = false;
          if (key_numpad0 == true) key_numpad0 = false;
          }
        }  // player_control == "keyboard"

      // escape - menu
      if (keyboard.IsKeyDown (Keys.Escape) && key_esc == false)  // escape single hit
        {
        key_esc = true;
        //Exit ();

        if (game_menu == false)
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

      // 2 - toggle wall drawing (developer)
      if (keyboard.IsKeyDown (Keys.D2) && key_2 == false)
        {
        key_2 = true;
        if (draw_walls == true) draw_walls = false;
        else draw_walls = true;
        }
      if (!keyboard.IsKeyDown (Keys.D2) && key_2 == true) key_2 = false;

      // 3 - toggle box drawing (developer)
      if (keyboard.IsKeyDown (Keys.D3) && key_3 == false)
        {
        key_3 = true;
        if (draw_boxes == true) draw_boxes = false;
        else draw_boxes = true;
        }
      if (!keyboard.IsKeyDown (Keys.D3) && key_3 == true) key_3 = false;

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

      // enter - refresh graphics (developer only)
      if (keyboard.IsKeyDown (Keys.Enter) && key_enter == false)
        {
        key_enter = true;
        //load_graphics ();
        }
      if (!keyboard.IsKeyDown (Keys.Enter) && key_enter == true) key_enter = false;

      // backspace - menu
      if (keyboard.IsKeyDown (Keys.Back) && key_backspace == false)
        {
        key_backspace = true;
        if (game_menu == false)
          {
          game_menu = true;
          menu_screen = "main";
          }
        else game_menu = false;
        }
      if (!keyboard.IsKeyDown (Keys.Back) && key_backspace == true) key_backspace = false;
      }

    ////////////////////////////////////////////////////////////////////////////////

    public bool sharp_turn (double angle1, double angle2)
      {
      double dir_degrees = MathHelper.ToDegrees (Convert.ToSingle (angle1));
      double last_dir_degrees = MathHelper.ToDegrees (Convert.ToSingle (angle2));
      double angle_difference = Math.Abs (dir_degrees - last_dir_degrees);
      if (angle_difference > 180) angle_difference = Math.Abs (angle_difference - 360);  // compensate for angles around the 0 degree mark
      if (angle_difference > 90)
        {
        //last_action = action;  // consider a sharp turn a new action
        return true;
        }
      else return false;
      }

    ////////////////////////////////////////////////////////////////////////////////

    bool character_on_ground (int c)
      {
      bool ground = false;

      character_control.character[c].z -= 1;
      if (character_in_brush (character_control.character[c]) >= 0) ground = true;
      if (character_in_fixture (character_control.character[c], true) >= 0) ground = true;
      character_control.character[c].z += 1;
      return ground;
      }

    ////////////////////////////////////////////////////////////////////////////////

    void skip_area ()
      {
      player_last_level = -1;// player_level;
      player_level += 1;
      if (player_level > 14) player_level = 0;

      enter_level ();
      }

    ////////////////////////////////////////////////////////////////////////////////

    void Controller_Input ()
      {
      double h_distance;

      controller = GamePad.GetState (PlayerIndex.One);

      if (controller.IsConnected)
        {
        // left stick
        if (controller.ThumbSticks.Left.X < 0 || controller.ThumbSticks.Left.X > 0 ||
            controller.ThumbSticks.Left.Y < 0 || controller.ThumbSticks.Left.Y > 0)
          {
          controller_left_stick = true;

          if (character_control.active (PLAYER))
            {
            // stick range = -1 to 1.  max stick should be max running speed.
            // diagonally, h_distance is about -.9 to .9, so we add .1 to adjust.
            // character movement code will reduce to max_self_velocity if too high
            h_distance = distance2d (0, 0, controller.ThumbSticks.Left.X, controller.ThumbSticks.Left.Y);
            character_control.character[PLAYER].self_velocity = character_control.character[PLAYER].max_self_velocity * (h_distance + .1);
            character_control.character[PLAYER].dir = get_direction (0, 0, controller.ThumbSticks.Left.X, controller.ThumbSticks.Left.Y);
            if (character_on_ground (PLAYER)) character_control.character[PLAYER].walk ();
            }
          }

        // left stick neutral
        else if (controller.ThumbSticks.Left.X == 0 && controller.ThumbSticks.Left.Y == 0)
          {
          if (controller_left_stick == true)
            {
            controller_left_stick = false;

            if (character_control.active (PLAYER) && (character_control.character[PLAYER].action == "walking" || character_control.character[PLAYER].action == "running")) character_control.character[PLAYER].stand ();  //character_control.character[PLAYER].self_velocity = 0;
            }
          }

        // X - punch, swing
        if (controller.Buttons.X == ButtonState.Pressed && controller_x == false)
          {
          controller_x = true;
          if (character_control.active (PLAYER))
            {
            if (character_on_ground (PLAYER)) character_punch (PLAYER);
            else character_jump_kick (PLAYER);
            }
          }
        else if (controller.Buttons.X == ButtonState.Released && controller_x == true)
          {
          controller_x = false;
          if (character_control.character[PLAYER].action == "punching") character_control.character[PLAYER].anim_frame_sequence = punch_rest_delay;
          }

        // A - jump
        if (controller.Buttons.A == ButtonState.Pressed && controller_a == false)
          {
          controller_a = true;

          if (character_control.active (PLAYER)) character_jump (PLAYER);
          }
        else if (controller.Buttons.A == ButtonState.Released && controller_a == true) controller_a = false;

        // B - push box / counter move
        if (controller.Buttons.B == ButtonState.Pressed && controller_b == false)
          {
          controller_b = true;
          if (character_control.character[PLAYER].action != "grabbing" && character_control.character[PLAYER].action != "pushing") character_grab_brush (PLAYER);
          }
        else if (controller.Buttons.B == ButtonState.Released && controller_b == true)
          {
          controller_b = false;
          if (game_state == GAME)
            {
            character_control.character[PLAYER].brush_grab = -1;
            if (character_control.character[PLAYER].action == "grabbing" || character_control.character[PLAYER].action == "pushing") character_control.character[PLAYER].stand ();
            }
          }

        // Y - special move
        if (controller.Buttons.Y == ButtonState.Pressed && controller_y == false)
          {
          controller_y = true;

          begin_special_attack ();
          }
        else if (controller.Buttons.Y == ButtonState.Released && controller_y == true) controller_y = false;

        // right trigger - shoot, throw

        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void enter_level ()
      {
      if (PLAYER > -1 && player_level > -1)
        {
        game_state = GAME;
        load_map ();
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void Update_Time (GameTime game_time)
      {
      //minute_change = false;

      if (game_second_last != game_time.TotalGameTime.Seconds)
        {
        game_second_last = game_time.TotalGameTime.Seconds;
        game_second += 1;
        fps = fps_counter;
        fps_counter = 0;
        }
      if (game_second >= 60)
        {
        game_second = 0;
        game_minute += 1;
        }
      if (game_minute >= 60)
        {
        game_minute = 0;
        game_hour += 1;
        }
      if (game_hour > 24) game_hour = 1;
      }

    ////////////////////////////////////////////////////////////////////////////////

    void Update_Lights ()
      {
      for (int l = 0; l < total_lights; l += 1)
        {
        if (light[l].type == FLICKERING)
          {
          if (light[l].on == true && rnd.Next (0, 50) == 0) light[l].on = false;
          else if (light[l].on == false && rnd.Next (0, 4) == 0) light[l].on = true;
          }

        else if (light[l].type == PULSING)
          {
          if (lighting_engine == 2)
            {
            if (light[l].waxing == true)  // getting brighter
              {
              light[l].alpha += light[l].pulse_speed;
              if (light[l].c == Color.White && light[l].alpha >= .8f)
                {
                light[l].alpha = .8f;
                light[l].waxing = false;
                }
              if (light[l].c != Color.White && light[l].alpha >= .55f)
                {
                light[l].alpha = .55f;
                light[l].waxing = false;
                }
              }
            else if (light[l].waxing == false)  // waning / getting dimmer
              {
              light[l].alpha -= light[l].pulse_speed;
              if (light[l].alpha <= 0f)
                {
                light[l].alpha = 0f;
                light[l].waxing = true;
                }
              }
            }
          else  // engine 1 or 3
            {
            if (light[l].waxing == true)  // getting brighter
              {
              light[l].dimness -= light[l].pulse_speed;
              if (light[l].dimness <= ambient_dark - .3f)
                {
                light[l].dimness = ambient_dark - .3f;
                light[l].waxing = false;
                }
              }
            else if (light[l].waxing == false)  // waning / getting dimmer
              {
              light[l].dimness += light[l].pulse_speed;
              if (light[l].dimness >= ambient_dark - .1f)//* .5f)
                {
                light[l].dimness = ambient_dark - .1f;//* .5f;
                light[l].waxing = true;
                }
              }
            }
          }
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void Update_Scrolling ()
      {
      Vector2 character_screen;

      // screen coordinates of player
      character_screen.X = character_control.character[PLAYER].x + screen.scroll_x;
      character_screen.Y = (screen.height - character_control.character[PLAYER].y) - (character_control.character[PLAYER].sprite_height / 2) - (character_control.character[PLAYER].z / 2) + screen.scroll_y;

      if (character_screen.X < scroll_border.X) screen.scroll_x += Convert.ToInt32 (scroll_border.X - character_screen.X);
      if (character_screen.X > scroll_border.X + scroll_border.Width) screen.scroll_x -= Convert.ToInt32 (character_screen.X - (scroll_border.X + scroll_border.Width));
      if (character_screen.Y < scroll_border.Y) screen.scroll_y += Convert.ToInt32 (scroll_border.Y - character_screen.Y);
      if (character_screen.Y > scroll_border.Y + scroll_border.Height) screen.scroll_y -= Convert.ToInt32 (character_screen.Y - (scroll_border.Y + scroll_border.Height));

      if (map.bg_scroll)
        {
        screen.bg1_scroll_x = Convert.ToInt32 (screen.scroll_x * screen.bg1_scroll_speed);
        screen.bg1_scroll_y = Convert.ToInt32 (screen.scroll_y * screen.bg1_scroll_speed);
        screen.bg2_scroll_x = Convert.ToInt32 (screen.scroll_x * screen.bg2_scroll_speed);
        screen.bg2_scroll_y = Convert.ToInt32 (screen.scroll_y * screen.bg2_scroll_speed);
        }
      else
        {
        screen.bg1_scroll_x = screen.scroll_x;
        screen.bg1_scroll_y = screen.scroll_y;
        screen.bg2_scroll_x = screen.scroll_x;
        screen.bg2_scroll_y = screen.scroll_y;
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void Check_Doors ()
      {
      int floor;         // brush_control.texture of brush_control.brush underneath the box
      int b, c;

      bool green_zone_found = false;
      bool red_zone_found = false;
      bool yellow_zone_found = false;

      bool green_zones_full = true;
      bool red_zones_full = true;
      bool yellow_zones_full = true;

      red_switch_down = false;
      yellow_doors_open = false;
      yellow_switch_down = false;
      blue_switch_down = false;

      green_doors_open = false;
      green_switch_down = false;
      red_doors_open = false;
      blue_doors_open = false;

      for (b = 0; b < brush_control.brush.Count; b += 1)
        {
        if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.FLOOR_ZONE_GREEN_TEST)
          {
          green_zone_found = true;
          floor = brush_control.point_in_brush (brush_control.brush[b].x + (brush_control.brush[b].width / 2), brush_control.brush[b].y + (brush_control.brush[b].length / 2), brush_control.brush[b].z + brush_control.brush[b].height + (tilesize / 4), false, false);
          if (floor == -1 || (floor > -1 && brush_control.brush[floor].top_texture_number != (int) Brush_Control.T.BOX_WOOD_TEST && brush_control.brush[floor].top_texture_number != (int) Brush_Control.T.BOX_METAL_TEST
              && brush_control.brush[floor].top_texture_number != (int) Brush_Control.T.BOX_ICE_TEST)) green_zones_full = false;
          }
        if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.FLOOR_ZONE_RED_TEST)
          {
          red_zone_found = true;
          floor = brush_control.point_in_brush (brush_control.brush[b].x + (brush_control.brush[b].width / 2), brush_control.brush[b].y + (brush_control.brush[b].length / 2), brush_control.brush[b].z + brush_control.brush[b].height + (tilesize / 4), false, false);
          if (floor == -1 || (floor > -1 && brush_control.brush[floor].top_texture_number != (int) Brush_Control.T.BOX_WOOD_TEST && brush_control.brush[floor].top_texture_number != (int) Brush_Control.T.BOX_METAL_TEST
              && brush_control.brush[floor].top_texture_number != (int) Brush_Control.T.BOX_ICE_TEST)) red_zones_full = false;
          }
        if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.FLOOR_ZONE_YELLOW_TEST)
          {
          yellow_zone_found = true;
          floor = brush_control.point_in_brush (brush_control.brush[b].x + (brush_control.brush[b].width / 2), brush_control.brush[b].y + (brush_control.brush[b].length / 2), brush_control.brush[b].z + brush_control.brush[b].height + (tilesize / 4), false, false);
          if (floor == -1 || (floor > -1 && brush_control.brush[floor].top_texture_number != (int) Brush_Control.T.BOX_WOOD_TEST && brush_control.brush[floor].top_texture_number != (int) Brush_Control.T.BOX_METAL_TEST
              && brush_control.brush[floor].top_texture_number != (int) Brush_Control.T.BOX_ICE_TEST)) yellow_zones_full = false;
          }
        }

      // if no goal zones are found in level, doors stay locked
      if (green_zone_found == false) green_zones_full = false;
      if (red_zone_found == false) red_zones_full = false;
      if (yellow_zone_found == false) yellow_zones_full = false;

      // check under boxes for switches (old method)
      //for (b = 0; b < brush_control.brush.Count; b += 1)
      //  {
      //  if (brush_control.brush[b].top_texture_number == BOX_TEST_WOOD)
      //    {
      //    floor = point_in_brush (brush_control.brush[b].x + (brush_control.brush[b].width / 2), brush_control.brush[b].y + (brush_control.brush[b].length / 2), brush_control.brush[b].z - (tilesize / 4), false);
      //    if (floor > -1 && brush_control.brush[floor].top_texture_number == SWITCH_TEST_GREEN) green_switch_down = true;
      //    if (floor > -1 && brush_control.brush[floor].top_texture_number == SWITCH_TEST_RED) red_switch_down = true;
      //    if (floor > -1 && brush_control.brush[floor].top_texture_number == SWITCH_TEST_YELLOW) yellow_switch_down = true;
      //    }
      //  }

      // check above switches for boxes and walls (new method)
      for (b = 0; b < brush_control.brush.Count; b += 1)
        {
        if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.SWITCH_RED_TEST
          && brush_control.point_in_brush (brush_control.brush[b].x + (brush_control.brush[b].width / 2), brush_control.brush[b].y + (brush_control.brush[b].length / 2), brush_control.brush[b].z + Convert.ToInt32 (tilesize * 1.5), false, false) > -1)
          {
          red_switch_down = true;
          }
        if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.SWITCH_YELLOW_TEST
          && brush_control.point_in_brush (brush_control.brush[b].x + (brush_control.brush[b].width / 2), brush_control.brush[b].y + (brush_control.brush[b].length / 2), brush_control.brush[b].z + Convert.ToInt32 (tilesize * 1.5), false, false) > -1)
          {
          yellow_switch_down = true;
          }
        if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.SWITCH_GREEN_TEST
          && brush_control.point_in_brush (brush_control.brush[b].x + (brush_control.brush[b].width / 2), brush_control.brush[b].y + (brush_control.brush[b].length / 2), brush_control.brush[b].z + Convert.ToInt32 (tilesize * 1.5), false, false) > -1)
          {
          green_switch_down = true;
          }
        if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.SWITCH_BLUE_TEST
          && brush_control.point_in_brush (brush_control.brush[b].x + (brush_control.brush[b].width / 2), brush_control.brush[b].y + (brush_control.brush[b].length / 2), brush_control.brush[b].z + Convert.ToInt32 (tilesize * 1.5), false, false) > -1)
          {
          blue_switch_down = true;
          }
        }

      // check for characters on switches
      //for (c = 0; c < total_characters; c += 1)
      for (c = 0; c < character_control.character.Count; c += 1)
        {
        if (character_control.active (c) && character_on_ground (c))
          {
          floor = brush_control.point_in_brush (character_control.character[c].x, character_control.character[c].y, character_control.character[c].z - (tilesize / 4), false, false);
          if (floor > -1 && brush_control.brush[floor].top_texture_number == (int) Brush_Control.T.SWITCH_GREEN_TEST) green_switch_down = true;
          if (floor > -1 && brush_control.brush[floor].top_texture_number == (int) Brush_Control.T.SWITCH_RED_TEST) red_switch_down = true;
          if (floor > -1 && brush_control.brush[floor].top_texture_number == (int) Brush_Control.T.SWITCH_YELLOW_TEST) yellow_switch_down = true;
          if (floor > -1 && brush_control.brush[floor].top_texture_number == (int) Brush_Control.T.SWITCH_BLUE_TEST) blue_switch_down = true;
          }
        }

      if (green_switch_down == true || green_zones_full == true) green_doors_open = true;
      if (red_switch_down == true || red_zones_full == true) red_doors_open = true;
      if (yellow_switch_down == true || yellow_zones_full == true) yellow_doors_open = true;
      if (blue_switch_down == true) blue_doors_open = true;

      // change solid value of door brushes based on switches
      foreach (Brush br in brush_control.brush)
        {
        if (blue_doors_open)
          {
          switch (br.top_texture_number)
            {
            case (int) Brush_Control.T.DOOR_BLUE_H_TOP_CLOSED_TEST:
            case (int) Brush_Control.T.DOOR_BLUE_V_TOP_CLOSED_TEST:
              if (blue_doors_open) br.solid = false;
              else br.solid = true;
              break;
            case (int) Brush_Control.T.DOOR_RED_H_TOP_CLOSED_TEST:
            case (int) Brush_Control.T.DOOR_RED_V_TOP_CLOSED_TEST:
              if (red_doors_open) br.solid = false;
              else br.solid = true;
              break;
            case (int) Brush_Control.T.DOOR_GREEN_H_TOP_CLOSED_TEST:
            case (int) Brush_Control.T.DOOR_GREEN_V_TOP_CLOSED_TEST:
              if (green_doors_open) br.solid = false;
              else br.solid = true;
              break;
            case (int) Brush_Control.T.DOOR_YELLOW_H_TOP_CLOSED_TEST:
            case (int) Brush_Control.T.DOOR_YELLOW_V_TOP_CLOSED_TEST:
              if (yellow_doors_open) br.solid = false;
              else br.solid = true;
              break;
            //case (int) Brush_Control.T.DOOR_PURPLE_H_TOP_CLOSED_TEST:
            //case (int) Brush_Control.T.DOOR_PURPLE_V_TOP_CLOSED_TEST:
            //  if (purple_doors_open) br.solid = false;
            //  else br.solid = true;
            //  break;
            }
          }
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    int point_in_character (int x, int y, int z)
      {
      int c = 0;
      int clip = -1;

      //while (clip == -1 && c < total_characters)
      while (clip == -1 && c < character_control.character.Count)
        {
        if (x >= character_control.character[c].x - (character_control.character[c].width / 2) && x <= character_control.character[c].x + (character_control.character[c].width / 2)
            && y >= character_control.character[c].y - (character_control.character[c].length / 2) && y <= character_control.character[c].y + (character_control.character[c].length / 2)
            && z >= character_control.character[c].z && z <= character_control.character[c].z + character_control.character[c].height)
          {
          clip = c;
          }
        c += 1;
        }
      return clip;
      }

    ////////////////////////////////////////////////////////////////////////////////

    int object_in_brush (int o)
      {
      int b = 0;
      int clip = -1;

      while (clip == -1 && b < brush_control.brush.Count)
        {
        if (object_control.obj[o].x + (object_control.obj[o].width / 2) >= brush_control.brush[b].x
            && object_control.obj[o].x - (object_control.obj[o].width / 2) <= brush_control.brush[b].x + brush_control.brush[b].width
            && object_control.obj[o].y + (object_control.obj[o].length / 2) >= brush_control.brush[b].y
            && object_control.obj[o].y - (object_control.obj[o].length / 2) <= brush_control.brush[b].y + brush_control.brush[b].length
            && object_control.obj[o].z + object_control.obj[o].height >= brush_control.brush[b].z
            && object_control.obj[o].z <= brush_control.brush[b].z + brush_control.brush[b].height)
          {
          if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.DOOR_RED_V_TOP_CLOSED_TEST && red_doors_open == true) clip = -1;
          else if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.DOOR_RED_H_TOP_CLOSED_TEST && red_doors_open == true) clip = -1;
          else if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.DOOR_GREEN_V_TOP_CLOSED_TEST && green_doors_open == true) clip = -1;
          else if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.DOOR_GREEN_H_TOP_CLOSED_TEST && green_doors_open == true) clip = -1;
          else if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.DOOR_YELLOW_V_TOP_CLOSED_TEST && yellow_doors_open == true) clip = -1;
          else if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.DOOR_YELLOW_H_TOP_CLOSED_TEST && yellow_doors_open == true) clip = -1;
          else if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.DOOR_BLUE_V_TOP_CLOSED_TEST && blue_doors_open == true) clip = -1;
          else if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.DOOR_BLUE_H_TOP_CLOSED_TEST && blue_doors_open == true) clip = -1;
          else clip = b;
          }
        b += 1;
        }
      return clip;
      }

    ////////////////////////////////////////////////////////////////////////////////

    int object_in_object (Object o)
      {
      return -1;
      }

    ////////////////////////////////////////////////////////////////////////////////

    int object_in_character (int o)
      {
      int clip = -1;

      //for (int c = 0; c < total_characters; c += 1)
      for (int c = 0; c < character_control.character.Count; c += 1)
        {
        if (character_control.active (c) && c != object_control.obj[o].source
            && character_control.character[c].x + (character_control.character[c].width / 2) > object_control.obj[o].x - (object_control.obj[o].width / 2)  // left of object
            && character_control.character[c].x - (character_control.character[c].width / 2) < object_control.obj[o].x + (object_control.obj[o].width / 2)  // right of object
            && character_control.character[c].y + character_control.character[c].length > object_control.obj[o].y  // south of object
            && character_control.character[c].y < object_control.obj[o].y + object_control.obj[o].length  // north of object
            && character_control.character[c].z + character_control.character[c].height > object_control.obj[o].z
            && character_control.character[c].z < object_control.obj[o].z + object_control.obj[o].height)
          {
          clip = c;
          if (object_control.obj[o].type == (int) Object_Control.O.HEALTH && object_control.obj[o].destroyed == false)
            {
            character_control.character[c].health += 20;
            particle_itemgrab (object_control.obj[o].x, object_control.obj[o].y, object_control.obj[o].z, 0);
//            soundBank.PlayCue ("pickup_glove_test");
            sound_pickup_glove.Play ();
            destroy_object (o);
            }
          else if (object_control.obj[o].type == (int) Object_Control.O.SHIRT_YELLOW && clip == PLAYER && object_control.obj[o].destroyed == false)
            {
            character_control.character[PLAYER].shirt_power = true;
            character_control.character[PLAYER].shirt = (int) Object_Control.O.SHIRT_YELLOW;// (int) Object_Control.O.SHIRT_YELLOW;
            particle_itemgrab (object_control.obj[o].x, object_control.obj[o].y, object_control.obj[o].z, 0);
//            soundBank.PlayCue ("pickup_glove_test");
            sound_pickup_glove.Play ();
            destroy_object (o);
            }
          else if (object_control.obj[o].type == (int) Object_Control.O.SHIRT_RED && clip == PLAYER && object_control.obj[o].destroyed == false)
            {
            character_control.character[PLAYER].shirt_fire = true;
            character_control.character[PLAYER].shirt = (int) Object_Control.O.SHIRT_RED;// "fire";
            particle_itemgrab (object_control.obj[o].x, object_control.obj[o].y, object_control.obj[o].z, 0);
//            soundBank.PlayCue ("pickup_glove_test");
            sound_pickup_glove.Play ();
            destroy_object (o);
            }
          else if (object_control.obj[o].type == (int) Object_Control.O.SHIRT_WHITE && clip == PLAYER && object_control.obj[o].destroyed == false)
            {
            character_control.character[PLAYER].shirt_ice = true;
            character_control.character[PLAYER].shirt = (int) Object_Control.O.SHIRT_WHITE;// "ice";
            particle_itemgrab (object_control.obj[o].x, object_control.obj[o].y, object_control.obj[o].z, 0);
//            soundBank.PlayCue ("pickup_glove_test");
            sound_pickup_glove.Play ();
            destroy_object (o);
            }
          else if (object_control.obj[o].type == (int) Object_Control.O.SHIRT_PURPLE && clip == PLAYER && object_control.obj[o].destroyed == false)
            {
            character_control.character[PLAYER].shirt_magnetic = true;
            character_control.character[PLAYER].shirt = (int) Object_Control.O.SHIRT_PURPLE;// "magnet";
            particle_itemgrab (object_control.obj[o].x, object_control.obj[o].y, object_control.obj[o].z, 0);
//            soundBank.PlayCue ("pickup_glove_test");
            sound_pickup_glove.Play ();
            destroy_object (o);
            }
          else if (object_control.obj[o].type == (int) Object_Control.O.SHIRT_BLUE && clip == PLAYER && object_control.obj[o].destroyed == false)
            {
            character_control.character[PLAYER].shirt_electric = true;
            character_control.character[PLAYER].shirt = (int) Object_Control.O.SHIRT_BLUE;// "electric";
            particle_itemgrab (object_control.obj[o].x, object_control.obj[o].y, object_control.obj[o].z, 0);
//            soundBank.PlayCue ("pickup_glove_test");
            sound_pickup_glove.Play ();
            destroy_object (o);
            }
          else if ((object_control.obj[o].type == (int) Object_Control.O.ROCK || object_control.obj[o].type == (int) Object_Control.O.ROCK_BROWN || object_control.obj[o].type == (int) Object_Control.O.ROCK_RED || object_control.obj[o].type == (int) Object_Control.O.ROCK_WHITE)
                    && clip != object_control.obj[o].source)
            {
            if (object_control.obj[o].velocity > 0) character_damage (c, 20, 4, 0, object_control.obj[o].x, object_control.obj[o].y, "impact", object_control.obj[o].source);
            }
          else if (object_control.obj[o].type == (int) Object_Control.O.KEYCARD && clip == PLAYER && object_control.obj[o].destroyed == false)
            {
            particle_itemgrab (object_control.obj[o].x, object_control.obj[o].y, object_control.obj[o].z, 0);
//            soundBank.PlayCue ("pickup_key_test");
            sound_pickup_key.Play ();
            destroy_object (o);
            }
          else if (object_control.obj[o].type == (int) Object_Control.O.COIN && clip == PLAYER && object_control.obj[o].destroyed == false)
            {
            character_control.character[c].coins += 1;
            particle_coingrab (object_control.obj[o].x, object_control.obj[o].y, object_control.obj[o].z);
            sound_pickup_coin.Play ();
            destroy_object (o);
            //obj[o].destroyed = true;
            }
          else if (object_control.obj[o].type == (int) Object_Control.O.SCRAP_METAL && object_control.obj[o].destroyed == false)
            {
            //character_control.character[c].metal += 1;
            particle_itemgrab (object_control.obj[o].x, object_control.obj[o].y, object_control.obj[o].z, 0);
            sound_pickup_glove.Play ();
            destroy_object (o);
            }
          else if (object_control.obj[o].type == (int) Object_Control.O.ENERGY && object_control.obj[o].destroyed == false)
            {
            //character_control.character[c].metal += 1;
            particle_itemgrab (object_control.obj[o].x, object_control.obj[o].y, object_control.obj[o].z, 0);
            sound_pickup_glove.Play ();
            destroy_object (o);
            }
          }
        }

      return clip;
      }

    ////////////////////////////////////////////////////////////////////////////////

    bool object_on_ground (Object o)
      {
      bool ground = false;
      for (int b = 0; b < brush_control.brush.Count; b++)
        {
        if (o.x + (o.width / 2) >= brush_control.brush[b].x && o.x - (o.width / 2) <= brush_control.brush[b].x + brush_control.brush[b].width &&
            o.y + (o.width / 2) >= brush_control.brush[b].y && o.y - (o.width / 2) <= brush_control.brush[b].y + brush_control.brush[b].length &&
            o.z >= brush_control.brush[b].z + brush_control.brush[b].height && o.z <= brush_control.brush[b].z + brush_control.brush[b].height + 1)
          ground = true;
        }
      return ground;
      }

    ////////////////////////////////////////////////////////////////////////////////

    double get_direction (double x1, double y1, double x2, double y2)
      {
      double dir_radians;
      double x_distance, y_distance;

      x_distance = x2 - x1;
      y_distance = y2 - y1;

      // get radians of direction
      if (x_distance > 0 && y_distance >= 0) dir_radians = Math.Atan (y_distance / x_distance);
      else if (x_distance > 0 && y_distance < 0) dir_radians = Math.Atan (y_distance / x_distance) + (2 * Math.PI);
      else if (x_distance < 0) dir_radians = Math.Atan (y_distance / x_distance) + Math.PI;
      else if (x_distance == 0 && y_distance > 0) dir_radians = MathHelper.ToRadians (90);//Math.PI / 2;
      else if (x_distance == 0 && y_distance < 0) dir_radians = MathHelper.ToRadians (270);//-1 * Math.PI / 2;
      else dir_radians = 0;  // x_distance = 0, y_distance = 0

      return dir_radians;
      }

    ////////////////////////////////////////////////////////////////////////////////

    void new_target_object (int c, int o)
      {
      character_control.character[c].target_type = "object";
      character_control.character[c].target = o;
      character_control.character[c].target_x = object_control.obj[o].x;
      character_control.character[c].target_y = object_control.obj[o].y;
      character_control.character[c].target_z = object_control.obj[o].z;
      character_control.character[c].subtarget_x = -1;
      character_control.character[c].subtarget_y = -1;
      character_control.character[c].subtarget_z = -1;
      }

    ////////////////////////////////////////////////////////////////////////////////

    bool point_near_point (int x1, int y1, int z1, int x2, int y2, int z2)
      {
      double x_distance, y_distance, z_distance, h_distance;

      x_distance = x1 - x2;
      y_distance = y1 - y2;
      z_distance = Math.Abs (z1 - z2);
      h_distance = Math.Sqrt ((x_distance * x_distance) + (y_distance * y_distance));

      if (h_distance < character_control.reach_distance * 2 && z_distance < character_control.reach_distance * 2) return true;
      else return false;
      }

    ////////////////////////////////////////////////////////////////////////////////

    bool character_near_object (int c, int o)
      {
      double x_distance, y_distance, z_distance, h_distance;

      x_distance = character_control.character[c].dx - object_control.obj[o].dx;
      y_distance = character_control.character[c].dy - object_control.obj[o].dy;
      z_distance = Math.Abs (character_control.character[c].dz - object_control.obj[o].dz);
      h_distance = Math.Sqrt ((x_distance * x_distance) + (y_distance * y_distance));

      if (h_distance < character_control.reach_distance * 2 && z_distance < character_control.reach_distance * 2) return true;
      else
      return false;
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

    double distance3d (double x1, double y1, double z1, double x2, double y2, double z2)
      {
      // formula correct?

      double x, y, z;
      double d = 0;

      x = x2 - x1;
      y = y2 - y1;
      z = z2 - z1;

      // sqrt[(x1-x2)2 + (y1-y2)2 + (z1-z2)2]

      d = Math.Sqrt ((x * x) + (y * y) + (z * z));

      if (d < 0) d = d * -1;

      return d;
      }

    ////////////////////////////////////////////////////////////////////////////////

    int y_draw_coordinate (int world_y, int world_z)
      {
      return Convert.ToInt32 ((screen.height - world_y) - (world_z * parallax) + screen.scroll_y);
      }

    ////////////////////////////////////////////////////////////////////////////////

    bool brush_on_screen (int b)
      {
      bool on_screen = true;

      // larger pieces do not conform to formula
      if (brush_control.brush[b].top_texture_number > (int) Brush_Control.T.SINGLE_PIECE) on_screen = true;
      else if (brush_control.brush[b].x + brush_control.brush[b].width + screen.scroll_x < 0) on_screen = false;
      else if (brush_control.brush[b].x + screen.scroll_x > screen.width) on_screen = false;
      else if ((screen.height - brush_control.brush[b].y) - (brush_control.brush[b].z * parallax) - (brush_control.brush[b].height * parallax) + screen.scroll_y + tilesize < 0) on_screen = false;
      else if ((screen.height - brush_control.brush[b].y - brush_control.brush[b].length) - (brush_control.brush[b].z * parallax) - (brush_control.brush[b].height * parallax) + screen.scroll_y > screen.height) on_screen = false;

      return on_screen;
      }

    ////////////////////////////////////////////////////////////////////////////////

    int free_particle ()  // finds the first particle effect slot not being used right now
      {
      int p = 0;
      while (p < max_effects && particle_effect[p].active == true) p++;
      if (p >= max_effects) p = 0;
      return p;
      }

    ////////////////////////////////////////////////////////////////////////////////

    void Update_Particles ()  // move around all the active effects
      {
      int e, p;
      int b_clip, c_clip;

      // for each active particle effect
      for (e = 0; e < max_effects; e += 1)
        {
        if (particle_effect[e].active == true)
          {
          particle_effect[e].update ();  // move particles around

          // for each live particle in that effect
          for (p = 0; p < 100; p += 1)  // 100 = Max_Particles, from ParticleEffect.cs
            {
            // map boundaries
            if (particle_effect[e].particle[p].alive == true)
              {
              if (particle_effect[e].particle[p].x < 0) particle_effect[e].particle[p].alive = false;
              else if (particle_effect[e].particle[p].x > map_width) particle_effect[e].particle[p].alive = false;
              else if (particle_effect[e].particle[p].y < 0) particle_effect[e].particle[p].alive = false;
              else if (particle_effect[e].particle[p].y > map_length) particle_effect[e].particle[p].alive = false;
              else if (particle_effect[e].particle[p].z < 0) particle_effect[e].particle[p].alive = false;
              else if (particle_effect[e].particle[p].z > map_height) particle_effect[e].particle[p].alive = false;
              }

            // collision
            if (particle_effect[e].particle[p].alive == true)
              {
              b_clip = brush_control.point_in_brush (particle_effect[e].particle[p].x, particle_effect[e].particle[p].y, particle_effect[e].particle[p].z, true, true);
              c_clip = point_in_character (particle_effect[e].particle[p].x, particle_effect[e].particle[p].y, particle_effect[e].particle[p].z);

              if (b_clip > -1)
                {
                particle_effect[e].particle[p].alive = false;
                if (particle_effect[e].particle[p].type == "fire")
                  {
                  if (brush_control.brush[b_clip].top_texture_number == (int) Brush_Control.T.BOX_WOOD_TEST)
                    {
                    // set wood on fire
                    }
                  if (brush_control.brush[b_clip].top_texture_number == (int) Brush_Control.T.BOX_METAL_TEST)
                    {
                    // heat up the metal
                    if (brush_control.brush[b_clip].temperature < 170 && rnd.Next (0, 6) == 0) brush_control.brush[b_clip].temperature += 1;
                    }
                  }
                }
              if (c_clip > -1 && character_control.active (c_clip))
                {
                if (particle_effect[e].particle[p].type == "fire")
                  {
                  if (particle_effect[e].particle[p].source_type == "character" && particle_effect[e].particle[p].source == c_clip) { }  // particle touched the guy who made it, nothing happens
                  else  // anybody else, counts as collision
                    {
                    // probability of damage by each individual flame
                    if (rnd.Next (0, 4) == 0 && character_control.character[c_clip].shirt != (int) Object_Control.O.SHIRT_RED)//"fire")
                      {
                      //character_damage (c_clip, 2, 0, 0, particle_effect[e].particle[p].x, particle_effect[e].particle[p].y, "fire", -1);
                      if (character_control.character[c_clip].on_fire == 0 && particle_effect[e].particle[p].source_type == "character" && particle_effect[e].particle[p].source == c_clip)
                        character_control.character[c_clip].on_fire = 0;
                      else character_control.character[c_clip].on_fire = 300;
                      }
                    particle_effect[e].particle[p].alive = false;
                    }
                  }
                }
              }
            }
          }
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void particle_coinsparkle (int x, int y, int z)
      {
      particle_effect[free_particle ()].create (light_sprite[(int) L.white], 1, x, y, z, 0, 0, 0, 0, 0, 0, 0, .5f, -.02f, 0, .06, .01, "light", "none", -1);
      }

    ////////////////////////////////////////////////////////////////////////////////

    void particle_coingrab (int x, int y, int z)
      {
      particle_effect[free_particle ()].create (effect_dollars, 1, x, y, z, 0, 0, 0, 0, 1.5, 0, 0, .6f, -.01f, 0, .6, .02, "light", "none", -1);
      particle_effect[free_particle ()].create (light_sprite[(int) L.yellow], 1, x, y, z, 0, 0, 0, 0, 0, 0, 0, .5f, -.01f, 0, .2, .02, "light", "none", -1);
      }

    ////////////////////////////////////////////////////////////////////////////////

    void particle_itemgrab (int x, int y, int z, int direction)
      {
      particle_effect[free_particle ()].create (light_sprite[(int) L.yellow], 1, x, y, z, direction, 0, 0, 0, 0, 0, 0, .5f, -.01f, 0, .2, .02, "light", "none", -1);
      }

    ////////////////////////////////////////////////////////////////////////////////

    void particle_superpunch (int x, int y, int z, int direction)
      {
      particle_effect[free_particle ()].create (light_sprite[(int) L.yellow], 1, x, y, z, direction, 0, 0, 0, 0, 0, 0, .5f, -.01f, 0, 1, 0, "light", "none", -1);
      }

    ////////////////////////////////////////////////////////////////////////////////

    void particle_steam (int x, int y, int z, int direction)
      {
      particle_effect[free_particle ()].create (effect_cold_energy, 1, x, y, z, 0, 3, 0, 0, .4, .1, 0, .3f, -.001f, 0, 1.75, .04, "air", "none", -1);
      }

    ////////////////////////////////////////////////////////////////////////////////

    void particle_flames (int x, int y, int z, string source_type, int source)
      {
      particle_effect[free_particle ()].create (effect_smoke, 1, x, y, z, 0, 0, 0, 0, .7, .25, 0, .25f, -.001f, 0, 1.5, .02, "smoke", source_type, source);

      if (rnd.Next (0, 3) == 0) particle_effect[free_particle ()].create (effect_flame_red, 1, x + rnd.Next (-8, 8), y + rnd.Next (-8, 8), z + rnd.Next (-8, 8), 0, 360, .4, .1, .7, .2, 0, .6f, -.015f, 0, 2.5, .01, "fire", source_type, source);// break;
      if (rnd.Next (0, 2) == 0) particle_effect[free_particle ()].create (effect_flame_orange, 1, x + rnd.Next (-8, 8), y + rnd.Next (-8, 8), z + rnd.Next (-8, 8), 0, 360, .4, .1, .7, .2, 0, .6f, -.015f, 0, 2.25, .01, "fire", source_type, source);// break;
      if (rnd.Next (0, 2) == 0) particle_effect[free_particle ()].create (effect_flame_yellow, 1, x + rnd.Next (-8, 8), y + rnd.Next (-8, 8), z + rnd.Next (-8, 8), 0, 360, .4, .1, .7, .2, 0, .6f, -.015f, 0, 2, .01, "fire", source_type, source);// break;
      if (rnd.Next (0, 3) == 0) particle_effect[free_particle ()].create (effect_flame_white, 1, x + rnd.Next (-8, 8), y + rnd.Next (-8, 8), z + rnd.Next (-8, 8), 0, 360, .4, .1, .7, .2, 0, .6f, -.015f, 0, 1.75, .01, "fire", source_type, source);// break;
      }

    ////////////////////////////////////////////////////////////////////////////////

    void particle_flamethrower (int x, int y, int z, int direction, string source_type, int source)
      {
      particle_effect[free_particle ()].create (effect_smoke, 1, x, y, z, direction, 3, 1, .5, .7, .25, 0, .25f, -.001f, 0, 1, .02, "smoke", source_type, source);
      particle_effect[free_particle ()].create (effect_flame_red, 2, x, y, z, direction, 3, 2, .5, 0, 0, 0, .75f, -.005f, 0, 1, .01, "fire", source_type, source);
      particle_effect[free_particle ()].create (effect_flame_orange, 3, x, y, z, direction, 3, 2, .5, 0, 0, 0, .75f, -.005f, 0, 1, .01, "fire", source_type, source);
      particle_effect[free_particle ()].create (effect_flame_yellow, 4, x, y, z, direction, 3, 2, .5, 0, 0, 0, .75f, -.005f, 0, 1, .01, "fire", source_type, source);
      particle_effect[free_particle ()].create (effect_flame_white, 5, x, y, z, direction, 3, 2, .5, 0, 0, 0, .75f, -.005f, 0, 1, .01, "fire", source_type, source);
      }

    ////////////////////////////////////////////////////////////////////////////////

    void particle_incinerator (int x, int y, int z, int direction, string source_type, int source)
      {
      if (rnd.Next (0, 3) == 0) particle_effect[free_particle ()].create (effect_smoke, 1, x, y, z, direction, 3, 1, .5, .7, .25, 0, .25f, -.001f, 0, 1, .02, "smoke", source_type, source);
      if (rnd.Next (0, 4) == 0) particle_effect[free_particle ()].create (effect_flame_red, 2, x, y, z, direction, 3, 2, .5, 0, 0, 0, .75f, -.005f, 0, 1.5, .01, "fire", source_type, source);
      if (rnd.Next (0, 4) == 0) particle_effect[free_particle ()].create (effect_flame_orange, 3, x, y, z, direction, 3, 2, .5, 0, 0, 0, .75f, -.005f, 0, 1.5, .01, "fire", source_type, source);
      if (rnd.Next (0, 4) == 0) particle_effect[free_particle ()].create (effect_flame_yellow, 4, x, y, z, direction, 3, 2, .5, 0, 0, 0, .75f, -.005f, 0, 1.5, .01, "fire", source_type, source);
      if (rnd.Next (0, 3) == 0) particle_effect[free_particle ()].create (effect_flame_white, 5, x, y, z, direction, 3, 2, .5, 0, 0, 0, .75f, -.005f, 0, 1.5, .01, "fire", source_type, source);
      }

    ////////////////////////////////////////////////////////////////////////////////

    void particle_freeze_ray (int x, int y, int z, int direction, string source_type, int source)
      {
      int new_x, new_y, new_z;
      int variance = 12;  // radius of beam

      //cold energy light
      particle_effect[free_particle ()].create (effect_cold_energy, 2, x, y, z, direction, 2, 6, 0, 0, 0, -.02, .5f, -.005f, 0, 1, 0, "freeze", source_type, source);

      //slightly randomize snow placement
      new_x = rnd.Next (x - variance, x + variance);
      new_y = rnd.Next (y - variance, y + variance);
      new_z = rnd.Next (z - variance, z + variance);
      particle_effect[free_particle ()].create (effect_snowflake, 2, new_x, new_y, new_z, direction, 2, 6, 2, 0, 0, -.02, 1f, -.01f, 0, 1, 0, "freeze", source_type, source);

      //multiple flakes
      new_x = rnd.Next (x - variance, x + variance);
      new_y = rnd.Next (y - variance, y + variance);
      new_z = rnd.Next (z - variance, z + variance);
      particle_effect[free_particle ()].create (effect_snowflake, 2, new_x, new_y, new_z, direction, 2, 6, 2, 0, 0, -.02, 1f, -.01f, 0, 1, 0, "freeze", source_type, source);
      }

    ////////////////////////////////////////////////////////////////////////////////

    void particle_green_tripwire (int x, int y, int z, int direction)
      {
      particle_effect[free_particle ()].create (pixel_green, 1, x, y, z, direction, 0, .5, 0, 0, 0, .01, .7f, -.01f, 0, 2, 0, "light", "none", -1);
      }

    ////////////////////////////////////////////////////////////////////////////////

    void color_flasher (Texture2D sprite)
      {
      color_flash_sprite = sprite;
      color_flash.reset ();
      color_flash.init (32, 0, 2);
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

    void fadeout () { fader.init (0, 255, 24); }
    void fadein () { fader.init (255, 0, 24); }

    ////////////////////////////////////////////////////////////////////////////////

    void Fade_Control (GameTime game_time)
      {
      if (fader.activated == true) // fade has been started
        {
        fader.fade (spriteBatch, solid_black, Vector2.Zero);

        // if fade in completed
        //if (fader.running == false && transition == NONE)
        //{
        //if (GameState == INTRO)
        //  {
        //  wait.init (60); // 2 second pause for ng logo
        //  transition = MENU;
        //  fader.reset ();
        //  fadeout ();
        //  }
        //if (GameState == MENU) passed_intro = true;
        //}

        // if fade out completed
        //else if (fader.running == false && transition != NONE)
        //{
        //GameState = transition;
        //if (transition == MENU && passed_intro == false) sound_blockhead = true;
        //else if (transition == GAME && newgame == true) New_Game (game_time);
        //transition = NONE;
        //fader.reset ();
        //fadein ();
        //}
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    protected override void Draw (GameTime game_time)
      {
      Vector2 brush_screen_draw;
      Vector2 v_draw, v_draw2, v_origin;//, v_subtarget;
      Rectangle r_source = Rectangle.Empty;
      Rectangle r_draw;
      int tx, ty, tz;
      int b, b1, b2, b3, b4, b5, b6, c, f, l, o, p;
      bool endloop, endloop2;
      int distance = 0;
      float opacity = 1f;
      float temp_fade;
      double shadow_scale;
      Draw_Slot draw_slot;
      int min_y, max_y, min_z, max_z;
      bool all_items_drawn;
      //int rotation = 0;
      int wall_to_north, wall_to_south, wall_to_west, wall_to_east;
      int floor_to_north, floor_to_south, floor_to_west, floor_to_east;
      bool shadow_north, shadow_south, shadow_west, shadow_east;
      bool floor_visible;
      int bg_start_x, bg_start_y;
      bool draw_brush;
      //int min_distance;
      string debug_string = string.Empty;

    //MouseState mouse_current = Mouse.GetState ();
    

      GraphicsDevice.Clear (Color.Black);

      if (lighting_engine == 2)
        {
        GraphicsDevice.SetRenderTarget (light_buffer);
        GraphicsDevice.Clear (ambient_light);
        spriteBatch.Begin (SpriteSortMode.Deferred, BlendState.Additive);

        for (l = 0; l < total_lights; l += 1)
          {
          if (light[l].on == true)
            {
            v_origin.X = light_sprite[(int) L.white].Width / 2;
            v_origin.Y = light_sprite[(int) L.white].Height / 2;
            v_draw.X = light[l].x + screen.scroll_x;
            v_draw.Y = y_draw_coordinate (light[l].y, light[l].z - tilesize);
            //spriteBatch.Draw (light_sprite[(int) L.white], v_draw, light[l].c);
            //spriteBatch.Draw (light_sprite[(int) L.white], v_draw, null, Color.White * .5f, 0f, v_origin, light[l].scale, SpriteEffects.None, 0);
            //if (light[l].c == Color.White) spriteBatch.Draw (light_sprite[(int) L.white], v_draw, null, light[l].c * .55f, 0f, v_origin, light[l].scale, SpriteEffects.None, 0);
            //else spriteBatch.Draw (light_sprite[(int) L.white], v_draw, null, light[l].c * .8f, 0f, v_origin, light[l].scale, SpriteEffects.None, 0);
            spriteBatch.Draw (light_sprite[(int) L.white], v_draw, null, light[l].c * light[l].alpha, 0f, v_origin, light[l].scale, SpriteEffects.None, 0);
            }
          }

        spriteBatch.End ();

        GraphicsDevice.SetRenderTarget (null);
        GraphicsDevice.Clear (Color.Black);
        }

      spriteBatch.Begin (SpriteSortMode.Immediate, BlendState.AlphaBlend);

      if (game_state == TITLE)
        {
        v_draw.X = (screen.width / 2) - (title_screen_test.Width / 2);
        v_draw.Y = (screen.height / 2) - (title_screen_test.Height / 2);
        spriteBatch.Draw (title_screen_test, v_draw, Color.White);
        }

      else if (game_state == GAME || game_state == CREATION)
        {
        // background layer 1
        for (bg_start_x = screen.bg1_scroll_x; bg_start_x + map.background.Width > 0; bg_start_x -= map.background.Width) { }
        for (bg_start_y = screen.bg1_scroll_y; bg_start_y + map.background.Height > 0; bg_start_y -= map.background.Height) { }
        for (v_draw.Y = bg_start_y; v_draw.Y < screen.height; v_draw.Y += map.background.Height)
          for (v_draw.X = bg_start_x; v_draw.X < screen.width; v_draw.X += map.background.Width)
            spriteBatch.Draw (map.background, v_draw, Color.White);

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


        // CREATE ORDERED LIST FOR DRAWING BRUSHES, OBJECTS & CHARACTERS

        // reset draw flags
        for (b = 0; b < brush_control.brush.Count; b += 1) brush_control.brush[b].drawn = false;
        foreach (Fixture fix in fixture_control.fixture) fix.drawn = false;
        for (o = 0; o < object_control.obj.Count; o += 1) object_control.obj[o].drawn = false;
        foreach (Character ch in character_control.character) ch.drawn = false;

        draw_slot.id = -1;
        draw_slot.type = "brush_control.brush";

        if (draw_order == 1)
          {
          // lowest grid elevation from back to front, then move up each layer

          l = 0;
          all_items_drawn = false;
          max_y = map_max_length;
          min_y = max_y - tilesize;
          min_z = 0;
          max_z = tilesize - 1;

          while (all_items_drawn == false && max_z < map_max_height && l < total_draw_slots)
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
                draw_slot.type = "brush_control.brush";
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
              draw_list[l].id = draw_slot.id;
              draw_list[l].type = draw_slot.type;
              if (draw_slot.type == "brush_control.brush") brush_control.brush[draw_slot.id].drawn = true;
              else if (draw_slot.type == "fixture") fixture_control.fixture[draw_slot.id].drawn = true;
              else if (draw_slot.type == "object") object_control.obj[draw_slot.id].drawn = true;
              else if (draw_slot.type == "character") character_control.character[draw_slot.id].drawn = true;
              l += 1;
              }
            else  // nothing left in this layer
              {
              min_z += tilesize;
              max_z += tilesize;
              all_items_drawn = false;
              }
            }
          }

        else if (draw_order == 2)
          {
          // backmost row from bottom up, then forward each row

          l = 0;
          all_items_drawn = false;
          max_y = map_max_length;        // the back row
          min_y = max_y - tilesize - 1;

          while (all_items_drawn == false && min_y >= 0 && l < total_draw_slots)
            {
            all_items_drawn = true;
            draw_slot.id = -1;
            max_z = map_max_height;
            min_z = 0;

            for (b = 0; b < brush_control.brush.Count; b += 1)
              {
              if (brush_control.brush[b].drawn == false && brush_control.brush[b].y >= min_y && brush_control.brush[b].y <= max_y
                  && brush_control.brush[b].z >= min_z && brush_control.brush[b].z <= max_z)
                {
                all_items_drawn = false;
                draw_slot.id = b;
                draw_slot.type = "brush_control.brush";
                max_z = brush_control.brush[b].z;
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
                max_z = fixture_control.fixture[f].z;
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
                max_z = object_control.obj[o].z;
                }
              }
            for (c = 0; c < character_control.character.Count; c += 1)
              {
              if (character_control.character[c].drawn == false && character_control.character[c].y >= min_y && character_control.character[c].y <= max_y
                && character_control.character[c].z >= min_z && character_control.character[c].z <= max_z)
                  //&& character_control.character[c].z + character_control.character[c].height >= min_z && character_control.character[c].z + character_control.character[c].height <= max_z)
                {
                all_items_drawn = false;
                draw_slot.id = c;
                draw_slot.type = "character";
                max_z = character_control.character[c].z;
                }
              }
            if (draw_slot.id > -1)  // found item to draw
              {
              draw_list[l].id = draw_slot.id;
              draw_list[l].type = draw_slot.type;
              if (draw_slot.type == "brush_control.brush") brush_control.brush[draw_slot.id].drawn = true;
              else if (draw_slot.type == "fixture") fixture_control.fixture[draw_slot.id].drawn = true;
              else if (draw_slot.type == "object") object_control.obj[draw_slot.id].drawn = true;
              else if (draw_slot.type == "character") character_control.character[draw_slot.id].drawn = true;
              l += 1;
              }
            else  // nothing left in this row
              {
              min_y -= tilesize;
              max_y -= tilesize;
              all_items_drawn = false;
              }
            }
          }

        else if (draw_order == 3)
          {
          // lowest grid elevation from back to front, then move up each layer
          // layers are half tilesize

          l = 0;
          all_items_drawn = false;
          //max_y = map_max_length;
          //min_y = max_y - tilesize;
          min_z = 0;
          max_z = (tilesize / 2) - 1;

          while (all_items_drawn == false && max_z < map_max_height && l < total_draw_slots)
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
                draw_slot.type = "brush_control.brush";
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
              draw_list[l].id = draw_slot.id;
              draw_list[l].type = draw_slot.type;
              if (draw_slot.type == "brush_control.brush") brush_control.brush[draw_slot.id].drawn = true;
              else if (draw_slot.type == "fixture") fixture_control.fixture[draw_slot.id].drawn = true;
              else if (draw_slot.type == "object") object_control.obj[draw_slot.id].drawn = true;
              else if (draw_slot.type == "character") character_control.character[draw_slot.id].drawn = true;
              l += 1;
              }
            else  // nothing left in this layer
              {
              min_z += tilesize / 2;
              max_z += tilesize / 2;
              all_items_drawn = false;
              }
            }
          }

        for (l = 0; l < total_draw_slots; l += 1)
          {
          if (draw_list[l].type == "brush_control.brush" && brush_on_screen (draw_list[l].id))
            {
            b = draw_list[l].id;

            brush_screen_draw.X = brush_control.brush[b].x + screen.scroll_x;
            brush_screen_draw.Y = Convert.ToInt32((screen.height - brush_control.brush[b].y - brush_control.brush[b].length) - (brush_control.brush[b].z * parallax) - (brush_control.brush[b].height * parallax) + screen.scroll_y);

            // background brush_control.texture
            if (brush_control.brush[b].background_texture > -1) spriteBatch.Draw (brush_control.texture[brush_control.brush[b].background_texture], brush_screen_draw, new Rectangle (0, 0, tilesize, tilesize), Color.White);

            //if (brush_control.brush[b].top_texture_number != INVISIBLE_WALL)
            //{
            if (draw_walls == false && (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.BRICK_RED_TEST || brush_control.brush[b].top_texture_number == (int) Brush_Control.T.BRICK_GREY_TEST
                  || brush_control.brush[b].top_texture_number == (int) Brush_Control.T.METAL_MINT_TOP_TEST || brush_control.brush[b].top_texture_number == (int) Brush_Control.T.METAL_BLUE_TOP_TEST
                  || brush_control.brush[b].top_texture_number == (int) Brush_Control.T.DRYWALL_YELLOW_TOP_TEST || brush_control.brush[b].top_texture_number == (int) Brush_Control.T.METAL_BLACK_TEST)) { }  // draw nothing
            else if (draw_boxes == false && brush_control.brush[b].top_texture_number == (int) Brush_Control.T.BOX_WOOD_TEST) { }  // draw nothing

            // animated surfaces (doors, buttons, etc)
            //else if (brush_control.brush[b].top_texture_number == DOOR_TEST_H_GREEN && green_doors_open == true) spriteBatch.Draw (brush_control.texture[DOOR_TEST_H_GREEN_OPEN], brush_screen_draw, Color.White);
            //else if (brush_control.brush[b].top_texture_number == DOOR_TEST_H_YELLOW && yellow_doors_open == true) spriteBatch.Draw (brush_control.texture[DOOR_TEST_H_YELLOW_OPEN], brush_screen_draw, Color.White);
            else if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.SWITCH_GREEN_TEST && green_doors_open == true) spriteBatch.Draw (brush_control.texture[(int) Brush_Control.T.SWITCH_GREEN_DOWN_TEST], brush_screen_draw, Color.White);
            else if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.SWITCH_RED_TEST && red_doors_open == true) spriteBatch.Draw (brush_control.texture[(int) Brush_Control.T.SWITCH_RED_DOWN_TEST], brush_screen_draw, Color.White);
            else if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.SWITCH_YELLOW_TEST && yellow_doors_open == true) spriteBatch.Draw (brush_control.texture[(int) Brush_Control.T.SWITCH_YELLOW_DOWN_TEST], brush_screen_draw, Color.White);
            else if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.SWITCH_BLUE_TEST && blue_doors_open == true) spriteBatch.Draw (brush_control.texture[(int) Brush_Control.T.SWITCH_BLUE_DOWN_TEST], brush_screen_draw, Color.White);

              /*
              // transparency test
              else if (brush_control.brush[b].top_texture_number == WALL_TEST_YELLOW
                && character_control.character[PLAYER].x > brush_control.brush[b].x - tilesize && character_control.character[PLAYER].x < brush_control.brush[b].x + (tilesize * 2)
                && character_control.character[PLAYER].y > brush_control.brush[b].y + tilesize && character_control.character[PLAYER].y < brush_control.brush[b].y + (tilesize * 2))
              //&& character_control.character[PLAYER].z > brush_control.brush[b].z - (tilesize / 2) && character_control.character[PLAYER].z < brush_control.brush[b].z + (tilesize * 1.5))
                {
                r_source.X = 0;
                r_source.Y = 0;
                r_source.Width = brush_control.texture[brush_control.brush[b].top_texture_number].Width;
                r_source.Height = tilesize / 2;// brush_control.texture[brush_control.brush[b].top_texture_number].Height / 2;
                
                r_draw.X = Convert.ToInt32 (v_draw.X);
                r_draw.Y = Convert.ToInt32 (v_draw.Y);
                r_draw.Width = brush_control.texture[brush_control.brush[b].top_texture_number].Width;
                r_draw.Height = r_source.Height;
                spriteBatch.Draw (brush_control.texture[brush_control.brush[b].top_texture_number], r_draw, r_source, Color.White * .75f);

                r_source.X = 0;
                r_source.Y = tilesize / 2;// brush_control.texture[brush_control.brush[b].top_texture_number].Height / 2;
                r_source.Width = brush_control.texture[brush_control.brush[b].top_texture_number].Width;
                r_source.Height = brush_control.texture[brush_control.brush[b].top_texture_number].Height - (tilesize / 2);
                
                r_draw.X = Convert.ToInt32 (v_draw.X);
                r_draw.Y = Convert.ToInt32 (v_draw.Y + (tilesize / 2));//(brush_control.texture[brush_control.brush[b].top_texture_number].Height / 2));
                r_draw.Width = brush_control.texture[brush_control.brush[b].top_texture_number].Width;
                r_draw.Height = r_source.Height;
                spriteBatch.Draw (brush_control.texture[brush_control.brush[b].top_texture_number], r_draw, r_source, Color.White);
                }
              */

            // large, one-piece units do not get tiled
            //else if (brush_control.brush[b].top_texture_number == BIG_MACHINE_TEST || brush_control.brush[b].top_texture_number == WARNING_SIGN_TEST1)
            else if (brush_control.brush[b].top_texture_number > (int) Brush_Control.T.SINGLE_PIECE && brush_control.brush[b].top_texture_number != (int) Brush_Control.T.INVISIBLE_WALL)
              {
              spriteBatch.Draw (brush_control.texture[brush_control.brush[b].top_texture_number], brush_screen_draw, Color.White);
              }

            // normal brushes use brush_control.texture sheets to create seamless, tiled walls and floors
            else
              {
              // bottom of brush_control.brush (certain transparent brushes only)
              if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.BOX_ICE_TEST)
                {
                r_source.X = brush_control.brush[b].top_texture_offset_x;
                r_source.Y = brush_control.brush[b].top_texture_offset_y;
                r_source.Width = tilesize;
                r_source.Height = tilesize;

                r_draw.X = Convert.ToInt32 (brush_screen_draw.X);
                r_draw.Y = Convert.ToInt32 ((brush_screen_draw.Y) + (tilesize * parallax));
                r_draw.Width = tilesize;
                r_draw.Height = tilesize;
                spriteBatch.Draw (brush_control.texture[brush_control.brush[b].top_texture_number], r_draw, r_source, Color.White * .9f);
                }

              // top of brush_control.brush

              // only draw if visible (not completely covered by a brush_control.brush above, unless that brush_control.brush is transparent)
              draw_brush = false;
              if (brush_control.point_in_brush (brush_control.brush[b].x + (tilesize / 2), brush_control.brush[b].y + (tilesize / 2), brush_control.brush[b].z + brush_control.brush[b].height + 1, true, false) == -1) draw_brush = true;
              else if (brush_control.point_in_brush (brush_control.brush[b].x + (tilesize / 2), brush_control.brush[b].y + (tilesize / 2), brush_control.brush[b].z + brush_control.brush[b].height + 1, true, false) > -1
                       && brush_control.brush[brush_control.point_in_brush (brush_control.brush[b].x + (tilesize / 2), brush_control.brush[b].y + (tilesize / 2), brush_control.brush[b].z + brush_control.brush[b].height + 1, true, false)].transparent == true) draw_brush = true;
              else if (brush_control.point_in_brush (brush_control.brush[b].x, brush_control.brush[b].y + (tilesize / 2), brush_control.brush[b].z + brush_control.brush[b].height + 1, true, false) == -1) draw_brush = true;
              else if (brush_control.point_in_brush (brush_control.brush[b].x, brush_control.brush[b].y + (tilesize / 2), brush_control.brush[b].z + brush_control.brush[b].height + 1, true, false) > -1
                       && brush_control.brush[brush_control.point_in_brush (brush_control.brush[b].x, brush_control.brush[b].y + (tilesize / 2), brush_control.brush[b].z + brush_control.brush[b].height + 1, true, false)].transparent == true) draw_brush = true;
              else if (brush_control.point_in_brush (brush_control.brush[b].x + tilesize - 1, brush_control.brush[b].y + (tilesize / 2), brush_control.brush[b].z + brush_control.brush[b].height + 1, true, false) == -1) draw_brush = true;
              else if (brush_control.point_in_brush (brush_control.brush[b].x + tilesize - 1, brush_control.brush[b].y + (tilesize / 2), brush_control.brush[b].z + brush_control.brush[b].height + 1, true, false) > -1
                       && brush_control.brush[brush_control.point_in_brush (brush_control.brush[b].x + tilesize - 1, brush_control.brush[b].y + (tilesize / 2), brush_control.brush[b].z + brush_control.brush[b].height + 1, true, false)].transparent == true) draw_brush = true;
              else if (brush_control.point_in_brush (brush_control.brush[b].x + (tilesize / 2), brush_control.brush[b].y + tilesize - 1, brush_control.brush[b].z + brush_control.brush[b].height + 1, true, false) == -1) draw_brush = true;
              else if (brush_control.point_in_brush (brush_control.brush[b].x + (tilesize / 2), brush_control.brush[b].y + tilesize - 1, brush_control.brush[b].z + brush_control.brush[b].height + 1, true, false) > -1
                       && brush_control.brush[brush_control.point_in_brush (brush_control.brush[b].x + (tilesize / 2), brush_control.brush[b].y + tilesize - 1, brush_control.brush[b].z + brush_control.brush[b].height + 1, true, false)].transparent == true) draw_brush = true;
              else if (brush_control.point_in_brush (brush_control.brush[b].x + (tilesize / 2), brush_control.brush[b].y, brush_control.brush[b].z + brush_control.brush[b].height + 1, true, false) == -1) draw_brush = true;
              else if (brush_control.point_in_brush (brush_control.brush[b].x + (tilesize / 2), brush_control.brush[b].y, brush_control.brush[b].z + brush_control.brush[b].height + 1, true, false) > -1
                       && brush_control.brush[brush_control.point_in_brush (brush_control.brush[b].x + (tilesize / 2), brush_control.brush[b].y, brush_control.brush[b].z + brush_control.brush[b].height + 1, true, false)].transparent == true) draw_brush = true;
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
                if (brush_control.brush[b].background_texture > -1) spriteBatch.Draw (brush_control.texture[brush_control.brush[b].background_texture], r_draw, new Rectangle (0, 0, tilesize, tilesize), Color.White);

                if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.INVISIBLE_WALL) { }
                else if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.DOOR_RED_V_TOP_CLOSED_TEST && red_doors_open == true) spriteBatch.Draw (brush_control.texture[(int) Brush_Control.T.DOOR_RED_V_TOP_OPEN_TEST], r_draw, r_source, Color.White);
                else if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.DOOR_RED_H_TOP_CLOSED_TEST && red_doors_open == true) spriteBatch.Draw (brush_control.texture[(int) Brush_Control.T.DOOR_RED_H_TOP_OPEN_TEST], r_draw, r_source, Color.White);
                else if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.DOOR_YELLOW_V_TOP_CLOSED_TEST && yellow_doors_open == true) spriteBatch.Draw (brush_control.texture[(int) Brush_Control.T.DOOR_YELLOW_V_TOP_OPEN_TEST], r_draw, r_source, Color.White);
                else if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.DOOR_YELLOW_H_TOP_CLOSED_TEST && yellow_doors_open == true) spriteBatch.Draw (brush_control.texture[(int) Brush_Control.T.DOOR_YELLOW_H_TOP_OPEN_TEST], r_draw, r_source, Color.White);
                else if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.DOOR_GREEN_V_TOP_CLOSED_TEST && green_doors_open == true) spriteBatch.Draw (brush_control.texture[(int) Brush_Control.T.DOOR_GREEN_V_TOP_OPEN_TEST], r_draw, r_source, Color.White);
                else if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.DOOR_GREEN_H_TOP_CLOSED_TEST && green_doors_open == true) spriteBatch.Draw (brush_control.texture[(int) Brush_Control.T.DOOR_GREEN_H_TOP_OPEN_TEST], r_draw, r_source, Color.White);
                else if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.DOOR_BLUE_V_TOP_CLOSED_TEST && blue_doors_open == true) spriteBatch.Draw (brush_control.texture[(int) Brush_Control.T.DOOR_BLUE_V_TOP_OPEN_TEST], r_draw, r_source, Color.White);
                else if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.DOOR_BLUE_H_TOP_CLOSED_TEST && blue_doors_open == true) spriteBatch.Draw (brush_control.texture[(int) Brush_Control.T.DOOR_BLUE_H_TOP_OPEN_TEST], r_draw, r_source, Color.White);
                else if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.EXIT_RED_V_TOP_CLOSED_TEST && red_doors_open == true) spriteBatch.Draw (brush_control.texture[(int) Brush_Control.T.EXIT_RED_V_TOP_OPEN_TEST], r_draw, r_source, Color.White);
                else if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.EXIT_RED_H_TOP_CLOSED_TEST && red_doors_open == true) spriteBatch.Draw (brush_control.texture[(int) Brush_Control.T.EXIT_RED_H_TOP_OPEN_TEST], r_draw, r_source, Color.White);
                else if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.BOX_ICE_TEST) spriteBatch.Draw (brush_control.texture[brush_control.brush[b].top_texture_number], r_draw, r_source, Color.White * .9f);
                else spriteBatch.Draw (brush_control.texture[brush_control.brush[b].top_texture_number], r_draw, r_source, Color.White);

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
                r_draw.Height = Convert.ToInt32(brush_control.brush[b].height * parallax);/// 2;

                r_source.X = brush_control.brush[b].front_texture_offset_x;
                r_source.Y = brush_control.brush[b].front_texture_offset_y;
                r_source.Width = tilesize;
                r_source.Height = brush_control.brush[b].height;

                //// brush_control.texture looping tall brushes
                //if (brush_control.brush[b].front_texture_number < (int) Brush_Control.T.SINGLE_PIECE && brush_control.brush[b].front_texture_offset_y + brush_control.brush[b].height > brush_control.texture[brush_control.brush[b].front_texture_number].Height)
                //  {
                //  r_source.Height = tilesize;
                //  r_draw.Height = tilesize / 2;
                //  }

                if (brush_control.brush[b].front_texture_number == (int) Brush_Control.T.FLOOR_GRATE_TEST)
                  {
                  r_source.Height = 6;
                  r_draw.Height = 6;
                  }

                if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.INVISIBLE_WALL) { }
                else if (brush_control.brush[b].front_texture_number == (int) Brush_Control.T.DOOR_GREEN_V_FRONT_CLOSED_TEST && green_doors_open == true) spriteBatch.Draw (brush_control.texture[(int) Brush_Control.T.DOOR_GREEN_V_FRONT_OPEN_TEST], r_draw, r_source, Color.White);
                else if (brush_control.brush[b].front_texture_number == (int) Brush_Control.T.DOOR_RED_V_FRONT_CLOSED_TEST && red_doors_open == true) spriteBatch.Draw (brush_control.texture[(int) Brush_Control.T.DOOR_RED_V_FRONT_OPEN_TEST], r_draw, r_source, Color.White);
                else if (brush_control.brush[b].front_texture_number == (int) Brush_Control.T.DOOR_BLUE_V_FRONT_CLOSED_TEST && blue_doors_open == true) spriteBatch.Draw (brush_control.texture[(int) Brush_Control.T.DOOR_BLUE_V_FRONT_OPEN_TEST], r_draw, r_source, Color.White);
                else if (brush_control.brush[b].front_texture_number == (int) Brush_Control.T.DOOR_YELLOW_V_FRONT_CLOSED_TEST && yellow_doors_open == true) spriteBatch.Draw (brush_control.texture[(int) Brush_Control.T.DOOR_YELLOW_V_FRONT_OPEN_TEST], r_draw, r_source, Color.White);
                else if (brush_control.brush[b].front_texture_number == (int) Brush_Control.T.EXIT_RED_V_FRONT_CLOSED_TEST && red_doors_open == true) spriteBatch.Draw (brush_control.texture[(int) Brush_Control.T.EXIT_RED_V_FRONT_OPEN_TEST], r_draw, r_source, Color.White);
                else if (brush_control.brush[b].front_texture_number == (int) Brush_Control.T.BOX_ICE_TEST) spriteBatch.Draw (brush_control.texture[brush_control.brush[b].front_texture_number], r_draw, r_source, Color.White * .9f);
                else
                  {
                  spriteBatch.Draw (brush_control.texture[brush_control.brush[b].front_texture_number], r_draw, r_source, Color.White);
                  spriteBatch.Draw (brush_control.texture[brush_control.brush[b].front_texture_number], r_draw, r_source, Color.Black * .3f);
                  }

                // brush_control.texture looping tall brushes
                //if (brush_control.brush[b].front_texture_number < (int) Brush_Control.T.SINGLE_PIECE && brush_control.brush[b].front_texture_offset_y + brush_control.brush[b].height > brush_control.texture[brush_control.brush[b].front_texture_number].Height)
                //  {
//                  r_source.Y = 0;
//                  r_draw.Y += tilesize / 2;
//                  spriteBatch.Draw (brush_control.texture[brush_control.brush[b].front_texture_number], r_draw, r_source, Color.White);
//                  spriteBatch.Draw (brush_control.texture[brush_control.brush[b].front_texture_number], r_draw, r_source, Color.Black * .3f);
                  //}

                // front stickers
                if (brush_control.brush[b].front_sticker > -1)
                  {
                  r_source.X = brush_control.brush[b].front_sticker_offset_x;
                  r_source.Y = brush_control.brush[b].front_sticker_offset_y;
                  r_source.Width = tilesize;
                  r_source.Height = tilesize;
                  r_draw.Width = tilesize;
                  r_draw.Height = Convert.ToInt32(tilesize * parallax);

                  if (brush_control.brush[b].front_sticker_type == "office") spriteBatch.Draw (sticker_office[brush_control.brush[b].front_sticker], r_draw, r_source, Color.White);
                  else if (brush_control.brush[b].front_sticker_type == "factory") spriteBatch.Draw (sticker_factory[brush_control.brush[b].front_sticker], r_draw, r_source, Color.White);
                  }

                // front shading
                //if (brush_control.brush[b].front_texture_number == DOOR_TEST_RED_V_FRONT_CLOSED && red_doors_open == false) spriteBatch.Draw (shading_door_test_closed, r_draw, Color.White * .3f);
                //else if (brush_control.brush[b].front_texture_number == DOOR_TEST_RED_V_FRONT_CLOSED && red_doors_open == true)  spriteBatch.Draw (shading_door_test_open, r_draw, Color.White * .3f);
                //else if (brush_control.brush[b].front_texture_number == DOOR_TEST_YELLOW_V_FRONT_CLOSED && yellow_doors_open == false) spriteBatch.Draw (shading_door_test_closed, r_draw, Color.White * .3f);
                //else if (brush_control.brush[b].front_texture_number == DOOR_TEST_YELLOW_V_FRONT_CLOSED && yellow_doors_open == true) spriteBatch.Draw (shading_door_test_open, r_draw, Color.White * .3f);
                //else if (brush_control.brush[b].front_texture_number == DOOR_TEST_GREEN_V_FRONT_CLOSED && green_doors_open == false) spriteBatch.Draw (shading_door_test_closed, r_draw, Color.White * .3f);
                //else if (brush_control.brush[b].front_texture_number == DOOR_TEST_GREEN_V_FRONT_CLOSED && green_doors_open == true) spriteBatch.Draw (shading_door_test_open, r_draw, Color.White * .3f);
                //else if (brush_control.brush[b].front_texture_number == DOOR_TEST_BLUE_V_FRONT_CLOSED && blue_doors_open == false) spriteBatch.Draw (shading_door_test_closed, r_draw, Color.White * .3f);
                //else if (brush_control.brush[b].front_texture_number == DOOR_TEST_BLUE_V_FRONT_CLOSED && blue_doors_open == true) spriteBatch.Draw (shading_door_test_open, r_draw, Color.White * .3f);
                //else if (brush_control.brush[b].front_texture_number == GATEWAY_TEST_V_FRONT_CLOSED) spriteBatch.Draw (shading_gateway_test, r_draw, Color.White * .3f);
                //else if (brush_control.brush[b].front_texture_number == BOX_TEST_ICE)
                //{
                //spriteBatch.Draw (brush_control.texture[brush_control.brush[b].front_texture_number], r_draw, r_source, Color.Black * .3f);
                //}
                //else spriteBatch.Draw (shading_wall, r_draw, Color.White * .3f);
                }
              }
            //}  // != INVISIBLE_WALL

            // red tinting for hot metal
            if ((brush_control.brush[b].top_texture_number == (int) Brush_Control.T.BOX_METAL_TEST || brush_control.brush[b].top_texture_number == (int) Brush_Control.T.METAL_MINT_TOP_TEST
                 || brush_control.brush[b].top_texture_number == (int) Brush_Control.T.METAL_BLUE_TOP_TEST || brush_control.brush[b].top_texture_number == (int) Brush_Control.T.METAL_BLACK_TEST)
                && brush_control.brush[b].temperature > 70)
              {
              spriteBatch.Draw (brush_control.texture[(int) Brush_Control.T.TEXTURE_HIGHLIGHT_RED], brush_screen_draw, Color.White * ((Convert.ToSingle (brush_control.brush[b].temperature) - 70f) / 300f));
              }

            // determine surroundings of floor brush_control.brush for shadow generation and seam blending
            wall_to_north = brush_control.point_in_brush (brush_control.brush[b].x + (tilesize / 2), Convert.ToInt32 (brush_control.brush[b].y + (tilesize * 1.5)), Convert.ToInt32 (brush_control.brush[b].z + (tilesize * 1.5)), true, false);
            wall_to_south = brush_control.point_in_brush (brush_control.brush[b].x + (tilesize / 2), brush_control.brush[b].y - (tilesize / 2), Convert.ToInt32 (brush_control.brush[b].z + (tilesize * 1.5)), true, false);
            wall_to_west = brush_control.point_in_brush (brush_control.brush[b].x - (tilesize / 2), brush_control.brush[b].y + (tilesize / 2), Convert.ToInt32 (brush_control.brush[b].z + (tilesize * 1.5)), true, false);
            wall_to_east = brush_control.point_in_brush (Convert.ToInt32 (brush_control.brush[b].x + (tilesize * 1.5)), brush_control.brush[b].y + (tilesize / 2), Convert.ToInt32 (brush_control.brush[b].z + (tilesize * 1.5)), true, false);
            floor_to_north = brush_control.brush_north_of_brush (brush_control.brush[b]);
            floor_to_south = brush_control.brush_south_of_brush (brush_control.brush[b]);
            floor_to_west = brush_control.brush_west_of_brush (brush_control.brush[b]);
            floor_to_east = brush_control.brush_east_of_brush (brush_control.brush[b]);

            if (toggle_texture_blending == true)
              {
              // top seam blending
              if (brush_control.brush[b].moveable == false)
                {
                // north
                if (floor_to_north > -1 && wall_to_north == -1 && brush_control.brush[floor_to_north].top_texture_number != brush_control.brush[b].top_texture_number
                    && brush_control.brush[floor_to_north].top_texture_number < (int) Brush_Control.T.SINGLE_PIECE
                    && brush_control.brush[floor_to_north].moveable == false)
                  {
                  int y = 0;
                  for (opacity = .5f; opacity > 0f; opacity -= .05f)//.1f)
                    {

                    r_source.X = brush_control.brush[floor_to_north].top_texture_offset_x;
                    r_source.Y = brush_control.brush[floor_to_north].top_texture_offset_y + tilesize + y;
                    if (r_source.Y > brush_control.texture[brush_control.brush[floor_to_north].top_texture_number].Height) r_source.Y = y;
                    r_source.Width = tilesize;
                    r_source.Height = 1;

                    r_draw.X = Convert.ToInt32 (brush_screen_draw.X);
                    r_draw.Y = Convert.ToInt32 (brush_screen_draw.Y + y);
                    r_draw.Width = r_source.Width;
                    r_draw.Height = 1;

                    spriteBatch.Draw (brush_control.texture[brush_control.brush[floor_to_north].top_texture_number], r_draw, r_source, Color.White * opacity);

                    y += 1;
                    }
                  }

                // south
                if (floor_to_south > -1 && wall_to_south == -1 && brush_control.brush[floor_to_south].top_texture_number != brush_control.brush[b].top_texture_number
                    && brush_control.brush[floor_to_south].top_texture_number < (int) Brush_Control.T.SINGLE_PIECE
                    && brush_control.brush[floor_to_south].moveable == false)
                  {
                  int y = 1;
                  for (opacity = .5f; opacity > 0f; opacity -= .05f)//.1f)
                    {
                    r_source.X = brush_control.brush[floor_to_south].top_texture_offset_x;
                    r_source.Y = brush_control.brush[floor_to_south].top_texture_offset_y - y;
                    if (r_source.Y < 0) r_source.Y = brush_control.texture[brush_control.brush[floor_to_south].top_texture_number].Height - y;
                    r_source.Width = tilesize;
                    r_source.Height = 1;

                    r_draw.X = Convert.ToInt32 (brush_screen_draw.X);
                    r_draw.Y = Convert.ToInt32 (brush_screen_draw.Y + tilesize - y);
                    r_draw.Width = r_source.Width;
                    r_draw.Height = 1;

                    spriteBatch.Draw (brush_control.texture[brush_control.brush[floor_to_south].top_texture_number], r_draw, r_source, Color.White * opacity);

                    y += 1;
                    }
                  }

                // east
                if (floor_to_east > -1 && wall_to_east == -1 && brush_control.brush[floor_to_east].top_texture_number != brush_control.brush[b].top_texture_number
                    && brush_control.brush[floor_to_east].top_texture_number < (int) Brush_Control.T.SINGLE_PIECE
                    && brush_control.brush[floor_to_east].moveable == false)
                  {
                  int x = 1;
                  for (opacity = .5f; opacity > 0f; opacity -= .05f)//.1f)
                    {
                    r_source.X = brush_control.brush[floor_to_east].top_texture_offset_x - x;
                    if (r_source.X < 0) r_source.X = brush_control.texture[brush_control.brush[floor_to_east].top_texture_number].Width - x;
                    r_source.Y = brush_control.brush[floor_to_east].top_texture_offset_y;
                    r_source.Width = 1;
                    r_source.Height = tilesize;

                    r_draw.X = Convert.ToInt32 (brush_screen_draw.X + tilesize - x);
                    r_draw.Y = Convert.ToInt32 (brush_screen_draw.Y);
                    r_draw.Width = r_source.Width;
                    r_draw.Height = r_source.Height;

                    spriteBatch.Draw (brush_control.texture[brush_control.brush[floor_to_east].top_texture_number], r_draw, r_source, Color.White * opacity);

                    x += 1;
                    }
                  }

                // west
                if (floor_to_west > -1 && wall_to_west == -1 && brush_control.brush[floor_to_west].top_texture_number != brush_control.brush[b].top_texture_number
                    && brush_control.brush[floor_to_west].top_texture_number < (int) Brush_Control.T.SINGLE_PIECE
                    && brush_control.brush[floor_to_west].moveable == false)
                  {
                  int x = 0;
                  for (opacity = .5f; opacity > 0f; opacity -= .05f)//.1f
                    {
                    r_source.X = brush_control.brush[floor_to_west].top_texture_offset_x + tilesize + x;
                    if (r_source.X > brush_control.texture[brush_control.brush[floor_to_west].top_texture_number].Width) r_source.X = x;
                    r_source.Y = brush_control.brush[floor_to_west].top_texture_offset_y;
                    r_source.Width = 1;
                    r_source.Height = tilesize;

                    r_draw.X = Convert.ToInt32 (brush_screen_draw.X + x);
                    r_draw.Y = Convert.ToInt32 (brush_screen_draw.Y);
                    r_draw.Width = r_source.Width;
                    r_draw.Height = r_source.Height;

                    spriteBatch.Draw (brush_control.texture[brush_control.brush[floor_to_west].top_texture_number], r_draw, r_source, Color.White * opacity);

                    x += 1;
                    }
                  }
                }
              }

            // WALL SHADOWS

            // if box is between grid spaces
            if (brush_control.brush[b].moving == true)
              {
              // old method - from wall
              shadow_west = false;
              shadow_south = false;
              shadow_east = false;

              // wall shadow south
              if ((brush_control.point_in_brush (brush_control.brush[b].x, brush_control.brush[b].y - (box_move / 2), brush_control.brush[b].z - (box_move / 2), false, false) > -1  // floor below bottom left
                   || brush_control.point_in_brush (brush_control.brush[b].x + (brush_control.brush[b].width / 2), brush_control.brush[b].y - (box_move / 2), brush_control.brush[b].z - (box_move / 2), false, false) > -1    // floor below bottom middle
                   || brush_control.point_in_brush (brush_control.brush[b].x + (brush_control.brush[b].width - 1), brush_control.brush[b].y - (box_move / 2), brush_control.brush[b].z - (box_move / 2), false, false) > -1)   // floor below bottom right
                   && brush_control.point_in_brush (brush_control.brush[b].x + (brush_control.brush[b].width / 2), brush_control.brush[b].y - (box_move / 2), brush_control.brush[b].z + (box_move / 2), false, false) == -1)  // no brush_control.brush in front (blocks shadow, draw unnecessary)
                {
                shadow_south = true;
                v_draw.X = brush_control.brush[b].x + screen.scroll_x;
                v_draw.Y = Convert.ToInt32((screen.height - brush_control.brush[b].y) - (brush_control.brush[b].z * parallax) + screen.scroll_y);
                spriteBatch.Draw (wall_shadow_south, v_draw, Color.White * .75f);
                }

              // wall shadow west
              if ((brush_control.point_in_brush (brush_control.brush[b].x - (box_move / 2), brush_control.brush[b].y + (brush_control.brush[b].length / 2), brush_control.brush[b].z - (box_move / 2), true, false) > -1
                   || brush_control.point_in_brush (brush_control.brush[b].x - (box_move / 2), brush_control.brush[b].y + (brush_control.brush[b].length / 2), brush_control.brush[b].z - (box_move / 2), true, false) > -1
                   || brush_control.point_in_brush (brush_control.brush[b].x - (box_move / 2), brush_control.brush[b].y + (brush_control.brush[b].length / 2), brush_control.brush[b].z - (box_move / 2), true, false) > -1)
                   && brush_control.point_in_brush (brush_control.brush[b].x - 1, brush_control.brush[b].y + (brush_control.brush[b].length / 2), brush_control.brush[b].z + (box_move / 2), true, false) == -1)
                {
                shadow_west = true;
                v_draw.X = brush_control.brush[b].x + screen.scroll_x - wall_shadow_west.Width;
                v_draw.Y = Convert.ToInt32((screen.height - brush_control.brush[b].y - brush_control.brush[b].length) - (brush_control.brush[b].z * parallax) + screen.scroll_y);
                spriteBatch.Draw (wall_shadow_west, v_draw, Color.White * .75f);
                }

              // wall shadow east
              if (brush_control.point_in_brush (brush_control.brush[b].x + brush_control.brush[b].width + (box_move / 2), brush_control.brush[b].y + (brush_control.brush[b].length / 2), brush_control.brush[b].z - (box_move / 2), true, false) > -1
                  && brush_control.point_in_brush (brush_control.brush[b].x + brush_control.brush[b].width + 1, brush_control.brush[b].y + (brush_control.brush[b].length / 2), brush_control.brush[b].z + (box_move / 2), true, false) == -1)
                {
                shadow_east = true;
                v_draw.X = brush_control.brush[b].x + brush_control.brush[b].width + screen.scroll_x;
                v_draw.Y = Convert.ToInt32((screen.height - brush_control.brush[b].y - brush_control.brush[b].length) - (brush_control.brush[b].z * parallax) + screen.scroll_y);
                spriteBatch.Draw (wall_shadow_east, v_draw, Color.White * .75f);
                }

              // wall shadow south west
              if (shadow_south == true && shadow_west == true)
                {
                v_draw.X = brush_control.brush[b].x - wall_shadow_south_west.Width + screen.scroll_x;
                v_draw.Y = Convert.ToInt32((screen.height - brush_control.brush[b].y) - (brush_control.brush[b].z * parallax) + screen.scroll_y);
                spriteBatch.Draw (wall_shadow_south_west, v_draw, Color.White * .75f);
                }

              // wall shadow south east
              if (shadow_south == true && shadow_east == true)
                {
                v_draw.X = brush_control.brush[b].x + brush_control.brush[b].width + screen.scroll_x;
                v_draw.Y = Convert.ToInt32((screen.height - brush_control.brush[b].y) - (brush_control.brush[b].z * parallax) + screen.scroll_y);
                spriteBatch.Draw (wall_shadow_south_east, v_draw, Color.White * .75f);
                }
              }


            // new method - on floor
            shadow_north = false;
            shadow_east = false;
            shadow_west = false;
            floor_visible = true;
            //if (point_in_brush (brush_control.brush[b].x + (tilesize / 2), brush_control.brush[b].y + (tilesize / 2), Convert.ToInt32(brush_control.brush[b].z + (tilesize * 1.5)), true) > -1) floor_visible = false;
            if (floor_visible == true)
              {
              // shadow from above
              b2 = brush_control.point_in_brush (brush_control.brush[b].x + (tilesize / 2), brush_control.brush[b].y + (tilesize / 2), Convert.ToInt32 (brush_control.brush[b].z + (tilesize * 1.5)), true, false);
              if (b2 > -1 && brush_control.brush[b2].moving == false)
                {
                //spriteBatch.Draw (wall_shadow_center, brush_screen_draw, Color.White * .75f);
                }
              // shadow from north
              b2 = brush_control.point_in_brush (brush_control.brush[b].x + (tilesize / 2), Convert.ToInt32 (brush_control.brush[b].y + (tilesize * 1.5)), Convert.ToInt32 (brush_control.brush[b].z + (tilesize * 1.5)), false, false);
              if (b2 > -1 && brush_control.brush[b2].moving == false)
                {
                shadow_north = true;
                spriteBatch.Draw (wall_shadow_south, brush_screen_draw, Color.White * .75f);
                }
              // shadow from west
              b2 = brush_control.point_in_brush (brush_control.brush[b].x - (tilesize / 2), brush_control.brush[b].y + (tilesize / 2), Convert.ToInt32 (brush_control.brush[b].z + (tilesize * 1.5)), true, false);
              if (b2 > -1 && brush_control.brush[b2].moving == false)
                {
                shadow_west = true;
                spriteBatch.Draw (wall_shadow_east, brush_screen_draw, Color.White * .75f);
                }
              // shadow from east
              b2 = brush_control.point_in_brush (Convert.ToInt32 (brush_control.brush[b].x + (tilesize * 1.5)), brush_control.brush[b].y + (tilesize / 2), Convert.ToInt32 (brush_control.brush[b].z + (tilesize * 1.5)), true, false);
              if (b2 > -1 && brush_control.brush[b2].moving == false)
                {
                shadow_east = true;
                v_draw.X = brush_screen_draw.X + tilesize - wall_shadow_west.Width;
                v_draw.Y = brush_screen_draw.Y;
                spriteBatch.Draw (wall_shadow_west, v_draw, Color.White * .75f);
                }
              // shadow from north west
              b2 = brush_control.point_in_brush (brush_control.brush[b].x - (tilesize / 2), Convert.ToInt32 (brush_control.brush[b].y + (tilesize * 1.5)), Convert.ToInt32 (brush_control.brush[b].z + (tilesize * 1.5)), true, false);
              if (shadow_north != true && shadow_west != true && b2 > -1 && brush_control.brush[b2].moving == false)
                {
                spriteBatch.Draw (wall_shadow_south_east, brush_screen_draw, Color.White * .75f);
                }
              // shadow from north east
              b2 = brush_control.point_in_brush (Convert.ToInt32 (brush_control.brush[b].x + (tilesize * 1.5)), Convert.ToInt32 (brush_control.brush[b].y + (tilesize * 1.5)), Convert.ToInt32 (brush_control.brush[b].z + (tilesize * 1.5)), true, false);
              if (shadow_north != true && shadow_east != true && b2 > -1 && brush_control.brush[b2].moving == false)
                {
                v_draw.X = brush_screen_draw.X + tilesize - wall_shadow_south_west.Width;
                v_draw.Y = brush_screen_draw.Y;
                spriteBatch.Draw (wall_shadow_south_west, v_draw, Color.White * .75f);
                }
              }
            if (character_control.character[PLAYER].brush_grab == b && character_control.character[PLAYER].grab_position != "above")
              {
              draw_character (PLAYER);
              }
            }

          else if (draw_list[l].type == "fixture")
            {
            fixture_control.draw (draw_list[l].id, spriteBatch, screen);

            f = draw_list[l].id;
            v_draw.X = fixture_control.fixture[f].x + screen.scroll_x;
            v_draw.Y = Convert.ToInt32(screen.height - fixture_control.fixture[f].y - fixture_control.fixture[f].length - (fixture_control.fixture[f].z * parallax) - (fixture_control.fixture[f].height * parallax) + screen.scroll_y);

            // fixture effects
            if (fixture_control.fixture[f].type == (int) Fixture_Control.F.WIRES_SOUTHEAST_TEST && fixture_control.fixture[f].powered == true)
              {
              spriteBatch.Draw (wires_southeast_powered_test, v_draw, Color.White);
              }
            else if (fixture_control.fixture[f].type == (int) Fixture_Control.F.VENDING_TEST)
              {
              // glow from lit machine
              r_draw.Width = light_sprite[(int) L.yellow].Width / 2;
              r_draw.Height = light_sprite[(int) L.yellow].Height / 2;
              r_draw.X = Convert.ToInt32 (v_draw.X + (fixture_control.fixture[f].width / 2) - (r_draw.Width / 2));
              r_draw.Y = Convert.ToInt32 (v_draw.Y + (fixture_control.fixture[f].height / 2) - (r_draw.Height / 2));
              spriteBatch.Draw (light_sprite[(int) L.yellow], r_draw, Color.White * .25f);
              }
            }
          else if (draw_list[l].type == "object" && object_control.obj[draw_list[l].id].destroyed == false)
            {
            o = draw_list[l].id;

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
            r_draw.Y = Convert.ToInt32((screen.height - ty) - (tz * parallax) + screen.scroll_y - 3);
            r_draw.Width = Convert.ToInt32 (object_control.object_sprite[object_control.obj[o].type, 0].Width * shadow_scale);
            r_draw.Height = Convert.ToInt32 (object_control.object_sprite[object_control.obj[o].type, 0].Height / 4 * shadow_scale);
            v_origin.X = object_control.object_sprite[object_control.obj[o].type, 0].Width / 2;
            v_origin.Y = 0;
            spriteBatch.Draw (object_control.object_sprite[object_control.obj[o].type, object_control.obj[o].skin], r_draw, r_source, Color.Black * temp_fade, MathHelper.ToRadians (0), v_origin, SpriteEffects.FlipVertically, 0);

            //      // draw object
            v_draw.X = object_control.obj[o].x - (object_control.obj[o].width / 2) + screen.scroll_x;
            v_draw.Y = Convert.ToInt32((screen.height - object_control.obj[o].y) - object_control.obj[o].height - (object_control.obj[o].z * parallax) + screen.scroll_y);
            spriteBatch.Draw (object_control.object_sprite[object_control.obj[o].type, object_control.obj[o].skin], v_draw, Color.White);

            if (debug == true)
              {
              // draw object id over object (remove later)
              v_draw2.X = v_draw.X + (object_control.object_sprite[object_control.obj[o].type, 0].Width / 2) - 5;
              v_draw2.Y = v_draw.Y - 15;
              spriteBatch.DrawString (debug_font, Convert.ToString (o), v_draw2, Color.White);
              }
            }
          else if (draw_list[l].type == "character") draw_character (draw_list[l].id);
          }

        if (lighting_engine != 2)
          {
          for (l = 0; l < total_lights; l += 1)
            {
            // don't draw lights that are fully transparent, off-screen or switched off
            if (light[l].alpha > 0f
                && light[l].x + (light_sprite[light[l].light_number].Width / 2 * light[l].scale) + screen.scroll_x > 0
                && light[l].x - (light_sprite[light[l].light_number].Width / 2 * light[l].scale) + screen.scroll_x < screen.width
                && (screen.height - light[l].y) + screen.scroll_y + (light_sprite[light[l].light_number].Width / 2 * light[l].scale) > 0
                && (screen.height - light[l].y) + screen.scroll_y - (light_sprite[light[l].light_number].Width / 2 * light[l].scale) < screen.height
                && light[l].on == true)
              {
              v_origin.X = light_sprite[light[l].light_number].Width / 2;
              v_origin.Y = light_sprite[light[l].light_number].Height / 2;
              v_draw.X = light[l].x + screen.scroll_x;
              v_draw.Y = (screen.height - light[l].y) + screen.scroll_y;
              spriteBatch.Draw (light_sprite[light[l].light_number], v_draw, null, Color.White * light[l].alpha, 0f, v_origin, light[l].scale, SpriteEffects.None, 0);
              }
            else if (light[l].on == false)
              {
              v_origin.X = light_sprite[(int) L.dark].Width / 2;
              v_origin.Y = light_sprite[(int) L.dark].Height / 2;
              v_draw.X = light[l].x + screen.scroll_x;
              v_draw.Y = (screen.height - light[l].y) + screen.scroll_y;
              spriteBatch.Draw (light_sprite[(int) L.dark], v_draw, null, Color.White * ambient_dark, 0f, v_origin, light[l].scale, SpriteEffects.None, 0);
              }
            if (light[l].type == PULSING && light[l].light_number != (int) L.dark)
              {
              v_origin.X = light_sprite[(int) L.dark].Width / 2;
              v_origin.Y = light_sprite[(int) L.dark].Height / 2;
              v_draw.X = light[l].x + screen.scroll_x;
              v_draw.Y = (screen.height - light[l].y) + screen.scroll_y;
              if (light[l].dimness > 0f) spriteBatch.Draw (light_sprite[(int) L.dark], v_draw, null, Color.White * light[l].dimness, 0f, v_origin, light[l].scale, SpriteEffects.None, 0);
              else if (light[l].dimness < 0f) spriteBatch.Draw (light_sprite[(int) L.white], v_draw, null, Color.White * (light[l].dimness * -1f), 0f, v_origin, light[l].scale, SpriteEffects.None, 0);
              }
            }
          }

        // particles
        for (p = 0; p < max_effects; p += 1)
          if (particle_effect[p].active == true) particle_effect[p].draw (spriteBatch, screen.width, screen.height, screen.scroll_x, screen.scroll_y);
        }  // if game_state == GAME

      // richard's pain and sorrow
      if (character_control.character[PLAYER].health < 75)
        {
        r_draw.X = 0;
        r_draw.Y = 0;
        r_draw.Width = screen.width;
        r_draw.Height = screen.height;
        spriteBatch.Draw (effect_pain, r_draw, Color.White * ((75f - character_control.character[PLAYER].health) / 75f));
        }

      // color flash
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

        spriteBatch.Draw (solid_black, r_draw, r_source, Color.White * .75f);

        if (menu_screen == "main")
          {
          spriteBatch.Draw (menu_exit_test, menu_exit_v, Color.White);
          }
        }

      //spriteBatch.Draw (fixture_sprite[PLANT1_TEST], Vector2.Zero, Color.White);
      //spriteBatch.Draw (light_sprite[Convert.ToInt16(L.red)], Vector2.Zero, Color.White * .5f);

      spriteBatch.End ();
      
      if (lighting_engine == 2)
        {
        BlendState blendState = new BlendState ();
        blendState.ColorSourceBlend = Blend.DestinationColor;
        blendState.ColorDestinationBlend = Blend.SourceColor;
        spriteBatch.Begin (SpriteSortMode.Immediate, blendState);
        spriteBatch.Draw (light_buffer, Vector2.Zero, Color.White);
        spriteBatch.End ();
        }

      // debugging stuff
      if (debug == true)
        {
        spriteBatch.Begin (SpriteSortMode.Immediate, BlendState.AlphaBlend);

        debug_string += "game state: ";
        if (game_state == GAME) debug_string += "Game";//spriteBatch.DrawString (debug_font, "Game", debug_pos, Color.Yellow);
        else if (game_state == CREATION) debug_string += "Creation";// spriteBatch.DrawString (debug_font, "Creation", debug_pos, Color.Yellow);
        debug_string += "\n";

        if (game_state == GAME)
          {
          // controller
          /*
          "left stick x: " + Convert.ToString (controller.ThumbSticks.Left.X)
          "left stick y: " + Convert.ToString (controller.ThumbSticks.Left.Y)
          */

          //player
          //debug_string += "left alt: " + key_leftalt + "\n";
          //debug_string += "box grab: " + Convert.ToString (character_control.character[PLAYER].brush_grab) + "\n";
          //debug_string += "grab position: " + character_control.character[PLAYER].grab_position + "\n";
          debug_string += "direction: " + Convert.ToString (MathHelper.ToDegrees (Convert.ToSingle (character_control.character[PLAYER].dir))) + "\n";
          debug_string += "last_dir: " + Convert.ToString (MathHelper.ToDegrees (Convert.ToSingle (character_control.character[PLAYER].last_dir))) + "\n";
          debug_string += "action: " + character_control.character[PLAYER].action + "\n";
          //debug_string += "screen x: " + Convert.ToString (character_control.character[PLAYER].x + screen.scroll_x) + "\n";
          //debug_string += "screen y: " + Convert.ToString (screen.height - character_control.character[PLAYER].y - (character_control.character[PLAYER].z / 2) + screen.scroll_y) + "\n";
          //debug_string += "runboost: " + character_control.character[PLAYER].runboost + "\n";
          debug_string += "skid_counter: " + character_control.character[PLAYER].skid_counter + "\n";

          //"mouse x: " + Convert.ToString (mouse_current.X)
          //"mouse y: " + Convert.ToString (mouse_current.Y)

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
          //  spriteBatch.DrawString (debug_font, "char 1 active: ", debug_pos, Color.Yellow);
          //  spriteBatch.DrawString (debug_font, Convert.ToString (character_control.active (1)), debug_pos, Color.Yellow);

          //  spriteBatch.DrawString (debug_font, "char 1 name: ", debug_pos, Color.Yellow);
          //  spriteBatch.DrawString (debug_font, Convert.ToString (character_control.character[1].name), debug_pos, Color.Yellow);
          //  }

          //if (total_characters > 2)
          //  {
          //  spriteBatch.DrawString (debug_font, "char 2 active: ", debug_pos, Color.Yellow);
          //  spriteBatch.DrawString (debug_font, Convert.ToString (character_control.active (2)), debug_pos, Color.Yellow);

          //  spriteBatch.DrawString (debug_font, "char 2 name: ", debug_pos, Color.Yellow);
          //  spriteBatch.DrawString (debug_font, Convert.ToString (character_control.character[2].name), debug_pos, Color.Yellow);
          //  }
          //*/

          /*
          // objects
          spriteBatch.DrawString (debug_font, "max obj: ", debug_pos, Color.Yellow);
          spriteBatch.DrawString (debug_font, Convert.ToString (max_objects), debug_pos, Color.Yellow);
          
          spriteBatch.DrawString (debug_font, "total obj: ", debug_pos, Color.Yellow);
          spriteBatch.DrawString (debug_font, Convert.ToString (total_objects), debug_pos, Color.Yellow);

          // object 0
          spriteBatch.DrawString (debug_font, "obj[0].x: ", debug_pos, Color.Yellow);
          spriteBatch.DrawString (debug_font, Convert.ToString (obj[0].x), debug_pos, Color.Yellow);

          spriteBatch.DrawString (debug_font, "obj[0].y: ", debug_pos, Color.Yellow);
          spriteBatch.DrawString (debug_font, Convert.ToString (obj[0].y), debug_pos, Color.Yellow);

          spriteBatch.DrawString (debug_font, "obj[0].z: ", debug_pos, Color.Yellow);
          spriteBatch.DrawString (debug_font, Convert.ToString (obj[0].z), debug_pos, Color.Yellow);

          spriteBatch.DrawString (debug_font, "obj[0].destroyed: ", debug_pos, Color.Yellow);
          spriteBatch.DrawString (debug_font, Convert.ToString (obj[0].destroyed), debug_pos, Color.Yellow);

          // object 1
          spriteBatch.DrawString (debug_font, "obj[1].x: ", debug_pos, Color.Yellow);
          spriteBatch.DrawString (debug_font, Convert.ToString (obj[1].x), debug_pos, Color.Yellow);

          spriteBatch.DrawString (debug_font, "obj[1].y: ", debug_pos, Color.Yellow);
          spriteBatch.DrawString (debug_font, Convert.ToString (obj[1].y), debug_pos, Color.Yellow);

          spriteBatch.DrawString (debug_font, "obj[1].z: ", debug_pos, Color.Yellow);
          spriteBatch.DrawString (debug_font, Convert.ToString (obj[1].z), debug_pos, Color.Yellow);

          spriteBatch.DrawString (debug_font, "obj[1].destroyed: ", debug_pos, Color.Yellow);
          spriteBatch.DrawString (debug_font, Convert.ToString (obj[1].destroyed), debug_pos, Color.Yellow);

          // object 12
          spriteBatch.DrawString (debug_font, "obj[2].x: ", debug_pos, Color.Yellow);
          spriteBatch.DrawString (debug_font, Convert.ToString (obj[2].x), debug_pos, Color.Yellow);

          spriteBatch.DrawString (debug_font, "obj[2].y: ", debug_pos, Color.Yellow);
          spriteBatch.DrawString (debug_font, Convert.ToString (obj[2].y), debug_pos, Color.Yellow);

          spriteBatch.DrawString (debug_font, "obj[2].z: ", debug_pos, Color.Yellow);
          spriteBatch.DrawString (debug_font, Convert.ToString (obj[2].z), debug_pos, Color.Yellow);

          spriteBatch.DrawString (debug_font, "obj[2].destroyed: ", debug_pos, Color.Yellow);
          spriteBatch.DrawString (debug_font, Convert.ToString (obj[2].destroyed), debug_pos, Color.Yellow);
          */

      //    // retard
      //    /*
      //    spriteBatch.DrawString (debug_font, "direction: ", debug_pos, Color.Yellow);
      //    spriteBatch.DrawString (debug_font, Convert.ToString (MathHelper.ToDegrees (Convert.ToSingle (character_control.character[0].dir))), debug_pos, Color.Yellow);
      //    spriteBatch.DrawString (debug_font, "action: ", debug_pos, Color.Yellow);
      //    if (character_control.character[0].action == "none") spriteBatch.DrawString (debug_font, "none", debug_pos, Color.Yellow);
      //    else if (character_control.character[0].action == "standing") spriteBatch.DrawString (debug_font, "standing", debug_pos, Color.Yellow);
      //    else if (character_control.character[0].action == "walking") spriteBatch.DrawString (debug_font, "walking", debug_pos, Color.Yellow);
      //    else if (character_control.character[0].action == "running") spriteBatch.DrawString (debug_font, "running", debug_pos, Color.Yellow);
      //    else if (character_control.character[0].action == "jumping") spriteBatch.DrawString (debug_font, "jumping", debug_pos, Color.Yellow);
      //    else if (character_control.character[0].action == "grabbing") spriteBatch.DrawString (debug_font, "grabbing", debug_pos, Color.Yellow);
      //    else if (character_control.character[0].action == "pushing") spriteBatch.DrawString (debug_font, "pushing", debug_pos, Color.Yellow);
      //    spriteBatch.DrawString (debug_font, "screen x: ", debug_pos, Color.Yellow);
      //    spriteBatch.DrawString (debug_font, Convert.ToString (character_control.character[0].x + scroll_x), debug_pos, Color.Yellow);
      //    spriteBatch.DrawString (debug_font, "screen y: ", debug_pos, Color.Yellow);
      //    spriteBatch.DrawString (debug_font, Convert.ToString (screen.height - character_control.character[0].y - (character_control.character[0].z / 2) + scroll_y), debug_pos, Color.Yellow);
      //    spriteBatch.DrawString (debug_font, "mouse x: ", debug_pos, Color.Yellow);
      //    spriteBatch.DrawString (debug_font, Convert.ToString (mouse_current.X), debug_pos, Color.Yellow);
      //    spriteBatch.DrawString (debug_font, "mouse y: ", debug_pos, Color.Yellow);
      //    spriteBatch.DrawString (debug_font, Convert.ToString (mouse_current.Y), debug_pos, Color.Yellow);
      //    */

      //    // brushes
      //    spriteBatch.DrawString (debug_font, "brush_grab: ", debug_pos, Color.Yellow);
      //    spriteBatch.DrawString (debug_font, Convert.ToString (character_control.character[PLAYER].brush_grab), debug_pos, Color.Yellow);

      //    if (character_control.character[(int) C.RICHARD].brush_grab > -1)
      //      {
      //      spriteBatch.DrawString (debug_font, "brush_control.brush[grab].x: ", debug_pos, Color.Yellow);
      //      spriteBatch.DrawString (debug_font, Convert.ToString (brush_control.brush[character_control.character[PLAYER].brush_grab].x), debug_pos, Color.Yellow);

      //      spriteBatch.DrawString (debug_font, "brush_control.brush[grab].y: ", debug_pos, Color.Yellow);
      //      spriteBatch.DrawString (debug_font, Convert.ToString (brush_control.brush[character_control.character[PLAYER].brush_grab].y), debug_pos, Color.Yellow);

      //      spriteBatch.DrawString (debug_font, "brush_control.brush[grab].moving: ", debug_pos, Color.Yellow);
      //      spriteBatch.DrawString (debug_font, Convert.ToString (brush_control.brush[character_control.character[PLAYER].brush_grab].moving), debug_pos, Color.Yellow);

      //      spriteBatch.DrawString (debug_font, "brush_control.brush[grab].moving_north: ", debug_pos, Color.Yellow);
      //      spriteBatch.DrawString (debug_font, Convert.ToString (brush_control.brush[character_control.character[PLAYER].brush_grab].moving_north), debug_pos, Color.Yellow);

      //      spriteBatch.DrawString (debug_font, "brush_control.brush[grab].moving_south: ", debug_pos, Color.Yellow);
      //      spriteBatch.DrawString (debug_font, Convert.ToString (brush_control.brush[character_control.character[PLAYER].brush_grab].moving_south), debug_pos, Color.Yellow);

      //      spriteBatch.DrawString (debug_font, "brush_control.brush[grab].moving_west: ", debug_pos, Color.Yellow);
      //      spriteBatch.DrawString (debug_font, Convert.ToString (brush_control.brush[character_control.character[PLAYER].brush_grab].moving_west), debug_pos, Color.Yellow);

      //      spriteBatch.DrawString (debug_font, "brush_control.brush[grab].moving_east: ", debug_pos, Color.Yellow);
      //      spriteBatch.DrawString (debug_font, Convert.ToString (brush_control.brush[character_control.character[PLAYER].brush_grab].moving_east), debug_pos, Color.Yellow);
      //      }

      //    // scroll box
      //    //shape.rectangle (spriteBatch, scroll_border.X, scroll_border.Y, scroll_border.X + scroll_border.Width, scroll_border.Y + scroll_border.Height, pixel_yellow, .5f);

      //    // test sprite
      //    //if (test_sprite == true) spriteBatch.Draw (light_sprite[green], Vector2.Zero, Color.White);
          }

        else if (game_state == CREATION)
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
        spriteBatch.DrawString (debug_font, debug_string, new Vector2 (50, 10), Color.LimeGreen);

        spriteBatch.End ();
        }

      fps_counter += 1;
      }

    ////////////////////////////////////////////////////////////////////////////////

    void begin_special_attack ()
      {
      if (character_control.character[PLAYER].shirt == (int) Object_Control.O.SHIRT_YELLOW) particle_superpunch (character_control.character[PLAYER].x, character_control.character[PLAYER].y, character_control.character[PLAYER].z + Convert.ToInt32 (character_control.character[PLAYER].height * .75), Convert.ToInt32 (MathHelper.ToDegrees (Convert.ToSingle (character_control.character[PLAYER].dir))));
      else if (character_control.character[PLAYER].shirt == (int) Object_Control.O.SHIRT_RED) particle_flamethrower (character_control.character[PLAYER].x, character_control.character[PLAYER].y, character_control.character[PLAYER].z + Convert.ToInt32 (character_control.character[PLAYER].height * .75), Convert.ToInt32 (MathHelper.ToDegrees (Convert.ToSingle (character_control.character[PLAYER].dir))), "character", PLAYER);
      else if (character_control.character[PLAYER].shirt == (int) Object_Control.O.SHIRT_WHITE) particle_freeze_ray (character_control.character[PLAYER].x, character_control.character[PLAYER].y, character_control.character[PLAYER].z + Convert.ToInt32 (character_control.character[PLAYER].height * .75), Convert.ToInt32 (MathHelper.ToDegrees (Convert.ToSingle (character_control.character[PLAYER].dir))), "character", PLAYER);
      }

    ////////////////////////////////////////////////////////////////////////////////

    void continue_special_attack ()
      {
      if (character_control.character[PLAYER].shirt == (int) Object_Control.O.SHIRT_YELLOW) character_control.character[PLAYER].action = "superpunch";
      else if (character_control.character[PLAYER].shirt == (int) Object_Control.O.SHIRT_RED) character_control.character[PLAYER].action = "flamethrower";
      else if (character_control.character[PLAYER].shirt == (int) Object_Control.O.SHIRT_WHITE) character_control.character[PLAYER].action = "freeze ray";
      }

    ////////////////////////////////////////////////////////////////////////////////

    void end_special_attack ()
      {
      if (character_control.character[PLAYER].action == "superpunch" || character_control.character[PLAYER].action == "freeze ray"
          || character_control.character[PLAYER].action == "flamethrower") character_control.character[PLAYER].action = "none";
      }

    ////////////////////////////////////////////////////////////////////////////////

    void character_throw_food (int c, double dir)
      {
      int r = 0;

      if (rock_color == 0) r = add_object ((int) Object_Control.O.ROCK, character_control.character[c].x, character_control.character[c].y, character_control.character[c].z + Convert.ToInt32 (character_control.character[c].height / 2));
      else if (rock_color == 1) r = add_object ((int) Object_Control.O.ROCK_BROWN, character_control.character[c].x, character_control.character[c].y, character_control.character[c].z + Convert.ToInt32 (character_control.character[c].height / 2));
      else if (rock_color == 2) r = add_object ((int) Object_Control.O.ROCK_RED, character_control.character[c].x, character_control.character[c].y, character_control.character[c].z + Convert.ToInt32 (character_control.character[c].height / 2));
      else if (rock_color == 3) r = add_object ((int) Object_Control.O.ROCK_WHITE, character_control.character[c].x, character_control.character[c].y, character_control.character[c].z + Convert.ToInt32 (character_control.character[c].height / 2));
      rock_color += 1;
      if (rock_color > 3) rock_color = 0;

      if (r > -1)
        {
        object_control.obj[r].dir = dir;
        object_control.obj[r].velocity = 7;// 8;
        object_control.obj[r].z_velocity = 1.6;// 1.5;
        object_control.obj[r].source = c;
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void character_grab_brush (int c)
      {
      int distance = 0;
      int max_reach = 50;
      Vector2 reach = Vector2.Zero;
      //int x_distance, y_distance;
      int b;
      int abs_x_distance, abs_y_distance;

      if (character_control.active (c) && character_on_ground (c))
        {
        character_control.character[c].brush_grab = -1;
        while (character_control.character[c].brush_grab == -1 && reach.X >= 0 && reach.X < map_max_width && reach.Y >= 0 && reach.Y < map_max_length && distance <= max_reach)
          {
          reach.X = Convert.ToSingle (character_control.character[c].x + (distance * Math.Cos (character_control.character[c].dir)));
          reach.Y = Convert.ToSingle (character_control.character[c].y + (distance * Math.Sin (character_control.character[c].dir)));

          // find box character is in front of
          character_control.character[c].brush_grab = brush_control.point_in_brush (Convert.ToInt32 (reach.X), Convert.ToInt32 (reach.Y), character_control.character[c].z, false, false);//true);
          distance += 1;
          }

        b = character_control.character[c].brush_grab;
        // only grab if moveable (done to prevent "push any wall with running start" bug)
        if (b < 0) { }  // not a brush_control.brush
        else if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.BOX_METAL_TEST && character_control.character[PLAYER].shirt != (int) Object_Control.O.SHIRT_YELLOW && character_control.character[PLAYER].shirt != (int) Object_Control.O.SHIRT_RED) { }  // too heavy
        else if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.BOX_ICE_TEST && character_control.character[PLAYER].shirt != (int) Object_Control.O.SHIRT_WHITE) { }  // too cold
        else if (brush_control.brush[b].moveable == true)
          {
          character_control.character[c].action = "grabbing";
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
          character_control.character[c].push_x = character_control.character[c].x;
          character_control.character[c].push_y = character_control.character[c].y;
          }
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void character_jump (int c)
      {
      if (character_control.active (c) && character_on_ground (c) && character_control.character[c].action != "grabbing" && character_control.character[c].action != "pushing")
        {
        // jumping counteracts effect of skidding, like mario
        if (character_control.character[c].action == "skidding")
          {
          character_control.character[c].ext_x_velocity = 0;
          character_control.character[c].ext_y_velocity = 0;
          }
        character_control.character[c].action = "jumping";
        character_control.character[c].self_z_velocity = 6;

        if (c == PLAYER) sound_richard_jump.Play ();
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void character_punch (int c)
      {
      if (character_control.active (c) && character_on_ground (c) && character_control.character[c].action != "grabbing" && character_control.character[c].action != "pushing")
        {
        character_control.character[c].action = "punching";
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
          if (character_near_object (c, o) && character_control.character_facing_object (character_control.character[c], object_control.obj[o]))
            {
            object_control.obj[o].dir = character_control.character[c].dir;
            object_control.obj[o].velocity = 3;// 2.5;
            object_control.obj[o].z_velocity = 1;
            }
          }
        //for (int c2 = 0; c2 < total_characters; c2 += 1)
        for (int c2 = 0; c2 < character_control.character.Count; c2 += 1)
          {
          if (c2 != c && character_control.reach_character (c, c2, brush_control.brush) && character_control.character[c].is_facing_character (character_control.character[c2]))
            {
            if (character_control.character[c].combo < 2)
              {
              if (character_control.character[c].shirt == (int) Object_Control.O.SHIRT_YELLOW) character_damage (c2, 15, 0, 0, character_control.character[c].x, character_control.character[c].y, "impact", c);
              else character_damage (c2, 10, 0, 0, character_control.character[c].x, character_control.character[c].y, "impact", c);
//              soundBank.PlayCue ("punch_test");
              sound_punch.Play ();
              }
            else if (character_control.character[c].combo == 2)
              {
              if (character_control.character[c].shirt == (int) Object_Control.O.SHIRT_YELLOW) character_damage (c2, 30, 0, 0, character_control.character[c].x, character_control.character[c].y, "impact", c);
              else character_damage (c2, 20, 0, 0, character_control.character[c].x, character_control.character[c].y, "impact", c);
//              soundBank.PlayCue ("kick_test");
              sound_kick.Play ();
              }
            else if (character_control.character[c].combo == 3)
              {
              if (character_control.character[c].shirt == (int) Object_Control.O.SHIRT_YELLOW) character_damage (c2, 60, 6, 9, character_control.character[c].x, character_control.character[c].y, "impact", c);
              else character_damage (c2, 40, 6, 9, character_control.character[c].x, character_control.character[c].y, "impact", c);
//              soundBank.PlayCue ("richard_buttsmash_test");
              sound_richard_buttsmash.Play ();
              }
            if (c == PLAYER) color_flasher (solid_white);
            }
          }
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void character_jump_kick (int c)
      {
      if (character_control.active (c) && character_control.character[c].action != "grabbing" && character_control.character[c].action != "pushing")
        {
        character_control.character[c].action = "jump kicking";
        //character_control.character[c].anim_frame_sequence = 0;
        //character_control.character[c].max_anim_frame_sequence = 15;
        //character_control.character[c].combo += 1;
        //if (character_control.character[c].combo > 3) character_control.character[c].combo = 0;
        character_control.character[c].self_velocity = 0;

        for (int o = 0; o < object_control.obj.Count; o += 1)
          {
          if (character_near_object (c, o))
            {
            object_control.obj[o].dir = character_control.character[c].dir;
            object_control.obj[o].velocity = 2.5;
            }
          }
        for (int c2 = 0; c2 < character_control.character.Count; c2 += 1)
          {
          if (c2 != c && character_control.reach_character (c, c2, brush_control.brush) && character_control.character[c].is_facing_character (character_control.character[c2]))
            {
            character_damage (c2, 30, 7, 0, character_control.character[c].x, character_control.character[c].y, "impact", c);
            sound_kick.Play ();
            if (c == PLAYER) color_flasher (solid_white);
            }
          }
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void character_damage (int c, int damage, double force, double vertical_force, int x_origin, int y_origin, string damage_type, int source)
      {
      if (character_control.character[c].injury_timer == 0)
        {
        character_control.character[c].health -= damage;
        character_control.character[c].action = "stunned";
        character_control.character[c].blinking = false;
        character_control.character[c].anim_frame_sequence = 0;
        character_control.character[c].max_anim_frame_sequence = 30;     // number of frames to stun character for after getting hit
        character_control.character[c].self_velocity = 0;
        character_control.character[c].apply_force_from_point (force, x_origin, y_origin);
        character_control.character[c].ext_z_velocity = vertical_force;  // goes airborn
        character_control.character[c].injury_timer = 10;
        if (damage_type == "impact") character_control.pow (c, false, false);

        // attack source of damage (like doom)
        if (c != PLAYER && character_control.active (source)) character_control.character[c].attack_character (character_control.character[source], source);

        // red flasher for player damage
        if (c == PLAYER)
          {
          if (damage_type == "impact")
            //soundBank.PlayCue ("richard_hurt_test");
            sound_richard_hurt.Play ();
          color_flasher (solid_red);
          }
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    //void character_pow (int c, bool low, bool behind)
    //  {
    //  character_control.character[c].pow1.opacity = .75f;// 1f;
    //  character_control.character[c].pow1.x = -1 * (pow_sprite_width / 2);
    //  character_control.character[c].pow1.y = 0 - character_control.character[c].sprite_height;
    //  character_control.character[c].pow1.behind = behind;
    //  character_control.character[c].pow1.color = rnd.Next (0, 3);
    //  character_control.character[c].pow1.shape = rnd.Next (0, 5);
    //  }

    ////////////////////////////////////////////////////////////////////////////////

    int add_fixture (int sprite_number, int x, int y, int z)
      {
      if (fixture_control.fixture.Count < Fixture_Control.max_fixtures)
        {
        fixture_control.add (sprite_number, x, y, z);
        total_draw_slots += 1;
        }

      return fixture_control.fixture.Count - 1;
      }

    ////////////////////////////////////////////////////////////////////////////////

    void Update_Fixtures ()
      {
      int f2, q, b_clip;

      //for (int f = 0; f < total_fixtures; f += 1)
      for (int f = 0; f < fixture_control.fixture.Count; f += 1)
        {
        if (fixture_control.fixture[f].total_frames > 1)
          {
          fixture_control.fixture[f].frame_counter += 1;
          if (fixture_control.fixture[f].frame_counter >= fixture_control.fixture[f].frame_delay)
            {
            fixture_control.fixture[f].frame_counter = 0;
            fixture_control.fixture[f].current_frame += 1;
            if (fixture_control.fixture[f].current_frame >= fixture_control.fixture[f].total_frames) fixture_control.fixture[f].current_frame = 0;
            }
          }

        // laser tripwires
        if (fixture_control.fixture[f].type == (int) Fixture_Control.F.LASER_HORIZONTAL_GREEN_TEST)
          {
          b_clip = fixture_control.fixture_in_brush (fixture_control.fixture[f], brush_control.brush);
          if (b_clip > -1) fixture_control.fixture[f].powered = true;
          }

        // power
        if (fixture_control.fixture[f].electric == true && fixture_control.fixture[f].powered == false)
          {
          //f2 = fixture_control.point_collide (fixture_control.fixture[f].x + fixture_control.fixture[f].width + (tilesize / 2), fixture_control.fixture[f].y + (fixture_control.fixture[f].length / 2), fixture_control.fixture[f].z);
          //if (f2 > -1)// && fixture_control.fixture[f2].electric == true)// && fixture_control.fixture[f2].powered == true)
          //fixture[f].powered = true;
          //else
          f2 = fixture_control.point_collide (fixture_control.fixture[f].x + fixture_control.fixture[f].width + (tilesize / 2), fixture_control.fixture[f].y + (fixture_control.fixture[f].length / 2), fixture_control.fixture[f].z + 8);
          if (f2 > -1 && fixture_control.fixture[f2].type == (int) Fixture_Control.F.LASER_HORIZONTAL_GREEN_TEST && fixture_control.fixture[f2].electric == true && fixture_control.fixture[f2].powered == true)
            fixture_control.fixture[f].powered = true;
          //else f2 = fixture_control.point_collide (fixture_control.fixture[f].x + fixture_control.fixture[f].width + (tilesize / 2), fixture_control.fixture[f].y + (fixture_control.fixture[f].length / 2), fixture_control.fixture[f].z + (tilesize / 2));
          //if (f2 > -1)// && fixture_control.fixture[f2].electric == true)// && fixture_control.fixture[f2].powered == true) 
          //  fixture_control.fixture[f].powered = true;
          //else f2 = fixture_control.point_collide (fixture_control.fixture[f].x + fixture_control.fixture[f].width + (tilesize / 2), fixture_control.fixture[f].y + (fixture_control.fixture[f].length / 2), fixture_control.fixture[f].z + tilesize);
          //if (f2 > -1)// && fixture_control.fixture[f2].electric == true)// && fixture_control.fixture[f2].powered == true) 
          //  fixture_control.fixture[f].powered = true;
          }

        // particles
        if (fixture_control.fixture[f].type == (int) Fixture_Control.F.LASER_HORIZONTAL_GREEN_TEST)
          {
          if (rnd.Next (0, 50) == 0) particle_green_tripwire (fixture_control.fixture[f].x + 6, fixture_control.fixture[f].y + 17, fixture_control.fixture[f].z + fixture_control.fixture[f].height, 0);
          if (rnd.Next (0, 50) == 0) particle_green_tripwire (fixture_control.fixture[f].x + fixture_control.fixture[f].width - 6, fixture_control.fixture[f].y + 17, fixture_control.fixture[f].z + fixture_control.fixture[f].height, 180);
          }

        }  // for (int f = 0
      }

    ///////////////////////////////////////////////////////////////////////////////

      void Add_Stickers ()
        {
        bool add_top, add_front;
        //Texture2D sticker;
        //int sticker_number;
        //int sticker_tile_x;
        //int sticker_tile_y;
        int found_brush;
        int found_fixture;
        //int x, y, z;
        //float alpha;

        for (int b = 0; b < brush_control.brush.Count; b += 1)
          {
          add_top = true;
          add_front = true;

          // top stickers
          if (brush_control.brush[b].top_sticker > 0) add_top = false;  // already has sticker

          found_brush = brush_control.brush_west_of_brush (brush_control.brush[b]);
          if (found_brush > -1 && brush_control.brush[found_brush].top_sticker > 0) add_top = false;  // stickers too close

          found_brush = brush_control.brush_east_of_brush (brush_control.brush[b]);
          if (found_brush > -1 && brush_control.brush[found_brush].top_sticker > 0) add_top = false;  // stickers too close

          found_brush = brush_control.brush_above_brush (brush_control.brush[b]);
          if (found_brush > -1 && brush_control.brush[found_brush].top_sticker > 0) add_top = false;  // sticker under wall

          found_fixture = fixture_control.fixture_above_brush (brush_control.brush[b]);
          if (found_fixture > -1) add_top = false;  // fixture sitting on floor

          if (add_top)
            {
            if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.BRICK_RED_TEST && rnd.Next (0, 15) == 1) apply_top_sticker (b, "factory");
            if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.TILE_BLACK_TEST && rnd.Next (0, 75) == 0) apply_top_sticker (b, "factory floor");
            if (brush_control.brush[b].top_texture_number == (int) Brush_Control.T.TILE_BROWN_TEST && rnd.Next (0, 150) == 0) apply_top_sticker (b, "office floor");
            }

          // front stickers
          if (brush_control.brush[b].front_sticker > 0) add_front = false;  // already has sticker

          found_brush = brush_control.brush_west_of_brush (brush_control.brush[b]);
          if (found_brush > -1 && brush_control.brush[found_brush].front_sticker > 0) add_front = false;  // stickers too close

          found_brush = brush_control.brush_east_of_brush (brush_control.brush[b]);
          if (found_brush > -1 && brush_control.brush[found_brush].front_sticker > 0) add_front = false;  // stickers too close

          found_brush = brush_control.brush_below_south_of_brush (brush_control.brush[b]);
          if (found_brush == -1) add_front = false;  // sticker outside building

          found_fixture = fixture_control.fixture_south_of_brush (brush_control.brush[b]);
          if (found_fixture > -1) add_front = false;  // sticker behind fixture

          if (add_front)
            {
            if (brush_control.brush[b].front_texture_number == (int) Brush_Control.T.BRICK_RED_TEST && rnd.Next (0, 8) == 0) apply_front_sticker (b, "factory");
            if (brush_control.brush[b].front_texture_number == (int) Brush_Control.T.DRYWALL_MINT_FRONT_TEST && rnd.Next (0, 8) == 0) apply_front_sticker (b, "office");
            if (brush_control.brush[b].front_texture_number == (int) Brush_Control.T.DRYWALL_PURPLE_FRONT_TEST && rnd.Next (0, 8) == 0) apply_front_sticker (b, "office");
            if (brush_control.brush[b].front_texture_number == (int) Brush_Control.T.DRYWALL_TAN_FRONT_TEST && rnd.Next (0, 8) == 0) apply_front_sticker (b, "office");
            if (brush_control.brush[b].front_texture_number == (int) Brush_Control.T.DRYWALL_YELLOW_FRONT_TEST && rnd.Next (0, 8) == 0) apply_front_sticker (b, "office");
            if (brush_control.brush[b].front_texture_number == (int) Brush_Control.T.METAL_MINT_FRONT_TEST && rnd.Next (0, 8) == 0) apply_front_sticker (b, "factory");
            }
          }

        // remove stickers from walls not visible
        //for (int b = 0; b < brush_control.brush.Count; b += 1)
        //  {
        //  if (brush_control.brush[b].front_sticker != 0
        //      && brush_control.point_in_brush (brush_control.brush[b].x + (brush_control.brush[b].width / 2), brush_control.brush[b].y - (tilesize / 2), brush_control.brush[b].z + (brush_control.brush[b].height / 2), true, false) > -1)
        //    brush_control.brush[b].front_sticker = 0;
        //  }
        }

      ///////////////////////////////////////////////////////////////////////////////

      void apply_top_sticker (int b, string sticker_type)
        {
        int sticker_number;
        Texture2D sticker;
        float alpha;
        int sticker_tile_x, sticker_tile_y;
        int x, y, z;
        int b2;

        if (sticker_type == "factory")
          {
          sticker_number = rnd.Next (0, total_factory_stickers);
          sticker = sticker_factory[sticker_number];
          alpha = 1f;
          }
        else if (sticker_type == "factory floor")
          {
          sticker_number = rnd.Next (0, total_factory_floor_stickers);
          sticker = sticker_factory_floor[sticker_number];
          if (sticker_number == 0) alpha = .1f;
          else alpha = 1f;
          }
        else
          {
          // default is office floor
          sticker_type = "office floor";
          sticker_number = rnd.Next (0, total_office_floor_stickers);
          sticker = sticker_office_floor[sticker_number];
          alpha = 1f;
          }

        sticker_tile_x = sticker.Width / tilesize;
        sticker_tile_y = sticker.Height / tilesize;

        z = 0;
        for (y = 0; y >= (sticker_tile_y - 1) * -1; y -= 1)
          {
          for (x = 0; x <= sticker_tile_x - 1; x += 1)
            {
            b2 = brush_control.brush_around_brush (b, x, y, z);
            if (b2 == -1) sticker_number = -1;  // brush_control.brush exists
            else if (brush_control.brush[b2].top_texture_number != brush_control.brush[b].top_texture_number) sticker_number = -1;  // textures match
            else if (brush_control.brush[b2].top_sticker > -1) sticker_number = -1;  // stickerless
            }
          }

        if (sticker_number > -1)
          {
          z = 0;
          for (y = 0; y >= (sticker_tile_y - 1) * -1; y -= 1)
            {
            for (x = 0; x <= sticker_tile_x - 1; x += 1)
              {
              b2 = brush_control.brush_around_brush (b, x, y, z);
              brush_control.brush[b2].top_sticker = sticker_number;
              brush_control.brush[b2].top_sticker_type = sticker_type;
              brush_control.brush[b2].top_sticker_offset_x = x * tilesize;
              brush_control.brush[b2].top_sticker_offset_y = y * -1 * tilesize;
              brush_control.brush[b2].top_sticker_alpha = alpha;
              }
            }
          }
        }

      ///////////////////////////////////////////////////////////////////////////////

      void apply_front_sticker (int b, string sticker_type)
        {
        int sticker_number;
        Texture2D sticker;
        float alpha;
        int sticker_tile_x, sticker_tile_z;
        int x, y, z;
        int b2;

        if (sticker_type == "factory")
          {
          sticker_number = rnd.Next (0, total_factory_stickers);
          sticker = sticker_factory[sticker_number];
          alpha = 1f;
          }
        else
          {
          // default is office
          sticker_type = "office";
          sticker_number = rnd.Next (0, total_office_stickers);
          sticker = sticker_office[sticker_number];
          alpha = 1f;
          }

        sticker_tile_x = sticker.Width / tilesize;
        sticker_tile_z = sticker.Height / tilesize;

        y = 0;
        for (z = 0; z >= (sticker_tile_z - 1) * -1; z -= 1)
          {
          for (x = 0; x <= sticker_tile_x - 1; x += 1)
            {
            b2 = brush_control.brush_around_brush (b, x, y, z);
            if (b2 == -1) sticker_number = -1;  // brush_control.brush exists
            else if (brush_control.brush[b2].front_texture_number != brush_control.brush[b].front_texture_number) sticker_number = -1;  // textures match
            else if (brush_control.brush[b2].front_sticker > -1) sticker_number = -1;  // stickerless
            }
          }

        if (sticker_number > -1)
          {
          y = 0;
          for (z = 0; z >= (sticker_tile_z - 1) * -1; z -= 1)
            {
            for (x = 0; x <= sticker_tile_x - 1; x += 1)
              {
              b2 = brush_control.brush_around_brush (b, x, y, z);
              brush_control.brush[b2].front_sticker = sticker_number;
              brush_control.brush[b2].front_sticker_type = sticker_type;
              brush_control.brush[b2].front_sticker_offset_x = x * tilesize;
              brush_control.brush[b2].front_sticker_offset_y = z * -1 * tilesize;
              brush_control.brush[b2].front_sticker_alpha = alpha;
              }
            }
          }
        }

      ////////////////////////////////////////////////////////////////////////////////

      int brush_on_fixture (Brush b)
        {
        int fixture_below = fixture_control.point_collide (b.x + (b.width / 2), b.y + (b.length / 2), b.z - 1);
        if (fixture_below == -1) fixture_below = fixture_control.point_collide (b.x + 1, b.y + 1, b.z - 1);
        if (fixture_below == -1) fixture_below = fixture_control.point_collide (b.x + b.width - 1, b.y + 1, b.z - 1);
        if (fixture_below == -1) fixture_below = fixture_control.point_collide (b.x + 1, b.y + b.length - 1, b.z - 1);
        if (fixture_below == -1) fixture_below = fixture_control.point_collide (b.x + b.width - 1, b.y + b.length - 1, b.z - 1);

        return fixture_below;
        }

      //////////////////////////////////////////////////////////////////////////////////

      //int brush_in_fixture (Brush b, bool solid_only)
      //  {
      //  int f = 0;
      //  int clip = -1;

      //  while (clip == -1 && f < fixture_control.fixture.Count)
      //    {
      //    if (b.x + b.width > fixture_control.fixture[f].x && b.x < fixture_control.fixture[f].x + fixture_control.fixture[f].width
      //        && b.y + b.length > fixture_control.fixture[f].y && b.y < fixture_control.fixture[f].y + fixture_control.fixture[f].length
      //        && b.z + b.height > fixture_control.fixture[f].z && b.z < fixture_control.fixture[f].z + fixture_control.fixture[f].height - 1)
      //      {
      //      if (solid_only == true && fixture_control.fixture[f].solid == false)
      //        {
      //        clip = -1;
      //        if (fixture_control.fixture[f].type == (int) Fixture_Control.F.LASER_HORIZONTAL_GREEN_TEST) fixture_control.fixture[f].powered = true;
      //        }
      //      else clip = f;
      //      }
      //    f += 1;
      //    }

      //  return clip;
      //  }

    ////////////////////////////////////////////////////////////////////////////////

    void push_north ()
      {
      int b;

      if (character_control.character[PLAYER].action == "grabbing" && brush_control.brush[character_control.character[PLAYER].brush_grab].moveable == true && character_control.character[PLAYER].grab_position == "below")  // push box (on grid)
        {
        if (brush_control.brush[character_control.character[PLAYER].brush_grab].top_texture_number == (int) Brush_Control.T.BOX_METAL_TEST && character_control.character[PLAYER].shirt != (int) Object_Control.O.SHIRT_YELLOW && character_control.character[PLAYER].shirt != (int) Object_Control.O.SHIRT_RED) return;
        else if (brush_control.brush[character_control.character[PLAYER].brush_grab].top_texture_number == (int) Brush_Control.T.BOX_ICE_TEST && character_control.character[PLAYER].shirt != (int) Object_Control.O.SHIRT_WHITE) return;
        else if (fixture_control.fixture_north_of_brush (brush_control.brush[character_control.character[PLAYER].brush_grab]) > -1) return;
        else
          {
          character_control.character[PLAYER].action = "pushing";
          b = character_control.character[PLAYER].brush_grab;
          brush_control.brush[b].moving = true;
          brush_control.brush[b].moving_north = true;
          brush_control.brush[b].destination_x = brush_control.brush[b].x;
          brush_control.brush[b].destination_y = brush_control.brush[b].y + box_move;
          brush_control.brush[b].ext_y_velocity = character_control.character[PLAYER].speed * .45;
          character_control.character[PLAYER].dx = brush_control.brush[b].x + (brush_control.brush[b].width / 2);
          character_control.character[PLAYER].dy = brush_control.brush[b].y - (tilesize / 3);
          character_control.character[PLAYER].push_x = character_control.character[PLAYER].x;
          character_control.character[PLAYER].push_y = character_control.character[PLAYER].y + box_move;
          character_control.character[PLAYER].push_dir = "up";
          character_control.character[PLAYER].self_x_velocity = 0;
          }
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void push_south ()
      {
      int b;

      if (character_control.character[PLAYER].action == "grabbing")// && brush_control.brush[character_control.character[PLAYER].brush_grab].moveable == true && character_control.character[PLAYER].grab_position == "above")
        {
        if (brush_control.brush[character_control.character[PLAYER].brush_grab].top_texture_number == (int) Brush_Control.T.BOX_METAL_TEST && character_control.character[PLAYER].shirt != (int) Object_Control.O.SHIRT_YELLOW && character_control.character[PLAYER].shirt != (int) Object_Control.O.SHIRT_RED) return;
        else if (brush_control.brush[character_control.character[PLAYER].brush_grab].top_texture_number == (int) Brush_Control.T.BOX_ICE_TEST && character_control.character[PLAYER].shirt != (int) Object_Control.O.SHIRT_WHITE) return;
        else if (fixture_control.fixture_south_of_brush (brush_control.brush[character_control.character[PLAYER].brush_grab]) > -1) return;
        else
          {
          character_control.character[PLAYER].action = "pushing";
          b = character_control.character[PLAYER].brush_grab;
          brush_control.brush[b].moving = true;
          brush_control.brush[b].moving_south = true;
          brush_control.brush[b].destination_x = brush_control.brush[b].x;
          brush_control.brush[b].destination_y = brush_control.brush[b].y - box_move;
          brush_control.brush[b].ext_y_velocity = character_control.character[PLAYER].speed * .45;
          character_control.character[PLAYER].dx = brush_control.brush[b].x + (brush_control.brush[b].width / 2);
          character_control.character[PLAYER].dy = brush_control.brush[b].y + brush_control.brush[b].length + (tilesize / 4);
          character_control.character[PLAYER].push_x = character_control.character[PLAYER].x;
          character_control.character[PLAYER].push_y = character_control.character[PLAYER].y - box_move;
          character_control.character[PLAYER].push_dir = "down";
          character_control.character[PLAYER].self_x_velocity = 0;
          }
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void push_west ()
      {
      int b;

      if (character_control.character[PLAYER].action == "grabbing" && brush_control.brush[character_control.character[PLAYER].brush_grab].moveable == true && character_control.character[PLAYER].grab_position == "right")  // push box (on grid)
        {
        if (brush_control.brush[character_control.character[PLAYER].brush_grab].top_texture_number == (int) Brush_Control.T.BOX_METAL_TEST && character_control.character[PLAYER].shirt != (int) Object_Control.O.SHIRT_YELLOW && character_control.character[PLAYER].shirt != (int) Object_Control.O.SHIRT_RED) return;
        else if (brush_control.brush[character_control.character[PLAYER].brush_grab].top_texture_number == (int) Brush_Control.T.BOX_ICE_TEST && character_control.character[PLAYER].shirt != (int) Object_Control.O.SHIRT_WHITE) { }
        else if (fixture_control.fixture_west_of_brush (brush_control.brush[character_control.character[PLAYER].brush_grab]) > -1) return;
        else
          {
          character_control.character[PLAYER].action = "pushing";
          b = character_control.character[PLAYER].brush_grab;
          brush_control.brush[b].moving = true;
          brush_control.brush[b].moving_west = true;
          brush_control.brush[b].destination_x = brush_control.brush[b].x - box_move;
          brush_control.brush[b].destination_y = brush_control.brush[b].y;
          brush_control.brush[b].ext_x_velocity = character_control.character[PLAYER].speed * .45;
          character_control.character[PLAYER].dx = brush_control.brush[b].x + brush_control.brush[b].width + (tilesize / 3);
          character_control.character[PLAYER].dy = brush_control.brush[b].y + (brush_control.brush[b].length / 2);
          character_control.character[PLAYER].push_x = character_control.character[PLAYER].x - box_move;
          character_control.character[PLAYER].push_y = character_control.character[PLAYER].y;
          character_control.character[PLAYER].push_dir = "left";
          character_control.character[PLAYER].self_y_velocity = 0;
          }
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void push_east ()
      {
      int b;

      if (character_control.character[PLAYER].action == "grabbing" && brush_control.brush[character_control.character[PLAYER].brush_grab].moveable == true && character_control.character[PLAYER].grab_position == "left")  // push box (on grid)
        {
        if (brush_control.brush[character_control.character[PLAYER].brush_grab].top_texture_number == (int) Brush_Control.T.BOX_METAL_TEST && character_control.character[PLAYER].shirt != (int) Object_Control.O.SHIRT_YELLOW && character_control.character[PLAYER].shirt != (int) Object_Control.O.SHIRT_RED) return;
        else if (brush_control.brush[character_control.character[PLAYER].brush_grab].top_texture_number == (int) Brush_Control.T.BOX_ICE_TEST && character_control.character[PLAYER].shirt != (int) Object_Control.O.SHIRT_WHITE) return;
        else if (fixture_control.fixture_east_of_brush (brush_control.brush[character_control.character[PLAYER].brush_grab]) > -1) return;
        else
          {
          character_control.character[PLAYER].action = "pushing";
          b = character_control.character[PLAYER].brush_grab;
          brush_control.brush[b].moving = true;
          brush_control.brush[b].moving_east = true;
          brush_control.brush[b].destination_x = brush_control.brush[b].x + box_move;
          brush_control.brush[b].destination_y = brush_control.brush[b].y;
          brush_control.brush[b].ext_x_velocity = character_control.character[PLAYER].speed * .45;
          character_control.character[PLAYER].dx = brush_control.brush[b].x - (tilesize / 3);
          character_control.character[PLAYER].dy = brush_control.brush[b].y + (brush_control.brush[b].length / 2);
          character_control.character[PLAYER].push_x = character_control.character[PLAYER].x + box_move;
          character_control.character[PLAYER].push_y = character_control.character[PLAYER].y;
          character_control.character[PLAYER].push_dir = "right";
          character_control.character[PLAYER].self_y_velocity = 0;
          }
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void continue_to_target (int c)
      {
          int tx, ty, tz;  // test location
          double td;       // test direction
          double xmove, ymove;
          //bool endloop, endloop2;
          int b;
          //bool positive;
          //int rotation;

          // face target
          //if (character_control.character[c].target_type == "object") character_control.character[c].dir = get_direction (character_control.character[c].x, character_control.character[c].y, obj[character_control.character[c].target].x, obj[character_control.character[c].target].y);
          //else if (character_control.character[c].target_type == "character")
          character_control.character[c].dir = get_direction (character_control.character[c].x, character_control.character[c].y, character_control.character[character_control.character[c].target].x, character_control.character[character_control.character[c].target].y);

          //positive = false;
          //rotation = 0;
          //endloop2 = false;
          //while (!endloop2)
          //  {
          // test for projected collision on path
          tx = character_control.character[c].x;
          ty = character_control.character[c].y;
          tz = character_control.character[c].z;
          td = character_control.character[c].dir;

          xmove = character_control.reach_distance / 4 * Math.Cos (td);
          ymove = character_control.reach_distance / 4 * Math.Sin (td);

          b = -1;
          bool endloop = false;
          while (!endloop)
            {
            tx += Convert.ToInt16 (xmove);
            ty += Convert.ToInt16 (ymove);
            b = brush_control.point_in_brush (tx, ty, tz, false, true);
            if (b >= 0) endloop = true;  // hit wall

            // if hit target or
            // went passed target without hitting wall or
            // made it more than 2 tiles without hitting wall
            if (point_near_point (tx, ty, tz, character_control.character[c].target_x, character_control.character[c].target_y, character_control.character[c].target_z)
                || distance2d (tx, ty, character_control.character[c].target_x, character_control.character[c].target_y) > distance2d (character_control.character[c].x, character_control.character[c].y, character_control.character[c].target_x, character_control.character[c].target_y))
            //|| distance2d (character_control.character[c].x, character_control.character[c].y, tx, ty) > tilesize * 2)
              {
              endloop = true;
              //      endloop2 = true;
              //      character_control.character[c].subtarget_x = tx;
              //      character_control.character[c].subtarget_y = ty;
              //      character_control.character[c].dir = td;
              }
            }

          if (b >= 0)  // hit wall
            {
            //    rotation += 15;
            //    if (positive == true) positive = false;
            //    else positive = true;
            //    if (positive == true) td = character_control.character[c].dir + MathHelper.ToRadians(rotation);
            //    else td = character_control.character[c].dir - MathHelper.ToRadians(rotation);
            //    if (td >= MathHelper.ToRadians(360)) td -= MathHelper.ToRadians(360);
            //    if (td < MathHelper.ToRadians(0)) td += MathHelper.ToRadians(360);
            //    if (rotation > 720)  // trapped
            //      {
            //      endloop = true;
            //      endloop2 = true;
            //      td = character_control.character[c].dir;
            //      }

            character_control.character[c].dir = get_direction (character_control.character[c].x, character_control.character[c].y, character_control.character[character_control.character[c].target].x, character_control.character[character_control.character[c].target].y);
            character_control.character[c].subtarget_x = character_control.character[character_control.character[c].target].x;
            character_control.character[c].subtarget_y = character_control.character[character_control.character[c].target].y;

            }
      //  }

      //if (character_control.character[c].subtarget_x != -1) character_control.character[c].dir = get_direction (character_control.character[c].x, character_control.character[c].y, character_control.character[c].subtarget_x, character_control.character[c].subtarget_y);
      //else
      //  {
      //  if (character_control.character[c].target_type == "object") character_control.character[c].dir = get_direction (character_control.character[c].x, character_control.character[c].y, obj[character_control.character[c].target].x, obj[character_control.character[c].target].y);
      //  else if (character_control.character[c].target_type == "character") character_control.character[c].dir = get_direction (character_control.character[c].x, character_control.character[c].y, character_control.character[character_control.character[c].target].x, character_control.character[character_control.character[c].target].y);
      //  }
      //character_control.character[c].self_velocity = character_control.character[c].speed * .3;

      //if (character_control.character[c].target_in_sight (character_control.character[character_control.character[c].target]))
      //  {
      //  character_control.character[c].dir = get_direction (character_control.character[c].x, character_control.character[c].y, character_control.character[character_control.character[c].target].x, character_control.character[character_control.character[c].target].y);
      //  character_control.character[c].subtarget_x = character_control.character[character_control.character[c].target].x;
      //  character_control.character[c].subtarget_y = character_control.character[character_control.character[c].target].y;
      //  }
      }

    ////////////////////////////////////////////////////////////////////////////////

    //int character_on_fixture (Character c)
    //  {
    //  // reset point for character and not for brush_control.brush?
    //  int fixture_below = fixture_control.point_collide (c.x + (c.width / 2), c.y + (c.length / 2), c.z - 1);
    //  if (fixture_below == -1) fixture_below = fixture_control.point_collide (c.x + 1, c.y + 1, c.z - 1);
    //  if (fixture_below == -1) fixture_below = fixture_control.point_collide (c.x + c.width - 1, c.y + 1, c.z - 1);
    //  if (fixture_below == -1) fixture_below = fixture_control.point_collide (c.x + 1, c.y + c.length - 1, c.z - 1);
    //  if (fixture_below == -1) fixture_below = fixture_control.point_collide (c.x + c.width - 1, c.y + c.length - 1, c.z - 1);

    //  return fixture_below;
    //  }

    ////////////////////////////////////////////////////////////////////////////////

    //bool character_sees_character (int c1, int c2)
    //  {
    //  // can character 1 see character 2?

    //  bool sees_character = true;
    //  double eye_x = character_control.character[c1].x;
    //  double eye_y = character_control.character[c1].y;
    //  double eye_z = character_control.character[c1].z + character_control.character[c1].height;
    //  double eye_dir = get_direction (character_control.character[c1].x, character_control.character[c1].y, character_control.character[c2].x, character_control.character[c2].y);
    //  double distance = distance2d (eye_x, eye_y, character_control.character[c2].x, character_control.character[c2].y);
    //  bool endloop = false;

    //  while (endloop == false)
    //    {
    //    eye_x += 4 * Math.Cos (eye_dir);
    //    eye_y += 4 * Math.Sin (eye_dir);
    //    if (brush_control.point_in_brush (Convert.ToInt32 (eye_x), Convert.ToInt32 (eye_y), Convert.ToInt32 (eye_z), true, true) >= 0) { sees_character = false; endloop = true; }
    //    if (distance2d (character_control.character[c1].x, character_control.character[c1].y, eye_x, eye_y) >= distance) { sees_character = true; endloop = true; }
    //    }

    //  return sees_character;
    //  }

    //////////////////////////////////////////////////////////////////////////////////

    //bool character_reach_character (int c1, int c2)
    //  {
    //  bool reach = false;
    //  double x_distance, y_distance, z_distance, h_distance;
    //  double arm_x, arm_y, arm_z;

    //  double reach_distance2 = character_control.reach_distance;
    //  if (c1 == PLAYER) reach_distance2 = character_control.reach_distance * 1.2;

    //  x_distance = character_control.character[c1].dx - character_control.character[c2].dx;
    //  y_distance = character_control.character[c1].dy - character_control.character[c2].dy;
    //  z_distance = Math.Abs (character_control.character[c1].dz - character_control.character[c2].dz);
    //  h_distance = Math.Sqrt ((x_distance * x_distance) + (y_distance * y_distance));

    //  // if he's close enough to hit him
    //  if (h_distance < reach_distance2 && z_distance < reach_distance2 * 2)
    //    {
    //    reach = true;

    //    arm_x = character_control.character[c1].x;
    //    arm_y = character_control.character[c1].y;
    //    arm_z = character_control.character[c1].z;

    //    // if there are no walls between them
    //    for (int d = 0; d < h_distance; d += 1)
    //      {
    //      arm_x += 1 * Math.Cos (Convert.ToInt32 (character_control.character[c1].dir));
    //      if (brush_control.point_in_brush (Convert.ToInt16 (arm_x), Convert.ToInt16 (arm_y), Convert.ToInt16 (arm_z), true, true) >= 0) reach = false;

    //      arm_y += 1 * Math.Sin (character_control.character[c1].dir);
    //      if (brush_control.point_in_brush (Convert.ToInt16 (arm_x), Convert.ToInt16 (arm_y), Convert.ToInt16 (arm_z), true, true) >= 0) reach = false;
    //      }
    //    }

    //  return reach;
    //  }

    ////////////////////////////////////////////////////////////////////////////////

    void draw_character (int c)//, SpriteBatch sb)
      {
      int tx, ty, tz;
      bool endloop, endloop2;
      int b, f;
      int distance, rotation;
      float temp_fade;
      double shadow_scale;
      Rectangle r_source, r_draw, r_shadow;
      Vector2 v_draw, v_draw2, v_origin, v_subtarget;

      //spriteBatch.Begin (SpriteSortMode.Immediate, BlendState.AlphaBlend);

      // draw floor shadow
      tx = character_control.character[c].x;
      ty = character_control.character[c].y;
      tz = character_control.character[c].z;

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

      distance = character_control.character[c].z - tz;
      //temp_fade = Convert.ToSingle (.3f - (distance / 300f));
      //if (temp_fade < .1f) temp_fade = .1f;
      temp_fade = Convert.ToSingle (.4f - (distance / 300f));
      if (temp_fade < .2f) temp_fade = .2f;
      //shadow_scale = 1.0 + (distance / 128.0);
      shadow_scale = 1.0 + (distance / 192.0);

      // find animation frame
      r_source.X = 2 + ((character_control.character[c].sprite_width + 1) * character_control.character[c].anim_frame);

      // find frames for direction facing
      if (character_control.character[c].dir >= MathHelper.ToRadians (225) && character_control.character[c].dir <= MathHelper.ToRadians (315)) r_source.Y = 1;
      else if (character_control.character[c].dir > MathHelper.ToRadians (45) && character_control.character[c].dir < MathHelper.ToRadians (135)) r_source.Y = 1 + character_control.character[c].sprite_height + 1;
      else if (character_control.character[c].dir >= MathHelper.ToRadians (135) && character_control.character[c].dir < MathHelper.ToRadians (225)) r_source.Y = 1 + (character_control.character[c].sprite_height + 1) * 2;
      else if (character_control.character[c].dir <= MathHelper.ToRadians (45) || character_control.character[c].dir > MathHelper.ToRadians (315)) r_source.Y = 1 + (character_control.character[c].sprite_height + 1) * 3;
      else r_source.Y = 1;// +(character_control.character[c].sprite_height + 1) * 3;

      r_source.Width = character_control.character[c].sprite_width - 1;
      r_source.Height = character_control.character[c].sprite_height;

      r_draw.Width = Convert.ToInt32 (character_control.character[c].sprite_width * shadow_scale);
      r_draw.Height = Convert.ToInt32 (character_control.character[c].sprite_height / 4 * shadow_scale);
      //r_draw.Width = r_source.Width;
      //r_draw.Height = r_source.Height;
      r_draw.X = tx + screen.scroll_x + 1;
      r_draw.Y = y_draw_coordinate (ty, tz) - (character_control.character[c].walk_pixels + 1);// / 2);

      if (dynamic_shadows)
        {
        v_origin.X = character_control.character[c].sprite_width / 2;
        v_origin.Y = 0;

        spriteBatch.Draw (character_control.character_sprite[character_control.character[c].sprite, 0], r_draw, r_source, Color.Black * temp_fade, MathHelper.ToRadians (0), v_origin, SpriteEffects.FlipVertically, 0);
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
      r_source.X = 1 + ((character_control.character[c].sprite_width + 1) * character_control.character[c].anim_frame);

      // find frames for direction facing
      if (character_control.character[c].dir >= MathHelper.ToRadians (225) && character_control.character[c].dir <= MathHelper.ToRadians (315)) r_source.Y = 1;
      else if (character_control.character[c].dir > MathHelper.ToRadians (45) && character_control.character[c].dir < MathHelper.ToRadians (135)) r_source.Y = 1 + character_control.character[c].sprite_height + 1;
      else if (character_control.character[c].dir >= MathHelper.ToRadians (135) && character_control.character[c].dir < MathHelper.ToRadians (225)) r_source.Y = 1 + (character_control.character[c].sprite_height + 1) * 2;
      else if (character_control.character[c].dir <= MathHelper.ToRadians (45) || character_control.character[c].dir > MathHelper.ToRadians (315)) r_source.Y = 1 + (character_control.character[c].sprite_height + 1) * 3;
      else r_source.Y = 1;// +(character_control.character[c].sprite_height + 1) * 3;

      r_source.Width = character_control.character[c].sprite_width - 1;
      r_source.Height = character_control.character[c].sprite_height;

      v_draw.X = character_control.character[c].x + screen.scroll_x;
      v_draw.Y = y_draw_coordinate (character_control.character[c].y, character_control.character[c].z) + character_control.character[c].walk_pixels;
      v_origin.X = character_control.character[c].sprite_width / 2;
      v_origin.Y = character_control.character[c].sprite_height;
      rotation = 0;

      spriteBatch.Draw (character_control.character_sprite[character_control.character[c].sprite, character_control.character[c].skin], v_draw, r_source, Color.White, MathHelper.ToRadians (rotation), v_origin, 1, SpriteEffects.None, 0);

      if (character_control.character[c].blinking == true)
        {
        r_source.X = 1 + (character_control.character[c].sprite_width + 1);
        spriteBatch.Draw (character_control.character_sprite[character_control.character[c].sprite, character_control.character[c].skin], v_draw, r_source, Color.White, MathHelper.ToRadians (rotation), v_origin, 1, SpriteEffects.None, 0);
        }

      if (debug == true && game_state == GAME)
        {
        // draw arrow (remove later)
        v_draw2.X = v_draw.X;
        v_draw2.Y = v_draw.Y - (character_control.character[c].sprite_height / 3);
        v_origin.X = 16;
        v_origin.Y = 16;
        spriteBatch.Draw (arrow_sprite, v_draw2, null, Color.White * .3f, Convert.ToSingle (character_control.character[c].dir) * -1, v_origin, 1, SpriteEffects.None, 0);
        }

      // draw POW
      if (character_control.character[c].pow1.opacity > 0f)
        {
        v_draw2.X = v_draw.X + character_control.character[c].pow1.x;
        v_draw2.Y = v_draw.Y + character_control.character[c].pow1.y;
        r_source.X = 1 + (character_control.character[c].pow1.shape * (Character_Control.pow_sprite_width + 1));
        r_source.Y = 1 + (character_control.character[c].pow1.color * (Character_Control.pow_sprite_height + 1));
        r_source.Width = Character_Control.pow_sprite_width;
        r_source.Height = Character_Control.pow_sprite_height;
        spriteBatch.Draw (pow_sprite, v_draw2, r_source, Color.White * character_control.character[c].pow1.opacity);
        character_control.character[c].pow1.opacity -= 0.03f;
        if (character_control.character[c].pow1.opacity < 0f) character_control.character[c].pow1.opacity = 0f;
        }

      // draw health over head
      if (game_state == GAME && debug == true)
        {
        v_draw2.X = v_draw.X - 15;
        v_draw2.Y = v_draw.Y - character_control.character[c].sprite_height - 20;
        spriteBatch.DrawString (debug_font, Convert.ToString (character_control.character[c].health), v_draw2, Color.White);
        }

      // draw subtarget location for ai navigation (remove later)
      if (debug == true && game_state == GAME && c != PLAYER && character_control.active (character_control.character[c].target))
        {
        //v_subtarget.X = character_control.character[character_control.character[c].target].x + scroll_x;
        v_subtarget.X = character_control.character[c].subtarget_x + screen.scroll_x;
        //v_subtarget.Y = y_draw_coordinate (character_control.character[character_control.character[c].target].y, character_control.character[character_control.character[c].target].z) + character_control.character[character_control.character[c].target].walk_pixels;
        v_subtarget.Y = y_draw_coordinate (character_control.character[c].subtarget_y, character_control.character[c].subtarget_z) + character_control.character[character_control.character[c].target].walk_pixels;
        v_draw2.X = v_subtarget.X - (target_sprite.Width / 2);
        v_draw2.Y = v_subtarget.Y - (target_sprite.Height / 2);

        spriteBatch.Draw (target_sprite, v_draw2, Color.White * 0.5f);
        shape.line (spriteBatch, Convert.ToInt32 (v_draw.X), Convert.ToInt32 (v_draw.Y), Convert.ToInt32 (v_subtarget.X), Convert.ToInt32 (v_subtarget.Y), pixel_yellow, 1f);
        }
      }

    //////////////////////////////////////////////////////////////////////////////////

    }
  }
