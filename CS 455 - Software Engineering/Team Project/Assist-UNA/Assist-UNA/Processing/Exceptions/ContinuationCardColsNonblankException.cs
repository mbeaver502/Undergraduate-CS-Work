using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**************************************************************************************************
 * 
 * Name: ContinuationCardColsNonblankException
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
 * 04/24/2014   THH     Original version.
 *                       
 *************************************************************************************************/

namespace Assist_UNA
{
    class ContinuationCardColsNonblankException : Exception
    {

        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        ContinuationCardColsNonblankException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       N/A   
         * Return:      N/A
         * Description: The default constructor.
         *              
         *****************************************************************************************/
        public ContinuationCardColsNonblankException() : base() { }

        /******************************************************************************************
         * 
         * Name:        ContinuationCardColsNonblankException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       The message to set as string.   
         * Return:      N/A
         * Description: The overloaded constructor that sets the message for the exception.
         *              
         *****************************************************************************************/
        public ContinuationCardColsNonblankException(string message) : base(message) { }

        /******************************************************************************************
         * 
         * Name:        ContinuationCardColsNonblankException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       The message to set as string and the inner exception.   
         * Return:      N/A
         * Description: The overloaded constructor that sets the message for the exception as well
         *              as the inner exception.
         *              
         *****************************************************************************************/
        public ContinuationCardColsNonblankException(string message, System.Exception inner) : base(message, inner) { }
    }
}
