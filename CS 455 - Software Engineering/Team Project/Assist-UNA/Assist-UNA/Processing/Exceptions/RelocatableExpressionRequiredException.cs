using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**************************************************************************************************
 * 
 * Name: RelocatableExpressionRequiredException
 * 
 * ================================================================================================
 * 
 * Description: This class is an exception handler for missing delimiters.
 *                       
 * ================================================================================================        
 * 
 * Modification History
 * --------------------
 * 04/23/2014   THH     Original version.
 *                       
 *************************************************************************************************/

namespace Assist_UNA
{
    class RelocatableExpressionRequiredException : Exception
    {

        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        RelocatableExpressionRequiredException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       N/A   
         * Return:      N/A
         * Description: The default constructor.
         *              
         *****************************************************************************************/
        public RelocatableExpressionRequiredException() : base() { }

        /******************************************************************************************
         * 
         * Name:        RelocatableExpressionRequiredException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       The message to set as string.   
         * Return:      N/A
         * Description: The overloaded constructor that sets the message for the exception.
         *              
         *****************************************************************************************/
        public RelocatableExpressionRequiredException(string message) : base(message) { }

        /******************************************************************************************
         * 
         * Name:        RelocatableExpressionRequiredException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       The message to set as string and the inner exception.   
         * Return:      N/A
         * Description: The overloaded constructor that sets the message for the exception as well
         *              as the inner exception.
         *              
         *****************************************************************************************/
        public RelocatableExpressionRequiredException(string message, System.Exception inner) : base(message, inner) { }
    }
}
