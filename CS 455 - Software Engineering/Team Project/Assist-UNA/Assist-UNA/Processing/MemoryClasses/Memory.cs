using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**************************************************************************************************
 * 
 * Name: Memory
 * 
 * ================================================================================================
 * 
 * Description: This class represents the ASSIST/UNA's simulator's memory. The memory contents are
 *              represented as an array of byte obejcts.            
 *                       
 * ================================================================================================        
 * 
 * Modification History
 * --------------------
 * 03/27/2014   JMB     Created class, members, and methods.
 * 03/28/2014   JMB     Updated documentation for class and each method.
 * 03/29/2014   JMB     Updated to work with AssistByte rather than Byte. Added LockByte and
 *                           UnlockByte methods.
 * 04/03/2014   JMB     Corrected variables to conform to standard.
 * 04/05/2014   JMB     Reordered methods to conform to standards.
 * 04/06/2014   JMB     Updated certain integer values to be unsigned, which is more appropriate
 *                          to their roles and uses.
 * 04/08/2014   JMB     Added new SetBytes method.
 * 04/22/2014   JMB     Updated to better conform to standards.
 *                      
 *************************************************************************************************/

namespace Assist_UNA
{
    class Memory
    {
        /* Constants. */
        private const int DEFAULT_INVALID_INT = -128;
        private const string DEFAULT_BYTE_VALUE = "F5";
        private const uint MAX_MEMORY_SIZE = 9999;
        private const uint MIN_MEMORY_SIZE = 256;


        /* Private members. */
        private AssistByte[] memoryContents;
        private uint memorySize;
       

        /* Public methods. */

        /******************************************************************************************
         * 
         * Name:        Memory
         * 
         * Author(s):   Michael Beaver
         *                        
         * Input:       N/A  
         * Return:      N/A   
         * Description: Default constructor creates and initializes an array of bytes of size
         *              MIN_MEMORY_SIZE.
         *              
         *****************************************************************************************/
        public Memory()
        {
            memorySize = MIN_MEMORY_SIZE;

            /* Create and initialize the actual memory contents array. */
            memoryContents = new AssistByte[memorySize];

            for (uint i = 0; i < memorySize; i++)
                memoryContents[i] = new AssistByte();

            InitializeMemoryContents();
        }

        /******************************************************************************************
         * 
         * Name:        Memory 
         * 
         * Author(s):   Michael Beaver        
         *              
         * Input:       The size is an unsigned integer.     
         * Return:      N/A  
         * Description: The overloaded constructor that allows the user to specify the size of the
         *              memory contents (the "size" parameter). The constructor will not use a size
         *              smaller than MIN_MEMORY_SIZE or larger than MAX_MEMORY_SIZE. The memory
         *              contents are created and initialized.
         *              
         *****************************************************************************************/
        public Memory(uint size)
        {
            /* Check memory size bounds. */
            if (size < MIN_MEMORY_SIZE)
                memorySize = MIN_MEMORY_SIZE;

            else if (size > MAX_MEMORY_SIZE)
                memorySize = MAX_MEMORY_SIZE;

            else
                memorySize = size;

            /* Create and initialize the actual memory contents array. */
            memoryContents = new AssistByte[memorySize];

            for (uint i = 0; i < memorySize; i++)
                memoryContents[i] = new AssistByte();

            InitializeMemoryContents();
        }

        /******************************************************************************************
         * 
         * Name:        GetByte   
         * 
         * Author(s):   Michael Beaver 
         *                   
         * Input:       The index is an unsigned integer.      
         * Return:      The requested byte is a byte.
         * Description: This method will find in the memory contents a byte specified by location
         *              "index" and return it as a byte object.
         *                         
         *****************************************************************************************/
        public AssistByte GetByte(uint index)
        {
            AssistByte result = new AssistByte();

            /* Attempt to retrieve the byte value at the given location. */
            try
            {
                result = memoryContents[index];
            }
                 
            /* Index was out of bounds. */
            catch (IndexOutOfRangeException)
            {
                result = null;
            }

            return result;
        }

