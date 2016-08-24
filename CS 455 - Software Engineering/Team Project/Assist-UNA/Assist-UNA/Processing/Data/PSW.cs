using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**************************************************************************************************
 * 
 * Name: PSW
 * 
 * ================================================================================================
 * 
 * Description: This class represents a portion of the ASSIST/UNA's simulator's Program Status
 *              Word (PSW). This class represents the location counter (instruction address) as an 
 *              array of LOC_SIZE bytes and the condition code as COND_CODE_SIZE integers (0 or 1).
 *                       
 * ================================================================================================        
 * 
 * Modification History
 * --------------------
 * 03/28/2014    JMB     Created class, members, and methods.
 * 03/28/2014    JMB     Updated all documentation.
 * 03/29/2014    JMB     Updated to work with AssistByte rather than Byte.
 * 04/03/2014    JMB     Corrected variables to conform to standard.
 * 04/05/2014    JMB     Reordered methods to conform to standards.
 * 04/06/2014    JMB     Updated certain integer values to be unsigned, which is more appropriate
 *                          to their roles and uses.
 *                    
 *************************************************************************************************/

namespace AssistData
{
    class PSW
    {
        /* Constants. */
        private const int DEFAULT_INVALID_INT = -128;
        private const string DEFAULT_BYTE_VALUE = "F4";
        private const uint COND_CODE_SIZE = 2;
        private const uint DEFAULT_COND_CODE_VALUE = 0;
        private const uint LOC_SIZE = 3;
        private const uint REGISTER_SIZE = 8;


        /* Private members. */
        private AssistByte[] locContents;
        private uint[] condCode;


        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        PSW
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A  
         * Return:      N/A   
         * Description: This default contructor initializes the PSW object to a default state.
         *              
         *****************************************************************************************/
        public PSW()
        {
            locContents = new AssistByte[LOC_SIZE];
            condCode = new uint[COND_CODE_SIZE];

            for (int i = 0; i < LOC_SIZE; i++)
                locContents[i] = new AssistByte();

            InitializeLOCContents();
            InitializeCondCodeContents();
        }

        /******************************************************************************************
         * 
         * Name:        GetBytes     
         * 
         * Author(s):   Michael Beaver
         *                         
         * Input:       The start and end are unsigned integers.   
         * Return:      The requested bytes are returned as an array of byte objects.    
         * Description: This method will return an array of byte objects. The array's range will be
         *              inclusive (so, [start, end]). If an error occurs, the returned result will
         *              be null. Otherwise, the returned result will be the array of byte objects.
         *                      
         *****************************************************************************************/
        public AssistByte[] GetBytes(uint start, uint end)
        {
            AssistByte[] result = new AssistByte[end - start + 1];
            uint i = 0;
            uint j = start;
            
            /* Do not use a negative range or invalid values. */
            if (end < start || start < 0 || start > LOC_SIZE || end < 0 || end > LOC_SIZE)
                result =  null;

            else
            {
                /* Attempt to copy the values. */
                try
                {
                    while (j <= end)
                    {
                        result[i] = locContents[j];

                        i++;
                        j++;
                    }
                }

                /* Index was out of bounds. */
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("Error: Index out of bounds!");

                    result = null;
                }
            }
            
            return result;
        }

        /******************************************************************************************
         * 
         * Name:        SetByte    
         * 
         * Author(s):   Michael Beaver      
         *              
         * Input:       The index is an unsigned integer, and value is an integer.   
         * Return:      The result is boolean.
         * Description: This method will attempt to set a byte with specified location ("index") to
         *              a new value ("value"). If an error occurs, the returned result will be
         *              false. Otherwise, the returned result will be true.
         *              
         *****************************************************************************************/
        public bool SetByte(uint index, int value)
        {
            bool result = false;

            /* Attempt to alter the byte's value. */
            try
            {
                //locContents[index] = Byte.Parse(value.ToString());
                result = locContents[index].SetValue(value);
            }

            /* The index was out of bounds. */
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Error: Index out of bounds!");

                result = false;
            }

