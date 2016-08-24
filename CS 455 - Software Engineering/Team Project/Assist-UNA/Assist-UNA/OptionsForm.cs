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

/**************************************************************************************************
 * 
 * Name: OptionsForm class
 * 
 * ================================================================================================
 * 
 * Description: This class is the options form part of the main GUI. 
 *                            
 * ================================================================================================
 * 
 * Modification History
 * --------------------
 * 03/09/2014   ACA     Original version.
 * 03/09/2014   ACA     Added data members and options form functionality. Documented code.
 * 03/17/2014   ACA     Fixed documentation.
 * 03/17/2014   THH     Made the start location center screen.
 * 03/20/2014   ACA     Added exception handling and changed how the buttons work.
 * 03/22/2014   ACA     Added color data members and their methods for future use.
 * 03/23/2014   ACA     Updated options with some UNA/Assist options.
 * 03/24/2014   ACA     Added option to set theme/colors.
 *                      The path to the PRT can now be set.
 * 03/29/2014   ACA     Some minor tweaks and documentation updates.
 * 04/01/2014   ACA     Added boolean to tell if comment color was changed, so it won't loop
 *                      through the code if it was not.
 * 04/18/2014   JMB     Fixed an error where TxtMaxSizeTextChanged would not enabled the Apply button.
 *  
 *************************************************************************************************/

namespace Assist_UNA
{
    public partial class OptionsForm : Form
    {
        
        /* Private members. */
        private bool commentColorChange = false;
        private bool commentColorChangeApplied = false;
        private Color backColorAccent = Color.FromArgb(255, 255, 162);
        private Color backColorAccentUNA = Color.FromArgb(255, 255, 162);
        private Color backColorAccentSystem = SystemColors.Control;
        private Color backColorMain = Color.FromArgb(152, 0, 230);
        private Color backColorMainUNA = Color.FromArgb(152, 0, 230);
        private Color backColorMainSystem = SystemColors.ControlDark;
        private Color backColorMain2 = Color.FromArgb(180, 100, 255);
        private Color backColorMain2UNA = Color.FromArgb(180, 100, 255);
        private Color backColorMain2System = SystemColors.ControlLight;
        private Color commentColor = Color.DeepPink;
        private Color defaultSourceColor = Color.Black;
        private Color formLabelTextColor = SystemColors.Control;
        private Color formLabelTextColorUNA = SystemColors.Control;
        private Color formLabelTextColorSystem = Color.Black;
        private Color formTextColor = Color.FromArgb(120, 0, 120);
        private Color formTextColorUNA = Color.FromArgb(120, 0, 120);
        private Color formTextColorSystem = Color.Black;
        private int maxInstructions = 5000;
        private int maxLines = 500;
        private int maxPages = 100;
        private int maxSize = 2700;
        private string pathPRT;


        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        OptionsForm
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will initialize the options form.
         *  
         *****************************************************************************************/
        public OptionsForm()
        {
            InitializeComponent();
            UpdateDisplay();
        }


        /******************************************************************************************
         * 
         * Name:        commentColorChanged
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      True if comment colors were changed, false otherwise.
         * Description: This method will return whether or not the comment color was changed.
         *  
         *****************************************************************************************/
        public bool commentColorChanged()
        {
            return commentColorChangeApplied;
        }


        /******************************************************************************************
         * 
         * Name:        GetBackColorAccent
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      The backColorAccent data member (Color type).
         * Description: This method will return the color type for the form's accent backcolor.
         *  
         *****************************************************************************************/
        public Color GetBackColorAccent()
        {
            return backColorAccent;
        }


        /******************************************************************************************
         * 
         * Name:        GetBackColorMain
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      The backColorMain data member (Color type).
         * Description: This method will return the color type for the form's backcolor.
         *  
         *****************************************************************************************/
        public Color GetBackColorMain()
        {
            return backColorMain;
        }


