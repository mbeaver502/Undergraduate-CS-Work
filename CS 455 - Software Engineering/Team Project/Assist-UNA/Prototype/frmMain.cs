﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Prototype
{

    public partial class frmMain : Form
    {
        private PrintDocument printDocument1 = new PrintDocument();
        private string stringToPrint;

        const int MAINFORM_WIDTH = 615;
        const int MAINFORM_MAX_WIDTH = 935;

        const int MAINFORM_HEIGHT = 485;
        const int MAINFORM_MAX_HEIGHT = 648;
                                                                                                                       
        public frmMain()
        {
            InitializeComponent();
            this.Size = new Size(MAINFORM_WIDTH, this.Height);
            this.Location = new Point(this.Location.X + ((MAINFORM_MAX_WIDTH - MAINFORM_WIDTH) / 2), this.Location.Y);
            //gbRegisters.Visible = false;

            this.Size = new Size(this.Width, MAINFORM_HEIGHT);
            this.Location = new Point(this.Location.X, this.Location.Y + ((MAINFORM_MAX_HEIGHT - MAINFORM_HEIGHT) / 2));
            lvMemory.Visible = false;

            printDocument1.PrintPage +=
                 new PrintPageEventHandler(printDocument1_PrintPage);
        }

        private void menuHelpAbout_Click(object sender, EventArgs e)
        {
            frmAbout ab = new frmAbout();
            ab.ShowDialog();
        }

        private void menuToolsOptions_Click(object sender, EventArgs e)
        {
            frmOptions opt = new frmOptions();
            opt.ShowDialog();
        }

        private void menuFileNew_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "You've made a new project!", "Create a New Project", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tsToolbarNew_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "You've made a new project!", "Create a New Project", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void menuFileOpen_Click(object sender, EventArgs e)
        {
            dlgOpen.ShowDialog();
        }

        private void tsToolbarOpen_Click(object sender, EventArgs e)
        {
            dlgOpen.ShowDialog();
        }

        private void menuFileSave_Click(object sender, EventArgs e)
        {
            dlgSave.ShowDialog();
        }

        private void menuFileSaveAs_Click(object sender, EventArgs e)
        {
            dlgSave.ShowDialog();
        }

        private void tsToolbarSave_ButtonClick(object sender, EventArgs e)
        {
            dlgSave.ShowDialog();
        }

        private void tsToolbarSaveSave_Click(object sender, EventArgs e)
        {
            dlgSave.ShowDialog();
        }

        private void tsToolbarSaveSaveAs_Click(object sender, EventArgs e)
        {
            dlgSave.ShowDialog();
        }

        private void menuFileImport_Click(object sender, EventArgs e)
        {
            dlgImport.ShowDialog();
        }

        private void menuFileExport_Click(object sender, EventArgs e)
        {
            dlgExport.ShowDialog();
        }

        private void menuAssembleAssemble_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "You've assembled your code!", "Assembled Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void menuAssembleAssembleDebug_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "You've assembled your code and are debugging it!", "Assembled Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void menuAssembleAssembleFinalRun_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "You've assembled your code and are doing a final run!", "Assembled Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tsToolbarAssemble_ButtonClick(object sender, EventArgs e)
        {
            MessageBox.Show(this, "You've assembled your code!", "Assembled Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tsToolbarAssembleAssembleDebug_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "You've assembled your code and are debugging it!", "Assembled Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tsToolbarAssembleAssembleFinalRun_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "You've assembled your code and are doing a final run!", "Assembled Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tsToolbarViewPRT_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(this, "You are now viewing your .PRT file!", "Viewing .PRT File", MessageBoxButtons.OK, MessageBoxIcon.Information);
            frmViewPRT viewPrt = new frmViewPRT();
            viewPrt.Show();
        }

        private void menuToolsViewPRT_Click(object sender, EventArgs e)
        {
            frmViewPRT viewPrt = new frmViewPRT();
            viewPrt.Show();
        }

        private void menuHelpOnlineHelp_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.una.edu");
        }

        private void menuFileExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSource_TextChanged(object sender, EventArgs e)
        {

            int lineStart = 0;
            int lineLength = 0;
            char firstChar;
            int lastCursorPos = txtSource.SelectionStart;

            // Loop through all the lines
            for (int i = 0;  i < txtSource.Lines.Count(); i++)
            {

                lineStart = txtSource.GetFirstCharIndexFromLine(i);
                lineLength = txtSource.Lines[i].Length;
                
                if (lineLength > 0) {

                    firstChar = txtSource.Lines[i][0];

                    // Set color of comment lines
                    if (firstChar == '*')
                    {

                        // Entire line
                        txtSource.Select(lineStart, lineLength); 
                        txtSource.SelectionColor = Color.Green;
                        
                    }

                }

                txtSource.DeselectAll();

            }

            txtSource.Select(lastCursorPos, lastCursorPos);
            txtSource.DeselectAll();

        }

        private void menuToolsRegisters_CheckedChanged(object sender, EventArgs e)
        {
            if (menuToolsRegisters.Checked == true)
            {

                this.Size = new Size(MAINFORM_MAX_WIDTH, this.Height);
                this.Location = new Point(this.Location.X - ((MAINFORM_MAX_WIDTH - MAINFORM_WIDTH) / 2), this.Location.Y);
                //gbRegisters.Visible = true;

            }

            else
            {

                this.Size = new Size(MAINFORM_WIDTH, this.Height);
                this.Location = new Point(this.Location.X + ((MAINFORM_MAX_WIDTH - MAINFORM_WIDTH) / 2), this.Location.Y);
                //gbRegisters.Visible = false;

            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

            lvMemory.Items.Add("00000000", 0);
            lvMemory.Items[0].SubItems.Add("F810F010F82F00141A12501F001807FE");
            lvMemory.Items[0].SubItems.Add("..0.......&.....");

            lvMemory.Items.Add("00000010", 1);
            lvMemory.Items[1].SubItems.Add("0000000400000006F5F5F5F5F5F5F5F5");
            lvMemory.Items[1].SubItems.Add("........55555555");

            lvMemory.Items.Add("00000020", 2);
            lvMemory.Items[2].SubItems.Add("F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5");
            lvMemory.Items[2].SubItems.Add("5555555555555555");

            lvMemory.Items.Add("00000030", 3);
            lvMemory.Items[3].SubItems.Add("F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5");
            lvMemory.Items[3].SubItems.Add("5555555555555555");

            lvMemory.Items.Add("00000040", 4);
            lvMemory.Items[4].SubItems.Add("F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5");
            lvMemory.Items[4].SubItems.Add("5555555555555555");

            lvMemory.Items.Add("00000050", 5);
            lvMemory.Items[5].SubItems.Add("F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5");
            lvMemory.Items[5].SubItems.Add("5555555555555555");

            lvMemory.Items.Add("00000060", 6);
            lvMemory.Items[6].SubItems.Add("F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5");
            lvMemory.Items[6].SubItems.Add("5555555555555555");

            lvMemory.Items.Add("00000070", 7);
            lvMemory.Items[7].SubItems.Add("F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5");
            lvMemory.Items[7].SubItems.Add("5555555555555555");

            lvMemory.Items.Add("00000080", 8);
            lvMemory.Items[8].SubItems.Add("F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5");
            lvMemory.Items[8].SubItems.Add("5555555555555555");

            lvMemory.Items.Add("00000090", 9);
            lvMemory.Items[9].SubItems.Add("F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5");
            lvMemory.Items[9].SubItems.Add("5555555555555555");

            lvMemory.Items.Add("000000A0", 10);
            lvMemory.Items[10].SubItems.Add("F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5");
            lvMemory.Items[10].SubItems.Add("5555555555555555");

            lvMemory.Items.Add("000000B0", 11);
            lvMemory.Items[11].SubItems.Add("F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5");
            lvMemory.Items[11].SubItems.Add("5555555555555555");

            lvMemory.Items.Add("000000C0", 12);
            lvMemory.Items[12].SubItems.Add("F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5");
            lvMemory.Items[12].SubItems.Add("5555555555555555");

            lvMemory.Items.Add("000000D0", 13);
            lvMemory.Items[13].SubItems.Add("F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5");
            lvMemory.Items[13].SubItems.Add("5555555555555555");

            lvMemory.Items.Add("000000E0", 14);
            lvMemory.Items[14].SubItems.Add("F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5");
            lvMemory.Items[14].SubItems.Add("5555555555555555");

            lvMemory.Items.Add("000000F0", 15);
            lvMemory.Items[15].SubItems.Add("F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5");
            lvMemory.Items[15].SubItems.Add("5555555555555555");

            lvMemory.Items.Add("00000100", 16);
            lvMemory.Items[16].SubItems.Add("F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5");
            lvMemory.Items[16].SubItems.Add("5555555555555555");
            
        }

        private void menuToolsMemory_CheckedChanged(object sender, EventArgs e)
        {
            if (menuToolsMemory.Checked == true)
            {

                this.Size = new Size(this.Width, MAINFORM_MAX_HEIGHT);
                this.Location = new Point(this.Location.X, this.Location.Y - ((MAINFORM_MAX_HEIGHT - MAINFORM_HEIGHT) / 2));
                lvMemory.Visible = true;

            }

            else
            {

                this.Size = new Size(this.Width, MAINFORM_HEIGHT);
                this.Location = new Point(this.Location.X, this.Location.Y + ((MAINFORM_MAX_HEIGHT - MAINFORM_HEIGHT) / 2));
                lvMemory.Visible = false;

            }
        }

        private void tsToolbarPrint_Click(object sender, EventArgs e)
        {
            ReadFile();

            PrintDialog pdi = new PrintDialog();

            if (pdi.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void ReadFile()
        {
            string docPath = @"c:\temp\testPage.txt";

            printDocument1.DocumentName = docPath;

            using (StreamWriter writer = File.CreateText(docPath))
            {
                foreach (string line in txtSource.Lines)
                    writer.WriteLine(line);
            }

            try
            {
                using (FileStream stream = new FileStream(docPath, FileMode.Open))
                using (StreamReader reader = new StreamReader(stream))
                {
                    stringToPrint = reader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("poop");
            }
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            int charactersOnPage = 0;
            int linesPerPage = 0;

            // Sets the value of charactersOnPage to the number of characters  
            // of printToString that will fit within the bounds of the page.
            e.Graphics.MeasureString(stringToPrint, this.Font,
                e.MarginBounds.Size, StringFormat.GenericTypographic,
                out charactersOnPage, out linesPerPage);

            // Draws the string within the bounds of the page
            e.Graphics.DrawString(stringToPrint, txtSource.Font, Brushes.Black,
                e.MarginBounds, StringFormat.GenericDefault);

            // Remove the portion of the string that has been printed.
            try
            {
                stringToPrint = stringToPrint.Substring(charactersOnPage);
                e.HasMorePages = (stringToPrint.Length > 0);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("balls");
            }
            // Check to see if more pages are to be printed.

        }

        private void txtRegisterPSW_TextChanged(object sender, EventArgs e)
        {

        }

        private void menuToolsOutput_Click(object sender, EventArgs e)
        {
            frmOutput output = new frmOutput();
            output.Show();
        }


      
    }
}
