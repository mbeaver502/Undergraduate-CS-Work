using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/**************************************************************************************************
 * 
 * Name: OutputForm class
 * 
 * ================================================================================================
 * 
 * Description: This class is the output form part of the main GUI. 
 *                            
 * ================================================================================================
 * 
 * Modification History
 * --------------------
 * 03/09/2014   ACA     Original version.
 * 03/17/2014   THH     Made the start location center screen.
 * 03/28/2014   ACA     Added functionality.
 * 03/29/2014   ACA     Added next button and some methods for it.
 * 04/19/2014   JMB     Changed dimensions so the console box is only 15 lines in height. Also the
 *                          window is no longer resizable. These constraints are for ease of 
 *                          programming with the Simulator.
 * 04/23/2014   ACA     Added an overloaded method for a main form and minor changes/updates.
 * 04/26/2014   ACA     Removed next button as debugging is now in main form.
 *                      Cannot close output windows while in debug mode to maintain consistancy.
 *                      Updated documentation.
 *                      Now focuses on textbox on show.
 *  
 *************************************************************************************************/

namespace Assist_UNA
{
    public partial class OutputForm : Form
    {

        /* Private members. */
        bool debug = false;
        bool nextClicked = false;
        MainForm main;

        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        OutputForm
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will initialize the output form.
         *  
         *****************************************************************************************/
        public OutputForm()
        {
            InitializeComponent();
        }


        /******************************************************************************************
         * 
         * Name:        OutputForm (overloaded)
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       The main form.
         * Return:      N/A
         * Description: This method will initialize the output form with a reference to mainform.
         *  
         *****************************************************************************************/
        public OutputForm(MainForm mainForm)
        {
            InitializeComponent();
            main = mainForm;
        }

        /******************************************************************************************
         * 
         * Name:        ContinueDebugging
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      True if next is clicked, false otherwise.
         * Description: This method will return whether or not the program should continue 
         *              debugging.
         *  
         *****************************************************************************************/
        public bool ContinueDebugging()
        {
            return nextClicked;
        }


        /******************************************************************************************
         * 
         * Name:        GetOutputText
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      The text in the output box (string).
         * Description: This method will initialize the output form.
         *  
         *****************************************************************************************/
        public string GetText()
        {
            return txtOutput.Text;
        }


        /******************************************************************************************
         * 
         * Name:        GetOutputText
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       True if debug mode is on, false otherwise.
         * Return:      N/A
         * Description: This method will set the output form into debug mode if needed.
         *  
         *****************************************************************************************/
        public void SetDebug(bool debugMode)
        {
            debug = debugMode;
        }


        /******************************************************************************************
         * 
         * Name:        SetText
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       String to be the new output form text.
         * Return:      N/A
         * Description: This method will set the output form text.
         *  
         *****************************************************************************************/
        public void SetText(string outputText)
        {
            txtOutput.Text = outputText;
        }


        /******************************************************************************************
         * 
         * Name:        BtnCloseClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method close the form without debugging.
         *  
         *****************************************************************************************/
        private void BtnCloseClick(object sender, EventArgs e)
        {
            this.Close();
        }


        /******************************************************************************************
         * 
         * Name:        OutputFormShown
         * 
         * Author(s):   Drew Aaron
         *              Michael Beaver
         *              
         * Input:       Form shown event.
         * Return:      N/A
         * Description: This method will change the form style to the new windows format.
         *  
         *****************************************************************************************/
        private void OutputFormShown(object sender, EventArgs e)
        {
            if (main != null)
            {
                /* Save last postion. */
                if ((this.Location.X == 0) && (this.Location.Y == 0))
                    this.Location = new Point(main.Location.X + 50, main.Location.Y + 50);

                /* In debug mode, there is no way to completly close the form, only hide it. */
                debug = main.GetDebugMode();
                if (debug)
                    btnClose.Text = "Hide";
                else
                    btnClose.Text = "Close";
            }
            txtOutput.Focus();
            txtOutput.Select(0, 0);
        }


        /******************************************************************************************
         * 
         * Name:        OutputFormFormClosing
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       Output form closing event.
         * Return:      N/A
         * Description: This method will change the form style a format which has no open or close
         *              animation, in order to have opening and closing the form instant.
         *  
         *****************************************************************************************/
        private void OutputFormFormClosing(object sender, FormClosingEventArgs e)
        {
            if (debug)
            {
                this.Hide();
                e.Cancel = true;
            }
        }
    }
}
