using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**************************************************************************************************
 * 
 * Name: EndProcessingException
 * 
 * ================================================================================================
 * 
 * Description: This class contains an empty basic exception throw to trigger shutdown upon a fatal
 *              exception within the Simulator. This class should be used via the ErrorDetection
 *              class. 
 *                       
 * ================================================================================================        
 * 
 * Modification History
 * --------------------
 * 04/27/2014   JMB     Original version (file created). Code originally implemented by CAF.
 *                          Migrated from Library.
 *                       
 *************************************************************************************************/

namespace Assist_UNA
{
    class EndProcessingException : Exception
    {
        /* Empty throw. Basic default exception. */
    }
}