        /******************************************************************************************
         * 
         * Name:        GetBackColorMain2
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      The backColorMain2 data member (Color type).
         * Description: This method will return the color type for the registers' backcolor.
         *  
         *****************************************************************************************/
        public Color GetBackColorMain2()
        {
            return backColorMain2;
        }


        /******************************************************************************************
         * 
         * Name:        GetCommentColor
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      The commentColor data member (Color type).
         * Description: This method will return the color type for source comments.
         *  
         *****************************************************************************************/
        public Color GetCommentColor()
        {
            return commentColor;
        }


        /******************************************************************************************
         * 
         * Name:        GetDefaultSourceColor
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      The defaultSourceColor data member (Color type).
         * Description: This method will return the color type for the source code text.
         *  
         *****************************************************************************************/
        public Color GetDefaultSourceColor()
        {
            return defaultSourceColor;
        }


        /******************************************************************************************
         * 
         * Name:        GetFormLabelTextColor
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      The formLabelTextColor data member (Color type).
         * Description: This method will return the default color type for the form's labels.
         *  
         *****************************************************************************************/
        public Color GetFormLabelTextColor()
        {
            return formLabelTextColor;
        }


        /******************************************************************************************
         * 
         * Name:        GetFormTextColor
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      The formTextColor data member (Color type).
         * Description: This method will return the default color type for the form's text.
         *  
         *****************************************************************************************/
        public Color GetFormTextColor()
        {
            return formTextColor;
        }


        /******************************************************************************************
        * 
        * Name:        GetMaxInstructions
        * 
        * Author(s):   Drew Aaron
        *              
        * Input:       N/A
        * Return:      The maxInstructions data member (int).
        * Description: This method will return maxInstructions.
        *  
        *****************************************************************************************/
        public int GetMaxInstructions()
        {
            return maxInstructions;
        }


        /******************************************************************************************
         * 
         * Name:        GetMaxLines
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      The maxLines data member (int).
         * Description: This method will return maxLines.
         *  
         *****************************************************************************************/
        public int GetMaxLines()
        {
            return maxLines;
        }


        /******************************************************************************************
         * 
         * Name:        GetMaxPages
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      The maxPages data member (int).
         * Description: This method will return maxPages.
         *  
         *****************************************************************************************/
        public int GetMaxPages()
        {
            return maxPages;
        }


        /******************************************************************************************
         * 
         * Name:        GetMaxSize
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      The maxSize data member (int).
         * Description: This method will return maxSize.
         *  
         *****************************************************************************************/
        public int GetMaxSize()
        {
            return maxSize;
        }


        /******************************************************************************************
         * 
         * Name:        GetPathPRT
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      The pathPRT data member (string).
         * Description: This method will return the alternate color type for the form's text.
         *  
         *****************************************************************************************/
        public string GetPathPRT()
        {
            return pathPRT;
        }

        /******************************************************************************************
         * 
         * Name:        SetBackColorAccent
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       New color for the backColorAccent data member (Color type).
         * Return:      N/A
         * Description: This method will set the color type for the form's accent backcolor.
         *  
         *****************************************************************************************/
        public void SetBackColorAccent(Color newColor)
        {
            backColorAccent = newColor;
            UpdateDisplay();
        }


        /******************************************************************************************
         * 
         * Name:        SetBackColorMain
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       New color for the backColorMain data member (Color type).
         * Return:      N/A
         * Description: This method will set the color type for the form's backcolor.
         *  
         *****************************************************************************************/
        public void SetBackColorMain(Color newColor)
        {
            backColorMain = newColor;
            UpdateDisplay();
        }


        /******************************************************************************************
         * 
         * Name:        SetBackColorMain2
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       New color for the backColorMain2 data member (Color type).
         * Return:      N/A
         * Description: This method will set the color type for the registers' backcolor.
         *  
         *****************************************************************************************/
        public void SetBackColorMain2(Color newColor)
        {
            backColorMain2 = newColor;
            UpdateDisplay();
        }


