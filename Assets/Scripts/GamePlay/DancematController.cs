using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DancematController : MonoBehaviour {

    public bool T, B, U, S, TL, TG, BL, BD;
    public string huruf;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        // default bernilai false
        T = B = U = S = TL = TG = BL = BD = false;

        // Input Manager : Type pada Horizontal dan Vertical harus "Joystick Axis"
        if (Input.GetAxis("Horizontal") > 0 && Input.GetAxis("Horizontal") < 1) T = B = true; // kanan dan kiri
        if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Vertical") < 1)     U = S = true; // atas dan bawah

        if (Input.GetAxis("Horizontal") == 1)  T = true; // kanan
        if (Input.GetAxis("Horizontal") == -1) B = true; // kiri
        if (Input.GetAxis("Vertical") == -1)   U = true; // atas
        if (Input.GetAxis("Vertical") == 1)    S = true; // bawah

        if (Input.GetKey(KeyCode.JoystickButton0)) BD = true; // kiri bawah
        if (Input.GetKey(KeyCode.JoystickButton1)) TL = true; // kanan atas
        if (Input.GetKey(KeyCode.JoystickButton2)) BL = true; // kiri atas
        if (Input.GetKey(KeyCode.JoystickButton3)) TG = true; // kanan bawah
        
        // pake keyboard dulu
        keyboardPlaceholderInput();

        // pengkodean huruf sesuai semaphore
        if (!transform.parent.GetComponent<GameStateManager>().isPaused)
        {
            huruf = "";
            if (!T && !B && !U && S && !TL && !TG && !BL && BD)
                huruf = "A";
            else if (!T && B && !U && S && !TL && !TG && !BL && !BD)
                huruf = "B";
            else if (!T && !B && !U && S && !TL && !TG && BL && !BD)
                huruf = "C";
            else if (!T && !B && U && S && !TL && !TG && !BL && !BD)
                huruf = "D";
            else if (!T && !B && !U && S && TL && !TG && !BL && !BD)
                huruf = "E";
            else if (T && !B && !U && S && !TL && !TG && !BL && !BD)
                huruf = "F";
            else if (!T && !B && !U && S && !TL && TG && !BL && !BD)
                huruf = "G";
            else if (!T && B && !U && !S && !TL && !TG && !BL && BD)
                huruf = "H";
            else if (!T && !B && !U && !S && !TL && !TG && BL && BD)
                huruf = "I";
            else if (T && !B && U && !S && !TL && !TG && !BL && !BD)
                huruf = "J";
            else if (!T && !B && U && !S && !TL && !TG && !BL && BD)
                huruf = "K";
            else if (!T && !B && !U && !S && TL && !TG && !BL && BD)
                huruf = "L";
            else if (T && !B && !U && !S && !TL && !TG && !BL && BD)
                huruf = "M";
            else if (!T && !B && !U && !S && !TL && TG && !BL && BD)
                huruf = "N";
            else if (!T && B && !U && !S && !TL && !TG && BL && !BD)
                huruf = "O";
            else if (!T && B && U && !S && !TL && !TG && !BL && !BD)
                huruf = "P";
            else if (!T && !B && !U && !S && TL && !TG && !BL && BD)
                huruf = "Q";
            else if (T && B && !U && !S && !TL && !TG && !BL && !BD)
                huruf = "R";
            else if (!T && B && !U && !S && !TL && TG && !BL && !BD)
                huruf = "S";
            else if (!T && !B && U && !S && !TL && !TG && BL && !BD)
                huruf = "T";
            else if (!T && !B && !U && !S && TL && !TG && BL && !BD)
                huruf = "U";
            else if (!T && !B && U && !S && !TL && TG && !BL && !BD)
                huruf = "V";
            else if (T && !B && !U && !S && TL && !TG && !BL && !BD)
                huruf = "W";
            else if (!T && !B && !U && !S && TL && TG && !BL && !BD)
                huruf = "X";
            else if (T && !B && !U && !S && !TL && !TG && BL && !BD)
                huruf = "Y";
            else if (T && !B && !U && !S && !TL && TG && !BL && !BD)
                huruf = "Z";
        }
    }
    
    private void keyboardPlaceholderInput()
    {
        if (GetComponent<AlphabetInputListener>().isControllerEnabled)
        {
            // timur
            if (Input.GetKey(KeyCode.D))
            {
                T = true;
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                T = false;
            }
            // barat
            if (Input.GetKey(KeyCode.A))
            {
                B = true;
            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                B = false;
            }
            // utara
            if (Input.GetKey(KeyCode.W))
            {
                U = true;
            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                U = false;
            }
            // selatan
            if (Input.GetKey(KeyCode.X))
            {
                S = true;
            }
            else if (Input.GetKeyUp(KeyCode.X))
            {
                S = false;
            }
            // barat daya
            if (Input.GetKey(KeyCode.Z))
            {
                BD = true;
            }
            else if (Input.GetKeyUp(KeyCode.Z))
            {
                BD = false;
            }
            // timur laut
            if (Input.GetKey(KeyCode.E))
            {
                TL = true;
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                TL = false;
            }
            // barat laut
            if (Input.GetKey(KeyCode.Q))
            {
                BL = true;
            }
            else if (Input.GetKeyUp(KeyCode.Q))
            {
                BL = false;
            }
            // tenggara
            if (Input.GetKey(KeyCode.C))
            {
                TG = true;
            }
            else if (Input.GetKeyUp(KeyCode.C))
            {
                TG = false;
            }
            // delete or backspace
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                //delete char
                if (GetComponent<Text>().text.Length > 0)
                {
                    GetComponent<Text>().text = GetComponent<Text>().text.Substring(0, GetComponent<Text>().text.Length - 1);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0)) // enter
            {
                transform.parent.FindChild("TextGenerator").GetComponent<TextGeneratorScript>().cek();
            }
        }
    }
}