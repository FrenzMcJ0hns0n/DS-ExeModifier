using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace DS_ExeModifier {

    static class K32_LoadLib {
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);
    }

    public partial class MainWindow : Window {
        public const string darkSoulsExe = "DARKSOULS.exe";
        public const string darkSoulsBakExe = "DARKSOULS.exe.bak";
        public string exeType;
        FileStream fsWriter;

        public string ReturnExeType() {
            byte[] bytes;

            using (BinaryReader br = new BinaryReader(File.OpenRead(darkSoulsExe))) {
                br.BaseStream.Position = 0x80; // 128
                bytes = br.ReadBytes(2);
                exeType = Encoding.Default.GetString(bytes);
            }

            switch (exeType) {
                case "T6":
                    return "release";
                case "´4":
                    return "debug";
                default:
                    return "error";
            }
        }

        public void EditExe(int dvdbnd, byte[] unicodeExpression) {

            if (exeType == "T6") // RELEASE VERSION
            {
                List<int> bnd0_r = new List<int>() { 0xD65EA4, 0xD660F8, 0xD66180, 0xD6627C, 0xD662C8, 0xD66318, 0xD66C90 };
                List<int> bnd1_r = new List<int>() { 0xD57F14, 0xD65DAC, 0xD65DF4, 0xD65FFC, 0xD6613C, 0xD6636C, 0xD66484, 0xD665F0, 0xD666E4 };
                List<int> bnd2_r = new List<int>() { 0xD66734, 0xD667C4, 0xD66EAC };
                List<int> bnd3_r = new List<int>() { 0xD65E34, 0xD663B0, 0xD66434 };

                if (dvdbnd == 0)
                {
                    foreach (int position in bnd0_r)
                    {
                        fsWriter.Position = position;
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                    }
                }
                else if (dvdbnd == 1)
                {
                    foreach (int position in bnd1_r)
                    {
                        fsWriter.Position = position;
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                    }
                }
                else if (dvdbnd == 2)
                {
                    foreach (int position in bnd2_r)
                    {
                        fsWriter.Position = position;
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                    }
                }
                else if (dvdbnd == 3)
                {
                    foreach (int position in bnd3_r)
                    {
                        fsWriter.Position = position;
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                    }
                }
            }
                    
            else if (exeType == "´4") // DEBUG VERSION
            {
                List<int> bnd0_d = new List<int>() { 0xD6816C, 0xD683C0, 0xD68448, 0xD68544, 0xD68590, 0xD685E0, 0xD68F58 };
                List<int> bnd1_d = new List<int>() { 0xD5C2D4, 0xD68074, 0xD680BC, 0xD682C4, 0xD68404, 0xD68634, 0xD6874C, 0xD688B8, 0xD689AC };
                List<int> bnd2_d = new List<int>() { 0xD689FC, 0xD68A8C, 0xD69174 };
                List<int> bnd3_d = new List<int>() { 0xD680FC, 0xD68678, 0xD686FC };
                if (dvdbnd == 0)
                {
                    foreach (int position in bnd0_d)
                    {
                        fsWriter.Position = position;
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                    }
                }
                else if (dvdbnd == 1)
                {
                    foreach (int position in bnd1_d)
                    {
                        fsWriter.Position = position;
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                    }
                }
                else if (dvdbnd == 2)
                {
                    foreach (int position in bnd2_d)
                    {
                        fsWriter.Position = position;
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                    }
                }
                else if (dvdbnd == 3)
                {
                    foreach (int position in bnd3_d)
                    {
                        fsWriter.Position = position;
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                    }
                }
            }    
            
        }

        public MainWindow() {
            // Preload d3d9.dll directly from the system directory to avoid any modified d3d9.dll files in the local directory (such as PvP Watchdog)
            IntPtr d3d9_preload = K32_LoadLib.LoadLibrary(Environment.SystemDirectory + @"\d3d9.dll");
            IntPtr dxgi_preload = K32_LoadLib.LoadLibrary(Environment.SystemDirectory + @"\dxgi.dll");
            InitializeComponent();

            if (!(File.Exists(darkSoulsExe))) {
                MessageBox.Show("Error : No EXE found !\n\nMake sure this program is in the same folder as \"" + darkSoulsExe + "\".");
                Environment.Exit(0);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            switch (ReturnExeType()) {
                case "release":
                    textBox_targetExe.Text = "Target EXE type : Release";
                    textBox_targetExe.Foreground = new SolidColorBrush(Colors.Green);
                    break;
                case "debug":
                    textBox_targetExe.Text = "Target EXE type : Debug";
                    textBox_targetExe.Foreground = new SolidColorBrush(Colors.Orange);
                    break;
                case "error":
                    textBox_targetExe.Text = "Target EXE type : Error";
                    textBox_targetExe.Foreground = new SolidColorBrush(Colors.Red);
                    button_Apply.IsEnabled = false;
                    radioButton_dvdbnd0fromArchive.IsEnabled = false; radioButton_dvdbnd0fromFolders.IsEnabled = false;
                    radioButton_dvdbnd1fromArchive.IsEnabled = false; radioButton_dvdbnd1fromFolders.IsEnabled = false;
                    radioButton_dvdbnd2fromArchive.IsEnabled = false; radioButton_dvdbnd2fromFolders.IsEnabled = false;
                    radioButton_dvdbnd3fromArchive.IsEnabled = false; radioButton_dvdbnd3fromFolders.IsEnabled = false;
                    MessageBox.Show("Error : Wrong EXE file.\n\nIt must be the fully patched Steam version of Dark Souls (release) or the debug build.");
                    break;
            }
        }

        private void button_Apply_Click(object sender, RoutedEventArgs e) {
            byte[] byt;

            if (radioButton_dvdbnd0fromArchive.IsChecked == false && radioButton_dvdbnd0fromFolders.IsChecked == false && 
                radioButton_dvdbnd1fromArchive.IsChecked == false && radioButton_dvdbnd1fromFolders.IsChecked == false &&
                radioButton_dvdbnd2fromArchive.IsChecked == false && radioButton_dvdbnd2fromFolders.IsChecked == false &&
                radioButton_dvdbnd3fromArchive.IsChecked == false && radioButton_dvdbnd3fromFolders.IsChecked == false) {
                MessageBox.Show("Please select at least one option.");
            } 
            else {
                if (File.Exists(darkSoulsBakExe)) {
                    File.Delete(darkSoulsBakExe);
                }
                File.Copy(darkSoulsExe, darkSoulsBakExe);

                try {

                    fsWriter = File.OpenWrite(darkSoulsExe); 

                    if (radioButton_dvdbnd0fromArchive.IsChecked == true) {
                        byt = Encoding.Unicode.GetBytes("dvdbnd0:");
                        EditExe(0, byt);
                    }
                    else if (radioButton_dvdbnd0fromFolders.IsChecked == true) {
                        byt = Encoding.Unicode.GetBytes("dvdroot:");
                        EditExe(0, byt);
                    }

                    if (radioButton_dvdbnd1fromArchive.IsChecked == true) {
                        byt = Encoding.Unicode.GetBytes("dvdbnd1:");
                        EditExe(1, byt);
                    }
                    else if (radioButton_dvdbnd1fromFolders.IsChecked == true) {
                        byt = Encoding.Unicode.GetBytes("dvdroot:");
                        EditExe(1, byt);
                    }

                    if (radioButton_dvdbnd2fromArchive.IsChecked == true) {
                        byt = Encoding.Unicode.GetBytes("dvdbnd2:");
                        EditExe(2, byt);
                    }
                    else if (radioButton_dvdbnd2fromFolders.IsChecked == true) {
                        byt = Encoding.Unicode.GetBytes("dvdroot:");
                        EditExe(2, byt);
                    }

                    if (radioButton_dvdbnd3fromArchive.IsChecked == true) {
                        byt = Encoding.Unicode.GetBytes("dvdbnd3:");
                        EditExe(3, byt);
                    }
                    else if (radioButton_dvdbnd3fromFolders.IsChecked == true) {
                        byt = Encoding.Unicode.GetBytes("dvdroot:");
                        EditExe(3, byt);
                    }

                    fsWriter.Close();

                }
                catch (Exception ex) {
                    MessageBox.Show(ex.ToString());
                    Environment.Exit(0);
                }

                MessageBox.Show("\"" + darkSoulsExe + "\" successfully modified !\n\nA backup of the original file has been created for safety :\n" + "\"" + darkSoulsBakExe + "\".");
                Environment.Exit(0);
            }
        }

        private void button_loadAllFromArchive_Click(object sender, RoutedEventArgs e) {
            radioButton_dvdbnd0fromArchive.IsChecked = true;
            radioButton_dvdbnd1fromArchive.IsChecked = true;
            radioButton_dvdbnd2fromArchive.IsChecked = true;
            radioButton_dvdbnd3fromArchive.IsChecked = true;
        }

        private void button_loadAllFromExtractedFolders_Click(object sender, RoutedEventArgs e) {
            radioButton_dvdbnd0fromFolders.IsChecked = true;
            radioButton_dvdbnd1fromFolders.IsChecked = true;
            radioButton_dvdbnd2fromFolders.IsChecked = true;
            radioButton_dvdbnd3fromFolders.IsChecked = true;
        }
    }
}