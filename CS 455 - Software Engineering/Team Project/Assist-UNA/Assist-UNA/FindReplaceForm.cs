﻿using System;
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
 * Name: FindReplaceForm class
 * 
 * ================================================================================================
 * 
 * Description: This class is the find and replace interface tool window. 
 *                            
 * ================================================================================================
 * 
 * Modification History
 * --------------------
 * 04/01/2014   ACA     Original version.
 * 04/17/2014   ACA     Added support for rich text boxes.
 *                      No longer matches case.
 *                      Fixed some logic that might be frustrating for user.
 *                      Disabled replace textbox and buttons when in search only mode.
 * 04/21/2014   ACA     Added more space between boxes and made find box smaller.
 *  
 *************************************************************************************************/

namespace Assist_UNA
{
    public partial class FindReplaceForm : Form
    {
        /* Private members. */
        RichTextBox txtSource;
        int position = 0;

        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        FindReplaceForm
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will initialize the search and replace form.
         *  
         *****************************************************************************************/
        public FindReplaceForm()
        {
            InitializeComponent();
        }


        /******************************************************************************************
         * 
         * Name:        FindReplaceForm (overloaded)
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       The bool replace will be true if search and replace is wanted, and false if
         *              only find is wanted. The txtInput parameter is a pointer to the source code
         *              editor.
         * Return:      N/A
         * Description: This method will initialize the search and replace form into the correct
         *              state and sets a pointer to the source code editor.
         *  
         *****************************************************************************************/
        public FindReplaceForm(bool replace, CustomSourceEditor @txtInput)
        {
            InitializeComponent();

            /* If it is a search-only form, hide and disable the replace items. */
            if (replace == false) 
            {
                this.Size = new Size(300, 72);
                btnReplace.Enabled = false;
                txtReplace.Enabled = false;
            }
                
            else
                this.Text = "Search and Replace";

            txtSource = txtInput;

            txtFind.Select();
        }


        /******************************************************************************************
         * 
         * Name:        FindReplaceForm (overloaded)
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       The bool replace will be true if search and replace is wanted, and false if
         *              only find is wanted. The txtInput parameter is a pointer to the source code
         *              editor.
         * Return:      N/A
         * Description: This method will initialize the search and replace form into the correct
         *              state and sets a pointer to the source code editor.
         *  
         *****************************************************************************************/
        public FindReplaceForm(bool replace, RichTextBox @txtInput)
        {
            InitializeComponent();

            /* If it is a search-only form, hide and disable the replace items. */
            if (replace == false)
            {
                this.Size = new Size(300, 75);
                btnReplace.Enabled = false;
                txtReplace.Enabled = false;
            }

            else
                this.Text = "Search and Replace";

            txtSource = txtInput;
        }


        /* Private methods. */

        /*****************************************************************************************
         * 
         * Name:        BtnFindClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will highlight the text in the source code editor that matches 
         *              the text entered into the find textbox, if any.              
         *  
         *****************************************************************************************/
        private void BtnFindClick(object sender, EventArgs e)
        {
            if ((txtFind.Text != "") && (txtSource.Text.ToUpper().Contains(txtFind.Text.ToUpper())))
            {
                if (position >= txtSource.Text.Length - 1)
                    position = 0;

                position = txtSource.Text.ToUpper().IndexOf(txtFind.Text.ToUpper(), position);

                if (position < 0)
                {
                    position = txtSource.Text.ToUpper().IndexOf(txtFind.Text.ToUpper(), 0);
                }

                txtSource.Select(position, txtFind.Text.Length);
                position += txtFind.Text.Length;
                txtSource.Focus();
            }
        }


        /*****************************************************************************************
         * 
         * Name:        BtnReplaceClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: This method will replace the text in the source code editor that matches 
         *              the text entered into the find textbox, if any, with the text entered into
         *              the replace text box.              
         *  
         *****************************************************************************************/
        private void BtnReplaceClick(object sender, EventArgs e)
        {
            if (txtSource.SelectedText.ToUpper() == txtFind.Text.ToUpper())
            {
                txtSource.SelectedText = txtReplace.Text;
            
                BtnFindClick(this, e);
            }
        }
    }
}
