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
 * 04/09/2014   THH     Fixed tab order. 
 * 04/15/2014   ACA     Fixed a PRT path issue.
 * 04/17/2014   ACA     Added key events method.
 *                      Added search support (ctrl+f).
 * 04/26/2014   ACA     Colors now change with main color scheme change
 * 04/27/2014   JMB     Fixed the methods for printing the PRT file.
 * 04/29/2014   ACA     Rare printing exception found, try/catch added.
 * 
 *********************************************************************************************/

namespace Assist_UNA
{
    public partial class ViewPRTForm : Form
    {
        /* Constants. */
        private const int CHARACTERS_PER_PAGE = 5355;

        /* Private members. */
        private List<string>.Enumerator pageEnumerator;
        private PrintDocument printPRT = new PrintDocument();
        private string pathPRT = "";
        private string pageToPrint = "";
        private string stringToPrint = "";
        private string totalPRTString = "";
        

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
         * Name:        SetBackColorMain
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       New color for the form's backcolor (Color type).
         * Return:      N/A
         * Description: This method will set the color type for the form's backcolor.
         *  
         *****************************************************************************************/
        public void SetBackColorMain(Color newColor)
        {
            this.BackColor = newColor;
        }


        /******************************************************************************************
         * 
         * Name:        SetButtonBackColor
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       New color for the button backcolors.
         * Return:      N/A
         * Description: This method will set the color type for the buttons' backcolor.
         *  
         *****************************************************************************************/
        public void SetButtonBackColor(Color newColor)
        {
            btnClose.BackColor = newColor;
            btnPrint.BackColor = newColor;
        }


        /******************************************************************************************
         * 
         * Name:        SetButtonTextColor
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       New color for the button forecolors.
         * Return:      N/A
         * Description: This method will set the color type for the buttons' text color.
         *  
         *****************************************************************************************/
        public void SetButtonTextColor(Color newColor)
        {
            btnClose.ForeColor = newColor;
            btnPrint.ForeColor = newColor;
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
        public void LoadPRT(string inputPath)
        {
            pathPRT = inputPath;
            StreamReader sr = new StreamReader(pathPRT);
            txtPRT.Text = sr.ReadToEnd();
            txtPRT.Text = txtPRT.Text.Replace("\f", Environment.NewLine);
            sr.Close();
        }


        /* Private methods. */

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
         *              Travis Hunt
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
         *              Travis Hunt    
         *              Michael Beaver
         * 
         * Input:       N/A
         * Return:      N/A
         * Description: This method will print the PRT contents in landscape.
         *  
         *****************************************************************************************/
        private void PrintPRT()
        {
            ReadPRT();
            
            string[] pages = totalPRTString.Split('\f');
            List<string> pageList = new List<string>(pages);
            PrintDialog printOptions = new PrintDialog();

            try
            {
                if (printOptions.ShowDialog() == DialogResult.OK)
                {
                    printPRT.PrinterSettings = printOptions.PrinterSettings;
                    printPRT.DefaultPageSettings.Landscape = true;

                    pageEnumerator = pageList.GetEnumerator();
                    pageEnumerator.MoveNext();
                    stringToPrint = pageEnumerator.Current;

                    printPRT.Print();
                }
            }

            catch
            {
                MessageBox.Show("Printing error. Check printing drivers.", "Printing error.");
            }
        }


        /******************************************************************************************
         * 
         * Name:        ReadPRT()
         * 
         * Author(s):   Clay Boren
         *              Drew Aaron
         *              Michael Beaver
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method writes the text from txtPRT to a temporary text file.
         *  
         *****************************************************************************************/
        private void ReadPRT()
        {
            printPRT.DocumentName = pathPRT;

            try
            {
                StreamReader reader = new StreamReader(pathPRT);
                totalPRTString = reader.ReadToEnd();
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
         *              Travis Hunt
         *              Michael Beaver
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will format the source code to a printable format.
         *  
         *****************************************************************************************/
        private void PrintDocumentOnPrintPage(object sender, PrintPageEventArgs e)
        {
            int charactersOnPage = 0;
            int linesPerPage = 0;
            
            /* Sets the value of charactersOnPage to the number of characters  
               of printToString that will fit within the bounds of the page. */
            e.Graphics.MeasureString(totalPRTString, txtPRT.Font,
                e.MarginBounds.Size, StringFormat.GenericDefault,
                out charactersOnPage, out linesPerPage);

            try
            {
                pageToPrint = stringToPrint;

                e.Graphics.DrawString(pageToPrint, txtPRT.Font, Brushes.Black, 
                    e.MarginBounds, StringFormat.GenericDefault);

                if (stringToPrint.Length > CHARACTERS_PER_PAGE)
                {
                    e.HasMorePages = true;
                    stringToPrint = stringToPrint.Substring(CHARACTERS_PER_PAGE + 1);
                }

                else if (pageEnumerator.MoveNext())
                {
                    e.HasMorePages = true;
                    stringToPrint = pageEnumerator.Current;
                }

                else
                    e.HasMorePages = false;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Print Error");
            }
        }


        /******************************************************************************************
         * 
         * Name:        TxtPRTKeyDown
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will perform actions depending on which key is pressed.
         *              Currently only used for opening a search form.
         *  
         *****************************************************************************************/
        private void TxtPRTKeyDown(object sender, KeyEventArgs e)
        {
            /* Open a search form if user presses ctrl + f. */
            if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.F)
            {
                FindReplaceForm findText = new FindReplaceForm(false, txtPRT);
                findText.Show();
            }
        }
    }
}