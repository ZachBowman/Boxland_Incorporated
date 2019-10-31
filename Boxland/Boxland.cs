// Boxland Incorporated

// 2011-2017
// Nightmare Games

using System;
using System.Collections.Generic;
using drawing = System.Drawing;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;

using Forms = System.Windows.Forms;

namespace Boxland
  {
  public partial class Boxland : Game
    {
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    RenderTarget2D light_buffer, floor_buffer;

    bool redraw_floor_buffer = true;

    public Random rnd = new Random();

    // game settings
    bool debug = false;
    bool randomized_map = false;
    Graphics_Feature draw_background = new Graphics_Feature();
    bool draw_walls = true;
    bool draw_boxes = true;
    Graphics_Feature draw_outlines = new Graphics_Feature();
    bool draw_lighting = true;
    bool use_floor_buffer = false;
    bool observe_creation = false;
    bool toggle_enemies = true;
    //int draw_order = 3;       // 1 = bottom to top first, 2 = back to front first, 3 = bottom to top in half layers
    bool dynamic_shadows = true;
    int framerate_control_counter = 0;

    // shortcuts for loading content
    const string Texture_Path = "images\\textures\\";
    const string character_path = "images\\characters\\";

    // general game variables
    Game_State state_property = Game_State.title;   // game, creation
    Game_State game_state
      {
      get {return state_property;}
      set
        {
        state_property = value;

        text_boxes.Clear ();

        if (value == Game_State.title)
          {
          int x = screen.width / 2 - 100;
          text_boxes.Add (new Text_Box ("New Game", x, 350, 0, 0, 1f, Color.Black, Click.new_game));
          text_boxes.Add (new Text_Box ("Exit", x, 450, 0, 0, 1f, Color.Black, Click.exit));
          }
        }
      }
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
    int player_level = 2;
    int player_last_level = -1;   // map area the player was in just before this (-1 = new game)
    public int total_levels;

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

      //width = 1280,             // pc wide
      //height = 768,

      //width = 1440,             // pc wide high res
      //height = 900,

      width = 1024,             // pc small window
      height = 600,

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
      fullscreen = false
      };

    // map
    Map map = new Map ();
    public const int tilesize = 96;
    public const double parallax = .75;
    //int map_tile_width;                       // total tile size of current map area
    //int map_tile_length;
    //int map_tile_height;
    //int map_char_width;                       // width of map in string characters
    //int map_width = 0;                        // total pixel size of current map area
    //int map_length = 0;
    //int map_height = 0;
    const int box_move = tilesize;
    Rectangle scroll_border;

    const int map_tile_max_width = 55;
    const int map_char_max_width = map_tile_max_width * 3;
    const int map_tile_max_length = 25;
    const int map_tile_max_height = 4;//3;

    const int map_max_width  = map_tile_max_width * tilesize;//2000;//1500;  // maximum allowable pixel size of map
    const int map_max_length = map_tile_max_length * tilesize;//2000;//1500;
    const int map_max_height = map_tile_max_height * tilesize;//400;

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
    Movement player_movement = Movement.keyboard;  // waits for input from keyboard or gamepad on title screen.

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
    //bool key_leftshift    = false;
    bool key_rightshift   = false;
    bool key_leftcontrol  = false;
    bool key_rightcontrol = false;
    bool key_leftalt      = false;
    //bool key_rightalt     = false;
    bool key_numpad0      = false;
    bool key_numpad4      = false;
    bool key_numpad5      = false;
    bool key_numpad6      = false;
    bool key_numpad8      = false;
    bool key_esc          = false;
    bool key_tab          = false;
    bool key_1            = false;
    bool key_2            = false;
    bool key_3            = false;
    bool key_4            = false;
    bool key_5            = false;
    bool key_6            = false;
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
    Texture2D title_logo_test;
    Vector2 menu_exit_v;
    //Text text;
    //Text_Control text_control = new Text_Control (spriteBatch);

    // graphics
    Shapes shape = new Shapes ();
    Texture2D pixel_yellow, pixel_black;

    // particles
    public Particle_Engine particle_engine = new Particle_Engine (parallax);

    const int max_light_sprites = 9;
    Texture2D[] light_sprite = new Texture2D[max_light_sprites];
    Color ambient_light = new Color (96, 96, 96);  // used only for light engine 2
    const int max_lights = 150;
    List<Light> light = new List<Light> ();

    // LIGHT TYPES
    const int SOLID      = 0;
    const int PULSING    = 1;
    const int FLICKERING = 2;

    // effects
    Texture2D solid_black;
    Texture2D wall_shadow_west, wall_shadow_south_west, wall_shadow_south, wall_shadow_south_east, wall_shadow_east;
    //Texture2D shading_wall, shading_door_test_closed, shading_door_test_open;//, shading_exit_test_closed, shading_exit_test_open;
    //Texture2D shading_gateway_test, shading_box_test_ice;
    Texture2D pow_sprite;
    Texture2D test_background1, test_background2, test_background3, test_background4, test_background5;
    Texture2D effect_snowflake, effect_cold_energy, effect_dollars;
    Texture2D effect_flame_white, effect_flame_yellow, effect_flame_orange, effect_flame_red, effect_smoke, effect_sparkle_white;
    Texture2D color_flash_sprite, solid_white, solid_red, effect_pain;
    Texture2D pixel_green;
    Texture2D wires_southeast_powered_test;
    Texture2D shadow_character_generic;
    Texture2D brush_outline_left, brush_outline_right, brush_outline_top, brush_outline_bottom;
    Texture2D brush_outline_top_left, brush_outline_top_right, brush_outline_bottom_left, brush_outline_bottom_right;
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
    //int rock_color = 0;

    // CHARACTERS
    Character_Control character_control = new Character_Control (tilesize);
    Texture2D[] character_shadow = new Texture2D[Character_Control.max_character_list];
    Texture2D arrow_sprite, target_sprite;
    const int max_characters = 30;
    public int PLAYER = -1;             // becomes non-negative after first time player is added to the map (used to retain stats between levels)
    //bool player_created = false;
    bool player_start_located = false;  // becomes true each time player location is added to map (in case of missing player position)
    int punch_rest_delay = 5;           // number of animation frames until character's fist retracts (for all combat moves)
    int random_retard1 = 0;             // number of randomly-placed standard retards to genereate, set on map load
    int random_retard2 = 0;             // number of randomly-placed rock-throwing retards to genereate, set on map load

    // draw list
    // TO DO: MAKE DRAW SLOTS + PARTICLE COUNT DYNAMIC BASED ON FPS
    int max_draw_slots = 700;
    List<Draw_Slot> draw_list = new List<Draw_Slot> ();

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
    //Vector2    debug_pos;
    //bool       test_sprite = false;
    //int        debug_int;
    int grab_x_distance = 0;
    int grab_y_distance = 0;
    //int Q = 0;  // test variable
    Keys last_key_pressed = Keys.Kanji;

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
      //Window.Position = new Point (0, 0);

      light_buffer = new RenderTarget2D (GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

      //player_level = 6;  // starting level, default 0

      load_map ();

      //if (player_added_to_map == false)
      //  {
      //  add_character (Name.RICHARD, 64, 64, 256);
      //  }

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

      //text = new Text (GraphicsDevice);
      //text_control = new Text_Control (spriteBatch);

      game_state = Game_State.title;

      load_bindings ();
      save_bindings ();
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
      // background
      test_background1 = Content.Load<Texture2D> ("images\\textures\\test_background1a");//test_background1a.png
      test_background2 = Content.Load<Texture2D> ("images\\textures\\test_background2a");//test_background2a.png
      test_background3 = Content.Load<Texture2D> ("images\\textures\\test_background3");//test_background3.png
      test_background4 = Content.Load<Texture2D> ("images\\textures\\test_background4a");//test_background4a.png
      test_background5 = Content.Load<Texture2D> ("images\\textures\\test_background5");//test_background5.png
      map.background = test_background4;

      // effects
      brush_outline_left         = Content.Load<Texture2D> ("images\\effects\\brush_outline_left");//effects\\brush_outline_left.png",   FileMode.Open, FileAccess.Read));
      brush_outline_right        = Content.Load<Texture2D> ("images\\effects\\brush_outline_right");//effects\\brush_outline_right.png",  FileMode.Open, FileAccess.Read));
      brush_outline_top          = Content.Load<Texture2D> ("images\\effects\\brush_outline_top");//effects\\brush_outline_top.png",    FileMode.Open, FileAccess.Read));
      brush_outline_bottom       = Content.Load<Texture2D> ("images\\effects\\brush_outline_bottom");//effects\\brush_outline_bottom.png
      brush_outline_top_left     = Content.Load<Texture2D> ("images\\effects\\brush_outline_top_left");//effects\\brush_outline_top_left.png
      brush_outline_top_right    = Content.Load<Texture2D> ("images\\effects\\brush_outline_top_right");//effects\\brush_outline_top_right.png
      brush_outline_bottom_left  = Content.Load<Texture2D> ("images\\effects\\brush_outline_bottom_left");//effects\\brush_outline_bottom_left.png
      brush_outline_bottom_right = Content.Load<Texture2D> ("images\\effects\\brush_outline_bottom_right");//effects\\brush_outline_bottom_right.png

      //wall_shadow_center     = Content.Load<Texture2D> ("images\\effects\\shadow_center");//effects\\shadow_center.png
      wall_shadow_west       = Content.Load<Texture2D> ("images\\effects\\shadow_left3");//effects\\shadow_left3.png
      wall_shadow_south_west = Content.Load<Texture2D> ("images\\effects\\shadow_lower_left3");//effects\\shadow_lower_left3.png
      wall_shadow_south      = Content.Load<Texture2D> ("images\\effects\\shadow_lower3");//effects\\shadow_lower3.png
      wall_shadow_south_east = Content.Load<Texture2D> ("images\\effects\\shadow_lower_right3");//effects\\shadow_lower_right3.png
      wall_shadow_east       = Content.Load<Texture2D> ("images\\effects\\shadow_right3");//effects\\shadow_right3.png

      //shading_wall = Content.Load<Texture2D> ("images\\effects\\shading_wall");//effects\\shading_wall.png
      //ConvertToPremultipliedAlpha (shading_wall, new Color (255, 0, 255, 255));

      //shading_door_test_closed = Content.Load<Texture2D> ("shading_door_test_closed");//effects\\shading_door_test_closed.png
      //ConvertToPremultipliedAlpha (shading_door_test_closed, new Color (255, 0, 255, 255));

      //shading_door_test_open = Content.Load<Texture2D> ("shading_door_test_open");//effects\\shading_door_test_open.png
      //ConvertToPremultipliedAlpha (shading_door_test_open, new Color (255, 0, 255, 255));

      //shading_gateway_test = Content.Load<Texture2D> ("shading_gateway_test");//effects\\shading_gateway_test.png
      //ConvertToPremultipliedAlpha (shading_gateway_test, new Color (255, 0, 255, 255));

      //shading_box_test_ice = Content.Load<Texture2D> ("shading_box_test_ice");//effects\\shading_box_test_ice3.png
      //ConvertToPremultipliedAlpha (shading_box_test_ice, new Color (255, 0, 255, 255));

      light_sprite[(int) Light_Color.blue]        = Content.Load<Texture2D> ("images\\effects\\light_blue");//effects\\light_blue.png
      light_sprite[(int) Light_Color.blue_pale]   = Content.Load<Texture2D> ("images\\effects\\light_blue_pale");//effects\\light_blue_pale.png
      light_sprite[(int) Light_Color.dark]        = Content.Load<Texture2D> ("images\\effects\\light_dark");//effects\\light_dark.png
      light_sprite[(int) Light_Color.fushia]      = Content.Load<Texture2D> ("images\\effects\\light_fushia");//effects\\light_fushia.png
      light_sprite[(int) Light_Color.green]       = Content.Load<Texture2D> ("images\\effects\\light_green");//effects\\light_green.png
      light_sprite[(int) Light_Color.red]         = Content.Load<Texture2D> ("images\\effects\\light_red");//effects\\light_red.png
      light_sprite[(int) Light_Color.white]       = Content.Load<Texture2D> ("images\\effects\\light_white");//effects\\light_white.png
      light_sprite[(int) Light_Color.yellow]      = Content.Load<Texture2D> ("images\\effects\\light_yellow");//effects\\light_yellow.png
      light_sprite[(int) Light_Color.yellow_pale] = Content.Load<Texture2D> ("images\\effects\\light_yellow_pale");//effects\\light_yellow_pale.png

      effect_snowflake     = Content.Load<Texture2D> ("images\\effects\\snowflake");//effects\\snowflake.png
      effect_cold_energy   = Content.Load<Texture2D> ("images\\effects\\cold_energy");//effects\\cold_energy.png
      effect_flame_white   = Content.Load<Texture2D> ("images\\effects\\flame_white");//effects\\flame_white.png
      effect_flame_yellow  = Content.Load<Texture2D> ("images\\effects\\flame_yellow");//effects\\flame_yellow.png
      effect_flame_orange  = Content.Load<Texture2D> ("images\\effects\\flame_orange");//effects\\flame_orange.png
      effect_flame_red     = Content.Load<Texture2D> ("images\\effects\\flame_red");//effects\\flame_red.png
      effect_smoke         = Content.Load<Texture2D> ("images\\effects\\smoke");//effects\\smoke.png
      effect_dollars       = Content.Load<Texture2D> ("images\\effects\\dollars");//effects\\dollars.png
      effect_sparkle_white = Content.Load<Texture2D> ("images\\effects\\sparkle_white");//effects\\sparkle_white.png

      solid_black  = Content.Load<Texture2D> ("images\\effects\\solid_black");//effects\\solid_black.png
      pixel_yellow = Content.Load<Texture2D> ("images\\effects\\pixel_yellow");//effects\\pixel_yellow.png
      pixel_black  = Content.Load<Texture2D> ("images\\effects\\pixel_black");//effects\\pixel_black.png
      solid_white  = Content.Load<Texture2D> ("images\\effects\\solid_white");
      solid_red    = Content.Load<Texture2D> ("images\\effects\\solid_red");
      effect_pain  = Content.Load<Texture2D> ("images\\effects\\pain");
      pixel_green  = Content.Load<Texture2D> ("images\\effects\\pixel_green");

      pow_sprite = Content.Load<Texture2D> ("images\\effects\\POW");
      arrow_sprite = Content.Load<Texture2D> ("images\\effects\\arrow");
      target_sprite = Content.Load<Texture2D> ("images\\effects\\target");

      wires_southeast_powered_test = Content.Load<Texture2D> ("images\\effects\\wires_southeast_powered_test2");//effects\\wires_southeast_powered_test2.png

      shadow_character_generic = Content.Load<Texture2D> ("images\\effects\\shadow_character_generic");//effects\\shadow_character_generic.png

      color_flash_sprite = solid_white;

      load_textures ();
      load_fixture_sprites ();
      load_character_sprites ();
      load_letter_sprites ();

      // stickers
      sticker_office[0] = Content.Load<Texture2D> ("images\\textures\\sticker_office0_test2");//Content.Load<Texture2D> ("_test_test");//sticker_office0_test2.png
      sticker_office[1] = Content.Load<Texture2D> ("images\\textures\\sticker_office1_test");//Content.Load<Texture2D> ("_test");//sticker_office1_test.png
      sticker_office[2] = Content.Load<Texture2D> ("images\\textures\\sticker_office2_test");//Content.Load<Texture2D> ("_test");//sticker_office2_test.png
      sticker_office[3] = Content.Load<Texture2D> ("images\\textures\\sticker_office3_test");//Content.Load<Texture2D> ("_test");//sticker_office3_test.png
      sticker_office[4] = Content.Load<Texture2D> ("images\\textures\\sticker_office4_test");//Content.Load<Texture2D> ("_test");//sticker_office4_test.png
      sticker_office[5] = Content.Load<Texture2D> ("images\\textures\\sticker_office5_test");//Content.Load<Texture2D> ("_test");//sticker_office5_test.png
      sticker_office[6] = Content.Load<Texture2D> ("images\\textures\\sticker_office6_test");//Content.Load<Texture2D> ("_test");//sticker_office6_test.png
      sticker_office[7] = Content.Load<Texture2D> ("images\\textures\\sticker_office7_test");//Content.Load<Texture2D> ("_test");//sticker_office7_test.png
      sticker_office[8] = Content.Load<Texture2D> ("images\\textures\\sticker_office8_test");//Content.Load<Texture2D> ("_test");//sticker_office8_test.png
      sticker_office[9] = Content.Load<Texture2D> ("images\\textures\\sticker_office9_test");//Content.Load<Texture2D> ("_test");//sticker_office9_test.png
      sticker_office[10] = Content.Load<Texture2D> ("images\\textures\\sticker_office10_test");//Content.Load<Texture2D> ("_test");//sticker_office10_test.png
      sticker_office[11] = Content.Load<Texture2D> ("images\\textures\\sticker_office11_test");//Content.Load<Texture2D> ("_test");//sticker_office11_test.png
      sticker_office[12] = Content.Load<Texture2D> ("images\\textures\\sticker_office12_test");//Content.Load<Texture2D> ("_test");//sticker_office12_test.png
      sticker_office[13] = Content.Load<Texture2D> ("images\\textures\\sticker_office13_test");//Content.Load<Texture2D> ("_test");//sticker_office13_test.png
      sticker_office[14] = Content.Load<Texture2D> ("images\\textures\\sticker_office14_test");//Content.Load<Texture2D> ("_test");//sticker_office14_test.png
      sticker_office[15] = Content.Load<Texture2D> ("images\\textures\\sticker_office15_test");//Content.Load<Texture2D> ("_test");//sticker_office15_test.png
      sticker_office[16] = Content.Load<Texture2D> ("images\\textures\\sticker_office16_test");//Content.Load<Texture2D> ("_test");//sticker_office16_test.png
      sticker_office[17] = Content.Load<Texture2D> ("images\\textures\\sticker_office17_test");//Content.Load<Texture2D> ("_test");//sticker_office17_test.png
      sticker_office[18] = Content.Load<Texture2D> ("images\\textures\\sticker_office18_test");//Content.Load<Texture2D> ("_test");//sticker_office18_test.png
      sticker_office[19] = Content.Load<Texture2D> ("images\\textures\\sticker_office19_test");//Content.Load<Texture2D> ("_test");//sticker_office19_test.png
      sticker_office[20] = Content.Load<Texture2D> ("images\\textures\\sticker_office20_test");//Content.Load<Texture2D> ("_test");//sticker_office20_2_test.png
      sticker_office[21] = Content.Load<Texture2D> ("images\\textures\\sticker_office21_test");//Content.Load<Texture2D> ("_test");//sticker_office21_test.png
      sticker_office[22] = Content.Load<Texture2D> ("images\\textures\\sticker_office22_test");//Content.Load<Texture2D> ("_test");//sticker_office22_double_test.png
      sticker_office[23] = Content.Load<Texture2D> ("images\\textures\\sticker_office23_test");//Content.Load<Texture2D> ("_test");//sticker_office23_test.png
      sticker_office[24] = Content.Load<Texture2D> ("images\\textures\\sticker_office24_test");//Content.Load<Texture2D> ("_test");//sticker_office24_test.png
      sticker_office[25] = Content.Load<Texture2D> ("images\\textures\\sticker_office25_test");//Content.Load<Texture2D> ("_test");//sticker_office25_double_test.png
      sticker_office[26] = Content.Load<Texture2D> ("images\\textures\\sticker_office26_test");//Content.Load<Texture2D> ("_test");//sticker_office26_test.png
      sticker_office[27] = Content.Load<Texture2D> ("images\\textures\\sticker_office27_test");//Content.Load<Texture2D> ("_test");//sticker_office27_test.png
      sticker_office[28] = Content.Load<Texture2D> ("images\\textures\\sticker_office28_test");//Content.Load<Texture2D> ("_test");//sticker_office28_test.png
      sticker_office[29] = Content.Load<Texture2D> ("images\\textures\\sticker_office29_test");//Content.Load<Texture2D> ("_test");//sticker_office29_test.png
      sticker_office[30] = Content.Load<Texture2D> ("images\\textures\\sticker_office30_test");//Content.Load<Texture2D> ("_test");//sticker_office30_test.png
      sticker_office[31] = Content.Load<Texture2D> ("images\\textures\\sticker_office31_test");//Content.Load<Texture2D> ("_test");//sticker_office31_test.png
      sticker_office[32] = Content.Load<Texture2D> ("images\\textures\\sticker_office32_test");//Content.Load<Texture2D> ("_test");//sticker_office32_test.png

      sticker_office_floor[0] = Content.Load<Texture2D> ("images\\textures\\sticker_office_floor0_test");//Content.Load<Texture2D> ("_test");//sticker_office_floor0_test.png

      sticker_factory[0]  = Content.Load<Texture2D> ("images\\textures\\sticker_factory0_test");//sticker_factory0_test.png
      sticker_factory[1]  = Content.Load<Texture2D> ("images\\textures\\sticker_factory1_test");//sticker_factory1_test.png
      sticker_factory[2]  = Content.Load<Texture2D> ("images\\textures\\sticker_factory2_test");//sticker_factory2_test.png
      sticker_factory[3]  = Content.Load<Texture2D> ("images\\textures\\sticker_factory3_test");//sticker_factory3_test.png
      sticker_factory[4]  = Content.Load<Texture2D> ("images\\textures\\sticker_factory4_test");//sticker_factory4_test.png
      sticker_factory[5]  = Content.Load<Texture2D> ("images\\textures\\sticker_factory5_test");//sticker_factory5_test.png
      sticker_factory[6]  = Content.Load<Texture2D> ("images\\textures\\sticker_factory6_test");//sticker_factory6_test.png
      sticker_factory[7]  = Content.Load<Texture2D> ("images\\textures\\sticker_factory7_test");//sticker_factory7_test.png
      sticker_factory[8]  = Content.Load<Texture2D> ("images\\textures\\sticker_factory8_test");//sticker_factory8_test.png
      sticker_factory[9]  = Content.Load<Texture2D> ("images\\textures\\sticker_factory9_test");//sticker_factory9_test.png
      sticker_factory[10] = Content.Load<Texture2D> ("images\\textures\\sticker_factory10_test");//sticker_factory10_test.png

      sticker_factory_floor[0] = Content.Load<Texture2D> ("images\\textures\\sticker_factory_floor0a_test");//sticker_factory_floor0a_test.png
      sticker_factory_floor[1] = Content.Load<Texture2D> ("images\\textures\\sticker_factory_floor1_test");//sticker_factory_floor1_test.png

      // objects
      object_control.object_sprite[(int) Object_Type.shirt_yellow, 0] = Content.Load<Texture2D> ("images\\objects\\shirt_power0_ink");//objects\\shirt_power0_ink.png
      object_control.object_sprite[(int) Object_Type.shirt_yellow, 1] = Content.Load<Texture2D> ("images\\objects\\shirt_power1_ink");//objects\\shirt_power1_ink.png
      object_control.object_sprite[(int) Object_Type.shirt_white, 0]  = Content.Load<Texture2D> ("images\\objects\\shirt_ice0_ink");//objects\\shirt_ice0_ink.png
      object_control.object_sprite[(int) Object_Type.shirt_white, 1]  = Content.Load<Texture2D> ("images\\objects\\shirt_ice1_ink");//objects\\shirt_ice1_ink.png
      object_control.object_sprite[(int) Object_Type.shirt_red, 0]    = Content.Load<Texture2D> ("images\\objects\\shirt_fire0_ink");//objects\\shirt_fire0_ink.png
      object_control.object_sprite[(int) Object_Type.shirt_teal, 0]   = Content.Load<Texture2D> ("images\\objects\\shirt_electric");//objects\\shirt_electric.png
      object_control.object_sprite[(int) Object_Type.shirt_fushia, 0] = Content.Load<Texture2D> ("images\\objects\\shirt_magnet0_ink");//objects\\shirt_magnet0_ink.png
      object_control.object_sprite[(int) Object_Type.shirt_fushia, 1] = Content.Load<Texture2D> ("images\\objects\\shirt_magnet1_ink");//objects\\shirt_magnet1_ink.png
      //object_control.object_sprite[(int) Object_Type.ROCK, 0]         = Content.Load<Texture2D> ("images\\objects\\rock_grey_test");//objects\\food_hotdog_ink.png
      //object_control.object_sprite[(int) Object_Type.ROCK_brown, 0]   = Content.Load<Texture2D> ("images\\objects\\rock_brown_test");//objects\\food_hamburger_ink.png
      //object_control.object_sprite[(int) Object_Type.ROCK_red, 0]     = Content.Load<Texture2D> ("images\\objects\\rock_red_test");//objects\\food_pizza_ink.png
      //object_control.object_sprite[(int) Object_Type.ROCK_white, 0]   = Content.Load<Texture2D> ("images\\objects\\rock_white_test");//objects\\rock_white_test.png
      object_control.object_sprite[(int) Object_Type.key, 0]          = Content.Load<Texture2D> ("images\\objects\\key0_test");//objects\\key1_ink.png
      object_control.object_sprite[(int) Object_Type.health, 0]       = Content.Load<Texture2D> ("images\\objects\\health0_ink");//objects\\health0_ink.png
      object_control.object_sprite[(int) Object_Type.health, 1]       = Content.Load<Texture2D> ("images\\objects\\health1_ink");//objects\\health1_ink.png
      object_control.object_sprite[(int) Object_Type.health, 2]       = Content.Load<Texture2D> ("images\\objects\\health2_ink");//objects\\health2_ink.png
      object_control.object_sprite[(int) Object_Type.health, 3]       = Content.Load<Texture2D> ("images\\objects\\health3_ink");//objects\\health3_ink.png
      object_control.object_sprite[(int) Object_Type.coin, 0]         = Content.Load<Texture2D> ("images\\objects\\coin0_ink");//objects\\coin0_ink.png
      object_control.object_sprite[(int) Object_Type.scrap_metal, 0]  = Content.Load<Texture2D> ("images\\objects\\scrap_metal_ink");//objects\\scrap_metal_ink.png
      object_control.object_sprite[(int) Object_Type.energy, 0]       = Content.Load<Texture2D> ("images\\objects\\energy0_ink");//objects\\energy0_ink.png

      // menu
      title_logo_test = Content.Load<Texture2D> ("images\\menu\\boxland_logo5a");//menu\\new_logo10.png
      //title_screen_test = menu\\title_screen_test2.png
      menu_exit_test = Content.Load<Texture2D> ("images\\menu\\menu_exit");//menu\\menu_exit.png
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

      //if (player_control == "none") Get_Input ();
      Keyboard_Input ();
      //if (player_control == "controller") Controller_Input ();

      //mouse_last = mouse_current;
      //mouse_current = Mouse.GetState();
      //if (mouse_current.LeftButton == ButtonState.Pressed)
      //  {
      //  int a = 0;
      //  a += 1;
      //  }
      Mouse_Input (game_time);  // used only for menus, etc

      Update_Time (game_time);
      if (game_state == Game_State.game)
        {
        if (!game_menu) Update_Brushes ();
        if (!game_menu) Update_Fixtures ();
        if (!game_menu) Update_Objects ();
        if (!game_menu) Update_Characters ();
        if (!game_menu) Update_Lights ();
        Update_Scrolling ();
        Framerate_Controller ();
        Graphics_Transition_Updates ();
        if (!game_menu) particle_engine.update ();
        Check_Doors ();
        }
      if (game_state == Game_State.creation)
        {
        Update_Scrolling ();
        if (!observe_creation == false) load_map ();
        }
//      Sound_Manager ();

      base.Update (game_time);
      }

    //////////////////////////////////////////////////////////////////////////////////

    void Framerate_Controller ()
      {
      // ADD COUNTER TO FIRE ONLY ONCE EVERY 5 SECONDS

      if (!debug)
        {
        framerate_control_counter += 1;
        if (framerate_control_counter >= 150)
          {
          framerate_control_counter = 0;

          if (fps < 30)
            {
            if (draw_outlines.on) draw_outlines.turn_off ();
            if (draw_background.on) draw_background.turn_off ();
            }
          else if (fps > 40)
            {
            if (!draw_background.on) draw_background.turn_on ();
            if (!draw_outlines.on) draw_outlines.turn_on ();
            }
          }
        }
      }

    //////////////////////////////////////////////////////////////////////////////////

    void Graphics_Transition_Updates ()
      {
      draw_outlines.update ();
      draw_background.update ();
      }

    //////////////////////////////////////////////////////////////////////////////////

    int add_object (int object_type, int x, int y, int z)
      {
      if (object_control.obj.Count < Object_Control.max_objects)
        {
        object_control.add (object_type, x, y, z);
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
          if (object_control.obj[o].type == (int) Object_Type.coin)
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
          if (brush_control.brush[b].top_texture_number == (int) Texture_Type.door_red_v_top_closed_test && red_doors_open == true) clip = -1;
          else if (brush_control.brush[b].top_texture_number == (int) Texture_Type.door_red_h_top_closed_test && red_doors_open == true) clip = -1;
          else if (brush_control.brush[b].top_texture_number == (int) Texture_Type.door_yellow_v_top_closed_test && yellow_doors_open == true) clip = -1;
          else if (brush_control.brush[b].top_texture_number == (int) Texture_Type.door_yellow_h_top_closed_test && yellow_doors_open == true) clip = -1;
          else if (brush_control.brush[b].top_texture_number == (int) Texture_Type.door_green_v_top_closed_test && green_doors_open == true) clip = -1;
          else if (brush_control.brush[b].top_texture_number == (int) Texture_Type.door_green_h_top_closed_test && green_doors_open == true) clip = -1;
          else if (brush_control.brush[b].top_texture_number == (int) Texture_Type.door_blue_v_top_closed_test && blue_doors_open == true) clip = -1;
          else if (brush_control.brush[b].top_texture_number == (int) Texture_Type.door_blue_h_top_closed_test && blue_doors_open == true) clip = -1;
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
        
        else if (brush_control.brush[b].moving)
          {
          brush_control.brush[b].moving = false;

          // SHOULDN'T THIS HAPPEN ONLY ONCE EACH FRAME?
          foreach (Brush brush in brush_control.brush)
            {
            brush_control.calculate_top_shadows (brush);
            }
          }

        // move north
        if (brush_control.brush[b].moving_north == true)
          {
          brush_control.brush[b].dy += brush_control.brush[b].ext_y_velocity;
          brush_control.brush[b].y = Convert.ToInt32 (brush_control.brush[b].dy);
          b_clip = brush_control.brush_in_brush (b);
          f_clip = brush_control.brush_in_fixture (brush_control.brush[b], fixture_control.fixture, true);

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

          // hit or went past destination
          if (brush_control.brush[b].dy >= brush_control.brush[b].destination_y)
            {
            if (character_control.character[PLAYER].brush_grab == b && character_control.character[PLAYER].action == Action.push)  // hit or went past destination
              {
              // set new destination if player is stil pushing
              brush_control.brush[b].destination_y += tilesize;
              }
            else
              {
              brush_control.brush[b].moving_north = false;
              brush_control.brush[b].ext_y_velocity = 0;
              brush_control.brush[b].y = brush_control.brush[b].destination_y;
              brush_control.brush[b].dy = brush_control.brush[b].destination_y;
              }
            }
          }

        // move south
        else if (brush_control.brush[b].moving_south == true)
          {
          brush_control.brush[b].dy += brush_control.brush[b].ext_y_velocity;
          brush_control.brush[b].y = Convert.ToInt32 (brush_control.brush[b].dy);
          b_clip = brush_control.brush_in_brush (b);
          f_clip = brush_control.brush_in_fixture (brush_control.brush[b], fixture_control.fixture, true);

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

          // hit or went past destination
          if (brush_control.brush[b].dy <= brush_control.brush[b].destination_y)
            {
            if (character_control.character[PLAYER].brush_grab == b && character_control.character[PLAYER].action == Action.push)
              {
              // set new destination if player is stil pushing
              brush_control.brush[b].destination_y -= tilesize;
              }
            else
              {
              brush_control.brush[b].moving_south = false;
              brush_control.brush[b].ext_y_velocity = 0;
              brush_control.brush[b].y = brush_control.brush[b].destination_y;
              brush_control.brush[b].dy = brush_control.brush[b].destination_y;
              }
            }
          }

        // move east
        if (brush_control.brush[b].moving_east == true)
          {
          brush_control.brush[b].dx += brush_control.brush[b].ext_x_velocity;
          brush_control.brush[b].x = Convert.ToInt32 (brush_control.brush[b].dx);
          b_clip = brush_control.brush_in_brush (b);
          f_clip = brush_control.brush_in_fixture (brush_control.brush[b], fixture_control.fixture, true);

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

          // hit or went past destination
          if (brush_control.brush[b].dx >= brush_control.brush[b].destination_x)
            {
            if (character_control.character[PLAYER].brush_grab == b && character_control.character[PLAYER].action == Action.push)
              {
              // set new destination if player is stil pushing
              brush_control.brush[b].destination_x += tilesize;
              }
            else
              {
              brush_control.brush[b].moving_east = false;
              brush_control.brush[b].ext_x_velocity = 0;
              brush_control.brush[b].x = brush_control.brush[b].destination_x;
              brush_control.brush[b].dx = brush_control.brush[b].destination_x;
              }
            }
          }

        // move west
        else if (brush_control.brush[b].moving_west == true)
          {
          brush_control.brush[b].dx += brush_control.brush[b].ext_x_velocity;
          brush_control.brush[b].x = Convert.ToInt32 (brush_control.brush[b].dx);
          b_clip = brush_control.brush_in_brush (b);
          f_clip = brush_control.brush_in_fixture (brush_control.brush[b], fixture_control.fixture, true);

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

          // hit or went past destination
          if (brush_control.brush[b].dx <= brush_control.brush[b].destination_x)
            {
            if (character_control.character[PLAYER].brush_grab == b && character_control.character[PLAYER].action == Action.push)
              {
              // set new destination if player is stil pushing
              brush_control.brush[b].destination_x -= tilesize;
              }
            else
              {
              brush_control.brush[b].moving_west = false;
              brush_control.brush[b].ext_x_velocity = 0;
              brush_control.brush[b].x = brush_control.brush[b].destination_x;
              brush_control.brush[b].dx = brush_control.brush[b].destination_x;
              }
            }
          }

        // snap brush_control.brush x and y to grid if not being moved
        // NEEDS TO BE ENHANCED WITH SMOOTH SNAPPING
        if (character_control.character[PLAYER].action != Action.grab && character_control.character[PLAYER].action != Action.push && brush_control.brush[b].moving == false)
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
            if (fixture_control.fixture[f].type == Fixture_Type.conveyor_north_test && fixture_control.fixture[f].on == true)
              {
              brush_control.brush[b].destination_y = brush_control.brush[b].y + tilesize;
              brush_control.brush[b].moving = true;
              brush_control.brush[b].moving_north = true;
              brush_control.brush[b].ext_y_velocity = conveyor_speed;
              }
            else if (fixture_control.fixture[f].type == Fixture_Type.conveyor_south_test && fixture_control.fixture[f].on == true)
              {
              brush_control.brush[b].destination_y = brush_control.brush[b].y - tilesize;
              brush_control.brush[b].moving = true;
              brush_control.brush[b].moving_south = true;
              brush_control.brush[b].ext_y_velocity = -1 * conveyor_speed;
              }
            else if (fixture_control.fixture[f].type == Fixture_Type.conveyor_west_test && fixture_control.fixture[f].on == true)
              {
              brush_control.brush[b].destination_x = brush_control.brush[b].x - tilesize;
              brush_control.brush[b].moving = true;
              brush_control.brush[b].moving_west = true;
              brush_control.brush[b].ext_x_velocity = -1 * conveyor_speed;
              }
            else if (fixture_control.fixture[f].type == Fixture_Type.conveyor_east_test && fixture_control.fixture[f].on == true)
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
        if (brush_control.brush[b].top_texture_number == (int) Texture_Type.box_ice_test)  // cold steam
          {
          if (rnd.Next (0, 100) == 0) particle_steam (brush_control.brush[b].x + rnd.Next (0, tilesize), brush_control.brush[b].y + rnd.Next (0, tilesize), brush_control.brush[b].z + brush_control.brush[b].height + 1, 270);
          }
        if (brush_control.brush[b].top_texture_number == (int) Texture_Type.incinerator_test_down)  // incinerator flames
          {
          particle_incinerator (brush_control.brush[b].x + (brush_control.brush[b].width / 2), brush_control.brush[b].y - 1, brush_control.brush[b].z + brush_control.brush[b].height - (tilesize / 4), 270, "brush_control.brush", b);
          }

        // door collisions
        //if (brush_control.brush[b].top_texture_number == Texture_Type.door_
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
        game_state = Game_State.creation;
        creation_mode = 1;

        map.tile_width = rnd.Next (8, 12);
        map.tile_length = rnd.Next (8, 12);
        map.tile_height = 3;
        //ambient_dark = 0;

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
        for (mz = 0; mz < map.tile_height; mz += 1)
          for (my = 0; my < map.tile_length; my += 1)
            for (mx = 0; mx < map.tile_width; mx += 1)
              matrixmap[mx, my, mz] = "..";

        // floor & walls
        for (my = 0; my < map.tile_length; my += 1)
          {
          for (mx = 0; mx < map.tile_width; mx += 1)
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
          mx = rnd.Next (1, map.tile_width - 2);
          creation_px = mx;
          creation_py = my + 1;
          creation_pz = 1;
          matrixmap[mx, my, 1] = "XR";
          }
        else if (random == 1)  // south
          {
          my = map.tile_length - 1;
          mx = rnd.Next (1, map.tile_width - 2);
          creation_px = mx;
          creation_py = my - 1;
          creation_pz = 1;
          matrixmap[mx, my, 1] = "XR";
          }
        else if (random == 2)  // west
          {
          mx = 0;
          my = rnd.Next (1, map.tile_length - 2);
          creation_px = mx + 1;
          creation_py = my;
          creation_pz = 1;
          matrixmap[mx, my, 1] = "Xr";
          }
        else  // east
          {
          mx = map.tile_width - 1;
          my = rnd.Next (1, map.tile_length - 2);
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
          if (matrixmap[creation_px, creation_py - 1, creation_pz] == "b " && creation_py < map.tile_length - 2)  // above
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
          if (matrixmap[creation_px - 1, creation_py, creation_pz] == "b " && creation_px < map.tile_width - 2)  // to the left
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
            else if (creation_direction == 1 && creation_py < map.tile_length - 2) endloop = true;
            else if (creation_direction == 2 && creation_px > 1) endloop = true;
            else if (creation_direction == 3 && creation_px < map.tile_width - 2) endloop = true;

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
              else if (creation_py < map.tile_length - 2 && matrixmap[creation_px, creation_py + 1, creation_pz] != "b ") creation_direction = 1;
              else if (creation_px > 1 && matrixmap[creation_px, creation_py + 1, creation_pz] != "b ") creation_direction = 2;
              else if (creation_px < map.tile_width - 2 && matrixmap[creation_px, creation_py + 1, creation_pz] != "b ") creation_direction = 3;
              }
            else  // box shuffle mode
              {
              // any direction (staying inside the room, not clipping a box or wall)
              if (creation_py > 1 && matrixmap[creation_px, creation_py - 1, creation_pz] != "b " && matrixmap[creation_px, creation_py - 1, creation_pz] != creation_wall) creation_direction = 0;
              else if (creation_py < map.tile_length - 2 && matrixmap[creation_px, creation_py + 1, creation_pz] != "b " && matrixmap[creation_px, creation_py + 1, creation_pz] != creation_wall) creation_direction = 1;
              else if (creation_px > 1 && matrixmap[creation_px, creation_py + 1, creation_pz] != "b " && matrixmap[creation_px, creation_py + 1, creation_pz] != creation_wall) creation_direction = 2;
              else if (creation_px < map.tile_width - 2 && matrixmap[creation_px, creation_py + 1, creation_pz] != "b " && matrixmap[creation_px, creation_py + 1, creation_pz] != creation_wall) creation_direction = 3;

              // still stuck, tunnelling allowed
              else if (creation_py > 1 && matrixmap[creation_px, creation_py - 1, creation_pz] != "b ") { creation_direction = 0; tunnel_exception = true; }
              else if (creation_py < map.tile_length - 2 && matrixmap[creation_px, creation_py + 1, creation_pz] != "b ") { creation_direction = 1; tunnel_exception = true; }
              else if (creation_px > 1 && matrixmap[creation_px, creation_py + 1, creation_pz] != "b ") { creation_direction = 2; tunnel_exception = true; }
              else if (creation_px < map.tile_width - 2 && matrixmap[creation_px, creation_py + 1, creation_pz] != "b ") { creation_direction = 3; tunnel_exception = true; }
              }
            }

          // creating a box (at random)
          // make sure there's space for a box opposite the direction player is moving
          // make sure new boxes are only created in walls to prevent solution errors from backtracking
          random = 1;
          if (creation_direction == 0 && matrixmap[creation_px, creation_py + 1, creation_pz] == creation_wall && creation_py < map.tile_length - 2) random = rnd.Next (0, 1);
          if (creation_direction == 1 && matrixmap[creation_px, creation_py - 1, creation_pz] == creation_wall && creation_py > 1) random = rnd.Next (0, 1);
          if (creation_direction == 2 && matrixmap[creation_px + 1, creation_py, creation_pz] == creation_wall && creation_px < map.tile_width - 2) random = rnd.Next (0, 1);
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
        else if (creation_direction == 1) creation_distance = rnd.Next (1, map.tile_length - 2 - creation_py);
        else if (creation_direction == 2) creation_distance = rnd.Next (1, creation_px - 1);
        else if (creation_direction == 3) creation_distance = rnd.Next (1, map.tile_width - 2 - creation_px);

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
        for (mz = 1; mz < map.tile_height; mz += 1)
          for (my = 0; my < map.tile_length; my += 1)
            for (mx = 0; mx < map.tile_width; mx += 1)
              if (matrixmap[mx, my, mz] == "b " && matrixmap[mx, my, mz - 1] == "zr") matrixmap[mx, my, mz - 1] = creation_floor;

        // add enemies
        if (toggle_enemies == true)
          {
          for (my = 0; my < map.tile_length; my += 1)
            {
            for (mx = 0; mx < map.tile_width; mx += 1)
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
        //if (lighting_engine == 1)
        //  {
        //  // ambient light
        //  random = rnd.Next (0, 4);
        //  if (random == 0) ambient_dark = .1f;
        //  else if (random == 1) ambient_dark = .2f;
        //  else if (random == 2) ambient_dark = .3f;
        //  else ambient_dark = .4f;

        //  for (my = 0; my < map_tile_length - 1; my += 1)
        //    {
        //    for (mx = 0; mx < map.tile_width - 1; mx += 1)
        //      {
        //      if (matrixmap[mx, my, 2] == ".."
        //          && matrixmap[mx - 1, my, 2] == ".." && matrixmap[mx + 1, my, 2] == ".."
        //          && matrixmap[mx, my - 1, 2] == ".." && matrixmap[mx, my + 1, 2] == "..")
        //        {
        //        random = rnd.Next (0, 5);
        //        if (random == 0)
        //          {
        //          random = rnd.Next (0, 7);
        //          if (random == 0) matrixmap[mx, my, 2] = "ly";
        //          if (random == 1) matrixmap[mx, my, 2] = "lb";
        //          if (random == 2) matrixmap[mx, my, 2] = "lY";
        //          if (random == 3) matrixmap[mx, my, 2] = "lB";
        //          if (random == 4) matrixmap[mx, my, 2] = "lR";
        //          else matrixmap[mx, my, 2] = "lw";
        //          }
        //        }
        //      }
        //    }
        //  }

        creation_moves = 0;
        game_state = Game_State.game;
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void add_brush (int top_texture_number, int front_texture_number, int x, int y, int z)
      {
      if (brush_control.brush.Count < Brush_Control.max_brushes)
        {
        brush_control.add (top_texture_number, front_texture_number, x, y, z, tilesize, tilesize, tilesize);
        //total_draw_slots += 1;
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

    // lighting engine 2
    void add_light (Color color, int x, int y, int z, float scale, float alpha, int type)
      {
      int radius;
      bool light_in_space = false;
      int r;
      //float ambient_brightness;

      //if (total_lights < max_lights)
      if (light.Count < max_lights)
        {
        radius = tilesize;
        //radius = Convert.ToInt32 (light_sprite[(int) Light_Color.white].Width * .2 * scale);
        //for (int l = 0; l < total_lights; l += 1)
        for (int l = 0; l < light.Count; l += 1)
          {
          if (x >= light[l].x - radius && x <= light[l].x + radius
              && y >= light[l].y - radius && y <= light[l].y + radius)
            light_in_space = true;
          }

        if (light_in_space == false)
          {
          r = rnd.Next (0, 100);
          if (r < 65) type = SOLID;
          else if (r < 95) type = PULSING;
          else type = FLICKERING;

          Light new_light = new Light ();
          
          new_light.x = x;
          new_light.y = y;
          new_light.z = z;
          new_light.scale = scale;

          //if (color == Color.White)
          //{
          // average rbg values of ambient light to get overall brightness value (0-255)
          //ambient_brightness = Convert.ToSingle ((ambient_light.R + ambient_light.G + ambient_light.B) / 3);
          //ambient_brightness = ambient_brightness / 255;  // convert to a (0.0-1.0) scale
          //alpha = 1f - ambient_brightness;
          //}
          new_light.alpha = alpha;
          new_light.type = type;
          new_light.on = true;
          new_light.waxing = false;
          new_light.dimness = 0f;
          new_light.c = color;

          if (type == PULSING)
            {
            r = rnd.Next (10, 30);
            new_light.pulse_speed = Convert.ToSingle (r) / 10000f;
            new_light.waxing = true;
            }
          else new_light.pulse_speed = 0f;

          light.Add (new_light);
          //total_lights += 1;
          }
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void remove_light (int l1)
      {
      if (l1 < light.Count) light.RemoveAt (l1);
      }

    ////////////////////////////////////////////////////////////////////////////////

    void destroy_object (int o)
      {
      // update_objects calls collision detection multiple times for each object every cycle.  therefor, a collision with a character
      // is detected multiple times.  object_control.obj[o].destroyed prevents multiple objects from being removed from the list for a single event.

      object_control.obj[o].destroyed = true;
      }

    ////////////////////////////////////////////////////////////////////////////////

    void remove_one_destroyed_object ()
      {
      int o;//, q;

      o = 0;
      while (o < object_control.obj.Count && object_control.obj[o].destroyed == false)
        {
        if (object_control.obj[o].destroyed) object_control.obj.RemoveAt (o);
        o += 1;
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void add_character (Name type, int x, int y, int z)
      {
      if (character_control.character.Count < max_characters)
        {
        character_control.add (type, x, y, z);
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void add_player ()
      {
      character_control.character.Add (new Character ());
      PLAYER = character_control.character.Count - 1;

      //player_created = true;
      character_control.character[PLAYER].type = Name.RICHARD;
      character_control.character[PLAYER].sprite = (int) Name.RICHARD;
      character_control.character[PLAYER].defaults ();
      }

    ////////////////////////////////////////////////////////////////////////////////

    void player_start_location (int x, int y, int z)
      {
      player_start_located = true;

      if (PLAYER == -1) add_player ();

      character_control.character[PLAYER].x = x + (tilesize / 2);
      character_control.character[PLAYER].y = y + (tilesize / 2);
      character_control.character[PLAYER].z = z;
      character_control.character[PLAYER].dx = character_control.character[PLAYER].x;
      character_control.character[PLAYER].dy = character_control.character[PLAYER].y;
      character_control.character[PLAYER].dz = character_control.character[PLAYER].z;
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
      if (player_level > total_levels) player_level = 0;

      enter_level ();
      }

    ////////////////////////////////////////////////////////////////////////////////

    void skip_back_area ()
      {
      player_last_level = -1;// player_level;
      player_level -= 1;
      if (player_level < 0) player_level = total_levels;

      enter_level ();
      }

    ////////////////////////////////////////////////////////////////////////////////

    void enter_level ()
      {
      if (PLAYER > -1 && player_level > -1)
        {
        game_state = Game_State.game;
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
      for (int l = 0; l < light.Count; l += 1)
        {
        if (light[l].type == FLICKERING)
          {
          if (light[l].on == true && rnd.Next (0, 80) == 0) light[l].on = false;
          else if (light[l].on == false && rnd.Next (0, 3) == 0) light[l].on = true;
          }
        else if (light[l].type == PULSING)
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
      yellow_switch_down = false;
      green_switch_down = false;
      blue_switch_down = false;

      bool red_doors_open_before    = red_doors_open;
      bool yellow_doors_open_before = yellow_doors_open;
      bool green_doors_open_before  = green_doors_open;
      bool blue_doors_open_before   = blue_doors_open;

      red_doors_open = false;
      yellow_doors_open = false;
      green_doors_open = false;
      blue_doors_open = false;

      for (b = 0; b < brush_control.brush.Count; b += 1)
        {
        if (brush_control.brush[b].top_texture_number == (int) Texture_Type.floor_zone_green_test)
          {
          green_zone_found = true;
          floor = brush_control.point_in_brush (brush_control.brush[b].x + (brush_control.brush[b].width / 2), brush_control.brush[b].y + (brush_control.brush[b].length / 2), brush_control.brush[b].z + brush_control.brush[b].height + (tilesize / 4), false, false);
          if (floor == -1 || (floor > -1 && brush_control.brush[floor].top_texture_number != (int) Texture_Type.box_wood && brush_control.brush[floor].top_texture_number != (int) Texture_Type.box_metal_test
              && brush_control.brush[floor].top_texture_number != (int) Texture_Type.box_ice_test)) green_zones_full = false;
          }
        if (brush_control.brush[b].top_texture_number == (int) Texture_Type.floor_zone_red_test)
          {
          red_zone_found = true;
          floor = brush_control.point_in_brush (brush_control.brush[b].x + (brush_control.brush[b].width / 2), brush_control.brush[b].y + (brush_control.brush[b].length / 2), brush_control.brush[b].z + brush_control.brush[b].height + (tilesize / 4), false, false);
          if (floor == -1 || (floor > -1 && brush_control.brush[floor].top_texture_number != (int) Texture_Type.box_wood && brush_control.brush[floor].top_texture_number != (int) Texture_Type.box_metal_test
              && brush_control.brush[floor].top_texture_number != (int) Texture_Type.box_ice_test)) red_zones_full = false;
          }
        if (brush_control.brush[b].top_texture_number == (int) Texture_Type.floor_zone_yellow_test)
          {
          yellow_zone_found = true;
          floor = brush_control.point_in_brush (brush_control.brush[b].x + (brush_control.brush[b].width / 2), brush_control.brush[b].y + (brush_control.brush[b].length / 2), brush_control.brush[b].z + brush_control.brush[b].height + (tilesize / 4), false, false);
          if (floor == -1 || (floor > -1 && brush_control.brush[floor].top_texture_number != (int) Texture_Type.box_wood && brush_control.brush[floor].top_texture_number != (int) Texture_Type.box_metal_test
              && brush_control.brush[floor].top_texture_number != (int) Texture_Type.box_ice_test)) yellow_zones_full = false;
          }
        }

      // if no goal zones are found in level, doors stay locked
      if (green_zone_found == false) green_zones_full = false;
      if (red_zone_found == false) red_zones_full = false;
      if (yellow_zone_found == false) yellow_zones_full = false;

      // check above switches for boxes and walls
      for (b = 0; b < brush_control.brush.Count; b += 1)
        {
        if (brush_control.brush[b].top_texture_number == (int) Texture_Type.switch_red_test
          && brush_control.point_in_brush (brush_control.brush[b].x + (brush_control.brush[b].width / 2), brush_control.brush[b].y + (brush_control.brush[b].length / 2), brush_control.brush[b].z + Convert.ToInt32 (tilesize * 1.5), false, false) > -1)
          {
          red_switch_down = true;
          }
        if (brush_control.brush[b].top_texture_number == (int) Texture_Type.switch_yellow_test
          && brush_control.point_in_brush (brush_control.brush[b].x + (brush_control.brush[b].width / 2), brush_control.brush[b].y + (brush_control.brush[b].length / 2), brush_control.brush[b].z + Convert.ToInt32 (tilesize * 1.5), false, false) > -1)
          {
          yellow_switch_down = true;
          }
        if (brush_control.brush[b].top_texture_number == (int) Texture_Type.switch_green_test
          && brush_control.point_in_brush (brush_control.brush[b].x + (brush_control.brush[b].width / 2), brush_control.brush[b].y + (brush_control.brush[b].length / 2), brush_control.brush[b].z + Convert.ToInt32 (tilesize * 1.5), false, false) > -1)
          {
          green_switch_down = true;
          }
        if (brush_control.brush[b].top_texture_number == (int) Texture_Type.switch_blue_test
          && brush_control.point_in_brush (brush_control.brush[b].x + (brush_control.brush[b].width / 2), brush_control.brush[b].y + (brush_control.brush[b].length / 2), brush_control.brush[b].z + Convert.ToInt32 (tilesize * 1.5), false, false) > -1)
          {
          blue_switch_down = true;
          }
        }

      // check for characters on switches
      for (c = 0; c < character_control.character.Count; c += 1)
        {
        if (character_control.active (c) && character_on_ground (c))
          {
          floor = brush_control.point_in_brush (character_control.character[c].x, character_control.character[c].y, character_control.character[c].z - (tilesize / 4), false, false);
          if (floor > -1 && brush_control.brush[floor].top_texture_number == (int) Texture_Type.switch_green_test) green_switch_down = true;
          if (floor > -1 && brush_control.brush[floor].top_texture_number == (int) Texture_Type.switch_red_test) red_switch_down = true;
          if (floor > -1 && brush_control.brush[floor].top_texture_number == (int) Texture_Type.switch_yellow_test) yellow_switch_down = true;
          if (floor > -1 && brush_control.brush[floor].top_texture_number == (int) Texture_Type.switch_blue_test) blue_switch_down = true;
          }
        }

      if (green_switch_down == true || green_zones_full == true) green_doors_open = true;
      if (red_switch_down == true || red_zones_full == true) red_doors_open = true;
      if (yellow_switch_down == true || yellow_zones_full == true) yellow_doors_open = true;
      if (blue_switch_down == true) blue_doors_open = true;

      // if door status changed, play sound
      if (red_doors_open != red_doors_open_before || yellow_doors_open != yellow_doors_open_before
          || green_doors_open != green_doors_open_before || blue_doors_open != blue_doors_open_before)
        sound_door.Play ();
      
      // change solid value of door brushes based on switches
      foreach (Brush br in brush_control.brush)
        {
        if (blue_doors_open)
          {
          switch (br.top_texture_number)
            {
            case (int) Texture_Type.door_blue_h_top_closed_test:
            case (int) Texture_Type.door_blue_v_top_closed_test:
              if (blue_doors_open) br.solid = false;
              else br.solid = true;
              break;
            case (int) Texture_Type.door_red_h_top_closed_test:
            case (int) Texture_Type.door_red_v_top_closed_test:
              if (red_doors_open) br.solid = false;
              else br.solid = true;
              break;
            case (int) Texture_Type.door_green_h_top_closed_test:
            case (int) Texture_Type.door_green_v_top_closed_test:
              if (green_doors_open) br.solid = false;
              else br.solid = true;
              break;
            case (int) Texture_Type.door_yellow_h_top_closed_test:
            case (int) Texture_Type.door_yellow_v_top_closed_test:
              if (yellow_doors_open) br.solid = false;
              else br.solid = true;
              break;
            //case (int) Texture_Type.door_purple_h_top_closed_test:
            //case (int) Texture_Type.door_purple_v_top_closed_test:
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
          if (brush_control.brush[b].top_texture_number == (int) Texture_Type.door_red_v_top_closed_test && red_doors_open == true) clip = -1;
          else if (brush_control.brush[b].top_texture_number == (int) Texture_Type.door_red_h_top_closed_test && red_doors_open == true) clip = -1;
          else if (brush_control.brush[b].top_texture_number == (int) Texture_Type.door_green_v_top_closed_test && green_doors_open == true) clip = -1;
          else if (brush_control.brush[b].top_texture_number == (int) Texture_Type.door_green_h_top_closed_test && green_doors_open == true) clip = -1;
          else if (brush_control.brush[b].top_texture_number == (int) Texture_Type.door_yellow_v_top_closed_test && yellow_doors_open == true) clip = -1;
          else if (brush_control.brush[b].top_texture_number == (int) Texture_Type.door_yellow_h_top_closed_test && yellow_doors_open == true) clip = -1;
          else if (brush_control.brush[b].top_texture_number == (int) Texture_Type.door_blue_v_top_closed_test && blue_doors_open == true) clip = -1;
          else if (brush_control.brush[b].top_texture_number == (int) Texture_Type.door_blue_h_top_closed_test && blue_doors_open == true) clip = -1;
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
      int clip;

      for (clip = 0; clip < character_control.character.Count; clip += 1)
        {
        if (character_control.active (clip) && clip != object_control.obj[o].source && object_control.obj[o].destroyed == false
            && character_control.character[clip].x + (character_control.character[clip].width / 2) > object_control.obj[o].x - (object_control.obj[o].width / 2)  // left of object
            && character_control.character[clip].x - (character_control.character[clip].width / 2) < object_control.obj[o].x + (object_control.obj[o].width / 2)  // right of object
            && character_control.character[clip].y + character_control.character[clip].length > object_control.obj[o].y  // south of object
            && character_control.character[clip].y < object_control.obj[o].y + object_control.obj[o].length  // north of object
            && character_control.character[clip].z + character_control.character[clip].height > object_control.obj[o].z
            && character_control.character[clip].z < object_control.obj[o].z + object_control.obj[o].height)
          {

          if (object_control.obj[o].type == (int) Object_Type.health)
            {
            character_control.character[clip].health += 20;
            particle_itemgrab (object_control.obj[o].x, object_control.obj[o].y, object_control.obj[o].z, 0);
            sound_pickup_glove.Play ();
            destroy_object (o);
            }
          else if (object_control.obj[o].type == (int) Object_Type.shirt_yellow && clip == PLAYER)
            {
            character_control.character[PLAYER].shirt_power = true;
            character_control.character[PLAYER].shirt = (int) Object_Type.shirt_yellow;// (int) Object_Type.shirt_yellow;
            particle_itemgrab (object_control.obj[o].x, object_control.obj[o].y, object_control.obj[o].z, 0);
            sound_pickup_glove.Play ();
            destroy_object (o);
            }
          else if (object_control.obj[o].type == (int) Object_Type.shirt_red && clip == PLAYER)
            {
            character_control.character[PLAYER].shirt_fire = true;
            character_control.character[PLAYER].shirt = (int) Object_Type.shirt_red;// "fire";
            particle_itemgrab (object_control.obj[o].x, object_control.obj[o].y, object_control.obj[o].z, 0);
            sound_pickup_glove.Play ();
            destroy_object (o);
            }
          else if (object_control.obj[o].type == (int) Object_Type.shirt_white && clip == PLAYER)
            {
            character_control.character[PLAYER].shirt_ice = true;
            character_control.character[PLAYER].shirt = (int) Object_Type.shirt_white;// "ice";
            particle_itemgrab (object_control.obj[o].x, object_control.obj[o].y, object_control.obj[o].z, 0);
            sound_pickup_glove.Play ();
            destroy_object (o);
            }
          else if (object_control.obj[o].type == (int) Object_Type.shirt_fushia && clip == PLAYER)
            {
            character_control.character[PLAYER].shirt_magnetic = true;
            character_control.character[PLAYER].shirt = (int) Object_Type.shirt_fushia;// "magnet";
            particle_itemgrab (object_control.obj[o].x, object_control.obj[o].y, object_control.obj[o].z, 0);
            sound_pickup_glove.Play ();
            destroy_object (o);
            }
          else if (object_control.obj[o].type == (int) Object_Type.shirt_teal && clip == PLAYER)
            {
            character_control.character[PLAYER].shirt_electric = true;
            character_control.character[PLAYER].shirt = (int) Object_Type.shirt_teal;// "electric";
            particle_itemgrab (object_control.obj[o].x, object_control.obj[o].y, object_control.obj[o].z, 0);
            sound_pickup_glove.Play ();
            destroy_object (o);
            }
          else if ((object_control.obj[o].type == (int) Object_Type.rock)
                    && clip != object_control.obj[o].source)
            {
            if (object_control.obj[o].velocity > 0) character_damage (clip, 20, 4, 0, object_control.obj[o].x, object_control.obj[o].y, "impact", object_control.obj[o].source);
            }
          else if (object_control.obj[o].type == (int) Object_Type.key && clip == PLAYER && object_control.obj[o].destroyed == false)
            {
            character_control.character[clip].keys += 1;
            particle_itemgrab (object_control.obj[o].x, object_control.obj[o].y, object_control.obj[o].z, 0);
            sound_pickup_key.Play ();
            destroy_object (o);
            }
          else if (object_control.obj[o].type == (int) Object_Type.coin && clip == PLAYER)
            {
            character_control.character[clip].coins += 1;
            particle_coingrab (object_control.obj[o].x, object_control.obj[o].y, object_control.obj[o].z);
            sound_pickup_coin.Play ();
            destroy_object (o);
            }
          else if (object_control.obj[o].type == (int) Object_Type.scrap_metal && clip == PLAYER)
            {
            character_control.character[clip].scrap_metal += 1;
            particle_itemgrab (object_control.obj[o].x, object_control.obj[o].y, object_control.obj[o].z, 0);
            sound_pickup_glove.Play ();
            destroy_object (o);
            }
          else if (object_control.obj[o].type == (int) Object_Type.energy && clip == PLAYER)
            {
            character_control.character[clip].energy += 1;
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

    bool brush_on_screen (int b)
      {
      bool on_screen = true;

      // larger pieces do not conform to formula
      if (brush_control.brush[b].top_texture_number > (int) Texture_Type.single_piece) on_screen = true;
      else if (brush_control.brush[b].x + brush_control.brush[b].width + screen.scroll_x < 0) on_screen = false;
      else if (brush_control.brush[b].x + screen.scroll_x > screen.width) on_screen = false;
      else if ((screen.height - brush_control.brush[b].y) - (brush_control.brush[b].z * parallax) - (brush_control.brush[b].height * parallax) + screen.scroll_y + tilesize < 0) on_screen = false;
      else if ((screen.height - brush_control.brush[b].y - brush_control.brush[b].length) - (brush_control.brush[b].z * parallax) - (brush_control.brush[b].height * parallax) + screen.scroll_y > screen.height) on_screen = false;

      return on_screen;
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
        //else if (transition == Game_State.game && newgame == true) New_Game (game_time);
        //transition = NONE;
        //fader.reset ();
        //fadein ();
        //}
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    int add_fixture (Fixture_Type type, int x, int y, int z)
      {
      if (fixture_control.fixture.Count < Fixture_Control.max_fixtures) fixture_control.add (type, x, y, z);

      return fixture_control.fixture.Count - 1;
      }

    ////////////////////////////////////////////////////////////////////////////////

    void Update_Fixtures ()
      {
      int f2, b_clip;

      for (int f = 0; f < fixture_control.fixture.Count; f += 1)
        {
        fixture_control.fixture[f].update_frames ();

        // laser tripwires
        if (fixture_control.fixture[f].type == Fixture_Type.laser_horizontal_green_test)
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
          if (f2 > -1 && fixture_control.fixture[f2].type == Fixture_Type.laser_horizontal_green_test && fixture_control.fixture[f2].electric == true && fixture_control.fixture[f2].powered == true)
            fixture_control.fixture[f].powered = true;
          //else f2 = fixture_control.point_collide (fixture_control.fixture[f].x + fixture_control.fixture[f].width + (tilesize / 2), fixture_control.fixture[f].y + (fixture_control.fixture[f].length / 2), fixture_control.fixture[f].z + (tilesize / 2));
          //if (f2 > -1)// && fixture_control.fixture[f2].electric == true)// && fixture_control.fixture[f2].powered == true) 
          //  fixture_control.fixture[f].powered = true;
          //else f2 = fixture_control.point_collide (fixture_control.fixture[f].x + fixture_control.fixture[f].width + (tilesize / 2), fixture_control.fixture[f].y + (fixture_control.fixture[f].length / 2), fixture_control.fixture[f].z + tilesize);
          //if (f2 > -1)// && fixture_control.fixture[f2].electric == true)// && fixture_control.fixture[f2].powered == true) 
          //  fixture_control.fixture[f].powered = true;
          }

        // particles
        if (fixture_control.fixture[f].type == Fixture_Type.laser_horizontal_green_test)
          {
          if (rnd.Next (0, 50) == 0) particle_green_tripwire (fixture_control.fixture[f].x + 6, fixture_control.fixture[f].y + 17, fixture_control.fixture[f].z + fixture_control.fixture[f].height, 0);
          if (rnd.Next (0, 50) == 0) particle_green_tripwire (fixture_control.fixture[f].x + fixture_control.fixture[f].width - 6, fixture_control.fixture[f].y + 17, fixture_control.fixture[f].z + fixture_control.fixture[f].height, 180);
          }

        }
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

        found_brush = brush_control.brush_west_of_brush (brush_control.brush[b], false, false);
        if (found_brush > -1 && brush_control.brush[found_brush].top_sticker > 0) add_top = false;  // stickers too close

        found_brush = brush_control.brush_east_of_brush (brush_control.brush[b], false, false);
        if (found_brush > -1 && brush_control.brush[found_brush].top_sticker > 0) add_top = false;  // stickers too close

        found_brush = brush_control.brush_above_brush (brush_control.brush[b], false, false);
        if (found_brush > -1 && brush_control.brush[found_brush].top_sticker > 0) add_top = false;  // sticker under wall

        found_fixture = fixture_control.fixture_above_brush (brush_control.brush[b]);
        if (found_fixture > -1) add_top = false;  // fixture sitting on floor

        if (add_top)
          {
          if (brush_control.brush[b].top_texture_number == (int) Texture_Type.brick_red && rnd.Next (0, 15) == 1) apply_top_sticker (b, "factory");
          if (brush_control.brush[b].top_texture_number == (int) Texture_Type.tile_black && rnd.Next (0, 75) == 0) apply_top_sticker (b, "factory floor");
          if (brush_control.brush[b].top_texture_number == (int) Texture_Type.tile_brown && rnd.Next (0, 150) == 0) apply_top_sticker (b, "office floor");
          }

        // front stickers
        if (brush_control.brush[b].front_sticker > 0) add_front = false;  // already has sticker
        found_brush = brush_control.brush_west_of_brush (brush_control.brush[b], false, false);
        if (found_brush > -1 && brush_control.brush[found_brush].front_sticker > 0) add_front = false;  // stickers too close

        found_brush = brush_control.brush_east_of_brush (brush_control.brush[b], false, false);
        if (found_brush > -1 && brush_control.brush[found_brush].front_sticker > 0) add_front = false;  // stickers too close

        found_brush = brush_control.brush_below_south_of_brush (brush_control.brush[b]);
        if (found_brush == -1) add_front = false;  // sticker outside building

        found_fixture = fixture_control.fixture_south_of_brush (brush_control.brush[b]);
        if (found_fixture > -1) add_front = false;  // sticker behind fixture

        if (add_front)
          {
          if (brush_control.brush[b].front_texture_number == (int) Texture_Type.brick_red && rnd.Next (0, 8) == 0) apply_front_sticker (b, "factory");
          if (brush_control.brush[b].front_texture_number == (int) Texture_Type.drywall_mint_front_test && rnd.Next (0, 8) == 0) apply_front_sticker (b, "office");
          if (brush_control.brush[b].front_texture_number == (int) Texture_Type.drywall_purple_front_test && rnd.Next (0, 8) == 0) apply_front_sticker (b, "office");
          if (brush_control.brush[b].front_texture_number == (int) Texture_Type.drywall_tan_front_test && rnd.Next (0, 8) == 0) apply_front_sticker (b, "office");
          if (brush_control.brush[b].front_texture_number == (int) Texture_Type.drywall_yellow_front_test && rnd.Next (0, 8) == 0) apply_front_sticker (b, "office");
          if (brush_control.brush[b].front_texture_number == (int) Texture_Type.metal_mint_front_test && rnd.Next (0, 8) == 0) apply_front_sticker (b, "factory");
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

    int brush_in_fixture (Brush b, bool solid_only)
      {
      int f = 0;
      int clip = -1;

      while (clip == -1 && f < fixture_control.fixture.Count)
        {
        if (b.x + b.width > fixture_control.fixture[f].x && b.x < fixture_control.fixture[f].x + fixture_control.fixture[f].width
            && b.y + b.length > fixture_control.fixture[f].y && b.y < fixture_control.fixture[f].y + fixture_control.fixture[f].length
            && b.z + b.height > fixture_control.fixture[f].z && b.z < fixture_control.fixture[f].z + fixture_control.fixture[f].height - 1)
          {
          if (solid_only == true && fixture_control.fixture[f].solid == false)
            {
            clip = -1;
            if (fixture_control.fixture[f].type == Fixture_Type.laser_horizontal_green_test) fixture_control.fixture[f].powered = true;
            }
          else clip = f;
          }
        f += 1;
        }

      return clip;
      }

    ////////////////////////////////////////////////////////////////////////////////

    void push_north ()
      {
      int b;

      if (character_control.character[PLAYER].action == Action.grab && brush_control.brush[character_control.character[PLAYER].brush_grab].moveable == true && character_control.character[PLAYER].grab_position == "below")  // push box (on grid)
        {
        if (brush_control.brush[character_control.character[PLAYER].brush_grab].top_texture_number == (int) Texture_Type.box_metal_test && character_control.character[PLAYER].shirt != (int) Object_Type.shirt_yellow && character_control.character[PLAYER].shirt != (int) Object_Type.shirt_red) return;
        else if (brush_control.brush[character_control.character[PLAYER].brush_grab].top_texture_number == (int) Texture_Type.box_ice_test && character_control.character[PLAYER].shirt != (int) Object_Type.shirt_white) return;
        else if (fixture_control.fixture_north_of_brush (brush_control.brush[character_control.character[PLAYER].brush_grab]) > -1) return;
        else
          {
          character_control.character[PLAYER].action = Action.push;
          b = character_control.character[PLAYER].brush_grab;
          brush_control.brush[b].moving = true;
          brush_control.brush[b].moving_north = true;
          brush_control.brush[b].destination_x = brush_control.brush[b].x;
          brush_control.brush[b].destination_y = brush_control.brush[b].y + box_move;
          brush_control.brush[b].ext_y_velocity = character_control.character[PLAYER].speed * .45;
          character_control.character[PLAYER].dx = brush_control.brush[b].x + (brush_control.brush[b].width / 2);
          character_control.character[PLAYER].dy = brush_control.brush[b].y - (tilesize / 3);          
          character_control.character[PLAYER].push_dir = "up";
          character_control.character[PLAYER].self_x_velocity = 0;
          redraw_floor_buffer = true;

          foreach (Brush brush in brush_control.brush)
            {
            brush_control.calculate_top_shadows (brush);
            }
          }
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void push_south ()
      {
      int b;

      if (character_control.character[PLAYER].action == Action.grab)// && brush_control.brush[character_control.character[PLAYER].brush_grab].moveable == true && character_control.character[PLAYER].grab_position == "above")
        {
        if (brush_control.brush[character_control.character[PLAYER].brush_grab].top_texture_number == (int) Texture_Type.box_metal_test && character_control.character[PLAYER].shirt != (int) Object_Type.shirt_yellow && character_control.character[PLAYER].shirt != (int) Object_Type.shirt_red) return;
        else if (brush_control.brush[character_control.character[PLAYER].brush_grab].top_texture_number == (int) Texture_Type.box_ice_test && character_control.character[PLAYER].shirt != (int) Object_Type.shirt_white) return;
        else if (fixture_control.fixture_south_of_brush (brush_control.brush[character_control.character[PLAYER].brush_grab]) > -1) return;
        else
          {
          character_control.character[PLAYER].action = Action.push;
          b = character_control.character[PLAYER].brush_grab;
          brush_control.brush[b].moving = true;
          brush_control.brush[b].moving_south = true;
          brush_control.brush[b].destination_x = brush_control.brush[b].x;
          brush_control.brush[b].destination_y = brush_control.brush[b].y - box_move;
          brush_control.brush[b].ext_y_velocity = -1 * character_control.character[PLAYER].speed * .45;
          character_control.character[PLAYER].dx = brush_control.brush[b].x + (brush_control.brush[b].width / 2);
          character_control.character[PLAYER].dy = brush_control.brush[b].y + brush_control.brush[b].length + (tilesize / 4);
          character_control.character[PLAYER].push_dir = "down";
          character_control.character[PLAYER].self_x_velocity = 0;
          redraw_floor_buffer = true;

          foreach (Brush brush in brush_control.brush)
            {
            brush_control.calculate_top_shadows (brush);
            }
          }
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void push_west ()
      {
      int b;

      if (character_control.character[PLAYER].action == Action.grab && brush_control.brush[character_control.character[PLAYER].brush_grab].moveable == true && character_control.character[PLAYER].grab_position == "right")  // push box (on grid)
        {
        if (brush_control.brush[character_control.character[PLAYER].brush_grab].top_texture_number == (int) Texture_Type.box_metal_test && character_control.character[PLAYER].shirt != (int) Object_Type.shirt_yellow && character_control.character[PLAYER].shirt != (int) Object_Type.shirt_red) return;
        else if (brush_control.brush[character_control.character[PLAYER].brush_grab].top_texture_number == (int) Texture_Type.box_ice_test && character_control.character[PLAYER].shirt != (int) Object_Type.shirt_white) { }
        else if (fixture_control.fixture_west_of_brush (brush_control.brush[character_control.character[PLAYER].brush_grab]) > -1) return;
        else
          {
          character_control.character[PLAYER].action = Action.push;
          b = character_control.character[PLAYER].brush_grab;
          brush_control.brush[b].moving = true;
          brush_control.brush[b].moving_west = true;
          brush_control.brush[b].destination_x = brush_control.brush[b].x - box_move;
          brush_control.brush[b].destination_y = brush_control.brush[b].y;
          brush_control.brush[b].ext_x_velocity = -1 * character_control.character[PLAYER].speed * .45;
          character_control.character[PLAYER].dx = brush_control.brush[b].x + brush_control.brush[b].width + (tilesize / 3);
          character_control.character[PLAYER].dy = brush_control.brush[b].y + (brush_control.brush[b].length / 2);
          character_control.character[PLAYER].push_dir = "left";
          character_control.character[PLAYER].self_y_velocity = 0;
          redraw_floor_buffer = true;

          foreach (Brush brush in brush_control.brush)
            {
            brush_control.calculate_top_shadows (brush);
            }
          }
        }
      }

    ////////////////////////////////////////////////////////////////////////////////

    void push_east ()
      {
      int b;

       if (character_control.character[PLAYER].action == Action.grab && brush_control.brush[character_control.character[PLAYER].brush_grab].moveable == true && character_control.character[PLAYER].grab_position == "left")  // push box (on grid)
        {
        if (brush_control.brush[character_control.character[PLAYER].brush_grab].top_texture_number == (int) Texture_Type.box_metal_test && character_control.character[PLAYER].shirt != (int) Object_Type.shirt_yellow && character_control.character[PLAYER].shirt != (int) Object_Type.shirt_red) return;
        else if (brush_control.brush[character_control.character[PLAYER].brush_grab].top_texture_number == (int) Texture_Type.box_ice_test && character_control.character[PLAYER].shirt != (int) Object_Type.shirt_white) return;
        else if (fixture_control.fixture_east_of_brush (brush_control.brush[character_control.character[PLAYER].brush_grab]) > -1) return;
        else
          {
          character_control.character[PLAYER].action = Action.push;
          b = character_control.character[PLAYER].brush_grab;
          brush_control.brush[b].moving = true;
          brush_control.brush[b].moving_east = true;
          brush_control.brush[b].destination_x = brush_control.brush[b].x + box_move;
          brush_control.brush[b].destination_y = brush_control.brush[b].y;
          brush_control.brush[b].ext_x_velocity = character_control.character[PLAYER].speed * .45;
          character_control.character[PLAYER].dx = brush_control.brush[b].x - (tilesize / 3);
          character_control.character[PLAYER].dy = brush_control.brush[b].y + (brush_control.brush[b].length / 2);
          character_control.character[PLAYER].push_dir = "right";
          character_control.character[PLAYER].self_y_velocity = 0;
          redraw_floor_buffer = true;

          foreach (Brush brush in brush_control.brush)
            {
            brush_control.calculate_top_shadows (brush);
            }
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

    int character_on_fixture (Character c)
      {
      // reset point for character and not for brush_control.brush?
      int fixture_below = fixture_control.point_collide (c.x + (c.width / 2), c.y + (c.length / 2), c.z - 1);
      if (fixture_below == -1) fixture_below = fixture_control.point_collide (c.x + 1, c.y + 1, c.z - 1);
      if (fixture_below == -1) fixture_below = fixture_control.point_collide (c.x + c.width - 1, c.y + 1, c.z - 1);
      if (fixture_below == -1) fixture_below = fixture_control.point_collide (c.x + 1, c.y + c.length - 1, c.z - 1);
      if (fixture_below == -1) fixture_below = fixture_control.point_collide (c.x + c.width - 1, c.y + c.length - 1, c.z - 1);

      return fixture_below;
      }

    ////////////////////////////////////////////////////////////////////////////////

    bool character_sees_character (int c1, int c2)
      {
      // can character 1 see character 2?

      bool sees_character = true;
      double eye_x = character_control.character[c1].x;
      double eye_y = character_control.character[c1].y;
      double eye_z = character_control.character[c1].z + character_control.character[c1].height;
      double eye_dir = get_direction (character_control.character[c1].x, character_control.character[c1].y, character_control.character[c2].x, character_control.character[c2].y);
      double distance = distance2d (eye_x, eye_y, character_control.character[c2].x, character_control.character[c2].y);
      bool endloop = false;

      while (endloop == false)
        {
        eye_x += 4 * Math.Cos (eye_dir);
        eye_y += 4 * Math.Sin (eye_dir);
        if (brush_control.point_in_brush (Convert.ToInt32 (eye_x), Convert.ToInt32 (eye_y), Convert.ToInt32 (eye_z), true, true) >= 0) { sees_character = false; endloop = true; }
        if (distance2d (character_control.character[c1].x, character_control.character[c1].y, eye_x, eye_y) >= distance) { sees_character = true; endloop = true; }
        }

      return sees_character;
      }

    //////////////////////////////////////////////////////////////////////////////////

    bool character_reach_character (int c1, int c2)
      {
      bool reach = false;
      double x_distance, y_distance, z_distance, h_distance;
      double arm_x, arm_y, arm_z;

      double reach_distance2 = character_control.reach_distance;
      if (c1 == PLAYER) reach_distance2 = character_control.reach_distance * 1.2;

      x_distance = character_control.character[c1].dx - character_control.character[c2].dx;
      y_distance = character_control.character[c1].dy - character_control.character[c2].dy;
      z_distance = Math.Abs (character_control.character[c1].dz - character_control.character[c2].dz);
      h_distance = Math.Sqrt ((x_distance * x_distance) + (y_distance * y_distance));

      // if he's close enough to hit him
      if (h_distance < reach_distance2 && z_distance < reach_distance2 * 2)
        {
        reach = true;

        arm_x = character_control.character[c1].x;
        arm_y = character_control.character[c1].y;
        arm_z = character_control.character[c1].z;

        // if there are no walls between them
        for (int d = 0; d < h_distance; d += 1)
          {
          arm_x += 1 * Math.Cos (Convert.ToInt32 (character_control.character[c1].dir));
          if (brush_control.point_in_brush (Convert.ToInt16 (arm_x), Convert.ToInt16 (arm_y), Convert.ToInt16 (arm_z), true, true) >= 0) reach = false;

          arm_y += 1 * Math.Sin (character_control.character[c1].dir);
          if (brush_control.point_in_brush (Convert.ToInt16 (arm_x), Convert.ToInt16 (arm_y), Convert.ToInt16 (arm_z), true, true) >= 0) reach = false;
          }
        }

      return reach;
      }

    ////////////////////////////////////////////////////////////////////////////////

    public void load_textures ()
      {
      for (int t = 0; t < brush_control.tile.Length; t += 1)
        {
        brush_control.tile[t] = new Brush_Control.Tile ();
        }

      load_tile (brush_control.tile[(int) Texture_Type.asphalt_test],                    "AS ", "asphalt_test");
      load_tile (brush_control.tile[(int) Texture_Type.box_ice_test],                    "i  ", "box_ice4");
      load_tile (brush_control.tile[(int) Texture_Type.box_metal_test],                  "m  ", "box_metal_test");
      load_tile (brush_control.tile[(int) Texture_Type.box_wood],                        "b  ", "box_wood");
      load_tile (brush_control.tile[(int) Texture_Type.brick_blue_test],                 "BB ", "brick_blue_test");
      load_tile (brush_control.tile[(int) Texture_Type.brick_grey_test],                 "BE ", "brick_grey_test");
      load_tile (brush_control.tile[(int) Texture_Type.brick_red],                       "BR ", "brick_red");
      load_tile (brush_control.tile[(int) Texture_Type.brick_white_test],                "BW ", "brick_white_test");
      load_tile (brush_control.tile[(int) Texture_Type.brick_yellow_test],               "BY ", "brick_yellow_test");
      load_tile (brush_control.tile[(int) Texture_Type.carpet_grey_test],                "CE ", "carpet_grey_test");
      load_tile (brush_control.tile[(int) Texture_Type.carpet_purple_test],              "CP ", "carpet_purple_test");
      load_tile (brush_control.tile[(int) Texture_Type.door_red_v_top_closed_test],      "", "door_test_red_vertical_top_closed");
      load_tile (brush_control.tile[(int) Texture_Type.door_red_v_top_open_test],        "", "door_test_red_vertical_top_open");
      load_tile (brush_control.tile[(int) Texture_Type.door_red_v_front_closed_test],    "", "door_test_red_vertical_front_closed");
      load_tile (brush_control.tile[(int) Texture_Type.door_red_v_front_open_test],      "", "door_test_red_vertical_front_open");
      load_tile (brush_control.tile[(int) Texture_Type.door_red_h_top_closed_test],      "", "door_test_red_horizontal_top_closed");
      load_tile (brush_control.tile[(int) Texture_Type.door_red_h_top_open_test],        "", "door_test_red_horizontal_top_open");
      load_tile (brush_control.tile[(int) Texture_Type.door_red_h_front_closed_test],    "", "door_test_red_horizontal_front_closed");
      load_tile (brush_control.tile[(int) Texture_Type.door_red_h_front_open_test],      "", "door_test_red_horizontal_front_open");
      load_tile (brush_control.tile[(int) Texture_Type.door_yellow_v_top_closed_test],   "", "door_test_yellow_vertical_top_closed");
      load_tile (brush_control.tile[(int) Texture_Type.door_yellow_v_top_open_test],     "", "door_test_yellow_vertical_top_open");
      load_tile (brush_control.tile[(int) Texture_Type.door_yellow_v_front_closed_test], "", "door_test_yellow_vertical_front_closed");
      load_tile (brush_control.tile[(int) Texture_Type.door_yellow_v_front_open_test],   "", "door_test_yellow_vertical_front_open");
      load_tile (brush_control.tile[(int) Texture_Type.door_yellow_h_top_closed_test],   "", "door_test_yellow_horizontal_top_closed");
      load_tile (brush_control.tile[(int) Texture_Type.door_yellow_h_top_open_test],     "", "door_test_yellow_horizontal_top_open");
      load_tile (brush_control.tile[(int) Texture_Type.door_yellow_h_front_closed_test], "", "door_test_yellow_horizontal_front_closed");
      load_tile (brush_control.tile[(int) Texture_Type.door_yellow_h_front_open_test],   "", "door_test_yellow_horizontal_front_open");
      load_tile (brush_control.tile[(int) Texture_Type.door_green_v_top_closed_test],    "", "door_test_green_vertical_top_closed");
      load_tile (brush_control.tile[(int) Texture_Type.door_green_v_top_open_test],      "", "door_test_green_vertical_top_open");
      load_tile (brush_control.tile[(int) Texture_Type.door_green_v_front_closed_test],  "", "door_test_green_vertical_front_closed");
      load_tile (brush_control.tile[(int) Texture_Type.door_green_v_front_open_test],    "", "door_test_green_vertical_front_open");
      load_tile (brush_control.tile[(int) Texture_Type.door_green_h_top_closed_test],    "", "door_test_green_horizontal_top_closed");
      load_tile (brush_control.tile[(int) Texture_Type.door_green_h_top_open_test],      "", "door_test_green_horizontal_top_open");
      load_tile (brush_control.tile[(int) Texture_Type.door_green_h_front_closed_test],  "", "door_test_green_horizontal_front_closed");
      load_tile (brush_control.tile[(int) Texture_Type.door_green_h_front_open_test],    "", "door_test_green_horizontal_front_open");
      load_tile (brush_control.tile[(int) Texture_Type.door_blue_v_top_closed_test],     "", "door_test_blue_vertical_top_closed");
      load_tile (brush_control.tile[(int) Texture_Type.door_blue_v_top_open_test],       "", "door_test_blue_vertical_top_open");
      load_tile (brush_control.tile[(int) Texture_Type.door_blue_v_front_closed_test],   "", "door_test_blue_vertical_front_closed");
      load_tile (brush_control.tile[(int) Texture_Type.door_blue_v_front_open_test],     "", "door_test_blue_vertical_front_open");
      load_tile (brush_control.tile[(int) Texture_Type.door_blue_h_top_closed_test],     "", "door_test_blue_horizontal_top_closed");
      load_tile (brush_control.tile[(int) Texture_Type.door_blue_h_top_open_test],       "", "door_test_blue_horizontal_top_open");
      load_tile (brush_control.tile[(int) Texture_Type.door_blue_h_front_closed_test],   "", "door_test_blue_horizontal_front_closed");
      load_tile (brush_control.tile[(int) Texture_Type.door_blue_h_front_open_test],     "", "door_test_blue_horizontal_front_open");
      load_tile (brush_control.tile[(int) Texture_Type.drywall_mint_top_test],           "DM ", "drywall_mint_top_test");
      load_tile (brush_control.tile[(int) Texture_Type.drywall_mint_front_test],         "",    "drywall_mint_front_test");
      load_tile (brush_control.tile[(int) Texture_Type.drywall_purple_top_test],         "DP ", "drywall_purple_top_test");
      load_tile (brush_control.tile[(int) Texture_Type.drywall_purple_front_test],       "",    "drywall_purple_front_test");
      load_tile (brush_control.tile[(int) Texture_Type.drywall_tan_top_test],            "DT ", "drywall_tan_top_test");
      load_tile (brush_control.tile[(int) Texture_Type.drywall_tan_front_test],          "",    "drywall_tan_front_test");
      load_tile (brush_control.tile[(int) Texture_Type.drywall_yellow_top_test],         "DY ", "drywall_yellow_top_test");
      load_tile (brush_control.tile[(int) Texture_Type.drywall_yellow_front_test],       "",    "drywall_yellow_front_test");
      load_tile (brush_control.tile[(int) Texture_Type.exit_red_v_top_closed_test],      "", "exit_test_red_vertical_top_closed");
      load_tile (brush_control.tile[(int) Texture_Type.exit_red_v_top_open_test],        "", "exit_test_red_vertical_top_open");
      load_tile (brush_control.tile[(int) Texture_Type.exit_red_v_front_closed_test],    "", "exit_test_red_vertical_front_closed");
      load_tile (brush_control.tile[(int) Texture_Type.exit_red_v_front_open_test],      "", "exit_test_red_vertical_front_open");
      load_tile (brush_control.tile[(int) Texture_Type.exit_red_h_top_closed_test],      "", "exit_test_red_horizontal_top_closed");
      load_tile (brush_control.tile[(int) Texture_Type.exit_red_h_top_open_test],        "", "exit_test_red_horizontal_top_open");
      load_tile (brush_control.tile[(int) Texture_Type.exit_red_h_front_closed_test],    "", "exit_test_red_horizontal_front_closed");
      load_tile (brush_control.tile[(int) Texture_Type.exit_red_h_front_open_test],      "", "exit_test_red_horizontal_front_open");
      load_tile (brush_control.tile[(int) Texture_Type.floor_grate_test],                "", "floor_grate_test");
      load_tile (brush_control.tile[(int) Texture_Type.floor_logo_test],                 "", "floor_test_logo");
      load_tile (brush_control.tile[(int) Texture_Type.floor_metal_test],                "FM ", "floor_metal_test");
      load_tile (brush_control.tile[(int) Texture_Type.floor_zone_green_test],           "ZG ", "loading_zone_green_test");
      load_tile (brush_control.tile[(int) Texture_Type.floor_zone_red_test],             "ZR ", "loading_zone_red_test");
      load_tile (brush_control.tile[(int) Texture_Type.floor_zone_yellow_test],          "ZY ", "loading_zone_yellow_test");
      load_tile (brush_control.tile[(int) Texture_Type.gateway_v_top_test],              "", "gateway_test_vertical_top");
      load_tile (brush_control.tile[(int) Texture_Type.gateway_v_front_closed_test],     "", "gateway_test_vertical_front_closed");
      load_tile (brush_control.tile[(int) Texture_Type.gateway_v_front_open_test],       "", "gateway_test_vertical_front_open");
      load_tile (brush_control.tile[(int) Texture_Type.gateway_h_top_test],              "", "gateway_test_horizontal_top");
      load_tile (brush_control.tile[(int) Texture_Type.gateway_h_front_test],            "", "gateway_test_horizontal_front");
      load_tile (brush_control.tile[(int) Texture_Type.grass],                           "GR ", "grass");
      load_tile (brush_control.tile[(int) Texture_Type.metal_blue_top_test],             "MB ", "metal_blue_top_test");
      load_tile (brush_control.tile[(int) Texture_Type.metal_blue_front_test],           "",    "metal_blue_front_test2");
      load_tile (brush_control.tile[(int) Texture_Type.metal_mint_top_test],             "MM ", "metal_mint_top_test");
      load_tile (brush_control.tile[(int) Texture_Type.metal_mint_front_test],           "",    "metal_mint_front_test");
      load_tile (brush_control.tile[(int) Texture_Type.sidewalk_test],                   "SW ", "sidewalk_test");
      load_tile (brush_control.tile[(int) Texture_Type.switch_green_test],               "", "switch_test_green");
      load_tile (brush_control.tile[(int) Texture_Type.switch_green_down_test],          "", "switch_test_green_down");
      load_tile (brush_control.tile[(int) Texture_Type.switch_red_test],                 "", "switch_test_red");
      load_tile (brush_control.tile[(int) Texture_Type.switch_red_down_test],            "", "switch_test_red_down");
      load_tile (brush_control.tile[(int) Texture_Type.switch_blue_test],                "", "switch_test_blue");
      load_tile (brush_control.tile[(int) Texture_Type.switch_blue_down_test],           "", "switch_test_blue_down");
      load_tile (brush_control.tile[(int) Texture_Type.switch_yellow_test],              "", "switch_test_yellow");
      load_tile (brush_control.tile[(int) Texture_Type.switch_yellow_down_test],         "", "switch_test_yellow_down");
      load_tile (brush_control.tile[(int) Texture_Type.texture_highlight_red],           "", "texture_highlight_red");
      load_tile (brush_control.tile[(int) Texture_Type.tile_black],                      "TK ", "tile_black");
      load_tile (brush_control.tile[(int) Texture_Type.tile_blue_test],                  "TB ", "tile_blue_test");
      load_tile (brush_control.tile[(int) Texture_Type.tile_brown],                      "TN ", "tile_brown");
      load_tile (brush_control.tile[(int) Texture_Type.tile_mint],                       "TM ", "tile_mint");
      load_tile (brush_control.tile[(int) Texture_Type.metal_black_test],                "MK ", "metal_black_test");
      load_tile (brush_control.tile[(int) Texture_Type.incinerator_test_up],             "", "incinerator_test_up");
      load_tile (brush_control.tile[(int) Texture_Type.incinerator_test_down],           "", "incinerator_test_down");
      load_tile (brush_control.tile[(int) Texture_Type.incinerator_test_down_front],     "", "incinerator_test_down_front");
      load_tile (brush_control.tile[(int) Texture_Type.incinerator_test_left],           "", "incinerator_test_left");
      load_tile (brush_control.tile[(int) Texture_Type.incinerator_test_right],          "", "incinerator_test_right");
      load_tile (brush_control.tile[(int) Texture_Type.big_machine_test],                "", "big_machine_test");
      load_tile (brush_control.tile[(int) Texture_Type.box_banded_test],                 "", "box_banded_test");
      }
    
    //////////////////////////////////////////////////////////////////////////////////

    void load_tile (Brush_Control.Tile tile, string new_symbol, string new_file, string new_set = null)
      {
      tile.map_symbol = new_symbol;
      tile.name = new_file;
      if (new_set != null) tile.set = new_set;

      tile.texture = Content.Load<Texture2D> (Texture_Path + new_file);
      }

    ////////////////////////////////////////////////////////////////////////////////

    public void load_fixture_sprites ()
      {
      string fixture_path = "images\\fixtures\\";

      fixture_control.fixture_sprite[(int) Fixture_Type.bench1_west_test] =              Content.Load<Texture2D> (fixture_path + "bench1_west_test");
      fixture_control.fixture_sprite[(int) Fixture_Type.chair1_south_test] =             Content.Load<Texture2D> (fixture_path + "chair1_south_test");
      fixture_control.fixture_sprite[(int) Fixture_Type.chair3_east_test] =              Content.Load<Texture2D> (fixture_path + "chair3_east_test");
      fixture_control.fixture_sprite[(int) Fixture_Type.chair3_north_test] =             Content.Load<Texture2D> (fixture_path + "chair3_north_test");
      fixture_control.fixture_sprite[(int) Fixture_Type.conveyor_north_test] =           Content.Load<Texture2D> (fixture_path + "conveyor_north_test");
      fixture_control.fixture_sprite[(int) Fixture_Type.conveyor_south_test] =           Content.Load<Texture2D> (fixture_path + "conveyor_south_test");
      fixture_control.fixture_sprite[(int) Fixture_Type.conveyor_east_test] =            Content.Load<Texture2D> (fixture_path + "conveyor_east_test");
      fixture_control.fixture_sprite[(int) Fixture_Type.conveyor_west_test] =            Content.Load<Texture2D> (fixture_path + "conveyor_west_test");
      fixture_control.fixture_sprite[(int) Fixture_Type.couch_south_test] =              Content.Load<Texture2D> (fixture_path + "couch_test");
      fixture_control.fixture_sprite[(int) Fixture_Type.desk1_test] =                    Content.Load<Texture2D> (fixture_path + "desk1_test");
      fixture_control.fixture_sprite[(int) Fixture_Type.door_mint_horizontal_test] =     Content.Load<Texture2D> (fixture_path + "door_mint_horizontal_test");
      fixture_control.fixture_sprite[(int) Fixture_Type.door_mint_vertical_test] =       Content.Load<Texture2D> (fixture_path + "door_mint_vertical_test");
      fixture_control.fixture_sprite[(int) Fixture_Type.door_white_horizontal_test] =    Content.Load<Texture2D> (fixture_path + "door_white_horizontal_test");
      fixture_control.fixture_sprite[(int) Fixture_Type.filing_test]                   = Content.Load<Texture2D> (fixture_path + "filing_cabinet_test");
      fixture_control.fixture_sprite[(int) Fixture_Type.keydoor_1_test]                = Content.Load<Texture2D> (fixture_path + "keydoor_1_test");
      fixture_control.fixture_sprite[(int) Fixture_Type.keydoor_10_test]               = Content.Load<Texture2D> (fixture_path + "keydoor_10_test");
      fixture_control.fixture_sprite[(int) Fixture_Type.laser_fixture_horizontal_test] = Content.Load<Texture2D> (fixture_path + "laser_fixture_horizontal_test");
      fixture_control.fixture_sprite[(int) Fixture_Type.laser_horizontal_green_test] =   Content.Load<Texture2D> (fixture_path + "laser_horizontal_green_test");
      fixture_control.fixture_sprite[(int) Fixture_Type.plant1_green_test] =             Content.Load<Texture2D> (fixture_path + "plant1_test");
      fixture_control.fixture_sprite[(int) Fixture_Type.plant1_mint_test] =              Content.Load<Texture2D> (fixture_path + "plant1_mint_test");
      fixture_control.fixture_sprite[(int) Fixture_Type.phone1_test] =                   Content.Load<Texture2D> (fixture_path + "phone1_test");//Texture2D.FromStream (GraphicsDevice, new FileStream (fixture_path + "phone1_test.png
      fixture_control.fixture_sprite[(int) Fixture_Type.table1_test] =                   Content.Load<Texture2D> (fixture_path + "table1_test");//Texture2D.FromStream (GraphicsDevice, new FileStream (fixture_path + "table1_test.png
      fixture_control.fixture_sprite[(int) Fixture_Type.table2_test] =                   Content.Load<Texture2D> (fixture_path + "table2_test");//Texture2D.FromStream (GraphicsDevice, new FileStream (fixture_path + "table2_test.png
      fixture_control.fixture_sprite[(int) Fixture_Type.table3_mint_test] =              Content.Load<Texture2D> (fixture_path + "table3_mint_test");//Texture2D.FromStream (GraphicsDevice, new FileStream (fixture_path + "table3_mint_test.png
      fixture_control.fixture_sprite[(int) Fixture_Type.table3_white_test]             = Content.Load<Texture2D> (fixture_path + "table3_white_test");//Texture2D.FromStream (GraphicsDevice, new FileStream (fixture_path + "table3_white_test.png
      fixture_control.fixture_sprite[(int) Fixture_Type.tv1_test]                      = Content.Load<Texture2D> (fixture_path + "tv1_test");//Texture2D.FromStream (GraphicsDevice, new FileStream (fixture_path + "tv1_test.png
      fixture_control.fixture_sprite[(int) Fixture_Type.tv2_test]                      = Content.Load<Texture2D> (fixture_path + "tv2_test");//Texture2D.FromStream (GraphicsDevice, new FileStream (fixture_path + "tv2_test.png
      fixture_control.fixture_sprite[(int) Fixture_Type.vending_red_test]              = Content.Load<Texture2D> (fixture_path + "vending_red_test");//Texture2D.FromStream (GraphicsDevice, new FileStream (fixture_path + "vending_red_test.png
      fixture_control.fixture_sprite[(int) Fixture_Type.vending_yellow_test]           = Content.Load<Texture2D> (fixture_path + "vending_yellow_test");//Texture2D.FromStream (GraphicsDevice, new FileStream (fixture_path + "vending_yellow_test.png
      fixture_control.fixture_sprite[(int) Fixture_Type.wires_horizontal_test]         = Content.Load<Texture2D> (fixture_path + "wires_horizontal_test");//Texture2D.FromStream (GraphicsDevice, new FileStream (fixture_path + "wires_horizontal_test.png
      fixture_control.fixture_sprite[(int) Fixture_Type.wires_vertical_test]           = Content.Load<Texture2D> (fixture_path + "wires_vertical_test");//Texture2D.FromStream (GraphicsDevice, new FileStream (fixture_path + "wires_vertical_test.png
      fixture_control.fixture_sprite[(int) Fixture_Type.wires_southeast_test]          = Content.Load<Texture2D> (fixture_path + "wires_southeast_test");//Texture2D.FromStream (GraphicsDevice, new FileStream (fixture_path + "wires_southeast_test.png
      }

    ////////////////////////////////////////////////////////////////////////////////

    public void load_character_sprites ()
      {
      character_control.character_sprite[(int) Name.RICHARD, 0]                                   = Content.Load<Texture2D> (character_path + "richard_green");//Texture2D.FromStream (GraphicsDevice, new FileStream (character_path + "richard_green.png
      character_control.character_sprite[(int) Name.RICHARD, (int) Object_Type.shirt_yellow] = Content.Load<Texture2D> (character_path + "richard_yellow");//Texture2D.FromStream (GraphicsDevice, new FileStream (character_path + "richard_yellow.png
      character_control.character_sprite[(int) Name.RICHARD, (int) Object_Type.shirt_white]  = Content.Load<Texture2D> (character_path + "richard_white");//Texture2D.FromStream (GraphicsDevice, new FileStream (character_path + "richard_red.png
      character_control.character_sprite[(int) Name.RICHARD, (int) Object_Type.shirt_red]    = Content.Load<Texture2D> (character_path + "richard_red");//Texture2D.FromStream (GraphicsDevice, new FileStream (character_path + "richard_white.png
      character_control.character_sprite[(int) Name.RICHARD, (int) Object_Type.shirt_teal]   = Content.Load<Texture2D> (character_path + "richard_teal");//Texture2D.FromStream (GraphicsDevice, new FileStream (character_path + "richard_teal.png
      character_control.character_sprite[(int) Name.RICHARD, (int) Object_Type.shirt_fushia] = Content.Load<Texture2D> (character_path + "richard_fushia");//Texture2D.FromStream (GraphicsDevice, new FileStream (character_path + "richard_fushia.png
      character_control.character_sprite[(int) Name.RICHARDS_DAD, 0]                              = Content.Load<Texture2D> (character_path + "richards_dad_test");//Texture2D.FromStream (GraphicsDevice, new FileStream (character_path + "richards_dad_test.png
      character_control.character_sprite[(int) Name.RETARD, 0]                                    = Content.Load<Texture2D> (character_path + "retard_tall_test");//Texture2D.FromStream (GraphicsDevice, new FileStream (character_path + "retard_tall_test.png
      character_control.character_sprite[(int) Name.SECRETARY, 0]                                 = Content.Load<Texture2D> (character_path + "secretary_test");//Texture2D.FromStream (GraphicsDevice, new FileStream (character_path + "secretary_test.png
      character_control.character_sprite[(int) Name.THROWING_RETARD, 0]                           = Content.Load<Texture2D> (character_path + "retard_tall_test");//Texture2D.FromStream (GraphicsDevice, new FileStream (character_path + "hitler_new1.png
      }

    //////////////////////////////////////////////////////////////////////////////////

    }
  }
