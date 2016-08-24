using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**************************************************************************************************
 * 
 * Name: ExceededMaxPagesException
 * 
 * ================================================================================================
 * 
 * Description: This class is an exception handler for programs that exceed the max number of 
 *              pages.
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
    class ExceededMaxPagesException : Exception
    {

        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        ExceededMaxPagesException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       N/A   
         * Return:      N/A
         * Description: The default constructor.
         *              
         *****************************************************************************************/
        public ExceededMaxPagesException() : base() { }

        /******************************************************************************************
         * 
         * Name:        ExceededMaxPagesException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       The message to set as string.   
         * Return:      N/A
         * Description: The overloaded constructor that sets the message for the exception.
         *              
         *****************************************************************************************/
        public ExceededMaxPagesException(string message) : base(message) { }

        /******************************************************************************************
         * 
         * Name:        ExceededMaxPagesException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       The message to set as string and the inner exception.   
         * Return:      N/A
         * Description: The overloaded constructor that sets the message for the exception as well
         *              as the inner exception.
         *              
         *****************************************************************************************/
        public ExceededMaxPagesException(string message, System.Exception inner) : base(message, inner) { }
    }
}
