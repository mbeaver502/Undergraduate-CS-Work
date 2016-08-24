using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**************************************************************************************************
 * 
 * Name: IllegalDuplicationFactorException
 * 
 * ================================================================================================
 * 
 * Description: This class is an exception handler for illegal constant types.
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
    class IllegalDuplicationFactorException : Exception
    {

        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        IllegalDuplicationFactorException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       N/A   
         * Return:      N/A
         * Description: The default constructor.
         *              
         *****************************************************************************************/
        public IllegalDuplicationFactorException() : base() { }

        /******************************************************************************************
         * 
         * Name:        IllegalDuplicationFactorException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       The message to set as string.   
         * Return:      N/A
         * Description: The overloaded constructor that sets the message for the exception.
         *              
         *****************************************************************************************/
        public IllegalDuplicationFactorException(string message) : base(message) { }

        /******************************************************************************************
         * 
         * Name:        IllegalDuplicationFactorException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       The message to set as string and the inner exception.   
         * Return:      N/A
         * Description: The overloaded constructor that sets the message for the exception as well
         *              as the inner exception.
         *              
         *****************************************************************************************/
        public IllegalDuplicationFactorException(string message, System.Exception inner) : base(message, inner) { }
    }
}