        /******************************************************************************************
         * 
         * Name:        SetCommentColor
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       New color for the commentColor data member (Color type).
         * Return:      N/A
         * Description: This method will set the color type for source comments.
         *  
         *****************************************************************************************/
        public void SetCommentColor(Color newColor)
        {
            commentColor = newColor;
            txtCommentColor.ForeColor = newColor;
        }


        /******************************************************************************************
         * 
         * Name:        SetDefaultSourceColor
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       New color for the defaultSourceColor data member (Color type).
         * Return:      N/A
         * Description: This method will set the color type for the source code text.
         *  
         *****************************************************************************************/
        public void SetDefaultSourceColor(Color newColor)
        {
            defaultSourceColor = newColor;
            UpdateDisplay();
        }


        /******************************************************************************************
         * 
         * Name:        SetFormLabelTextColor
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       New color for the formLabelTextColor data member (Color type).
         * Return:      N/A
         * Description: This method will set the default color type for the form's labels.
         *  
         *****************************************************************************************/
        public void SetFormLabelTextColor(Color newColor)
        {
            formLabelTextColor = newColor;
            UpdateDisplay();
        }


        /******************************************************************************************
         * 
         * Name:        SetFormTextColor
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       New color for the formTextColor data member (Color type).
         * Return:      N/A
         * Description: This method will set the default color type for the form's text.
         *  
         *****************************************************************************************/
        public void SetFormTextColor(Color newColor)
        {
            formTextColor = newColor;
            UpdateDisplay();
        }


        /******************************************************************************************
         * 
         * Name:        SetMaxInstructions
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       The current maximum number of instructions
         *              (int).
         * Return:      N/A
         * Description: This method will set the maxInstructions data member equal to the given 
         *              integer (which is usually the current maxInstructions in MainForm). Also 
         *              sets the correct number in the max instructions text box.
         *  
         *****************************************************************************************/
        public void SetMaxInstructions(int e)
        {
            maxInstructions = e;
            txtMaxInstructions.Clear();
            txtMaxInstructions.AppendText(Convert.ToString(e));
            btnOptionsApply.Enabled = false;
        }


        /******************************************************************************************
         * 
         * Name:        SetMaxLines
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       The current maximum number of lines (int).
         * Return:      N/A
         * Description: This method will set the maxLines data member equal to the given integer 
         *              (which is usually the current maxLines in MainForm). Also sets the correct
         *              number in the max lines text box.
         *  
         *****************************************************************************************/
        public void SetMaxLines(int e)
        {
            maxLines = e;
            txtMaxLines.Clear();
            txtMaxLines.AppendText(Convert.ToString(e));
            btnOptionsApply.Enabled = false;
        }


        /******************************************************************************************
         * 
         * Name:        SetMaxPages
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       The current maximum number of pages (int).
         * Return:      N/A
         * Description: This method will set the maxPages data member equal to the given integer 
         *              (which is usually the current maxPages in MainForm). Also sets the correct
         *              number in the max pages text box.
         *  
         *****************************************************************************************/
        public void SetMaxPages(int e)
        {
            maxPages = e;
            txtMaxPages.Clear();
            txtMaxPages.AppendText(Convert.ToString(e));
            btnOptionsApply.Enabled = false;
        }


        /******************************************************************************************
         * 
         * Name:        SetMaxSize
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       The current maximum file size in bytes 
         *              (int).
         * Return:      N/A
         * Description: This method will set the maxSize data member equal to the given integer 
         *              (which is usually the current maxSize in MainForm). Also sets the correct
         *              number in the max size text box.
         *  
         *****************************************************************************************/
        public void SetMaxSize(int e)
        {
            maxSize = e;
            txtMaxSize.Clear();
            txtMaxSize.AppendText(Convert.ToString(e));
            btnOptionsApply.Enabled = false;
        }


