using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security;
using System.Threading.Tasks;

namespace FortuneStreetCharacterCreator
{
    public partial class Form1 : Form
    {
        String BRRESstring;
        byte[] BRRES;
        byte[] MDL0; //"MDL0"
        byte[] MDL0Name = new byte[32];
        int MDL0NameLocation;

        byte[] CHR0; //"CHR0"

        List<String> CHR0Names = new List<String>();
        List<int> CHR0NameLocations = new List<int>();
        int NumCHR0 = 0;
        int LastCHR0 = 0;

        byte[] nullByte = new byte[1];

        String Chosen = "NULL";
        String Alena = "ch_dq_aln";
        String Bianca = "ch_dq_bnk";
        String Carver = "ch_dq_hsn";
        String Angelo = "ch_dq_kkr";
        String Kirryl = "ch_dq_krf";
        String Platypunk = "ch_dq_mmj";
        String Princessa = "ch_dq_pdn";
        String Patty = "ch_dq_red";
        String Dragonlord = "ch_dq_ruo";
        String Slime = "ch_dq_slm";
        String Stella = "ch_dq_snd";
        String Yangus = "ch_dq_ygs";
        String Jessica = "ch_dq_zsc";
        String Healslime = "ch_en_hsm";
        String Lakitu = "ch_en_jgm";
        String Moneybag = "ch_en_odl";
        String Bowser = "ch_nt_cpa";
        String Bowserjr = "ch_nt_cpj";
        String Birdo = "ch_nt_ctr";
        String Diddy = "ch_nt_ddk";
        String Donkey = "ch_nt_dkk";
        String Daisy = "ch_nt_dzy";
        String Toad = "ch_nt_knp";
        String Luigi = "ch_nt_lig";
        String Mario = "ch_nt_mro";
        String Peach = "ch_nt_pch";
        String Waluigi = "ch_nt_wlg";
        String Wario = "ch_nt_wro";
        String Yoshi = "ch_nt_yss";



        public Form1()
        {
            InitializeComponent();
            setup();
        }

