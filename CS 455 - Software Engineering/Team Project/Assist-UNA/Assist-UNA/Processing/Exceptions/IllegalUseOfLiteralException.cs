using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**************************************************************************************************
 * 
 * Name: IllegalUseOfLiteralException
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
    class IllegalUseOfLiteralException : Exception
    {

        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        IllegalUseOfLiteralException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       N/A   
         * Return:      N/A
         * Description: The default constructor.
         *              
         *****************************************************************************************/
        public IllegalUseOfLiteralException() : base() { }

        /******************************************************************************************
         * 
         * Name:        IllegalUseOfLiteralException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       The message to set as string.   
         * Return:      N/A
         * Description: The overloaded constructor that sets the message for the exception.
         *              
         *****************************************************************************************/
        public IllegalUseOfLiteralException(string message) : base(message) { }

        /******************************************************************************************
         * 
         * Name:        IllegalUseOfLiteralException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       The message to set as string and the inner exception.   
         * Return:      N/A
         * Description: The overloaded constructor that sets the message for the exception as well
         *              as the inner exception.
         *              
         *****************************************************************************************/
        public IllegalUseOfLiteralException(string message, System.Exception inner) : base(message, inner) { }
    }
}