        /******************************************************************************************
         * 
         * Name:        SetPathPRT
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       The path to the associated PRT file. 
         *              (int).
         * Return:      N/A
         * Description: This method will set the pathPRT data member equal to the given string. 
         *              Also sets the correct path in the PRT path text box.
         *  
         *****************************************************************************************/
        public void SetPathPRT(string newPath)
        {
            pathPRT = newPath;
            txtBrowsePRT.Text = pathPRT;
        }


        /* Private methods. */


        /******************************************************************************************
         * 
         * Name:        BtnAccentClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: Will open a color dialog to let the user choose a new color for the accent.
         *  
         *****************************************************************************************/
        private void BtnAccentClick(object sender, EventArgs e)
        {
            ColorDialog dlgColor = new ColorDialog();
            DialogResult choice = dlgColor.ShowDialog();
            
            if (choice == DialogResult.OK)
            {
                txtAccent.BackColor = dlgColor.Color;
                txtFormText.BackColor = dlgColor.Color;
                btnOptionsApply.Enabled = true;
            }

        }

        /******************************************************************************************
         * 
         * Name:        BtnBackColorClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: Will open a color dialog to let the user choose a new color for the main
         *              background color.
         *  
         *****************************************************************************************/

        private void BtnBackColorClick(object sender, EventArgs e)
        {
            ColorDialog dlgColor = new ColorDialog();
            DialogResult choice = dlgColor.ShowDialog();
            
            if (choice == DialogResult.OK)
            {
                txtBackColor.BackColor = dlgColor.Color;
                btnOptionsApply.Enabled = true;
            }
        }


        /******************************************************************************************
         * 
         * Name:        BtnBackColor2Click
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: Will open a color dialog to let the user choose a new color for the
         *              registers' background color.
         *  
         *****************************************************************************************/
        private void BtnBackColor2Click(object sender, EventArgs e)
        {
            ColorDialog dlgColor = new ColorDialog();
            DialogResult choice = dlgColor.ShowDialog();

            if (choice == DialogResult.OK)
            {
                txtBackColor2.BackColor = dlgColor.Color;
                txtLabelColor.BackColor = dlgColor.Color;
                btnOptionsApply.Enabled = true;
            }
        }


        /******************************************************************************************
         * 
         * Name:        BtnBrowsePRTClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: Will open an open file dialog to let the user choose the associated PRT
         *              file.
         *  
         *****************************************************************************************/
        private void BtnBrowsePRTClick(object sender, EventArgs e)
        {
            OpenFileDialog dlgOpen = new OpenFileDialog();
            dlgOpen.Filter = "PRT Files (*.PRT)|*.PRT";
            DialogResult choice = dlgOpen.ShowDialog();

            if (choice == DialogResult.OK)
            {
                txtBrowsePRT.Text = dlgOpen.FileName;
                btnOptionsApply.Enabled = true;
            }
        }


        /******************************************************************************************
         * 
         * Name:        BtnCommentColorClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: Will open a color dialog to let the user choose a new color for comments.
         *  
         *****************************************************************************************/
        private void BtnCommentColorClick(object sender, EventArgs e)
        {
            ColorDialog dlgColor = new ColorDialog();
            DialogResult choice = dlgColor.ShowDialog();

            if (choice == DialogResult.OK)
            {
                txtCommentColor.ForeColor = dlgColor.Color;
                btnOptionsApply.Enabled = true;
                commentColorChange = true;
            }
        }


        /******************************************************************************************
         * 
         * Name:        BtnFormTextClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: Will open a color dialog to let the user choose a new color for the text
         *              in the main form.
         *  
         *****************************************************************************************/
        private void BtnFormTextClick(object sender, EventArgs e)
        {
            ColorDialog dlgColor = new ColorDialog();
            DialogResult choice = dlgColor.ShowDialog();

            if (choice == DialogResult.OK)
            {
                txtFormText.ForeColor = dlgColor.Color;
                btnOptionsApply.Enabled = true;
            }
        }


