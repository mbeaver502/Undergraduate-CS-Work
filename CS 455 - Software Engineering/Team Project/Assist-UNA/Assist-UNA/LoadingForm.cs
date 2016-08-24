using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

/*************************************************************************************************
 * 
 * Name: InputForm class
 * 
 * ================================================================================================
 * 
 * Description: This class is a loading form used when starting ASSIST-UNA.
 *                            
 * ================================================================================================
 * 
 * Modification History
 * --------------------
 * 03/22/2014   ACA     Original version.
 *   
 *************************************************************************************************/
namespace Assist_UNA
{
    public partial class LoadingForm : Form
    {
        /******************************************************************************************
         * 
         * Name:        LoadingForm        
         * 
         * Author(s):   Drew Aaron
         *              
         * Input:       N/A 
         * Return:      N/A
         * Description: This method will construct and initialize the loading splash screen.
         *              
         *****************************************************************************************/
        public LoadingForm()
        {
            InitializeComponent();
        }
    }
}
