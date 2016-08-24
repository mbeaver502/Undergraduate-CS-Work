using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**************************************************************************************************
 * 
 * Name: InvalidSymbolException
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
    class InvalidSymbolException : Exception
    {

        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        InvalidSymbolException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       N/A   
         * Return:      N/A
         * Description: The default constructor.
         *              
         *****************************************************************************************/
        public InvalidSymbolException() : base() { }

        /******************************************************************************************
         * 
         * Name:        InvalidSymbolException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       The message to set as string.   
         * Return:      N/A
         * Description: The overloaded constructor that sets the message for the exception.
         *              
         *****************************************************************************************/
        public InvalidSymbolException(string message) : base(message) { }

        /******************************************************************************************
         * 
         * Name:        InvalidSymbolException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       The message to set as string and the inner exception.   
         * Return:      N/A
         * Description: The overloaded constructor that sets the message for the exception as well
         *              as the inner exception.
         *              
         *****************************************************************************************/
        public InvalidSymbolException(string message, System.Exception inner) : base(message, inner) { }
    }
}