        /******************************************************************************************
         * 
         * Name:        BtnLabelColorClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: Will open a color dialog to let the user choose a new color for the labels
         *              in the registers area.
         *  
         *****************************************************************************************/
        private void BtnLabelColorClick(object sender, EventArgs e)
        {
            ColorDialog dlgColor = new ColorDialog();
            DialogResult choice = dlgColor.ShowDialog();

            if (choice == DialogResult.OK)
            {
                txtLabelColor.ForeColor = dlgColor.Color;
                btnOptionsApply.Enabled = true;
            }
        }


        /******************************************************************************************
         * 
         * Name:        BtnOptionsApplyClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: Will update the data members to the values currently in their respective
         *              text boxes.
         *  
         *****************************************************************************************/
        private void BtnOptionsApplyClick(object sender, EventArgs e)
        {
            int catcher = 0;
            try 
            {
                catcher = maxLines;
                maxLines = Convert.ToInt32(txtMaxLines.Text);
            }
            catch (Exception) {
                txtMaxLines.Text = Convert.ToString(catcher);
                return;
            }

            try
            {
                catcher = maxInstructions;
                maxInstructions = Convert.ToInt32(txtMaxInstructions.Text);
            }
            catch (Exception)
            {
                txtMaxInstructions.Text = Convert.ToString(catcher);
                return;
            }

            try
            {
                catcher = maxPages;
                maxPages = Convert.ToInt32(txtMaxPages.Text);
            }
            catch (Exception)
            {
                txtMaxPages.Text = Convert.ToString(catcher);
                return;
            }

            try
            {
                catcher = maxPages;
                maxSize = Convert.ToInt32(txtMaxSize.Text);
            }
            catch (Exception)
            {
                txtMaxSize.Text = Convert.ToString(catcher);
                return;
            }

            /* Set colors and PRT path. */
            backColorAccent = txtAccent.BackColor;
            backColorMain = txtBackColor.BackColor;
            backColorMain2 = txtBackColor2.BackColor;
            commentColor = txtCommentColor.ForeColor;
            formLabelTextColor = txtLabelColor.ForeColor;
            formTextColor = txtFormText.ForeColor;
            pathPRT = txtBrowsePRT.Text;

            if (commentColorChange)
                commentColorChangeApplied = true;

            /* Disable the apply button since everything is now current. */
            btnOptionsApply.Enabled = false;
        }


        /******************************************************************************************
         * 
         * Name:        BtnOptionsCloseClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: Closes the form.
         *  
         *****************************************************************************************/
        private void BtnOptionsCloseClick(object sender, EventArgs e)
        {
            this.Close();
        }


        /******************************************************************************************
         * 
         * Name:        RadCustomCheckedChanged
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: Enables/disables the custom color buttons on check change.
         *  
         *****************************************************************************************/
        private void RadCustomCheckedChanged(object sender, EventArgs e)
        {
            btnOptionsApply.Enabled = true;

            if (radCustom.Checked)
            {
                btnAccent.Enabled = true;
                btnBackColor.Enabled = true;
                btnBackColor2.Enabled = true;
                btnFormText.Enabled = true;
                btnLabelColor.Enabled = true;
            }

            else
            {
                btnAccent.Enabled = false;
                btnBackColor.Enabled = false;
                btnBackColor2.Enabled = false;
                btnFormText.Enabled = false;
                btnLabelColor.Enabled = false;
            }
        }


