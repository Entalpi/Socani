using UnityEngine;

// Game defining color palette
static class SocaniColor {
  public static Color InformationText {
    get { return new Color(1.0f, 0.9517242f, 0.75f); }
  }

  // Neutral color 
  public static Color ActionableText {
    get { return new Color(1.0f, 0.9517242f, 0.75f); }
  }

  public static Color PositiveText {
    get { return new Color(0.1803922f, 0.8000001f, 0.4392157f); }
  }

  public static Color NegativeText {
    get { return new Color(1.0f, 0.8896552f, 0.0f, 0.75f); }
  }
}