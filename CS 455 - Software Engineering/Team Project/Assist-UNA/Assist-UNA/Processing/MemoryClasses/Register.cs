using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**************************************************************************************************
 * 
 * Name: Register
 * 
 * ================================================================================================
 * 
 * Description: This class represents one of the ASSIST/UNA's simulator's registers. The register 
 *              contents are represented as an array of byte obejcts.          
 *                       
 * ================================================================================================        
 * 
 * Modification History
 * --------------------
 * 03/28/2014   JMB     Created class, members, and methods.
 * 03/28/2014   JMB     Updated all documentation.
 * 03/29/2014   JMB     Updated to work with AssistByte rather than Byte.
 * 04/03/2014   JMB     Corrected variables to conform to standard.
 * 04/05/2014   JMB     Reordered methods to conform to standards.
 * 04/06/2014   JMB     Updated certain integer values to be unsigned, which is more appropriate
 *                          to their roles and uses.
 * 04/08/2014   JMB     Added additional SetBytes method.
 * 04/10/2014   JMB     Fixed a fault in SetBytes(uint index, string value).
 * 04/22/2014   JMB     Updated to better conform to standards.
 *                      
 *************************************************************************************************/

namespace Assist_UNA
{
    class Register
    {
        /* Constants. */
        private const int DEFAULT_INVALID_INT = -128;
        private const string DEFAULT_BYTE_VALUE = "F4";
        private const uint NUM_HEX_DIGITS = 8;
        private const uint REGISTER_SIZE = 4;


