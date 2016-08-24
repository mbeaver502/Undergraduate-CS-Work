using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**************************************************************************************************
 * 
 * Name: ExceededMaxInstructionException
 * 
 * ================================================================================================
 * 
 * Description: This class is an exception handler for programs that exceed the max number of 
 *              instructions.
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
    class ExceededMaxInstructionException : Exception
    {

        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        ExceededMaxInstructionException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       N/A   
         * Return:      N/A
         * Description: The default constructor.
         *              
         *****************************************************************************************/
        public ExceededMaxInstructionException() : base() { }

        /******************************************************************************************
         * 
         * Name:        ExceededMaxInstructionException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       The message to set as string.   
         * Return:      N/A
         * Description: The overloaded constructor that sets the message for the exception.
         *              
         *****************************************************************************************/
        public ExceededMaxInstructionException(string message) : base(message) { }

        /******************************************************************************************
         * 
         * Name:        ExceededMaxInstructionException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       The message to set as string and the inner exception.   
         * Return:      N/A
         * Description: The overloaded constructor that sets the message for the exception as well
         *              as the inner exception.
         *              
         *****************************************************************************************/
        public ExceededMaxInstructionException(string message, System.Exception inner) : base(message, inner) { }
    }
}
