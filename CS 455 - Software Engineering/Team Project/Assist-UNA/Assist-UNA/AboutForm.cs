using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;


/**************************************************************************************************
 * 
 * Name: AboutForm class
 * 
 * ================================================================================================
 * 
 * Description: This class is the about form part of the main GUI. 
 *                            
 * ================================================================================================
 * 
 * Modification History
 * --------------------
 * 03/09/2014   ACA     Original version.
 * 03/17/2014   THH     Changed the start location to be center screen.
 *                      Corrected some commenting format issues.
 * 03/28/2014   ACA     Updated method names to standards.
 * 04/01/2014   JMB     Added keypress events.
 * 04/03/2014   ACA     Updated about form and documentation.
 * 04/03/2014   JMB     Updated documentation and form appearance.
 * 04/24/2014   CAF     Added logo.
 * 04/26/2014   ACA     Colors now update with main form color scheme.
 *  
 **************************************************************************************************/
namespace Assist_UNA
{
    public partial class AboutForm : Form
    {
        /* Constants. */
        private const int NUM_KEYS = 10;
        private const string DATA = "aHR0cHM6Ly93d3cueW91dHViZS5jb20vd2F0Y2g/dj1DVmg3bFZNVFFCbw==";


        /* Private members. */
        List<KeyEventArgs> keys = new List<KeyEventArgs>();


        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        OptionsForm
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will initialize the about form.
         *  
         ******************************************************************************************/
        public AboutForm()
        {
            InitializeComponent();
            txtInfo.Focus();
            lblVersion.Text = "ASSIST/UNA v";
            lblVersion.Text += 
                Application.ProductVersion.Remove(Application.ProductVersion.IndexOf('.') + 2);
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
            txtAbout.BackColor = newColor;
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
            this.BackColor = newColor;
            lblVersion.BackColor = newColor;
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
            btnAboutClose.BackColor = newColor;
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
            lblVersion.ForeColor = newColor;
            btnAboutClose.ForeColor = newColor;
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
            txtAbout.ForeColor = newColor;
        }


        /* Private Methods */


        /******************************************************************************************
         * 
         * Name:        BtnAboutCloseClick
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       User click event
         * Return:      N/A
         * Description: Will close the about form.
         *  
         ******************************************************************************************/
        private void BtnAboutCloseClick(object sender, EventArgs e)
        {
            this.Close();
        }


        /******************************************************************************************
         * 
         * Name:        KeyHandle
         * 
         * Author(s):   Michael Beaver
         *              Drew Aaron
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will handle keypress inputs.
         *  
         ******************************************************************************************/
        private void KeyHandle()
        {
            string decodedData = Encoding.UTF8.GetString(Convert.FromBase64String(DATA));
            
            /* First two keys must be the UP arrow. */
            if (keys[0].KeyData != Keys.Up && keys[1].KeyData != Keys.Up)
            {
                keys.Clear();
                return;
            }

            /* Third and fourth keys must be DOWN arrow. */
            else if (keys[2].KeyData != Keys.Down && keys[3].KeyData != Keys.Down)
            {
                keys.Clear();
                return;
            }
                
            /* Fifth and sixth keys must be LEFT arrow and RIGHT arrow. */
            else if (keys[4].KeyData != Keys.Left && keys[5].KeyData != Keys.Right)
            {
                keys.Clear();
                return;
            }

            /* Seventh and eighth keys must be LEFT arrow and RIGHT arrow. */
            else if (keys[6].KeyData != Keys.Left && keys[7].KeyData != Keys.Right)
            {
                keys.Clear();
                return;
            }

            /* Last two keys must be B and A. */
            else if (keys[8].KeyData != Keys.B || keys[9].KeyData != Keys.A)
            {
                keys.Clear();
                return;
            }

            Process.Start(decodedData);
            keys.Clear();
        }


        /******************************************************************************************
         * 
         * Name:        TxtInfoKeyUp
         * 
         * Author(s):   Michael Beaver
         *              Drew Aaron
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will capture the release of keypresses.
         *  
         ******************************************************************************************/
        private void TxtInfoKeyUp(object sender, KeyEventArgs e)
        {
            /* Add certain key data to key vector. */
            if (e.KeyData == Keys.Up || e.KeyData == Keys.Down || e.KeyData == Keys.Left || 
                e.KeyData == Keys.Right || e.KeyData == Keys.A || e.KeyData == Keys.B)
                    keys.Add(e);

            /* The keys need to be handled once there are NUM_KEYS keys in the vector. */
            if (keys.Count() == NUM_KEYS)
                KeyHandle();

            if (!txtInfo.Focused)
                txtInfo.Focus();
        }


        /******************************************************************************************
         * 
         * Name:        AboutFormActivated
         * 
         * Author(s):   Michael Beaver
         *              
         * Input:       N/A
         * Return:      N/A
         * Description: This method will refocus onto the txtInfo textbox.
         *  
         ******************************************************************************************/
        private void AboutFormActivated(object sender, EventArgs e)
        {
            txtInfo.Focus();
        }
    }
}
