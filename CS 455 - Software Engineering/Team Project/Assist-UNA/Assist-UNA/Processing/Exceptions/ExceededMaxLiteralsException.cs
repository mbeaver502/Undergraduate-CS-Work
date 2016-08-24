using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**************************************************************************************************
 * 
 * Name: ExceededMaxLiteralsException
 * 
 * ================================================================================================
 * 
 * Description: This class is an exception handler for programs that exceed the max number of 
 *              lines.
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
    class ExceededMaxLiteralsException : Exception
    {

        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        ExceededMaxLiteralsException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       N/A   
         * Return:      N/A
         * Description: The default constructor.
         *              
         *****************************************************************************************/
        public ExceededMaxLiteralsException() : base() { }

        /******************************************************************************************
         * 
         * Name:        ExceededMaxLiteralsException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       The message to set as string.   
         * Return:      N/A
         * Description: The overloaded constructor that sets the message for the exception.
         *              
         *****************************************************************************************/
        public ExceededMaxLiteralsException(string message) : base(message) { }

        /******************************************************************************************
         * 
         * Name:        ExceededMaxLiteralsException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       The message to set as string and the inner exception.   
         * Return:      N/A
         * Description: The overloaded constructor that sets the message for the exception as well
         *              as the inner exception.
         *              
         *****************************************************************************************/
        public ExceededMaxLiteralsException(string message, System.Exception inner) : base(message, inner) { }
    }
}
