using System;
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

namespace Assist_UNA
{
    /**********************************************************************************************
     * 
     * Name: ViewPRTForm class
     * 
     * ============================================================================================
     * 
     * Description: This class is the view PRT form window.
     *                            
     * ============================================================================================         
     * 
     * Modification History
     * --------------------
     * 03/16/2014   ACA     Original version.
     * 03/17/2014   ACA     Added functionality.
     * 03/17/2014   THH     Made the start location center screen.
     * 03/29/2014   THH     Fixed error handling of opening PRT when moved or open with another
     *                      application with Drew.
     * 04/01/2014   CLB     Added printing functionality to the PRT.
     * 04/03/2014   CLB     Added printing functionality to the PRT.
     * 04/05/2014   CLB     Added printing functionality to the PRT.
     * 
     *********************************************************************************************/
    public partial class ViewPRTForm : Form
    {
        /* Private variables */
        private PrintDocument printPRT = new PrintDocument();
        private string stringToPrint;
        
        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        ViewPRTForm
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will initialize the view PRT form
         *  
         *****************************************************************************************/
        public ViewPRTForm()
        {
            InitializeComponent();

            printPRT.PrintPage += new PrintPageEventHandler(PrintDocumentOnPrintPage);
        }


        /******************************************************************************************
         * 
         * Name:        LoadPRT
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       pathPRT is a string containing the path to the .PRT file.
         * Return:      N/A
         * Description: This method will load the contents of the .PRT file into the text box.
         *  
         *****************************************************************************************/
        public void LoadPRT(string pathPRT)
        {
            StreamReader sr = new StreamReader(pathPRT);
            txtPRT.Text = sr.ReadToEnd();
            sr.Close();    
        }


        /******************************************************************************************
         * 
         * Name:        BtnCloseClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event
         * Return:      N/A
         * Description: This method will close the view PRT form.
         *  
         *****************************************************************************************/
        private void BtnCloseClick(object sender, EventArgs e)
        {
            this.Close();
        }


        /******************************************************************************************
         * 
         * Name:        BtnPrintClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event
         * Return:      N/A
         * Description: This method will print the PRT contents in landscape.
         *  
         *****************************************************************************************/
        private void BtnPrintClick(object sender, EventArgs e)
        {
            PrintPRT();
        }


        /******************************************************************************************
         * 
         * Name:        PrintPRT
         * 
         * Author(s):   Clay Boren
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will print the PRT contents in landscape.
         *  
         *****************************************************************************************/
        private void PrintPRT()
        {
            ReadPRT();

            PrintDialog printOptions = new PrintDialog();
            MainForm m = new MainForm();
            m.PrinterSettings(printOptions);

            if (printOptions.ShowDialog() == DialogResult.OK)
            {
                printPRT.DefaultPageSettings.Landscape = true;
                printPRT.Print();
            }
        }


        /******************************************************************************************
         * 
         * Name:        ReadPRT()
         * 
         * Author(s):   Clay Boren
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method writes the text from txtPRT to a temporary text file.
         *  
         *****************************************************************************************/
        private void ReadPRT()
        {
            string prtDocPath = @"c:\temp\prtPage.txt";

            printPRT.DocumentName = prtDocPath;

            using (StreamWriter writer = File.CreateText(prtDocPath))
            {
                foreach (string line in txtPRT.Lines)
                    writer.WriteLine(line);
            }

            try
            {
                using (FileStream stream = new FileStream(prtDocPath, FileMode.Open))
                using (StreamReader reader = new StreamReader(stream))
                {
                    stringToPrint = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /******************************************************************************************
         * 
         * Name:        PrintDocumentOnPrintPage
         * 
         * Author(s):   Clay Boren
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will write the PRT text to the Print Page in the correct
         *              formatting.
         *  
         *****************************************************************************************/
        private void PrintDocumentOnPrintPage(object sender, PrintPageEventArgs e)
        {
            int charactersOnPage = 0;
            int linesPerPage = 0;

            /* Sets the value of charactersOnPage to the number of characters  
               of printToString that will fit within the bounds of the page. */
            e.Graphics.MeasureString(stringToPrint, this.Font,
                e.MarginBounds.Size, StringFormat.GenericTypographic,
                out charactersOnPage, out linesPerPage);

            /* Draws the string within the bounds of the page */
            e.Graphics.DrawString(stringToPrint, txtPRT.Font, Brushes.Black,
                e.MarginBounds, StringFormat.GenericDefault);

            /* Remove the portion of the string that has been printed. */
            try
            {
                stringToPrint = stringToPrint.Substring(charactersOnPage);
                e.HasMorePages = (stringToPrint.Length > 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
    }
}
