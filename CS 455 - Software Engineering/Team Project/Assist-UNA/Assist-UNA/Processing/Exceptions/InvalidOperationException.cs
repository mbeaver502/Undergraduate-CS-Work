using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**************************************************************************************************
 * 
 * Name: InvalidOperationException
 * 
 * ================================================================================================
 * 
 * Description: This class is an exception handler for invalid operations.
 *                       
 * ================================================================================================        
 * 
 * Modification History
 * --------------------
 * 04/06/2014   THH     Original version.
 *                       
 *************************************************************************************************/

namespace Assist_UNA
{
    class InvalidOperationException : Exception
    {

        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        InvalidOperationException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       N/A   
         * Return:      N/A
         * Description: The default constructor.
         *              
         *****************************************************************************************/
        public InvalidOperationException() : base() { }

        /******************************************************************************************
         * 
         * Name:        InvalidOperationException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       The message to set as string.   
         * Return:      N/A
         * Description: The overloaded constructor that sets the message for the exception.
         *              
         *****************************************************************************************/
        public InvalidOperationException(string message) : base(message) { }

        /******************************************************************************************
         * 
         * Name:        InvalidOperationException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       The message to set as string and the inner exception.   
         * Return:      N/A
         * Description: The overloaded constructor that sets the message for the exception as well
         *              as the inner exception.
         *              
         *****************************************************************************************/
        public InvalidOperationException(string message, System.Exception inner) : base(message, inner) { }
    }
}
