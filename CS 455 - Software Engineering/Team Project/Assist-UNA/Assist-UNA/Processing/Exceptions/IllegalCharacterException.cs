using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**************************************************************************************************
 * 
 * Name: IllegalCharacterException
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
    class IllegalCharacterException : Exception
    {

        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        IllegalCharacterException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       N/A   
         * Return:      N/A
         * Description: The default constructor.
         *              
         *****************************************************************************************/
        public IllegalCharacterException() : base() { }

        /******************************************************************************************
         * 
         * Name:        IllegalCharacterException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       The message to set as string.   
         * Return:      N/A
         * Description: The overloaded constructor that sets the message for the exception.
         *              
         *****************************************************************************************/
        public IllegalCharacterException(string message) : base(message) { }

        /******************************************************************************************
         * 
         * Name:        IllegalCharacterException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       The message to set as string and the inner exception.   
         * Return:      N/A
         * Description: The overloaded constructor that sets the message for the exception as well
         *              as the inner exception.
         *              
         *****************************************************************************************/
        public IllegalCharacterException(string message, System.Exception inner) : base(message, inner) { }
    }
}
