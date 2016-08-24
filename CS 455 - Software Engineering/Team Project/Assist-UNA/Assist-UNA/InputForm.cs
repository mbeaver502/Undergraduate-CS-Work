using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*************************************************************************************************
 * 
 * Name: InputForm class
 * 
 * ================================================================================================
 * 
 * Description: This class is an input form for user entered text.
 *                            
 * ================================================================================================
 * 
 * Modification History
 * --------------------
 * 03/22/2014   ACA     Original version.
 * 03/29/2014   ACA     Added another constructor with a character limit.
 * 04/19/2014   JMB     Changed border style to fixed.
 *  
 *************************************************************************************************/
namespace Assist_UNA
{
    
    public partial class InputForm : Form
    {
        /* Private Members. */
        bool canceled = false;
        bool okClicked = false;
        string input = "";

        /* Public Methods. */

        /******************************************************************************************
         * 
         * Name:        InputForm
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will initialize the input form with default settings.
         *  
         *****************************************************************************************/
        public InputForm()
        {
            InitializeComponent();
            this.Text = "";
            lblPrompt.Text = "";
        }


        /******************************************************************************************
         * 
         * Name:        InputForm (Overloaded)
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       The string for the prompt.
         * Return:      N/A
         * Description: This method will initialize the input form with a specified prompt.
         *  
         *****************************************************************************************/
        public InputForm(string prompt)
        {
            InitializeComponent();
            this.Text = "";
            lblPrompt.Text = prompt;
        }


        /******************************************************************************************
         * 
         * Name:        InputForm (Overloaded)
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       A string for the title and a string for the prompt.
         * Return:      N/A
         * Description: This method will initialize the input form with a specified title and 
         *              prompt with a character limit.
         *  
         *****************************************************************************************/
        public InputForm(string title, string prompt, int charLimit)
        {
            InitializeComponent();
            
            this.Text = title;
            lblPrompt.Text = prompt;
            
            if (charLimit >= 0)
                txtInput.MaxLength = charLimit;
        }


        /******************************************************************************************
         * 
         * Name:        InputForm (Overloaded)
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       A string for the title and a string for the prompt.
         * Return:      N/A
         * Description: This method will initialize the input form with a specified title and 
         *              prompt.
         *  
         *****************************************************************************************/
        public InputForm(string title, string prompt)
        {
            InitializeComponent();
            this.Text = title;
            lblPrompt.Text = prompt;
        }


        /******************************************************************************************
         * 
         * Name:        Canceled
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      The canceled data member.
         * Description: This method will return true if the input form was canceled, otherwise 
         *              false.
         *  
         *****************************************************************************************/
        public bool Canceled()
        {
            return canceled;
        }


        /******************************************************************************************
         * 
         * Name:        GetInput
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      The input data member.
         * Description: This method will return the input in the text box when Ok is clicked.
         *  
         *****************************************************************************************/
        public string GetInput()
        {
            return input;
        }


        /******************************************************************************************
         * 
         * Name:        BtnOkClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will set the textbox text as the input and close the form.
         *  
         *****************************************************************************************/
        private void BtnOkClick(object sender, EventArgs e)
        {
            input = txtInput.Text;
            okClicked = true;
            this.Close();
        }


        /******************************************************************************************
         * 
         * Name:        BtnOkClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will set canceled to true and close the form.
         *  
         *****************************************************************************************/
        private void BtnCancelClick(object sender, EventArgs e)
        {
            canceled = true;
            this.Close();
        }


        /******************************************************************************************
         * 
         * Name:        InputFormFormClosing
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       Form is in the process of closing.
         * Return:      N/A
         * Description: This method will set canceled to true when the form is closing if the Ok
         *              button is not clicked.
         *  
         *****************************************************************************************/
        private void InputFormFormClosing(object sender, FormClosingEventArgs e)
        {
            if (okClicked == false)
                canceled = true;
        }
    }
}
