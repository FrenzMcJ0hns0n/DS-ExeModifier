using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace DS_ExeModifier {

	static class K32_LoadLib{
            [System.Runtime.InteropServices.DllImport("kernel32.dll")]
            public static extern IntPtr LoadLibrary(string dllToLoad);
    }

    public partial class MainWindow : Window {
       
        public const string darkSoulsExe = "DARKSOULS.exe";
        public const string darkSoulsBakExe = "DARKSOULS.exe.bak";
        public string exeType;
        FileStream fsWriter;

        public string CheckDarkSoulsExe() {
            byte[] bytes;

            using (BinaryReader b = new BinaryReader(File.OpenRead(darkSoulsExe))) {
                b.BaseStream.Position = 0x80; // 128
                bytes = b.ReadBytes(2);
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
            switch (exeType) {

                // RELEASE
                case "T6":
                    if (dvdbnd == 0) {
                        fsWriter.Position = 0xD65EA4; // = 14048932
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                        fsWriter.Position = 0xD660F8; // = 14049528
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                        fsWriter.Position = 0xD66180; // = 14049664
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                        fsWriter.Position = 0xD6627C; // = 14049916
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                        fsWriter.Position = 0xD662C8; // = 14049992
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                        fsWriter.Position = 0xD66318; // = 14050072
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                        fsWriter.Position = 0xD66C90; // = 14052496
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                    }
                    else if (dvdbnd == 1) {
                        fsWriter.Position = 0xD57F14; // = 13991700
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                        fsWriter.Position = 0xD65DAC; // = 14048684
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                        fsWriter.Position = 0xD65DF4; // = 14048756
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                        fsWriter.Position = 0xD65FFC; // = 14049276
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                        fsWriter.Position = 0xD6613C; // = 14049596
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                        fsWriter.Position = 0xD6636C; // = 14050156
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                        fsWriter.Position = 0xD66484; // = 14050436
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                        fsWriter.Position = 0xD665F0; // = 14050800
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                        fsWriter.Position = 0xD666E4; // = 14051044
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                    }
                    break;

                // DEBUG
                case "´4":
                    if (dvdbnd == 0) {
                        fsWriter.Position = 0xD6816C; // = 14057836
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                        fsWriter.Position = 0xD683C0; // = 14058432
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                        fsWriter.Position = 0xD68448; // = 14058568
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                        fsWriter.Position = 0xD68544; // = 14058820
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                        fsWriter.Position = 0xD68590; // = 14058896
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                        fsWriter.Position = 0xD685E0; // = 14058976
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                        fsWriter.Position = 0xD68F58; // = 14061400
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                    }
                    else if (dvdbnd == 1) {
                        fsWriter.Position = 0xD5C2D4; // = 14009044
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                        fsWriter.Position = 0xD68074; // = 14057588
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                        fsWriter.Position = 0xD680BC; // = 14057660
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                        fsWriter.Position = 0xD682C4; // = 14058180
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                        fsWriter.Position = 0xD68404; // = 14058500
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                        fsWriter.Position = 0xD68634; // = 14059060
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                        fsWriter.Position = 0xD6874C; // = 14059340
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                        fsWriter.Position = 0xD688B8; // = 14059704
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                        fsWriter.Position = 0xD689AC; // = 14059948
                        fsWriter.Write(unicodeExpression, 0, unicodeExpression.Length);
                    }
                    break;
            }
        }

        public string MessageDoneModifyingEXE() {
            return "\""+ darkSoulsExe + "\" successfully modified !\n\nThe original file has been backup for safety :\n" + "\"" + darkSoulsBakExe + "\"";
        }

        public MainWindow() {
			// Load the preloader DLL that loads d3d9.dll from system directory early instead of from local directory when GUI is initialized
            IntPtr d3d9_preload = K32_LoadLib.LoadLibrary(@"C:\Windows\SysWOW64\d3d9.dll");
            InitializeComponent();
            if (!(File.Exists(darkSoulsExe))) {
                MessageBox.Show("Error : No EXE found !\n\nMake sure this program is in the same folder as \"" + darkSoulsExe + "\"");
                Environment.Exit(0);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            switch (CheckDarkSoulsExe()) {
                case "release":
                    textBox_targetExe.Text = "Target EXE type : Release";
                    textBox_targetExe.Foreground = new SolidColorBrush(Colors.Green);
                    break;
                case "debug":
                    textBox_targetExe.Text = "Target EXE type : Debug";
                    textBox_targetExe.Foreground = new SolidColorBrush(Colors.Orange);
                    break;
                default:
                    textBox_targetExe.Text = "Target EXE type : Error";
                    textBox_targetExe.Foreground = new SolidColorBrush(Colors.Red);
                    button_Apply.IsEnabled = false;
                    radioButton_dvdbnd0fromArchive.IsEnabled = false;
                    radioButton_dvdbnd0fromFolders.IsEnabled = false;
                    radioButton_dvdbnd1fromArchive.IsEnabled = false;
                    radioButton_dvdbnd1fromFolders.IsEnabled = false;
                    MessageBox.Show("Error : Wrong EXE file.\n\nIt must be the fully patched Steam version of Dark Souls (release) or the debug version.");
                    break;
            }
        }

        private void button_Apply_Click(object sender, RoutedEventArgs e) {

            byte[] byt;

            if (radioButton_dvdbnd0fromArchive.IsChecked == true 
             || radioButton_dvdbnd0fromFolders.IsChecked == true 
             || radioButton_dvdbnd1fromArchive.IsChecked == true 
             || radioButton_dvdbnd1fromFolders.IsChecked == true) {

                if (File.Exists(darkSoulsBakExe)) { File.Delete(darkSoulsBakExe); }
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

                    fsWriter.Close();
                }

                catch (Exception ex) {
                    MessageBox.Show(ex.ToString());
                    Environment.Exit(0);
                }

                MessageBox.Show(MessageDoneModifyingEXE());
                Environment.Exit(0);

            } 
            else { MessageBox.Show("Please select at least one option."); }
        }

    }
}