        public void setup()
        {
            SelectCharacterSlot();
            MDL0 = Encoding.ASCII.GetBytes("MDL0");
            CHR0 = Encoding.ASCII.GetBytes("CHR0");
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        static int SearchBytes(byte[] haystack, int StartingIndex, byte[] needle , int increment)
        {
            var len = needle.Length;
            var limit = haystack.Length - len;
            for (var i = StartingIndex; i <= limit; i += increment)
            {
                var k = 0;
                for (; k < len; k++)
                {
                    if (needle[k] != haystack[i + k]) break;
                }
                if (k == len) return i;
            }
            return -1;
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectCharacterSlot();
        }

            void SelectCharacterSlot()
        {
            if (CharacterSlotDropDown.Text == "Alena") Chosen = Alena;
            if (CharacterSlotDropDown.Text == "Bianca") Chosen = Bianca;
            if (CharacterSlotDropDown.Text == "Carver") Chosen = Carver;
            if (CharacterSlotDropDown.Text == "Angelo") Chosen = Angelo;
            if (CharacterSlotDropDown.Text == "Kirryl") Chosen = Kirryl;
            if (CharacterSlotDropDown.Text == "Platypunk") Chosen = Platypunk;
            if (CharacterSlotDropDown.Text == "Princessa") Chosen = Princessa;
            if (CharacterSlotDropDown.Text == "Patty") Chosen = Patty;
            if (CharacterSlotDropDown.Text == "Dragonlord") Chosen = Dragonlord;
            if (CharacterSlotDropDown.Text == "Slime") Chosen = Slime;
            if (CharacterSlotDropDown.Text == "Stella") Chosen = Stella;
            if (CharacterSlotDropDown.Text == "Yangus") Chosen = Yangus;
            if (CharacterSlotDropDown.Text == "Jessica") Chosen = Jessica;
            if (CharacterSlotDropDown.Text == "Healslime") Chosen = Healslime;
            if (CharacterSlotDropDown.Text == "Lakitu") Chosen = Lakitu;
            if (CharacterSlotDropDown.Text == "Moneybag") Chosen = Moneybag;
            if (CharacterSlotDropDown.Text == "Bowser") Chosen = Bowser;
            if (CharacterSlotDropDown.Text == "Bowser Jr.") Chosen = Bowserjr;
            if (CharacterSlotDropDown.Text == "Birdo") Chosen = Birdo;
            if (CharacterSlotDropDown.Text == "Diddy Kong") Chosen = Diddy;
            if (CharacterSlotDropDown.Text == "Donkey Kong") Chosen = Donkey;
            if (CharacterSlotDropDown.Text == "Daisy") Chosen = Daisy;
            if (CharacterSlotDropDown.Text == "Toad") Chosen = Toad;
            if (CharacterSlotDropDown.Text == "Luigi") Chosen = Luigi;
            if (CharacterSlotDropDown.Text == "Mario") Chosen = Mario;
            if (CharacterSlotDropDown.Text == "Peach") Chosen = Peach;
            if (CharacterSlotDropDown.Text == "Waluigi") Chosen = Waluigi;
            if (CharacterSlotDropDown.Text == "Wario") Chosen = Wario;
            if (CharacterSlotDropDown.Text == "Yoshi") Chosen = Yoshi;
            label1.Text = Chosen;
        }

        void FindMDL0()
        {
            int MDL0Index = SearchBytes(BRRES, 0, MDL0, 4);
            textBox1.Text += "\r\n" + Encoding.ASCII.GetString(BRRES, MDL0Index, 4);
            byte[] MDL0NumSections = new byte[4];
            Array.Copy(BRRES, MDL0Index + 8, MDL0NumSections, 0, 4);
            Array.Reverse(MDL0NumSections);
            textBox1.Text += "\r\n" + BitConverter.ToInt32(MDL0NumSections, 0).ToString();
            int sections = BitConverter.ToInt32(MDL0NumSections, 0);
            int MDL0NameLocationOffset = 0;
            if (sections == 11) MDL0NameLocationOffset = (BitConverter.ToInt32(MDL0NumSections, 0) + 7) * 4 + MDL0Index;
            else MDL0NameLocationOffset = (BitConverter.ToInt32(MDL0NumSections, 0) + 6) * 4 + MDL0Index;
            byte[] MDL0NameOffset = new byte[4];
            Array.Copy(BRRES, MDL0NameLocationOffset, MDL0NameOffset, 0, 4);
            Array.Reverse(MDL0NameOffset);
            MDL0NameLocation = BitConverter.ToInt32(MDL0NameOffset, 0) + MDL0Index;
            textBox1.Text += "\r\n" +  MDL0NameLocation.ToString();         
            Array.Copy(BRRES, MDL0NameLocation, MDL0Name, 0, 32);
            textBox1.Text += "\r\n" +  Encoding.ASCII.GetString(MDL0Name, 0, MDL0Name.Length);
        }

        void RenameMDL0()
        {
            int MDL0NameLength = SearchBytes(MDL0Name, 0, nullByte, 1);
            byte[] chosenBytes = Encoding.ASCII.GetBytes(Chosen);
            Array.Copy(chosenBytes, 0, BRRES, MDL0NameLocation, Chosen.Length);
            if (Chosen.Length < MDL0NameLength)
            {
                Array.Copy(nullByte, 0, BRRES, MDL0NameLocation + Chosen.Length, 1);
            }
        }

        int FindCHR0(int Start)
        {
            byte[] CHR0Name = new byte[32];

            int CHR0Index = SearchBytes(BRRES, Start, CHR0, 4);
            if (CHR0Index == -1) return -1;
            textBox1.Text += Encoding.ASCII.GetString(BRRES, CHR0Index, 4);
            byte[] CHR0NumSections = new byte[4];
            Array.Copy(BRRES, CHR0Index + 8, CHR0NumSections, 0, 4);
            Array.Reverse(CHR0NumSections);
            textBox1.Text += BitConverter.ToInt32(CHR0NumSections, 0).ToString();
            int CHR0NameLocationOffset = BitConverter.ToInt32(CHR0NumSections, 0) * 4 + CHR0Index + 4;
            byte[] CHR0NameOffset = new byte[4];
            Array.Copy(BRRES, CHR0NameLocationOffset, CHR0NameOffset, 0, 4);
            Array.Reverse(CHR0NameOffset);
            int CHR0NameLocation = BitConverter.ToInt32(CHR0NameOffset, 0) + CHR0Index;
            textBox1.Text += CHR0NameLocation.ToString();
            Array.Copy(BRRES, CHR0NameLocation, CHR0Name, 0, 32);
            textBox1.Text += Encoding.ASCII.GetString(CHR0Name, 0, CHR0Name.Length);

            CHR0Names.Add(Encoding.ASCII.GetString(CHR0Name, 0, CHR0Name.Length));
            CHR0NameLocations.Add(CHR0NameLocation);
            NumCHR0++;

            return CHR0Index;
        }

        void RenameAllCHR0()
        {
            RenameCHR0("_buys", buys.SelectedIndex);
            RenameCHR0("_cannon_01", cannon01.SelectedIndex);
            RenameCHR0("_cannon_02", cannon02.SelectedIndex);
            RenameCHR0("_dice_e", dicee.SelectedIndex);
            RenameCHR0("_dice_m", dicem.SelectedIndex);
            RenameCHR0("_dice_s", dices.SelectedIndex);
            RenameCHR0("_goal", goal.SelectedIndex);
            RenameCHR0("_jump", jump.SelectedIndex);
            RenameCHR0("_landing", landing.SelectedIndex);
            RenameCHR0("_levelup", levelup.SelectedIndex);
            RenameCHR0("_lowrank", lowrank.SelectedIndex);
            RenameCHR0("_lowrank_s", lowranks.SelectedIndex);
            RenameCHR0("_payment_01", payment01.SelectedIndex);

            RenameCHR0("_serif_01", serif01.SelectedIndex);
            RenameCHR0("_serif_02", serif02.SelectedIndex);
            RenameCHR0("_serif_03", serif03.SelectedIndex);
            RenameCHR0("_serif_04", serif04.SelectedIndex);
            RenameCHR0("_serif_05", serif05.SelectedIndex);
            RenameCHR0("_serif_06", serif06.SelectedIndex);
            RenameCHR0("_stand_01", stand01.SelectedIndex);
            RenameCHR0("_stand_02", stand02.SelectedIndex);
            RenameCHR0("_walk_left", walkleft.SelectedIndex);
            RenameCHR0("_walk_right", walkright.SelectedIndex);
        }

        void RenameCHR0(String FSAnim, int replaceThis)
        {
            String Animation = Chosen + FSAnim;
            String replaceString = CHR0Names[replaceThis];
            int replaceLocation = CHR0NameLocations[replaceThis];
            byte[] replaceStringByte = Encoding.ASCII.GetBytes(replaceString);


            int CHR0NameLength = SearchBytes(replaceStringByte, 0, nullByte, 1);
            byte[] animationBytes = Encoding.ASCII.GetBytes(Animation);
            Array.Copy(animationBytes, 0, BRRES, replaceLocation, Animation.Length);
            if (Animation.Length < CHR0NameLength)
            {
                Array.Copy(nullByte, 0, BRRES, replaceLocation + Animation.Length, 1);
            }
        }

        private void Label3_Click(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "BRRES File|*.brres";
            openFileDialog1.Title = "Open BRRES File";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    BRRES = File.ReadAllBytes(openFileDialog1.FileName);
                    BRRESstring = Encoding.ASCII.GetString(BRRES);
                    textBox1.Text += BRRESstring;

                    FindMDL0();
                    int i = 0;
                    while (LastCHR0 != -1)
                    {
                        LastCHR0 = FindCHR0(LastCHR0 + 16);
                        i++;
                        this.Text = "searching for animations " + i.ToString();
                        if (i == 1000) break;

                    }
                    this.Text = "Found " + CHR0Names.Count.ToString() + "animations";
                    setDropDowns();
                    saveButton.Enabled = true;


                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            RenameMDL0();
            RenameAllCHR0();


            saveFileDialog1.Filter = "BRRES File|*.brres";
            saveFileDialog1.Title = "Save NEW BRRES File";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                //foreach( char c in BRRES )
                //{
                //    fs.Write()
                //}
                fs.Write(BRRES, 0, BRRES.Length);


                fs.Close();
                
            }
        }