        /******************************************************************************************
         * 
         * Name:        RadSystemCheckedChanged
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: Changes the theme colors to the system theme colors.
         *  
         *****************************************************************************************/
        private void RadSystemCheckedChanged(object sender, EventArgs e)
        {
            /* Enable the apply button since settings have changed. */
            btnOptionsApply.Enabled = true;

            /* Set the colors to the system theme colors. */
            if (radSystem.Checked)
            {
                txtAccent.BackColor = backColorAccentSystem;
                txtBackColor.BackColor = backColorMainSystem;
                txtBackColor2.BackColor = backColorMain2System;
                txtLabelColor.ForeColor = formLabelTextColorSystem;
                txtLabelColor.BackColor = backColorMain2System;
                txtFormText.ForeColor = formTextColorSystem;
                txtFormText.BackColor = backColorAccentSystem;
            }
        }


        /******************************************************************************************
         * 
         * Name:        RadUNACheckedChanged
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: Changes the theme colors to the system theme colors.
         *  
         *****************************************************************************************/
        private void RadUNACheckedChanged(object sender, EventArgs e)
        {
            /* Enable the apply button since settings have changed. */
            btnOptionsApply.Enabled = true;

            /* Set the colors to the UNA theme colors. */
            if (radUNA.Checked)
            {
                txtAccent.BackColor = backColorAccentUNA;
                txtBackColor.BackColor = backColorMainUNA;
                txtBackColor2.BackColor = backColorMain2UNA;
                txtLabelColor.ForeColor = formLabelTextColorUNA;
                txtLabelColor.BackColor = backColorMain2UNA;
                txtFormText.ForeColor = formTextColorUNA;
                txtFormText.BackColor = backColorAccentUNA;
            }
        }


        /******************************************************************************************
         * 
         * Name:        TxtMaxLinesTextChanged
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event.
         * Return:      N/A
         * Description: Enables the apply button once the values are changed.
         *  
         *****************************************************************************************/
        private void TxtMaxLinesTextChanged(object sender, EventArgs e)
        {
            btnOptionsApply.Enabled = true;
        }


        /******************************************************************************************
         * 
         * Name:        TxtMaxInstructionsTextChanged
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event
         * Return:      N/A
         * Description: Enables the apply button once the values are changed.
         *  
         *****************************************************************************************/
        private void TxtMaxInstructionsTextChanged(object sender, EventArgs e)
        {
            btnOptionsApply.Enabled = true;
        }


        /******************************************************************************************
         * 
         * Name:        TxtMaxPagesTextChanged
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event
         * Return:      N/A
         * Description: Enables the apply button once the values are changed.
         *  
         *****************************************************************************************/
        private void TxtMaxPagesTextChanged(object sender, EventArgs e)
        {
            btnOptionsApply.Enabled = true;
        }


        /******************************************************************************************
         * 
         * Name:        TxtMaxSizeTextChanged
         * 
         * Author(s):   Drew Aaron
         *              Michael Beaver
         *              
         * Input:       User click event
         * Return:      N/A
         * Description: Enables the apply button once the values are changed.
         *  
         *****************************************************************************************/
        private void TxtMaxSizeTextChanged(object sender, EventArgs e)
        {
            btnOptionsApply.Enabled = true;
        }


        /******************************************************************************************
         * 
         * Name:        UpdateDisplay
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: Updates the display when the form variables are set.
         *  
         *****************************************************************************************/
        private void UpdateDisplay()
        {
            /* Set the correct selected theme. */
            if ((backColorAccent == backColorAccentUNA) && (backColorMain == backColorMainUNA) &&
                (backColorMain2 == backColorMain2UNA) && (formTextColor == formTextColorUNA) &&
                (formLabelTextColor == formLabelTextColorUNA))
                    radUNA.Checked = true;

            else if ((backColorAccent == backColorAccentSystem) && (backColorMain ==
                backColorMainSystem) && (backColorMain2 == backColorMain2System) && (formTextColor
                == formTextColorSystem) && (formLabelTextColor == formLabelTextColorSystem))
                    radSystem.Checked = true;

            else
                radCustom.Checked = true;

            /* Disable apply button. */
            btnOptionsApply.Enabled = false;
            txtMaxLines.Focus();
        }

    }
}