        /******************************************************************************************
         * 
         * Name:        GetBytes     
         * 
         * Author(s):   Michael Beaver
         *                         
         * Input:       The start and end are unsigned integers.   
         * Return:      The requested bytes are returned as an array of byte objects.    
         * Description: This method will locate and return an array of requested byte objects. The
         *              range used is inclusive (so, [start, end]). If there is an error, the 
         *              return value will be null.
         *                      
         *****************************************************************************************/
        public AssistByte[] GetBytes(uint start, uint end)
        {
            /* Inclusive range. */
            AssistByte[] result = new AssistByte[end - start + 1];
            uint i = 0;
            uint j = start;

            /* Do not use a negative range. */
            if (end < start)
                result = null;

            /* Attempt to copy the values. */
            else
            {
                try
                {
                    while (j <= end)
                    {
                        result[i] = memoryContents[j];

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
         * Name:        LockByte    
         * 
         * Author(s):   Michael Beaver
         *                    
         * Input:       The index is an unsigned integer.
         * Return:      The result is boolean.
         * Description: This method will lock the byte in memory specified by the given address
         *              ("index"). If an error occurs, the returned result will be false. Otherwise,
         *              the returned result will be true.
         *                   
         *****************************************************************************************/
        public bool LockByte(uint index)
        {
            bool result = false;

            try
            {
                /* The byte is already locked. */
                if (memoryContents[index].IsLocked())
                    result = true;

                /* Lock the byte. */
                else
                {
                    memoryContents[index].ToggleLock();
                    result = true;
                }
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
         * Input:       The index is an unsigned integer, and value is an integer.   
         * Return:      The result is boolean.
         * Description: This method will attempt to set a byte in memory to a specified integer
         *              value. The specific byte's location is given by "index." If an error
         *              occurs, the returned result will be false. Otherwise the returned result
         *              will be true.          
         *              
         *****************************************************************************************/
        public bool SetByte(uint index, int value)
        {
            bool result = false;

            /* Attempt to alter the byte's value. */
            try
            {
                result = memoryContents[index].SetValue(value);
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
         * Input:       The index is an unsigned integer, value is an integer, and the lockFlag 
         *                  is boolean. 
         * Return:      The result is boolean.
         * Description: This method will attempt to set a byte in memory to a specified integer
         *              value. The specific byte's location is given by "index." The "lockFlag"
         *              specifies whether or not the byte is to be locked. If an error occurs, the 
         *              returned result will be false. Otherwise the returned result will be true.          
         *              
         *****************************************************************************************/
        public bool SetByte(uint index, int value, bool lockFlag)
        {
            bool result = false;

            /* Attempt to alter the byte's value. */
            try
            {
                result = memoryContents[index].SetValue(value);

                /* Update the byte lock. */
                if (lockFlag == true && memoryContents[index].IsLocked() == false)
                    memoryContents[index].ToggleLock();

                else if (lockFlag == false && memoryContents[index].IsLocked() == true)
                    memoryContents[index].ToggleLock();
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
         * Description: This method will attempt to set a byte's value to a new hexadecimal 
         *              value specified by the user ("value"). The byte's location in memory is 
         *              given by "index." If an error occurs, the returned result will be false. 
         *              Otherwise, the returned result will be true.
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
                result = memoryContents[index].SetValue(tempVal);
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
         * Name:        SetByte     
         * 
         * Author(s):   Michael Beaver     
         *              
         * Input:       The index is an unsigned integer, the value is a string, and the lockFlag 
         *                  is boolean.    
         * Return:      The result is boolean.
         * Description: This method will attempt to set a byte's value to a new hexadecimal 
         *              value specified by the user ("value"). The byte's location in memory is 
         *              given by "index." The "lockFlag" specifies whether or not the byte is to
         *              be locked. If an error occurs, the returned result will be false. 
         *              Otherwise, the returned result will be true.
         *                          
         *****************************************************************************************/
        public bool SetByte(uint index, string value, bool lockFlag)
        {
            bool result = false;
            int tempVal;

            /* Attempt to set the byte to the new value. */
            try
            {
                tempVal = Convert.ToSByte(value, 16);
                result = memoryContents[index].SetValue(tempVal);

                /* Update the byte lock. */
                if (lockFlag == true && memoryContents[index].IsLocked() == false)
                    memoryContents[index].ToggleLock();

                else if (lockFlag == false && memoryContents[index].IsLocked() == true)
                    memoryContents[index].ToggleLock();
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
         * Description: This method will attempt to set a sequence of bytes (starting at "index")
         *              in memory to new values specified in the array "values." This method
         *              assumes the values are in sequence (i.e., contiguous). If an error occurs,
         *              the returned result will be false. Otherwise, the returned result will be
         *              true.
         *              
         *****************************************************************************************/
        public bool SetBytes(uint index, int[] values)
        {
            bool result = false;
            uint offset = 0;

            /* Attempt to set the sequence of bytes. */
            try
            {
                foreach(int val in values)
                {
                    result = memoryContents[index + offset].SetValue(val);
                    offset++;
                }
            }

            /* An index was out of bounds. */
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
         * Input:       The index is an unsigned integer, values is an array of integers, and the 
         *                  lockFlag is boolean.
         * Return:      The result is boolean.     
         * Description: This method will attempt to set a sequence of bytes (starting at "index")
         *              in memory to new values specified in the array "values." This method
         *              assumes the values are in sequence (i.e., contiguous). The "lockFlag"
         *              specifies whether or not the bytes are to be locked. If an error occurs,
         *              the returned result will be false. Otherwise, the returned result will be
         *              true.
         *              
         *****************************************************************************************/
        public bool SetBytes(uint index, int[] values, bool lockFlag)
        {
            bool result = false;
            uint offset = 0;

            /* Attempt to set the sequence of bytes. */
            try
            {
                foreach(int val in values)
                {
                    result = memoryContents[index + offset].SetValue(val);

                    /* Update the byte lock. */
                    if (lockFlag == true && memoryContents[index + offset].IsLocked() == false)
                        memoryContents[index + offset].ToggleLock();

                    else if (lockFlag == false && memoryContents[index + offset].IsLocked() == true)
                        memoryContents[index + offset].ToggleLock();

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

            return result;
        }

        /******************************************************************************************
         * 
         * Name:        SetBytes      
         * 
         * Author(s):   Michael Beaver 
         *                       
         * Input:       Indices is an array of unsigned integers, and values is an array of integers.    
         * Return:      The result is boolean. 
         * Description: This method will attempt to set bytes to new values specified in the
         *              "values" array. The locations of the bytes are given in the "indices"
         *              array. The bytes do NOT have to be contiguous in memory. The counts of the 
         *              "indices" and "values" arrays must be equal, or an error will result. If 
         *              an error occurs, the returned result will be false. Otherwise, the returned 
         *              result will be true.
         *                      
         *****************************************************************************************/
        public bool SetBytes(uint[] indices, int[] values)
        {
            bool result = false;
            uint currentIndex;
            uint i = 0;

            /* Check if the arrays are equal in size. */
            if (indices.Count() != values.Count())
                result = false;

            else
            {
                /* Attempt to set the bytes to the new values. */
                try
                {
                    foreach(int val in values)
                    {
                        currentIndex = indices[i];
                        result = memoryContents[currentIndex].SetValue(val);

                        i++;
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
         * Input:       Indices is an array of unsigned integers, values is an array of integers, 
         *                  and lockFlag is boolean.
         * Return:      The result is boolean. 
         * Description: This method will attempt to set bytes to new values specified in the
         *              "values" array. The locations of the bytes are given in the "indices"
         *              array. The bytes do NOT have to be contiguous in memory. The counts of the 
         *              "indices" and "values" arrays must be equal, or an error will result. The
         *              "lockFlag" specifies whether or not the bytes are to be locked. If an
         *              error occurs, the returned result will be false. Otherwise, the returned 
         *              result will be true.
         *                      
         *****************************************************************************************/
        public bool SetBytes(uint[] indices, int[] values, bool lockFlag)
        {
            bool result = false;
            uint currentIndex;
            uint i = 0;

            /* Check if the arrays are equal in size. */
            if (indices.Count() != values.Count())
                result = false;

            else
            {
                /* Attempt to set the bytes to the new values. */
                try
                {
                    foreach(int val in values)
                    {
                        currentIndex = indices[i];
                        result = memoryContents[currentIndex].SetValue(val);

                        /* Update the byte lock. */
                        if (lockFlag == true && memoryContents[currentIndex].IsLocked() == false)
                            memoryContents[currentIndex].ToggleLock();

                        else if ((!lockFlag) && memoryContents[currentIndex].IsLocked() == true)
                            memoryContents[currentIndex].ToggleLock();

                        i++;
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
         * Name:        SetBytes     
         * 
         * Author(s):   Michael Beaver  
         *                         
         * Input:       The index is an unsigned integer, and value is a string.
         * Return:      The result is boolean.
         * Description: This method will attempt to set a contiguous sequence of bytes in memory
         *              to new, user-specified value ("value"). The location of the first byte
         *              is given by "index." The value in "value" should be hexadecimal. If an
         *              error occurs, the returned result will be false. Otherwise, the returned
         *              result will be true.
         *                
         *****************************************************************************************/
        public bool SetBytes(uint index, string value)
        {
            bool result = false;
            int i = 0;
            int length = 0;
            int tempVal;
            string temp;
            uint offset = 0;

            /* Attempt to set the bytes to the new values. */
            try
            {
                length = value.Length;
                while (i < length)
                {
                    temp = value[i].ToString() + value[i + 1].ToString();
                    tempVal = Convert.ToSByte(temp, 16);
                    result = memoryContents[index + offset].SetValue(tempVal);

                    if (result == false)
                        break;

                    i += 2;
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
         * Description: This method will attempt to set a contiguous sequence of bytes in memory
         *              to new, user-specified values ("values"). The location of the first byte
         *              is given by "index." The values in "values" should be hexadecimal. If an
         *              error occurs, the returned result will be false. Otherwise, the returned
         *              result will be true.
         *                
         *****************************************************************************************/
        public bool SetBytes(uint index, string[] values)
        {
            bool result = false;
            int tempVal;
            uint offset = 0;

            /* Attempt to set the bytes to the new values. */
            try
            {
                foreach(string val in values)
                {
                    tempVal = Convert.ToSByte(val, 16);
                    result = memoryContents[index + offset].SetValue(tempVal);

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

            return result;
        }

        /******************************************************************************************
         * 
         * Name:        SetBytes     
         * 
         * Author(s):   Michael Beaver  
         *                         
         * Input:       The index is an unsigned integer, values is an array of strings, and 
         *                  lockFlag is boolean.
         * Return:      The result is boolean.
         * Description: This method will attempt to set a contiguous sequence of bytes in memory
         *              to new, user-specified values ("values"). The location of the first byte
         *              is given by "index." The values in "values" should be hexadecimal. The 
         *              "lockFlag" specifies whether or not the bytes are to be locked. If an
         *              error occurs, the returned result will be false. Otherwise, the returned
         *              result will be true.
         *                
         *****************************************************************************************/
        public bool SetBytes(uint index, string[] values, bool lockFlag)
        {
            bool result = false;
            int tempVal;
            uint offset = 0;

            /* Attempt to set the bytes to the new values. */
            try
            {
                foreach(string val in values)
                {
                    tempVal = Convert.ToSByte(val, 16);
                    result = memoryContents[index + offset].SetValue(tempVal);

                    /* Update the byte lock. */
                    if (lockFlag == true && memoryContents[index + offset].IsLocked() == false)
                        memoryContents[index + offset].ToggleLock();

                    else if ((!lockFlag) && memoryContents[index + offset].IsLocked() == true)
                        memoryContents[index + offset].ToggleLock();

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

            return result;
        }

        /******************************************************************************************
         * 
         * Name:        SetBytes    
         * 
         * Author(s):   Michael Beaver
         *                    
         * Input:       Indices is an array of unsigned integers, and values is an array of strings.   
         * Return:      The result is boolean.
         * Description: This method will attempt to set bytes to new values. The bytes' locations
         *              are given by the "indices" array and do not need to be contiguous. The new
         *              values are given by the "values" array and should be in hexadecimal. The
         *              sizes of the "indices" and "values" arrays must be equal. If an error
         *              occurs, the returned result will be false. Otherwise, the returned result
         *              will be true.
         *                   
         *****************************************************************************************/
        public bool SetBytes(uint[] indices, string[] values)
        {
            bool result = false;
            int tempVal;
            uint currentIndex;
            uint i = 0;

            /* Make sure the arrays are the same size. */
            if (indices.Count() != values.Count())
                result = false;

            else
            {
                /* Attempt to set the bytes to their new values. */
                try
                {
                    foreach(string val in values)
                    {
                        currentIndex = indices[i];
                        tempVal = Convert.ToSByte(val, 16);
                        result = memoryContents[currentIndex].SetValue(tempVal);

                        i++;
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
         * Name:        SetBytes    
         * 
         * Author(s):   Michael Beaver
         *                    
         * Input:       Indices is an array of unsigned integers, values is an array of strings, 
         *                  and lockFlag is boolean.
         * Return:      The result is boolean.
         * Description: This method will attempt to set bytes to new values. The bytes' locations
         *              are given by the "indices" array and do not need to be contiguous. The new
         *              values are given by the "values" array and should be in hexadecimal. The
         *              sizes of the "indices" and "values" arrays must be equal. The "lockFlag"
         *              specifies whether or not the bytes are to be locked. If an error occurs,
         *              the returned result will be false. Otherwise, the returned result will be
         *              true.
         *                   
         *****************************************************************************************/
        public bool SetBytes(uint[] indices, string[] values, bool lockFlag)
        {
            bool result = false;
            int tempVal;
            uint currentIndex;
            uint i = 0;

            /* Make sure the arrays are the same size. */
            if (indices.Count() != values.Count())
                result = false;

            else
            {
                /* Attempt to set the bytes to their new values. */
                try
                {
                    foreach(string val in values)
                    {
                        currentIndex = indices[i];
                        tempVal = Convert.ToSByte(val, 16);
                        result = memoryContents[currentIndex].SetValue(tempVal);

                        /* Update the byte lock. */
                        if (lockFlag == true && memoryContents[currentIndex].IsLocked() == false)
                            memoryContents[currentIndex].ToggleLock();

                        else if ((!lockFlag) && memoryContents[currentIndex].IsLocked() == true)
                            memoryContents[currentIndex].ToggleLock();

                        i++;
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
         * Name:        UnlockByte    
         * 
         * Author(s):   Michael Beaver
         *                    
         * Input:       The index is an unsigned integer.   
         * Return:      The result is boolean.
         * Description: This method will unlock the byte in memory at the given specified address
         *              ("index"). If an error occurs, the returned result will be false. Otherwise,
         *              the returned result will be true.
         *                   
         *****************************************************************************************/
        public bool UnlockByte(uint index)
        {
            bool result = false;

            try
            {
                /* The byte is already unlocked. */
                if (memoryContents[index].IsLocked() == false)
                    result = true;

                /* Unlock the byte. */
                else
                {
                    memoryContents[index].ToggleLock();
                    result = true;
                }
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
         * Name:        GetByteInt     
         * 
         * Author(s):   Michael Beaver
         *              
         * Input:       The index is an unsigned integer.   
         * Return:      The integer value of the byte is an integer.   
         * Description: This method will return the integer value of a byte in memory, whose 
         *              location is specified by "index." If there is an error, the returned
         *              result will be -1.
         *              
         *****************************************************************************************/
        public int GetByteInt(uint index)
        {
            int result;

            /* Get the integer value. */
            try
            {
                result = memoryContents[index].GetInt();
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
         * Description: This method will return the integer value of a series of bytes in memory. 
         *              This method will take a sequence of bytes (inclusive, [start, end]) and
         *              calculate the value of that sequence of bytes. If there is an error,
         *              the returned result will be -1.
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
         * Name:        GetMemorySize     
         * 
         * Author(s):   Michael Beaver
         *                      
         * Input:       N/A
         * Return:      The size of memory is an unsigned integer. 
         * Description: This method returns the size of memory.
         *              
         *****************************************************************************************/
        public uint GetMemorySize()
        {
            return memorySize;
        }

        /******************************************************************************************
         * 
         * Name:        GetByteHex     
         * 
         * Author(s):   Michael Beaver
         *                          
         * Input:       The index is an unsigned integer.    
         * Return:      The hexadecimal value is a string.  
         * Description: This method will return the hexadecimal representation as a string of a
         *              byte in memory, whose location is specified by "index." If there is an
         *              error, the returned result will be null.
         *                          
         *****************************************************************************************/
        public string GetByteHex(uint index)
        {
            string result;

            /* Get the hexadecimal representation. */
            try
            {
                result = memoryContents[index].GetHex();
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
         * Description: This method will return a string of bytes in hexadecimal representation. 
         *              The range is inclusive (so, [start, end]). If there is an error, the
         *              returned result will be null.
         *                     
         *****************************************************************************************/
        public string GetBytesString(uint start, uint end)
        { 
            string result = "";
            uint i = start;   
            
            /* Do not use a negative range. */
            if (end < start)
                result = null;

            else
            {
                /* Create the hexadecimal string. */
                try
                {
                    while (i <= end)
                    {
                        result += memoryContents[i].GetHex();

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

        /******************************************************************************************
         * 
         * Name:        GetEBCDIC      
         * 
         * Author(s):   Michael Beaver
         *                         
         * Input:       The start and end values are unsigned integers.   
         * Return:      The EBCDIC characters form a string.   
         * Description: This method will return the EBCDIC character representation of a sequence
         *              of bytes in memory. The range is inclusive (so, [start, end]). If an error
         *              occurs, the returned result will be null.
         *                     
         *****************************************************************************************/
        public string GetEBCDIC(uint start, uint end)
        {
            string result = "";
            
            /* Do not use a negative range. */
            if (end < start)
                result = null;

            else
            {
                /* Form the EBCDIC character string. */
                try
                {
                    for (uint i = start; i <= end; i++)
                        result += ToEBCDIC(memoryContents[i].GetHex());
                }

                /* An index was out of bounds. */
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
         * Name:        ToEBCDIC   
         * 
         * Author(s):   Michael Beaver
         *                  
         * Input:       The value is a string. 
         * Return:      The result is a string.
         * Description: This method will return the EBCDIC character representation for a given
         *              hexadecimal value ("value"). All non-printable, non-viewable characters
         *              are represented by periods (".") as is done in ASSIST/I. If an error
         *              occurs, the returned result will be a period. Otherwise, the returned
         *              result will be the converted character.
         *              
         *****************************************************************************************/
        private string ToEBCDIC(string value)
        {
            int index;
            string[] EBCDICValues = {".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".",
                ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".",
                ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".",
                ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", 
                ".", ".", ".", ".", " ", ".", ".", ".", ".", ".", ".", ".", ".", ".", "¢", ".",
                "<", "(", "+", "|", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "!", "$",
                "*", ")", ";", "¬", "-", "/", ".", ".", ".", ".", ".", ".", ".", ".", "¦", ",", 
                "%", "_", ">", "?", ".", ".", ".", ".", ".", ".", ".", ".", ".", "`", ":", "#",
                "@", "'", "=", "\"", ".", "a", "b", "c", "d", "e", "f", "g", "h", "i", ".", ".",
                ".", ".", ".", ".", ".", "j", "k", "l", "m", "n", "o", "p", "q", "r", ".", ".", 
                ".", ".", ".", ".", ".", "~", "s", "t", "u", "v", "w", "x", "y", "z", ".", ".",
                ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".",
                ".", ".", ".", ".", "{", "A", "B", "C", "D", "E", "F", "G", "H", "I", ".", ".", 
                ".", ".", ".", ".", "}", "J", "K", "L", "M", "N", "O", "P", "Q", "R", ".", ".",
                ".", ".", ".", ".", "\\", ".", "S", "T", "U", "V", "W", "X", "Y", "Z", ".", ".",
                ".", ".", ".", ".", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", ".", ".", 
                ".", ".", ".", "."};
            string result;

            index = Convert.ToInt32(value, 16);

            /* Convert to the EBCDIC character. */
            try
            {
                result = EBCDICValues[index];
            }

            /* The index was out of bounds. */
            catch (IndexOutOfRangeException)
            {
                result = ".";
            }

            return result;
        }

        /******************************************************************************************
         * 
         * Name:        InitializeMemoryContents    
         * 
         * Author(s):   Michael Beaver 
         *                       
         * Input:       N/A
         * Return:      N/A
         * Description: This method is used by the class constructor(s) to initialize the memory
         *              contents to the DEFAULT_BYTE_VALUE. The DEFAULT_BYTE_VALUE should be
         *              in hexadecimal.              
         *              
         *****************************************************************************************/
        private void InitializeMemoryContents()
        {
            string tempVal;

            /* Set all bytes in memory to the default byte value. */
            for (uint i = 0; i < memorySize; i++)
            {
                tempVal = Convert.ToString(Convert.ToSByte(DEFAULT_BYTE_VALUE, 16));
                memoryContents[i].SetValue(tempVal);
            }
        }
    }
}