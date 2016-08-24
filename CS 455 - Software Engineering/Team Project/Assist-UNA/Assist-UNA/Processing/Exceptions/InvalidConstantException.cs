using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**************************************************************************************************
 * 
 * Name: InvalidConstantException
 * 
 * ================================================================================================
 * 
 * Description: This class is an exception handler for invalid symbols.
 *                       
 * ================================================================================================        
 * 
 * Modification History
 * --------------------
 * 04/11/2014   THH     Original version.
 *                       
 *************************************************************************************************/

namespace Assist_UNA
{
    class InvalidConstantException : Exception
    {

        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        InvalidConstantException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       N/A   
         * Return:      N/A
         * Description: The default constructor.
         *              
         *****************************************************************************************/
        public InvalidConstantException() : base() { }

        /******************************************************************************************
         * 
         * Name:        InvalidConstantException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       The message to set as string.   
         * Return:      N/A
         * Description: The overloaded constructor that sets the message for the exception.
         *              
         *****************************************************************************************/
        public InvalidConstantException(string message) : base(message) { }

        /******************************************************************************************
         * 
         * Name:        InvalidConstantException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       The message to set as string and the inner exception.   
         * Return:      N/A
         * Description: The overloaded constructor that sets the message for the exception as well
         *              as the inner exception.
         *              
         *****************************************************************************************/
        public InvalidConstantException(string message, System.Exception inner) : base(message, inner) { }
    }
}
