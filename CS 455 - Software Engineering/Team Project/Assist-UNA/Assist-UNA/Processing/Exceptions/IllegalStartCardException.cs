using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**************************************************************************************************
 * 
 * Name: IllegalStartCardException
 * 
 * ================================================================================================
 * 
 * Description: This class is an exception handler for missing delimiters.
 *                       
 * ================================================================================================        
 * 
 * Modification History
 * --------------------
 * 04/22/2014   THH     Original version.
 *                       
 *************************************************************************************************/

namespace Assist_UNA
{
    class IllegalStartCardException : Exception
    {

        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        IllegalStartCardException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       N/A   
         * Return:      N/A
         * Description: The default constructor.
         *              
         *****************************************************************************************/
        public IllegalStartCardException() : base() { }

        /******************************************************************************************
         * 
         * Name:        IllegalStartCardException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       The message to set as string.   
         * Return:      N/A
         * Description: The overloaded constructor that sets the message for the exception.
         *              
         *****************************************************************************************/
        public IllegalStartCardException(string message) : base(message) { }

        /******************************************************************************************
         * 
         * Name:        IllegalStartCardException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       The message to set as string and the inner exception.   
         * Return:      N/A
         * Description: The overloaded constructor that sets the message for the exception as well
         *              as the inner exception.
         *              
         *****************************************************************************************/
        public IllegalStartCardException(string message, System.Exception inner) : base(message, inner) { }
    }
}