            return result;
        }

        /******************************************************************************************
         * 
         * Name:        SetByte     
         * 
         * Author(s):   Michael Beaver     
         *              
         * Input:       The index is an unsigned integer, and the value is a string.    
         * Return:      The result is boolean.
         * Description: This method will attempt to set a byte with specified location ("index") to
         *              a new value ("value"). The value in "value" should be hexadecimal. If an 
         *              error occurs, the returned result will be false. Otherwise, the returned 
         *              result will be true. 
         *                          
         *****************************************************************************************/
        public bool SetByte(uint index, string value)
        {
            bool result = false;
            int tempVal;

            /* Attempt to set the byte to the new value. */
            try
            {
                tempVal = Convert.ToSByte(value, 16);
                //locContents[index] = Byte.Parse(tempVal.ToString());
                result = locContents[index].SetValue(tempVal);
            }

            /* A value could not be converted. */
            catch (OverflowException)
            {
                Console.WriteLine("Error: Overflow during conversion!");

                result = false;
            }

            /* The value is not in a valid format. */
            catch (FormatException)
            {
                Console.WriteLine("Error: Invalid format!");

                result = false;
            }

            /* An index was out of bounds. */
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Error: Index out of bounds!");

                result = false;
            }

            return result;
        }

        /******************************************************************************************
         * 
         * Name:        SetBytes    
         * 
         * Author(s):   Michael Beaver  
         *                         
         * Input:       The index is an unsigned integer, and values is an array of integers.   
         * Return:      The result is boolean.     
         * Description: This method will attempt to set a contiguous sequence of bytes starting at
         *              "index" to a series of values specified by the "values" array. If an error
         *              occurs, the returned result will be false. Otherwise, the returned result
         *              will be true.
         *              
         *****************************************************************************************/
        public bool SetBytes(uint index, int[] values)
        {
            bool result = false;
            uint offset = 0;

            /* Bounds checking. */
            if (index < 0 || index > LOC_SIZE)
                result = false;

            else
            {
                /* Attempt to set the sequence of bytes. */
                try
                {
                    foreach (int val in values)
                    {
                        //locContents[index + offset] = Byte.Parse(val.ToString());
                        result = locContents[index + offset].SetValue(val);

                        if (index + offset == LOC_SIZE)
                            break;

                        offset++;
                    }
                }

                /* An index was out of bounds. */
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("Error: Index out of bounds!");

                    result = false;
                }
            }

            return result;
        }

        /******************************************************************************************
         * 
         * Name:        SetBytes     
         * 
         * Author(s):   Michael Beaver  
         *                         
         * Input:       The index is an unsigned integer, and values is an array of strings.
         * Return:      The result is boolean.
         * Description: This method will attempt to set a contiguous sequence of bytes starting at
         *              "index" to a series of values specified by the "values" array. The values
         *              in the "values" array should be in hexadecimal representation. If an error
         *              occurs, the returned result will be false. Otherwise, the returned result
         *              will be true.
         *                
         *****************************************************************************************/
        public bool SetBytes(uint index, string[] values)
        {
            bool result = false;
            int tempVal;
            uint offset = 0;

            /* Bounds checking. */
            if (index < 0 || index > LOC_SIZE)
                result = false;

            else
            {
                /* Attempt to set the bytes to the new values. */
                try
                {
                    foreach (string val in values)
                    {
                        tempVal = Convert.ToSByte(val, 16);
                        //locContents[index + offset] = Byte.Parse(tempVal.ToString());
                        result = locContents[index + offset].SetValue(tempVal);

                        if (index + offset == LOC_SIZE)
                            break;

                        offset++;
                    }
                }

                /* A value could not be converted. */
                catch (OverflowException)
                {
                    Console.WriteLine("Error: Overflow during conversion!");

                    result = false;
                }

                /* The value is not in a valid format. */
                catch (FormatException)
                {
                    Console.WriteLine("Error: Invalid format!");

                    result = false;
                }

                /* An index was out of bounds. */
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("Error: Index out of bounds!");

                    result = false;
                }
            }

            return result;
        }

        /******************************************************************************************
         * 
         * Name:        SetCondCode     
         * 
         * Author(s):   Michael Beaver 
         *                       
         * Input:       The value is an unsigned integer.      
         * Return:      The result is boolean. 
         * Description: This method will attempt to update the condition code bits based on a 
         *              specified integer value ("value"). The condition code must be in the range
         *              [0, 3]. If an error occurs, the returned result will be false. Otherwise,
         *              the returned result will be true.
         *                 
         *****************************************************************************************/
        public bool SetCondCode(uint value)
        {
            bool result = false;

            /* Update the condition code based on the given value. */
            switch (value)
            {
                case 0:
                    {
                        condCode[0] = 0;
                        condCode[1] = 0;

                        result = true;
                        break;
                    }

                case 1:
                    {
                        condCode[0] = 0;
                        condCode[1] = 1;

                        result = true;
                        break;
                    }

                case 2:
                    {
                        condCode[0] = 1;
                        condCode[1] = 0;

                        result = true;
                        break;
                    }

                case 3:
                    {
                        condCode[0] = 1;
                        condCode[1] = 1;

                        result = true;
                        break;
                    }

                /* Invalid value specified. */
                default:
                    {
                        result = false;
                        break;
                    }
            }

            return result;
        }

        /******************************************************************************************
         * 
         * Name:        SetCondCode     
         * 
         * Author(s):   Michael Beaver 
         *                       
         * Input:       High and low are unsigned integers.      
         * Return:      The result is boolean. 
         * Description: This method will attempt to update the condition code bits based on  
         *              specified integer values ("high" and "low"). The condition code must be in 
         *              the range [0, 3]. This means "high" and "low" must be either 0 or 1. If an 
         *              error occurs, the returned result will be false. Otherwise, the returned 
         *              result will be true.
         *                 
         *****************************************************************************************/
        public bool SetCondCode(uint high, uint low)
        {
            bool result;

            /* Do not use invalid values. */
            if (high < 0 || high > 1 || low < 0 || low > 1)
                result = false;

            /* Update the condition code based on the given value. */
            else
            {
                condCode[0] = high;
                condCode[1] = low;

                result = true;
            }

            return result;
        }

        /******************************************************************************************
         * 
         * Name:        GetBytesInt     
         * 
         * Author(s):   Michael Beaver 
         *                       
         * Input:       The start and end values are unsigned integers.      
         * Return:      The integer value of the bytes is an integer.  
         * Description: This method will return the integer value of a specified inclusive range
         *              of bytes (so, [start, end]). If an error occurs, the returned result will
         *              be -1. Otherwise, the returned result will be the integer value.
         *                 
         *****************************************************************************************/
        public int GetBytesInt(uint start, uint end)
        {
            int result;
            string bytesString;

            /* Get the sequence of bytes as a string. */
            bytesString = GetBytesString(start, end);

            /* The sequence of bytes does not exist, or an error occurred. */
            if (bytesString == null)
                result = DEFAULT_INVALID_INT;

            /* Actually calculate the integer value, if possible. */
            else
                result = Convert.ToInt32(bytesString, 16);

            return result;
        }

        /******************************************************************************************
         * 
         * Name:        GetCondCodeInt     
         * 
         * Author(s):   Michael Beaver 
         *                       
         * Input:       N/A      
         * Return:      The result is an unsigned integer.  
         * Description: This method will return the integer value of the condition code.
         *                 
         *****************************************************************************************/
        public uint GetCondCodeInt()
        {
            uint result;

            /* 00 or 01 implies 0 or 1, respectively. */
            if (condCode[0] == 0)
            {
                if (condCode[1] == 0)
                    result = 0;

                else
                    result = 1;
            }

            /* 10 or 11 implies 2 or 3, respectively. */
            else
            {
                if (condCode[1] == 0)
                    result = 2;

                else
                    result = 3;
            }

            return result;
        }

        /******************************************************************************************
         * 
         * Name:        GetBytesString     
         * 
         * Author(s):   Michael Beaver         
         *              
         * Input:       The start and end values are unsigned integers. 
         * Return:      The hexadecimal bytes are a string.  
         * Description: This method will return the hexadecimal string of bytes of a specified
         *              inclusive range (so, [start, end]). If an error occurs, the returned
         *              result will be null. Otherwise, the returned result will be the 
         *              hexadecimal string.
         *                     
         *****************************************************************************************/
        public string GetBytesString(uint start, uint end)
        {
            string result = "";
            uint i = start;
            
            /* Do not use a negative range or invalid values. */
            if (end < start || start < 0 || start > LOC_SIZE || end < 0 || end > LOC_SIZE)
                result = null;

            else
            {
                /* Create the hexadecimal string. */
                try
                {
                    while (i <= end)
                    {
                        result += locContents[i].GetHex();

                        i++;
                    }
                }

                /* Index was out of bounds. */
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("Error: Index out of bounds!");

                    result = null;
                }
            }

            return result;
        }


        /* Private methods. */

        /******************************************************************************************
         * 
         * Name:        InitializeCondCodeContents    
         * 
         * Author(s):   Michael Beaver 
         *                       
         * Input:       N/A
         * Return:      N/A
         * Description: This method is used by the class constructor(s) to initialize the condition
         *              code contents to the DEFAULT_COND_CODE_VALUE. The DEFAULT_COND_CODE_VALUE 
         *              should be 0 or 1.              
         *              
         *****************************************************************************************/
        private void InitializeCondCodeContents()
        {
            for (uint i = 0; i < COND_CODE_SIZE; i++)
                condCode[i] = DEFAULT_COND_CODE_VALUE;
        }

        /******************************************************************************************
         * 
         * Name:        InitializeLOCContents    
         * 
         * Author(s):   Michael Beaver 
         *                       
         * Input:       N/A
         * Return:      N/A
         * Description: This method is used by the class constructor(s) to initialize the LOC
         *              contents to the DEFAULT_BYTE_VALUE. The DEFAULT_BYTE_VALUE should be
         *              in hexadecimal.              
         *              
         *****************************************************************************************/
        private void InitializeLOCContents()
        {
            string tempVal;

            /* Set all bytes in memory to the default byte value. */
            for (uint i = 0; i < LOC_SIZE; i++)
            {
                tempVal = Convert.ToString(Convert.ToSByte(DEFAULT_BYTE_VALUE, 16));
                //locContents[i] = Byte.Parse(tempVal);
                locContents[i].SetValue(tempVal);
            }
        }

    }

}