        /* Private members. */
        private AssistByte[] registerContents;


        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        Register
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A  
         * Return:      N/A   
         * Description: This default constructor will create and initialize the register contents.
         *              The register contents will be of size REGISTER_SIZE.
         *              
         *****************************************************************************************/
        public Register()
        {
            /* Create and initialize the actual register contents array. */
            registerContents = new AssistByte[REGISTER_SIZE];

            for (uint i = 0; i < REGISTER_SIZE; i++)
                registerContents[i] = new AssistByte();

            InitializeRegisterContents();
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
            if (end < start || start < 0 || start > REGISTER_SIZE || end < 0 || end > REGISTER_SIZE)
                result = null;

            else
            {
                /* Attempt to copy the values. */
                try
                {
                    while (j <= end)
                    {
                        result[i] = registerContents[j];

                        i++;
                        j++;
                    }
                }

                /* Index was out of bounds. */
                catch (IndexOutOfRangeException)
                {
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
                result = registerContents[index].SetValue(value);
            }

            /* The index was out of bounds. */
            catch (IndexOutOfRangeException)
            {
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
                result = registerContents[index].SetValue(tempVal);
            }

            /* The value could not be converted. */
            catch (OverflowException)
            {
                result = false;
            }

            /* The value is not in a valid format. */
            catch (FormatException)
            {
                result = false;
            }

            /* The index is out of bounds. */
            catch (IndexOutOfRangeException)
            {
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
         * Input:       The index is an unsigned integer, and value is an integer.   
         * Return:      The result is boolean.     
         * Description: This method will attempt to set a contiguous sequence of bytes starting at
         *              "index" to the value specified by "value." If an error occurs, the returned 
         *              result will be false. Otherwise, the returned result will be true.
         *              
         *****************************************************************************************/
        public bool SetBytes(uint index, int value)
        {
            bool result = false;
            int i = 0;
            string hexValue;
            string tempByte;
            uint offset = 0;

            /* Bounds checking. */
            if (index < 0 || index > REGISTER_SIZE)
                result = false;

            else
            {
                hexValue = value.ToString("X").PadLeft(Convert.ToInt32(NUM_HEX_DIGITS), '0');

                /* Attempt to set the sequence of bytes. */
                try
                {
                    while (i < NUM_HEX_DIGITS)
                    {
                        tempByte = hexValue[i].ToString() + hexValue[i + 1].ToString();
                        result = this.SetByte(index + offset, tempByte);

                        /* Stop if there is an error. */
                        if (result == false)
                            break;

                        i += 2;
                        offset++;
                    }
                }

                /* An index was out of bounds. */
                catch (IndexOutOfRangeException)
                {
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
            if (index < 0 || index > REGISTER_SIZE)
                result = false;

            else
            {
                /* Attempt to set the sequence of bytes. */
                try
                {
                    foreach(int val in values)
                    {
                        result = registerContents[index + offset].SetValue(val);

                        if (index + offset == REGISTER_SIZE)
                            break;

                        offset++;
                    }
                }

                /* An index was out of bounds. */
                catch (IndexOutOfRangeException)
                {
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
         * Input:       The index is an unsigned integer, and value is a string.   
         * Return:      The result is boolean.     
         * Description: This method will attempt to set a contiguous sequence of bytes starting at
         *              "index" to the value specified by "value." If an error occurs, the returned 
         *              result will be false. Otherwise, the returned result will be true.
         *              
         *****************************************************************************************/
        public bool SetBytes(uint index, string value)
        {
            bool result = false;
            int i = 0;
            string hexString;
            string tempByte;
            uint offset = 0;

            /* Bounds checking. */
            if (index < 0 || index > REGISTER_SIZE)
                result = false;

            else
            {
                hexString = value.PadLeft((int)NUM_HEX_DIGITS, '0');

                /* Attempt to set the sequence of bytes. */
                try
                {
                    while (i < NUM_HEX_DIGITS)
                    {
                        tempByte = hexString[i].ToString() + hexString[i + 1].ToString();
                        result = this.SetByte(index + offset, tempByte);

                        /* Stop if there is an error. */
                        if (result == false)
                            break;

                        i += 2;
                        offset++;
                    }
                }

                /* An index was out of bounds. */
                catch (IndexOutOfRangeException)
                {
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
            if (index < 0 || index > REGISTER_SIZE)
                result = false;

            else
            {
                /* Attempt to set the bytes to the new values. */
                try
                {
                    foreach(string val in values)
                    {
                        tempVal = Convert.ToSByte(val, 16);
                        result = registerContents[index + offset].SetValue(tempVal);

                        if (index + offset == REGISTER_SIZE)
                            break;

                        offset++;
                    }
                }

                /* A value could not be converted. */
                catch (OverflowException)
                {
                    result = false;
                }

                /* The value is not in a valid format. */
                catch (FormatException)
                {
                    result = false;
                }

                /* An index was out of bounds. */
                catch (IndexOutOfRangeException)
                {
                    result = false;
                }
            }

            return result;
        }             

        /******************************************************************************************
         * 
         * Name:        GetByteInt     
         * 
         * Author(s):   Michael Beaver
         *              
         * Input:       The index is an unsigned integer.   
         * Return:      The integer value of the byte is an integer.   
         * Description: This method will return the integer value of a specified byte (given by
         *              "index"). If an error occurs, the returned result will be -1. Otherwise,
         *              the returned result will be the integer value of the byte.
         *              
         *****************************************************************************************/
        public int GetByteInt(uint index)
        {
            int result;

            /* Get the integer value. */
            try
            {
                result = registerContents[index].GetInt();
            }

            /* Index is out of bounds. */
            catch (IndexOutOfRangeException)
            {
                result = DEFAULT_INVALID_INT;
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
         * Name:        GetByteHex     
         * 
         * Author(s):   Michael Beaver
         *                          
         * Input:       The index is an unsigned integer.    
         * Return:      The hexadecimal value is a string.  
         * Description: This method will return the hexadecimal representation of a specified 
         *              byte (given by "index"). If an error occurs, the returned result will be
         *              null. Otherwise, the returned result will be the hexadecimal string.
         *                          
         *****************************************************************************************/
        public string GetByteHex(uint index)
        {
            string result;

            /* Get the hexadecimal representation. */
            try
            {
                result = registerContents[index].GetHex();
            }

            /* Index is out of bounds. */
            catch (IndexOutOfRangeException)
            {
                result = null;
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
            if (end < start || start < 0 || start > REGISTER_SIZE || end < 0 || end > REGISTER_SIZE)
                result = null;

            else
            {
                /* Create the hexadecimal string. */
                try
                {
                    while (i <= end)
                    {
                        result += registerContents[i].GetHex();

                        i++;
                    }
                }

                /* Index was out of bounds. */
                catch (IndexOutOfRangeException)
                {
                    result = null;
                }
            }
            
            return result;
        }     


        /* Private methods. */

        /******************************************************************************************
         * 
         * Name:        InitializeRegisterContents    
         * 
         * Author(s):   Michael Beaver 
         *                       
         * Input:       N/A
         * Return:      N/A
         * Description: This method is used by the class constructor(s) to initialize the register
         *              contents to the DEFAULT_BYTE_VALUE. The DEFAULT_BYTE_VALUE should be
         *              in hexadecimal.              
         *              
         *****************************************************************************************/
        private void InitializeRegisterContents()
        {
            string tempVal;

            /* Set all bytes in memory to the default byte value. */
            for (uint i = 0; i < REGISTER_SIZE; i++)
            {
                tempVal = Convert.ToString(Convert.ToSByte(DEFAULT_BYTE_VALUE, 16));
                registerContents[i].SetValue(tempVal);
            }
        }

    }

}
