using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**************************************************************************************************
 * 
 * Name: LabelRequiredException
 * 
 * ================================================================================================
 * 
 * Description: This class is an exception handler for missing delimiters.
 *                       
 * ================================================================================================        
 * 
 * Modification History
 * --------------------
 * 04/22/2014   THH     Original version.
 *                       
 *************************************************************************************************/

namespace Assist_UNA
{
    class LabelRequiredException : Exception
    {

        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        LabelRequiredException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       N/A   
         * Return:      N/A
         * Description: The default constructor.
         *              
         *****************************************************************************************/
        public LabelRequiredException() : base() { }

        /******************************************************************************************
         * 
         * Name:        LabelRequiredException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       The message to set as string.   
         * Return:      N/A
         * Description: The overloaded constructor that sets the message for the exception.
         *              
         *****************************************************************************************/
        public LabelRequiredException(string message) : base(message) { }

        /******************************************************************************************
         * 
         * Name:        LabelRequiredException        
         * 
         * Author(s):   Travis Hunt     
         *              
         * Input:       The message to set as string and the inner exception.   
         * Return:      N/A
         * Description: The overloaded constructor that sets the message for the exception as well
         *              as the inner exception.
         *              
         *****************************************************************************************/
        public LabelRequiredException(string message, System.Exception inner) : base(message, inner) { }
    }
}
