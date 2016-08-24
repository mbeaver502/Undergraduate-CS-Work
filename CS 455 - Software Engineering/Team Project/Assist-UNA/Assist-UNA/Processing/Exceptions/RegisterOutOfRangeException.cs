using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**************************************************************************************************
 * 
 * Name: RegisterOutOfRangeException
 * 
 * ================================================================================================
 * 
 * Description: This class is an exception handler for registers that are out of range.
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
    class RegisterOutOfRangeException : Exception
    {

        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        RegisterOutOfRangeException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       N/A   
         * Return:      N/A
         * Description: The default constructor.
         *              
         *****************************************************************************************/
        public RegisterOutOfRangeException() : base() { }

        /******************************************************************************************
         * 
         * Name:        RegisterOutOfRangeException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       The message to set as string.   
         * Return:      N/A
         * Description: The overloaded constructor that sets the message for the exception.
         *              
         *****************************************************************************************/
        public RegisterOutOfRangeException(string message) : base(message) { }

        /******************************************************************************************
         * 
         * Name:        RegisterOutOfRangeException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       The message to set as string and the inner exception.   
         * Return:      N/A
         * Description: The overloaded constructor that sets the message for the exception as well
         *              as the inner exception.
         *              
         *****************************************************************************************/
        public RegisterOutOfRangeException(string message, System.Exception inner) : base(message, inner) { }
    }
}
