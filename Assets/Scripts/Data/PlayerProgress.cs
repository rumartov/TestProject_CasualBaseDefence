using System;

namespace Data
{
  [Serializable]
  public class PlayerProgress
  {
    public State HeroState;
    public WorldData WorldData;
    public PlayerProgress()
    {
      WorldData = new WorldData();
      HeroState = new State();
    }
  }
}