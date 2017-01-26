/* Rewired Constants
   This list was generated on 1/26/2017 12:42:24 PM
   The list applies to only the Rewired Input Manager from which it was generated.
   If you use a different Rewired Input Manager, you will have to generate a new list.
   If you make changes to the exported items in the Rewired Input Manager, you will need to regenerate this list.
*/

namespace RewiredConsts {
    public static class Action {
        // Default
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "Yell")]
        public const int Yell = 0;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "AdjustVolume")]
        public const int Volume = 1;
        [Rewired.Dev.ActionIdFieldInfo(categoryName = "Default", friendlyName = "Pause")]
        public const int Pause = 2;
    }
    public static class Category {
        public const int Default = 0;
    }
    public static class Layout {
        public static class Joystick {
            public const int Default = 0;
        }
        public static class Keyboard {
            public const int Default = 0;
            public const int Player1 = 1;
            public const int Player2 = 2;
        }
        public static class Mouse {
            public const int Default = 0;
        }
        public static class CustomController {
            public const int Default = 0;
        }
    }
}
