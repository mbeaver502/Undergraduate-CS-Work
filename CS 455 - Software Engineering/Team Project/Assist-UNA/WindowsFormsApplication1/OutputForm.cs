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
 *  
 *************************************************************************************************/

namespace Assist_UNA
{
    public partial class OutputForm : Form
    {

        /* Private members. */
        bool nextClicked = false;

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
        public void setDebug(bool debugMode)
        {
            if (debugMode)
                btnNext.Visible = true;
            else
                btnNext.Visible = false;
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
         * Name:        BtnNextClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method close the form and tell the main form to continue debugging.
         *  
         *****************************************************************************************/
        private void BtnNextClick(object sender, EventArgs e)
        {
            nextClicked = true;
            this.Close();
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
            nextClicked = false;
            this.Close();
        }


        /******************************************************************************************
         * 
         * Name:        OutputFormShown
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       Form shown event.
         * Return:      N/A
         * Description: This method will change the form style to the new windows format.
         *  
         *****************************************************************************************/
        private void OutputFormShown(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.Sizable;
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
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
        }
    }
}
