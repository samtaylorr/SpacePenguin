using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{

    public struct Mapping {
        public string xbox;
        public string playstation;
        public string nintendoSwitch;
        public string kbm;

        public Mapping(string xbox, string ps, string ns, string kbm){
            this.xbox = xbox;
            this.playstation = ps;
            this.nintendoSwitch = ns;
            this.kbm = kbm;
        }
    }

    public enum Platform{
        XBOX,
        PLAYSTATION,
        SWITCH,
        KBM
    }

    static Platform CurrentPlatform() {
        if(Application.platform == RuntimePlatform.PS4)               {     return Platform.PLAYSTATION;    }
        if(Application.platform == RuntimePlatform.XboxOne)           {     return Platform.XBOX;           }
        if(Application.platform == RuntimePlatform.PS5)               {     return Platform.PLAYSTATION;    }
        if(Application.platform == RuntimePlatform.Switch)            {     return Platform.SWITCH;         }
        else { return Platform.KBM; }
    }

    public static string ButtonPath(Mapping mapping){
        Platform platform = CurrentPlatform();

        if(platform == Platform.PLAYSTATION)    { return mapping.playstation;    }
        if(platform == Platform.XBOX)           { return mapping.xbox;           }
        if(platform == Platform.SWITCH)         { return mapping.nintendoSwitch; }
        else                                    { return mapping.kbm;            }
    }

    public static class ButtonMappings {
        public static Mapping DPAD_UP = new Mapping(
            "Buttons/Xbox/XboxSeriesX_Dpad_Up",
            "Buttons/PS5/PS5_Dpad_Up",
            "Buttons/Switch/Switch_Dpad_Up",
            "Buttons/KBM/Dark/W_Key_Dark"
        );

        public static Mapping DPAD_DOWN = new Mapping(
            "Buttons/Xbox/XboxSeriesX_Dpad_Down",
            "Buttons/PS5/PS5_Dpad_Down",
            "Buttons/Switch/Switch_Dpad_Down",
            "Buttons/KBM/Dark/S_Key_Dark"
        );

        public static Mapping DPAD_LEFT = new Mapping(
            "Buttons/Xbox/XboxSeriesX_Dpad_Left",
            "Buttons/PS5/PS5_Dpad_Left",
            "Buttons/Switch/Switch_Dpad_Left",
            "Buttons/KBM/Dark/D_Key_Dark"
        );

        public static Mapping DPAD_RIGHT = new Mapping(
            "Buttons/Xbox/XboxSeriesX_Dpad_Right",
            "Buttons/PS5/PS5_Dpad_Right",
            "Buttons/Switch/Switch_Dpad_Right",
            "Buttons/KBM/Dark/A_Key_Dark"
        );

        public static Mapping A_KEY = new Mapping(
            "Buttons/Xbox/XboxSeriesX_A",
            "Buttons/PS5/PS5_Cross",
            "Buttons/Switch/Switch_A",
            "Buttons/KBM/Dark/Enter_Key_Dark"
        );

        public static Mapping B_KEY = new Mapping(
            "Buttons/Xbox/XboxSeriesX_B",
            "Buttons/PS5/PS5_Circle",
            "Buttons/Switch/Switch_B",
            "Buttons/KBM/Dark/Backspace_Alt_Key_Dark"
        );

        public static Mapping X_KEY = new Mapping(
            "Buttons/Xbox/XboxSeriesX_X",
            "Buttons/PS5/PS5_Square",
            "Buttons/Switch/Switch_X",
            "Buttons/KBM/Dark/X_Key_Dark"
        );

        public static Mapping Y_KEY = new Mapping(
            "Buttons/Xbox/XboxSeriesX_Y",
            "Buttons/PS5/PS5_Triangle",
            "Buttons/Switch/Switch_Y",
            "Buttons/KBM/Dark/Space_Key_Dark"
        );

        public static Mapping LEFT_TRIGGER = new Mapping(
            "Buttons/Xbox/XboxSeriesX_LT",
            "Buttons/PS5/PS5_R2",
            "Buttons/Switch/Switch_LT",
            "Buttons/KBM/Dark/Mouse_Left_Key_Dark"
        );

        public static Mapping RIGHT_TRIGGER = new Mapping(
            "Buttons/Xbox/XboxSeriesX_RT",
            "Buttons/PS5/PS5_L2",
            "Buttons/Switch/Switch_RT",
            "Buttons/KBM/Dark/Mouse_Right_Key_Dark"
        );
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
