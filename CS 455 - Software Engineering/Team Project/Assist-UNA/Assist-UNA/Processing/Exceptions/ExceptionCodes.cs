using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**************************************************************************************************
 * 
 * Name: ExceptionCodes
 * 
 * ================================================================================================
 * 
 * Description: This class contains all the ASSIST/I codes for runtime exceptions.
 *                       
 * ================================================================================================        
 * 
 * Modification History
 * --------------------
 * 04/27/2014   JMB     Original version. Migrated from Library.
 *                       
 *************************************************************************************************/

namespace Assist_UNA
{
    static class ExceptionCodes
    {
        /* Constants. */
        public const string ADDRESSING_EXCEPTION = "0C5";
        public const string DATA_EXCEPTION = "0C7";
        public const string DEC_DIVIDE_EXCEPTION = "0CB";
        public const string DEC_OVERFLOW_EXCEPTION = "0CA";
        public const string FP_DIVIDE_EXCEPTION = "0C9";
        public const string FP_OVERFLOW_EXCEPTION = "0C8";
        public const string OPERATION_EXCEPTION = "0C1";
        public const string PROTECTION_EXCEPTION = "0C4";
        public const string SPECIFICATION_EXCEPTION = "0C6";
    }
}