        private void setDropDowns()
        {
            buys.Items.AddRange(CHR0Names.ToArray());
            cannon01.Items.AddRange(CHR0Names.ToArray());
            cannon02.Items.AddRange(CHR0Names.ToArray());
            dicee.Items.AddRange(CHR0Names.ToArray());
            dicem.Items.AddRange(CHR0Names.ToArray());
            dices.Items.AddRange(CHR0Names.ToArray());
            goal.Items.AddRange(CHR0Names.ToArray());
            jump.Items.AddRange(CHR0Names.ToArray());
            landing.Items.AddRange(CHR0Names.ToArray());
            levelup.Items.AddRange(CHR0Names.ToArray());
            lowrank.Items.AddRange(CHR0Names.ToArray());
            lowranks.Items.AddRange(CHR0Names.ToArray());
            payment01.Items.AddRange(CHR0Names.ToArray());

            serif01.Items.AddRange(CHR0Names.ToArray());
            serif02.Items.AddRange(CHR0Names.ToArray());
            serif03.Items.AddRange(CHR0Names.ToArray());
            serif04.Items.AddRange(CHR0Names.ToArray());
            serif05.Items.AddRange(CHR0Names.ToArray());
            serif06.Items.AddRange(CHR0Names.ToArray());
            stand01.Items.AddRange(CHR0Names.ToArray());
            stand02.Items.AddRange(CHR0Names.ToArray());
            walkleft.Items.AddRange(CHR0Names.ToArray());
            walkright.Items.AddRange(CHR0Names.ToArray());

            int nudge = 0;
            if (CHR0Names.Count >= 45) nudge = 0;

            buys.SelectedIndex = 0 + nudge;
            cannon01.SelectedIndex = 1 + nudge;
            cannon02.SelectedIndex = 2 + nudge;
            dicee.SelectedIndex = 3 + nudge;
            dicem.SelectedIndex = 4 + nudge;
            dices.SelectedIndex = 5 + nudge;
            goal.SelectedIndex = 6 + nudge;
            jump.SelectedIndex = 7 + nudge;
            landing.SelectedIndex = 8 + nudge;
            levelup.SelectedIndex = 9 + nudge;
            lowrank.SelectedIndex = 10 + nudge;
            lowranks.SelectedIndex = 11 + nudge;
            payment01.SelectedIndex = 12 + nudge;

            serif01.SelectedIndex = 13 + nudge;
            serif02.SelectedIndex = 14 + nudge;
            serif03.SelectedIndex = 15 + nudge;
            serif04.SelectedIndex = 16 + nudge;
            serif05.SelectedIndex = 17 + nudge;
            serif06.SelectedIndex = 18 + nudge;
            stand01.SelectedIndex = 19 + nudge;
            stand02.SelectedIndex = 20 + nudge;
            walkleft.SelectedIndex = 21 + nudge;
            walkright.SelectedIndex = 22 + nudge;


        }
    }
}
