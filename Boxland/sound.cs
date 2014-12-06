// Boxland, inc.
// 2011 Nightmare Games
// XNA Game Studio

namespace Boxland
  {
  public partial class Game1 : Microsoft.Xna.Framework.Game
    {
    void Sound_Manager ()
      {
      if (sound_richard_knockout_test == true)
        {
        soundBank.PlayCue ("richard_death_test");
        sound_richard_knockout_test = false;
        }
      if (sound_retard_knockout_test == true)
        {
        soundBank.PlayCue ("retard_death_test");
        sound_retard_knockout_test = false;
        }
      }
    }
  }
